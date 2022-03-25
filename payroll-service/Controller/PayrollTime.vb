Imports System.IO
Imports System.Text
Imports System.Windows.Forms
Imports MySql.Data.MySqlClient
Imports NPOI.HSSF.UserModel
Imports NPOI.SS.UserModel
Imports utility_service


Namespace Controller
    Public Class PayrollTime
        Public Shared Function GetPayrollTime(databaseManager As Manager.Mysql, Optional id As Integer = 0, Optional employee_id As String = "") As Model.PayrollTime
            Dim payrollTime As Model.PayrollTime = Nothing
            Using reader As MySqlDataReader = databaseManager.ExecuteDataReader(
                String.Format("SELECT * FROM payroll_management.payroll_time; where id={0} or employee_id='{1}' LIMIT 1;", id, employee_id))
                If reader.HasRows Then
                    reader.Read()
                    payrollTime = New Model.PayrollTime(reader)
                End If
            End Using

            Return payrollTime
        End Function

        Public Shared Function LoadPayrollTimes(databaseManager As Manager.Mysql, Optional location As String = "", Optional payroll_code As String = "", Optional bank_category As String = "", Optional payroll_date As String = "", Optional job_title As String = "", Optional completeDetail As Boolean = False) As List(Of Model.PayrollTime)
            Dim payrollTimes As New List(Of Model.PayrollTime)
            Using reader As MySqlDataReader = databaseManager.ExecuteDataReader(
                String.Format("SELECT * FROM payroll_management.payroll_time_complete where total_hours>0 AND (payroll_code='{1}' AND bank_category='{2}' AND payroll_date='{3}');", location, payroll_code, bank_category, payroll_date, job_title))

                While reader.Read()
                    payrollTimes.Add(New Model.PayrollTime(reader))
                End While
            End Using

            If completeDetail Then
                payrollTimes.ForEach(Sub(item As Model.PayrollTime) CompletePayrollTimeDetail(databaseManager, item))
            End If

            Return payrollTimes
        End Function

        Public Shared Function CompletePayrollTimeDetail(databaseManager As Manager.Mysql, payrollTime As Model.PayrollTime) As Model.PayrollTime
            Try
                payrollTime.EE = Employee.GetEmployee(databaseManager, payrollTime.EE_Id)
            Catch ex As Exception
                MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
            Return payrollTime
        End Function

        Public Shared Sub SavePayrollTime(databaseManager As Manager.Mysql, payrollTime As Model.PayrollTime)
            'check if employee exists in the database
            Try
                Dim Command As New MySqlCommand("REPLACE INTO payroll_management.payroll_time (ee_id,total_hours,total_ots,total_rd_ot,total_h_ot,total_nd,total_tardy,has_pcv,payroll_date,payroll_name)VALUES(?,?,?,?,?,?,?,?,?,?)", databaseManager.Connection)
                Command.Parameters.AddWithValue("p1", payrollTime.EE_Id)
                Command.Parameters.AddWithValue("p2", payrollTime.Total_Hours)
                Command.Parameters.AddWithValue("p3", payrollTime.Total_OTs)
                Command.Parameters.AddWithValue("p4", payrollTime.Total_RD_OT)
                Command.Parameters.AddWithValue("p5", payrollTime.Total_H_OT)
                Command.Parameters.AddWithValue("p6", payrollTime.Total_ND)
                Command.Parameters.AddWithValue("p7", payrollTime.Total_Tardy)
                Command.Parameters.AddWithValue("p10", payrollTime.Has_PCV)
                Command.Parameters.AddWithValue("p11", payrollTime.Payroll_Date)
                Command.Parameters.AddWithValue("p12", payrollTime.Payroll_Name)

                Command.ExecuteNonQuery()
            Catch ex As Exception
                MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

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


        Public Shared Sub SavePayrollTimeToDBF(databaseManager As Manager.Mysql, payrollDate As Date, payroll_code As String, bank_category As String, dbfPath As String)
            Try
                Dim dbfFields As List(Of DotNetDBF.DBFField) = GetDBFFields()

                Dim dbfRecords As New List(Of String())
                Dim transferDetails As New List(Of String)
                Dim payrollTimes As List(Of Model.PayrollTime) = LoadPayrollTimes(databaseManager, payroll_code:=payroll_code, bank_category:=bank_category, payroll_date:=payrollDate.ToString("yyyy-MM-dd"), completeDetail:=True)

                ExportEFile(String.Format("{0}\{1}_{2}_{3}.XLS", dbfPath, payroll_code, bank_category, payrollDate.ToString("yyyyMMdd")), payrollDate, payroll_code, bank_category, payrollTimes)
                ExportDBF(String.Format("{0}\{1}_{2}_{3}.DBF", dbfPath, payroll_code, bank_category, payrollDate.ToString("yyyyMMdd")), payrollDate, payrollTimes)
            Catch ex As Exception
                MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Shared Sub ExportDBF(location As String, payrollDate As Date, records As List(Of Model.PayrollTime))
            If records.Count > 0 Then
                Dim dbfStream As Stream = File.Create(location)

                Dim dbfWriter As New DotNetDBF.DBFWriter(dbfStream)
                dbfWriter.CharEncoding = Encoding.UTF8
                dbfWriter.Fields = GetDBFFields().ToArray

                For r As Integer = 0 To records.Count - 1
                    records(r).CODE = IIf(payrollDate.Day = 15, 1, 2)
                    dbfWriter.WriteRecord(records(r).ToDBFRecordFormat)
                Next

                dbfWriter.Close()
                dbfStream.Close()
            End If
        End Sub

        Private Shared Sub ExportEFile(location As String, payrollDate As Date, payroll_code As String, bank_category As String, records As List(Of Model.PayrollTime))
            If records.Count = 0 Then Exit Sub

            Dim cutoffRange As Date() = GetCutoffRange(payrollDate)

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

                records(r).ToEERowFormat(nRow)
            Next

            Using nEFile As IO.FileStream = New FileStream(location, FileMode.Create, FileAccess.Write)
                nWorkbook.Write(nEFile)
            End Using
        End Sub

        Public Shared Sub ExportTransferLog(databaseManager As Manager.Mysql, location As String, payrollDate As Date, payroll_code As String)
            Dim transferDetails As New List(Of String())
            Dim previousPayrollDate As Date = GetPreviousPayrollDate(payrollDate)
            Dim previousCutoffRange As Date() = GetCutoffRange(previousPayrollDate)

            Dim nWorkbook As New HSSFWorkbook()
            Dim nSheet As ISheet = nWorkbook.CreateSheet("Sheet1")


            Dim i As Integer = 0
            Dim nRow As IRow = nSheet.CreateRow(i)
            nRow.CreateCell(2).SetCellValue("TRANSFERRED Log")

            Dim payrollCodes As List(Of Model.PayrollCode) = PayrollCode.CollectFromPreviousCutOff(databaseManager, payroll_code, previousCutoffRange)
            For Each payrollCode As Model.PayrollCode In payrollCodes
                Dim ee As Model.Employee = Employee.GetEmployee(databaseManager, payrollCode.EE_Id)
                If ee.Payroll_Code <> payrollCode.Payroll_code Then
                    transferDetails.Add({ee.EE_Id, ee.Fullname, String.Format("Transferred from {0} to {1}", ee.Payroll_Code, payrollCode.Payroll_code)})
                End If
            Next

            'Dim bankCategories As List(Of Model.BankCategory) = BankCategory.CollectFromPreviousCutOff(databaseManager, bank_category, previousCutoffRange)
            'For Each bankCategory As Model.BankCategory In bankCategories
            '    Dim ee As Model.Employee = Employee.GetEmployee(databaseManager, bankCategory.EE_Id)
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

        Private Shared Function GetCutoffRange(payrollDate As Date) As Date()
            If {28, 29, 30}.Contains(payrollDate.Day) Then
                Return {New Date(payrollDate.Year, payrollDate.Month, 5), New Date(payrollDate.Year, payrollDate.Month, 19)}
            ElseIf 15 = (payrollDate.Day) Then
                Dim previousMonth As Date = payrollDate.AddMonths(-1)
                'Return {New Date(previousMonth.Year, previousMonth.Month, 20), New Date(payrollDate.Year, payrollDate.Month, 4)}
                Return {New Date(previousMonth.Year, previousMonth.Month, 20), New Date(payrollDate.Year, payrollDate.Month, 11)}
            End If
            Return Nothing
        End Function

        Private Shared Function GetPreviousPayrollDate(currentPayrollDate As Date) As Date
            Dim previousPayrollDate As Date
            If {28, 29, 30}.Contains(currentPayrollDate.Day) Then
                previousPayrollDate = String.Format("{0}-{1:00}-15", currentPayrollDate.Year, currentPayrollDate.Month)
            ElseIf 15 = (currentPayrollDate.Day) Then
                Dim day As String = "30"
                Dim _date = currentPayrollDate.AddMonths(-1)
                If _date.Month = 2 Then
                    day = IIf(Date.IsLeapYear(_date.Year), 29, 28)
                End If

                previousPayrollDate = String.Format("{0}-{1:00}-{2}", _date.Year, _date.Month, day)
            End If

            Return Date.Parse(previousPayrollDate)
        End Function

    End Class
End Namespace
