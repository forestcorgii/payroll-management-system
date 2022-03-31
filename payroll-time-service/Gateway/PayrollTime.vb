Imports System.IO
Imports System.Text
Imports System.Windows.Forms
Imports MySql.Data.MySqlClient
Imports NPOI.HSSF.UserModel
Imports NPOI.SS.UserModel
Imports payroll_service


Namespace Gateway
    Public Class PayrollTime_
        Public Shared Function Find(databaseManager As utility_service.Manager.Mysql, payroll_name As String, ee_id As String) As Model.PayrollTime
            Dim payrollTime As Model.PayrollTime = Nothing
            Using reader As MySqlDataReader = databaseManager.ExecuteDataReader(
                String.Format("SELECT * FROM payroll_management.payroll_time; where payroll_name={0} or ee_id='{1}' LIMIT 1;", payroll_name, ee_id))
                If reader.HasRows Then
                    reader.Read()
                    payrollTime = New Model.PayrollTime(reader)
                End If
            End Using

            Return payrollTime
        End Function

        Public Shared Function Collect(databaseManager As utility_service.Manager.Mysql, Optional location As String = "", Optional payroll_code As String = "", Optional bank_category As String = "", Optional payroll_date As String = "", Optional job_title As String = "", Optional completeDetail As Boolean = False) As List(Of Model.PayrollTime)
            Dim payrollTimes As New List(Of Model.PayrollTime)
            Using reader As MySqlDataReader = databaseManager.ExecuteDataReader(
                String.Format("SELECT * FROM payroll_management.payroll_time_complete where total_hours>0 AND (payroll_code='{1}' AND bank_category='{2}' AND payroll_date='{3}');", location, payroll_code, bank_category, payroll_date, job_title))

                While reader.Read()
                    payrollTimes.Add(New Model.PayrollTime(reader))
                End While
            End Using

            If completeDetail Then
                payrollTimes.ForEach(Sub(item As Model.PayrollTime) Controller.PayrollTime_.CompleteDetail(databaseManager, item))
            End If

            Return payrollTimes
        End Function

        Public Shared Sub Save(databaseManager As utility_service.Manager.Mysql, payrollTime As Model.PayrollTime)
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



    End Class
End Namespace
