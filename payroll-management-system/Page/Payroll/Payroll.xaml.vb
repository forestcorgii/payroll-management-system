Class Payroll
    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        btnPayregUpload.IsChecked = True
    End Sub


    Private Sub btnPayregUpload_Click(sender As Object, e As RoutedEventArgs) Handles btnPayregUpload.Checked ', btnPayregUpload.Unchecked
        frmPayroll.Navigate(New ProcessPayreg)
    End Sub

    Private Sub btnReport_Click(sender As Object, e As RoutedEventArgs) Handles btnReport.Checked ', btnAdjustment.Unchecked
        frmPayroll.Navigate(New Report)
    End Sub

    Private Sub btnAdjustment_Click(sender As Object, e As RoutedEventArgs) Handles btnAdjustment.Checked ', btnAdjustment.Unchecked
        frmPayroll.Navigate(New Adjustment)
    End Sub

End Class
