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
        Dim tsMain As System.Windows.Forms.ToolStrip
        Dim tsSep1 As System.Windows.Forms.ToolStripSeparator
        Me.tsbBack = New System.Windows.Forms.ToolStripButton()
        Me.tsbHome = New System.Windows.Forms.ToolStripButton()
        Me.tslPluginTitle = New System.Windows.Forms.ToolStripLabel()
        Me.tsbPluginTool = New System.Windows.Forms.ToolStripDropDownButton()
        Me.tsmAddPluginFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmPluginsAddDir = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsPluginSep1 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsmPluginsRescan = New System.Windows.Forms.ToolStripMenuItem()
        Me.ahcActivityHost = New ExtensibleFramework.Core.ActivityHostControl()
        tsMain = New System.Windows.Forms.ToolStrip()
        tsSep1 = New System.Windows.Forms.ToolStripSeparator()
        tsMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'tsMain
        '
        tsMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbBack, Me.tsbHome, tsSep1, Me.tslPluginTitle, Me.tsbPluginTool})
        tsMain.Location = New System.Drawing.Point(0, 0)
        tsMain.Name = "tsMain"
        tsMain.Size = New System.Drawing.Size(480, 25)
        tsMain.TabIndex = 1
        tsMain.Text = "ToolStrip1"
        '
        'tsbBack
        '
        Me.tsbBack.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbBack.Image = Global.ExtensibleFramework.App.My.Resources.Resources.NavBack
        Me.tsbBack.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbBack.Name = "tsbBack"
        Me.tsbBack.Size = New System.Drawing.Size(23, 22)
        Me.tsbBack.Text = "Back"
        '
        'tsbHome
        '
        Me.tsbHome.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbHome.Image = Global.ExtensibleFramework.App.My.Resources.Resources.HomeHS
        Me.tsbHome.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbHome.Name = "tsbHome"
        Me.tsbHome.Size = New System.Drawing.Size(23, 22)
        Me.tsbHome.Text = "Home"
        '
        'tsSep1
        '
        tsSep1.Name = "tsSep1"
        tsSep1.Size = New System.Drawing.Size(6, 25)
        '
        'tslPluginTitle
        '
        Me.tslPluginTitle.Name = "tslPluginTitle"
        Me.tslPluginTitle.Size = New System.Drawing.Size(0, 22)
        '
        'tsbPluginTool
        '
        Me.tsbPluginTool.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbPluginTool.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmAddPluginFile, Me.tsmPluginsAddDir, Me.tsPluginSep1, Me.tsmPluginsRescan})
        Me.tsbPluginTool.Image = Global.ExtensibleFramework.App.My.Resources.Resources.plugin
        Me.tsbPluginTool.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbPluginTool.Name = "tsbPluginTool"
        Me.tsbPluginTool.Size = New System.Drawing.Size(75, 22)
        Me.tsbPluginTool.Text = "Plugins"
        '
        'tsmAddPluginFile
        '
        Me.tsmAddPluginFile.Image = Global.ExtensibleFramework.App.My.Resources.Resources.plugin_add
        Me.tsmAddPluginFile.Name = "tsmAddPluginFile"
        Me.tsmAddPluginFile.Size = New System.Drawing.Size(169, 22)
        Me.tsmAddPluginFile.Text = "Add Plugin File"
        '
        'tsmPluginsAddDir
        '
        Me.tsmPluginsAddDir.Image = Global.ExtensibleFramework.App.My.Resources.Resources.folder_add
        Me.tsmPluginsAddDir.Name = "tsmPluginsAddDir"
        Me.tsmPluginsAddDir.Size = New System.Drawing.Size(169, 22)
        Me.tsmPluginsAddDir.Text = "Add Plugin Folder"
        '
        'tsPluginSep1
        '
        Me.tsPluginSep1.Name = "tsPluginSep1"
        Me.tsPluginSep1.Size = New System.Drawing.Size(166, 6)
        '
        'tsmPluginsRescan
        '
        Me.tsmPluginsRescan.Image = Global.ExtensibleFramework.App.My.Resources.Resources.folder_find
        Me.tsmPluginsRescan.Name = "tsmPluginsRescan"
        Me.tsmPluginsRescan.Size = New System.Drawing.Size(169, 22)
        Me.tsmPluginsRescan.Text = "Rescan Plugins"
        '
        'ahcActivityHost
        '
        Me.ahcActivityHost.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ahcActivityHost.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.ahcActivityHost.Location = New System.Drawing.Point(0, 25)
        Me.ahcActivityHost.Name = "ahcActivityHost"
        Me.ahcActivityHost.Size = New System.Drawing.Size(480, 335)
        Me.ahcActivityHost.TabIndex = 0
        Me.ahcActivityHost.Text = "ActivityHostControl1"
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(480, 360)
        Me.Controls.Add(Me.ahcActivityHost)
        Me.Controls.Add(tsMain)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.MinimumSize = New System.Drawing.Size(496, 398)
        Me.Name = "frmMain"
        Me.Text = "Extensible Framework"
        tsMain.ResumeLayout(False)
        tsMain.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents tsbBack As System.Windows.Forms.ToolStripButton
    Private WithEvents tsbHome As System.Windows.Forms.ToolStripButton
    Private WithEvents ahcActivityHost As ExtensibleFramework.Core.ActivityHostControl
    Private WithEvents tslPluginTitle As System.Windows.Forms.ToolStripLabel
    Private WithEvents tsbPluginTool As System.Windows.Forms.ToolStripDropDownButton
    Private WithEvents tsmAddPluginFile As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents tsmPluginsAddDir As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents tsPluginSep1 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents tsmPluginsRescan As System.Windows.Forms.ToolStripMenuItem

End Class
