Imports NPOI.SS.UserModel
Imports payroll_service.Util
Namespace Model
    Namespace KS

        Public Class PayrollInfo
            Inherits payroll_service.PayrollInfo
            Implements IPayrollInfo

            Sub New()

            End Sub

            ''' <summary>
            ''' Computes Government deductions like Pagibig, SSS, Philhealth, and W.TAX.
            ''' </summary>
            ''' <param name="payroll15th_GROSS_PAY"></param>
            Public Sub Compute30thPayroll(payroll15th_GROSS_PAY As Double) Implements IPayrollInfo.Compute30thPayroll
                ComputeAll(GROSS_PAY + payroll15th_GROSS_PAY)
            End Sub
            Public Sub PastePayslip(nSheet As ISheet, pageIdx As Integer, payslipPosition As PayslipPositionChoices) Implements IPayrollInfo.PastePayslip
                Dim startRowIdx As Integer = 67 * pageIdx
                Try
                    If payslipPosition = PayslipPositionChoices.LOWER_RIGHT Or payslipPosition = PayslipPositionChoices.LOWER_LEFT Then
                        startRowIdx += 31
                    End If

                    Dim startColIdx As Integer = 0
                    If payslipPosition = PayslipPositionChoices.LOWER_RIGHT Or payslipPosition = PayslipPositionChoices.UPPER_RIGHT Then
                        startColIdx = 5
                    End If

                    nSheet.GetRow(startRowIdx + 3).CreateCell(startColIdx + 1).SetCellValue(Employee_Id)

                    nSheet.GetRow(startRowIdx + 3).CreateCell(startColIdx + 2).SetCellValue(PayrollDate)

                    nSheet.GetRow(startRowIdx + 5).CreateCell(startColIdx + 1).SetCellValue(Fullname)

                    nSheet.GetRow(startRowIdx + 7).CreateCell(startColIdx + 4).SetCellValue(GROSS_PAY)
                    nSheet.GetRow(startRowIdx + 15).CreateCell(startColIdx + 4).SetCellValue(ADJUST1)

                    nSheet.GetRow(startRowIdx + 17).CreateCell(startColIdx + 4).SetCellValue(GROSS_PAY)
                    nSheet.GetRow(startRowIdx + 18).CreateCell(startColIdx + 4).SetCellValue(ADJUST2)
                    nSheet.GetRow(startRowIdx + 19).CreateCell(startColIdx + 4).SetCellValue(-WITHHOLDING_TAX)
                    nSheet.GetRow(startRowIdx + 20).CreateCell(startColIdx + 4).SetCellValue(-SSS_PHIC_EE)

                    nSheet.GetRow(startRowIdx + 22).CreateCell(startColIdx + 4).SetCellValue(NET_PAY)

                    nSheet.GetRow(startRowIdx + 27).CreateCell(startColIdx + 1).SetCellValue(Employee_Id)
                    nSheet.GetRow(startRowIdx + 28).CreateCell(startColIdx + 1).SetCellValue(Fullname)

                    nSheet.GetRow(startRowIdx + 28).CreateCell(startColIdx + 4).SetCellValue(NET_PAY)
                Catch ex As Exception
                    '    MessageBox.Show(ex.Message, "Error pasting Payslip.", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Console.WriteLine(ex.Message)
                End Try
            End Sub

            ''' <summary>
            ''' Used in Government Computation, DBF Editor, and Payslip.
            ''' </summary>
            ''' <param name="row"></param>
            ''' <returns></returns>
            Public Function GetPayrollDetail(row As IRow) Implements IPayrollInfo.GetPayrollDetail
                Dim fullname_raw As String() = row.GetCell(1).StringCellValue.Trim(")").Split("(")
                If fullname_raw.Length < 2 Then Return Nothing
                Fullname = fullname_raw(0).Trim
                Employee_Id = fullname_raw(1).Trim

                Dim adjustIdx As Integer = 0 'adjust idx for 15th payroll
                If PayrollDate.Substring(0, 2) = "15" Then
                    adjustIdx = 1
                End If

                GROSS_PAY = row.GetCell(5).NumericCellValue
                SSS_EE = row.GetCell(9).NumericCellValue
                SSS_ER = row.GetCell(10).NumericCellValue

                Pagibig_EE = row.GetCell(7).NumericCellValue
                Pagibig_ER = row.GetCell(8).NumericCellValue

                PHIC = row.GetCell(11).NumericCellValue
                PHIC = row.GetCell(11 + adjustIdx).NumericCellValue
                WITHHOLDING_TAX = row.GetCell(12 + adjustIdx).NumericCellValue

                '            If PayrollDate.Substring(0, 2) = "15" Then
                ADJUST1 = row.GetCell(13 + adjustIdx).NumericCellValue
                ADJUST2 = row.GetCell(14 + adjustIdx).NumericCellValue
                'Else 'adj1 will be on adj1
                '    ADJUST1 = Pagibig_EE
                '    ADJUST2 = row.GetCell(13).NumericCellValue + row.GetCell(14).NumericCellValue
                'End If

                NET_PAY = GROSS_PAY - (SSS_EE + PHIC + WITHHOLDING_TAX + ADJUST1 + ADJUST2)

                Return Me
            End Function

            ''' <summary>
            ''' 
            ''' </summary>
            ''' <param name="row"></param>
            ''' <returns></returns>
            Public Function WriteGovernment(row As IRow) As Object Implements IPayrollInfo.WriteGovernment
                row.GetCell(7).SetCellValue(Pagibig_EE) 'EE
                row.GetCell(8).SetCellValue(Pagibig_EE) 'ER
                row.GetCell(9).SetCellValue(SSS_EE) 'EE
                row.GetCell(10).SetCellValue(SSS_ER) 'ER

                Dim adjustIdx As Integer = 0 'adjust idx for 15th payroll
                If PayrollDate.Substring(0, 2) = "15" Then adjustIdx = 1

                If PayrollDate.Substring(0, 2) = "15" Then
                    row.GetCell(11).SetCellValue(SSS_PHIC_EE) 'SSS+Phic EE
                    row.GetCell(12).SetCellValue(SSS_PHIC_ER) 'SSS+Phic EE
                    row.GetCell(13).SetCellValue(WITHHOLDING_TAX) 'SSS+Phic EE
                Else
                    row.GetCell(11).SetCellValue(PHIC) 'SSS+Phic EE
                    row.GetCell(12).SetCellValue(WITHHOLDING_TAX)
                    row.GetCell(13).SetCellValue(Pagibig_EE) 'ADJUST 1
                End If

                Dim net As Double = GROSS_PAY - (SSS_EE + PHIC + WITHHOLDING_TAX) + (ADJUST1 + ADJUST2)
                row.GetCell(15 + adjustIdx).SetCellValue(net)

                Return row
            End Function

            Public Sub ComputeAll(monthlyGross As Double)
                Dim pagibig As Double() = ComputePagIbig(monthlyGross)
                Pagibig_EE = pagibig(0)
                Pagibig_ER = pagibig(1)
                Dim sss As Double() = ComputeSSS(monthlyGross)
                SSS_EE = sss(1)
                SSS_ER = sss(0)

                PHIC = ComputePhic(monthlyGross)
                PHIC = PHIC

                WITHHOLDING_TAX = ComputeWithholding(monthlyGross - (PHIC + SSS_EE + Pagibig_EE))
            End Sub

        End Class


    End Namespace
End Namespace

