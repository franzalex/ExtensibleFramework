Imports ExtensibleFramework.Core

Public Class Home
    Inherits ExtensibleFramework.Core.Plugin

    Dim _mainActivities As List(Of ExtensibleFramework.Core.ActivityLauncher)
    Dim homeActivityID As String

    Public Sub New()
        ' this is a crude way of getting the activity's ID
        ' it disposes the created control after use
        Using hac As New HomeActivity()
            homeActivityID = hac.ID
        End Using

        ' create an empty list of activity startup info
        _mainActivities = New List(Of Core.ActivityLauncher)()
    End Sub

    ''' <summary>
    ''' Creates the activity associated with the specified <paramref name="activityID" />.
    ''' </summary>
    ''' <param name="activityID">The activity ID of the activity to be created.</param>
    ''' <returns>An instance of <seealso cref="ActivityControl" />.</returns>
    Public Overrides Function CreateActivity(activityID As String) As Core.ActivityControl
        ' create a new instance of the activity whose ID is passed
        Select Case activityID
            Case homeActivityID
                Return New HomeActivity()
            Case Else
                Return Nothing
        End Select
    End Function

    ''' <summary>Gets the description of the plugin.</summary>
    Public Overrides ReadOnly Property Description As String
        Get
            Return "Home screen plugin for Extensible Application Framework"
        End Get
    End Property

    ''' <summary>
    ''' Gets the ID of the <see cref="ActivityControl" /> to use for executing the specified command.
    ''' </summary>
    ''' <param name="command">The command to be evaluated.</param>
    ''' <returns>
    ''' The ID of the <see cref="ActivityControl" /> to use for executing <paramref name="command" />.
    ''' </returns><remarks>
    ''' If a non-empty string is returned and <seealso cref="Plugin.SupportsCommand" /> is <c>true</c>,
    ''' the activity whose ID is returned will be initialized with <paramref name="command" />.
    ''' On the other hand, if an empty string is returned, <paramref name="command" /> will be passed
    ''' to <see cref="Plugin.RunCommand" />.
    ''' </remarks>
    Public Overrides Function GetActivityForCommand(command As String) As String
        ' the only command known is 'go-home'.
        ' when the activity for that command is requested, return the home activity's ID
        If command.ToLower() = "go-home" Then
            Return homeActivityID
        Else
            Return Nothing
        End If
    End Function

    ''' <summary>Gets the activities that can be launched directly from this plugin.</summary>
    Public Overrides ReadOnly Property ActivityLaunchers As IEnumerable(Of Core.ActivityLauncher)
        Get
            Return _mainActivities.AsReadOnly() ' return as readonly to prevent modifications
        End Get
    End Property

    ''' <summary>Gets the name of the plugin.</summary>
    Public Overrides ReadOnly Property Name As String
        Get
            Return "Extensible Application Framework Home"
        End Get
    End Property

    ''' <summary>Runs the specified command.</summary>
    ''' <param name="command">The command to be run.</param>
    ''' <returns>The result produced from running the command.</returns>
    Public Overrides Function RunCommand(command As String) As Object
        ' this plugin does not run any special commands
        Return Nothing
    End Function

    ''' <summary>
    ''' Gets a value indicating whether the plugin supports the specified command.
    ''' </summary>
    ''' <param name="command">The command to be evaluated.</param>
    ''' <returns>
    '''   <c>true</c> if the plugin supports the specified command; else <c>false</c>.
    ''' </returns>
    Public Overrides Function SupportsCommand(command As String) As Boolean
        ' this plugin supports only the "go-home" command
        Return command.ToLower() = "go-home"
    End Function
End Class
