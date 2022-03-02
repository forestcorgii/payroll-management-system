Imports NPOI.SS.UserModel

Namespace Model

    Namespace Perday
        'Public Shared Sub WriteGovernment(row As IRow)
        '    Dim employee_id As String = Trim(row.GetCell(1).StringCellValue)
        '    Dim employee_name As String = Trim(row.GetCell(2).StringCellValue)
        '    Dim gross As Double = row.GetCell(13).NumericCellValue
        '    Dim sss_comp As Double() = ComputeSSS(gross)
        '    Dim phic As Double = ComputePhic(gross)

        '    row.GetCell(16).SetCellValue(sss_comp(0)) 'ER
        '    row.GetCell(17).SetCellValue(sss_comp(1)) 'EE
        '    row.GetCell(18).SetCellValue(phic) 'PhilHealth
        'End Sub

        'Public Shared Function FillPayrollDetail(ByRef newPayslip As PayslipInfo, row As IRow)
        '    With newPayslip
        '        .Employee_Id = row.GetCell(1).StringCellValue
        '        .Fullname = row.GetCell(2).StringCellValue
        '        .REG_HRS = row.GetCell(3).NumericCellValue
        '        .R_OT = row.GetCell(4).NumericCellValue
        '        .RD_OT = row.GetCell(5).NumericCellValue
        '        .RD_8 = row.GetCell(6).NumericCellValue
        '        .HOL_OT = row.GetCell(7).NumericCellValue
        '        .HOL_OT8 = row.GetCell(8).NumericCellValue
        '        .ND = row.GetCell(9).NumericCellValue
        '        .ABS_TAR = row.GetCell(10).NumericCellValue
        '        .REG_PAY = row.GetCell(11).NumericCellValue
        '        .ADJUST1 = row.GetCell(12).NumericCellValue
        '        .GROSS_PAY = row.GetCell(13).NumericCellValue
        '        .ADJUST2 = row.GetCell(14).NumericCellValue
        '        .WITHHOLDING_TAX = row.GetCell(15).NumericCellValue
        '        .SSS_EE = row.GetCell(16).NumericCellValue
        '        .PHIC = row.GetCell(18).NumericCellValue
        '        .NET_PAY = row.GetCell(19).NumericCellValue

        '        'If .PayrollDate.Substring(0, 2) = "15" Then 'pagibig contri will be on adj1 and adj1 will be added on adj2
        '        '    .ADJUST1 = row.GetCell(13 + adjustIdx).NumericCellValue
        '        '    .ADJUST2 = row.GetCell(14 + adjustIdx).NumericCellValue
        '        'Else 'adj1 will be on adj1
        '        '    .ADJUST1 = row.GetCell(7).NumericCellValue
        '        '    .ADJUST2 = row.GetCell(13).NumericCellValue + row.GetCell(14).NumericCellValue
        '        'End If

        '    End With
        '    Return newPayslip
        'End Function

        Public Class PayrollInfo
            Inherits payroll_service.PayrollInfo
            Implements IPayrollInfo

            Sub New()

            End Sub
            Sub New(_pA As PayrollInfo, _pB As PayrollInfo)
                GROSS_PAY = _pA.GROSS_PAY + _pB.GROSS_PAY
                Employee_Id = _pA.Employee_Id
                Fullname = _pA.Fullname
                PayrollDate = _pB.PayrollDate
                ComputeAll()
            End Sub

            Public Sub PastePayslip(nSheet As ISheet, pageIdx As Integer, payslipPosition As Util.PayslipPositionChoices) Implements IPayrollInfo.PastePayslip
                Throw New NotImplementedException()
            End Sub

            Public Function GetPayrollDetail(row As IRow) As Object Implements IPayrollInfo.GetPayrollDetail

                Employee_Id = row.GetCell(1).StringCellValue
                Fullname = row.GetCell(2).StringCellValue
                REG_HRS = row.GetCell(3).NumericCellValue
                ADJUST1 = row.GetCell(12).NumericCellValue
                GROSS_PAY = row.GetCell(13).NumericCellValue
                ADJUST2 = row.GetCell(14).NumericCellValue
                WITHHOLDING_TAX = row.GetCell(15).NumericCellValue
                SSS_EE = row.GetCell(16).NumericCellValue
                PHIC = row.GetCell(18).NumericCellValue
                NET_PAY = row.GetCell(19).NumericCellValue

                'If .PayrollDate.Substring(0, 2) = "15" Then 'pagibig contri will be on adj1 and adj1 will be added on adj2
                '    .ADJUST1 = row.GetCell(13 + adjustIdx).NumericCellValue
                '    .ADJUST2 = row.GetCell(14 + adjustIdx).NumericCellValue
                'Else 'adj1 will be on adj1
                '    .ADJUST1 = row.GetCell(7).NumericCellValue
                '    .ADJUST2 = row.GetCell(13).NumericCellValue + row.GetCell(14).NumericCellValue
                'End If

                Return Me
            End Function

            Public Function WriteGovernment(row As IRow) As Object Implements IPayrollInfo.WriteGovernment
                row.GetCell(16).SetCellValue(SSS_ER) 'ER
                row.GetCell(17).SetCellValue(SSS_EE) 'EE
                row.GetCell(18).SetCellValue(PHIC) 'PhilHealth
            End Function

            Public Sub ComputeAll()
                Dim pagibig As Double() = ComputePagIbig(GROSS_PAY)
                Pagibig_ER = pagibig(0)
                Pagibig_EE = pagibig(1)
                Dim sss As Double() = ComputeSSS(GROSS_PAY)
                SSS_ER = sss(0)
                SSS_EE = sss(1)

                WITHHOLDING_TAX = ComputeWithholding(GROSS_PAY)
                PHIC = ComputePhic(GROSS_PAY)
            End Sub

            Public Sub Compute30thPayroll(Payroll15th_GROSS_PAY As Double) Implements IPayrollInfo.Compute30thPayroll
                Throw New NotImplementedException()
            End Sub
        End Class

    End Namespace
End Namespace
