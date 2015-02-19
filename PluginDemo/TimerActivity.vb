Imports System.Windows.Forms

Public Class TimerActivity
    Inherits ExtensibleFramework.Core.ActivityControl

    Dim stopwatch As Stopwatch
    Dim oldTime As TimeSpan

    ''' <summary>Displays the time elapsed on the stopwatch.</summary>
    Private Sub DisplayTheTime()
        Dim dt = New Date(oldTime.Add(stopwatch.Elapsed).Ticks)
        lblTimer.Text = String.Format("{0:HH:mm:ss.f}", dt)
    End Sub

    Private Sub TimerActivity_Initializing(sender As Object, e As ExtensibleFramework.Core.InitializingEventArgs) Handles Me.Initializing
        ' the Initializing event is raised when the control is being prepared to be displayed.
        ' it is recommended to load and set default values of controls here.
        ' resource intensive operations should be reserved for the ActivityControl.Started event

        ' initialize the stopwatch
        stopwatch = New Stopwatch()

        ' try to get the previous stopwatch value
        Dim timerValue = Me.Settings.GetValue("timer.Ticks", 0L)
        oldTime = New TimeSpan(timerValue)

        ' display the time on the label
        DisplayTheTime()

        Me.Text = "Timer"
        btnReset.Enabled = Not stopwatch.IsRunning
    End Sub

    Private Sub TimerActivity_Paused(sender As Object, e As EventArgs) Handles Me.Paused
        ' the Paused event is raised when the control's window is minimized or loses focus
        ' pause activities such as rendering and screen updates here 
        '   since the user is not likely to see or interact with them

        tmrUpdate.Stop()
    End Sub

    Private Sub TimerActivity_Restart(sender As Object, e As ExtensibleFramework.Core.RestartEventArgs) Handles Me.Restart
        ' The Restart event is raised when the activity is started after being previously stopped.
        ' The most likely way this event will be raised is when the user navigates backwards to
        '   this activity.
        ' Values that were persisted in the StoppedEventArgs.State property during the 
        '   ActivityControl.Stopped event will be returned in the RestartedEventArgs.State property.
        '   You can use this to restore the state of your activity.

        ' restore the stopwatch's running state
        If CBool(e.State) = True Then
            stopwatch.Start()
            tmrUpdate.Start()
        End If
    End Sub

    Private Sub TimerActivity_Resumed(sender As Object, e As EventArgs) Handles Me.Resumed
        ' the Resumed event is raised when the control's window regains focus
        '   or is restored after being minimized
        ' Resume rendering and screen updates here for user interaction

        If stopwatch.IsRunning Then
            tmrUpdate.Start()
        End If
    End Sub

    Private Sub TimerActivity_Started(sender As Object, e As EventArgs) Handles Me.Started
        ' The Started event is raised when the control has been initialized and is displayed to the 
        '   user for interaction.

        MessageBox.Show("This is the Timer Activity." & vbCrLf &
                        "This activity demonstrates basic event handling on the ActivityControl.")
    End Sub

    Private Sub TimerActivity_Stopped(sender As Object, e As ExtensibleFramework.Core.StoppedEventArgs) Handles Me.Stopped
        ' This event is raised when this activity has been stopped and removed from the window.
        ' A stopped activity may or may not be resumed so it is recommended to save volatile values 
        '   here. You should also stop all processing in anticipation of termination.
        ' The state of some controls such as caret position, scroll position and focused controls
        '   can be persisted in the StoppedEventArgs.State property in the event of a restart.
        ' The StoppedEventArgs.CleanUp property indicates whether large objects should be disposed of
        '   or not. It is set to True when the user navigates backwards and may not return to this
        '   activity.

        ' save the timer's running state
        e.State = stopwatch.IsRunning

        stopwatch.Stop()
        tmrUpdate.Stop()
        Me.Settings.SetValue("timer.Ticks", oldTime.Add(stopwatch.Elapsed).Ticks)
    End Sub

    Private Sub TimerActivity_Stopping(sender As Object, e As ExtensibleFramework.Core.StoppingEventArgs) Handles Me.Stopping
        ' Raised just before the activity is stopped. It allows you to prompt user to save open documents, etc.
        ' Setting the StoppingEventArgs.Cancel property to True prevents the activity from being stopped.

        If stopwatch.IsRunning AndAlso
            MessageBox.Show("Navigating will cause the timer to stop." & vbCrLf &
                            "Do you want to continue?",
                            "Timer Alert", MessageBoxButtons.YesNo) = DialogResult.No Then
            ' user does not want to stop the timer
            e.Cancel = True
        End If
    End Sub

    Private Sub btnStart_Click(sender As Object, e As EventArgs) Handles btnStart.Click
        ' start & stop the timer
        If Not stopwatch.IsRunning Then
            stopwatch.Start()
            tmrUpdate.Start()
            btnStart.Text = "&Stop"

        Else
            stopwatch.Stop()
            tmrUpdate.Stop()
            btnStart.Text = "&Start"
        End If

        btnReset.Enabled = Not stopwatch.IsRunning
    End Sub

    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        ' reset the timer
        stopwatch.Reset()
        oldTime = New TimeSpan(0)
        DisplayTheTime()
    End Sub

    Private Sub tmrUpdate_Tick(sender As Object, e As EventArgs) Handles tmrUpdate.Tick
        tmrUpdate.Stop()
        DisplayTheTime()
        tmrUpdate.Start()
    End Sub
End Class
