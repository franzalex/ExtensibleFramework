Imports System.Windows.Forms

Public Class dlgPluginPaths

    Dim _pluginDirs As List(Of String)

    ''' <summary>Gets the plugin directories.</summary>
    Public ReadOnly Property PluginDirectories As IEnumerable(Of String)
        Get
            Return _pluginDirs.AsReadOnly()
        End Get
    End Property

    Private Sub DialogButtons_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click, cmdCancel.Click
        Me.DialogResult = If(sender Is cmdOK, DialogResult.OK, DialogResult.Cancel)
        Me.Close()
    End Sub

    Private Sub dlgPluginPaths_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If Me.DialogResult = Windows.Forms.DialogResult.OK Then
            _pluginDirs = (From item In lvwDirList.Items.OfType(Of ListViewItem)()
                             Select item.Text Distinct
                             Order By Text Ascending).ToList()
        End If
    End Sub

    Private Sub dlgPluginPaths_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _pluginDirs = New List(Of String)()

        ' load the plugin directories 
        lvwDirList.Items.Clear()

        For Each pluginDir In My.Application.Settings.GetValue("pluginPaths", New String() {})
            lvwDirList.Items.Add(pluginDir)
        Next


        ' set the enabled state of the toolbar buttons
        tsbRemoveFolder.Enabled = lvwDirList.SelectedItems.Count > 0
    End Sub

    Private Sub lvwDirList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lvwDirList.SelectedIndexChanged
        ' set the enabled state of the toolbar buttons
        tsbRemoveFolder.Enabled = lvwDirList.SelectedItems.Count > 0
    End Sub

    Private Sub tsbAddFolder_Click(sender As Object, e As EventArgs) Handles tsbAddFolder.Click
        Using fbd As New FolderBrowserDialog() With {.SelectedPath = My.Application.Directory,
                                             .Description = "Select plugin directory."}
            If fbd.ShowDialog() = Windows.Forms.DialogResult.OK Then
                lvwDirList.Items.Add(fbd.SelectedPath)
            End If
        End Using

    End Sub

    Private Sub tsbRemoveFolder_Click(sender As Object, e As EventArgs) Handles tsbRemoveFolder.Click
        ' remove all selected items
        For i = lvwDirList.SelectedIndices.Count - 1 To 0 Step -1
            lvwDirList.Items.RemoveAt(lvwDirList.SelectedIndices(i))
        Next
    End Sub
End Class
