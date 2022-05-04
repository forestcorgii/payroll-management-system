Imports System.Collections.ObjectModel
Imports employee_module
Imports payroll_module.Payroll

Class AdjustmentBilling
    Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        DatabaseManager.Connection.Open()
        'Dim adjustments As New ObservableCollection(Of AdjustmentRecordModel)(AdjustmentRecordGateway.Collect(DatabaseManager))
        Dim payrollCodes As List(Of String) = EmployeeGateway.CollectPayrollCodes(DatabaseManager)
        'Dim billings As List(Of AdjustmentBillingModel) = AdjustmentBillingGateway.Collect(DatabaseManager)
        DatabaseManager.Connection.Close()

        Bindings = New AdjustmentBillingBinding
        Bindings.PayrollCodes = payrollCodes
        Bindings.PayrollCode = DefaultPayrollCode
        Bindings.PayrollDate = DefaultPayrollDate

    End Sub
    Private Bindings As AdjustmentBillingBinding

    Private Sub Adjustment_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded

        grdAdjustmentBillingDetail.DataContext = Bindings
    End Sub

    Private Sub btnImport_Click(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub btnExport_Click(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub btnSave_Click(sender As Object, e As RoutedEventArgs)

    End Sub

End Class
