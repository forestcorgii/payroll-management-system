Imports MySql.Data.MySqlClient

Namespace Gateway
    Public Class DownloadLog
        Public Shared Function GetId(databaseManager As utility_service.Manager.Mysql, payrollDate As Date) As Integer
            Dim Id As Integer = 0
            Using reader As MySqlDataReader = databaseManager.ExecuteDataReader(String.Format("SELECT id FROM payroll_management.download_log WHERE payroll_date='{0}' ORDER BY log_created DESC LIMIT 1;", payrollDate.ToString("yyyy-MM-dd")))
                If reader.HasRows Then
                    reader.Read()
                    Id = reader.Item("id")
                End If
            End Using
            Return Id
        End Function
        Public Shared Sub SaveDownloadLog(databaseManager As utility_service.Manager.Mysql, downloadLog As Model.DownloadLog)
            Dim command As New MySqlCommand("INSERT INTO payroll_management.download_log (payroll_date,total_page,last_page_downloaded,status)VALUES(?,?,?,?);", databaseManager.Connection)
            command.Parameters.AddWithValue("p1", downloadLog.Payroll_Date)
            command.Parameters.AddWithValue("p2", downloadLog.TotalPage)
            command.Parameters.AddWithValue("p3", downloadLog.Last_Page_Downloaded)
            command.Parameters.AddWithValue("p4", downloadLog.Status)

            command.ExecuteNonQuery()
        End Sub
        Public Shared Sub UpdateStatus(databaseManager As utility_service.Manager.Mysql, downloadLog As Model.DownloadLog)
            Dim command As New MySqlCommand("UPDATE payroll_management.download_log SET last_page_downloaded=?, status=? WHERE id=?;", databaseManager.Connection)
            command.Parameters.AddWithValue("p1", downloadLog.Last_Page_Downloaded)
            command.Parameters.AddWithValue("p2", downloadLog.Status)
            command.Parameters.AddWithValue("p3", downloadLog.id)

            command.ExecuteNonQuery()
        End Sub
    End Class

End Namespace
