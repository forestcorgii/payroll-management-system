Imports System.Windows.Forms
Imports MySql.Data.MySqlClient
Imports utility_service

Namespace Controller
    Public Class BankName
        Public Shared Sub SaveBankName(databaseManager As Manager.Mysql, bankName As String, ee_id As Integer)
            Try
                Dim command As New MySqlCommand("INSERT INTO payroll_management.bank_name (ee_id, bank_name)VALUES(?,?)", databaseManager.Connection)
                command.Parameters.AddWithValue("p1", ee_id)
                command.Parameters.AddWithValue("p2", bankName)
                command.ExecuteNonQuery()
            Catch ex As Exception
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Public Shared Function GetHistory(databaseManager As Manager.Mysql, ee_id As Integer) As Model.BankNameHistory
            Dim bankNameHistory As New Model.BankNameHistory
            Try
                Using reader As MySqlDataReader = databaseManager.ExecuteDataReader(String.Format("SELECT * FROM payroll_management.bank_name WHERE ee_id={0} ORDER BY date_created DESC", ee_id))
                    While reader.Read
                        bankNameHistory.History.Add(New Model.BankName(reader))
                    End While
                End Using
            Catch ex As Exception
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
            Return bankNameHistory
        End Function
    End Class

End Namespace