Imports NPOI.SS.UserModel
Imports payroll_service.Util
Imports PayrollCreditSegregation.Util
Namespace Model
    Namespace KS
        'Public Shared Function WriteGovernment(row As IRow, payrollDate As String)
        '    'Dim fullname_raw As String() = row.GetCell(1).StringCellValue.Trim(")").Split("(")
        '    'If fullname_raw.Length < 2 Then Return Nothing

        '    'Dim employee_name As String = fullname_raw(0).Trim
        '    'Dim employee_id As String = fullname_raw(1).Trim

        '    Dim gross As Double = row.GetCell(5).NumericCellValue

        '    Dim sss_comp As Double() = ComputeSSS(gross)
        '    Dim phic As Double = ComputePhic(gross)
        '    Dim pagibig As Double = ComputePagIbig(gross)
        '    Dim withholding_tax As Double = ComputeWithholding(gross)

        '    row.GetCell(7).SetCellValue(pagibig) 'EE
        '    row.GetCell(8).SetCellValue(100) 'ER
        '    row.GetCell(9).SetCellValue(sss_comp(1)) 'EE
        '    row.GetCell(10).SetCellValue(sss_comp(0)) 'ER

        '    Dim adjustIdx As Integer = 0 'adjust idx for 15th payroll
        '    If payrollDate.Substring(0, 2) = "15" Then adjustIdx = 1

        '    Dim adj1 As Double, adj2 As Double
        '    If payrollDate.Substring(0, 2) = "15" Then
        '        row.GetCell(11).SetCellValue(sss_comp(1) + phic) 'SSS+Phic EE
        '        row.GetCell(12).SetCellValue(sss_comp(0) + phic) 'SSS+Phic EE
        '        row.GetCell(13).SetCellValue(withholding_tax) 'SSS+Phic EE

        '        row.GetCell(11).SetCellValue(sss_comp(1) + phic) 'SSS+Phic EE
        '        row.GetCell(12).SetCellValue(sss_comp(0) + phic) 'SSS+Phic ER

        '        adj1 = row.GetCell(13 + adjustIdx).NumericCellValue
        '        adj2 = row.GetCell(14 + adjustIdx).NumericCellValue
        '    Else
        '        adj1 = pagibig
        '        row.GetCell(13).SetCellValue(adj1)

        '        adj2 = row.GetCell(14).NumericCellValue

        '        row.GetCell(11).SetCellValue(phic) 'SSS+Phic EE
        '        row.GetCell(12).SetCellValue(withholding_tax)
        '    End If

        '    Dim net As Double = gross - (sss_comp(1) + phic + withholding_tax + adj1 + adj2)
        '    row.GetCell(15 + adjustIdx).SetCellValue(net)

        '    Return row
        'End Function

        'Public Shared Function FillPayrollDetail(ByRef newPayslip As PayrollInfo, row As IRow)
        '    With newPayslip
        '        Dim fullname_raw As String() = row.GetCell(1).StringCellValue.Trim(")").Split("(")
        '        If fullname_raw.Length < 2 Then Return Nothing
        '        .Fullname = fullname_raw(0).Trim
        '        .Employee_Id = fullname_raw(1).Trim

        '        Dim adjustIdx As Integer = 0 'adjust idx for 15th payroll
        '        If .PayrollDate.Substring(0, 2) = "15" Then adjustIdx = 1

        '        .GROSS_PAY = row.GetCell(5).NumericCellValue
        '        .SSS_EE = row.GetCell(9).NumericCellValue
        '        .SSS_ER = row.GetCell(10).NumericCellValue

        '        .Pagibig_EE = row.GetCell(7).NumericCellValue
        '        .Pagibig_ER = row.GetCell(8).NumericCellValue

        '        .PHIC_EE = row.GetCell(11).NumericCellValue
        '        .PHIC_ER = row.GetCell(11 + adjustIdx).NumericCellValue
        '        .WITHHOLDING_TAX = row.GetCell(12 + adjustIdx).NumericCellValue

        '        If .PayrollDate.Substring(0, 2) = "15" Then 'pagibig contri will be on adj1 and adj1 will be added on adj2
        '            .ADJUST1 = row.GetCell(13 + adjustIdx).NumericCellValue
        '            .ADJUST2 = row.GetCell(14 + adjustIdx).NumericCellValue
        '        Else 'adj1 will be on adj1
        '            .ADJUST1 = .Pagibig_EE
        '            .ADJUST2 = row.GetCell(13).NumericCellValue + row.GetCell(14).NumericCellValue
        '        End If

        '        .NET_PAY = .GROSS_PAY - (.SSS_EE + .PHIC_EE + .WITHHOLDING_TAX + .ADJUST1 + .ADJUST2) 'row.GetCell(15 + adjustIdx).NumericCellValue

        '        Return newPayslip
        '    End With
        'End Function

        'Public Shared Function GetPayrollDate(nSheet As ISheet) As String
        '    If nSheet.GetRow(3) Is Nothing Then
        '        MsgBox("Cannot Find Payroll Date")
        '        Return Nothing
        '    End If
        '    Return nSheet.GetRow(3).GetCell(1).StringCellValue.Trim.Replace("*", "").Trim
        'End Function

        'Public Shared Function GetPayroll(payregPath As String) As List(Of PayrollInfo)
        '    Dim sourceFileInfo As New IO.FileInfo(payregPath)

        '    Dim payrolls As New List(Of PayrollInfo)
        '    Dim nWorkBook_Payreg As IWorkbook
        '    Using nNewPayreg As IO.FileStream = New FileStream(sourceFileInfo.FullName, FileMode.Open, FileAccess.Read)
        '        nWorkBook_Payreg = New HSSFWorkbook(nNewPayreg)
        '    End Using

        '    Dim nSheet As ISheet = nWorkBook_Payreg.GetSheetAt(0)
        '    'Get Payroll Date 

        '    Dim rawPayrollDate As String = GetPayrollDate(nSheet) 'nSheet.GetRow(3).GetCell(1).StringCellValue.Trim.Replace("*", "").Trim

        '    For j As Integer = 4 To nSheet.LastRowNum
        '        Dim nRow As IRow = nSheet.GetRow(j)
        '        If nRow IsNot Nothing AndAlso nRow.GetCell(1) IsNot Nothing Then
        '            Dim newPayslip As New PayrollInfo()
        '            With newPayslip
        '                .PayrollDate = rawPayrollDate
        '                If FillPayrollDetail(newPayslip, nRow) Is Nothing Then Continue For
        '                ' If Perday.FillPayrollDetail(newPayslip, nRow) Is Nothing Then Continue For
        '            End With
        '            payrolls.Add(newPayslip)
        '        End If
        '    Next

        '    Return payrolls
        'End Function


        Public Class PayrollInfo
            Inherits payroll_service.PayrollInfo
            Implements IPayrollInfo

            Sub New()

            End Sub
            'Public Property PayrollDate As String Implements IPayrollInfo.PayrollDate
            'Public Property Employee_Id As String Implements IPayrollInfo.Employee_Id
            'Public Property Fullname As String Implements IPayrollInfo.Fullname
            'Public Property REG_HRS As Double Implements IPayrollInfo.REG_HRS
            'Public Property ADJUST1 As Double Implements IPayrollInfo.ADJUST1
            'Public Property GROSS_PAY As Double Implements IPayrollInfo.GROSS_PAY
            'Public Property ADJUST2 As Double Implements IPayrollInfo.ADJUST2
            'Public Property WITHHOLDING_TAX As Double Implements IPayrollInfo.WITHHOLDING_TAX
            'Public Property Pagibig_EE As Double Implements IPayrollInfo.Pagibig_EE
            'Public Property Pagibig_ER As Double Implements IPayrollInfo.Pagibig_ER
            'Public Property SSS_EE As Double Implements IPayrollInfo.SSS_EE
            'Public Property SSS_ER As Double Implements IPayrollInfo.SSS_ER
            'Public Property PHIC As Double Implements IPayrollInfo.PHIC
            'Public ReadOnly Property SSS_PHIC_EE As Double Implements IPayrollInfo.SSS_PHIC_EE
            '    Get
            '        Return SSS_EE + PHIC
            '    End Get
            'End Property
            'Public ReadOnly Property SSS_PHIC_ER As Double Implements IPayrollInfo.SSS_PHIC_ER
            '    Get
            '        Return SSS_ER + PHIC
            '    End Get
            'End Property

            'Public Property NET_PAY As Double Implements IPayrollInfo.NET_PAY
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

        '    Public Class MonthlyPayroll
        '        Implements PayrollInfo

        '        'Public PayrollA As PayrollInfo
        '        'Public PayrollB As PayrollInfo

        '        Sub New()
        '        End Sub

        '        Sub New(_pA As PayrollInfo, _pB As PayrollInfo)
        '            GROSS_PAY = _pA.GROSS_PAY + _pB.GROSS_PAY
        '            Employee_Id = _pA.Employee_Id
        '            Fullname = _pA.Fullname

        '            ComputeAll()
        '        End Sub
        '#Region "Interface Fields"
        '        Public Property PayrollDate As String Implements PayrollInfo.PayrollDate
        '        Public Property Employee_Id As String Implements PayrollInfo.Employee_Id
        '        Public Property Fullname As String Implements PayrollInfo.Fullname
        '        Public Property REG_HRS As Double Implements PayrollInfo.REG_HRS
        '        Public Property ADJUST1 As Double Implements PayrollInfo.ADJUST1
        '        Public Property GROSS_PAY As Double Implements PayrollInfo.GROSS_PAY
        '        Public Property ADJUST2 As Double Implements PayrollInfo.ADJUST2
        '        Public Property WITHHOLDING_TAX As Double Implements PayrollInfo.WITHHOLDING_TAX
        '        Public Property Pagibig_EE As Double Implements PayrollInfo.Pagibig_EE
        '        Public Property Pagibig_ER As Double Implements PayrollInfo.Pagibig_ER
        '        Public Property SSS_EE As Double Implements PayrollInfo.SSS_EE
        '        Public Property SSS_ER As Double Implements PayrollInfo.SSS_ER
        '        Public Property PHIC As Double Implements PayrollInfo.PHIC
        '        Public Property PHIC_EE As Double Implements PayrollInfo.PHIC_EE
        '        Public Property PHIC_ER As Double Implements PayrollInfo.PHIC_ER
        '        Public ReadOnly Property SSS_PHIC_EE As Double Implements PayrollInfo.SSS_PHIC_EE
        '            Get
        '                Return SSS_EE + PHIC_EE
        '            End Get
        '        End Property
        '        Public ReadOnly Property SSS_PHIC_ER As Double Implements PayrollInfo.SSS_PHIC_ER
        '            Get
        '                Return SSS_ER + PHIC_ER
        '            End Get
        '        End Property
        '        Public Property NET_PAY As Double Implements PayrollInfo.NET_PAY


        '#End Region
        '        Public Sub ComputeAll()
        '            ComputePagIbig()
        '            ComputeSSS()
        '            ComputePhic()
        '            ComputeWithholding()
        '        End Sub

        '        Public Sub ComputeSSS()
        '            Dim multiplier As Integer = CInt(((GROSS_PAY * 2) - 2750) \ 500)

        '            Dim ER_rsc As Double = Math.Min(255 + (42.5 * multiplier), 1700) 'MIN(255+(42.5*B6);1700)
        '            Dim EE_rsc As Double = Math.Min(135 + (22.5 * multiplier), 900) '=MIN(135+(22.5*B10);900)

        '            Dim ER_ec As Double = If(multiplier <= 23, 10, 30) '=IF(B11<23;10;30)
        '            Dim EE_ec As Double = 0

        '            Dim multiplier_mpc As Integer = Math.Max(0, multiplier - 34)
        '            Dim ER_mpf As Double = Math.Min(42.5 * multiplier_mpc, 425) '=MIN(42.5*G10;425)
        '            Dim EE_mpf As Double = Math.Min(22.5 * multiplier_mpc, 225) '=MIN(22.5*G6;225)

        '            SSS_ER = ER_rsc + ER_ec + ER_mpf
        '            SSS_EE = EE_rsc + EE_ec + EE_mpf
        '        End Sub

        '        Public Sub PastePayslip(nSheet As ISheet, pageIdx As Integer, payslipPosition As PayslipPositionChoices) Implements PayrollInfo.PastePayslip
        '            Throw New NotImplementedException()
        '        End Sub

        '        Public Function ComputePagIbig() As Double
        '            Pagibig_EE = GROSS_PAY * 0.02
        '            Pagibig_ER = 100
        '        End Function

        '        Public Function ComputePhic() As Double
        '            Dim _PHIC As Double = 0
        '            Select Case GROSS_PAY
        '                Case >= 70000
        '                    _PHIC = 1800
        '                Case >= 10000.01
        '                    _PHIC = GROSS_PAY * 0.03
        '                Case <= 10000
        '                    _PHIC = 300
        '            End Select

        '            PHIC = _PHIC
        '            PHIC_EE = _PHIC
        '            PHIC_ER = _PHIC
        '        End Function

        '        Public Function ComputeWithholding() As Double
        '            Select Case GROSS_PAY
        '                Case >= 666667
        '                    WITHHOLDING_TAX = 200833.33 + ((GROSS_PAY - 666667) * 0.35)
        '                Case >= 166667
        '                    WITHHOLDING_TAX = 40833.33 + ((GROSS_PAY - 166667) * 0.32)
        '                Case >= 66667
        '                    WITHHOLDING_TAX = 10833.33 + ((GROSS_PAY - 66667) * 0.3)
        '                Case >= 33333
        '                    WITHHOLDING_TAX = 2500 + ((GROSS_PAY - 33333) * 0.25)
        '                Case >= 20833.01
        '                    WITHHOLDING_TAX = 0 + ((GROSS_PAY - 20833.01) * 0.2)
        '                Case <= 20833
        '                    WITHHOLDING_TAX = 0
        '            End Select
        '        End Function

        '        Public Function GetPayrollDetail(row As IRow) As Object Implements PayrollInfo.GetPayrollDetail
        '            Throw New NotImplementedException()
        '        End Function

        '        Public Function WriteGovernment(row As IRow) As Object Implements PayrollInfo.WriteGovernment
        '            'Dim fullname_raw As String() = row.GetCell(1).StringCellValue.Trim(")").Split("(")
        '            'If fullname_raw.Length < 2 Then Return Nothing

        '            'Dim employee_name As String = fullname_raw(0).Trim
        '            'Dim employee_id As String = fullname_raw(1).Trim

        '            Dim gross As Double = row.GetCell(5).NumericCellValue

        '            row.GetCell(7).SetCellValue(Pagibig_EE) 'EE
        '            row.GetCell(8).SetCellValue(Pagibig_EE) 'ER
        '            row.GetCell(9).SetCellValue(SSS_EE) 'EE
        '            row.GetCell(10).SetCellValue(SSS_ER) 'ER

        '            Dim adjustIdx As Integer = 0 'adjust idx for 15th payroll
        '            If PayrollDate.Substring(0, 2) = "15" Then adjustIdx = 1

        '            Dim adj1 As Double, adj2 As Double
        '            If PayrollDate.Substring(0, 2) = "15" Then
        '                row.GetCell(11).SetCellValue(SSS_PHIC_EE) 'SSS+Phic EE
        '                row.GetCell(12).SetCellValue(SSS_PHIC_ER) 'SSS+Phic EE
        '                row.GetCell(13).SetCellValue(WITHHOLDING_TAX) 'SSS+Phic EE

        '                'row.GetCell(11).SetCellValue(sss_comp(1) + phic) 'SSS+Phic EE
        '                'row.GetCell(12).SetCellValue(sss_comp(0) + phic) 'SSS+Phic ER

        '                adj1 = row.GetCell(13 + adjustIdx).NumericCellValue
        '                adj2 = row.GetCell(14 + adjustIdx).NumericCellValue
        '            Else
        '                adj1 = Pagibig_EE
        '                row.GetCell(13).SetCellValue(adj1)

        '                adj2 = row.GetCell(14).NumericCellValue

        '                row.GetCell(11).SetCellValue(phic) 'SSS+Phic EE
        '                row.GetCell(12).SetCellValue(WITHHOLDING_TAX)
        '            End If

        '            Dim net As Double = gross - (SSS_EE + PHIC_EE + WITHHOLDING_TAX + adj1 + adj2)
        '            row.GetCell(15 + adjustIdx).SetCellValue(net)

        '            Return row
        '        End Function

        '    End Class


    End Namespace
End Namespace

