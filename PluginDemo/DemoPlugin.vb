Imports ExtensibleFramework.Core

Public Class DemoPlugin
    Inherits ExtensibleFramework.Core.Plugin


    Dim launchers() As ActivityLauncher

    ''' <summary>Gets the description of the plugin.</summary>
    Public Overrides ReadOnly Property Description As String
        Get
            Return "Sample Extensible Application Framework plugin"
        End Get
    End Property

    ''' <summary>Gets the name of the plugin.</summary>
    Public Overrides ReadOnly Property Name As String
        Get
            Return "Demo Plugin"
        End Get
    End Property

    ''' <summary>Gets the launchers for launching activities directly for this plugin.</summary>
    Public Overrides ReadOnly Property ActivityLaunchers As IEnumerable(Of ActivityLauncher)
        Get
            If launchers Is Nothing OrElse launchers.Length = 0 Then
                ' create two launchers for the two main activities in the plugin
                launchers = {New ActivityLauncher("Demo.Plugin.Pong",
                                                  "Pong - Game", My.Resources.PongIcon),
                             New ActivityLauncher("Demo.Plugin.Timer",
                                                  "Timer", My.Resources.Clock)
                            }
            End If

            Return launchers
        End Get
    End Property

    ''' <summary>
    ''' Creates the activity associated with the specified <paramref name="activityID" />.
    ''' </summary>
    ''' <param name="activityID">The activity ID of the activity to be created.</param>
    ''' <returns>An instance of <seealso cref="ActivityControl" />.</returns>
    Public Overrides Function CreateActivity(activityID As String) As ActivityControl

        ' create an instance of the activity control whose ID is specified
        Select Case activityID
            Case "Demo.Plugin.Timer"
                Return New TimerActivity()

            Case "Demo.Plugin.Pong"
                Return New PongGame()

            Case Else
                Return Nothing
        End Select
    End Function

    ''' <summary>
    ''' Gets the ID of the <see cref="ActivityControl" /> to use for executing the specified command.
    ''' </summary>
    ''' <param name="command">The command to be evaluated.</param>
    ''' <returns>
    ''' The ID of the <see cref="ActivityControl" /> to use for executing <paramref name="command" />.
    ''' </returns><remarks>
    ''' The <paramref name="command" /> parameter will be will be passed to the
    ''' <see cref="Plugin.RunCommand" /> method if a <c>null</c> or empty string is returned.
    ''' </remarks>
    Public Overrides Function GetActivityForCommand(command As String) As String
        ' try to determine if a given command can be run

        If command.StartsWith("run-timer") Then
            Return "Demo.Plugin.TimerActivity"
        ElseIf command.StartsWith("edit-text") Then
            Return "Demo.Plugin.NotepadActivity"
        Else
            Return ""
        End If
    End Function

    ''' <summary>Runs the specified command.</summary>
    ''' <param name="command">The command to be run.</param>
    ''' <returns>The result produced from running the command.</returns>
    Public Overrides Function RunCommand(command As String) As Object
        ' no commands are run outside activities so return Nothing for every command that is passed.
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
        ' shell off to the GetActivityForCommand() method to determine
        ' whether the plugin supports the given command.

        Return Not String.IsNullOrEmpty(Me.GetActivityForCommand(command))
    End Function
End Class
