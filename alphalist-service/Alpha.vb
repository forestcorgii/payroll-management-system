Imports System.IO
Imports System.Windows.Forms
Imports NPOI.HSSF.UserModel
Imports NPOI.SS.UserModel

Public Class Alpha
    Private Const V As String = ","

    Private Minrate As Double = 0
    Private Oldminrate As Double = 0
    Private Perdayfields As String = "D.ID,D.CODE,D.REG_HRS, D.R_OT, D.RD_OT,D.HOL_OT, D.ND, D.ADJUST1,D.GROSS_PAY,D.ADJUST2,D.TAX,CAST(D.SSS_EE as Numeric(10,2)), CAST(D.SSS_ER as Numeric(10,2)),D.PHIC,D.NET_PAY,D.REG_PAY,M.RATE,M.STATUS,M.LASTNAME,M.FIRSTNAME,D.RD_8,D.HOL_OT8"

    Private KSfield As String = "D.ID,D.RECS1,D.KS1,D.HOURS1,D.GROSS_PAY1,D.ND1,D.NET_PAY1,D.RECS2,D.KS2,D.HOURS2,D.GROSS_PAY2,D.ND2,D.NET_PAY2,D.PAGIBIG_EE,D.PAGIBIG_ER,D.SSS_EE, CAST(D.SSS_ER as Numeric(10,2)),D.TAX,M.ID,M.LASTNAME,M.FIRSTNAME,M.CHAR"

    Private payDates As String() ' = New String() {"0120", "0220", "0320", "0420", "0520", "0620", "0720", "0820", "0920", "1020", "1120", "1220", "1320"}

    Private rawfinalout As New List(Of List(Of FinalOutCls))
    Dim path As String = Nothing
    'Dim tempfinal As New List(Of FinalOutCls)
    Dim finalout As New List(Of FinalOutCls)
    Private xlspath As String

    Public EEDataFile As String
    Public OutputFile As String

    Public Site As String = "Leyte"
    Public Paycode As String = "Per Day"
    Public Company As String = "FPOSI"
    Public EndOfYearPeriod As String
    Public RegionNumber As String
    Public DestinationFolder As String
    Public RegisteredName As String
    Public EmployerTIN As String
    Public BranchCode As String



    Public TargetYear As String = "20"



    Public PerdayFolders As New List(Of String)
    Public KSFolders As New List(Of String)
    Public Function Valid() As Boolean
        Return True
    End Function

    Private Sub collectDates(targetYear As String)
        payDates = New String() {
            "01" & targetYear & "-",
            "02" & targetYear & "-",
            "03" & targetYear & "-",
            "04" & targetYear & "-",
            "05" & targetYear & "-",
            "06" & targetYear & "-",
            "07" & targetYear & "-",
            "08" & targetYear & "-",
            "09" & targetYear & "-",
            "10" & targetYear & "-",
            "11" & targetYear & "-",
            "12" & targetYear & "-"
        }
    End Sub

    Private Function validateDBFFolder(payFolder As String, ByRef master_initial As String, ByRef alldate As String) As Boolean
        'CHECK DATES
        collectDates(EndOfYearPeriod.Substring(2))
        Dim temptdate() As String = Directory.GetFiles(payFolder, "*.dbf")
        If temptdate.Length < 50 Then
            'MessageBox.Show("Some DBF/s are Missing", "Error in " & New DirectoryInfo(payFolder).Name, MessageBoxButtons.OK, MessageBoxIcon.Error)
            'Return False
        End If

        'GET DATES
        For Each d In temptdate
            Dim tempstr As FileInfo = New FileInfo(d)
            Dim temparr() As String = tempstr.Name.Split("-")
            If tempstr.Name.ToUpper.Contains("ALL") And temparr(0).ToUpper.Contains("D") Then 'SEAN changed temparr(2) to tempstr
                alldate = tempstr.Name 'temparr(1)
            End If
        Next

        'GET MASTER INITIAL
        For Each d In temptdate
            Dim tempstr As FileInfo = New FileInfo(d)
            Dim temparr() As String = tempstr.Name.Split("-")
            If temparr(0).ToUpper.Contains("M") Then
                master_initial = temparr(0)
                Exit For
            ElseIf temparr(0).ToUpper.Contains("N") Then
                master_initial = temparr(0)
                Exit For
            End If
        Next
        Return True
    End Function

    Private Sub runKS()
        For Each payFolder As String In KSFolders

            Dim connectionString As String = "Provider=vfpoledb;" & "Data Source=" & payFolder & ";Extended Properties=dBase IV"
            Using olecon As New OleDb.OleDbConnection(connectionString)
                olecon.Open()

                Dim alldate = ""
                Dim master_initial As String = ""
                Dim tempfinal As New List(Of FinalOutCls)

                If validateDBFFolder(payFolder, master_initial, alldate) Then
                    Dim employees = GetEmployeeList(payDates, olecon)

                    'GET INFORMATIONS SUCH AS OT,RATES,CONTRIBS, ETC.
                    For Each dt In payDates
                        For i = 1 To 2
                            Dim details As String = "D-" & dt & i
                            Dim master As String = master_initial & "-" & dt & i ';
                            Try
                                GetKSEmpInfo(details, master, employees, KSfield, olecon)
                            Catch
                            End Try
                        Next
                    Next

                    'GET INFO FROM 13TH
                    'StatusLabel.Text = "Collecting Nontax13th. . ."
                    GetKSNontax13th(alldate, employees, olecon) '"D-" & alldate & "-ALL"

                    'GET INFO FROM EEDATA
                    'StatusLabel.Text = "Collecting From EEData. . ."
                    readEEdata(employees)

                    KSComputation(employees, tempfinal)
                    rawfinalout.Add(tempfinal)
                End If

                olecon.Close()
            End Using

        Next
    End Sub
    Private Sub runPerDay()
        For Each payFolder As String In PerdayFolders

            Dim connectionString As String = "Provider=VFPOLEDB;" & "Data Source=" & payFolder & ";Extended Properties=dBase IV"
            Using olecon As New OleDb.OleDbConnection(connectionString)
                olecon.Open()
                'StatusLabel.Text = "Processing" & payFolder & ". . ."
                Dim alldate = ""
                Dim master_initial As String = ""
                Dim tempfinal As New List(Of FinalOutCls)
                If validateDBFFolder(payFolder, master_initial, alldate) Then
                    Dim employees = GetEmployeeList(payDates, olecon)

                    'GET INFORMATIONS SUCH AS OT,RATES,CONTRIBS, ETC.
                    For Each dt In payDates
                        For i = 1 To 2
                            Dim details As String = "D-" & dt & i
                            Dim master As String = master_initial & "-" & dt & i ';
                            'Application.DoEvents()
                            Try
                                GetPerDayEmpInfo(details, master, employees, Perdayfields, olecon)
                            Catch ex As Exception
                                'MsgBox(String.Format("{0} {1} {2}", details, " ERROR ", ex.Message))
                                Console.WriteLine(details & " ERROR " & ex.Message)
                                Console.WriteLine()
                            End Try
                        Next
                    Next

                    ''GET INFO FROM 13TH
                    'StatusLabel.Text = "Collecting Nontax13th. . ."
                    GetPerdayNontax13th(alldate, employees, olecon) '"D-" & alldate & "-ALL"

                    'GET INFO FROM EEDATA
                    'StatusLabel.Text = "Collecting From EEData. . ."
                    readEEdata(employees)

                    PerdayComputation(employees, tempfinal)
                    rawfinalout.Add(tempfinal)
                End If

                olecon.Close()
            End Using

        Next
    End Sub

    Public Sub Start()
        xlspath = Site & "-BIR-" & Now.ToString("MMddyyyy")

        If Valid() Then

            Select Case Site
                Case "Manila"
                    Oldminrate = 0
                    Minrate = 67.13
                Case "Batangas"
                    Oldminrate = 0
                    Minrate = 46.63
                Case "Leyte"
                    Oldminrate = 39.38
                    Minrate = 40.63
            End Select

            finalout = New List(Of FinalOutCls)

            runPerDay()

            runKS()
            'StatusLabel.Text = "Merging "
            finalout = Merge(rawfinalout)

            'check for duplicate
            Dim dups = (From res In finalout Group By res.Fullname Into dps = Group Where dps.Count > 1 Order By dps.Count Descending Select dps.ToList).ToList

            For i As Integer = 0 To dups.Count - 1
                For j As Integer = 1 To dups(i).Count - 1
                    With dups(i)(0)
                        .JanNov += dups(i)(j).JanNov
                        .Advances += dups(i)(j).Advances
                        .Benefits += dups(i)(j).Benefits
                        .December += dups(i)(j).December
                        .GrossComp += dups(i)(j).GrossComp
                        .holiday += dups(i)(j).holiday
                        .NDiff += dups(i)(j).NDiff
                        .Nontax13th += dups(i)(j).Nontax13th
                        .NonTaxSalary += dups(i)(j).NonTaxSalary
                        .Overtime += dups(i)(j).Overtime
                        .perday += dups(i)(j).perday
                        .permonth += dups(i)(j).permonth
                        .peryear += dups(i)(j).peryear
                        .Refund += dups(i)(j).Refund
                        .TaxDue += dups(i)(j).TaxDue
                        .TotalIncome += dups(i)(j).TotalIncome
                        .WithHeld += dups(i)(j).WithHeld
                        .final += .final
                    End With
                Next
            Next

            For i As Integer = 0 To dups.Count - 1
                For j As Integer = 0 To dups(i).Count - 1
                    For Each finaloutItem In finalout
                        If finaloutItem.ID = dups(i)(j).ID AndAlso j = 0 Then
                            'edit count
                            finaloutItem = dups(i)(0)
                            Exit For
                        ElseIf finaloutItem.ID = dups(i)(j).ID AndAlso j > 0 Then
                            'delete
                            finalout.Remove(finaloutItem)
                            Exit For
                        End If
                    Next
                Next
            Next

            OutputCSV(finalout, OutputFile)

            MsgBox("Done", MsgBoxStyle.Information)
            'StatusLabel.Text = "Not Running."

        End If
    End Sub

    Private Function GetValue(sheet As ISheet, rowIdx As Integer, colIdx As Integer) As String
        Dim row = sheet.GetRow(rowIdx)
        If row IsNot Nothing Then
            Dim cell = row.GetCell(colIdx)
            If cell IsNot Nothing Then
                Return cell.ToString
            End If
        End If
        Return ""
    End Function

    Public Sub readEEdata(ByRef emplist As List(Of EmployeeList))
        Dim da As HSSFWorkbook
        Using fs As FileStream = New FileStream(EEDataFile, FileMode.Open, FileAccess.Read)
            da = New HSSFWorkbook(fs)
        End Using

        Dim sheet As ISheet = da.GetSheetAt(0)
        Dim appender As Int16 = 2
        If GetValue(sheet, 0, 0).ToUpper.Contains("ID") Or GetValue(sheet, 1, 0).ToUpper.Contains("ID") Then 'A
            appender = 0
        ElseIf GetValue(sheet, 0, 1).ToUpper.Contains("ID") Or GetValue(sheet, 1, 0).ToUpper.Contains("ID") Then 'A
            appender = 1
        Else
            MsgBox("Can't find header.")
        End If
        If appender = 2 Then MsgBox("Invalid EE Data.")
        For Each emp In emplist

            For row = 2 To sheet.LastRowNum
                With emp
                    Try
                        Dim tempid As String = GetValue(sheet, row, 0 + appender).Trim 'A
                        If Not IsNothing(tempid) Then
                            If .id.ToUpper = tempid Then
                                .MIDDLENAME = GetValue(sheet, row, 4 + appender) 'E
                                .EXT = GetValue(sheet, row, 5 + appender) 'F
                                .TIN = GetValue(sheet, row, 6 + appender) 'G
                                Exit For
                            End If
                        End If
                    Catch ex As Exception
                        MessageBox.Show("Employee ID: " & emp.id & " is not found in EE Data.")
                    End Try
                End With
            Next
        Next

    End Sub

    Public Function GetEmployeeList(ByVal dates As String(), olecon As OleDb.OleDbConnection) As List(Of EmployeeList)
        GetEmployeeList = New List(Of EmployeeList)
        Dim IDarr As New List(Of String)
        For Each dt In dates
            For i = 1 To 2
                Dim olecom As New OleDb.OleDbCommand
                Dim reader As OleDb.OleDbDataReader
                Try
                    olecom = New OleDb.OleDbCommand("Select ID from " & "D-" & dt & i, olecon)
                    reader = olecom.ExecuteReader
                Catch ex As Exception
                    ' MessageBox.Show(ex.Message, "Error" & New DirectoryInfo(olecom.Connection.DataSource).Name, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Continue For
                End Try

                While reader.Read
                    IDarr.Add(reader(0).ToString)
                End While
            Next
            IDarr = IDarr.Distinct().ToList
        Next

        For Each id In IDarr
            Dim temp As New EmployeeList
            With temp
                .id = id
                .name = FindName(dates.ToList, id, olecon)
            End With
            GetEmployeeList.Add(temp)
        Next

        Return GetEmployeeList
    End Function

    Public Function FindName(ByVal dates As List(Of String), ByVal id As String, olecon As OleDb.OleDbConnection) As String
        For Each dt In dates
            For i = 1 To 2
                Dim olecom2 As New OleDb.OleDbCommand
                Dim reader2 As OleDb.OleDbDataReader
                Try
                    olecom2 = New OleDb.OleDbCommand("Select LASTNAME,FIRSTNAME from " & "M-" & dt & i & " WHERE ID='" & id & "'", olecon)
                    reader2 = olecom2.ExecuteReader
                Catch ex As Exception
                    Try
                        olecom2 = New OleDb.OleDbCommand("Select LASTNAME,FIRSTNAME from " & "N-" & dt & i & " WHERE ID='" & id & "'", olecon)
                        reader2 = olecom2.ExecuteReader
                    Catch ex2 As Exception
                        '    MessageBox.Show("Error: " & ex2.Message, "Error in " & New DirectoryInfo(olecom2.Connection.DataSource).Name, MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Continue For
                    End Try
                End Try

                If reader2.HasRows Then
                    While reader2.Read
                        Return reader2(0).ToString.Trim & "," & reader2(1).ToString.Trim
                    End While
                End If
            Next
        Next
        Return ""
    End Function
    Public Sub GetKSEmpInfo(ByVal details As String, ByVal master As String, ByRef emlist As List(Of EmployeeList), ByRef fields As String, olecon As OleDb.OleDbConnection)
        Dim olecom As New OleDb.OleDbCommand
        olecom = New OleDb.OleDbCommand("Select " & fields & " from " & details & " D INNER JOIN " & master & " M ON D.ID = M.ID ORDER BY M.LASTNAME;", olecon)
        Dim reader As OleDb.OleDbDataReader
        reader = olecom.ExecuteReader
        Dim dts() As String = details.Split("-")
        Dim tempdts As Integer = Val(dts(1).ToString.Substring(0, 2))
        While reader.Read
            For Each emp In emlist
                With emp
                    If emp.id.ToString.ToUpper.Trim = reader(0).ToString.ToUpper.Trim Then
                        If Val(dts(2).ToString) = 2 Then
                            .KSRATE = 39.38 'NOT SURE
                            '.KSID += CDbl(reader(0).ToString)
                            .KSRECS1 += CDbl(reader(1).ToString)
                            .KSKS1 += CDbl(reader(2).ToString)
                            .KSHOURS1 += CDbl(reader(3).ToString)
                            .KSND1 += CDbl(reader(5).ToString)
                            .KSNETPAY1 += CDbl(reader(6).ToString)
                            .KSRECS2 += CDbl(reader(7).ToString)
                            .KSKS2 += CDbl(reader(8).ToString)
                            .KSHOURS2 += CDbl(reader(9).ToString)
                            '.KSGROSSPAY2 += CDbl(reader(10).ToString)
                            .KSND2 += CDbl(reader(11).ToString)
                            .KSNETPAY2 += CDbl(reader(12).ToString)
                            .KSPAGIBIGEE += CDbl(reader(13).ToString)
                            .KSPAGIBIGER += CDbl(reader(14).ToString)
                            .KSSSSEE += CDbl(reader(15).ToString)
                            .KSSSSER += CDbl(reader(16).ToString)

                            If tempdts < 12 Then
                                .KSJANNOV += CDbl(reader(17).ToString)
                            Else
                                .KSDECEMBER += CDbl(reader(17).ToString)
                            End If

                        End If
                        If dts(2).Trim = 1 Then
                            If IsNothing(.KSSTARTDATE) Then
                                Dim month As String = dts(1).Substring(0, 2)
                                Dim year As String = "20" & dts(1).Substring(2, 2)
                                Dim day As String = 1
                                Dim wholedate As String = month & "/" & day & "/" & year
                                .KSSTARTDATE = wholedate
                            End If
                        Else
                            If IsNothing(.KSSTARTDATE) Then
                                Dim month As String = dts(1).Substring(0, 2)
                                Dim year As String = "20" & dts(1).Substring(2, 2)
                                Dim day As String = 30
                                Dim wholedate As String = month & "/" & day & "/" & year
                                .KSSTARTDATE = wholedate
                            End If
                        End If
                        If dts(2).Trim = 1 Then
                            .KSGROSSPAY += CDbl(reader(4).ToString)
                        ElseIf dts(2).Trim = 2 Then
                            .KSGROSSPAY += CDbl(reader(10).ToString)
                        End If
                        If dts(2).Trim = 1 Then
                            Dim month As String = dts(1).Substring(0, 2)
                            Dim year As String = "20" & dts(1).Substring(2, 2)
                            Dim day As String = 1
                            Dim wholedate As String = month & "/" & day & "/" & year
                            .KSENDDATE = wholedate
                        Else
                            Dim month As String = dts(1).Substring(0, 2)
                            Dim year As String = "20" & dts(1).Substring(2, 2)
                            Dim day As String = 30
                            Dim wholedate As String = month & "/" & day & "/" & year
                            .KSENDDATE = wholedate
                        End If
                    End If
                End With
            Next
        End While
    End Sub
    Public Sub GetPerDayEmpInfo(ByVal details As String, ByVal master As String, ByRef emlist As List(Of EmployeeList), ByRef fields As String, olecon As OleDb.OleDbConnection)
        Dim olecom As New OleDb.OleDbCommand
        olecom = New OleDb.OleDbCommand("Select " & fields & " from " & details & " D INNER JOIN " & master & " M ON D.ID = M.ID ORDER BY M.LASTNAME;", olecon)
        Dim reader As OleDb.OleDbDataReader
        reader = olecom.ExecuteReader
        Dim dts() As String = details.Split("-")
        Dim tempdts As Integer = Val(dts(1).ToString.Substring(0, 2))
        While reader.Read
            Try

                For i As Integer = 0 To emlist.Count - 1 'Each emp In emlist
                    Dim emp As EmployeeList = emlist(i)
                    If emp.id.ToString.ToUpper.Trim = "DMS1" And reader(0).ToString.ToUpper.Trim = "DMS1" Then
                        Console.WriteLine()
                    End If
                    With emp
                        If emp.id.ToString.ToUpper.Trim = reader(0).ToString.ToUpper.Trim Then
                            .Code = reader(1).ToString
                            If Val(dts(2).ToString) = 2 Then
                                .Reg_Hrs += CDbl(reader(2).ToString)
                                .R_OT += CDbl(reader(3).ToString)
                                .RD_OT += CDbl(reader(4).ToString)
                                .HOL_OT += CDbl(reader(5).ToString)
                                .ND += CDbl(reader(6).ToString)
                                If Val(.Code) = 2 Then .ADJUST1 += CDbl(reader(7).ToString) * -1
                                .GROSS_PAY += CDbl(reader(8).ToString)
                                .ADJUST2 += CDbl(reader(9).ToString)
                                .TAX += CDbl(reader(10).ToString)
                                .SSS_EE += CDbl(reader(11).ToString)
                                .SSS_ER += CDbl(reader(12).ToString)
                                .PHIC += CDbl(reader(13).ToString)
                                .NETPAY += CDbl(reader(14).ToString)
                                .REG_PAY += CDbl(reader(15).ToString)
                                .RATE = CDbl(reader(16).ToString)
                                .RD_8 = CDbl(reader(20).ToString)
                                .HOL_OT8 = CDbl(reader(21).ToString)

                            End If
                            .STATUS = reader(17).ToString
                            If dts(2).Trim = 1 Then
                                If IsNothing(.STARTDATE) Then
                                    Dim month As String = dts(1).Substring(0, 2)
                                    Dim year As String = "20" & dts(1).Substring(2, 2)
                                    Dim day As String = 1
                                    Dim wholedate As String = month & "/" & day & "/" & year
                                    .STARTDATE = wholedate
                                End If
                            Else
                                If IsNothing(.STARTDATE) Then
                                    Dim month As String = dts(1).Substring(0, 2)
                                    Dim year As String = "20" & dts(1).Substring(2, 2)
                                    Dim day As String = 16
                                    Dim wholedate As String = month & "/" & day & "/" & year
                                    .STARTDATE = wholedate
                                End If
                            End If

                            If dts(2).Trim = 1 Then
                                Dim month As String = dts(1).Substring(0, 2)
                                Dim year As String = "20" & dts(1).Substring(2, 2)
                                Dim day As String = 1
                                Dim wholedate As String = month & "/" & day & "/" & year
                                .ENDDATE = wholedate
                            Else
                                Dim month As String = dts(1).Substring(0, 2)
                                Dim year As String = "20" & dts(1).Substring(2, 2)
                                Dim day As String = Nothing
                                If month = 2 Then
                                    day = 28
                                Else
                                    day = 30
                                End If
                                Dim wholedate As String = month & "/" & day & "/" & year
                                .ENDDATE = wholedate
                            End If

                            If tempdts < 12 Then
                                .JANNOV += CDbl(reader(10).ToString)
                            Else
                                .DECEMBER += CDbl(reader(10).ToString)
                            End If
                            Exit For
                        End If
                    End With
                Next

            Catch ex As Exception
                MsgBox(String.Format("{0} {1} {2} {3}", reader(0).ToString, details, " ERROR ", ex.Message))
            End Try
        End While
    End Sub

    Public Sub GetKSNontax13th(ByVal Detailallpath As String, ByRef emplist As List(Of EmployeeList), olecon As OleDb.OleDbConnection)
        Dim olecom As New OleDb.OleDbCommand
        olecom = New OleDb.OleDbCommand("SELECT ID,NET_PAY1 FROM " & Detailallpath, olecon)
        Dim reader As OleDb.OleDbDataReader
        Try
            reader = olecom.ExecuteReader
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End Try

        While reader.Read
            For Each emp In emplist
                With emp
                    If reader("ID").ToString.ToUpper.Trim = .id.ToUpper.Trim Then
                        .KSNONTAX13TH = CDbl(reader("NET_PAY1").ToString)
                    End If
                End With
            Next
        End While
    End Sub
    Public Sub GetPerdayNontax13th(ByVal Detailallpath As String, ByRef emplist As List(Of EmployeeList), olecon As OleDb.OleDbConnection)
        Dim olecom As New OleDb.OleDbCommand
        olecom = New OleDb.OleDbCommand("SELECT ID,GROSS_PAY FROM " & Detailallpath, olecon)
        Dim reader As OleDb.OleDbDataReader
        Try
            reader = olecom.ExecuteReader
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End Try

        While reader.Read
            For Each emp In emplist
                With emp
                    If reader("ID").ToString.ToUpper.Trim = .id.ToUpper.Trim Then
                        .NONTAX13TH = CDbl(reader("GROSS_PAY").ToString)
                    End If
                End With
            Next
        End While
    End Sub

    Public Sub Output(ByVal finalout As List(Of FinalOutCls), ByVal path As String)
        Dim xlapp As New Excel.Application
        Dim xlwb As Excel.Workbook
        Dim xlws As Excel.Worksheet

        xlwb = xlapp.Workbooks.Open(path)

        Dim alpha71ctr As Integer = 9
        Dim alpha73ctr As Integer = 9
        Dim alpha75ctr As Integer = 9

        xlws = xlwb.Sheets(1)
        xlws.Range("A1").Value = Company
        xlws = xlwb.Sheets(2)
        xlws.Range("A1").Value = Company
        xlws = xlwb.Sheets(3)
        xlws.Range("A1").Value = Company

        For Each out In finalout
            With out

                StatusLabel.Text = "Writing For Output: " & .ID & "-" & .FirstName & "," & .LastName

                Application.DoEvents()
                If Not Val(.Benefits) = 0 Then
                    If .Rate = Minrate Or .Rate = Oldminrate Then
                        xlws = xlwb.Sheets(3)
                        '7.5 ALPHA
                        xlws.Range("A" & alpha75ctr).Value = .ID
                        xlws.Range("B" & alpha75ctr).Value = .Tin
                        xlws.Range("C" & alpha75ctr).Value = .LastName
                        xlws.Range("D" & alpha75ctr).Value = .FirstName
                        xlws.Range("E" & alpha75ctr).Value = .MiddlesName
                        xlws.Range("V" & alpha75ctr).Value = .DateFrom
                        xlws.Range("W" & alpha75ctr).Value = .DateEnd
                        xlws.Range("X" & alpha75ctr).Value = .GrossComp
                        xlws.Range("Y" & alpha75ctr).Value = .perday
                        xlws.Range("Z" & alpha75ctr).Value = .permonth
                        xlws.Range("AA" & alpha75ctr).Value = .peryear

                        xlws.Range("AB" & alpha75ctr).Value = 313
                        xlws.Range("AC" & alpha75ctr).Value = .holiday.ToString("0.00")
                        xlws.Range("AD" & alpha75ctr).Value = Val(.Overtime + .RD_OT).ToString("0.00")
                        xlws.Range("AE" & alpha75ctr).Value = Val(.NDiff).ToString("0.00")
                        xlws.Range("AG" & alpha75ctr).Value = .Nontax13th
                        xlws.Range("AI" & alpha75ctr).Value = .Benefits
                        xlws.Range("AJ" & alpha75ctr).Value = Val(.NonTaxSalary).ToString("0.00")
                        xlws.Range("AY" & alpha75ctr).Value = "Y"

                        alpha75ctr += 1
                    ElseIf .Rate > Minrate Then
                        Dim tempend() As String = .DateEnd.Split("/")
                        If Val(tempend(0)) = 12 Then
                            '7.3

                            xlws = xlwb.Sheets(1)

                            xlws.Range("A" & alpha73ctr).Value = .ID
                            xlws.Range("B" & alpha73ctr).Value = .Tin
                            xlws.Range("C" & alpha73ctr).Value = .LastName
                            xlws.Range("D" & alpha73ctr).Value = .FirstName
                            xlws.Range("E" & alpha73ctr).Value = .MiddlesName
                            xlws.Range("F" & alpha73ctr).Value = .GrossComp
                            xlws.Range("G" & alpha73ctr).Value = .Nontax13th
                            xlws.Range("I" & alpha73ctr).Value = .Benefits
                            xlws.Range("L" & alpha73ctr).Value = .TotalIncome
                            xlws.Range("N" & alpha73ctr).Value = .TotalIncome
                            xlws.Range("O" & alpha73ctr).Value = .TotalIncome
                            If Not Val(.TaxDue) = 0 Then xlws.Range("S" & alpha73ctr).Value = .TotalIncome
                            xlws.Range("T" & alpha73ctr).Value = .TaxDue
                            xlws.Range("U" & alpha73ctr).Value = .JanNov
                            xlws.Range("V" & alpha73ctr).Value = .Advances
                            xlws.Range("W" & alpha73ctr).Value = .Refund
                            xlws.Range("X" & alpha73ctr).Value = .TaxDue
                            xlws.Range("Y" & alpha73ctr).Value = "Y"
                            xlws.Range("Z" & alpha73ctr).Value = .December
                            xlws.Range("AA" & alpha73ctr).Value = .final * -1

                            alpha73ctr += 1

                        Else
                            '7.1

                            xlws = xlwb.Sheets(2)

                            xlws.Range("A" & alpha71ctr).Value = .ID
                            xlws.Range("B" & alpha71ctr).Value = .Tin
                            xlws.Range("C" & alpha71ctr).Value = .LastName
                            xlws.Range("D" & alpha71ctr).Value = .FirstName
                            xlws.Range("E" & alpha71ctr).Value = .MiddlesName
                            xlws.Range("F" & alpha71ctr).Value = .DateFrom
                            xlws.Range("G" & alpha71ctr).Value = .DateEnd

                            xlws.Range("H" & alpha71ctr).Value = .GrossComp
                            xlws.Range("I" & alpha71ctr).Value = .Nontax13th
                            xlws.Range("K" & alpha71ctr).Value = .Benefits
                            xlws.Range("N" & alpha71ctr).Value = .TotalIncome
                            xlws.Range("P" & alpha71ctr).Value = .TotalIncome
                            xlws.Range("Q" & alpha71ctr).Value = .TotalIncome
                            If Not Val(.TaxDue) = 0 Then xlws.Range("U" & alpha71ctr).Value = .TotalIncome
                            xlws.Range("V" & alpha71ctr).Value = .TaxDue
                            xlws.Range("W" & alpha71ctr).Value = .JanNov
                            xlws.Range("X" & alpha71ctr).Value = .Advances
                            xlws.Range("Y" & alpha71ctr).Value = .Refund
                            xlws.Range("Z" & alpha71ctr).Value = .TaxDue
                            xlws.Range("AA" & alpha71ctr).Value = "Y"
                            xlws.Range("AB" & alpha71ctr).Value = .December
                            xlws.Range("AC" & alpha71ctr).Value = .final * -1

                            alpha71ctr += 1

                        End If
                    End If
                End If
            End With
        Next


        xlwb.Save()
        xlwb.Close()
        xlapp.Quit()

    End Sub

    Public Sub OutPutKS(ByVal finalout As List(Of FinalOutCls), ByVal path As String)
        Dim xlapp As New Excel.Application
        Dim xlwb As Excel.Workbook
        Dim xlws As Excel.Worksheet

        xlwb = xlapp.Workbooks.Open(path)

        Dim alpha71ctr As Integer = 0
        Dim alpha73ctr As Integer = 0
        Dim alpha75ctr As Integer = 0


        Dim payname As New DirectoryInfo(path)
        '7.3
        xlws = xlwb.Sheets(1)
        Dim xlrange As Excel.Range = xlws.UsedRange
        xlws.Range("A1").Value = Company
        alpha73ctr = xlrange.Rows.Count + 1
        '7.1
        xlws = xlwb.Sheets(2)
        xlrange = xlws.UsedRange
        xlws.Range("A1").Value = Company
        alpha71ctr = xlrange.Rows.Count + 1

        '7.5
        xlws = xlwb.Sheets(3)
        xlrange = xlws.UsedRange
        xlws.Range("A1").Value = Company
        alpha75ctr = xlrange.Rows.Count + 1

        For Each recs In finalout
            With recs

                StatusLabel.Text = "Writing For Output: " & .ID & "-" & .FirstName & "," & .LastName

                Application.DoEvents()
                If Not Val(.Benefits) = 0 Then
                    If payname.Name.ToUpper.Trim = "k12a".ToUpper.Trim Then
                        xlws = xlwb.Sheets(3)
                        '7.5 ALPHA
                        xlws.Range("A" & alpha73ctr).Value = .ID
                        xlws.Range("B" & alpha73ctr).Value = .Tin
                        xlws.Range("C" & alpha73ctr).Value = .LastName
                        xlws.Range("D" & alpha73ctr).Value = .FirstName
                        xlws.Range("E" & alpha73ctr).Value = .MiddlesName
                        xlws.Range("F" & alpha73ctr).Value = .GrossComp
                        xlws.Range("G" & alpha73ctr).Value = .Nontax13th
                        xlws.Range("I" & alpha73ctr).Value = .Benefits
                        xlws.Range("L" & alpha73ctr).Value = .TotalIncome
                        xlws.Range("N" & alpha73ctr).Value = .TotalIncome
                        xlws.Range("O" & alpha73ctr).Value = .TotalIncome
                        If Not Val(.TaxDue) = 0 Then xlws.Range("S" & alpha73ctr).Value = .TotalIncome
                        xlws.Range("T" & alpha73ctr).Value = .TaxDue
                        xlws.Range("U" & alpha73ctr).Value = .JanNov
                        xlws.Range("V" & alpha73ctr).Value = .Advances
                        xlws.Range("W" & alpha73ctr).Value = .Refund
                        xlws.Range("X" & alpha73ctr).Value = .TaxDue
                        xlws.Range("Y" & alpha73ctr).Value = "Y"
                        xlws.Range("Z" & alpha73ctr).Value = .December
                        xlws.Range("AA" & alpha73ctr).Value = .final * -1
                        alpha75ctr += 1
                    Else
                        Dim tempend() As String = .DateEnd.Split("/")
                        If Val(tempend(0)) = 12 Then
                            '7.3
                            xlws = xlwb.Sheets(1)
                            xlws.Range("A" & alpha73ctr).Value = .ID
                            xlws.Range("B" & alpha73ctr).Value = .Tin
                            xlws.Range("C" & alpha73ctr).Value = .LastName
                            xlws.Range("D" & alpha73ctr).Value = .FirstName
                            xlws.Range("E" & alpha73ctr).Value = .MiddlesName
                            xlws.Range("F" & alpha73ctr).Value = .GrossComp
                            xlws.Range("G" & alpha73ctr).Value = .Nontax13th
                            xlws.Range("I" & alpha73ctr).Value = .Benefits
                            xlws.Range("L" & alpha73ctr).Value = .TotalIncome
                            xlws.Range("N" & alpha73ctr).Value = .TotalIncome
                            xlws.Range("O" & alpha73ctr).Value = .TotalIncome
                            If Not Val(.TaxDue) = 0 Then xlws.Range("S" & alpha73ctr).Value = .TotalIncome
                            xlws.Range("T" & alpha73ctr).Value = .TaxDue
                            xlws.Range("U" & alpha73ctr).Value = .JanNov
                            xlws.Range("V" & alpha73ctr).Value = .Advances
                            xlws.Range("W" & alpha73ctr).Value = .Refund
                            xlws.Range("X" & alpha73ctr).Value = .TaxDue
                            xlws.Range("Y" & alpha73ctr).Value = "Y"
                            xlws.Range("Z" & alpha73ctr).Value = .December
                            xlws.Range("AA" & alpha73ctr).Value = .final * -1
                            alpha73ctr += 1
                        Else
                            '7.1
                            xlws = xlwb.Sheets(2)
                            xlws.Range("A" & alpha71ctr).Value = .ID
                            xlws.Range("B" & alpha71ctr).Value = .Tin
                            xlws.Range("C" & alpha71ctr).Value = .LastName
                            xlws.Range("D" & alpha71ctr).Value = .FirstName
                            xlws.Range("E" & alpha71ctr).Value = .MiddlesName
                            xlws.Range("F" & alpha71ctr).Value = .DateFrom
                            xlws.Range("G" & alpha71ctr).Value = .DateEnd
                            xlws.Range("H" & alpha71ctr).Value = .GrossComp
                            xlws.Range("I" & alpha71ctr).Value = .Nontax13th
                            xlws.Range("K" & alpha71ctr).Value = .Benefits
                            xlws.Range("N" & alpha71ctr).Value = .TotalIncome
                            xlws.Range("P" & alpha71ctr).Value = .TotalIncome
                            xlws.Range("Q" & alpha71ctr).Value = .TotalIncome
                            If Not Val(.TaxDue) = 0 Then xlws.Range("U" & alpha71ctr).Value = .TotalIncome
                            xlws.Range("V" & alpha71ctr).Value = .TaxDue
                            xlws.Range("W" & alpha71ctr).Value = .JanNov
                            xlws.Range("X" & alpha71ctr).Value = .Advances
                            xlws.Range("Y" & alpha71ctr).Value = .Refund
                            xlws.Range("Z" & alpha71ctr).Value = .TaxDue
                            xlws.Range("AA" & alpha71ctr).Value = "Y"
                            xlws.Range("AB" & alpha71ctr).Value = .December
                            xlws.Range("AC" & alpha71ctr).Value = .final * -1

                            alpha71ctr += 1
                        End If
                    End If
                End If
            End With
        Next


        xlwb.Save()
        xlwb.Close()
        xlapp.Quit()

    End Sub

    Public Sub OutputCSV(ByVal finalout As List(Of FinalOutCls), ByVal path As String)
        ' Dim schedule As String = ""
        Dim d1list As New List(Of ALPHADTL)
        Dim d2list As New List(Of ALPHADTL)
        For i As Integer = 0 To finalout.Count - 1
            Dim out As FinalOutCls = finalout(i)
            If Not Val(out.Benefits) = 0 Then
                Dim csv As New ALPHADTL
                With csv
                    .id = out.ID

                    If out.MiddlesName Is Nothing Then out.MiddlesName = ""
                    If out.Tin Is Nothing Then out.Tin = ""

                    .FIRST_NAME = out.FirstName.Replace("Ñ", "N").Replace("ñ", "n").Replace(".", "")
                    .LAST_NAME = out.LastName.Replace("Ñ", "N").Replace("ñ", "n").Replace(".", "")
                    .MIDDLE_NAME = out.MiddlesName.Replace("Ñ", "N").Replace("ñ", "n").Replace(".", "")
                    .TIN = out.Tin.Replace("-", "")
                    .EMPLOYMENT_FROM = out.DateFrom
                    .EMPLOYMENT_TO = out.DateEnd
                    .NATIONALITY = ""
                    .EMPLOYMENT_STATUS = "R"
                    If out.Rate = Minrate Or (Site = "Leyte" And out.Rate = Oldminrate) Then

                        .ACTUAL_AMT_WTHLD = 0
                        .FACTOR_USED = 313
                        .PRES_TAXABLE_SALARIES = 0 'Val(out.NonTaxSalary).ToString("0.00")
                        .PRES_TAXABLE_13TH_MONTH = 0 'TAXABLE 13th Month & other benefits
                        .PRES_NONTAX_SALARIES = Val(out.NonTaxSalary).ToString("0.00") 'TAXABLE salaries & other forms of comp
                        .PRES_NONTAX_13TH_MONTH = out.Nontax13th
                        .PRES_NONTAX_SSS_GSIS_OTH_CONT = out.Benefits
                        .GROSS_COMP_INCOME = 0 'out.GrossComp
                        .PRES_NONTAX_DE_MINIMIS = 0
                        .PRES_TOTAL_COMP = 0 ' out.GrossComp
                        .PRES_TOTAL_NONTAX_COMP_INCOME = out.GrossComp 'Val(out.NonTaxSalary).ToString("0.00")
                        .PRES_NONTAX_GROSS_COMP_INCOME = out.GrossComp

                        .PRES_NONTAX_BASIC_SMW_DAY = CInt(out.perday.ToString("0.00").Split(".")(0))
                        .PRES_NONTAX_BASIC_SMW_MONTH = CInt(out.permonth.ToString("0.00").Split(".")(0))
                        .PRES_NONTAX_BASIC_SMW_YEAR = CInt(out.peryear.ToString("0.00").Split(".")(0))

                        .PRES_NONTAX_HOLIDAY_PAY = out.holiday.ToString("0.00")
                        .PRES_NONTAX_OVERTIME_PAY = Val(out.Overtime + out.RD_OT).ToString("0.00")
                        .PRES_NONTAX_NIGHT_DIFF = Val(out.NDiff).ToString("0.00")
                        .PRES_NONTAX_HAZARD_PAY = 0
                        .NONTAX_BASIC_SAL = 0 ' Val(out.NonTaxSalary).ToString("0.00")
                        .TAX_BASIC_SAL = 0

                        d2list.Add(csv)
                    Else
                        If out.NonTaxSalary > 250000 Then
                            .GROSS_COMP_INCOME = Val(out.NonTaxSalary).ToString("0.00")
                            .TAX_BASIC_SAL = Val(out.NonTaxSalary).ToString("0.00")
                        Else
                            .PRES_NONTAX_SALARIES = Val(out.NonTaxSalary).ToString("0.00")
                        End If
                        .NONTAX_BASIC_SAL = 0
                        .PRES_TAXABLE_SALARIES = 0

                        .PRES_TAXABLE_13TH_MONTH = 0
                        .PRES_NONTAX_13TH_MONTH = out.Nontax13th
                        .PRES_NONTAX_SSS_GSIS_OTH_CONT = out.Benefits

                        If out.ID = "4E5A" Then
                            Console.WriteLine("")
                        End If

                        .PRES_TAX_WTHLD = out.JanNov
                        .AMT_WTHLD_DEC = out.Advances
                        .OVER_WTHLD = out.Refund
                        .ACTUAL_AMT_WTHLD = IIf(out.TaxDue > out.JanNov, out.JanNov + out.Advances, out.JanNov - out.Refund)

                        .TAX_DUE = out.TaxDue
                        If Not Val(out.TaxDue) = 0 Then .NET_TAXABLE_COMP_INCOME = out.TotalIncome

                        .PRES_NONTAX_DE_MINIMIS = 0

                        .PRES_TOTAL_NONTAX_COMP_INCOME = out.Benefits + out.Nontax13th + out.NonTaxSalary
                        .PRES_NONTAX_GROSS_COMP_INCOME = out.GrossComp

                        .December = out.December
                        .Final = out.final

                        d1list.Add(csv)
                    End If
                End With
            End If
        Next

        d1list.Sort()
        d2list.Sort()

        Using d1wrtr As StreamWriter = New StreamWriter(String.Format("{0}/{1}-{2}-{3}-D1.CSV", OutputFile, Now.ToString("yyyyMMddHH"), Site, Company))
            d1wrtr.WriteLine(ALPHADTL.toCSVRow2Header)
            For i As Integer = 0 To d1list.Count - 1
                d1list(i).toCSVRow2Item(d1wrtr)
            Next
        End Using

        Using d2wrtr As StreamWriter = New StreamWriter(String.Format("{0}/{1}-{2}-{3}-D2.CSV", OutputFile, Now.ToString("yyyyMMddHH"), Site, Company))
            d2wrtr.WriteLine(ALPHADTL.toCSVRow2Header)
            For i As Integer = 0 To d2list.Count - 1
                d2list(i).toCSVRow2Item(d2wrtr)
            Next
        End Using
    End Sub

    Public Sub ConvertSelected(selectedOutput As String())
        Dim connectionString As String = "Provider=vfpoledb;" & "Data Source=" & DestinationFolder & ";Extended Properties=dBase IV"
        Using olecon As New OleDb.OleDbConnection(connectionString)
            olecon.Open()

            Dim olecom As New OleDb.OleDbCommand
            olecom = New OleDb.OleDbCommand("DELETE FROM Alphadtl.DBF;", olecon)
            olecom.ExecuteNonQuery()

            For i As Integer = 0 To selectedOutput.Length - 1
                Dim masterlist As New List(Of ALPHADTL)
                Dim sched As String = New DirectoryInfo(selectedOutput(i)).Name.Split("-")(3).Substring(0, 2)

                If {"D1", "D2"}.Contains(sched) Then
                    MessageBox.Show("Invalid Schedule(D1, D2), please check your filename. ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If

                Using rdr As StreamReader = New StreamReader(selectedOutput(i))
                    rdr.ReadLine()
                    Dim ln As String = rdr.ReadLine
                    Dim delimiter As String = ","
                    If ln.Split(";").Length >= 32 Then
                        delimiter = ";"
                    End If
                    If ln.Split(vbTab).Length >= 32 Then
                        delimiter = vbTab
                    End If
                    While ln IsNot Nothing
                        Dim cls As String() = ln.Replace("'", "").Replace("-", "0").Split(delimiter)
                        If cls(0) = "" Then Exit While
                        Dim d As New ALPHADTL
                        With d
                            .FORM_TYPE = "1604C"
                            .EMPLOYER_BRANCH_CODE = BranchCode
                            .EMPLOYER_TIN = EmployerTIN
                            .RETRN_PERIOD = "12/31/" & EndOfYearPeriod
                            .BRANCH_CODE = "0000"
                            .REGION_NUM = RegionNumber
                            .REGISTERED_NAME = RegisteredName
                            .SUBS_FILING = "Y"
                            .SCHEDULE_NUM = sched

                            .id = cls(0)
                            'If .id = "ACA1" Then
                            '    Console.WriteLine("")
                            'End If

                            .FIRST_NAME = cls(1).Trim
                            .LAST_NAME = cls(2).Trim
                            .MIDDLE_NAME = cls(3).Trim
                            .TIN = cls(4).Trim
                            .EMPLOYMENT_FROM = cls(5)
                            .EMPLOYMENT_TO = cls(6)
                            .ACTUAL_AMT_WTHLD = cls(7)
                            .FACTOR_USED = cls(8)
                            .PRES_TAXABLE_SALARIES = cls(9)
                            .PRES_TAXABLE_13TH_MONTH = cls(10)
                            .PRES_TAX_WTHLD = cls(11)
                            .PRES_NONTAX_SALARIES = cls(12) 'change to PRES_NONTAX_HOLIDAY_PAY + PRES_NONTAX_OVERTIME_PAY + PRES_NONTAX_NIGHT_DIFF + PRES_NONTAX_HAZARD_PAY + PRES_NONTAX_13TH_MONTH + PRES_NONTAX_SSS_GSIS_OTH_CONT
                            .PRES_NONTAX_HOLIDAY_PAY = cls(27)
                            .PRES_NONTAX_OVERTIME_PAY = cls(28)
                            .PRES_NONTAX_NIGHT_DIFF = cls(29)
                            .PRES_NONTAX_HAZARD_PAY = cls(30)
                            .PRES_NONTAX_13TH_MONTH = cls(13) '13th
                            .PRES_NONTAX_SSS_GSIS_OTH_CONT = cls(14)
                            .OVER_WTHLD = cls(15)
                            .AMT_WTHLD_DEC = cls(16)
                            .NET_TAXABLE_COMP_INCOME = cls(18)

                            If sched = "D1" Then
                                Dim presNonTax As Double = Double.Parse(cls(12))
                                Dim taxBasicSal As Double = Double.Parse(cls(32)) 'cls(19))
                                .GROSS_COMP_INCOME = taxBasicSal + .PRES_TAXABLE_13TH_MONTH 'cls(19)
                                .PRES_TOTAL_COMP = taxBasicSal + .PRES_TAXABLE_13TH_MONTH 'cls(21) ' change to GROSS_COMP_INCOME + PRES_TAXABLE_13TH_MONTH
                                .PRES_TOTAL_NONTAX_COMP_INCOME = .PRES_NONTAX_13TH_MONTH + .PRES_NONTAX_SSS_GSIS_OTH_CONT + presNonTax 'cls(22) 'change to PRES_NONTAX_13TH_MONTH + PRES_NONTAX_SSS_GSIS_OTH_CONT
                                .PRES_NONTAX_GROSS_COMP_INCOME = .PRES_TOTAL_NONTAX_COMP_INCOME + .PRES_TOTAL_COMP 'cls(23) 'change to PRES_TOTAL_NONTAX_COMP_INCOME + PRES_TOTAL_COMP
                                .TAX_DUE = Computetax(.PRES_NONTAX_GROSS_COMP_INCOME - .PRES_TOTAL_NONTAX_COMP_INCOME) 'cls(17) 'change to ComputeTax(PRES_NONTAX_GROSS_COMP_INCOME-PRES_TOTAL_NONTAX_COMP_INCOME) 
                            Else
                                .GROSS_COMP_INCOME = cls(19)
                                .PRES_TOTAL_COMP = cls(21) ' change to GROSS_COMP_INCOME + PRES_TAXABLE_13TH_MONTH
                                .PRES_TOTAL_NONTAX_COMP_INCOME = cls(22) 'change to PRES_NONTAX_13TH_MONTH + PRES_NONTAX_SSS_GSIS_OTH_CONT
                                .PRES_NONTAX_GROSS_COMP_INCOME = cls(23) 'change to PRES_TOTAL_NONTAX_COMP_INCOME + PRES_TOTAL_COMP
                                .TAX_DUE = cls(17) 'change to ComputeTax(PRES_NONTAX_GROSS_COMP_INCOME-PRES_TOTAL_NONTAX_COMP_INCOME) 
                            End If
                            .PRES_NONTAX_DE_MINIMIS = cls(20)
                            .PRES_NONTAX_BASIC_SMW_DAY = cls(24)
                            .PRES_NONTAX_BASIC_SMW_MONTH = cls(25)
                            .PRES_NONTAX_BASIC_SMW_YEAR = cls(26)

                            .NONTAX_BASIC_SAL = cls(31)
                            .TAX_BASIC_SAL = cls(32)
                            .NATIONALITY = cls(33)
                            .REASON_SEPARATION = cls(34)
                            .EMPLOYMENT_STATUS = cls(35)
                        End With
                        If d.id = "D717" Then
                            Console.WriteLine("")
                        End If

                        masterlist.Add(d)
                        ln = rdr.ReadLine
                    End While
                End Using

                masterlist.Sort()
                For j As Integer = 0 To masterlist.Count - 1
                    masterlist(j).SEQUENCE_NUM = j + 1
                    Dim da As String = masterlist(j).ToInsertStatement("Alphadtl.DBF")
                    olecom = New OleDb.OleDbCommand(da, olecon)
                    olecom.ExecuteNonQuery()
                Next
            Next

            olecon.Close()
        End Using


        MsgBox("Done", MsgBoxStyle.Information)
    End Sub

    Public Sub KSComputation(ByVal Emplist As List(Of EmployeeList), ByRef finalout As List(Of FinalOutCls))
        For Each emp In Emplist
            Dim tempfinal As New FinalOutCls
            Dim name() As String = emp.name.Split(",")
            With tempfinal
                .ID = emp.id
                .company = Company
                .Tin = emp.TIN
                .Rate = emp.RATE
                .FirstName = name(1).ToString
                .LastName = name(0).ToString
                .MiddlesName = emp.MIDDLENAME

                .Nontax13th = emp.KSNONTAX13TH
                .DateFrom = emp.KSSTARTDATE 'FROM
                .DateEnd = emp.KSENDDATE 'TO
                .GrossComp = emp.KSGROSSPAY + emp.KSNONTAX13TH 'GROSS COMP
                .perday = Minrate * 8 'PER DAY
                .permonth = Convert.ToInt64(.perday * 24)
                .peryear = "168081.00" 'PER YEAR
                .Nontax13th = emp.KSNONTAX13TH
                .Benefits = emp.KSSSSEE + emp.KSPAGIBIGEE 'SSS PAGIBIG PHILHEALT
                .NonTaxSalary = emp.KSGROSSPAY - .Benefits 'NONTAX SALARIES
                .TotalIncome = .GrossComp - .Benefits
                .TotalIncome = .TotalIncome - .Nontax13th 'TOTAL INCOME
                Dim taxduecomp As Double = .Benefits + .Nontax13th
                taxduecomp = .GrossComp - taxduecomp
                .TaxDue = Computetax(taxduecomp) 'TAX DUE

                .JanNov = emp.KSJANNOV 'JAN-NOV
                .December = emp.KSDECEMBER 'DECEMBER

                If .TaxDue = 0 And Not .JanNov = 0 Then
                    .Refund = .JanNov 'REFUND
                Else
                    If .TaxDue > .JanNov Then
                        .Advances = .TaxDue - .JanNov 'ADVANCES
                    Else
                        .Refund = .JanNov - .TaxDue 'REFUND
                    End If
                End If

                .final = emp.KSJANNOV + emp.KSDECEMBER
                .final = .final - .TaxDue 'FINAL

            End With
            finalout.Add(tempfinal)
        Next
    End Sub
    Public Sub PerdayComputation(ByVal Emplist As List(Of EmployeeList), ByRef finalout As List(Of FinalOutCls))
        For Each emp In Emplist
            Dim tempfinal As New FinalOutCls
            Dim name() As String = emp.name.Split(",")

            With tempfinal
                .ID = emp.id
                .company = Company
                .Tin = emp.TIN
                .Rate = emp.RATE
                .FirstName = name(1).ToString
                .LastName = name(0).ToString
                .MiddlesName = emp.MIDDLENAME
                .DateFrom = emp.STARTDATE
                .DateEnd = emp.ENDDATE

                .Nontax13th = emp.NONTAX13TH
                .GrossComp = emp.GROSS_PAY + emp.NONTAX13TH
                .Benefits = emp.SSS_EE + emp.PHIC + emp.ADJUST1
                .TotalIncome = .GrossComp - .Benefits
                .TotalIncome = .TotalIncome - emp.NONTAX13TH
                Dim taxduecomp As Double = .Benefits + .Nontax13th
                taxduecomp = .GrossComp - taxduecomp
                .TaxDue = Computetax(taxduecomp) 'TAX DUE
                .December = emp.DECEMBER
                .JanNov = emp.JANNOV
                .final = emp.JANNOV + emp.DECEMBER
                .final = .final - .TaxDue
                .perday = emp.RATE * 8
                .permonth = CInt(.perday.ToString("0.00").Split(".")(0)) * 24
                .peryear = .permonth * 12
                .holiday = emp.RATE * 2.0
                .holiday = .holiday * emp.HOL_OT
                .Overtime = emp.RATE * 1.25
                .Overtime = .Overtime * emp.R_OT
                .RD_OT = emp.RATE * 1.3
                .RD_OT = .RD_OT * emp.RD_OT
                .NDiff = emp.RATE * 0.1
                .NDiff = .NDiff * emp.ND

                If .Rate = Minrate Or .Rate = Oldminrate Then
                    .NonTaxSalary = .GrossComp - .holiday
                    .NonTaxSalary -= .RD_OT
                    .NonTaxSalary -= .Overtime
                    .NonTaxSalary -= .NDiff
                    .NonTaxSalary -= .Nontax13th
                    .NonTaxSalary -= .Benefits
                Else
                    .NonTaxSalary = .GrossComp - (.Nontax13th + emp.SSS_EE + emp.ADJUST1 + emp.PHIC)
                End If

                If .TaxDue = 0 And Not .JanNov = 0 Then
                    .Refund = .JanNov 'REFUND
                Else
                    If .TaxDue > .JanNov Then
                        .Advances = .TaxDue - .JanNov 'ADVANCES
                    Else
                        .Refund = .JanNov - .TaxDue 'REFUND
                    End If
                End If





            End With
            finalout.Add(tempfinal)
        Next
    End Sub

    Public Function Merge(ByVal finallist As List(Of List(Of FinalOutCls))) As List(Of FinalOutCls)
        Dim result As New List(Of FinalOutCls)

        'LOOP FROM RAW RECORD
        For Each out In finallist
            For Each tmp In out
                With tmp

                    'FROM ADDED RECORD
                    Dim Isdup As Boolean = False
                    For Each res In result
                        With res
                            If tmp.ID.ToUpper.Trim = .ID.ToUpper.Trim Then
                                'If tmp.ID = "A883" Then
                                '    Console.WriteLine("")
                                'End If
                                'increment

                                Dim tmpfrmdt() As String = tmp.DateFrom.Split("/")
                                Dim tmpendt() As String = tmp.DateEnd.Split("/")
                                Dim resdtfrom() As String = res.DateFrom.Split("/")
                                Dim resdtend() As String = res.DateEnd.Split("/")

                                If Val(tmpfrmdt(0)) < Val(resdtfrom(0)) Then
                                    res.DateFrom = tmp.DateFrom
                                End If
                                If Val(tmpendt(0)) > Val(resdtend(0)) Then
                                    res.DateEnd = tmp.DateEnd
                                    res.Rate = tmp.Rate
                                    res.perday = tmp.Rate * 8
                                    res.permonth = CInt(.perday.ToString("0.00").Split(".")(0)) * 24
                                    res.peryear = .permonth * 12
                                End If

                                res.GrossComp += tmp.GrossComp
                                res.Nontax13th += tmp.Nontax13th
                                res.Benefits += tmp.Benefits
                                res.TotalIncome += tmp.TotalIncome
                                res.TaxDue += tmp.TaxDue
                                res.JanNov += tmp.JanNov
                                res.Advances += tmp.Advances
                                res.Refund += tmp.Refund
                                res.December += tmp.December
                                res.final += tmp.final
                                res.holiday += tmp.holiday
                                res.Overtime += tmp.Overtime
                                res.RD_OT += tmp.RD_OT
                                res.NDiff += tmp.NDiff
                                res.NonTaxSalary += tmp.NonTaxSalary
                                Isdup = True
                                Exit For
                            End If
                        End With
                    Next
                    If Isdup = False Then
                        result.Add(tmp)
                    End If

                End With
            Next
        Next

        Return result
    End Function

    Public Sub Review(ByRef Finalout As List(Of FinalOutCls), ByVal path As String)
        Dim xlapp As New Excel.Application
        Dim xlwb As Excel.Workbook
        Dim xlws As Excel.Worksheet

        xlwb = xlapp.Workbooks.Open(path)
        xlws = xlwb.Sheets(3)
        Dim rowctr As Integer = 9

        Dim xlrange As Excel.Range = xlws.UsedRange
        For x = 0 To xlrange.Rows.Count - 1
            For Each out In Finalout
                With out
                    Try
                        If xlws.Range("A" & rowctr).Value IsNot Nothing Then
                            If xlws.Range("A" & rowctr).Value.ToString.ToUpper.Trim = .ID.ToUpper.Trim Then
                                .GrossComp += Val(xlws.Range("X" & rowctr).Value)
                                .Nontax13th += Val(xlws.Range("AG" & rowctr).Value)
                                .Benefits += Val(xlws.Range("AI" & rowctr).Value)
                                .NonTaxSalary += Val(xlws.Range("AJ" & rowctr).Value)
                                xlws.Rows(rowctr).Delete()
                            End If
                        End If
                    Catch ex As Exception
                        MessageBox.Show("Error: " & ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Continue For
                    End Try
                End With
            Next
        Next

        xlwb.Save()
        xlwb.Close()
        xlapp.Quit()

    End Sub


    Public Function Computetax(ByVal taxSal As Double) As Double
        Dim temptax As Double = New Double

        Select Case taxSal
            Case 250000 To 399999
                temptax = taxSal - 250000
                temptax = temptax * 0.2
            Case 400000 To 799999
                temptax = taxSal - 400000
                temptax = temptax * 0.25
                temptax = temptax + 30000
            Case 800000 To 1999999
                temptax = taxSal - 800000
                temptax = temptax * 0.3
                temptax = temptax + 130000
            Case 2000000 To 7999999
                temptax = taxSal - 2000000
                temptax = temptax * 0.32
                temptax = temptax + 490000
            Case Is > 8000000
                temptax = taxSal - 8000000
                temptax = temptax * 0.35
                temptax = temptax + 2410000
            Case Else
                temptax = 0
        End Select


        Return temptax
    End Function





End Class