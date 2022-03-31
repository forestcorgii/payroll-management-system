Imports MySql.Data.MySqlClient

Namespace CardNumber
    Public Class Model
        Public EE_Id As String
        Public Card_Number As String
        Public Date_Created As Date

        Sub New()

        End Sub
        Sub New(reader As MySqlDataReader)
            EE_Id = reader.Item("ee_id")
            Date_Created = reader.Item("date_created")

            Card_Number = reader.Item("card_number")
        End Sub
    End Class

    'Public Class CardNumberHistory
    '    Public EE_Id As Integer
    '    Public History As List(Of CardNumber)
    'End Class
End Namespace