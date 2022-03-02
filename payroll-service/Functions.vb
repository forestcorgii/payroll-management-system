Imports System.Data.OleDb
Imports System.IO
Imports System.Net.Mail
Imports NPOI.SS.UserModel

Module Functions
    Public compressionKey As Integer = 5
    Public Sub OpenConnection(ByRef c As OleDbConnection, ByVal mdbPath As String, ByVal mdbPassword As String)
        c = New OleDbConnection("Provider=Microsoft.Jet.Oledb.4.0;Data Source=" & mdbPath & ";Jet OLEDB:Database Password=" & mdbPassword)
        c.Open()
    End Sub

    Public Function Reverse(ByVal value As String) As String
        Dim arr() As Char = value.ToCharArray()
        Array.Reverse(arr)
        Return New String(arr)
    End Function

    Public Function EncryptText(ByVal str As String) As String
        Dim res As String = ""
        str = Reverse(str)
        Dim i As Integer
        For i = 0 To str.ToCharArray.Length() - 1
            res = res & Chr(Asc(str.Chars(i)) + compressionKey)
        Next i
        EncryptText = res
    End Function

    Public Function DecryptText(ByVal str As String) As String
        Dim res = ""
        str = Reverse(str)
        Dim i As Integer
        For i = 0 To str.ToCharArray.Length() - 1
            res = res & Chr(Asc(str.Chars(i)) - compressionKey)
        Next i
        DecryptText = res
    End Function



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

    Public Function ComputeSSS(GROSS_PAY As Double) As Double()
        Dim multiplier As Integer = CInt(((GROSS_PAY * 2) - 2750) \ 500)

        Dim ER_rsc As Double = Math.Min(255 + (42.5 * multiplier), 1700) 'MIN(255+(42.5*B6);1700)
        Dim EE_rsc As Double = Math.Min(135 + (22.5 * multiplier), 900) '=MIN(135+(22.5*B10);900)

        Dim ER_ec As Double = If(multiplier <= 23, 10, 30) '=IF(B11<23;10;30)
        Dim EE_ec As Double = 0

        Dim multiplier_mpc As Integer = Math.Max(0, multiplier - 34)
        Dim ER_mpf As Double = Math.Min(42.5 * multiplier_mpc, 425) '=MIN(42.5*G10;425)
        Dim EE_mpf As Double = Math.Min(22.5 * multiplier_mpc, 225) '=MIN(22.5*G6;225)

        Return {ER_rsc + ER_ec + ER_mpf, EE_rsc + EE_ec + EE_mpf}
    End Function

    Public Function ComputePagIbig(GROSS_PAY As Double) As Double()
        Return {GROSS_PAY * 0.02, 100}
    End Function

    Public Function ComputePhic(GROSS_PAY As Double) As Double
        Dim _PHIC As Double = 0
        Select Case GROSS_PAY
            Case >= 70000
                _PHIC = 1800
            Case >= 10000.01
                _PHIC = GROSS_PAY * 0.03
            Case <= 10000
                _PHIC = 300
        End Select

        Return _PHIC
    End Function

    Public Function ComputeWithholding(taxable_pay As Double) As Double
        Dim WITHHOLDING_TAX As Double = 0
        Select Case taxable_pay
            Case >= 666667
                WITHHOLDING_TAX = 200833.33 + ((taxable_pay - 666667) * 0.35)
            Case >= 166667
                WITHHOLDING_TAX = 40833.33 + ((taxable_pay - 166667) * 0.32)
            Case >= 66667
                WITHHOLDING_TAX = 10833.33 + ((taxable_pay - 66667) * 0.3)
            Case >= 33333
                WITHHOLDING_TAX = 2500 + ((taxable_pay - 33333) * 0.25)
            Case >= 20833.01
                WITHHOLDING_TAX = 0 + ((taxable_pay - 20833.01) * 0.2)
            Case <= 20833
                WITHHOLDING_TAX = 0
        End Select
        Return WITHHOLDING_TAX
    End Function

End Module

