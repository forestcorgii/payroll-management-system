Namespace Adjustment
    Public Class Model
        Public Id As Integer
        Public Name As String


        Public Adjust_Type As AdjustmentChoices
        Public Duration As DurationChoices
    End Class

    Public Enum DurationChoices
        CONTINUOUS
        HAS_BALANCES

    End Enum
    Public Enum AdjustmentChoices
        ADJUST1 = 1
        ADJUST2 = 2
    End Enum
End Namespace

