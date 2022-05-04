Imports System.Collections.ObjectModel
Imports System.Windows.Forms
Imports employee_module
Imports payroll_module.Payroll

Class AdjustmentRecord
    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        DatabaseManager.Connection.Open()
        'Dim adjustments As New ObservableCollection(Of AdjustmentRecordModel)(AdjustmentRecordGateway.Collect(DatabaseManager))
        Dim payrollCodes As List(Of String) = EmployeeGateway.CollectPayrollCodes(DatabaseManager)
        'Dim records As List(Of AdjustmentRecordModel) = AdjustmentRecordGateway.Collect(DatabaseManager)
        DatabaseManager.Connection.Close()

        Bindings = New AdjustmentRecordBinding
        'Bindings.AdjustmentRecords = records
        Bindings.PayrollCodes = payrollCodes
        Bindings.PayrollCode = DefaultPayrollCode
    End Sub

    Private Bindings As AdjustmentRecordBinding

    Private Sub Adjustment_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        grdAdjustmentRecordDetail.DataContext = Bindings
    End Sub


    Private Sub btnImport_Click(sender As Object, e As RoutedEventArgs)
        Using openFile As New OpenFileDialog
            openFile.Filter = "Adjustment Import Files(*.xls)|*.xls"
            openFile.Multiselect = False
            If openFile.ShowDialog = DialogResult.OK Then

                DatabaseManager.Connection.Open()
                'Dim adjustments As New ObservableCollection(Of AdjustmentRecordModel)(AdjustmentRecordController.ImportRecords(DatabaseManager, openFile.FileName))
                Dim adjustments As List(Of AdjustmentRecordModel) = AdjustmentRecordController.ImportRecords(DatabaseManager, openFile.FileName)
                DatabaseManager.Connection.Close()
                'Bindings.AdjustmentRecords = adjustments
            End If
        End Using
    End Sub

    Private Sub btnExport_Click(sender As Object, e As RoutedEventArgs)

    End Sub


    Private Sub dtPayrollDate_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        Try
            Dim selectedPayrollCode As String = cbPayrollCode.Text
            If sender IsNot Nothing Then
                If sender.name = "dtPayrollDate" Then
                    DefaultPayrollDate = e.AddedItems.Item(0)
                ElseIf sender.name = "cbPayrollCode" Then
                    DefaultPayrollCode = e.AddedItems.Item(0)
                End If
            End If
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
    End Sub

    Private Sub btnSave_Click(sender As Object, e As RoutedEventArgs)

    End Sub
End Class
