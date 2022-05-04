

Imports System.Globalization
Imports System.IO
Imports employee_module
Imports utility_service
Imports NPOI.HSSF.UserModel
Imports NPOI.SS.UserModel

Namespace Payroll

    Public Class PayRegisterController
        Public Shared Function GetTotalAmount(payrolls As List(Of PayrollModel)) As Double
            Dim amount As Double = 0
            For i As Integer = 0 To payrolls.Count - 1
                amount += payrolls(i).Gross_Pay
            Next
            Return amount
        End Function

        Public Shared Function GetBankEECount(payrolls As List(Of PayrollModel), bankName As String, bankCategory As String) As List(Of PayrollModel)
            Dim filteredPayrolls As New List(Of PayrollModel)
            For i As Integer = 0 To payrolls.Count - 1
                If bankName = payrolls(i).Bank_Name And bankCategory = payrolls(i).Bank_Category And payrolls(i).Net_Pay > 0 Then
                    filteredPayrolls.Add(payrolls(i))
                End If
            Next
            Return filteredPayrolls
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="payrolls"></param>
        ''' <returns>Payrolls with Negative Net Pay.</returns>
        Public Shared Function GetBankEECount_Negative(payrolls As List(Of PayrollModel)) As List(Of PayrollModel)
            Dim filteredPayrolls As New List(Of PayrollModel)
            For i As Integer = 0 To payrolls.Count - 1
                If payrolls(i).Net_Pay <= 0 Then
                    filteredPayrolls.Add(payrolls(i))
                End If
            Next
            Return filteredPayrolls
        End Function


#Region "Process PayRegister"
        Public Shared Sub ProcessPayRegister(databaseManager As Manager.Mysql, payregPath As String, loggingService As monitoring_module.Logging.LoggingService)
            Dim nWorkBook As IWorkbook
            Using nNewPayreg As IO.FileStream = New FileStream(payregPath, FileMode.Open, FileAccess.Read)
                nWorkBook = New HSSFWorkbook(nNewPayreg)
            End Using

            Dim nSheet As ISheet = nWorkBook.GetSheetAt(0)

            Dim grossIdx As Integer = FindHeaderColumnIndex("GROSS", nSheet)
            Dim regpayIdx As Integer = FindHeaderColumnIndex("REG_PAY", nSheet)
            Dim idIdx As Integer = FindHeaderColumnIndex("ID", nSheet)
            Dim nameIdx As Integer = FindHeaderColumnIndex("NAME", nSheet)
            Dim payrollDate As Date = FindPayrollDate(nSheet)

            For i As Integer = 5 To nSheet.LastRowNum
                Dim row As IRow = nSheet.GetRow(i)
                If row IsNot Nothing Then
                    Dim employee_id As String = ""
                    If idIdx > 0 Then
                        If row.GetCell(idIdx) Is Nothing Then Continue For
                        employee_id = row.GetCell(idIdx).StringCellValue.Trim
                    ElseIf nameIdx > 0 Then
                        Dim name_args As String() = ParseEmployeeDetail(row, nameIdx)
                        If name_args Is Nothing Then Continue For
                        employee_id = name_args(1).Trim
                    End If

                    Dim _employee As EmployeeModel = EmployeeGateway.Find(databaseManager, ee_id:=employee_id)
                    If _employee Is Nothing Then
                        _employee = EmployeeGateway.Save(databaseManager, New EmployeeModel() With {.EE_Id = employee_id}, loggingService)
                    End If

                    Dim newPayroll As New PayrollModel
                    newPayroll.EE = _employee
                    newPayroll.EE_Id = _employee.EE_Id
                    newPayroll.Payroll_Date = payrollDate
                    newPayroll.Gross_Pay = row.GetCell(grossIdx).NumericCellValue
                    newPayroll.Reg_Pay = row.GetCell(regpayIdx).NumericCellValue
                    PayrollGateway.Save(databaseManager, newPayroll)

                    Dim newPayrollInfo As New PayrollSnapshotModel
                    newPayrollInfo.EE_Id = _employee.EE_Id
                    newPayrollInfo.Payroll_Date = payrollDate
                    newPayrollInfo.Payroll_Code = _employee.EE_Id
                    newPayrollInfo.EE_Id = _employee.EE_Id

                    If {28, 29, 30}.Contains(payrollDate.Day) Then ' If 30th Payroll
                        'GENERATE GOVERNMENT COMPUTATION
                        'GET 15TH AND 30TH PAYROLL
                        Dim payroll15th As PayrollModel = PayrollGateway.Find(databaseManager, _employee.EE_Id, payrollDate.ToString("yyyy-MM-15"))
                        Dim payroll30th As PayrollModel = PayrollGateway.Find(databaseManager, _employee.EE_Id, payrollDate.ToString("yyyy-MM-dd"))
                        If payroll30th Is Nothing Then Continue For

                        Dim payroll15th_GROSS_PAY As Double = 0
                        Dim payroll15th_REG_PAY As Double = 0
                        If payroll15th IsNot Nothing Then
                            payroll15th_GROSS_PAY = payroll15th.Gross_Pay
                            payroll15th_REG_PAY = payroll15th.Reg_Pay
                        End If

                        'INSTANTIATE A GOVERNMENT MODEL WITH 15TH AND 30TH PAYROLL AND EE ID AS PARAMETER
                        Dim goverment As New GovernmentModel
                        goverment.Payroll_Name = newPayroll.Payroll_Name
                        goverment.EE_Id = _employee.EE_Id
                        goverment.Monthly_Reg_Pay = payroll15th_REG_PAY + payroll30th.Reg_Pay
                        goverment.Monthly_Gross_Pay = payroll15th_GROSS_PAY + payroll30th.Gross_Pay
                        goverment = GovernmentGateway.Compute(goverment)
                        'SAVE GOVERMENT MODEL
                        GovernmentGateway.Save(databaseManager, goverment)
                    End If
                End If
            Next
        End Sub

        Public Shared Function FindHeaderColumnIndex(header As String, sheet As ISheet) As Integer
            For Each row As IRow In {sheet.GetRow(0), sheet.GetRow(1), sheet.GetRow(2)}
                If row IsNot Nothing Then
                    For Each cell In row.Cells
                        If cell.StringCellValue.ToUpper = header Then
                            Return cell.ColumnIndex
                        End If
                    Next
                End If
            Next
            Return Nothing
        End Function

        Public Shared Function FindPayrollDate(nSheet As ISheet) As Date
            Dim payrollDateRaw As String = ""

            If nSheet.GetRow(3) IsNot Nothing AndAlso nSheet.GetRow(3).GetCell(1) IsNot Nothing Then
                payrollDateRaw = nSheet.GetRow(3).GetCell(1).StringCellValue.Trim.Replace("*", "").Trim
            ElseIf nSheet.GetRow(4) IsNot Nothing AndAlso nSheet.GetRow(4).GetCell(0) IsNot Nothing Then
                payrollDateRaw = nSheet.GetRow(4).GetCell(0).StringCellValue.Split(":")(1).Trim
            End If

            Return Date.ParseExact(payrollDateRaw, "dd MMMM yyyy", CultureInfo.InvariantCulture)
        End Function

        ''' <summary>
        ''' Parses Fullname and Employee ID.
        ''' </summary>
        ''' <param name="row"></param>
        ''' <returns></returns>
        Public Shared Function ParseEmployeeDetail(row As IRow, nameIdx As Integer) As String()
            If row.GetCell(nameIdx) IsNot Nothing Then
                Dim fullname_raw As String() = row.GetCell(nameIdx).StringCellValue.Trim(")").Split("(")
                If fullname_raw.Length < 2 Then Return Nothing

                Return {fullname_raw(0).Trim, fullname_raw(1).Trim}
            End If
            Return Nothing
        End Function
