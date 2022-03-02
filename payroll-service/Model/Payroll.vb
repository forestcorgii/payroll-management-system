Imports MySql.Data.MySqlClient

Namespace Model
    Public Class Payroll
        Public Id As Integer

        Public Payroll_Date As Date

        Public EE_Id As Integer
        Public EE As Employee

        Public Gross_Pay As Double
        Public Regular_Pay As Double
        Public Net_Pay As Double

        Public Pagibig_EE As Double
        Public Pagibig_ER As Double
        Public SSS_EE As Double
        Public SSS_ER As Double
        Public PhilHealth As Double

        Public Adjust1 As Double
        Public Adjust2 As Double
        Public Withholding_Tax As Double

        Sub New()

        End Sub
        Sub New(reader As MySqlDataReader)
        End Sub


    End Class

End Namespace
