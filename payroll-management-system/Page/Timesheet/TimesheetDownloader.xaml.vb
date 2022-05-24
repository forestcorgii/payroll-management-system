Imports System.ComponentModel
Imports payroll_module
Imports employee_module
Imports payroll_module.Payroll
Imports time_module.Model
Imports utility_service
Imports System.Threading.Tasks

Class TimeDownloaderPage
    Private DownloadLog As DownloadLogModel
    Private TimeResponse As TimeResponseData

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub TimeDownloader_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        DatabaseManager.Connection.Open()
        cbPayrollCode.ItemsSource = EmployeeGateway.CollectPayrollCodes(DatabaseManager)
        DatabaseManager.Connection.Close()

        dtPayrollDate.SelectedDate = DefaultPayrollDate
        cbPayrollCode.SelectedItem = DefaultPayrollCode
    End Sub

    Private Async Sub dtPayrollDate_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If dtPayrollDate.SelectedDate Is Nothing Then Exit Sub

        Dim selectedDate As Date = dtPayrollDate.SelectedDate
        Dim selectedPayrollCode As String = cbPayrollCode.Text
        If sender IsNot Nothing Then
            If sender.name = "dtPayrollDate" Then
                selectedDate = e.AddedItems.Item(0)
            ElseIf sender.name = "cbPayrollCode" Then
                selectedPayrollCode = e.AddedItems.Item(0)
            End If
        End If
        If selectedPayrollCode = "" Or selectedDate = Nothing Then Exit Sub

        If {15, 30}.Contains(selectedDate.Day) Or ({2}.Contains(selectedDate.Month) And {29, 28}.Contains(selectedDate.Day)) Then
            'GET CUT OFF RANGE
            Dim cutoffRange As Date() = PayrollController.GetCutoffRange(selectedDate)
            'GET A SUMMARY FROM SERVER
            ctrlLoader.Visibility = Visibility.Visible
            TimeResponse = Await TimeDownloaderAPIManager.GetSummary(cutoffRange(0), cutoffRange(1), selectedPayrollCode)
            ctrlLoader.Visibility = Visibility.Collapsed
            If TimeResponse IsNot Nothing Then
                lbEmployeeCount.Text = String.Format("Total Employee Count: {0}", TimeResponse.totalCount)
                lbPageCount.Text = String.Format("Total Page Count: {0}", TimeResponse.totalPage)
                lbUnconfirmedEmployeeCount.Text = String.Format("Total Unconfirmed: {0}", TimeResponse.unconfirmedTimesheet.Length)
                lbConfirmedEmployeeCount.Text = String.Format("Total Confirmed: {0}", TimeResponse.totalConfirmed)

                DatabaseManager.Connection.Open()
                Dim _employees As New List(Of EmployeeModel)
                For i As Integer = 0 To TimeResponse.unconfirmedTimesheet.Length - 1
                    Dim _employee As EmployeeModel = Await EmployeeGateway.FindAsync(DatabaseManager, HRMSAPIManager, TimeResponse.unconfirmedTimesheet(i).EE_Id, LoggingService)
                    If _employee IsNot Nothing Then
                        _employees.Add(_employee)
                    Else
                        _employees.Add(New EmployeeModel With {.EE_Id = TimeResponse.unconfirmedTimesheet(i).EE_Id})
                    End If
                Next
                lstUnconfirmedEmployees.ItemsSource = _employees
                DatabaseManager.Connection.Close()
            End If

            'FIND OR CREATE DOWNLOAD LOG
            DatabaseManager.Connection.Open()
            DownloadLog = DownloadLogGateway.Find(DatabaseManager, selectedDate, cbPayrollCode.Text)
            DatabaseManager.Connection.Close()
            If DownloadLog IsNot Nothing Then
                lbStatus.Text = String.Format("STATUS: {0} Date Created: {1}", DownloadLog.Status.ToString, DownloadLog.DateTimeCreated)
                lbPage.Text = String.Format("{0}/{1}", DownloadLog.Last_Page_Downloaded, DownloadLog.TotalPage)
                If DownloadLog.Status = DownloadLogModel.DownloadStatusChoices.DONE Then
                    btnDownload.Content = "Re-Download"
                Else
                    btnDownload.Content = "Resume Download"
                End If
            Else
                DownloadLog = New DownloadLogModel With {.Payroll_Date = selectedDate, .Payroll_Code = cbPayrollCode.Text}
                lbStatus.Text = "STATUS: NOT DOWNLOADED YET."
                btnDownload.Content = "Download"
                lbPage.Text = ""
            End If
        End If
    End Sub

    Private Sub btnDownload_Click(sender As Object, e As RoutedEventArgs)
        Select Case btnDownload.Content
            Case "Cancel"
                btnDownload.Content = "Cancelling.."
            Case "Re-Download", "Download"
                DownloadLog.TotalPage = TimeResponse.totalPage
                DownloadLog.Last_Page_Downloaded = 0
                DownloadLog.Status = DownloadLogModel.DownloadStatusChoices.PENDING
                lbPage.Text = "0/0"
                btnDownload.Content = "Cancel"
            Case "Resume Download"
                btnDownload.Content = "Cancel"
        End Select

        If Not bgProcessor.IsBusy Then
            bgProcessor.RunWorkerAsync()
        End If
    End Sub


    Private WithEvents bgProcessor As New BackgroundWorker
    Private Async Sub bgProcessor_DoWork(sender As Object, e As DoWorkEventArgs) Handles bgProcessor.DoWork
        Dim _databaseManager As New utility_service.Manager.Mysql(DatabaseConfiguration)
        _databaseManager.Connection.Open()
        Try
            'GET CUT OFF RANGE
            Dim cutoffRange As Date() = PayrollController.GetCutoffRange(DownloadLog.Payroll_Date)
            Dim pages As List(Of Integer) = TimesheetGateway.CollectUnconfirmedTSByPage(_databaseManager, DownloadLog.Payroll_Code, DownloadLog.Payroll_Date.ToString("yyyy-MM-dd"))
            Dispatcher.Invoke(Sub()
                                  pb.Maximum = DownloadLog.TotalPage
                                  pb.Value = DownloadLog.Last_Page_Downloaded
                              End Sub)
            'RUN THROUGH PAGES
            If pages.Count = 0 OrElse (pages.Count = 1 And pages(0) = 0) Then
                If MessageBox.Show("Re-Download?", "Re-Download", MessageBoxButton.YesNo, MessageBoxImage.Question) = MessageBoxResult.Yes Then
                    Dispatcher.Invoke(Sub()
                                          pb.Maximum = DownloadLog.TotalPage
                                          pb.Value = DownloadLog.Last_Page_Downloaded
                                      End Sub)
                    For DownloadLog.Last_Page_Downloaded = DownloadLog.Last_Page_Downloaded To DownloadLog.TotalPage
                        Await GetPageContent(_databaseManager, cutoffRange, DownloadLog.Payroll_Code, DownloadLog.Last_Page_Downloaded)
                    Next
                End If
            Else
                Dispatcher.Invoke(Sub()
                                      pb.Maximum = DownloadLog.TotalPage
                                      pb.Value = 0
                                  End Sub)
                For Each _page As Integer In pages
                    Await GetPageContent(_databaseManager, cutoffRange, DownloadLog.Payroll_Code, _page)
                Next
            End If

            If DownloadLog.Last_Page_Downloaded >= DownloadLog.TotalPage Then
                DownloadLog.Last_Page_Downloaded = DownloadLog.TotalPage
                DownloadLog.Status = DownloadLogModel.DownloadStatusChoices.DONE
                DownloadLogGateway.Update(_databaseManager, DownloadLog)
            End If
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            MessageBox.Show(ex.Message, "bgProcessor", MessageBoxButton.OK, MessageBoxImage.Error)
        End Try
        Dispatcher.Invoke(Sub()
                              lbPage.Text = String.Format("{0}/{1}", DownloadLog.Last_Page_Downloaded, DownloadLog.TotalPage)
                              btnDownload.Content = "Cancelled"
                              'dtPayrollDate_SelectionChanged(Nothing, Nothing)
                          End Sub)
        _databaseManager.Connection.Close()
        MessageBox.Show("Download Finished.", "Done", MessageBoxButton.OK, MessageBoxImage.Information)
    End Sub

    Private Async Function GetPageContent(_databaseManager As Manager.Mysql, cutoffRange As Date(), payrollCode As String, page As Integer) As Task(Of Boolean)
        Try
            Dim errCounter As Integer = 0
            While True 'errCounter < 10
                Dim payrollTimes As time_module.Model.PayrollTime() = Await TimeDownloaderAPIManager.GetPageContent(cutoffRange(0), cutoffRange(1), page, payrollCode)
                If payrollTimes IsNot Nothing Then
                    For i As Integer = 0 To payrollTimes.Count - 1
                        TimesheetController.ProcessPayrollTime(_databaseManager, DownloadLog.Payroll_Date, payrollTimes(i), DownloadLog.Last_Page_Downloaded, LoggingService)
                    Next

                    DownloadLogGateway.Update(_databaseManager, DownloadLog)

                    Dispatcher.Invoke(Sub()
                                          pb.Value = page
                                          lbPage.Text = String.Format("{0}/{1}", page, DownloadLog.TotalPage)
                                      End Sub)
                    errCounter = 0
                    Exit While
                Else
                    errCounter += 1
                End If
            End While

            Return True
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Return False
        End Try
    End Function


    Private Sub btnReset_Click(sender As Object, e As RoutedEventArgs)
        If DownloadLog.Status = DownloadLogModel.DownloadStatusChoices.PENDING Then 'ONLY IF PENDING
            'GET CUTOFF RANGE
            Dim cutoffRange As Date() = PayrollController.GetCutoffRange(DownloadLog.Payroll_Date)
            'SET LAST PAGE DOWNLOADED TO ZERO.
            DownloadLog.Last_Page_Downloaded = 0
            'GET TOTAL PAGE AGAIN
            'DownloadLog.TotalPage = Await TimeDownloaderAPIManager.GetSummary(cutoffRange(0), cutoffRange(1), DownloadLog.Payroll_Code)
            'SAVE
            DatabaseManager.Connection.Open()
            DownloadLogGateway.Save(DatabaseManager, DownloadLog)
            DatabaseManager.Connection.Close()

            lbPage.Text = "0/0"

            lbStatus.Text = "STATUS: RESET."
            btnDownload.Content = "Download"
        End If
    End Sub

End Class
