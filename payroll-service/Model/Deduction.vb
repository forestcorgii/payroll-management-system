Namespace Model
    Public Class DeductionTitle
        Public Id As Integer
        Public Title As String
    End Class

    Public Class DeductionItem
        Public Title As String
        Public Amount As Double
        Public Adjust_Type As AdjustTypeChoices

        Public Last_Deduction As Date
    End Class

    Public Class DeductionList
        Public Adjust2_Total As Double
        Public Adjust1_Total As Double

        Public Adjust1 As List(Of DeductionItem)
        Public Adjust2 As List(Of DeductionItem)
    End Class



    Public Enum AdjustTypeChoices
        ADJUST1
        ADJUST2
    End Enum
End Namespace

