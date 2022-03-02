Namespace Model
    Public Class AccountNumber
        Public EE_Id As Integer
        Public Account_Number As String
        Public Date_Created As Date
    End Class

    Public Class AccountNumberHistory
        Public EE_Id As Integer
        Public History As List(Of AccountNumber)
    End Class
End Namespace