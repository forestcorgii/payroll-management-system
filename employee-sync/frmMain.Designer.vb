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
        Me.components = New System.ComponentModel.Container()
        Me.bgwSync = New System.ComponentModel.BackgroundWorker()
        Me.tmLister = New System.Windows.Forms.Timer(Me.components)
        Me.lbStatus = New System.Windows.Forms.Label()
        Me.pb = New System.Windows.Forms.ProgressBar()
        Me.SuspendLayout()
        '
        'bgwSync
        '
        '
        'tmLister
        '
        Me.tmLister.Enabled = True
        '
        'lbStatus
        '
        Me.lbStatus.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbStatus.Location = New System.Drawing.Point(12, 9)
        Me.lbStatus.Name = "lbStatus"
        Me.lbStatus.Size = New System.Drawing.Size(312, 60)
        Me.lbStatus.TabIndex = 0
        '
        'pb
        '
        Me.pb.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pb.Location = New System.Drawing.Point(0, 72)
        Me.pb.Name = "pb"
        Me.pb.Size = New System.Drawing.Size(336, 23)
        Me.pb.TabIndex = 1
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(336, 95)
        Me.Controls.Add(Me.pb)
        Me.Controls.Add(Me.lbStatus)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmMain"
        Me.Text = "employee-sync"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents bgwSync As System.ComponentModel.BackgroundWorker
    Friend WithEvents tmLister As Timer
    Friend WithEvents lbStatus As Label
    Friend WithEvents pb As ProgressBar
End Class
