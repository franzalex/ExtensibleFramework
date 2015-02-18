''' <summary>Provides data for the <see cref="ActivityControl.Initializing"/> event.</summary>
<DebuggerStepThrough()>
Public Class InitializingEventArgs
    Inherits EventArgs

    Dim _initCmd As String

    ''' <summary>Initializes a new instance of the <see cref="InitializingEventArgs"/> class.</summary>
    ''' <param name="initializationCommand">The initialization command.</param>
    Public Sub New(initializationCommand As String)
        _initCmd = initializationCommand
    End Sub

    ''' <summary>Gets the initialization command.</summary>
    ''' <value>The initialization command.</value>
    Public ReadOnly Property InitializationCommand As String
        Get
            Return _initCmd
        End Get
    End Property
End Class

''' <summary>Provides data for the <see cref="ActivityControl.Stopping"/> event.</summary>
<DebuggerStepThrough()>
Public Class StoppingEventArgs
    Inherits System.ComponentModel.CancelEventArgs

    ''' <summary>Initializes a new instance of the <see cref="StoppingEventArgs"/> class.</summary>
    Public Sub New()
        Me.New(False)
    End Sub

    ''' <summary>Initializes a new instance of the <see cref="StoppingEventArgs"/> class.</summary>
    ''' <param name="cancel">true to cancel the event; otherwise, false.</param>
    Public Sub New(cancel As Boolean)
        MyBase.New(cancel)
    End Sub
End Class

''' <summary>Provides data for the <seealso cref="ActivityControl.Stopped"/> event.</summary>
<DebuggerStepThrough()>
Public Class StoppedEventArgs
    Inherits EventArgs

    Dim _cleanup As Boolean

    ''' <summary>Initializes a new instance of the <see cref="StoppedEventArgs" /> class.</summary>
    ''' <param name="cleanup">
    ''' If set to <c>true</c> a clean up should be performed in the 
    ''' <see cref="ActivityControl.Stopped" /> event.
    ''' </param>
    Public Sub New(cleanup As Boolean)
        _cleanup = cleanup
    End Sub

    ''' <summary>
    ''' Gets a value indicating whether large objects should be cleaned up.
    ''' </summary>
    ''' <value><c>true</c> if a clean up should be performed; otherwise, <c>false</c>.</value>
    ''' <remarks>
    ''' <para>
    ''' This property is set to <c>true</c> when the user navigates backwards and the 
    ''' <see cref="ActivityControl"/> is not likely to be displayed in its current state again. 
    ''' It should be treated as a signal to start performing clean-up of resource-intensive objects 
    ''' and should a precursor to the <see cref="ActivityControl.Dispose" /> method.
    ''' </para>
    ''' </remarks>
    Public ReadOnly Property CleanUp As Boolean
        Get
            Return _cleanup
        End Get
    End Property

    ''' <summary>Gets or sets the state of the <see cref="ActivityControl"/> prior to stopping.</summary>
    ''' <value>The state of the <see cref="ActivityControl"/>.</value>
    ''' <remarks>
    ''' This object will be used to restart the <seealso cref="ActivityControl"/> when the user
    ''' returns to it later.
    ''' </remarks>
    Public Property State As Object
End Class

''' <summary>Provides data for the <see cref="ActivityControl.Restart"/> event.</summary>
<DebuggerStepThrough()>
Public Class RestartEventArgs
    Inherits EventArgs

    Dim _state As Object

    ''' <summary>Initializes a new instance of the <see cref="RestartEventArgs"/> class.</summary>
    ''' <param name="state">The state to restore the <see cref="ActivityControl"/> to.</param>
    Public Sub New(state As Object)
        _state = state
    End Sub

    ''' <summary>Gets the state to restore the <see cref="ActivityControl"/> to.</summary>
    ''' <remarks>
    ''' The value of this property is what was previously saved to the <seealso cref="StoppedEventArgs.State"/>
    ''' property in the <seealso cref="E:ActivityControl.Stopping"/> event.
    ''' </remarks>
    Public ReadOnly Property State As Object
        Get
            Return _state
        End Get
    End Property
End Class