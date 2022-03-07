Imports MySql.Data.MySqlClient

Namespace Model
    Public Class Payroll
        Public Id As Integer

        Public Payroll_Date As Date

        Public EE_Id As Integer
        Public EE As Employee

        Public Gross_Pay As Double
        Public Net_Pay As Double

        Public ReadOnly Property Payroll_Name As String
            Get
                Return String.Format("{0}_{1:yyyyMMdd}", EE.Employee_Id, Payroll_Date)
            End Get
        End Property


        Public Government As Government

        Public Adjustments As List(Of AdjustmentLog)
        Public ReadOnly Property Adjust1 As Double
            Get
                Dim adj As Double = 0
                For Each adjustment In Adjustments
                    If adjustment.Adjust_Type = AdjustTypeChoices.ADJUST1 Then
                        adj += adjustment.Amount
                    End If
                Next
                Return adj
            End Get
        End Property
        Public ReadOnly Property Adjust2 As Double
            Get
                Dim adj As Double = 0
                For Each adjustment In Adjustments
                    If adjustment.Adjust_Type = AdjustTypeChoices.ADJUST2 Then
                        adj += adjustment.Amount
                    End If
                Next
                Return adj
            End Get
        End Property

        Sub New()

        End Sub
        Sub New(reader As MySqlDataReader)
        End Sub


    End Class

End Namespace
