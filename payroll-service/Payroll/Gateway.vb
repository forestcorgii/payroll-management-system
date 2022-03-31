Imports System.Windows.Forms
Imports MySql.Data.MySqlClient
Imports utility_service

Namespace Payroll
    Public Class Gateway

        Public Shared Function Find(databaseManager As Manager.Mysql, ee_id As String, payrollDate As String) As Model
            Dim payroll As Model = Nothing
            Using reader As MySqlDataReader = databaseManager.ExecuteDataReader(
                String.Format("SELECT * FROM payroll_management.payroll where ee_id='{0}' AND payroll_date='{1}' LIMIT 1;", ee_id, payrollDate))
                If reader.HasRows Then
                    reader.Read()
                    payroll = New Model(reader)
                End If
            End Using

            Return payroll
        End Function
        Public Shared Sub Save(databaseManager As Manager.Mysql, payroll As Model)
            Try
                Dim command As New MySqlCommand("REPLACE INTO payroll_management.payroll (ee_id,payroll_date,gross_pay,payroll_name,adjust1,adjust2,net_pay)VALUES(?,?,?,?,?,?,?)", databaseManager.Connection)
                command.Parameters.AddWithValue("p1", payroll.EE_Id)
                command.Parameters.AddWithValue("p2", payroll.Payroll_Date)
                command.Parameters.AddWithValue("p3", payroll.Gross_Pay)
                command.Parameters.AddWithValue("p4", payroll.Payroll_Name)
                command.Parameters.AddWithValue("p5", payroll.Adjust1)
                command.Parameters.AddWithValue("p6", payroll.Adjust2)
                command.Parameters.AddWithValue("p7", payroll.Net_Pay)
                command.ExecuteNonQuery()
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Error Saving Payroll.", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub
    End Class
End Namespace
