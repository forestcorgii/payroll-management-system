Imports System.Windows.Forms
Imports MySql.Data.MySqlClient
Imports utility_service

Namespace Controller
    Public Class Payroll


        Public Shared Sub SavePayroll(databaseManager As Manager.Mysql, payroll As Model.Payroll)
            Try
                Dim command As New MySqlCommand("REPLACE INTO payroll_management.payroll (ee_id,payroll_date,gross_pay,payroll_name)VALUES(?,?,?,?)", databaseManager.Connection)
                command.Parameters.AddWithValue("p1", payroll.EE_Id)
                command.Parameters.AddWithValue("p2", payroll.Payroll_Date)
                command.Parameters.AddWithValue("p3", payroll.Gross_Pay)
                command.Parameters.AddWithValue("p4", payroll.Payroll_Name)
                command.ExecuteNonQuery()
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Error Saving Payroll.", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub
    End Class
End Namespace
