<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dlgPluginPaths
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
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.lvwDirList = New System.Windows.Forms.ListView()
        Me.tsMain = New System.Windows.Forms.ToolStrip()
        Me.tsbAddFolder = New System.Windows.Forms.ToolStripButton()
        Me.tsbRemoveFolder = New System.Windows.Forms.ToolStripButton()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.tsMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.AutoSize = True
        Me.TableLayoutPanel1.ColumnCount = 3
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.Controls.Add(Me.cmdOK, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.cmdCancel, 2, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 312)
        Me.TableLayoutPanel1.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.Padding = New System.Windows.Forms.Padding(12, 6, 9, 9)
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(284, 50)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'cmdOK
        '
        Me.cmdOK.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.cmdOK.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cmdOK.Location = New System.Drawing.Point(116, 10)
        Me.cmdOK.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(75, 27)
        Me.cmdOK.TabIndex = 0
        Me.cmdOK.Text = "OK"
        '
        'cmdCancel
        '
        Me.cmdCancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cmdCancel.Location = New System.Drawing.Point(197, 10)
        Me.cmdCancel.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(75, 27)
        Me.cmdCancel.TabIndex = 1
        Me.cmdCancel.Text = "Cancel"
        '
        'lvwDirList
        '
        Me.lvwDirList.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvwDirList.Location = New System.Drawing.Point(0, 25)
        Me.lvwDirList.Name = "lvwDirList"
        Me.lvwDirList.ShowItemToolTips = True
        Me.lvwDirList.Size = New System.Drawing.Size(284, 287)
        Me.lvwDirList.TabIndex = 1
        Me.lvwDirList.UseCompatibleStateImageBehavior = False
        Me.lvwDirList.View = System.Windows.Forms.View.List
        '
        'tsMain
        '
        Me.tsMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.tsMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbAddFolder, Me.tsbRemoveFolder})
        Me.tsMain.Location = New System.Drawing.Point(0, 0)
        Me.tsMain.Name = "tsMain"
        Me.tsMain.Size = New System.Drawing.Size(284, 25)
        Me.tsMain.TabIndex = 2
        '
        'tsbAddFolder
        '
        Me.tsbAddFolder.Image = Global.ExtensibleFramework.App.My.Resources.Resources.folder_add
        Me.tsbAddFolder.Name = "tsbAddFolder"
        Me.tsbAddFolder.Size = New System.Drawing.Size(85, 22)
        Me.tsbAddFolder.Text = "&Add Folder"
        '
        'tsbRemoveFolder
        '
        Me.tsbRemoveFolder.Image = Global.ExtensibleFramework.App.My.Resources.Resources.folder_delete
        Me.tsbRemoveFolder.Name = "tsbRemoveFolder"
        Me.tsbRemoveFolder.Size = New System.Drawing.Size(106, 22)
        Me.tsbRemoveFolder.Text = "&Remove Folder"
        '
        'dlgPluginPaths
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(284, 362)
        Me.Controls.Add(Me.lvwDirList)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Controls.Add(Me.tsMain)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "dlgPluginPaths"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Plugin Paths"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.tsMain.ResumeLayout(False)
        Me.tsMain.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents tsbAddFolder As System.Windows.Forms.ToolStripButton
    Private WithEvents tsbRemoveFolder As System.Windows.Forms.ToolStripButton
    Private WithEvents tsMain As System.Windows.Forms.ToolStrip
    Private WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Private WithEvents cmdOK As System.Windows.Forms.Button
    Private WithEvents cmdCancel As System.Windows.Forms.Button
    Private WithEvents lvwDirList As System.Windows.Forms.ListView

End Class