#End Region

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="row"></param>
        ''' <returns></returns>
        Public Shared Function WriteGovernment_KS(row As IRow, payroll As PayrollModel, payrollDate As String) As Object
            row.GetCell(7).SetCellValue(payroll.Government.Pagibig_EE) 'EE
            row.GetCell(8).SetCellValue(payroll.Government.Pagibig_EE) 'ER
            row.GetCell(9).SetCellValue(payroll.Government.SSS_EE) 'EE
            row.GetCell(10).SetCellValue(payroll.Government.SSS_ER) 'ER

            Dim adjustIdx As Integer = 0 'adjust idx for 15th payroll
            If payrollDate.Substring(0, 2) = "15" Then adjustIdx = 1

            If payrollDate.Substring(0, 2) = "15" Then
                row.GetCell(11).SetCellValue(payroll.Government.SSS_EE + payroll.Government.PhilHealth) 'SSS+Phic EE
                row.GetCell(12).SetCellValue(payroll.Government.SSS_ER + payroll.Government.PhilHealth) 'SSS+Phic EE
                row.GetCell(13).SetCellValue(payroll.Government.Withholding_Tax) 'SSS+Phic EE
            Else
                row.GetCell(11).SetCellValue(payroll.Government.PhilHealth) 'SSS+Phic EE
                row.GetCell(12).SetCellValue(payroll.Government.Withholding_Tax)
                row.GetCell(13).SetCellValue(payroll.Government.Pagibig_EE) 'ADJUST 1
            End If

            Dim net As Double = payroll.Gross_Pay - (payroll.Government.SSS_EE + payroll.Government.PhilHealth + payroll.Government.Withholding_Tax + payroll.Adjust1 + payroll.Adjust2)
            row.GetCell(15 + adjustIdx).SetCellValue(net)

            Return row
        End Function







    End Class
End Namespace
