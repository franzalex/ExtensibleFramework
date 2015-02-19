<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TimerActivity
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
        Dim tlpMain As System.Windows.Forms.TableLayoutPanel
        Dim tlpButtons As System.Windows.Forms.TableLayoutPanel
        Me.lblTimer = New System.Windows.Forms.Label()
        Me.btnReset = New System.Windows.Forms.Button()
        Me.btnStart = New System.Windows.Forms.Button()
        Me.tmrUpdate = New System.Windows.Forms.Timer(Me.components)
        tlpMain = New System.Windows.Forms.TableLayoutPanel()
        tlpButtons = New System.Windows.Forms.TableLayoutPanel()
        tlpMain.SuspendLayout()
        tlpButtons.SuspendLayout()
        Me.SuspendLayout()
        '
        'tlpMain
        '
        tlpMain.ColumnCount = 6
        tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 19.0!))
        tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 93.0!))
        tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 19.0!))
        tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        tlpMain.Controls.Add(Me.lblTimer, 2, 1)
        tlpMain.Controls.Add(tlpButtons, 3, 0)
        tlpMain.Dock = System.Windows.Forms.DockStyle.Fill
        tlpMain.Location = New System.Drawing.Point(0, 0)
        tlpMain.Name = "tlpMain"
        tlpMain.RowCount = 3
        tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        tlpMain.Size = New System.Drawing.Size(373, 231)
        tlpMain.TabIndex = 3
        '
        'lblTimer
        '
        Me.lblTimer.AutoSize = True
        Me.lblTimer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTimer.Font = New System.Drawing.Font("Consolas", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTimer.Location = New System.Drawing.Point(41, 97)
        Me.lblTimer.Name = "lblTimer"
        Me.lblTimer.Size = New System.Drawing.Size(197, 37)
        Me.lblTimer.TabIndex = 0
        Me.lblTimer.Text = "00:00:00.0"
        Me.lblTimer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'tlpButtons
        '
        tlpButtons.ColumnCount = 1
        tlpButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        tlpButtons.Controls.Add(Me.btnReset, 0, 2)
        tlpButtons.Controls.Add(Me.btnStart, 0, 1)
        tlpButtons.Dock = System.Windows.Forms.DockStyle.Fill
        tlpButtons.Location = New System.Drawing.Point(241, 0)
        tlpButtons.Margin = New System.Windows.Forms.Padding(0)
        tlpButtons.Name = "tlpButtons"
        tlpButtons.RowCount = 4
        tlpMain.SetRowSpan(tlpButtons, 3)
        tlpButtons.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        tlpButtons.RowStyles.Add(New System.Windows.Forms.RowStyle())
        tlpButtons.RowStyles.Add(New System.Windows.Forms.RowStyle())
        tlpButtons.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        tlpButtons.Size = New System.Drawing.Size(93, 231)
        tlpButtons.TabIndex = 1
        '
        'btnReset
        '
        Me.btnReset.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnReset.Location = New System.Drawing.Point(3, 118)
        Me.btnReset.Name = "btnReset"
        Me.btnReset.Size = New System.Drawing.Size(87, 27)
        Me.btnReset.TabIndex = 1
        Me.btnReset.Text = "&Reset"
        Me.btnReset.UseVisualStyleBackColor = True
        '
        'btnStart
        '
        Me.btnStart.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnStart.Location = New System.Drawing.Point(3, 85)
        Me.btnStart.Name = "btnStart"
        Me.btnStart.Size = New System.Drawing.Size(87, 27)
        Me.btnStart.TabIndex = 0
        Me.btnStart.Text = "&Start"
        Me.btnStart.UseVisualStyleBackColor = True
        '
        'tmrUpdate
        '
        '
        'TimerActivity
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(tlpMain)
        Me.Name = "TimerActivity"
        Me.Size = New System.Drawing.Size(373, 231)
        tlpMain.ResumeLayout(False)
        tlpMain.PerformLayout()
        tlpButtons.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblTimer As System.Windows.Forms.Label
    Friend WithEvents btnReset As System.Windows.Forms.Button
    Friend WithEvents btnStart As System.Windows.Forms.Button
    Private WithEvents tmrUpdate As System.Windows.Forms.Timer

End Class
