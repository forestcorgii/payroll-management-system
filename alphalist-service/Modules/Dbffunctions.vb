Imports System.IO
Imports System.Data.Sql
Imports System.Data.OleDb
Imports System.Text.RegularExpressions

Module Dbffunctions

    Public Function IfExist(ByVal id As String) As Boolean
        Dim ds As New DataSet

        IfExist = False

        masterConnection = New OleDb.OleDbConnection(mdbconstring)
        masterConnection.Open()
        'masterDA.SelectCommand = New OleDb.OleDbCommand("SELECT * FROM(PayRegTbl) WHERE ID='" & id & "' AND DATER NOT LIKE '%13%'", masterConnection)
        masterDA.SelectCommand = New OleDb.OleDbCommand("SELECT * FROM(PayRegTbl) WHERE ID='" & id & "'", masterConnection)
        masterDA.Fill(ds, "PayRegTbl")

        If ds.Tables("PayRegTbl").Rows.Count > 0 Then
            IfExist = True
        End If

        masterConnection.Close()

    End Function

    Public Function getstartdate(ByVal ID As String) As String
        Dim startdate As String = Nothing
        Using sqlcon As New OleDbConnection(mdbconstring)
            sqlcon.Open()
            Dim com As New OleDbCommand("SELECT MIN(DBDATE) AS START_DATE FROM PayRegTbl WHERE ID='" & ID & "';", sqlcon)
            Dim adpt As New OleDbDataAdapter(com)
            Dim dt As New DataTable
            adpt.Fill(dt)

            startdate = dt.Rows(0)(0).ToString
            Return startdate

        End Using

    End Function

    Public Function getenddate(ByVal ID As String) As String
        Dim enddate As String = Nothing
        Using sqlcon As New OleDbConnection(mdbconstring)
            sqlcon.Open()
            Dim com As New OleDbCommand("SELECT MAX(DBDATE) AS START_DATE FROM PayRegTbl WHERE ID='" & ID & "';", sqlcon)
            Dim adpt As New OleDbDataAdapter(com)
            Dim dt As New DataTable
            adpt.Fill(dt)

            enddate = dt.Rows(0)(0).ToString
            Return enddate

        End Using
    End Function

    Public Function getgross(ByVal ID As String) As Double
        Dim grosscom As Double = 0
        Using sqlcon As New OleDbConnection(mdbconstring)
            sqlcon.Open()
            Dim com As New OleDbCommand("SELECT SUM(GROSS_PAY) AS GROSS_PAY FROM PayRegTbl WHERE ID='" & ID & "';", sqlcon)
            Dim adpt As New OleDbDataAdapter(com)
            Dim dt As New DataTable
            adpt.Fill(dt)

            If IsDBNull(dt.Rows(0)(0)) Then
                grosscom = 0
            Else
                grosscom = dt.Rows(0)(0).ToString
            End If

            Return CDbl(grosscom)

        End Using
    End Function

    Public Function getSSSPAGPHIL(ByVal ID As String) As Double
        Dim alltax As Double = 0
        Using sqlcon As New OleDbConnection(mdbconstring)
            sqlcon.Open()
            Dim com As New OleDbCommand("SELECT (SUM(SSS_EE) + SUM(PHIC) + SUM(ADJUST1 * -1)) AS NONTAX_SSS_PAG_IBIG FROM PayRegTbl WHERE ID='" & ID & "';", sqlcon)
            Dim adpt As New OleDbDataAdapter(com)
            Dim dt As New DataTable
            adpt.Fill(dt)

            If IsDBNull(dt.Rows(0)(0)) Then
                alltax = 0
            Else
                alltax = dt.Rows(0)(0).ToString
            End If

            Return CDbl(alltax)

        End Using
    End Function

    Public Function gethol(ByVal ID As String) As Double
        Dim hol As Double = 0
        Using sqlcon As New OleDbConnection(mdbconstring)
            sqlcon.Open()
            Dim com As New OleDbCommand("SELECT SUM(HOL_OT*RATE) AS HOLIDAY FROM PayRegTbl WHERE ID='" & ID & "';", sqlcon)
            Dim adpt As New OleDbDataAdapter(com)
            Dim dt As New DataTable
            adpt.Fill(dt)

            If IsDBNull(dt.Rows(0)(0)) Then
                hol = 0
            Else
                hol = dt.Rows(0)(0).ToString
            End If

            Return CDbl(hol)

        End Using
    End Function

    Public Function get13th(ByVal ID As String) As Double
        Dim nontax As Double = 0
        Using sqlcon As New OleDbConnection(mdbconstring)
            sqlcon.Open()
            Dim com As New OleDbCommand("SELECT NONTAX_13TH AS NONTAX_13TH FROM PayRegTbl WHERE ID='" & ID & "';", sqlcon)
            Dim adpt As New OleDbDataAdapter(com)
            Dim dt As New DataTable
            adpt.Fill(dt)

            If IsDBNull(dt.Rows(0)(0)) Then
                nontax = 0
            Else
                nontax = dt.Rows(0)(0).ToString
            End If

            Return CDbl(nontax)

        End Using
    End Function

    Public Function gettotalinc(ByVal ID As String) As Double
        Dim totalinc As Double = 0
        Using sqlcon As New OleDbConnection(mdbconstring)
            sqlcon.Open()
            Dim com As New OleDbCommand("SELECT SUM(GROSS_PAY) - (SUM(SSS_EE) + SUM(PHIC) + SUM(ADJUST1 * -1)) AS TOTAL_INCOME FROM PayRegTbl WHERE ID='" & ID & "';", sqlcon)
            Dim adpt As New OleDbDataAdapter(com)
            Dim dt As New DataTable
            adpt.Fill(dt)

            If IsDBNull(dt.Rows(0)(0)) Then
                totalinc = 0
            Else
                totalinc = dt.Rows(0)(0).ToString
            End If

            Return CDbl(totalinc)

        End Using
    End Function

    Public Function gettaxdue(ByVal ID As String) As Double
        Dim taxdue As Double = 0
        Using sqlcon As New OleDbConnection(mdbconstring)
            sqlcon.Open()
            Dim com As New OleDbCommand("SELECT SUM(GROSS_PAY) - (SUM(SSS_EE) + SUM(PHIC) + SUM(ADJUST1 * -1)) AS TAX_DUE FROM PayRegTbl WHERE ID='" & ID & "';", sqlcon)
            Dim adpt As New OleDbDataAdapter(com)
            Dim dt As New DataTable
            adpt.Fill(dt)

            If IsDBNull(dt.Rows(0)(0)) Then
                taxdue = 0
            Else
                taxdue = Computetax(dt.Rows(0)(0).ToString)
            End If

            Return CDbl(taxdue)

        End Using
    End Function

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

    Public Function getjannov(ByVal ID As String) As Double
        Dim jannov As Double = 0
        Using sqlcon As New OleDbConnection(mdbconstring)
            sqlcon.Open()
            Dim com As New OleDbCommand("Select TOP 22 TAX from PayRegTbl where ID='" & ID & "' ORDER BY DBDATE ASC;", sqlcon)
            Dim adpt As New OleDbDataAdapter(com)
            Dim dt As New DataTable
            adpt.Fill(dt)

            For i = 0 To dt.Rows.Count - 1
                jannov += CDbl(dt.Rows(i)(0).ToString)
            Next

        End Using
        Return jannov
    End Function

    Public Function getdecember(ByVal ID As String) As Double
        getdecember = New Double
        Using sqlcon As New OleDbConnection(mdbconstring)
            sqlcon.Open()
            Dim com As New OleDbCommand("Select TOP 1 TAX from PayRegTbl where ID='" & ID & "' ORDER BY DBDATE DESC;", sqlcon)
            Dim adpt As New OleDbDataAdapter(com)
            Dim dt As New DataTable
            adpt.Fill(dt)

            getdecember = CDbl(dt.Rows(0)(0).ToString)
        End Using

        Return getdecember
    End Function


    Public Function getperday(ByVal id As String) As Double
        Dim perday As Double = 0
        Using sqlcon As New OleDbConnection(mdbconstring)
            sqlcon.Open()
            Dim com As New OleDbCommand("SELECT RATE*8 AS PER_DAY FROM PayRegTbl WHERE ID='" & id & "';", sqlcon)
            Dim adpt As New OleDbDataAdapter(com)
            Dim dt As New DataTable
            adpt.Fill(dt)

            If IsDBNull(dt.Rows(0)(0)) Then
                perday = 0
            Else
                perday = Computetax(dt.Rows(0)(0).ToString)
            End If

            Return CDbl(perday)

        End Using
    End Function

    Public Function getholiday(ByVal id As String) As Double
        Dim hol As Double = 0
        Using sqlcon As New OleDbConnection(mdbconstring)
            sqlcon.Open()
            Dim com As New OleDbCommand("SELECT SUM(HOL_OT*RATE)/12 AS HOLIDAY FROM PayRegTbl WHERE ID='" & id & "';", sqlcon)
            Dim adpt As New OleDbDataAdapter(com)
            Dim dt As New DataTable
            adpt.Fill(dt)

            If IsDBNull(dt.Rows(0)(0)) Then
                hol = 0
            Else
                hol = Computetax(dt.Rows(0)(0).ToString)
            End If

            Return CDbl(hol)

        End Using
    End Function

    Public Function getovertime(ByVal id As String) As Double
        Dim over As Double = 0
        Using sqlcon As New OleDbConnection(mdbconstring)
            sqlcon.Open()
            Dim com As New OleDbCommand("SELECT SUM(R_OT*RATE)/12 AS OVERTIME FROM PayRegTbl WHERE ID='" & id & "';", sqlcon)
            Dim adpt As New OleDbDataAdapter(com)
            Dim dt As New DataTable
            adpt.Fill(dt)

            If IsDBNull(dt.Rows(0)(0)) Then
                over = 0
            Else
                over = Computetax(dt.Rows(0)(0).ToString)
            End If

            Return CDbl(over)

        End Using
    End Function

    Public Function getnightdiff(ByVal id As String) As Double
        Dim nd As Double = 0
        Using sqlcon As New OleDbConnection(mdbconstring)
            sqlcon.Open()
            Dim com As New OleDbCommand("SELECT SUM(ND*RATE)/12 AS NIGHT_DIFFERENTIAL FROM PayRegTbl WHERE ID='" & id & "';", sqlcon)
            Dim adpt As New OleDbDataAdapter(com)
            Dim dt As New DataTable
            adpt.Fill(dt)

            If IsDBNull(dt.Rows(0)(0)) Then
                nd = 0
            Else
                nd = Computetax(dt.Rows(0)(0).ToString)
            End If

            Return CDbl(nd)

        End Using
    End Function

End Module
