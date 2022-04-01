Namespace Payroll

    Namespace Adjustment
        Namespace Log
            Public Class Model
                Public ReadOnly Property Log_Name As String
                    Get
                        Return String.Format("{0}_{1}", Name, Payroll_Name)
                    End Get
                End Property

                Public Record_Id As Integer
                Public Record As Record.Model

                Public Name As String
                Public ee_id As String
                Public Payroll_Name As String

                Public Amount As Double

                Public Adjust_Type As AdjustmentChoices

                Public Date_Created As Date

            End Class

        End Namespace

    End Namespace

End Namespace
