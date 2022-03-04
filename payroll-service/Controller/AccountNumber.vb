Imports System.Windows.Forms
Imports MySql.Data.MySqlClient
Imports utility_service

Namespace Controller
    Public Class AccountNumber
        Public Shared Sub SaveAccountNumber(databaseManager As Manager.Mysql, accountNumber As String, ee_id As Integer)
            Try
                Dim command As New MySqlCommand("INSERT INTO payroll_management.account_number (ee_id, account_number)VALUES(?,?)", databaseManager.Connection)
                command.Parameters.AddWithValue("p1", ee_id)
                command.Parameters.AddWithValue("p2", accountNumber)
                command.ExecuteNonQuery()
            Catch ex As Exception
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Public Shared Function GetHistory(databaseManager As Manager.Mysql, ee_id As Integer) As Model.AccountNumberHistory
            Dim accountNumberHistory As New Model.AccountNumberHistory
            Try
                Using reader As MySqlDataReader = databaseManager.ExecuteDataReader(String.Format("SELECT * FROM payroll_management.account_number WHERE ee_id={0} ORDER BY date_created DESC", ee_id))
                    While reader.Read
                        accountNumberHistory.History.Add(New Model.AccountNumber(reader))
                    End While
                End Using
            Catch ex As Exception
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
            Return accountNumberHistory
        End Function
    End Class

End Namespace