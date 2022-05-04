Imports MySql.Data.MySqlClient

Namespace Payroll
    Public Class PayRegisterModel
        Public Property Id As String

        Public Property Payroll_Date As Date
        Public Property Payroll_Code As String
        Public Property Bank_Category As String

        Public Property Full_Path As String

        Public Property Date_Created As Date
        Public Property Date_Modified As Date

        Public Property Total_Gross As Double
        Public Property Total_Net As Double
        Public Property Total_EE As Integer


        Public ReadOnly Property GetId As String
            Get
                Return String.Format("{0}{1}", Payroll_Code, Bank_Category)
            End Get
        End Property

        Sub New()
        End Sub
        Sub New(reader As MySqlDataReader)
            'Id = reader("Id")
            Payroll_Date = reader("payroll_date")
            Payroll_Code = reader("payroll_code")
            Bank_Category = reader("bank_category")

            Total_EE = reader("total_ee")
            Total_Gross = reader("total_gross")
            Total_Net = reader("total_net")
            'Full_Path = reader("full_path")
            'Date_Created = reader("date_created")
            'Date_Modified = reader("date_modified")
        End Sub
    End Class

End Namespace
