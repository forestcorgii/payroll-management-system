Imports MySql.Data.MySqlClient

Namespace Model
    Public Class AccountNumber
        Public EE_Id As String
        Public Account_Number As String
        Public Date_Created As Date

        Sub New()

        End Sub
        Sub New(reader As MySqlDataReader)
            EE_Id = reader.Item("ee_id")
            Date_Created = reader.Item("date_created")

            Account_Number = reader.Item("account_number")
        End Sub
    End Class

    Public Class AccountNumberHistory
        Public EE_Id As Integer
        Public History As List(Of AccountNumber)
    End Class
End Namespace