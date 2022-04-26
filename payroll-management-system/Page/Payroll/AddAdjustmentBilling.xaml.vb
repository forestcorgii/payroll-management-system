Imports System.Collections.ObjectModel
Imports payroll_module.Payroll

Public Class AddAdjustmentBilling
    Private SelectedRecord As AdjustmentBillingModel
    Private Sub Adjustment_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        'DatabaseManager.Connection.Open()
        'Dim adjustments As New ObservableCollection(Of AdjustmentBillingModel)(AdjustmentBillingGateway.Collect(DatabaseManager))
        'DatabaseManager.Connection.Close()

        'SelectedRecord = New AdjustmentBillingModel() With {.Date_Effective = Now, .Date_Expires = Now}
        grpAdjustmentDetail.DataContext = SelectedRecord
    End Sub

    Private Sub btnSave_Click(sender As Object, e As RoutedEventArgs)
        DatabaseManager.Connection.Open()
        AdjustmentBillingGateway.Save(DatabaseManager, SelectedRecord)
        DatabaseManager.Connection.Close()
    End Sub

End Class
