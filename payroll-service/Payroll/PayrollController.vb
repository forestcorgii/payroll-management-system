Imports System.Windows.Forms
Imports employee_module
Imports System.IO
Imports NPOI.HSSF.UserModel
Imports NPOI.SS.UserModel
Imports payroll_module.Payroll

Namespace Payroll
    Public Class PayrollController
        Public Shared Function GetCutoffRange(payrollDate As Date) As Date()
            If {28, 29, 30}.Contains(payrollDate.Day) Then
                Return {New Date(payrollDate.Year, payrollDate.Month, 5), New Date(payrollDate.Year, payrollDate.Month, 19)}
            ElseIf 15 = payrollDate.Day Then
                Dim previousMonth As Date = payrollDate.AddMonths(-1)
                Return {New Date(previousMonth.Year, previousMonth.Month, 20), New Date(payrollDate.Year, payrollDate.Month, 4)}
            End If
            Return Nothing
        End Function

        Public Shared Function GetPreviousPayrollDate(currentPayrollDate As Date) As Date
            Dim previousPayrollDate As Date
            If {28, 29, 30}.Contains(currentPayrollDate.Day) Then
                previousPayrollDate = String.Format("{0}-{1:00}-15", currentPayrollDate.Year, currentPayrollDate.Month)
            ElseIf 15 = currentPayrollDate.Day Then
                Dim day As String = "30"
                Dim _date = currentPayrollDate.AddMonths(-1)
                If _date.Month = 2 Then
                    day = IIf(Date.IsLeapYear(_date.Year), 29, 28)
                End If

                previousPayrollDate = String.Format("{0}-{1:00}-{2}", _date.Year, _date.Month, day)
            End If

            Return Date.Parse(previousPayrollDate)
        End Function

        Public Shared Function CompleteDetail(databaseManager As utility_service.Manager.Mysql, payroll As PayrollModel) As PayrollModel
            Try
                payroll.EE = EmployeeGateway.Find(databaseManager, payroll.EE_Id)

                If Not payroll.Have_Government Then
                    payroll.Government = GovernmentGateway.Find(databaseManager, payroll.Payroll_Name)
                End If

                If Not payroll.Have_Adjustment Then
                    payroll.AdjustmentLogs = AdjustmentRecordController.CollectOrGenerateBillings(databaseManager, payroll.EE_Id, payroll.Payroll_Name)
                    payroll = PayrollGateway.Save(databaseManager, payroll)
                End If

            Catch ex As Exception
                MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
            Return payroll
        End Function





