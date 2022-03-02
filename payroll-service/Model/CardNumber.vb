Namespace Model
    Public Class CardNumber
        Public EE_Id As Integer
        Public Card_Number As String
        Public Date_Created As Date
    End Class

    Public Class CardNumberHistory
        Public EE_Id As Integer
        Public History As List(Of CardNumber)
    End Class
End Namespace