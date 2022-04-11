Imports MySql.Data.MySqlClient
Imports utility_service

Namespace Payroll
    Public Class AdjustmentBillingGateway

        Public Shared Sub SaveAdjustmentLog(databaseManager As Manager.Mysql, adjustmentLog As AdjustmentBillingModel)
            Try
                Dim command As New MySqlCommand("REPLACE INTO payroll_db.adjustment_billing (log_name,name,ee_id,payroll_name,amount)VALUES(?,?,?,?,?)", databaseManager.Connection)
                command.Parameters.AddWithValue("p1", adjustmentLog.Log_Name)
                command.Parameters.AddWithValue("p1b", adjustmentLog.Name)
                command.Parameters.AddWithValue("p2", adjustmentLog.ee_id)
                command.Parameters.AddWithValue("p3", adjustmentLog.Payroll_Name)
                command.Parameters.AddWithValue("p4", adjustmentLog.Amount)
                command.ExecuteNonQuery()
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try
        End Sub
    End Class


End Namespace
