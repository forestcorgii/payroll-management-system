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


        Sub New()

        End Sub
        Sub New(reader As MySqlDataReader)
        End Sub


    End Class

End Namespace
