<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class HomeActivity
    Inherits ExtensibleFramework.Core.ActivityControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.lvwHomeItems = New System.Windows.Forms.ListView()
        Me.iml48px = New System.Windows.Forms.ImageList(Me.components)
        Me.SuspendLayout()
        '
        'lvwHomeItems
        '
        Me.lvwHomeItems.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvwHomeItems.LargeImageList = Me.iml48px
        Me.lvwHomeItems.Location = New System.Drawing.Point(0, 0)
        Me.lvwHomeItems.MultiSelect = False
        Me.lvwHomeItems.Name = "lvwHomeItems"
        Me.lvwHomeItems.Size = New System.Drawing.Size(467, 346)
        Me.lvwHomeItems.TabIndex = 1
        Me.lvwHomeItems.UseCompatibleStateImageBehavior = False
        Me.lvwHomeItems.View = System.Windows.Forms.View.Tile
        '
        'iml48px
        '
        Me.iml48px.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit
        Me.iml48px.ImageSize = New System.Drawing.Size(48, 48)
        Me.iml48px.TransparentColor = System.Drawing.Color.Transparent
        '
        'HomeActivity
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.lvwHomeItems)
        Me.Name = "HomeActivity"
        Me.Size = New System.Drawing.Size(467, 346)
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents lvwHomeItems As System.Windows.Forms.ListView
    Private WithEvents iml48px As System.Windows.Forms.ImageList

End Class
