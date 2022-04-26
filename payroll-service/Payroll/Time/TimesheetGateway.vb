Imports System.IO
Imports System.Windows.Forms
Imports MySql.Data.MySqlClient


Namespace Payroll
    Public Class TimesheetGateway
        Public Shared Function Find(databaseManager As utility_service.Manager.Mysql, payroll_name As String, Optional completeDetail As Boolean = False) As TimesheetModel
            Dim payrollTime As TimesheetModel = Nothing
            Using reader As MySqlDataReader = databaseManager.ExecuteDataReader(
            String.Format("SELECT * FROM payroll_db.payroll_time_complete where payroll_name='{0}' LIMIT 1;", payroll_name))
                If reader.HasRows Then
                    reader.Read()
                    payrollTime = New TimesheetModel(reader)
                End If
            End Using

            If payrollTime IsNot Nothing AndAlso completeDetail Then
                TimesheetController.CompleteDetail(databaseManager, payrollTime)
            End If
            Return payrollTime
        End Function

        Public Shared Function Collect(databaseManager As utility_service.Manager.Mysql, Optional location As String = "", Optional payroll_code As String = "", Optional bank_category As String = "", Optional payroll_date As String = "", Optional is_confirmed As Boolean = Nothing, Optional completeDetail As Boolean = False) As List(Of TimesheetModel)
            Dim payrollTimes As New List(Of TimesheetModel)
            Dim is_confirmed_snip As String = ""
            If is_confirmed Then
                is_confirmed_snip = String.Format("AND is_confirmed={0}", is_confirmed)
            End If
            Using reader As MySqlDataReader = databaseManager.ExecuteDataReader(
            String.Format("SELECT * FROM payroll_db.payroll_time_complete where total_hours>0 {4} AND (payroll_code='{1}' AND bank_category='{2}' AND payroll_date='{3}') ORDER BY last_name;", location, payroll_code, bank_category, payroll_date, is_confirmed_snip))
                While reader.Read()
                    payrollTimes.Add(New TimesheetModel(reader))
                End While
            End Using

            If completeDetail Then
                payrollTimes.ForEach(Sub(item As TimesheetModel) TimesheetController.CompleteDetail(databaseManager, item))
            End If

            Return payrollTimes
        End Function

        Public Shared Sub Save(databaseManager As utility_service.Manager.Mysql, payrollTime As time_module.Model.PayrollTime)
            'check if employee exists in the database
            Try
                Dim Command As New MySqlCommand("REPLACE INTO payroll_db.time (ee_id,total_hours,total_ots,total_rd_ot,total_h_ot,total_nd,total_tardy,has_pcv,payroll_date,payroll_name,is_confirmed)VALUES(?,?,?,?,?,?,?,?,?,?,?)", databaseManager.Connection)
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
                Command.Parameters.AddWithValue("p12a", payrollTime.Is_Confirmed)

                Command.ExecuteNonQuery()
            Catch ex As Exception
                MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub



    End Class
End Namespace