#Region "Export Bank Report"
        Public Shared Sub ExportBankReport(ByRef payArr As List(Of PayrollModel), filePath As String, payrollDate As Date, payrollCodes As String, bankName As String, bankCategory As String, companyName As String)
            Dim excelTemplate As String = String.Format("{0}\templates\template-{1}.xls", AppDomain.CurrentDomain.BaseDirectory, bankName)
            Dim METROTACFile As String = filePath & "\" & bankName
            If Not File.Exists(excelTemplate) Then
                MsgBox("File not found:" & vbNewLine & excelTemplate, MsgBoxStyle.Critical, "Oops")
                Exit Sub
            End If
            Try
                Dim filename As String = String.Format("{0}-{1}_{2:yyyyMMdd}-{3}", companyName, payrollCodes, payrollDate, bankName)
                Dim duplicateCount As Integer = Directory.GetFiles(filePath, "**" & filename & "**").Length
                If duplicateCount > 0 Then
                    filename = String.Format("{0}{1}({2}).xls", filePath, filename, duplicateCount)
                Else
                    filename = String.Format("{0}{1}.xls", filePath, filename)
                End If

                File.Copy(excelTemplate, filename)

                Dim nWorkbook As IWorkbook
                Using nTemplateFile As IO.FileStream = New FileStream(filename, FileMode.OpenOrCreate, FileAccess.Read)
                    nWorkbook = New HSSFWorkbook(nTemplateFile)
                End Using

                If bankName = "CHK" Then
                    ExportDataCHECK(payArr, filename, nWorkbook)
                Else
                    Dim nSheet As ISheet = nWorkbook.GetSheetAt(0)
                    Select Case (bankName & " " & bankCategory).Trim
                        Case "UCPB CCARD"
                            ExportDataUCPB(payArr, nSheet)
                        Case "CHINABANK CCARD"
                            ExportDataCHINABANK(payArr, nSheet)
                        Case "ZEROS"
                            ExportDataZeros(payArr, nSheet)
                    End Select

                    Using nReportFile As IO.FileStream = New FileStream(filename, FileMode.Open, FileAccess.Write)
                        nWorkbook.Write(nReportFile)
                    End Using
                End If

            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try
        End Sub

        Public Shared Sub ExportDataUCPB(ByRef payArr As List(Of PayrollModel), sheet As ISheet)
            For i As Integer = 0 To payArr.Count - 1
                Dim rec As PayrollModel = payArr(i)
                Dim row As IRow = sheet.CreateRow(i)
                row.CreateCell(0).SetCellValue(rec.EE.First_Name)
                row.CreateCell(1).SetCellValue(rec.EE.Last_Name)
                row.CreateCell(2).SetCellValue(rec.EE.Middle_Name)
                If Trim(rec.EE.Account_Number).Length = 14 Then
                    row.CreateCell(3).SetCellValue("5900010" & rec.EE.Card_Number)
                Else
                    row.CreateCell(3).SetCellValue(rec.EE.Card_Number)
                End If
                row.CreateCell(4).SetCellValue(rec.Net_Pay)
            Next
        End Sub

        Public Shared Sub ExportDataCHINABANK(ByRef payArr As List(Of PayrollModel), sheet As ISheet)
            For i As Integer = 0 To payArr.Count - 1
                Dim rec As PayrollModel = payArr(i)
                Dim row As IRow = sheet.CreateRow(i + 5)
                row.CreateCell(3).SetCellValue(rec.EE.Account_Number)
                row.CreateCell(4).SetCellValue(rec.EE.Last_Name)
                row.CreateCell(5).SetCellValue(rec.EE.First_Name)
                row.CreateCell(6).SetCellValue(rec.EE.Middle_Name)
                row.CreateCell(7).SetCellValue(rec.Net_Pay)
            Next
        End Sub


        Public Shared Sub ExportDataMETROPALO(ByRef payArr As List(Of PayrollModel), sheet As ISheet)
            For i As Integer = 0 To payArr.Count - 1
                Dim rec As PayrollModel = payArr(i)
                Dim row As IRow = sheet.CreateRow(i)
                row.CreateCell(0).SetCellValue(rec.EE.Last_Name)
                row.CreateCell(1).SetCellValue(rec.EE.First_Name)
                row.CreateCell(2).SetCellValue(rec.EE.Middle_Name)
                row.CreateCell(3).SetCellValue("756" & rec.EE.Account_Number)
                row.CreateCell(4).SetCellValue(rec.Net_Pay)
            Next
        End Sub
        Public Shared Sub ExportDataMETROTAC(ByRef payArr As List(Of PayrollModel), sheet As ISheet)
            For i As Integer = 0 To payArr.Count - 1
                Dim rec As PayrollModel = payArr(i)
                Dim row As IRow = sheet.CreateRow(i)
                row.CreateCell(0).SetCellValue(rec.EE.Last_Name)
                row.CreateCell(1).SetCellValue(rec.EE.First_Name)
                row.CreateCell(2).SetCellValue(rec.EE.Middle_Name)
                row.CreateCell(3).SetCellValue("525" & rec.EE.Account_Number)
                row.CreateCell(4).SetCellValue(rec.Net_Pay)
            Next
        End Sub

        Public Shared Sub ExportDataZeros(ByRef payArr As List(Of PayrollModel), sheet As ISheet)
            Dim row As IRow = sheet.CreateRow(0)
            row.CreateCell(0).SetCellValue("IDNo")
            row.CreateCell(1).SetCellValue("Fullname")
            row.CreateCell(2).SetCellValue("Amount")

            For i As Integer = 0 To payArr.Count - 1
                Dim rec As PayrollModel = payArr(i)
                row = sheet.CreateRow(i + 1)
                row.CreateCell(0).SetCellValue(rec.EE.EE_Id)
                row.CreateCell(1).SetCellValue(rec.EE.Fullname)
                row.CreateCell(2).SetCellValue(rec.Net_Pay)
            Next
        End Sub




#Region "CHECK"
        Public Shared Sub ExportDataCHECK(ByRef payArr As List(Of PayrollModel), filePath As String, workbook As IWorkbook)
            Try
                Dim xl200DOWNSheet As ISheet = workbook.GetSheetAt(0)
                Dim xl7500DOWNSheet As ISheet = workbook.GetSheetAt(1)
                Dim xl7500UPSheet As ISheet = workbook.GetSheetAt(2)
                Dim xl100KUPSheet As ISheet = workbook.GetSheetAt(3)

                Dim Records200DOWN As PayrollModel() = (From res In payArr Where res.Net_Pay <= 200 And res.Net_Pay > 0 Select res).ToArray
                Dim Records7500DOWN As PayrollModel() = (From res In payArr Where res.Net_Pay < 7500 And res.Net_Pay > 200 Select res).ToArray
                Dim Recordsd7500UP As PayrollModel() = (From res In payArr Where res.Net_Pay >= 7500 Select res).ToArray
                Dim Records100KUP As PayrollModel() = (From res In payArr Where res.Net_Pay >= 100000 Select res).ToArray

                WriteSpecificReport(xl7500UPSheet, Recordsd7500UP)
                WriteSpecificReport(xl7500DOWNSheet, Records7500DOWN)
                WriteSpecificReport(xl200DOWNSheet, Records200DOWN)
                WriteSpecificReport(xl100KUPSheet, Records100KUP)

                Using nNewPayreg As IO.FileStream = New FileStream(filePath, FileMode.Open, FileAccess.Write)
                    workbook.Write(nNewPayreg)
                End Using
            Catch ex As Exception
                MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical, "Oops")
            End Try
        End Sub
        Public Shared Sub WriteSpecificReport(sheet As ISheet, payrollRecords As PayrollModel())
            Dim row As IRow = sheet.CreateRow(0)
            row.CreateCell(0).SetCellValue("IDNo")
            row.CreateCell(1).SetCellValue("Fullname")
            row.CreateCell(2).SetCellValue("Amount")

            For i As Integer = 0 To payrollRecords.Count - 1
                Dim rec As PayrollModel = payrollRecords(i)
                row = sheet.CreateRow(i + 1)
                row.CreateCell(0).SetCellValue(rec.EE.EE_Id)
                row.CreateCell(1).SetCellValue(rec.EE.Fullname)
                row.CreateCell(2).SetCellValue(rec.Net_Pay)
            Next
        End Sub
#End Region
#End Region
    End Class

End Namespace
