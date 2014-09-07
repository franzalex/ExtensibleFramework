Imports System.Windows.Forms
Imports System.Drawing

' LOW:  Add methods to get result of running the control before it is stopped

Public Class ActivityControl
    Inherits Windows.Forms.UserControl

    Dim _hostControl As ActivityHostControl

    ''' <summary>Occurs when the <see cref="ActivityControl"/> is started.</summary>
    Public Event Start As EventHandler(Of StartEventArgs)
    ''' <summary>Occurs when the <see cref="ActivityControl"/> is suspended.</summary>
    ''' <remarks>
    ''' This event is raised when the <see cref="ActivityHostControl"/> is minimized or loses focus.
    ''' </remarks>
    Public Event Paused As EventHandler
    ''' <summary>Occurs when the <see cref="ActivityControl"/> is resumed.</summary>
    Public Event Resumed As EventHandler
    ''' <summary>Occurs when the <see cref="ActivityControl"/> is stopping.</summary>
    ''' <remarks>
    ''' This event is typically raised when the user is navigating away from this <see cref="ActivityControl"/>
    ''' or the <see cref="ActivityHostControl"/> is being closed.
    ''' </remarks>
    Public Event Stopping As EventHandler(Of StoppingEventArgs)
    ''' <summary>Occurs when the <see cref="ActivityControl"/> is stopped.</summary>
    ''' <remarks>
    ''' This event is raised when the <see cref="ActivityHostControl"/> detemines that this
    ''' <see cref="ActivityControl"/> is no longer the active activity.
    ''' </remarks>
    Public Event [Stop] As EventHandler(Of StopEventArgs)
    ''' <summary>
    ''' Occurs when the <see cref="ActivityControl"/> is started after it has been previously stopped.
    ''' </summary>
    Public Event Restart As EventHandler(Of RestartEventArgs)

    Public Sub New()
        ' initialize defaults
        MyBase.Font = SystemFonts.MessageBoxFont
    End Sub

    ''' <summary>Gets the ID of this <seealso cref="ActivityControl"/>.</summary>
    Public Overridable ReadOnly Property ID As String
        Get
            Return Me.GetType().FullName
        End Get
    End Property

    ''' <summary>Gets the window hosting this <see cref="ActivityControl"/>.</summary>
    Public ReadOnly Property HostControl As ActivityHostControl
        Get
            Return _hostControl
        End Get
    End Property

    ''' <summary>Gets an ActivityLauncher that can be used to start this control.</summary>
    ''' <value>An ActivityLauncher that can be used to start this control.</value>
    Public Overridable ReadOnly Property Launcher As ActivityLauncher
        Get
            Static _launcher As ActivityLauncher

            ' create a launcher if none has been created yet
            If _launcher Is Nothing Then
                _launcher = New ActivityLauncher(Me.Text, Me.Icon)
            End If

            Return _launcher
        End Get
    End Property

    ''' <summary>Gets the icon associated with this <see cref="ActivityControl"/>..</summary>
    Public Overridable ReadOnly Property Icon As Image
        Get
            Return New Bitmap(1, 1)
        End Get
    End Property

    ''' <summary>Gets the result of running this <seealso cref="ActivityControl"/>.</summary>
    Public Overridable ReadOnly Property Result As Object
        Get
            Return Nothing
        End Get
    End Property

    ''' <summary>Gets the text associated with this control.</summary>
    ''' <returns>The text associated with this control.</returns>
    Public Overridable Shadows ReadOnly Property Text As String
        Get
            Return ""
        End Get
    End Property

#Region "Event Raisers"
    ''' <summary>Raises the <see cref="E:Started" /> event.</summary>
    ''' <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    Protected Overridable Sub OnStart(e As StartEventArgs)
        RaiseEvent Start(Me, e)
    End Sub

    ''' <summary>Raises the <see cref="E:Suspended" /> event.</summary>
    ''' <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    Protected Overridable Sub OnPaused(e As EventArgs)
        RaiseEvent Paused(Me, e)
    End Sub

    ''' <summary>Raises the <see cref="E:Resumed" /> event.</summary>
    ''' <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    Protected Overridable Sub OnResumed(e As EventArgs)
        RaiseEvent Resumed(Me, e)
    End Sub

    ''' <summary>Raises the <see cref="E:Stopping" /> event.</summary>
    ''' <param name="e">The <see cref="StoppingEventArgs"/> instance containing the event data.</param>
    Protected Overridable Sub OnStopping(e As StoppingEventArgs)
        RaiseEvent Stopping(Me, e)
    End Sub

    ''' <summary>Raises the <see cref="E:Stopped" /> event.</summary>
    ''' <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    Protected Overridable Sub OnStop(e As StopEventArgs)
        RaiseEvent Stop(Me, e)
    End Sub

    ''' <summary>Raises the <see cref="E:Restarted" /> event.</summary>
    ''' <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    Protected Overridable Sub OnRestart(e As RestartEventArgs)
        RaiseEvent Restart(Me, e)
    End Sub
#End Region

#Region "State changing methods"
    ''' <summary>Starts the activity.</summary>
    ''' <param name="initializationCommand">The initialization command.</param>
    Public Sub StartActivity(initializationCommand As String)
        Me.OnStart(New StartEventArgs(initializationCommand))
    End Sub

    ''' <summary>Pauses the activity.</summary>
    Public Sub PauseActivity()
        Me.OnPaused(EventArgs.Empty)
    End Sub

    ''' <summary>Resumes the activity.</summary>
    Public Sub ResumeActivity()
        Me.OnResumed(EventArgs.Empty)
    End Sub

    ''' <summary>Determines whether this instance can be stopped.</summary>
    ''' <returns><c>true</c> if this <seealso cref="ActivityControl"/> can be stopped.</returns>
    Public Function CanStopActivity() As Boolean
        Dim e = New StoppingEventArgs()
        Me.OnStopping(e)
        Return Not e.Cancel
    End Function

    ''' <summary>Stops the activity.</summary>
    ''' <param name="performCleanup">A resource cleanup will be performed if set to <c>true</c>.</param>
    ''' <returns>The state of the control prior to stopping</returns>
    Public Function StopActivity(performCleanup As Boolean) As Object
        Dim e = New StopEventArgs(performCleanup)
        Me.OnStop(e)
        Return e.State
    End Function

    ''' <summary>Restarts the activity.</summary>
    ''' <param name="state">The state to restore the <seealso cref="ActivityControl"/> to.</param>
    Public Sub RestartActivity(state As Object)
        Me.OnRestart(New RestartEventArgs(state))
    End Sub
#End Region

    Private Sub ActivityControl_ParentChanged(sender As Object, e As EventArgs) Handles Me.ParentChanged
        _hostControl = Nothing

        Dim p = Me.Parent

        ' iteratively search parents for an instance of ActivityHostControl
        Do While p IsNot Nothing
            Dim ahc = TryCast(p, ActivityHostControl)

            If ahc IsNot Nothing Then
                _hostControl = ahc
                Exit Do
            Else
                p = p.Parent
            End If
        Loop
    End Sub
End Class
