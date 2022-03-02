'Imports System.IO
Namespace Controller

    'Public Class Export
    '    Public Shared Sub ExportDataUCPB(ByRef payArr As List(Of model.PayrollRecord), ByVal filePath As String, ByVal fileName As String)
    '        Dim excelTemplateUCPB As String = Application.StartupPath & "\" & frmEmployees.appFolder & "\template-UCPB.xls"
    '        Dim ucpbFile As String = filePath & "\" & fileName
    '        If Not File.Exists(excelTemplateUCPB) Then
    '            MsgBox("File not found:" & vbNewLine & excelTemplateUCPB, MsgBoxStyle.Critical, "Oops")
    '            Exit Sub
    '        End If

    '        Dim xlsApp As Excel.Application = Nothing
    '        Dim xlsWorkBooks As Excel.Workbooks = Nothing
    '        Dim xlsWB As Excel.Workbook = Nothing
    '        Dim xlWorkSheet As Excel.Worksheet = Nothing

    '        lblStatus.Text = "Exporting UCPB..."
    '        Application.DoEvents()

    '        Try
    '            Dim orig As String = fileName
    '            Dim x As Integer = 1
    '            While File.Exists(ucpbFile)
    '                fileName = orig.Substring(0, orig.LastIndexOf(".")) & " (" & x & ").xls"
    '                ucpbFile = filePath & "\" & fileName
    '                x = x + 1
    '            End While

    '            File.Copy(excelTemplateUCPB, ucpbFile)

    '            xlsApp = New Excel.Application
    '            xlsApp.Visible = False
    '            xlsApp.DisplayAlerts = False
    '            xlsWorkBooks = xlsApp.Workbooks
    '            xlsWB = xlsWorkBooks.Open(ucpbFile)
    '            xlWorkSheet = xlsWB.Sheets(1)
    '            xlsWB.Activate()
    '            xlWorkSheet.Activate()

    '            Dim ctr As Integer = 1
    '            cntUCPB = 0

    '            '==============================================================
    '            For i As Integer = 0 To (payArr.Count - 1)
    '                Dim rec As model.PayrollRecord = payArr(i)
    '                If rec._bankName = "UCPB" Then
    '                    xlWorkSheet.Range("A" & ctr).Value = rec._firstName
    '                    xlWorkSheet.Range("B" & ctr).Value = rec._lastName
    '                    If Trim(rec._accountNumber).Length = 14 Then
    '                        xlWorkSheet.Range("C" & ctr).Value = "5900010" & rec._accountNumber
    '                    Else
    '                        xlWorkSheet.Range("C" & ctr).Value = rec._accountNumber
    '                    End If
    '                    xlWorkSheet.Range("D" & ctr).Value = rec._amount
    '                    ctr = ctr + 1
    '                    cntUCPB = cntUCPB + 1
    '                    totalUCPB = totalUCPB + rec._amount
    '                End If
    '                'pgrStatus.Value = (i + 1)
    '                lblStatus.Text = "Exporting UCPB " & (i + 1) & " of " & (payArr.Count + 1) & " ..."
    '                Application.DoEvents()
    '            Next
    '            '==============================================================


    '            lblStatus.Text = "Saving UCPB file..."
    '            Application.DoEvents()

    '            xlWorkSheet.Cells.Select()
    '            xlWorkSheet.Cells.EntireColumn.AutoFit()

    '            If ctr > 1 Then
    '                xlWorkSheet.Range("A1:D" & (ctr - 1)).Select()
    '                xlsApp.Selection.Sort(Key1:=xlWorkSheet.Range("B1"), Order1:=Excel.XlSortOrder.xlAscending, Key2:=xlWorkSheet.Range("A1"), Order2:=Excel.XlSortOrder.xlAscending, Header:=Excel.XlYesNoGuess.xlGuess, OrderCustom:=1, MatchCase:=False, Orientation:=Excel.Constants.xlTopToBottom)
    '            End If

    '            xlWorkSheet.Range("E1").Select()
    '            xlsApp.DisplayAlerts = False
    '            xlsWB.SaveAs(ucpbFile, 1) ' 1== xls

    '            'WriteEmail("UCPB")
    '        Catch ex As Exception
    '            MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical, "Oops")
    '        Finally
    '            If xlsWB IsNot Nothing Then
    '                xlsWB.Close()
    '            End If
    '            xlsWB = Nothing

    '            If xlsApp IsNot Nothing Then
    '                xlsApp.Quit()
    '            End If
    '            xlsApp = Nothing

    '            If conn IsNot Nothing Then
    '                conn.Close()
    '            End If
    '        End Try
    '    End Sub

    '    Public Shared Sub ExportDataMETROPALO_20211001(ByRef payArr As List(Of model.PayrollRecord), ByVal filePath As String, ByVal fileName As String)
    '        Dim excelTemplateMETROPALO As String = Application.StartupPath & "\" & frmEmployees.appFolder & "\template-METROPALO_20211001.xls"
    '        Dim METROPALOFile As String = filePath & "\" & fileName
    '        If Not File.Exists(excelTemplateMETROPALO) Then
    '            MsgBox("File not found:" & vbNewLine & excelTemplateMETROPALO, MsgBoxStyle.Critical, "Oops")
    '            Exit Sub
    '        End If

    '        Dim xlsApp As Excel.Application = Nothing
    '        Dim xlsWorkBooks As Excel.Workbooks = Nothing
    '        Dim xlsWB As Excel.Workbook = Nothing
    '        Dim xlWorkSheet As Excel.Worksheet = Nothing

    '        lblStatus.Text = "Exporting METROPALO..."
    '        Application.DoEvents()

    '        Try
    '            Dim orig As String = fileName
    '            Dim x As Integer = 1
    '            While File.Exists(METROPALOFile)
    '                fileName = orig.Substring(0, orig.LastIndexOf(".")) & " (" & x & ").xls"
    '                METROPALOFile = filePath & "\" & fileName
    '                x = x + 1
    '            End While

    '            File.Copy(excelTemplateMETROPALO, METROPALOFile)

    '            xlsApp = New Excel.Application
    '            xlsApp.Visible = False
    '            xlsApp.DisplayAlerts = False
    '            xlsWorkBooks = xlsApp.Workbooks
    '            xlsWB = xlsWorkBooks.Open(METROPALOFile)
    '            xlWorkSheet = xlsWB.Sheets(1)
    '            xlsWB.Activate()
    '            xlWorkSheet.Activate()

    '            Dim ctr As Integer = 2
    '            cntMETROPALO = 0
    '            Dim seriesNum As Integer = 0

    '            'Data Sheet
    '            For i As Integer = 0 To (payArr.Count - 1)
    '                Dim rec As model.PayrollRecord = payArr(i)
    '                If rec._bankName = "METROBANK-PALO" Then
    '                    seriesNum = seriesNum + 1
    '                    xlWorkSheet.Range("A" & ctr).Value = rec._lastName
    '                    xlWorkSheet.Range("B" & ctr).Value = rec._firstName
    '                    xlWorkSheet.Range("C" & ctr).Value = rec._middleName
    '                    'xlWorkSheet.Range("C" & ctr).Value = xlWorkSheet.Range("B3").Value
    '                    xlWorkSheet.Range("D" & ctr).Value = "756" & rec._accountNumber
    '                    xlWorkSheet.Range("E" & ctr).Value = rec._amount
    '                    ctr = ctr + 1
    '                    cntMETROPALO = cntMETROPALO + 1
    '                    totalMETROPALO = totalMETROPALO + rec._amount
    '                End If
    '                'pgrStatus.Value = (i + 1)
    '                lblStatus.Text = "Exporting METROPALO " & (i + 1) & " of " & (payArr.Count + 1) & " ..."
    '                Application.DoEvents()
    '            Next


    '            lblStatus.Text = "Saving METROPALO file..."
    '            Application.DoEvents()


    '            'xlWorkSheet.Range("A1").Select()
    '            xlsApp.DisplayAlerts = False
    '            xlsWB.SaveAs(METROPALOFile, 1) ' 1== xls

    '            WriteEmail("METROPALO")
    '        Catch ex As Exception
    '            MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical, "Oops")
    '        Finally
    '            If xlsWB IsNot Nothing Then
    '                xlsWB.Close()
    '            End If
    '            xlsWB = Nothing

    '            If xlsApp IsNot Nothing Then
    '                xlsApp.Quit()
    '            End If
    '            xlsApp = Nothing

    '            If conn IsNot Nothing Then
    '                conn.Close()
    '            End If
    '        End Try
    '    End Sub
    '    Public Shared Sub ExportDataMETROTAC_20211001(ByRef payArr As List(Of model.PayrollRecord), ByVal filePath As String, ByVal fileName As String)
    '        Dim excelTemplateMETROTAC As String = Application.StartupPath & "\" & frmEmployees.appFolder & "\template-METROTAC_20211001.xls"
    '        Dim METROTACFile As String = filePath & "\" & fileName
    '        If Not File.Exists(excelTemplateMETROTAC) Then
    '            MsgBox("File not found:" & vbNewLine & excelTemplateMETROTAC, MsgBoxStyle.Critical, "Oops")
    '            Exit Sub
    '        End If

    '        Dim xlsApp As Excel.Application = Nothing
    '        Dim xlsWorkBooks As Excel.Workbooks = Nothing
    '        Dim xlsWB As Excel.Workbook = Nothing
    '        Dim xlWorkSheet As Excel.Worksheet = Nothing


    '        lblStatus.Text = "Exporting METROTAC..."
    '        Application.DoEvents()

    '        Try
    '            Dim orig As String = fileName
    '            Dim x As Integer = 1
    '            While File.Exists(METROTACFile)
    '                fileName = orig.Substring(0, orig.LastIndexOf(".")) & " (" & x & ").xls"
    '                METROTACFile = filePath & "\" & fileName
    '                x = x + 1
    '            End While

    '            File.Copy(excelTemplateMETROTAC, METROTACFile)

    '            xlsApp = New Excel.Application
    '            xlsApp.Visible = False
    '            xlsApp.DisplayAlerts = False
    '            xlsWorkBooks = xlsApp.Workbooks
    '            xlsWB = xlsWorkBooks.Open(METROTACFile)
    '            xlWorkSheet = xlsWB.Sheets(1)
    '            xlsWB.Activate()
    '            xlWorkSheet.Activate()

    '            Dim ctr As Integer = 2
    '            cntMETROTAC = 0
    '            Dim seriesNum As Integer = 0

    '            'Data Sheet
    '            For i As Integer = 0 To (payArr.Count - 1)
    '                Dim rec As model.PayrollRecord = payArr(i)
    '                If rec._bankName = "METROBANK-TAC" Then
    '                    seriesNum = seriesNum + 1
    '                    xlWorkSheet.Range("A" & ctr).Value = rec._lastName
    '                    xlWorkSheet.Range("B" & ctr).Value = rec._firstName
    '                    xlWorkSheet.Range("B" & ctr).Value = rec._middleName
    '                    'xlWorkSheet.Range("C" & ctr).Value = xlWorkSheet.Range("B3").Value
    '                    xlWorkSheet.Range("D" & ctr).Value = "525" & rec._accountNumber
    '                    xlWorkSheet.Range("E" & ctr).Value = rec._amount
    '                    ctr = ctr + 1
    '                    cntMETROTAC = cntMETROTAC + 1
    '                    totalMETROTAC = totalMETROTAC + rec._amount
    '                End If
    '                'pgrStatus.Value = (i + 1)
    '                lblStatus.Text = "Exporting METROTAC " & (i + 1) & " of " & (payArr.Count + 1) & " ..."
    '                Application.DoEvents()
    '            Next


    '            lblStatus.Text = "Saving METROTAC file..."
    '            Application.DoEvents()

    '            'xlWorkSheet.Range("A1").Select()
    '            xlsApp.DisplayAlerts = False
    '            xlsWB.SaveAs(METROTACFile, 1) ' 1== xls

    '            WriteEmail("METROTAC")
    '        Catch ex As Exception
    '            MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical, "Oops")
    '        Finally
    '            If xlsWB IsNot Nothing Then
    '                xlsWB.Close()
    '            End If
    '            xlsWB = Nothing

    '            If xlsApp IsNot Nothing Then
    '                xlsApp.Quit()
    '            End If
    '            xlsApp = Nothing

    '            If conn IsNot Nothing Then
    '                conn.Close()
    '            End If
    '        End Try

    '    End Sub

    '    Public Shared Sub ExportDataNegative(ByRef payArr As List(Of model.PayrollRecord), ByVal filePath As String, ByVal fileName As String)
    '        Dim excelTemplateNegative As String = Application.StartupPath & "\" & frmEmployees.appFolder & "\template-ZEROS.xls"
    '        Dim negativeFile As String = filePath & "\" & fileName
    '        If Not File.Exists(excelTemplateNegative) Then
    '            MsgBox("File not found:" & vbNewLine & excelTemplateNegative, MsgBoxStyle.Critical, "Oops")
    '            Exit Sub
    '        End If

    '        Dim xlsApp As Excel.Application = Nothing
    '        Dim xlsWorkBooks As Excel.Workbooks = Nothing
    '        Dim xlsWB As Excel.Workbook = Nothing
    '        Dim xlWorkSheet As Excel.Worksheet = Nothing

    '        lblStatus.Text = "Exporting Negatives/Zeros..."
    '        Application.DoEvents()

    '        Try
    '            Dim orig As String = fileName
    '            Dim x As Integer = 1
    '            While File.Exists(negativeFile)
    '                fileName = orig.Substring(0, orig.LastIndexOf(".")) & " (" & x & ").xls"
    '                negativeFile = filePath & "\" & fileName
    '                x = x + 1
    '            End While

    '            File.Copy(excelTemplateNegative, negativeFile)

    '            xlsApp = New Excel.Application
    '            xlsApp.Visible = False
    '            xlsApp.DisplayAlerts = False
    '            xlsWorkBooks = xlsApp.Workbooks
    '            xlsWB = xlsWorkBooks.Open(negativeFile)
    '            xlWorkSheet = xlsWB.Sheets(1)
    '            xlsWB.Activate()
    '            xlWorkSheet.Activate()

    '            Dim ctr As Integer = 1
    '            cntNegative = 0
    '            For i As Integer = 0 To (payArr.Count - 1)
    '                Dim rec As model.PayrollRecord = payArr(i)
    '                If ctr = 1 Then
    '                    xlWorkSheet.Range("A" & ctr).Value = "IDNo"
    '                    xlWorkSheet.Range("B" & ctr).Value = "Firstname"
    '                    xlWorkSheet.Range("C" & ctr).Value = "Lastname"
    '                    xlWorkSheet.Range("D" & ctr).Value = "Bank"
    '                    xlWorkSheet.Range("E" & ctr).Value = "Account Number"
    '                    xlWorkSheet.Range("F" & ctr).Value = "Amount"
    '                End If

    '                If rec._bankName.StartsWith("#") Then
    '                    xlWorkSheet.Range("A" & ctr + 1).Value = rec._idNo
    '                    xlWorkSheet.Range("B" & ctr + 1).Value = rec._firstName
    '                    xlWorkSheet.Range("C" & ctr + 1).Value = rec._lastName
    '                    xlWorkSheet.Range("D" & ctr + 1).Value = rec._bankName.Replace("#", "")
    '                    xlWorkSheet.Range("E" & ctr + 1).Value = rec._accountNumber
    '                    xlWorkSheet.Range("F" & ctr + 1).Value = rec._amount
    '                    ctr = ctr + 1
    '                    cntNegative = cntNegative + 1
    '                    totalNegative = totalNegative + rec._amount
    '                End If
    '                'pgrStatus.Value = (i + 1)
    '                lblStatus.Text = "Exporting Negative " & (i + 1) & " of " & (payArr.Count + 1) & " ..."
    '                Application.DoEvents()
    '            Next

    '            lblStatus.Text = "Saving Negative file..."
    '            Application.DoEvents()

    '            xlWorkSheet.Cells.Select()
    '            xlWorkSheet.Cells.EntireColumn.AutoFit()

    '            If ctr > 1 Then
    '                xlWorkSheet.Range("A2:F" & (ctr - 1)).Select()
    '                xlsApp.Selection.Sort(Key1:=xlWorkSheet.Range("C2"), Order1:=Excel.XlSortOrder.xlAscending, Key2:=xlWorkSheet.Range("B2"), Order2:=Excel.XlSortOrder.xlAscending, Header:=Excel.XlYesNoGuess.xlGuess, OrderCustom:=1, MatchCase:=False, Orientation:=Excel.Constants.xlTopToBottom)
    '            End If

    '            xlWorkSheet.Range("A1").Select()
    '            xlsApp.DisplayAlerts = False
    '            xlsWB.SaveAs(negativeFile, 1) ' 1== xls

    '            WriteEmail("Negative")
    '        Catch ex As Exception
    '            MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical, "Oops")
    '        Finally
    '            If xlsWB IsNot Nothing Then
    '                xlsWB.Close()
    '            End If
    '            xlsWB = Nothing

    '            If xlsApp IsNot Nothing Then
    '                xlsApp.Quit()
    '            End If
    '            xlsApp = Nothing

    '            If conn IsNot Nothing Then
    '                conn.Close()
    '            End If
    '        End Try
    '    End Sub

    '    Public Shared Sub ExportDataCHINABANK(ByVal payArr As List(Of model.PayrollRecord), ByVal filePath As String, ByVal fileName As String)
    '        payArr = SortArraylist(payArr)
    '        Dim excelTemplateCBC As String = Application.StartupPath & "\" & frmEmployees.appFolder & "\template-CHINABANK.xls"
    '        Dim cbcFile As String = filePath & "\" & fileName
    '        If Not File.Exists(excelTemplateCBC) Then
    '            MsgBox("File not found:" & vbNewLine & excelTemplateCBC, MsgBoxStyle.Critical, "Oops")
    '            Exit Sub
    '        End If

    '        Dim xlsApp As Excel.Application = Nothing
    '        Dim xlsWorkBooks As Excel.Workbooks = Nothing
    '        Dim xlsWB As Excel.Workbook = Nothing
    '        Dim xlWorkSheet As Excel.Worksheet = Nothing

    '        lblStatus.Text = "Exporting CBC..."
    '        Application.DoEvents()
    '        Try
    '            Dim orig As String = fileName
    '            Dim x As Integer = 1
    '            While File.Exists(cbcFile)
    '                fileName = orig.Substring(0, orig.LastIndexOf(".")) & " (" & x & ").xls"
    '                cbcFile = filePath & "\" & fileName
    '                x = x + 1
    '            End While


    '            File.Copy(excelTemplateCBC, cbcFile)
    '            Application.DoEvents()

    '            xlsApp = New Excel.Application
    '            xlsApp.Visible = False
    '            xlsApp.DisplayAlerts = False
    '            xlsWorkBooks = xlsApp.Workbooks
    '            xlsWB = xlsWorkBooks.Open(cbcFile)
    '            xlWorkSheet = xlsWB.Sheets(1)
    '            xlsWB.Activate()
    '            xlWorkSheet.Activate()

    '            Dim ctr As Integer = 5
    '            cntCBC = 0
    '            For i As Integer = 0 To (payArr.Count - 1)
    '                Dim rec As model.PayrollRecord = payArr(i)
    '                If rec._bankName = "CHINABANK" Then
    '                    xlWorkSheet.Range("D" & ctr).Value = rec._accountNumber
    '                    xlWorkSheet.Range("E" & ctr).Value = rec._lastName
    '                    xlWorkSheet.Range("F" & ctr).Value = rec._firstName
    '                    xlWorkSheet.Range("G" & ctr).Value = rec._middleName
    '                    xlWorkSheet.Range("H" & ctr).Value = rec._amount

    '                    ctr = ctr + 1
    '                    cntCBC = cntCBC + 1
    '                    totalCBC = totalCBC + rec._amount
    '                End If
    '                'pgrStatus.Value = (i + 1)
    '                lblStatus.Text = "Exporting CBC " & (i + 1) & " of " & (payArr.Count + 1) & " ..."
    '                Application.DoEvents()
    '            Next

    '            'xlWorkSheet.Range("A4:D" & (ctr - 1)).Select()
    '            'xlsApp.Selection.Sort(Key1:=xlWorkSheet.Range("B4"), Order1:=Excel.XlSortOrder.xlAscending, Header:=Excel.XlYesNoGuess.xlGuess, OrderCustom:=1, MatchCase:=False, Orientation:=Excel.Constants.xlTopToBottom)

    '            lblStatus.Text = "Saving CBC file..."
    '            Application.DoEvents()
    '            xlsApp.DisplayAlerts = False
    '            xlsWB.SaveAs(cbcFile, 1) ' 1== xls

    '            WriteEmail("CHINABANK")
    '        Catch ex As Exception
    '            MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical, "Oops")
    '        Finally
    '            If xlsWB IsNot Nothing Then
    '                xlsWB.Close()
    '            End If
    '            xlsWB = Nothing

    '            If xlsApp IsNot Nothing Then
    '                xlsApp.Quit()
    '            End If
    '            xlsApp = Nothing

    '            If conn IsNot Nothing Then
    '                conn.Close()
    '            End If
    '        End Try
    '    End Sub

    '    Public Shared Sub ExportDataCHECK(ByRef payArr As List(Of model.PayrollRecord), ByVal filePath As String, ByVal fileName As String)
    '        Dim excelTemplateCHECK As String = Application.StartupPath & "\" & frmEmployees.appFolder & "\template-CHECK.xls"
    '        Dim checkFile As String = filePath & "\" & fileName
    '        If Not File.Exists(excelTemplateCHECK) Then
    '            MsgBox("File not found:" & vbNewLine & excelTemplateCHECK, MsgBoxStyle.Critical, "Oops")
    '            Exit Sub
    '        End If

    '        Dim xlsApp As Excel.Application = Nothing
    '        Dim xlsWorkBooks As Excel.Workbooks = Nothing
    '        Dim xlsWB As Excel.Workbook = Nothing
    '        'Dim xlWorkSheet As Excel.Worksheet = Nothing
    '        Dim xl7500UPSheet As Excel.Worksheet = Nothing
    '        Dim xl7500DOWNSheet As Excel.Worksheet = Nothing
    '        Dim xl200DOWNSheet As Excel.Worksheet = Nothing
    '        Dim xl100KUPSheet As Excel.Worksheet = Nothing

    '        lblStatus.Text = "Exporting CHECK..."
    '        Application.DoEvents()
    '        Try
    '            Dim orig As String = fileName
    '            Dim x As Integer = 1
    '            While File.Exists(checkFile)
    '                fileName = orig.Substring(0, orig.LastIndexOf(".")) & " (" & x & ").xls"
    '                checkFile = filePath & "\" & fileName
    '                x = x + 1
    '            End While

    '            File.Copy(excelTemplateCHECK, checkFile)

    '            xlsApp = New Excel.Application
    '            xlsApp.Visible = False
    '            xlsApp.DisplayAlerts = False
    '            xlsWorkBooks = xlsApp.Workbooks
    '            xlsWB = xlsWorkBooks.Open(checkFile)

    '            'xlWorkSheet = xlsWB.Sheets(1)
    '            xl7500UPSheet = xlsWB.Sheets(1)
    '            xl7500DOWNSheet = xlsWB.Sheets(2)
    '            xl200DOWNSheet = xlsWB.Sheets(3)
    '            xl100KUPSheet = xlsWB.Sheets(4)

    '            xlsWB.Activate()
    '            xl7500UPSheet.Activate()
    '            xl7500DOWNSheet.Activate()
    '            xl200DOWNSheet.Activate()
    '            xl100KUPSheet.Activate()

    '            Dim ctr As Integer = 1
    '            cntCHK = 0


    '            Dim Recordsd7500UP As model.PayrollRecord() = (From res In payArr Where Double.Parse(res._amount) >= 7500 Select res).ToArray
    '            Dim Records7500DOWN As model.PayrollRecord() = (From res In payArr Where Double.Parse(res._amount) < 7500 Select res).ToArray
    '            Dim Records200DOWN As model.PayrollRecord() = (From res In payArr Where Double.Parse(res._amount) >= 200 Select res).ToArray
    '            Dim Records100KUP As model.PayrollRecord() = (From res In payArr Where Double.Parse(res._amount) >= 100000 Select res).ToArray

    '            WriteSpecificReport(xl7500UPSheet, Recordsd7500UP)
    '            WriteSpecificReport(xl7500DOWNSheet, Records7500DOWN)
    '            WriteSpecificReport(xl200DOWNSheet, Records200DOWN)
    '            WriteSpecificReport(xl100KUPSheet, Records100KUP)


    '            'xlWorkSheet.Range("A1").Value = "IDNo"
    '            'xlWorkSheet.Range("B1").Value = "Fullname"
    '            'xlWorkSheet.Range("C1").Value = "Amount"

    '            'For i As Integer = 0 To (payArr.Count - 1)
    '            '    Dim rec As model.PayrollRecord = payArr(i)
    '            '    If rec._bankName = "**CHECK**" Then

    '            '        xlWorkSheet.Range("A" & ctr + 1).Value = rec._idNo
    '            '        xlWorkSheet.Range("B" & ctr + 1).Value = rec._fullName
    '            '        xlWorkSheet.Range("C" & ctr + 1).Value = rec._amount
    '            '        ctr = ctr + 1
    '            '        cntCHK = cntCHK + 1
    '            '        totalCHK = totalCHK + rec._amount
    '            '    End If
    '            '    'pgrStatus.Value = (i + 1)
    '            '    lblStatus.Text = "Exporting CHECK " & (i + 1) & " of " & (payArr.Count + 1) & " ..."
    '            '    Application.DoEvents()
    '            'Next

    '            'lblStatus.Text = "Saving CHECK file..."
    '            'Application.DoEvents()
    '            'xlWorkSheet.Cells.Select()
    '            'xlWorkSheet.Cells.EntireColumn.AutoFit()

    '            ''If ctr > 1 Then
    '            ''xlWorkSheet.Range("A2:C" & (ctr - 1)).Select()
    '            ''xlsApp.Selection.Sort(Key1:=xlWorkSheet.Range("B2"), Order1:=Excel.XlSortOrder.xlAscending, Header:=Excel.XlYesNoGuess.xlGuess, OrderCustom:=1, MatchCase:=False, Orientation:=Excel.Constants.xlTopToBottom)
    '            ''End If

    '            'xlWorkSheet.Range("E1").Select()


    '            xlsApp.DisplayAlerts = False
    '            xlsWB.SaveAs(checkFile, 1) ' 1== xls

    '            WriteEmail("CHECK")
    '        Catch ex As Exception
    '            MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical, "Oops")
    '        Finally
    '            If xlsWB IsNot Nothing Then
    '                xlsWB.Close()
    '            End If
    '            xlsWB = Nothing

    '            If xlsApp IsNot Nothing Then
    '                xlsApp.Quit()
    '            End If
    '            xlsApp = Nothing

    '            If conn IsNot Nothing Then
    '                conn.Close()
    '            End If
    '        End Try
    '    End Sub

    'End Class
End Namespace
