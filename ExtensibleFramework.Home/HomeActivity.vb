Imports ExtensibleFramework.Core

Public Class HomeActivity
    ' IMPORTANT: You *MUST* inherit from the ExtensibleFramework.Core.ActivityControl
    Inherits ExtensibleFramework.Core.ActivityControl

    Public Overrides ReadOnly Property Text As String
        Get
            Return "Home"
        End Get
    End Property

    Private Sub HomeActivity_Start(sender As Object, e As Core.StartEventArgs) Handles Me.Start
        ' Initialize control by getting all activity launchers in plug-ins currently 
        ' loaded and display them in the ListView

        ' exit the subroutine if there is no ActivityHostControl hosting this activity.
        ' proceeding otherwise can cause an exception
        If Me.HostControl Is Nothing Then Return

        ' get all the activity launchers in the host control
        For Each plugin In DirectCast(Me.Parent, ActivityHostControl).Plugins
            For Each al In plugin.ActivityLaunchers

                Dim imagekey = plugin.UniqueID + "@" + al.ActivityID
                Dim launcherInfo = New String() {al.ActivityID, al.InitializationCommand}

                iml32px.Images.Add(imagekey, al.Icon)

                lvwHomeItems.Items.Add(New ListViewItem() With {.Text = al.Caption,
                                                                .ToolTipText = al.Description,
                                                                .ImageKey = imagekey,
                                                                .Tag = launcherInfo})
            Next
        Next
    End Sub

    Private Sub HomeActivity_Stop(sender As Object, e As StopEventArgs) Handles Me.Stop
        ' release resources by clearing all the loaded items
        iml32px.Images.Clear()
        lvwHomeItems.Items.Clear()
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
End Class
