<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.lstPerday = New System.Windows.Forms.ListView()
        Me.Folders = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.cmbSite = New System.Windows.Forms.ComboBox()
        Me.cmbCompany = New System.Windows.Forms.ComboBox()
        Me.btnRunAlpha = New System.Windows.Forms.Button()
        Me.cmbEEData = New System.Windows.Forms.ComboBox()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.lbStatus = New System.Windows.Forms.ToolStripStatusLabel()
        Me.PB1 = New System.Windows.Forms.ProgressBar()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnClear = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.tbRegisteredName = New System.Windows.Forms.TextBox()
        Me.tbEmployerTIN = New System.Windows.Forms.TextBox()
        Me.tbEndOfPeriod = New System.Windows.Forms.TextBox()
        Me.tbRegionNo = New System.Windows.Forms.TextBox()
        Me.tbDestinationFolder = New System.Windows.Forms.TextBox()
        Me.lstKS = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.mnOpenOutputFolder = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnOpenEEDataFolder = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnRefresh = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnRunConverter = New System.Windows.Forms.Button()
        Me.lstPendingConversion = New System.Windows.Forms.ListView()
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.tbBranchCode = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lstCompanyTemplates = New System.Windows.Forms.ListBox()
        Me.StatusStrip1.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'lstPerday
        '
        Me.lstPerday.AllowDrop = True
        Me.lstPerday.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstPerday.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.Folders})
        Me.lstPerday.HideSelection = False
        Me.lstPerday.Location = New System.Drawing.Point(15, 122)
        Me.lstPerday.Name = "lstPerday"
        Me.lstPerday.Size = New System.Drawing.Size(332, 138)
        Me.lstPerday.TabIndex = 0
        Me.lstPerday.UseCompatibleStateImageBehavior = False
        Me.lstPerday.View = System.Windows.Forms.View.Details
        '
        'Folders
        '
        Me.Folders.Text = "Perday"
        Me.Folders.Width = 304
        '
        'cmbSite
        '
        Me.cmbSite.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSite.FormattingEnabled = True
        Me.cmbSite.Location = New System.Drawing.Point(76, 270)
        Me.cmbSite.Name = "cmbSite"
        Me.cmbSite.Size = New System.Drawing.Size(105, 22)
        Me.cmbSite.TabIndex = 1
        '
        'cmbCompany
        '
        Me.cmbCompany.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbCompany.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCompany.FormattingEnabled = True
        Me.cmbCompany.Location = New System.Drawing.Point(76, 298)
        Me.cmbCompany.Name = "cmbCompany"
        Me.cmbCompany.Size = New System.Drawing.Size(536, 22)
        Me.cmbCompany.TabIndex = 3
        '
        'btnRunAlpha
        '
        Me.btnRunAlpha.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRunAlpha.Location = New System.Drawing.Point(618, 300)
        Me.btnRunAlpha.Name = "btnRunAlpha"
        Me.btnRunAlpha.Size = New System.Drawing.Size(107, 48)
        Me.btnRunAlpha.TabIndex = 5
        Me.btnRunAlpha.Text = "Run Alpha"
        Me.btnRunAlpha.UseVisualStyleBackColor = True
        '
        'cmbEEData
        '
        Me.cmbEEData.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbEEData.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbEEData.FormattingEnabled = True
        Me.cmbEEData.Location = New System.Drawing.Point(76, 326)
        Me.cmbEEData.Name = "cmbEEData"
        Me.cmbEEData.Size = New System.Drawing.Size(536, 22)
        Me.cmbEEData.TabIndex = 4
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lbStatus})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 557)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(737, 22)
        Me.StatusStrip1.TabIndex = 6
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'lbStatus
        '
        Me.lbStatus.Name = "lbStatus"
        Me.lbStatus.Size = New System.Drawing.Size(0, 17)
        '
        'PB1
        '
        Me.PB1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PB1.Location = New System.Drawing.Point(0, 551)
        Me.PB1.Name = "PB1"
        Me.PB1.Size = New System.Drawing.Size(737, 5)
        Me.PB1.TabIndex = 7
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 273)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(28, 14)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "Site"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 329)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(48, 14)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "EE Data"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 301)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(56, 14)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "Company"
        '
        'btnClear
        '
        Me.btnClear.Location = New System.Drawing.Point(187, 268)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(114, 24)
        Me.btnClear.TabIndex = 12
        Me.btnClear.Text = "Clear"
        Me.btnClear.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(198, 497)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(66, 14)
        Me.Label5.TabIndex = 13
        Me.Label5.Text = "Region No."
        '
        'Label6
        '
        Me.Label6.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(317, 498)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(66, 14)
        Me.Label6.TabIndex = 14
        Me.Label6.Text = "Tax TIN No."
        '
        'Label7
        '
        Me.Label7.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(12, 525)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(101, 14)
        Me.Label7.TabIndex = 15
        Me.Label7.Text = "Registered Name"
        '
        'Label8
        '
        Me.Label8.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(12, 469)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(108, 14)
        Me.Label8.TabIndex = 16
        Me.Label8.Text = "Destination Folder"
        '
        'Label9
        '
        Me.Label9.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(12, 497)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(105, 14)
        Me.Label9.TabIndex = 17
        Me.Label9.Text = "End of Year Period"
        '
        'tbRegisteredName
        '
        Me.tbRegisteredName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbRegisteredName.Location = New System.Drawing.Point(126, 522)
        Me.tbRegisteredName.Name = "tbRegisteredName"
        Me.tbRegisteredName.Size = New System.Drawing.Size(486, 22)
        Me.tbRegisteredName.TabIndex = 18
        '
        'tbEmployerTIN
        '
        Me.tbEmployerTIN.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.tbEmployerTIN.Location = New System.Drawing.Point(389, 494)
        Me.tbEmployerTIN.Name = "tbEmployerTIN"
        Me.tbEmployerTIN.Size = New System.Drawing.Size(128, 22)
        Me.tbEmployerTIN.TabIndex = 19
        '
        'tbEndOfPeriod
        '
        Me.tbEndOfPeriod.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.tbEndOfPeriod.Location = New System.Drawing.Point(126, 494)
        Me.tbEndOfPeriod.Name = "tbEndOfPeriod"
        Me.tbEndOfPeriod.Size = New System.Drawing.Size(66, 22)
        Me.tbEndOfPeriod.TabIndex = 20
        '
        'tbRegionNo
        '
        Me.tbRegionNo.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.tbRegionNo.Location = New System.Drawing.Point(270, 494)
        Me.tbRegionNo.Name = "tbRegionNo"
        Me.tbRegionNo.Size = New System.Drawing.Size(41, 22)
        Me.tbRegionNo.TabIndex = 21
        '
        'tbDestinationFolder
        '
        Me.tbDestinationFolder.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbDestinationFolder.Location = New System.Drawing.Point(126, 466)
        Me.tbDestinationFolder.Name = "tbDestinationFolder"
        Me.tbDestinationFolder.Size = New System.Drawing.Size(487, 22)
        Me.tbDestinationFolder.TabIndex = 22
        '
        'lstKS
        '
        Me.lstKS.AllowDrop = True
        Me.lstKS.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstKS.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1})
        Me.lstKS.HideSelection = False
        Me.lstKS.Location = New System.Drawing.Point(353, 122)
        Me.lstKS.Name = "lstKS"
        Me.lstKS.Size = New System.Drawing.Size(372, 138)
        Me.lstKS.TabIndex = 23
        Me.lstKS.UseCompatibleStateImageBehavior = False
        Me.lstKS.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "KS"
        Me.ColumnHeader1.Width = 353
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnOpenOutputFolder, Me.mnOpenEEDataFolder, Me.mnRefresh})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(737, 24)
        Me.MenuStrip1.TabIndex = 27
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'mnOpenOutputFolder
        '
        Me.mnOpenOutputFolder.Name = "mnOpenOutputFolder"
        Me.mnOpenOutputFolder.Size = New System.Drawing.Size(125, 20)
        Me.mnOpenOutputFolder.Text = "Open Output Folder"
        '
        'mnOpenEEDataFolder
        '
        Me.mnOpenEEDataFolder.Name = "mnOpenEEDataFolder"
        Me.mnOpenEEDataFolder.Size = New System.Drawing.Size(126, 20)
        Me.mnOpenEEDataFolder.Text = "Open EE Data Folder"
        '
        'mnRefresh
        '
        Me.mnRefresh.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.mnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.mnRefresh.Image = CType(resources.GetObject("mnRefresh.Image"), System.Drawing.Image)
        Me.mnRefresh.Name = "mnRefresh"
        Me.mnRefresh.Size = New System.Drawing.Size(28, 20)
        Me.mnRefresh.Text = "Refresh"
        '
        'btnRunConverter
        '
        Me.btnRunConverter.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRunConverter.Location = New System.Drawing.Point(618, 469)
        Me.btnRunConverter.Name = "btnRunConverter"
        Me.btnRunConverter.Size = New System.Drawing.Size(107, 72)
        Me.btnRunConverter.TabIndex = 28
        Me.btnRunConverter.Text = "Convert"
        Me.btnRunConverter.UseVisualStyleBackColor = True
        '
        'lstPendingConversion
        '
        Me.lstPendingConversion.AllowDrop = True
        Me.lstPendingConversion.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstPendingConversion.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader2})
        Me.lstPendingConversion.FullRowSelect = True
        Me.lstPendingConversion.HideSelection = False
        Me.lstPendingConversion.Location = New System.Drawing.Point(15, 354)
        Me.lstPendingConversion.Name = "lstPendingConversion"
        Me.lstPendingConversion.Size = New System.Drawing.Size(710, 106)
        Me.lstPendingConversion.TabIndex = 29
        Me.lstPendingConversion.UseCompatibleStateImageBehavior = False
        Me.lstPendingConversion.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Pending Conversion (Double Click to Open)"
        Me.ColumnHeader2.Width = 599
        '
        'tbBranchCode
        '
        Me.tbBranchCode.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.tbBranchCode.Location = New System.Drawing.Point(540, 494)
        Me.tbBranchCode.Name = "tbBranchCode"
        Me.tbBranchCode.Size = New System.Drawing.Size(72, 22)
        Me.tbBranchCode.TabIndex = 31
        '
        'Label4
        '
        Me.Label4.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(520, 497)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(17, 14)
        Me.Label4.TabIndex = 32
        Me.Label4.Text = " - "
        '
        'lstCompanyTemplates
        '
        Me.lstCompanyTemplates.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstCompanyTemplates.FormattingEnabled = True
        Me.lstCompanyTemplates.ItemHeight = 14
        Me.lstCompanyTemplates.Location = New System.Drawing.Point(15, 27)
        Me.lstCompanyTemplates.Name = "lstCompanyTemplates"
        Me.lstCompanyTemplates.Size = New System.Drawing.Size(710, 88)
        Me.lstCompanyTemplates.TabIndex = 33
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(737, 579)
        Me.Controls.Add(Me.lstCompanyTemplates)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.tbBranchCode)
        Me.Controls.Add(Me.lstPendingConversion)
        Me.Controls.Add(Me.btnRunConverter)
        Me.Controls.Add(Me.lstKS)
        Me.Controls.Add(Me.tbDestinationFolder)
        Me.Controls.Add(Me.tbRegionNo)
        Me.Controls.Add(Me.tbEndOfPeriod)
        Me.Controls.Add(Me.tbEmployerTIN)
        Me.Controls.Add(Me.tbRegisteredName)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.btnClear)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.cmbEEData)
        Me.Controls.Add(Me.btnRunAlpha)
        Me.Controls.Add(Me.cmbCompany)
        Me.Controls.Add(Me.cmbSite)
        Me.Controls.Add(Me.lstPerday)
        Me.Controls.Add(Me.PB1)
        Me.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "frmMain"
        Me.Text = "Form1"
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lstPerday As ListView
    Friend WithEvents Folders As ColumnHeader
    Friend WithEvents cmbSite As ComboBox
    Friend WithEvents cmbCompany As ComboBox
    Friend WithEvents btnRunAlpha As Button
    Friend WithEvents cmbEEData As ComboBox
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents lbStatus As ToolStripStatusLabel
    Friend WithEvents PB1 As ProgressBar
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents btnClear As Button
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents tbRegisteredName As TextBox
    Friend WithEvents tbEmployerTIN As TextBox
    Friend WithEvents tbEndOfPeriod As TextBox
    Friend WithEvents tbRegionNo As TextBox
    Friend WithEvents tbDestinationFolder As TextBox
    Friend WithEvents lstKS As ListView
    Friend WithEvents ColumnHeader1 As ColumnHeader
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents mnOpenOutputFolder As ToolStripMenuItem
    Friend WithEvents mnOpenEEDataFolder As ToolStripMenuItem
    Friend WithEvents mnRefresh As ToolStripMenuItem
    Friend WithEvents btnRunConverter As Button
    Friend WithEvents lstPendingConversion As ListView
    Friend WithEvents ColumnHeader2 As ColumnHeader
    Friend WithEvents tbBranchCode As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents lstCompanyTemplates As ListBox
End Class
