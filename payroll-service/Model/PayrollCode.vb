Namespace Model
    Public Class PayrollCode
        Public EE_Id As Integer
        Public Payroll_code As String
        Public Date_Created As Date
    End Class

    Public Class PayrollCodeHistory
        Public EE_Id As Integer
        Public History As List(Of PayrollCode)
    End Class
End Namespace