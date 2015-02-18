Imports ExtensibleFramework.Core

Public Class HomeActivity
    ' IMPORTANT: You *MUST* inherit from the ExtensibleFramework.Core.ActivityControl
    Inherits ExtensibleFramework.Core.ActivityControl

    Private Sub HomeActivity_Started(sender As Object, e As EventArgs) Handles Me.Started, Me.Restart
        Me.Text = "Home"

        ' Initialize control by getting all activity launchers in plugins currently 
        ' loaded and display them in the ListView
        Dim iconSize = lvwHomeItems.LargeImageList.ImageSize
        lvwHomeItems.TileSize = New Drawing.Size(Math.Max(192, 128 + iconSize.Width), iconSize.Height)
        Me.lvwHomeItems.Items.Clear()
        Me.ScanForActivityLaunchers()
    End Sub

    Private Sub HomeActivity_Stopped(sender As Object, e As StoppedEventArgs) Handles Me.Stopped
        ' release resources by clearing all the loaded items
        If e.CleanUp Then
            iml48px.Images.Clear()
            lvwHomeItems.Items.Clear()
        End If
    End Sub

    Private Sub lvwHomeItems_DoubleClick(sender As Object, e As EventArgs) Handles lvwHomeItems.DoubleClick
        ' run the activity associated with the double clicked activity launcher item

        If lvwHomeItems.SelectedItems.Count = 0 Then Return ' no selection
        If Me.HostControl Is Nothing Then Return ' no host control to run activities

        ' get the launching information
        Dim launchInfo = DirectCast(lvwHomeItems.SelectedItems(0).Tag, String())
        Dim activityID = launchInfo(0)
        Dim initCmd = launchInfo(1)

        ' run the activity
        Me.HostControl.RunActivity(activityID, initCmd)
    End Sub

    Private Sub lvwHomeItems_KeyUp(sender As Object, e As KeyEventArgs) Handles lvwHomeItems.KeyUp
        ' raise double click when user presses enter
        If e.KeyCode = Keys.Enter AndAlso lvwHomeItems.SelectedItems.Count > 0 Then
            lvwHomeItems_DoubleClick(sender, e)
        End If
    End Sub

    ''' <summary>Scans for plugins.</summary>
    Private Sub ScanForActivityLaunchers()
        ' exit the subroutine if there is no ActivityHostControl hosting this activity.
        ' proceeding otherwise can cause an exception
        If Me.HostControl Is Nothing Then Return

        ' get all the activity launchers in the host control
        For Each plugin In DirectCast(Me.Parent, ActivityHostControl).Plugins
            For Each al In plugin.ActivityLaunchers

                Dim imagekey = al.ActivityID + "@" + plugin.UniqueID
                Dim launcherInfo = New String() {al.ActivityID, al.InitializationCommand}

                iml48px.Images.Add(imagekey, al.Icon)

                lvwHomeItems.Items.Add(New ListViewItem() With {.Text = al.Caption,
                                                                .ToolTipText = al.Description,
                                                                .ImageKey = imagekey,
                                                                .Tag = launcherInfo})
            Next
        Next
    End Sub
End Class
