Namespace Payroll

    Namespace Adjustment
        Namespace Record
            Public Class Model
                Public Id As Integer
                Public Name As String
                Public ee_id As String

                Public Adjustment_Id As Integer
                Public Adjustment As Adjustment.Model

                Public Date_Expired As Date
                Public Amount As Double
                Public Remaining_Balance As Double

                Public Adjust_Type As AdjustmentChoices
            End Class

        End Namespace

    End Namespace

End Namespace
