<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PongGame
    Inherits ExtensibleFramework.Core.ActivityControl

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.tmrUpdate = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'PongGame
        '
        Me.Name = "PongGame"
        Me.Size = New System.Drawing.Size(360, 270)
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents tmrUpdate As System.Windows.Forms.Timer

End Class
