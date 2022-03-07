Imports MySql.Data.MySqlClient
Imports utility_service

Namespace Controller
    Public Class Adjustment

        Public Shared Sub SaveAdjustmentLog(databaseManager As Manager.Mysql, adjustmentLog As Model.AdjustmentLog)
            Try
                Dim command As New MySqlCommand("INSERT INTO payroll_management.adjustment_log (name,ee_id,payroll_name,amount,adjust_type)VALUES(?,?,?,?,?)", databaseManager.Connection)
                command.Parameters.AddWithValue("p1", adjustmentLog.Name)
                command.Parameters.AddWithValue("p2", adjustmentLog.ee_id)
                command.Parameters.AddWithValue("p3", adjustmentLog.Payroll_Name)
                command.Parameters.AddWithValue("p4", adjustmentLog.Amount)
                command.Parameters.AddWithValue("p5", adjustmentLog.Adjust_Type)
                command.ExecuteNonQuery()
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try
        End Sub


    End Class

End Namespace
