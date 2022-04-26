Imports payroll_module.Payroll
Class BankReport
    Private PayregisterDetail As PayregisterBinding
    Private Sub Report_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        PayregisterDetail = New PayregisterBinding
        grdPayregisterDetail.DataContext = PayregisterDetail

        dtPayrollDate.SelectedDate = DefaultPayrollDate
    End Sub

    Private Sub lstPayrollCodes_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles lstPayrollCodes.SelectionChanged
        DatabaseManager.Connection.Open()
        Dim payrolls As New List(Of PayrollModel)
        Dim payrollCodes As New List(Of String)

        For Each payrollCode In lstPayrollCodes.SelectedItems
            payrolls.AddRange(PayrollGateway.CollectPayrolls(DatabaseManager, dtPayrollDate.SelectedDate, payrollCode, True))
            payrollCodes.Add(payrollCode)
        Next
        DatabaseManager.Connection.Close()

        PayregisterDetail.Payrolls = payrolls
        PayregisterDetail.PayrollCodes = payrollCodes

        lstPayrolls.ItemsSource = PayregisterDetail.Payrolls
    End Sub
    'Private Sub btnRun_Click(sender As Object, e As RoutedEventArgs) Handles btnRun.Click
    '    Console.WriteLine(lstPayrollCodes.SelectedItems)
    'End Sub

    Private Sub dtPayrollDate_SelectedDateChanged(sender As Object, e As SelectionChangedEventArgs) Handles dtPayrollDate.SelectedDateChanged
        If dtPayrollDate.SelectedDate IsNot Nothing Then
            DatabaseManager.Connection.Open()
            'GET A COLLECTION OF AVAILABLE PAYROLL CODE BASED ON THE PAYROLL DATE.
            Dim payroll_codes As List(Of String) = PayrollGateway.CollectPayrollCodes(DatabaseManager, dtPayrollDate.SelectedDate)
            DatabaseManager.Connection.Close()

            lstPayrollCodes.ItemsSource = payroll_codes
        End If
    End Sub

    Private Sub btnGenerateReport_Click(sender As Object, e As RoutedEventArgs)
        Dim UCPBs As List(Of PayrollModel) = PayRegisterController.GetBankEECount(PayregisterDetail.Payrolls, "UCPB", "CCARD")
        Dim CHINABANKs As List(Of PayrollModel) = PayRegisterController.GetBankEECount(PayregisterDetail.Payrolls, "CHINABANK", "CCARD")
        Dim CHKs As List(Of PayrollModel) = PayRegisterController.GetBankEECount(PayregisterDetail.Payrolls, "", "CHK")
        CHKs.AddRange(PayRegisterController.GetBankEECount(PayregisterDetail.Payrolls, "UCPB", "CHK"))
        CHKs.AddRange(PayRegisterController.GetBankEECount(PayregisterDetail.Payrolls, "CHINABANK", "CHK"))

        Dim ZEROs As List(Of PayrollModel) = PayRegisterController.GetBankEECount_Negative(PayregisterDetail.Payrolls)

        Dim filePath As String = AppDomain.CurrentDomain.BaseDirectory & "Report\"
        IO.Directory.CreateDirectory(filePath)

        PayrollController.ExportBankReport(UCPBs, filePath, dtPayrollDate.SelectedDate, PayregisterDetail.PayrollCodesAll, "UCPB", "CCARD", "IDCSI")
        PayrollController.ExportBankReport(CHINABANKs, filePath, dtPayrollDate.SelectedDate, PayregisterDetail.PayrollCodesAll, "CHINABANK", "CCARD", "IDCSI")
        PayrollController.ExportBankReport(CHKs, filePath, dtPayrollDate.SelectedDate, PayregisterDetail.PayrollCodesAll, "CHK", "", "IDCSI")
        PayrollController.ExportBankReport(ZEROs, filePath, dtPayrollDate.SelectedDate, PayregisterDetail.PayrollCodesAll, "ZEROS", "", "IDCSI")

    End Sub
End Class
