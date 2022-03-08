Imports MySql.Data.MySqlClient

Namespace Model
    Public Class Employee

        Public EE_Id As String
        Public First_Name As String = ""
        Public Last_Name As String = ""
        Public Middle_Name As String = ""
        Public Location As String = ""
        Public TIN As String = ""

        Public Card_Number As String = ""
        Public Account_Number As String = ""
        Public Payroll_Code As String = ""
        Public Bank_Category As String = ""
        Public Bank_Name As String = ""

        Public Date_Modified As Date

        Public ReadOnly Property Fullname As String
            Get
                Return ""
            End Get
        End Property

        Sub New()

        End Sub

        Sub New(reader As MySqlDataReader)
            EE_Id = reader.Item("ee_id")
            First_Name = reader.Item("first_name")
            Last_Name = reader.Item("last_name")
            Middle_Name = reader.Item("middle_name")
            Account_Number = reader.Item("account_number")
            Card_Number = reader.Item("card_number")
            Bank_Category = reader.Item("bank_category")
            Bank_Name = reader.Item("bank_name")
            Payroll_Code = reader.Item("payroll_code")
            Location = reader.Item("location")
            TIN = reader.Item("tin")
            Date_Modified = reader.Item("date_modified")
        End Sub
    End Class

End Namespace
