Imports System.Windows.Forms
Imports MySql.Data.MySqlClient
Imports utility_service

Namespace CardNumber
    Public Class Gateway
        Public Shared Sub Save(databaseManager As Manager.Mysql, cardNumber As String, ee_id As String)
            Try
                Dim command As New MySqlCommand("INSERT INTO payroll_management.card_number (ee_id, card_number)VALUES(?,?);", databaseManager.Connection)
                command.Parameters.AddWithValue("p1", ee_id)
                command.Parameters.AddWithValue("p2", cardNumber)
                command.ExecuteNonQuery()
            Catch ex As Exception
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        'Public Shared Function GetHistory(databaseManager As Manager.Mysql, ee_id As String) As Model.CardNumberHistory
        '    Dim cardNumberHistory As New Model.CardNumberHistory
        '    Try
        '        Using reader As MySqlDataReader = databaseManager.ExecuteDataReader(String.Format("SELECT * FROM payroll_management.card_number WHERE ee_id={0} ORDER BY date_created DESC", ee_id))
        '            While reader.Read
        '                cardNumberHistory.History.Add(New Model.CardNumber(reader))
        '            End While
        '        End Using
        '    Catch ex As Exception
        '        MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    End Try
        '    Return cardNumberHistory
        'End Function
    End Class

End Namespace