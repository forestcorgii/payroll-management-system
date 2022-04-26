Imports System.Windows.Forms
Imports NPOI.SS.UserModel
Imports payroll_module.Payroll

Namespace Payroll
    Public Class PayslipController

        Public Shared Sub PastePayslip(nSheet As ISheet, pageIdx As Integer, payroll As PayrollModel, payslipPosition As PayslipPositionChoices)
            Dim startRowIdx As Integer = 67 * pageIdx
            Try
                If payslipPosition = PayslipPositionChoices.LOWER_RIGHT Or payslipPosition = PayslipPositionChoices.LOWER_LEFT Then
                    startRowIdx += 31
                End If

                Dim startColIdx As Integer = 0
                If payslipPosition = PayslipPositionChoices.LOWER_RIGHT Or payslipPosition = PayslipPositionChoices.UPPER_RIGHT Then
                    startColIdx = 5
                End If

                nSheet.GetRow(startRowIdx + 3).CreateCell(startColIdx + 1).SetCellValue(payroll.EE.EE_Id)

                nSheet.GetRow(startRowIdx + 3).CreateCell(startColIdx + 2).SetCellValue(payroll.Payroll_Date)

                nSheet.GetRow(startRowIdx + 5).CreateCell(startColIdx + 1).SetCellValue(payroll.EE.Fullname)

                nSheet.GetRow(startRowIdx + 7).CreateCell(startColIdx + 4).SetCellValue(payroll.Gross_Pay)
                nSheet.GetRow(startRowIdx + 15).CreateCell(startColIdx + 4).SetCellValue(payroll.Adjust1)

                nSheet.GetRow(startRowIdx + 17).CreateCell(startColIdx + 4).SetCellValue(payroll.Gross_Pay)
                nSheet.GetRow(startRowIdx + 18).CreateCell(startColIdx + 4).SetCellValue(payroll.Adjust2)
                nSheet.GetRow(startRowIdx + 19).CreateCell(startColIdx + 4).SetCellValue(-payroll.Government.Withholding_Tax)
                nSheet.GetRow(startRowIdx + 20).CreateCell(startColIdx + 4).SetCellValue(-(payroll.Government.SSS_EE + payroll.Government.PhilHealth))

                nSheet.GetRow(startRowIdx + 22).CreateCell(startColIdx + 4).SetCellValue(payroll.Net_Pay)

                nSheet.GetRow(startRowIdx + 27).CreateCell(startColIdx + 1).SetCellValue(payroll.EE.EE_Id)
                nSheet.GetRow(startRowIdx + 28).CreateCell(startColIdx + 1).SetCellValue(payroll.EE.Fullname)

                nSheet.GetRow(startRowIdx + 28).CreateCell(startColIdx + 4).SetCellValue(payroll.Net_Pay)
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Error pasting Payslip.", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End Try
        End Sub
        Public Sub LayoutPage(pageIdx As Integer, nSheet As ISheet)
            Dim startRowIdx As Integer = 67 * pageIdx
            Try

                '----------------CREATE ROWS------------------------------------
                For i As Integer = startRowIdx To startRowIdx + 67
                    nSheet.CreateRow(i) '.HeightInPoints = 50.0F
                Next
                '----------------STYLE-----------------------------------------

                '------------DASHES-----------------------------------------
                For Each idx As Integer In {0, 24, 31, 55, 62}
                    '------------HORIZONTAL-------------
                    For i As Integer = 0 To 10
                        nSheet.GetRow(startRowIdx + idx).CreateCell(i).SetCellValue("-- -- -- -- -- -- -- --")
                    Next
                    '------------VERTICAL---------------
                    nSheet.GetRow(startRowIdx + idx).CreateCell(0).SetCellValue("|-")
                    nSheet.GetRow(startRowIdx + idx).CreateCell(5).SetCellValue("|")
                    nSheet.GetRow(startRowIdx + idx).CreateCell(10).SetCellValue("|")
                Next

                For Each range As Integer() In {New Integer() {1, 23}, New Integer() {25, 30}, New Integer() {32, 54}, New Integer() {56, 61}}
                    '------------VERTICAL---------------
                    For i As Integer = range(0) To range(1)
                        nSheet.GetRow(startRowIdx + i).CreateCell(0).SetCellValue("|")
                        nSheet.GetRow(startRowIdx + i).CreateCell(5).SetCellValue("|")
                        nSheet.GetRow(startRowIdx + i).CreateCell(10).SetCellValue("|")
                    Next
                Next
                '------------------------------------------------------------------------

                '-------------------------------TITLES-----------------------------------
                For Each rowIdx As Integer In {7, 38}
                    For Each colIdx As Integer In {0, 5}
                        Dim titlesA As String() = {"REG", "R_OT", "RD_OT", "RD_8", "HOL_OT", "HOL_OT8", "ND", "ABS_TAR", "ADJUST1"}
                        For i As Integer = 0 To titlesA.Length - 1
                            nSheet.GetRow(startRowIdx + rowIdx + i).CreateCell(colIdx + 1).SetCellValue(titlesA(i))
                            nSheet.GetRow(startRowIdx + rowIdx + i).CreateCell(colIdx + 4).SetCellValue(0)
                        Next
                        Dim titlesB As String() = {"ADJUST2", "WITHHOLDING TAX", "SSS+PHIC"}
                        For i As Integer = 0 To titlesB.Length - 1
                            nSheet.GetRow(startRowIdx + rowIdx + i + 11).CreateCell(colIdx + 2).SetCellValue(titlesB(i))
                            nSheet.GetRow(startRowIdx + rowIdx + i + 11).CreateCell(colIdx + 4).SetCellValue(0)
                        Next
                        For Each titlesC As Object() In {New Object() {"GROSS PAY", 10}, New Object() {"NET", 15}}
                            nSheet.GetRow(startRowIdx + rowIdx + titlesC(1) - 1).CreateCell(colIdx + 4).SetCellValue("------------------")
                            nSheet.GetRow(startRowIdx + rowIdx + titlesC(1)).CreateCell(colIdx + 1).SetCellValue(titlesC(0).ToString)
                            nSheet.GetRow(startRowIdx + rowIdx + titlesC(1)).CreateCell(colIdx + 4).SetCellValue(0)
                        Next
                    Next
                Next
                '------------------------------------------------------------------------
            Catch ex As Exception
                '   MessageBox.Show(ex.Message, "Error in Layout Page.", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Console.WriteLine(ex.Message)
            End Try
        End Sub



        Public Enum PayslipPositionChoices
            UPPER_LEFT
            UPPER_RIGHT
            LOWER_LEFT
            LOWER_RIGHT
        End Enum
    End Class

End Namespace
