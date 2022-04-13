Imports System.Windows.Forms
Imports MySql.Data.MySqlClient
Imports utility_service
Namespace Payroll

    Namespace Government

        Public Class Gateway

            Public Shared Function Compute(government As Government.model) As Government.model

                Dim monthlyGross As Double = government.Monthly_Gross_Pay

                government.Pagibig_EE = monthlyGross * 0.02
                If government.Pagibig_EE >= 21 And government.Pagibig_EE < 100 Then
                    government.Pagibig_ER = government.Pagibig_EE
                ElseIf government.Pagibig_EE < 21 Then
                    government.Pagibig_ER = government.Pagibig_EE * 2
                Else
                    government.Pagibig_ER = 100
                End If

                Dim multiplier As Integer = CInt(((monthlyGross * 2) - 2750) \ 500)

                Dim ER_rsc As Double = Math.Min(255 + (42.5 * multiplier), 1700) 'MIN(255+(42.5*B6);1700)
                Dim EE_rsc As Double = Math.Min(135 + (22.5 * multiplier), 900) '=MIN(135+(22.5*B10);900)

                Dim ER_ec As Double = If(multiplier <= 23, 10, 30) '=IF(B11<23;10;30)
                Dim EE_ec As Double = 0

                Dim multiplier_mpc As Integer = Math.Max(0, multiplier - 34)
                Dim ER_mpf As Double = Math.Min(42.5 * multiplier_mpc, 425) '=MIN(42.5*G10;425)
                Dim EE_mpf As Double = Math.Min(22.5 * multiplier_mpc, 225) '=MIN(22.5*G6;225)
                government.SSS_EE = EE_rsc + EE_ec + EE_mpf
                government.SSS_ER = ER_rsc + ER_ec + ER_mpf

                Select Case monthlyGross
                    Case >= 70000
                        government.PhilHealth = 1800
                    Case >= 10000.01
                        government.PhilHealth = monthlyGross * 0.03
                    Case <= 10000
                        government.PhilHealth = 300
                End Select

                Dim taxable_pay As Double = monthlyGross - (government.PhilHealth + government.SSS_EE + government.Pagibig_EE)
                Select Case taxable_pay
                    Case >= 666667
                        government.Withholding_Tax = 200833.33 + ((taxable_pay - 666667) * 0.35)
                    Case >= 166667
                        government.Withholding_Tax = 40833.33 + ((taxable_pay - 166667) * 0.32)
                    Case >= 66667
                        government.Withholding_Tax = 10833.33 + ((taxable_pay - 66667) * 0.3)
                    Case >= 33333
                        government.Withholding_Tax = 2500 + ((taxable_pay - 33333) * 0.25)
                    Case >= 20833.01
                        government.Withholding_Tax = 0 + ((taxable_pay - 20833.01) * 0.2)
                    Case <= 20833
                        government.Withholding_Tax = 0
                End Select

                Return government
            End Function

            Public Shared Function Find(databaseManager As Manager.Mysql, payroll_name As String) As Government.model
                Dim government As Government.model = Nothing
                Using reader As MySqlDataReader = databaseManager.ExecuteDataReader(
                    String.Format("SELECT * FROM payroll_db.government where payroll_name='{1}' LIMIT 1;", payroll_name))
                    If reader.HasRows Then
                        reader.Read()
                        government = New Government.Model(reader)
                    End If
                End Using

                Return government
            End Function
            Public Shared Sub Save(databaseManager As Manager.Mysql, government As Government.model)
                Try
                    Dim command As New MySqlCommand("REPLACE INTO payroll_db.government (ee_id,monthly_gross_pay,monthly_reg_pay,sss_ee,sss_er,pagibig_ee,pagibig_er,philhealth,tax,payroll_name)VALUES(?,?,?,?,?,?,?,?,?,?)", databaseManager.Connection)
                    command.Parameters.AddWithValue("p1", government.EE_Id)
                    command.Parameters.AddWithValue("p2", government.Monthly_Gross_Pay)
                    command.Parameters.AddWithValue("reg", government.Monthly_Reg_Pay)
                    command.Parameters.AddWithValue("p4", government.SSS_EE)
                    command.Parameters.AddWithValue("p5", government.SSS_ER)
                    command.Parameters.AddWithValue("p6", government.Pagibig_EE)
                    command.Parameters.AddWithValue("p7", government.Pagibig_ER)
                    command.Parameters.AddWithValue("p8", government.PhilHealth)
                    command.Parameters.AddWithValue("p9", government.Withholding_Tax)
                    command.Parameters.AddWithValue("p10", government.Payroll_Name)
                    command.ExecuteNonQuery()
                Catch ex As Exception
                    MessageBox.Show(ex.Message, "Error Saving Payroll.", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End Sub


        End Class
    End Namespace
End Namespace
