Namespace Model



    Public Class Adjustment
        Public Id As Integer
        Public Name As String
        Public ee_id As String
        Public Payroll_Name As String

        Public Date_Expired As Date
        Public Amount As Double
        Public Remaining_Balance As Double

        Public Adjust_Type As AdjustTypeChoices
    End Class

    Public Class AdjustmentLog
        Public ReadOnly Property Log_Name As String
            Get
                Return String.Format("{0}_{1}", Name, Payroll_Name)
            End Get
        End Property
        Public Name As String
        Public ee_id As String
        Public Payroll_Name As String

        Public Amount As Double

        Public Adjust_Type As AdjustTypeChoices

        Public Date_Created As Date
    End Class


    Public Enum AdjustTypeChoices
        ADJUST1 = 1
        ADJUST2 = 2
    End Enum
End Namespace

