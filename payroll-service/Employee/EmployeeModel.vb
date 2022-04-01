Imports MySql.Data.MySqlClient

Namespace Employee
    Public Class EmployeeModel

        Public Property EE_Id As String = ""
        Public Property First_Name As String = ""
        Public Property Last_Name As String = ""
        Public Property Middle_Name As String = ""
        Public Property Location As String = ""
        Public Property TIN As String = ""

        Public Property Card_Number As String = ""
        Public Property Account_Number As String = ""
        Public Property Payroll_Code As String = ""
        Public Property Bank_Category As String = ""
        Public Property Bank_Name As String = ""

        Public Date_Modified As Date

        Public ReadOnly Property Fullname As String
            Get
                Return String.Format("{0},{1} {2}", Last_Name, First_Name, IIf(Middle_Name <> "", Middle_Name(0) & ".", ""))
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
