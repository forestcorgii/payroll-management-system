Class Payroll
    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        btnPayregUpload.IsChecked = True
    End Sub


    Private Sub btnPayregUpload_Click(sender As Object, e As RoutedEventArgs) Handles btnPayregUpload.Checked ', btnPayregUpload.Unchecked
        frmPayroll.Navigate(New PayRegisterSummary)
    End Sub

    Private Sub btnAdjustmentRecord_Click(sender As Object, e As RoutedEventArgs) Handles btnAdjustmentRecord.Checked ', btnAdjustment.Unchecked
        frmPayroll.Navigate(New AdjustmentRecord)
    End Sub
    Private Sub btnAdjustmentBilling_Click(sender As Object, e As RoutedEventArgs) Handles btnAdjustmentBilling.Checked ', btnAdjustment.Unchecked
        frmPayroll.Navigate(New AdjustmentBilling)
    End Sub

End Class
