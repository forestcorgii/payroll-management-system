'Imports System.IO
Imports System.IO
Imports NPOI.HSSF.UserModel
Imports NPOI.SS.UserModel

Namespace Controller

    Public Class Export
        Public Shared Sub ExportDataUCPB(ByRef payArr As List(Of Payroll.Model), ByVal filePath As String, ByVal fileName As String)
            Dim excelTemplateUCPB As String = "" 'Application.StartupPath & "\" & frmEmployees.appFolder & "\template-UCPB.xls"
            Dim ucpbFile As String = filePath & "\" & fileName
            If Not File.Exists(excelTemplateUCPB) Then
                MsgBox("File not found:" & vbNewLine & excelTemplateUCPB, MsgBoxStyle.Critical, "Oops")
                Exit Sub
            End If

            Try
                Dim orig As String = fileName
                Dim x As Integer = 1
                While File.Exists(ucpbFile)
                    fileName = orig.Substring(0, orig.LastIndexOf(".")) & " (" & x & ").xls"
                    ucpbFile = filePath & "\" & fileName
                    x = x + 1
                End While

                File.Copy(excelTemplateUCPB, ucpbFile)

                '==============================================================
                For i As Integer = 0 To (payArr.Count - 1)
                    Dim rec As Payroll.Model = payArr(i)
                    'If rec._bankName = "UCPB" Then
                    '    '    xlWorkSheet.Range("A" & ctr).Value = rec._firstName
                    '    '    xlWorkSheet.Range("B" & ctr).Value = rec._lastName
                    '    '    If Trim(rec._accountNumber).Length = 14 Then
                    '    '        xlWorkSheet.Range("C" & ctr).Value = "5900010" & rec._accountNumber
                    '    '    Else
                    '    '        xlWorkSheet.Range("C" & ctr).Value = rec._accountNumber
                    '    '    End If
                    '    '    xlWorkSheet.Range("D" & ctr).Value = rec._amount
                    '    '    ctr = ctr + 1
                    '    '    cntUCPB = cntUCPB + 1
                    '    '    totalUCPB = totalUCPB + rec._amount
                    'End If
                Next
                '==============================================================

            Catch ex As Exception
                MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical, "Oops")
            End Try
        End Sub

        Public Shared Sub ExportDataMETROPALO_20211001(ByRef payArr As List(Of Payroll.Model), ByVal filePath As String, ByVal fileName As String)
            Dim excelTemplateMETROPALO As String = "" 'Application.StartupPath & "\" & frmEmployees.appFolder & "\template-METROPALO_20211001.xls"
            Dim METROPALOFile As String = filePath & "\" & fileName
            If Not File.Exists(excelTemplateMETROPALO) Then
                MsgBox("File not found:" & vbNewLine & excelTemplateMETROPALO, MsgBoxStyle.Critical, "Oops")
                Exit Sub
            End If

            Try
                Dim orig As String = fileName
                Dim x As Integer = 1
                While File.Exists(METROPALOFile)
                    fileName = orig.Substring(0, orig.LastIndexOf(".")) & " (" & x & ").xls"
                    METROPALOFile = filePath & "\" & fileName
                    x = x + 1
                End While

                File.Copy(excelTemplateMETROPALO, METROPALOFile)
                'Data Sheet
                For i As Integer = 0 To (payArr.Count - 1)
                    Dim rec As Payroll.Model = payArr(i)
                    'If rec._bankName = "METROBANK-PALO" Then
                    '    'seriesNum = seriesNum + 1
                    '    'xlWorkSheet.Range("A" & ctr).Value = rec._lastName
                    '    'xlWorkSheet.Range("B" & ctr).Value = rec._firstName
                    '    'xlWorkSheet.Range("C" & ctr).Value = rec._middleName
                    '    'xlWorkSheet.Range("D" & ctr).Value = "756" & rec._accountNumber
                    '    'xlWorkSheet.Range("E" & ctr).Value = rec._amount
                    '    'ctr = ctr + 1
                    '    'cntMETROPALO = cntMETROPALO + 1
                    '    'totalMETROPALO = totalMETROPALO + rec._amount
                    'End If
                Next

            Catch ex As Exception
                MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical, "Oops")
            End Try
        End Sub
        Public Shared Sub ExportDataMETROTAC_20211001(ByRef payArr As List(Of Payroll.Model), ByVal filePath As String, ByVal fileName As String)
            Dim excelTemplateMETROTAC As String = "" ' Application.StartupPath & "\" & frmEmployees.appFolder & "\template-METROTAC_20211001.xls"
            Dim METROTACFile As String = filePath & "\" & fileName
            If Not File.Exists(excelTemplateMETROTAC) Then
                MsgBox("File not found:" & vbNewLine & excelTemplateMETROTAC, MsgBoxStyle.Critical, "Oops")
                Exit Sub
            End If
            Try
                Dim orig As String = fileName
                Dim x As Integer = 1
                While File.Exists(METROTACFile)
                    fileName = orig.Substring(0, orig.LastIndexOf(".")) & " (" & x & ").xls"
                    METROTACFile = filePath & "\" & fileName
                    x = x + 1
                End While

                File.Copy(excelTemplateMETROTAC, METROTACFile)


                'Data Sheet
                For i As Integer = 0 To (payArr.Count - 1)
                    Dim rec As Payroll.Model = payArr(i)
                    'If rec._bankName = "METROBANK-TAC" Then
                    '    'seriesNum = seriesNum + 1
                    '    'xlWorkSheet.Range("A" & ctr).Value = rec._lastName
                    '    'xlWorkSheet.Range("B" & ctr).Value = rec._firstName
                    '    'xlWorkSheet.Range("B" & ctr).Value = rec._middleName
                    '    'xlWorkSheet.Range("D" & ctr).Value = "525" & rec._accountNumber
                    '    'xlWorkSheet.Range("E" & ctr).Value = rec._amount
                    '    'ctr = ctr + 1
                    '    'cntMETROTAC = cntMETROTAC + 1
                    '    'totalMETROTAC = totalMETROTAC + rec._amount
                    'End If
                Next

            Catch ex As Exception
                MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical, "Oops")
            End Try

        End Sub

        Public Shared Sub ExportDataNegative(ByRef payArr As List(Of Payroll.Model), ByVal filePath As String, ByVal fileName As String)
            Dim excelTemplateNegative As String = "" ' Application.StartupPath & "\" & frmEmployees.appFolder & "\template-ZEROS.xls"
            Dim negativeFile As String = filePath & "\" & fileName
            If Not File.Exists(excelTemplateNegative) Then
                MsgBox("File not found:" & vbNewLine & excelTemplateNegative, MsgBoxStyle.Critical, "Oops")
                Exit Sub
            End If

            Try
                Dim orig As String = fileName
                Dim x As Integer = 1
                While File.Exists(negativeFile)
                    fileName = orig.Substring(0, orig.LastIndexOf(".")) & " (" & x & ").xls"
                    negativeFile = filePath & "\" & fileName
                    x = x + 1
                End While

                File.Copy(excelTemplateNegative, negativeFile)


                For i As Integer = 0 To (payArr.Count - 1)
                    Dim rec As Payroll.Model = payArr(i)
                    'If ctr = 1 Then
                    'xlWorkSheet.Range("A" & ctr).Value = "IDNo"
                    'xlWorkSheet.Range("B" & ctr).Value = "Firstname"
                    'xlWorkSheet.Range("C" & ctr).Value = "Lastname"
                    'xlWorkSheet.Range("D" & ctr).Value = "Bank"
                    'xlWorkSheet.Range("E" & ctr).Value = "Account Number"
                    'xlWorkSheet.Range("F" & ctr).Value = "Amount"
                    'End If

                    'If rec._bankName.StartsWith("#") Then
                    '    'xlWorkSheet.Range("A" & ctr + 1).Value = rec._idNo
                    '    'xlWorkSheet.Range("B" & ctr + 1).Value = rec._firstName
                    '    'xlWorkSheet.Range("C" & ctr + 1).Value = rec._lastName
                    '    'xlWorkSheet.Range("D" & ctr + 1).Value = rec._bankName.Replace("#", "")
                    '    'xlWorkSheet.Range("E" & ctr + 1).Value = rec._accountNumber
                    '    'xlWorkSheet.Range("F" & ctr + 1).Value = rec._amount
                    '    'ctr = ctr + 1
                    '    'cntNegative = cntNegative + 1
                    '    'totalNegative = totalNegative + rec._amount
                    'End If
                Next


            Catch ex As Exception
                MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical, "Oops")
            End Try
        End Sub

        Public Shared Sub ExportDataCHINABANK(ByVal payArr As List(Of Payroll.Model), ByVal filePath As String, ByVal fileName As String)
            'payArr = SortArraylist(payArr)
            Dim excelTemplateCBC As String = "" ' Application.StartupPath & "\" & frmEmployees.appFolder & "\template-CHINABANK.xls"
            Dim cbcFile As String = filePath & "\" & fileName
            If Not File.Exists(excelTemplateCBC) Then
                MsgBox("File not found:" & vbNewLine & excelTemplateCBC, MsgBoxStyle.Critical, "Oops")
                Exit Sub
            End If

            Try
                Dim orig As String = fileName
                Dim x As Integer = 1
                While File.Exists(cbcFile)
                    fileName = orig.Substring(0, orig.LastIndexOf(".")) & " (" & x & ").xls"
                    cbcFile = filePath & "\" & fileName
                    x = x + 1
                End While


                File.Copy(excelTemplateCBC, cbcFile)

                For i As Integer = 0 To (payArr.Count - 1)
                    Dim rec As Payroll.Model = payArr(i)
                    'If rec._bankName = "CHINABANK" Then
                    '    'xlWorkSheet.Range("D" & ctr).Value = rec._accountNumber
                    '    'xlWorkSheet.Range("E" & ctr).Value = rec._lastName
                    '    'xlWorkSheet.Range("F" & ctr).Value = rec._firstName
                    '    'xlWorkSheet.Range("G" & ctr).Value = rec._middleName
                    '    'xlWorkSheet.Range("H" & ctr).Value = rec._amount

                    '    'ctr = ctr + 1
                    '    'cntCBC = cntCBC + 1
                    '    'totalCBC = totalCBC + rec._amount
                    'End If
                Next

            Catch ex As Exception
                MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical, "Oops")
            End Try
        End Sub

        Public Shared Sub ExportDataCHECK(ByRef payArr As List(Of Payroll.Model), ByVal filePath As String, ByVal fileName As String)
            Dim excelTemplateCHECK As String = "" 'Application.StartupPath & "\" & frmEmployees.appFolder & "\template-CHECK.xls"
            Dim checkFile As String = filePath & "\" & fileName
            If Not File.Exists(excelTemplateCHECK) Then
                MsgBox("File not found:" & vbNewLine & excelTemplateCHECK, MsgBoxStyle.Critical, "Oops")
                Exit Sub
            End If

            Try

                Dim orig As String = fileName
                Dim x As Integer = 1
                While File.Exists(checkFile)
                    fileName = orig.Substring(0, orig.LastIndexOf(".")) & " (" & x & ").xls"
                    checkFile = filePath & "\" & fileName
                    x = x + 1
                End While

                File.Copy(excelTemplateCHECK, checkFile)

                Dim nWorkBook As New HSSFWorkbook
                Dim xl7500UPSheet As ISheet = nWorkBook.CreateSheet("7500UP")
                Dim xl7500DOWNSheet As ISheet = nWorkBook.CreateSheet("7500DOWN")
                Dim xl200DOWNSheet As ISheet = nWorkBook.CreateSheet("200DOWN")
                Dim xl100KUPSheet As ISheet = nWorkBook.CreateSheet("100KUP")

                Dim Recordsd7500UP As Payroll.Model() = (From res In payArr Where Double.Parse(res.Net_Pay) >= 7500 Select res).ToArray
                Dim Records7500DOWN As Payroll.Model() = (From res In payArr Where Double.Parse(res.Net_Pay) < 7500 Select res).ToArray
                Dim Records200DOWN As Payroll.Model() = (From res In payArr Where Double.Parse(res.Net_Pay) >= 200 Select res).ToArray
                Dim Records100KUP As Payroll.Model() = (From res In payArr Where Double.Parse(res.Net_Pay) >= 100000 Select res).ToArray

                WriteSpecificReport(xl7500UPSheet, Recordsd7500UP)
                WriteSpecificReport(xl7500DOWNSheet, Records7500DOWN)
                WriteSpecificReport(xl200DOWNSheet, Records200DOWN)
                WriteSpecificReport(xl100KUPSheet, Records100KUP)

                Using nNewPayreg As IO.FileStream = New FileStream(checkFile, FileMode.Open, FileAccess.Write)
                    nWorkBook.Write(nNewPayreg)
                End Using
            Catch ex As Exception
                MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical, "Oops")
            End Try
        End Sub
        Public Shared Sub WriteSpecificReport(sheet As ISheet, payrollRecords As Payroll.Model())
            Dim row As IRow = sheet.CreateRow(0)
            row.CreateCell(0).SetCellValue("IDNo")
            row.CreateCell(1).SetCellValue("Fullname")
            row.CreateCell(2).SetCellValue("Amount")

            For i As Integer = 0 To (payrollRecords.Count - 1)
                Dim rec As Payroll.Model = payrollRecords(i)
                If rec.EE.Bank_Category = "**CHECK**" Then
                    row = sheet.CreateRow(i + 1)
                    row.CreateCell(0).SetCellValue(rec.EE.EE_Id)
                    row.CreateCell(1).SetCellValue(rec.EE.Fullname)
                    row.CreateCell(2).SetCellValue(rec.Net_Pay)
                End If
            Next
        End Sub

    End Class
End Namespace
