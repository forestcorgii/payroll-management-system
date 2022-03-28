Imports payroll_service
Imports utility_service

Class GenerateDBFPage
    Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub btnSettings_Click(sender As Object, e As RoutedEventArgs) Handles btnSettings.Click

    End Sub

    Private Sub btnGenerateDBF_Click(sender As Object, e As RoutedEventArgs)
        Dim payDay As Integer = Date.Parse(dtPayrollDate.SelectedDate).Day
        Dim payMonth As Integer = Date.Parse(dtPayrollDate.SelectedDate).Month
        If {15, 30}.Contains(payDay) Or
            ({28, 29}.Contains(payDay) And payMonth = 2) Or
            (payDay = 13 And payMonth = 12) Then

            Dim startupPath As String = String.Format("{0}/DBF", AppDomain.CurrentDomain.BaseDirectory)
            IO.Directory.CreateDirectory(startupPath)

            DatabaseManager.Connection.Open()
            Dim payrollCodes As List(Of String) = Controller.PayrollCode.GetAllPayrollCodes(DatabaseManager)
            Dim bankCategories As List(Of String) = Controller.BankCategory.GetAllBankCategory(DatabaseManager)

            For Each payrollCode As String In payrollCodes
                For Each bankCategory As String In bankCategories
                    payroll_time_service.Controller.PayrollTime.SavePayrollTimeToDBF(DatabaseManager, dtPayrollDate.SelectedDate, payrollCode, bankCategory, startupPath)
                Next
                payroll_time_service.Controller.PayrollTime.ExportTransferLog(DatabaseManager, startupPath, dtPayrollDate.SelectedDate, payrollCode)
            Next
            DatabaseManager.Connection.Close()
        End If
    End Sub
End Class
