Imports System.IO
Imports System.Text
Imports System.Windows.Forms
Imports MySql.Data.MySqlClient
Imports NPOI.HSSF.UserModel
Imports NPOI.SS.UserModel
Imports payroll_module.Employee
Imports monitoring_module

Namespace Payroll

    Namespace Time

        Public Class Controller
            Public Shared Function ToDBFRecordFormat(payrollTime As Model) As String()
                Return {payrollTime.DATER, payrollTime.CODE, payrollTime.EE_Id, payrollTime.Total_Hours, payrollTime.Total_OTs, payrollTime.Total_RD_OT, 0, payrollTime.Total_H_OT, 0, payrollTime.Total_ND, payrollTime.Total_Tardy, payrollTime.Allowance, 0, 0, 0, 0, 0, 0, 0, 0, 0}
            End Function
            Public Shared Sub ToEERowFormat(row As IRow, payrollTime As Model)
                row.CreateCell(1).SetCellValue(payrollTime.EE_Id)
                row.CreateCell(2).SetCellValue(payrollTime.EE.Fullname)
                row.CreateCell(3).SetCellValue(payrollTime.Total_Hours)
                row.CreateCell(4).SetCellValue(payrollTime.Total_OTs)
                row.CreateCell(5).SetCellValue(payrollTime.Total_RD_OT)
                row.CreateCell(6).SetCellValue(payrollTime.Total_H_OT)
                row.CreateCell(7).SetCellValue(payrollTime.Total_ND)
                row.CreateCell(8).SetCellValue(payrollTime.Total_Tardy)
            End Sub
            Public Shared Sub ProcessPayrollTime(databaseManager As utility_service.Manager.Mysql, payrollDate As Date, payrollTime As time_module.Model.PayrollTime, loggingService As Logging.LoggingService)
                Try
                    'check if employee exists in the database, create one if not.
                    Dim command As New MySqlCommand
                    Dim _employee As Employee.EmployeeModel = Employee.EmployeeGateway.Find(databaseManager, ee_id:=payrollTime.EE_Id)
                    Dim ee_id As String = 0
                    If _employee Is Nothing Then
                        _employee = Employee.EmployeeGateway.Save(databaseManager, New Employee.EmployeeModel() With {.EE_Id = payrollTime.EE_Id}, loggingService)
                    End If
                    ee_id = _employee.EE_Id
                    payrollTime.Payroll_Date = payrollDate
                    Time.Gateway.Save(databaseManager, payrollTime)

                    Dim allowanceLog As New Adjustment.Log.Model
                    With allowanceLog
                        .Name = "ALLOWANCE"
                        .ee_id = ee_id
                        .Payroll_Name = payrollTime.Payroll_Name
                        .Amount = payrollTime.Allowance
                        .Adjust_Type = Adjustment.AdjustmentChoices.ADJUST1
                    End With
                    Adjustment.Log.Gateway.SaveAdjustmentLog(databaseManager, allowanceLog)
                Catch ex As Exception
                    Console.WriteLine(ex.Message)
                    MessageBox.Show(ex.Message, "SavePayrollAsync", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End Sub

            Public Shared Function CompleteDetail(databaseManager As utility_service.Manager.Mysql, payrollTime As Model) As Model
                Try
                    payrollTime.EE = Employee.EmployeeGateway.Find(databaseManager, payrollTime.EE_Id)
                Catch ex As Exception
                    MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
                Return payrollTime
            End Function


            Private Shared Function GetDBFFields() As List(Of DotNetDBF.DBFField)
                Dim flds As New List(Of DotNetDBF.DBFField)
                flds.Add(New DotNetDBF.DBFField("DATER", DotNetDBF.NativeDbType.Numeric, 10, 0))
                flds.Add(New DotNetDBF.DBFField("CODE", DotNetDBF.NativeDbType.Numeric, 10, 0))
                flds.Add(New DotNetDBF.DBFField("ID", DotNetDBF.NativeDbType.Char, 4, 0))
                flds.Add(New DotNetDBF.DBFField("REG_HRS", DotNetDBF.NativeDbType.Float, 10, 2))
                flds.Add(New DotNetDBF.DBFField("R_OT", DotNetDBF.NativeDbType.Float, 10, 2))
                flds.Add(New DotNetDBF.DBFField("RD_OT", DotNetDBF.NativeDbType.Float, 10, 2))
                flds.Add(New DotNetDBF.DBFField("RD_8", DotNetDBF.NativeDbType.Float, 10, 2))
                flds.Add(New DotNetDBF.DBFField("HOL_OT", DotNetDBF.NativeDbType.Float, 10, 2))
                flds.Add(New DotNetDBF.DBFField("HOL_OT8", DotNetDBF.NativeDbType.Float, 10, 2))
                flds.Add(New DotNetDBF.DBFField("ND", DotNetDBF.NativeDbType.Float, 10, 2))
                flds.Add(New DotNetDBF.DBFField("ABS_TAR", DotNetDBF.NativeDbType.Float, 10, 2))
                flds.Add(New DotNetDBF.DBFField("ADJUST1", DotNetDBF.NativeDbType.Float, 10, 2))
                flds.Add(New DotNetDBF.DBFField("GROSS_PAY", DotNetDBF.NativeDbType.Float, 10, 2))
                flds.Add(New DotNetDBF.DBFField("ADJUST2", DotNetDBF.NativeDbType.Float, 10, 2))
                flds.Add(New DotNetDBF.DBFField("TAX", DotNetDBF.NativeDbType.Float, 10, 2))
                flds.Add(New DotNetDBF.DBFField("SSS_EE", DotNetDBF.NativeDbType.Float, 10, 2))
                flds.Add(New DotNetDBF.DBFField("SSS_ER", DotNetDBF.NativeDbType.Float, 10, 2))
                flds.Add(New DotNetDBF.DBFField("PHIC", DotNetDBF.NativeDbType.Float, 10, 2))
                flds.Add(New DotNetDBF.DBFField("NET_PAY", DotNetDBF.NativeDbType.Float, 10, 2))
                flds.Add(New DotNetDBF.DBFField("REG_PAY", DotNetDBF.NativeDbType.Float, 10, 2))
                flds.Add(New DotNetDBF.DBFField("TAG", DotNetDBF.NativeDbType.Char, 1, 0))
                Return flds
            End Function


            Public Shared Sub SavePayrollTimeToDBF(databaseManager As utility_service.Manager.Mysql, payrollDate As Date, payroll_code As String, bank_category As String, dbfPath As String)
                Try
                    Dim dbfFields As List(Of DotNetDBF.DBFField) = GetDBFFields()

                    Dim dbfRecords As New List(Of String())
                    Dim transferDetails As New List(Of String)
                    Dim payrollTimes As List(Of Model) = Gateway.Collect(databaseManager, payroll_code:=payroll_code, bank_category:=bank_category, payroll_date:=payrollDate.ToString("yyyy-MM-dd"), completeDetail:=True)

                    ExportEFile(String.Format("{0}\{1}_{2}_{3}.XLS", dbfPath, payroll_code, bank_category, payrollDate.ToString("yyyyMMdd")), payrollDate, payroll_code, bank_category, payrollTimes)
                    ExportDBF(String.Format("{0}\{1}_{2}_{3}.DBF", dbfPath, payroll_code, bank_category, payrollDate.ToString("yyyyMMdd")), payrollDate, payrollTimes)
                Catch ex As Exception
                    MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End Sub

            Private Shared Sub ExportDBF(location As String, payrollDate As Date, records As List(Of Model))
                If records.Count > 0 Then
                    Dim dbfStream As Stream = File.Create(location)

                    Dim dbfWriter As New DotNetDBF.DBFWriter(dbfStream)
                    dbfWriter.CharEncoding = Encoding.UTF8
                    dbfWriter.Fields = GetDBFFields().ToArray

                    For r As Integer = 0 To records.Count - 1
                        records(r).CODE = IIf(payrollDate.Day = 15, 1, 2)
                        dbfWriter.WriteRecord(ToDBFRecordFormat(records(r)))
                    Next

                    dbfWriter.Close()
                    dbfStream.Close()
                End If
            End Sub

            Private Shared Sub ExportEFile(location As String, payrollDate As Date, payroll_code As String, bank_category As String, records As List(Of Model))
                If records.Count = 0 Then Exit Sub

                Dim cutoffRange As Date() = Payroll.Controller.GetCutoffRange(payrollDate)

                Dim nWorkbook As New HSSFWorkbook()
                Dim nSheet As ISheet = nWorkbook.CreateSheet("Sheet1")

                Dim nRow As IRow = nSheet.CreateRow(0)
                nRow.CreateCell(2).SetCellValue(String.Format("{0} - {1}", payroll_code, bank_category))
                nRow = nSheet.CreateRow(1)
                nRow.CreateCell(2).SetCellValue(String.Format("{0:MMMM d} - {1:MMMM dd, yyyy}", cutoffRange(0), cutoffRange(1)))
                nRow = nSheet.CreateRow(3)
                nRow.CreateCell(0).SetCellValue("#")
                nRow.CreateCell(1).SetCellValue("# ID")
                nRow.CreateCell(2).SetCellValue("NAME")
                nRow.CreateCell(3).SetCellValue("REG HRS")
                nRow.CreateCell(4).SetCellValue("R OT")
                nRow.CreateCell(5).SetCellValue("RD OT")
                nRow.CreateCell(6).SetCellValue("HOL OT")
                nRow.CreateCell(7).SetCellValue("ND")
                nRow.CreateCell(8).SetCellValue("TARDY")

                For r As Integer = 0 To records.Count - 1
                    nRow = nSheet.CreateRow(4 + r)
                    nRow.CreateCell(0).SetCellValue(r + 1)

                    ToEERowFormat(nRow, records(r))
                Next

                Using nEFile As IO.FileStream = New FileStream(location, FileMode.Create, FileAccess.Write)
                    nWorkbook.Write(nEFile)
                End Using
            End Sub

            Public Shared Sub ExportTransferLog(databaseManager As utility_service.Manager.Mysql, location As String, payrollDate As Date, payroll_code As String)
                Dim transferDetails As New List(Of String())
                Dim previousPayrollDate As Date = Payroll.Controller.GetPreviousPayrollDate(payrollDate)
                Dim previousCutoffRange As Date() = Payroll.Controller.GetCutoffRange(previousPayrollDate)

                Dim nWorkbook As New HSSFWorkbook()
                Dim nSheet As ISheet = nWorkbook.CreateSheet("Sheet1")

                Dim i As Integer = 0
                Dim nRow As IRow = nSheet.CreateRow(i)
                nRow.CreateCell(2).SetCellValue("TRANSFERRED Log")

                Dim payrollCodes As List(Of PayrollCode.Model) = PayrollCode.Gateway.CollectFromPreviousCutOff(databaseManager, payroll_code, previousCutoffRange)
                For Each payrollCode As PayrollCode.Model In payrollCodes
                    Dim ee As Employee.EmployeeModel = Employee.EmployeeGateway.Find(databaseManager, payrollCode.EE_Id)
                    If ee.Payroll_Code <> payrollCode.Payroll_code Then
                        transferDetails.Add({ee.EE_Id, ee.Fullname, String.Format("Transferred from {0} to {1}", ee.Payroll_Code, payrollCode.Payroll_code)})
                    End If
                Next

                'Dim bankCategories As List(Of Model.BankCategory) = BankCategory.CollectFromPreviousCutOff(databaseManager, bank_category, previousCutoffRange)
                'For Each bankCategory As Model.BankCategory In bankCategories
                '    Dim ee As employee.model = Employee.GetEmployee(databaseManager, bankCategory.EE_Id)
                '    If ee.Bank_Category <> bankCategory.Bank_Category Then
                '        transferDetails.Add({ee.EE_Id, ee.Fullname, String.Format("Transferred from {0} to {1}", ee.Bank_Category, bankCategory.Bank_Category)})
                '    End If
                'Next

                For r As Integer = 0 To transferDetails.Count - 1
                    nRow = nSheet.CreateRow(i + r)
                    nRow.CreateCell(0).SetCellValue(transferDetails(r)(0))
                    nRow.CreateCell(1).SetCellValue(transferDetails(r)(1))
                    nRow.CreateCell(2).SetCellValue(transferDetails(r)(2))
                Next
                'String.Format("{0}\{1}_{2}_{3}.DBF", dbfPath, payroll_code, bank_category, payrollDate.ToString("yyyyMMdd"))

                Using transferLogWriter As IO.FileStream = New FileStream(String.Format("{0}\{1}_{2}.XLS", location, payroll_code, payrollDate.ToString("yyyyMMdd")), FileMode.Create, FileAccess.Write)
                    nWorkbook.Write(transferLogWriter)
                End Using
            End Sub

        End Class
    End Namespace
End Namespace
