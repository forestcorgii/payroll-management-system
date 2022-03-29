Imports System.ComponentModel
Imports payroll_time_service

Public Class TimeDownloader

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        DatabaseManager.Connection.Open()
        cbPayrollCode.ItemsSource = payroll_service.Controller.PayrollCode.GetAllPayrollCodes(DatabaseManager)
        DatabaseManager.Connection.Close()
    End Sub

    Private DownloadLog As Model.DownloadLog

    Private Sub dtPayrollDate_SelectedDateChanged(sender As Object, e As SelectionChangedEventArgs)
        Dim selectedDate As Date = dtPayrollDate.SelectedDate
        If {15, 30}.Contains(selectedDate.Day) Or ({2}.Contains(selectedDate.Month) And {29, 28}.Contains(selectedDate.Day)) Then
            'FIND OR CREATE DOWNLOAD LOG
            DatabaseManager.Connection.Open()
            DownloadLog = Gateway.DownloadLog.Find(DatabaseManager, selectedDate, cbPayrollCode.Text)
            DatabaseManager.Connection.Close()
            If downloadLog IsNot Nothing Then
                lbStatus.Text = String.Format("STATUS: {0} Date Created: {1}", DownloadLog.Status.ToString, DownloadLog.DateTimeCreated)
                lbPage.Text = String.Format("{0}/{1}", DownloadLog.Last_Page_Downloaded, DownloadLog.TotalPage)

                If DownloadLog.Status = Model.DownloadLog.DownloadStatusChoices.DONE Then
                    btnDownload.Content = "Re-Download"
                Else
                    btnDownload.Content = "Resume Download"
                End If
            Else
                DownloadLog = New Model.DownloadLog With {.Payroll_Date = selectedDate, .Payroll_Code = cbPayrollCode.Text}
                lbStatus.Text = "STATUS: NOT DOWNLOADED YET."
                btnDownload.Content = "Download"
            End If
        End If
    End Sub

    Private Sub btnDownload_Click(sender As Object, e As RoutedEventArgs)
        Select Case btnDownload.Content
            Case "Cancel"
                'START CANCELLING
                btnDownload.Content = "Cancelling.."
            Case "Re-Download", "Download"
                DownloadLog.Last_Page_Downloaded = 0
                DownloadLog.Status = Model.DownloadLog.DownloadStatusChoices.PENDING
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
            Dim cutoffRange As Date() = Controller.PayrollTime.GetCutoffRange(DownloadLog.Payroll_Date)
            'GET TOTAL PAGE
            DownloadLog.TotalPage = Await TimeDownloaderAPIManager.GetTotalPage(cutoffRange(0), cutoffRange(1), DownloadLog.Payroll_Code)
            Gateway.DownloadLog.Save(_databaseManager, DownloadLog)
            'RUN THROUGH PAGES
            While DownloadLog.Last_Page_Downloaded < DownloadLog.TotalPage
                Dim payrollTimes As Model.PayrollTime() = Await TimeDownloaderAPIManager.GetPageContent(cutoffRange(0), cutoffRange(1), DownloadLog.Last_Page_Downloaded, DownloadLog.Payroll_Code)
                For i As Integer = 0 To payrollTimes.Count - 1
                    Controller.PayrollTime.ProcessPayrollTime(_databaseManager, DownloadLog.Payroll_Date, payrollTimes(i))
                Next

                Gateway.DownloadLog.Update(_databaseManager, DownloadLog)
                DownloadLog.Last_Page_Downloaded += 1

                Dim toCancel As Boolean = False
                Dispatcher.Invoke(Sub()
                                      lbPage.Text = String.Format("{0}/{1}", DownloadLog.Last_Page_Downloaded, DownloadLog.TotalPage)
                                      toCancel = btnDownload.Content <> "Cancel"
                                  End Sub)
                If toCancel Then
                    e.Cancel = True
                    Exit Try
                End If
            End While
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            MessageBox.Show(ex.Message, "bgProcessor", MessageBoxButton.OK, MessageBoxImage.Error)
        End Try
        Dispatcher.Invoke(Sub()
                              lbPage.Text = String.Format("{0}/{1}", DownloadLog.Last_Page_Downloaded, DownloadLog.TotalPage)
                              btnDownload.Content = "Cancelled"
                              dtPayrollDate_SelectedDateChanged(Nothing, Nothing)
                          End Sub)
        _databaseManager.Connection.Close()
    End Sub

    Private Sub TimeDownloader_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded

    End Sub

End Class
