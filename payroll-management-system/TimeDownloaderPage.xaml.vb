Imports System.ComponentModel
Imports payroll_module
Imports employee_module
Imports payroll_module.Payroll
Class TimeDownloaderPage
    Private DownloadLog As Time.DownloadLog.Model

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        DatabaseManager.Connection.Open()
        cbPayrollCode.ItemsSource = EmployeeGateway.CollectPayrollCodes(DatabaseManager)
        DatabaseManager.Connection.Close()
    End Sub

    Private Sub TimeDownloader_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded

    End Sub

    Private Sub dtPayrollDate_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If dtPayrollDate.SelectedDate Is Nothing Then Exit Sub

        Dim selectedDate As Date = dtPayrollDate.SelectedDate
        Dim selectedPayrollCode As String = cbPayrollCode.Text
        If sender IsNot Nothing Then
            If sender.name = "dtPayrollDate" Then
                selectedDate = e.AddedItems.Item(0)
            ElseIf sender.name = "cbPayrollCode" Then
                cbPayrollCode.Text = e.AddedItems.Item(0)
            End If
        End If
        If selectedPayrollCode = "" Or selectedDate = Nothing Then Exit Sub

        If {15, 30}.Contains(selectedDate.Day) Or ({2}.Contains(selectedDate.Month) And {29, 28}.Contains(selectedDate.Day)) Then
            'FIND OR CREATE DOWNLOAD LOG
            DatabaseManager.Connection.Open()
            DownloadLog = Time.DownloadLog.Gateway.Find(DatabaseManager, selectedDate, cbPayrollCode.Text)
            DatabaseManager.Connection.Close()
            If DownloadLog IsNot Nothing Then
                lbStatus.Text = String.Format("STATUS: {0} Date Created: {1}", DownloadLog.Status.ToString, DownloadLog.DateTimeCreated)
                lbPage.Text = String.Format("{0}/{1}", DownloadLog.Last_Page_Downloaded, DownloadLog.TotalPage)

                If DownloadLog.Status = Time.DownloadLog.Model.DownloadStatusChoices.DONE Then
                    btnDownload.Content = "Re-Download"
                Else
                    btnDownload.Content = "Resume Download"
                End If
            Else
                DownloadLog = New Time.DownloadLog.Model With {.Payroll_Date = selectedDate, .Payroll_Code = cbPayrollCode.Text}
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
                DownloadLog.TotalPage = 0
                DownloadLog.Last_Page_Downloaded = 0
                DownloadLog.Status = Time.DownloadLog.Model.DownloadStatusChoices.PENDING
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
            Dim cutoffRange As Date() = Controller.GetCutoffRange(DownloadLog.Payroll_Date)
            'GET TOTAL PAGE IF IT IS ZERO
            If DownloadLog.TotalPage = 0 Then
                DownloadLog.TotalPage = Await TimeDownloaderAPIManager.GetTotalPage(cutoffRange(0), cutoffRange(1), DownloadLog.Payroll_Code)
                DownloadLog = Time.DownloadLog.Gateway.Save(_databaseManager, DownloadLog)
            End If

            Dispatcher.Invoke(Sub()
                                  pb.Maximum = DownloadLog.TotalPage
                                  pb.Value = DownloadLog.Last_Page_Downloaded
                              End Sub)
            'RUN THROUGH PAGES
            For DownloadLog.Last_Page_Downloaded = DownloadLog.Last_Page_Downloaded To DownloadLog.TotalPage
                Dim payrollTimes As time_module.Model.PayrollTime() = Await TimeDownloaderAPIManager.GetPageContent(cutoffRange(0), cutoffRange(1), DownloadLog.Last_Page_Downloaded, DownloadLog.Payroll_Code)
                For i As Integer = 0 To payrollTimes.Count - 1
                    Time.Controller.ProcessPayrollTime(_databaseManager, DownloadLog.Payroll_Date, payrollTimes(i), LoggingService)
                Next

                Time.DownloadLog.Gateway.Update(_databaseManager, DownloadLog)

                Dim toCancel As Boolean = False
                Dispatcher.Invoke(Sub()
                                      pb.Value = DownloadLog.Last_Page_Downloaded
                                      lbPage.Text = String.Format("{0}/{1}", DownloadLog.Last_Page_Downloaded, DownloadLog.TotalPage)
                                      toCancel = btnDownload.Content <> "Cancel"
                                  End Sub)
                If toCancel Then
                    e.Cancel = True
                    Exit Try
                End If
            Next
            If DownloadLog.Last_Page_Downloaded >= DownloadLog.TotalPage Then
                DownloadLog.Last_Page_Downloaded = DownloadLog.TotalPage
                DownloadLog.Status = Time.DownloadLog.Model.DownloadStatusChoices.DONE
                Time.DownloadLog.Gateway.Update(_databaseManager, DownloadLog)
            End If
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            MessageBox.Show(ex.Message, "bgProcessor", MessageBoxButton.OK, MessageBoxImage.Error)
        End Try
        Dispatcher.Invoke(Sub()
                              lbPage.Text = String.Format("{0}/{1}", DownloadLog.Last_Page_Downloaded, DownloadLog.TotalPage)
                              btnDownload.Content = "Cancelled"
                              dtPayrollDate_SelectionChanged(Nothing, Nothing)
                          End Sub)
        _databaseManager.Connection.Close()
        MessageBox.Show("Download Finished.", "Done", MessageBoxButton.OK, MessageBoxImage.Information)
    End Sub

    Private Async Sub btnReset_Click(sender As Object, e As RoutedEventArgs)
        If DownloadLog.Status = Time.DownloadLog.Model.DownloadStatusChoices.PENDING Then 'ONLY IF PENDING
            'GET CUTOFF RANGE
            Dim cutoffRange As Date() = Controller.GetCutoffRange(DownloadLog.Payroll_Date)
            'SET LAST PAGE DOWNLOADED TO ZERO.
            DownloadLog.Last_Page_Downloaded = 0
            'GET TOTAL PAGE AGAIN
            DownloadLog.TotalPage = Await TimeDownloaderAPIManager.GetTotalPage(cutoffRange(0), cutoffRange(1), DownloadLog.Payroll_Code)
            'SAVE
            DatabaseManager.Connection.Open()
            Time.DownloadLog.Gateway.Save(DatabaseManager, DownloadLog)
            DatabaseManager.Connection.Close()

            lbPage.Text = "0/0"

            lbStatus.Text = "STATUS: RESET."
            btnDownload.Content = "Download"
        End If
    End Sub

End Class
