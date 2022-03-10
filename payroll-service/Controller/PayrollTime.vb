Imports System.IO
Imports System.Text
Imports System.Windows.Forms
Imports MySql.Data.MySqlClient
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

        Public Shared Sub SavePayrollTimeToDBF(databaseManager As Manager.Mysql, payrollDate As Date, payroll_code As String, bank_category As String, dbfPath As String, coder As String)
            Try
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


                Dim records As New List(Of String())
                'Dim values As List(Of Model.PayrollTime) = arg(0)
                Dim payrollTimes As List(Of Model.PayrollTime) = LoadPayrollTimes(databaseManager, payroll_code:=payroll_code, bank_category:=bank_category, payroll_date:=payrollDate.ToString("yyyy-MM-dd"), completeDetail:=True)
                For r As Integer = 0 To payrollTimes.Count - 1
                    payrollTimes(r).CODE = coder
                    records.Add(payrollTimes(r).ToDBFRecordFormat)
                Next

                If records.Count > 0 Then
                    Dim s As Stream = File.Create(String.Format("{0}/{1}_{2}_{3}.DBF", dbfPath, payroll_code, bank_category, payrollDate.ToString("yyyyMMdd")))
                    Dim writer As New DotNetDBF.DBFWriter(s)
                    writer.CharEncoding = Encoding.UTF8
                    writer.Fields = flds.ToArray
                    For Each record As String() In records
                        writer.WriteRecord(record)
                    Next
                    writer.Close()
                    s.Close()
                    'WriteTimesheetExcelReport(Path.ChangeExtension(dbfPath, "xls"), arg(0), values(0).PayrollCode, values(0).BankCategory, payrollDate)
                End If

            Catch ex As Exception
                MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub
    End Class
End Namespace
