Imports System.Collections.ObjectModel
Imports payroll_module.Payroll

Class Adjustment
    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private SelectedRecord As AdjustmentRecordModel
    Private Sub Adjustment_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        DatabaseManager.Connection.Open()
        Dim adjustments As New ObservableCollection(Of AdjustmentRecordModel)(AdjustmentRecordGateway.Collect(DatabaseManager))
        DatabaseManager.Connection.Close()

        lstAdjustments.ItemsSource = adjustments


        SelectedRecord = New AdjustmentRecordModel() With {.Date_Effective = Now, .Date_Expires = Now}
        grpAdjustmentDetail.DataContext = SelectedRecord
    End Sub

    Private Sub lstAdjustments_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles lstAdjustments.SelectionChanged
        SelectedRecord = lstAdjustments.SelectedItem
        grpAdjustmentDetail.DataContext = SelectedRecord
    End Sub

    Private Sub btnSave_Click(sender As Object, e As RoutedEventArgs)
        DatabaseManager.Connection.Open()
        AdjustmentRecordGateway.Save(DatabaseManager, SelectedRecord)
        DatabaseManager.Connection.Close()
    End Sub

    Private Sub btnImport_Click(sender As Object, e As RoutedEventArgs)

    End Sub
End Class
