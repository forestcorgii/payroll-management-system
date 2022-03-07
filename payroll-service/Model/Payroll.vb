﻿Imports MySql.Data.MySqlClient

Namespace Model
    Public Class Payroll
        Public Id As Integer

        Public Payroll_Date As Date

        Public EE_Id As Integer
        Public EE As Employee

        Public Gross_Pay As Double
        Public Regular_Pay As Double
        Public Net_Pay As Double

        Public Adjust1 As Double
        Public Adjust2 As Double
        Public Withholding_Tax As Double

        Public ReadOnly Property Payroll_Name As String
            Get
                Return String.Format("{0}_{1:yyyyMMdd}", EE.Employee_Id, Payroll_Date)
            End Get
        End Property

        Sub New()

        End Sub
        Sub New(reader As MySqlDataReader)
        End Sub


    End Class

End Namespace
