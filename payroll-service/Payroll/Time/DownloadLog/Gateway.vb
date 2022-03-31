Imports MySql.Data.MySqlClient

Namespace Payroll
    Namespace Time

        Namespace DownloadLog
            Public Class Gateway
                Public Shared Function Find(databaseManager As utility_service.Manager.Mysql, payrollDate As Date, payrollCode As String) As Model
                    Dim downloadLoag As Model = Nothing
                    Using reader As MySqlDataReader = databaseManager.ExecuteDataReader(String.Format("SELECT * FROM payroll_management.download_log WHERE payroll_date='{0}' AND payroll_code='{1}' ORDER BY log_created DESC LIMIT 1;", payrollDate.ToString("yyyy-MM-dd"), payrollCode))
                        If reader.HasRows Then
                            reader.Read()
                            downloadLoag = New Model(reader)
                        End If
                    End Using
                    Return downloadLoag
                End Function
                Public Shared Function Save(databaseManager As utility_service.Manager.Mysql, downloadLog As Model) As Model
                    Dim command As New MySqlCommand("INSERT INTO payroll_management.download_log (payroll_date,payroll_code,total_page,last_page_downloaded,status)VALUES(?,?,?,?,?);", databaseManager.Connection)
                    command.Parameters.AddWithValue("p1", downloadLog.Payroll_Date)
                    command.Parameters.AddWithValue("p11", downloadLog.Payroll_Code)
                    command.Parameters.AddWithValue("p2", downloadLog.TotalPage)
                    command.Parameters.AddWithValue("p3", downloadLog.Last_Page_Downloaded)
                    command.Parameters.AddWithValue("p4", downloadLog.Status)

                    command.ExecuteNonQuery()

                    Console.WriteLine("SAVE {0:G}", Now)
                    Return Find(databaseManager, downloadLog.Payroll_Date, downloadLog.Payroll_Code)
                End Function
                Public Shared Sub Update(databaseManager As utility_service.Manager.Mysql, downloadLog As Model)
                    Dim command As New MySqlCommand("UPDATE payroll_management.download_log SET last_page_downloaded=?, status=? WHERE id=?;", databaseManager.Connection)
                    command.Parameters.AddWithValue("p1", downloadLog.Last_Page_Downloaded)
                    command.Parameters.AddWithValue("p2", downloadLog.Status)
                    command.Parameters.AddWithValue("p3", downloadLog.id)

                    command.ExecuteNonQuery()
                End Sub
            End Class

        End Namespace
    End Namespace
End Namespace
