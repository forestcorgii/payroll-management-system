Class Timesheet
    Dim TimeDownloaderPage As TimeDownloaderPage
    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        TimeDownloaderPage = New TimeDownloaderPage
        btnDownload.IsChecked = True
    End Sub

    Private Sub btnCompare_Checked(sender As Object, e As RoutedEventArgs) Handles btnCompare.Checked
        frmTimesheet.Navigate(New PayrollTimeComparer)
    End Sub

    Private Sub btnDownload_Checked(sender As Object, e As RoutedEventArgs) Handles btnDownload.Checked
        frmTimesheet.Navigate(TimeDownloaderPage)
    End Sub

    Private Sub btnGenerateDBF_Checked(sender As Object, e As RoutedEventArgs) Handles btnGenerateDBF.Checked
        frmTimesheet.Navigate(New GenerateDBFPage)
    End Sub

End Class
