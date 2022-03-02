Imports System.IO
Imports NPOI.HSSF.UserModel
Imports NPOI.SS.UserModel

Public Class frmMain

    Private CompanyTemplates As New clsCompanyTemplates
    Private SelectedCompanyTemplate As clsCompanyTemplate = Nothing

    Public Bin As String
    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = Application.ProductName & " v" & Application.ProductVersion
        Bin = New DirectoryInfo(Application.StartupPath).Parent.FullName
        CompanyTemplateLocation = Bin & "\CompanyTemplates.XML"

        cmbCompany.Items.AddRange({"INTERNATIONAL DATA CONVERSION SOLUTIONS INC", "ACCUDATA INC", "FREIGHT PROCESS OUTSOURCING SOLUTIONS INC"})
        cmbSite.Items.AddRange({"Manila", "Batangas", "Leyte"})

        Dim files As String() = Directory.GetFiles(Bin & "\EE Data")
        cmbEEData.Items.Clear()
        For Each ee As String In files
            cmbEEData.Items.Add(New DirectoryInfo(ee).Name)
        Next

        setupDirectories()
        setup()

        tbEndOfPeriod.Text = Now.AddYears(-1).ToString("yyyy")
    End Sub

    Private CompanyTemplateLocation As String
    Public Sub setup()
        lstPendingConversion.Items.Clear()
        Dim files As String() = Directory.GetFiles(Bin & "\Output", "*.CSV")
        For Each fl As String In files
            lstPendingConversion.Items.Add(New DirectoryInfo(fl).Name)
        Next

        If File.Exists(CompanyTemplateLocation) Then
            CompanyTemplates = XmlSerialization.ReadFromFile(CompanyTemplateLocation, CompanyTemplates)
        Else
            XmlSerialization.WriteToFile(CompanyTemplateLocation, CompanyTemplates)
        End If

        lstCompanyTemplates.Items.Clear()
        For Each comptemp In CompanyTemplates.Items
            lstCompanyTemplates.Items.Add(comptemp)
        Next

        tbDestinationFolder.Text = CompanyTemplates.DestinationFolder
        lstCompanyTemplates.SelectedIndex = 0
    End Sub

    Private Sub setupDirectories()
        Directory.CreateDirectory(Bin & "\EE Data")
        Directory.CreateDirectory(Bin & "\Output")
        Directory.CreateDirectory(Bin & "\Deleted")
    End Sub

    Private Sub mnOpenOutputFolder_Click(sender As Object, e As EventArgs) Handles mnOpenOutputFolder.Click
        Process.Start(Bin & "\Output")
    End Sub

    Private Sub mnOpenEEDataFolder_Click(sender As Object, e As EventArgs) Handles mnOpenEEDataFolder.Click
        Process.Start(Bin & "\EE Data")
    End Sub

    Private Sub mnRefresh_Click(sender As Object, e As EventArgs) Handles mnRefresh.Click
        Dim files As String() = Directory.GetFiles(Bin & "\EE Data")
        cmbEEData.Items.Clear()
        For Each ee As String In files
            cmbEEData.Items.Add(New DirectoryInfo(ee).Name)
        Next

        setup()

    End Sub


    Private runAlpha As New Alpha
    Private Sub btnStart_Click(sender As Object, e As EventArgs) Handles btnRunAlpha.Click
        Dim perdayFolders As New List(Of String)
        For i As Integer = 0 To lstPerday.Items.Count - 1
            perdayFolders.Add(lstPerday.Items.Item(i).Text)
        Next

        Dim ksFolders As New List(Of String)
        For i As Integer = 0 To lstKS.Items.Count - 1
            ksFolders.Add(lstKS.Items.Item(i).Text)
        Next

        runAlpha = New Alpha()
        With runAlpha
            .Company = cmbCompany.Text
            .Site = cmbSite.Text
            .TargetYear = Now.ToString("yy")
            .PerdayFolders = perdayFolders
            .KSFolders = ksFolders
            .EEDataFile = Bin & "\EE Data\" & cmbEEData.Text
            .OutputFile = Bin & "\Output"
            .PB1 = PB1
            .StatusLabel = lbStatus
            .DestinationFolder = tbDestinationFolder.Text
            .EndOfYearPeriod = tbEndOfPeriod.Text
            .RegionNumber = tbRegionNo.Text
            .RegisteredName = tbRegisteredName.Text
            .EmployerTIN = tbEmployerTIN.Text
        End With

        runAlpha.Start()

        setup()
    End Sub


    Private Sub btnRunConverter_Click(sender As Object, e As EventArgs) Handles btnRunConverter.Click
        If SelectedCompanyTemplate Is Nothing Then
            Dim newTemplate As New clsCompanyTemplate
            With newTemplate
                .TIN = tbEmployerTIN.Text
                .BranchCode = tbBranchCode.Text
                .RegionNumber = tbRegionNo.Text
                .RegisteredName = tbRegisteredName.Text

                .SiteIdx = cmbSite.SelectedIndex
                .CompanyIdx = cmbCompany.SelectedIndex
            End With
            CompanyTemplates.Items.Add(newTemplate)
        Else
            With SelectedCompanyTemplate
                .TIN = tbEmployerTIN.Text
                .BranchCode = tbBranchCode.Text
                .RegionNumber = tbRegionNo.Text
                .RegisteredName = tbRegisteredName.Text

                .SiteIdx = cmbSite.SelectedIndex
                .CompanyIdx = cmbCompany.SelectedIndex
            End With
            CompanyTemplates.Items(lstCompanyTemplates.SelectedIndex) = SelectedCompanyTemplate
        End If

        If Not CompanyTemplates.DestinationFolder = tbDestinationFolder.Text Then
            CompanyTemplates.DestinationFolder = tbDestinationFolder.Text
        End If

        XmlSerialization.WriteToFile(CompanyTemplateLocation, CompanyTemplates)

        If Not Directory.Exists(tbDestinationFolder.Text) Then
            MessageBox.Show("Input Destination Folder", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Dim selectedFiles As New List(Of String)
        For i As Integer = 0 To lstPendingConversion.SelectedItems.Count - 1
            selectedFiles.Add(Bin & "\Output\" & lstPendingConversion.SelectedItems(i).Text)
        Next

        With runAlpha
            .Company = cmbCompany.Text
            .Site = cmbSite.Text
            .TargetYear = Now.ToString("yy")
            .EEDataFile = Bin & "\EE Data\" & cmbEEData.Text
            .OutputFile = Bin & "\Output"
            .PB1 = PB1
            .StatusLabel = lbStatus
            .DestinationFolder = tbDestinationFolder.Text
            .EndOfYearPeriod = tbEndOfPeriod.Text
            .RegionNumber = tbRegionNo.Text
            .RegisteredName = tbRegisteredName.Text
            .EmployerTIN = tbEmployerTIN.Text
            .BranchCode = tbBranchCode.Text
        End With
        runAlpha.ConvertSelected(selectedFiles.ToArray)
        mnRefresh.PerformClick()
    End Sub

    Private Sub lstPerday_DragDrop(sender As Object, e As DragEventArgs) Handles lstPerday.DragDrop
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            Dim paths() As String
            paths = e.Data.GetData(DataFormats.FileDrop)

            For Each p In paths
                lstPerday.Items.Add(p)
            Next
        End If
    End Sub

    Private Sub lstPerday_DragEnter(sender As Object, e As DragEventArgs) Handles lstPerday.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

    Private Sub lstKS_DragDrop(sender As Object, e As DragEventArgs) Handles lstKS.DragDrop
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            Dim paths() As String
            paths = e.Data.GetData(DataFormats.FileDrop)

            For Each p In paths
                lstKS.Items.Add(p)
            Next
        End If
    End Sub

    Private Sub lstKS_DragEnter(sender As Object, e As DragEventArgs) Handles lstKS.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        lstPerday.Items.Clear()
        lstKS.Items.Clear()
    End Sub

    Private Sub lstPendingConversion_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles lstPendingConversion.MouseDoubleClick
        Process.Start(Bin & "\Output\" & lstPendingConversion.SelectedItems(0).Text)
    End Sub

    Private Sub tbBranchCode_TextChanged(sender As Object, e As EventArgs) Handles tbBranchCode.TextChanged
        tbBranchCode.Text = CInt(tbBranchCode.Text).ToString("0000")
        tbBranchCode.SelectionStart = tbBranchCode.TextLength
    End Sub

    Private Sub lstCompanyTemplates_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstCompanyTemplates.SelectedIndexChanged
        SelectedCompanyTemplate = lstCompanyTemplates.SelectedItem
        With SelectedCompanyTemplate
            cmbSite.SelectedIndex = .SiteIdx
            cmbCompany.SelectedIndex = .CompanyIdx

            tbEmployerTIN.Text = .TIN
            tbBranchCode.Text = .BranchCode
            tbRegionNo.Text = .RegionNumber
            tbRegisteredName.Text = .RegisteredName
        End With
    End Sub
End Class

