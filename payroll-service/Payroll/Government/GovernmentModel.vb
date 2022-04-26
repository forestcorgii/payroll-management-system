Imports MySql.Data.MySqlClient
Imports employee_module
Namespace Payroll

    Public Class GovernmentModel
        Public Payroll_Name As String

        Public EE_Id As String
        Public EE As EmployeeModel

        Public Property Monthly_Gross_Pay As Double
        Public Property Monthly_Reg_Pay As Double
        Public Property Monthly_Net_Pay As Double

        Public Property Pagibig_EE As Double
        Public Property Pagibig_ER As Double
        Public Property SSS_EE As Double
        Public Property SSS_ER As Double
        Public Property PhilHealth As Double

        Public Property Withholding_Tax As Double

        Public ReadOnly Property TotalDeduction As Double
            Get
                Return Pagibig_EE + SSS_EE + PhilHealth + Withholding_Tax
            End Get
        End Property

        Sub New()

        End Sub

        Sub New(reader As MySqlDataReader)
            Payroll_Name = reader("payroll_name")
            EE_Id = reader("ee_id")
            Monthly_Gross_Pay = reader("monthly_gross_pay")
            Monthly_Reg_Pay = reader("monthly_reg_pay")
            'Monthly_Net_Pay = reader("monthly_net_pay")
            Pagibig_EE = reader("pagibig_ee")
            Pagibig_ER = reader("pagibig_er")
            SSS_EE = reader("sss_ee")
            SSS_ER = reader("sss_er")
            PhilHealth = reader("philhealth")
            Withholding_Tax = reader("tax")
        End Sub

    End Class

End Namespace
