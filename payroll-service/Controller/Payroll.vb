Imports System.Windows.Forms
Imports NPOI.SS.UserModel
Imports payroll_service.Util
Imports utility_service

Namespace Controller
    Public Class Payroll



        ''' <summary>
        ''' Used in Government Computation, DBF Editor, and Payslip.
        ''' </summary>
        ''' <param name="row"></param>
        Public Shared Sub GetPayrollDetail(row As IRow, payroll As Model.Payroll, payrollDate As String)
            'Dim fullname_raw As String() = row.GetCell(1).StringCellValue.Trim(")").Split("(")
            'If fullname_raw.Length < 2 Then Return Nothing
            'Fullname = fullname_raw(0).Trim
            'Employee_Id = fullname_raw(1).Trim

            Dim adjustIdx As Integer = 0 'adjust idx for 15th payroll
            If payrollDate.Substring(0, 2) = "15" Then
                adjustIdx = 1
            End If

            payroll.Gross_Pay = row.GetCell(5).NumericCellValue
            payroll.SSS_EE = row.GetCell(9).NumericCellValue
            payroll.SSS_ER = row.GetCell(10).NumericCellValue

            payroll.Pagibig_EE = row.GetCell(7).NumericCellValue
            payroll.Pagibig_ER = row.GetCell(8).NumericCellValue

            payroll.PhilHealth = row.GetCell(11).NumericCellValue
            payroll.PhilHealth = row.GetCell(11 + adjustIdx).NumericCellValue
            payroll.Withholding_Tax = row.GetCell(12 + adjustIdx).NumericCellValue

            payroll.Adjust1 = row.GetCell(13 + adjustIdx).NumericCellValue
            payroll.Adjust2 = row.GetCell(14 + adjustIdx).NumericCellValue

            payroll.Net_Pay = payroll.Gross_Pay - (payroll.SSS_EE + payroll.PhilHealth + payroll.Withholding_Tax + payroll.Adjust1 + payroll.Adjust2)

        End Sub

        Public Shared Sub ComputeGovernment(payroll30th As Model.Payroll, payroll15th_GROSS_PAY As Double, payroll As Model.Payroll)
            Dim monthlyGross As Double = payroll30th.Gross_Pay = payroll15th_GROSS_PAY

            payroll.Pagibig_EE = monthlyGross * 0.02
            If payroll.Pagibig_EE >= 21 And payroll.Pagibig_EE < 100 Then
                payroll.Pagibig_ER = payroll.Pagibig_EE
            ElseIf payroll.Pagibig_EE < 21 Then
                payroll.Pagibig_ER = payroll.Pagibig_EE * 2
            Else
                payroll.Pagibig_ER = 100
            End If



            Dim multiplier As Integer = CInt(((monthlyGross * 2) - 2750) \ 500)

            Dim ER_rsc As Double = Math.Min(255 + (42.5 * multiplier), 1700) 'MIN(255+(42.5*B6);1700)
            Dim EE_rsc As Double = Math.Min(135 + (22.5 * multiplier), 900) '=MIN(135+(22.5*B10);900)

            Dim ER_ec As Double = If(multiplier <= 23, 10, 30) '=IF(B11<23;10;30)
            Dim EE_ec As Double = 0

            Dim multiplier_mpc As Integer = Math.Max(0, multiplier - 34)
            Dim ER_mpf As Double = Math.Min(42.5 * multiplier_mpc, 425) '=MIN(42.5*G10;425)
            Dim EE_mpf As Double = Math.Min(22.5 * multiplier_mpc, 225) '=MIN(22.5*G6;225)
            payroll.SSS_EE = EE_rsc + EE_ec + EE_mpf
            payroll.SSS_ER = ER_rsc + ER_ec + ER_mpf


            Select Case monthlyGross
                Case >= 70000
                    payroll.PhilHealth = 1800
                Case >= 10000.01
                    payroll.PhilHealth = monthlyGross * 0.03
                Case <= 10000
                    payroll.PhilHealth = 300
            End Select


            Dim taxable_pay As Double = monthlyGross - (payroll.PhilHealth + payroll.SSS_EE + payroll.Pagibig_EE)
            Select Case taxable_pay
                Case >= 666667
                    payroll.Withholding_Tax = 200833.33 + ((taxable_pay - 666667) * 0.35)
                Case >= 166667
                    payroll.Withholding_Tax = 40833.33 + ((taxable_pay - 166667) * 0.32)
                Case >= 66667
                    payroll.Withholding_Tax = 10833.33 + ((taxable_pay - 66667) * 0.3)
                Case >= 33333
                    payroll.Withholding_Tax = 2500 + ((taxable_pay - 33333) * 0.25)
                Case >= 20833.01
                    payroll.Withholding_Tax = 0 + ((taxable_pay - 20833.01) * 0.2)
                Case <= 20833
                    payroll.Withholding_Tax = 0
            End Select
        End Sub

    End Class
End Namespace
