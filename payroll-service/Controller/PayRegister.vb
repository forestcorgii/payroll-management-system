Imports System.Globalization
Imports System.IO
Imports NPOI.HSSF.UserModel
Imports NPOI.SS.UserModel
Imports utility_service

Namespace Controller

    Public Class PayRegister
#Region "Process PayRegister"
        Public Shared Sub ProcessPayRegister(databaseManager As Manager.Mysql, payregPath As String)
            Dim nWorkBook As IWorkbook
            Using nNewPayreg As IO.FileStream = New FileStream(payregPath, FileMode.Open, FileAccess.Read)
                nWorkBook = New HSSFWorkbook(nNewPayreg)
            End Using

            Dim nSheet As ISheet = nWorkBook.GetSheetAt(0)

            Dim grossIdx As Integer = FindHeaderColumnIndex("GROSS", nSheet)
            Dim idIdx As Integer = FindHeaderColumnIndex("ID", nSheet)
            Dim nameIdx As Integer = FindHeaderColumnIndex("NAME", nSheet)
            Dim payrollDate As Date = FindPayrollDate(nSheet)

            For i As Integer = 5 To nSheet.LastRowNum
                Dim row As IRow = nSheet.GetRow(i)
                If row IsNot Nothing Then
                    Dim employee As Model.Employee = Controller.Employee.GetEmployees(databaseManager, employee_id:="DYYJ")
                    Dim newPayroll As New Model.Payroll
                    newPayroll.EE_Id = employee.Id
                    newPayroll.Payroll_Date = payrollDate
                    newPayroll.Gross_Pay = row.GetCell(grossIdx).NumericCellValue
                    'Controller.Payroll.
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

            If nSheet.GetRow(3) IsNot Nothing Then
                payrollDateRaw = nSheet.GetRow(3).GetCell(1).StringCellValue.Trim.Replace("*", "").Trim
            ElseIf nSheet.GetRow(4) IsNot Nothing Then
                payrollDateRaw = nSheet.GetRow(4).GetCell(1).StringCellValue.Split(":")(1).Trim
            End If

            Return Date.ParseExact(payrollDateRaw, "dd MMMM yyyy", CultureInfo.InvariantCulture)
        End Function
#End Region

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="row"></param>
        ''' <returns></returns>
        Public Shared Function WriteGovernment_KS(row As IRow, payroll As Model.Payroll, payrollDate As String) As Object
            row.GetCell(7).SetCellValue(payroll.Pagibig_EE) 'EE
            row.GetCell(8).SetCellValue(payroll.Pagibig_EE) 'ER
            row.GetCell(9).SetCellValue(payroll.SSS_EE) 'EE
            row.GetCell(10).SetCellValue(payroll.SSS_ER) 'ER

            Dim adjustIdx As Integer = 0 'adjust idx for 15th payroll
            If payrollDate.Substring(0, 2) = "15" Then adjustIdx = 1

            If payrollDate.Substring(0, 2) = "15" Then
                row.GetCell(11).SetCellValue(payroll.SSS_EE + payroll.PhilHealth) 'SSS+Phic EE
                row.GetCell(12).SetCellValue(payroll.SSS_ER + payroll.PhilHealth) 'SSS+Phic EE
                row.GetCell(13).SetCellValue(payroll.Withholding_Tax) 'SSS+Phic EE
            Else
                row.GetCell(11).SetCellValue(payroll.PhilHealth) 'SSS+Phic EE
                row.GetCell(12).SetCellValue(payroll.Withholding_Tax)
                row.GetCell(13).SetCellValue(payroll.Pagibig_EE) 'ADJUST 1
            End If

            Dim net As Double = payroll.Gross_Pay - (payroll.SSS_EE + payroll.PhilHealth + payroll.Withholding_Tax + payroll.Adjust1 + payroll.Adjust2)
            row.GetCell(15 + adjustIdx).SetCellValue(net)

            Return row
        End Function
    End Class
End Namespace