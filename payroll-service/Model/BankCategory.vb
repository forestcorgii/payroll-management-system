Imports MySql.Data.MySqlClient

Namespace Model
    Public Class BankCategory
        Public EE_Id As String
        Public Bank_Category As String
        Public Date_Created As Date

        Sub New()

        End Sub
        Sub New(reader As MySqlDataReader)
            EE_Id = reader.Item("ee_id")
            Date_Created = reader.Item("date_created")

            Bank_Category = reader.Item("bank_category")
        End Sub
    End Class

    Public Class BankCategoryHistory
        Public EE_Id As Integer
        Public History As List(Of BankCategory)
    End Class
End Namespace