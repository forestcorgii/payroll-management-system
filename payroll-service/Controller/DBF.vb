Imports System.IO
Imports System.Text
Imports System.Windows.Forms

Public Class DBF
    Public Sub SavePayrollTimeToDBF(dbfPath As String, arg As Object(), payrollDate As Date)
        Try
            Dim flds As New List(Of DotNetDBF.DBFField)
            flds.Add(New DotNetDBF.DBFField("DATER", DotNetDBF.NativeDbType.Numeric, 10, 0))
            flds.Add(New DotNetDBF.DBFField("CODER", DotNetDBF.NativeDbType.Numeric, 10, 0))
            flds.Add(New DotNetDBF.DBFField("ID", DotNetDBF.NativeDbType.Char, 4, 0))
            flds.Add(New DotNetDBF.DBFField("REG_HRS", DotNetDBF.NativeDbType.Float, 10, 2))
            flds.Add(New DotNetDBF.DBFField("R_OT", DotNetDBF.NativeDbType.Float, 10, 2))
            flds.Add(New DotNetDBF.DBFField("RD_OT", DotNetDBF.NativeDbType.Float, 10, 2))
            flds.Add(New DotNetDBF.DBFField("RD_8", DotNetDBF.NativeDbType.Float, 10, 2))
            flds.Add(New DotNetDBF.DBFField("HOL_OT", DotNetDBF.NativeDbType.Float, 10, 2))
            flds.Add(New DotNetDBF.DBFField("HOL_OT8", DotNetDBF.NativeDbType.Float, 10, 2))
            flds.Add(New DotNetDBF.DBFField("ND", DotNetDBF.NativeDbType.Float, 10, 2))
            flds.Add(New DotNetDBF.DBFField("ABS_TAR", DotNetDBF.NativeDbType.Float, 10, 2))
            flds.Add(New DotNetDBF.DBFField("ADJUST1", DotNetDBF.NativeDbType.Float, 10, 2))
            flds.Add(New DotNetDBF.DBFField("GROSS_PAY", DotNetDBF.NativeDbType.Float, 10, 2))
            flds.Add(New DotNetDBF.DBFField("ADJUST2", DotNetDBF.NativeDbType.Float, 10, 2))
            flds.Add(New DotNetDBF.DBFField("TAX", DotNetDBF.NativeDbType.Float, 10, 2))
            flds.Add(New DotNetDBF.DBFField("SSS_EE", DotNetDBF.NativeDbType.Float, 10, 2))
            flds.Add(New DotNetDBF.DBFField("SSS_ER", DotNetDBF.NativeDbType.Float, 10, 2))
            flds.Add(New DotNetDBF.DBFField("PHIC", DotNetDBF.NativeDbType.Float, 10, 2))
            flds.Add(New DotNetDBF.DBFField("NET_PAY", DotNetDBF.NativeDbType.Float, 10, 2))
            flds.Add(New DotNetDBF.DBFField("REG_PAY", DotNetDBF.NativeDbType.Float, 10, 2))
            flds.Add(New DotNetDBF.DBFField("TAG", DotNetDBF.NativeDbType.Char, 1, 0))

            Dim records As New List(Of String())
            Dim values As List(Of Model.PayrollTimeInfo) = arg(0)
            For r As Integer = 0 To values.Count - 1
                values(r).CODER = arg(1)
                records.Add(values(r).ToDBFRecordFormat)
            Next

            If records.Count > 0 Then
                Dim s As Stream = File.Create(dbfPath)
                Dim writer As New DotNetDBF.DBFWriter(s)
                writer.CharEncoding = Encoding.UTF8
                writer.Fields = flds.ToArray
                For Each record As String() In records
                    writer.WriteRecord(record)
                Next
                writer.Close()
                s.Close()

                'WriteTimesheetExcelReport(Path.ChangeExtension(dbfPath, "xls"), arg(0), values(0).PayrollCode, values(0).BankCategory, payrollDate)
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class
