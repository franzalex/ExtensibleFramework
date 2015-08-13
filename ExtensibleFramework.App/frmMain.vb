Option Infer On

Public Class frmMain

    Private pluginDirs As New HashSet(Of String)(StringComparer.CurrentCultureIgnoreCase) ' HashSet prevents adding same item twice

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' you have to manually listen to the activity host control's navigation event
        AddHandler ahcActivityHost.Navigator.HasNavigated, AddressOf ActivityHost_HasNavigated


        ' read the plugin directories from the settings
        For Each path In My.Application.Settings.GetValue("pluginPaths", New String() {})
            pluginDirs.Add(path)
        Next

        ' set the application data directory. 
        ' plugin settings will be loaded from here when wee scan
        ahcActivityHost.SetDataDirectory(My.Application.AppDataDir)

        ' load all plugins located in the solution
        tsmPluginsRescan.PerformClick()


        ' find an activity for the go-home command
        ahcActivityHost.RunCommand("go-home")
    End Sub

    Private Sub ActivityHost_HasNavigated(sender As ExtensibleFramework.Core.Navigator, e As ExtensibleFramework.Core.Navigator.LocationChangedEventArgs)
        tsbBack.Enabled = ahcActivityHost.Navigator.CanGoBack
        tsbHome.Enabled = ahcActivityHost.CurrentActivity.ID <> "ExtensibleFramework.Home.HomeActivity"

        tslPluginTitle.Text = "Location: " & e.NewLocation.Text ' ahcActivityHost.CurrentActivity.Text
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

#If AutoAddSolutionDir Then  ' add the solution directory if the application is being debugged
        ' get the solution directory (typically 3 directories up from debug output)
        Dim solutionDir = My.Application.Directory
        For i = 1 To 3
            solutionDir = System.IO.Directory.GetParent(solutionDir).FullName
        Next
        pluginDirs.Add(solutionDir)
#End If

        ' add the default plugin path
        pluginDirs.Add(System.IO.Path.Combine(My.Application.Directory, "Plugins"))

        ahcActivityHost.ScanForPlugins(pluginDirs.ToArray())
    End Sub

    Private Sub tsmPluginsAddDir_Click(sender As Object, e As EventArgs) Handles tsmEditPluginsDirs.Click
        Using dlg = New dlgPluginPaths()

            If dlg.ShowDialog() = Windows.Forms.DialogResult.OK Then
                pluginDirs = New HashSet(Of String)(dlg.PluginDirectories, StringComparer.CurrentCultureIgnoreCase)
                My.Application.Settings.SetValue("pluginPaths", pluginDirs.ToArray())

                tsmPluginsRescan.PerformClick()
            End If
        End Using
    End Sub

    Private Sub tsmAddPluginFile_Click(sender As Object, e As EventArgs)
        Using ofd As New OpenFileDialog() With {.InitialDirectory = My.Application.Directory,
                                                .Filter = "Plugin Info File|(*.info.txt)"}
            If ofd.ShowDialog() = Windows.Forms.DialogResult.OK Then
                pluginDirs.Add(System.IO.Directory.GetParent(ofd.FileName).FullName)

                ahcActivityHost.ScanForPlugins(pluginDirs.ToArray())
            End If
        End Using
    End Sub
End Class
