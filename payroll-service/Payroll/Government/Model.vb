Imports MySql.Data.MySqlClient

Namespace Payroll

    Namespace Government
        Public Class Model
            Public Payroll_Name As String

            Public EE_Id As String
            Public EE As Employee.Model

            Public Monthly_Gross_Pay As Double
            Public Monthly_Reg_Pay As Double
            Public Monthly_Net_Pay As Double

            Public Pagibig_EE As Double
            Public Pagibig_ER As Double
            Public SSS_EE As Double
            Public SSS_ER As Double
            Public PhilHealth As Double

            Public Withholding_Tax As Double

            Sub New()

            End Sub

            Sub New(reader As MySqlDataReader)
                Payroll_Name = reader("payroll_name")
                EE_Id = reader("ee_id")
                Monthly_Gross_Pay = reader("monthly_gross_pay")
                Monthly_Reg_Pay = reader("monthly_reg_pay")
                Monthly_Net_Pay = reader("monthly_net_pay")
                Pagibig_EE = reader("pagibig_ee")
                Pagibig_ER = reader("pagibig_er")
                SSS_EE = reader("sss_ee")
                SSS_ER = reader("sss_er")
                PhilHealth = reader("philhealth")
                Withholding_Tax = reader("tax")
            End Sub

        End Class

    End Namespace
End Namespace
