Imports MySql.Data.MySqlClient

Namespace Model
    Public Class PayrollCode
        Public EE_Id As String
        Public Payroll_code As String
        Public Date_Created As Date

        Sub New()

        End Sub
        Sub New(reader As MySqlDataReader)
            EE_Id = reader.Item("ee_id")
            Date_Created = reader.Item("date_created")

            Payroll_code = reader.Item("payroll_code")
        End Sub
    End Class

    Public Class PayrollCodeHistory
        Public EE_Id As Integer
        Public History As List(Of PayrollCode)
    End Class
End Namespace