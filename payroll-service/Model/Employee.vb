Imports MySql.Data.MySqlClient

Namespace Model
    Public Class Employee

        Public Id As Integer
        Public Employee_Id As String
        Public First_Name As String
        Public Last_Name As String
        Public Middle_Name As String

        Public Card_Number As String
        Public Account_Number As String
        Public Payroll_Code As String
        Public Bank_Category As String

        Public TIN As String
        Public Date_Modified As Date

        Public ReadOnly Property Fullname As String
            Get
                Return ""
            End Get
        End Property

        Sub New()

        End Sub

        Sub New(reader As MySqlDataReader)
            Id = reader.Item("id")
            Employee_Id = reader.Item("employee_id")
            First_Name = reader.Item("first_name")
            Last_Name = reader.Item("last_name")
            Middle_Name = reader.Item("middle_name")
            Account_Number = reader.Item("account_number")
            Card_Number = reader.Item("card_number")
            TIN = reader.Item("tin")
            Date_Modified = reader.Item("date_modified")
        End Sub
    End Class

End Namespace
