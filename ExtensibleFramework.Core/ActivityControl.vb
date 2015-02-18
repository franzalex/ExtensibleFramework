Imports System.Windows.Forms
Imports System.Drawing

' LOW:  Add methods to get result of running the control before it is stopped

Public Class ActivityControl
    Inherits Windows.Forms.UserControl

    Dim _hostControl As ActivityHostControl
    Dim _settings As Core.Settings
    Friend pluginID As String
    Dim _state As ActivityState

    ''' <summary>Occurs when the <see cref="ActivityControl"/> is being initialized.</summary>
    Public Event Initializing As EventHandler(Of InitializingEventArgs)
    ''' <summary>Occurs <see cref="ActivityControl"/> has been initialized and is visible to the user.</summary>
    Public Event Started As EventHandler
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
    ''' This event is raised when the <see cref="ActivityHostControl"/> determines that this
    ''' <see cref="ActivityControl"/> is no longer the active activity.
    ''' </remarks>
    Public Event Stopped As EventHandler(Of StoppedEventArgs)
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

    ''' <summary>Gets the ActivityLaunchers that can be used to start this control.</summary>
    ''' <value>An array of <see cref="ActivityLauncher"/> that can be used to start this control.</value>
    Public Overridable ReadOnly Property Launchers As ActivityLauncher()
        Get
            Static _launcher As ActivityLauncher

            ' create a launcher if none has been created yet
            If _launcher Is Nothing Then
                _launcher = New ActivityLauncher(Me.ID, Me.Text, Me.Icon)
            End If

            Return {_launcher}
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

    ''' <summary>Gets the plugin's settings.</summary>
    Public Property Settings As Settings
        Get
            Return _settings
        End Get
        Protected Friend Set(value As Settings)
            _settings = value
        End Set
    End Property

    ''' <summary>Gets the state of the <see cref="ActivityControl"/>.</summary>
    Public ReadOnly Property State As ActivityState
        Get
            Return _state
        End Get
    End Property

    ''' <summary>Gets the text associated with this control.</summary>
    ''' <returns>The text associated with this control.</returns>
    Public Overridable Shadows Property Text As String
        Get
            Return If(MyBase.Text.IsNullOrEmpty(), Me.Name, MyBase.Text)
        End Get
        Protected Set(value As String)
            MyBase.Text = value
        End Set
    End Property

#Region "Event Raisers"
    ''' <summary>Raises the <see cref="E:Initializing" /> event.</summary>
    ''' <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    Protected Overridable Sub OnInitializing(e As InitializingEventArgs)
        _state = ActivityState.Initializing
        RaiseEvent Initializing(Me, e)
        _state = ActivityState.Initialized
    End Sub

    ''' <summary>Raises the <see cref="E:Started" /> event.</summary>
    ''' <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    Protected Overridable Sub OnStarted(e As EventArgs)
        _state = ActivityState.Running
        RaiseEvent Started(Me, e)
    End Sub

    ''' <summary>Raises the <see cref="E:Suspended" /> event.</summary>
    ''' <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    Protected Overridable Sub OnPaused(e As EventArgs)
        _state = ActivityState.Paused
        RaiseEvent Paused(Me, e)
    End Sub

    ''' <summary>Raises the <see cref="E:Resumed" /> event.</summary>
    ''' <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    Protected Overridable Sub OnResumed(e As EventArgs)
        _state = ActivityState.Running
        RaiseEvent Resumed(Me, e)
    End Sub

    ''' <summary>Raises the <see cref="E:Stopping" /> event.</summary>
    ''' <param name="e">The <see cref="StoppingEventArgs"/> instance containing the event data.</param>
    Protected Overridable Sub OnStopping(e As StoppingEventArgs)
        _state = ActivityState.Stopping
        RaiseEvent Stopping(Me, e)
    End Sub

    ''' <summary>Raises the <see cref="E:Stopped" /> event.</summary>
    ''' <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    Protected Overridable Sub OnStopped(e As StoppedEventArgs)
        _state = ActivityState.Stopped
        RaiseEvent Stopped(Me, e)
    End Sub

    ''' <summary>Raises the <see cref="E:Restarted" /> event.</summary>
    ''' <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    Protected Overridable Sub OnRestart(e As RestartEventArgs)
        _state = ActivityState.Restarting
        RaiseEvent Restart(Me, e)
        _state = ActivityState.Running
    End Sub
#End Region

#Region "State changing methods"
    ''' <summary>Initializes the activity.</summary>
    ''' <param name="initializationCommand">The initialization command.</param>
    Public Sub Initialize(initializationCommand As String)
        Me.OnInitializing(New InitializingEventArgs(initializationCommand))
    End Sub

    ''' <summary>Starts the activity.</summary>
    Public Sub StartActivity()
        Me.OnStarted(EventArgs.Empty)
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
        Dim e = New StoppedEventArgs(performCleanup)
        Me.OnStopped(e)
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

Public Enum ActivityState
    ''' <summary>Indicates that the <see cref="ActivityControl"/> is initializing.</summary>
    Initializing
    ''' <summary>
    ''' Indicates that the <see cref="ActivityControl" /> has been initialized 
    ''' but has not been displayed to the user.
    ''' </summary>
    Initialized
    ''' <summary>Indicates that the <see cref="ActivityControl"/> is currently active.</summary>
    Running
    ''' <summary>
    ''' Indicates that the <see cref="ActivityControl" /> has been paused due to its
    ''' parent form losing focus.
    ''' </summary>
    Paused
    ''' <summary>
    ''' Indicates that the <see cref="ActivityControl" /> is being restarted 
    ''' after being stopped previously.
    ''' </summary>
    Restarting
    ''' <summary>Indicates that the <see cref="ActivityControl" /> is being stopped.</summary>
    Stopping
    ''' <summary>
    ''' Indicates that the <see cref="ActivityControl" /> has been stopped
    ''' and is no longer visible to the user.
    ''' </summary>
    Stopped
End Enum
