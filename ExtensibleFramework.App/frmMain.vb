Option Infer On

Public Class frmMain

    Private pluginDirs As New HashSet(Of String) 'using HashSet prevents adding the same item twice

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' you have to manually listen to the activity host control's navigation event
        AddHandler ahcActivityHost.Navigator.HasNavigated, AddressOf ActivityHost_HasNavigated


        ' get the solution directory (typically 3 diretories up from debug output)
        Dim exeFile = System.Reflection.Assembly.GetExecutingAssembly().Location
        Dim myDir = System.IO.Directory.GetParent(exeFile).FullName
        Dim solutionDir = My.Application.Directory
        For i = 1 To 3
            solutionDir = System.IO.Directory.GetParent(solutionDir).FullName
        Next

        pluginDirs.Add(solutionDir)


        ' load all plugins located in the solution
        ahcActivityHost.ScanForPlugins(pluginDirs.ToArray())


        ' find an activity for the go-home command
        ahcActivityHost.RunCommand("go-home")
    End Sub

    Private Sub ActivityHost_HasNavigated(sender As ExtensibleFramework.Core.Navigator, e As ExtensibleFramework.Core.Navigator.LocationChangedEventArgs)
        tsbBack.Enabled = ahcActivityHost.Navigator.CanGoBack
        tsbHome.Enabled = ahcActivityHost.CurrentActivity.ID <> "ExtensibleFramework.Home.HomeActivity"

        tslPluginTitle.Text = "Location: " & ahcActivityHost.CurrentActivity.Text
    End Sub

    Private Sub tsbBack_Click(sender As Object, e As EventArgs) Handles tsbBack.Click
        ahcActivityHost.Navigator.GoBack()
    End Sub

    Private Sub tsbHome_Click(sender As Object, e As EventArgs) Handles tsbHome.Click
        ' go back through all the items in the history
        Dim curIndex = ahcActivityHost.Navigator.IndexOf(ahcActivityHost.Navigator.CurrentLocation)
        ahcActivityHost.Navigator.GoBack(-curIndex)
    End Sub

    Private Sub tsmPluginsRescan_Click(sender As Object, e As EventArgs) Handles tsmPluginsRescan.Click
        ahcActivityHost.ScanForPlugins(pluginDirs.ToArray())
    End Sub

    Private Sub tsmPluginsAddDir_Click(sender As Object, e As EventArgs) Handles tsmPluginsAddDir.Click
        Using fbd As New FolderBrowserDialog() With {.SelectedPath = My.Application.Directory,
                                                     .Description = "Select plugin directory."}
            If fbd.ShowDialog() = Windows.Forms.DialogResult.OK Then
                pluginDirs.Add(fbd.SelectedPath)

                ahcActivityHost.ScanForPlugins(pluginDirs.ToArray())
            End If
        End Using
    End Sub

    Private Sub tsmAddPluginFile_Click(sender As Object, e As EventArgs) Handles tsmAddPluginFile.Click
        Using ofd As New OpenFileDialog() With {.InitialDirectory = My.Application.Directory,
                                                .Filter = "Plugin Info File|(*.info.txt)"}
            If ofd.ShowDialog() = Windows.Forms.DialogResult.OK Then
                pluginDirs.Add(System.IO.Directory.GetParent(ofd.FileName).FullName)

                ahcActivityHost.ScanForPlugins(pluginDirs.ToArray())
            End If
        End Using
    End Sub
End Class
