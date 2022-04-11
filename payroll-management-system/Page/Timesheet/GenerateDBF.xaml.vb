Imports employee_module
Imports payroll_module.Payroll
Imports time_module.Model

Class GenerateDBFPage
    Private TimeResponse As TimeResponseData

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        DatabaseManager.Connection.Open()
        cbPayrollCode.ItemsSource = EmployeeGateway.CollectPayrollCodes(DatabaseManager)
        DatabaseManager.Connection.Close()
    End Sub


    Private Sub btnGenerateDBF_Click(sender As Object, e As RoutedEventArgs)
        Dim payDay As Integer = Date.Parse(dtPayrollDate.SelectedDate).Day
        Dim payMonth As Integer = Date.Parse(dtPayrollDate.SelectedDate).Month
        If {15, 30}.Contains(payDay) Or
            ({28, 29}.Contains(payDay) And payMonth = 2) Or
            (payDay = 13 And payMonth = 12) Then

            Dim startupPath As String = String.Format("{0}/DBF/{1:yyyyMMdd}", AppDomain.CurrentDomain.BaseDirectory, dtPayrollDate.SelectedDate)
            IO.Directory.CreateDirectory(startupPath)

            DatabaseManager.Connection.Open()
            Dim bankCategories As List(Of String) = EmployeeGateway.CollectBankCategories(DatabaseManager)
            For Each bankCategory As String In bankCategories
                TimesheetController.SavePayrollTimeToDBF(DatabaseManager, dtPayrollDate.SelectedDate, cbPayrollCode.Text, bankCategory, startupPath)
            Next

            'Dim payrollCodes As List(Of String) = EmployeeGateway.CollectPayrollCodes(DatabaseManager)
            'Dim bankCategories As List(Of String) = EmployeeGateway.CollectBankCategories(DatabaseManager)
            'For Each payrollCode As String In payrollCodes
            '    For Each bankCategory As String In bankCategories
            '        TimesheetController.SavePayrollTimeToDBF(DatabaseManager, dtPayrollDate.SelectedDate, payrollCode, bankCategory, startupPath)
            '    Next
            '    TimesheetController.ExportTransferLog(DatabaseManager, startupPath, dtPayrollDate.SelectedDate, payrollCode)
            'Next
            DatabaseManager.Connection.Close()
        End If
    End Sub
End Class
