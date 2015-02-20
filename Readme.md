# Implementing Extensible Application Framework Plugins

This document contains instructions on how to create, debug and deploy plugins
for the Extensible Application Framework.


## Create and Configure Project

### Project References
Plugins are class libraries (.DLL files) whose member classes inherit classes 
from the `ExtensibleFramework.Core` namespace. This requires, therefore, that
you add a reference to the `ExtensibleFramework.Core.dll` file in your
project.

Follow the process of adding a reference to a project in Visual Studio for the
language you are developing in to add a reference to the 
`ExtensibleFramework.Core.dll` file.  
Optionally, set the *Copy To Local* of the reference to **False**. Though the
Extensible Framework program is designed to load libraries only when required,
doing this will greatly improve the performance of the application.


### The INFO.TXT File
In order to determine which class libraries are plugins, the extensible 
framework application scans all directories for files whose names match the 
expression *.info.txt (e.g. *myFirstPlugin.info.txt*). 

Each line of this INFO.TXT file should contain the  file name of a plugin 
relative to the directory in which the INFO.TXT file is located but preferably
in the same directory as the plugin's .DLL file.

#### Creating the INFO.TXT File
Though it is fairly easy to create the INFO.TXT file manually, an easier and
more robust way of doing this is to have Visual Studio generate the file when
building the project using build events.

 1. Right click on the project in Solution Explorer and click on Properties.
 2. Go to the *Compile* tab and then click on the *Build Events* button.
 3. Enter the following in the Post-build event command line text box:  
    `CMD.EXE /C ECHO "$(TargetFileName)" > "$(TargetDir)$(TargetName).info.txt"`
 4. Click on OK to save the configuration.


With this, an INFO.TXT file with the same name as your output file in the build
output directory. The INFO.TXT file will also be automatically populated with 
the file name of the build output (the DLL file containing the plugin).


## Creating the Plugin
All of the classes required to create a fully-functional plugin are in the 
`ExtensibleFramework.Core` namespace. Creating a plugin is just a matter of
inheriting these classes and modifying the inherited methods to suit your needs.

### The plugin descriptor class
The plugin descriptor class serves as the hub of all interaction with your 
plugin. Ideally, each plugin assembly will have only one plugin descriptor 
class. However, it is possible to put multiple plugin descriptor classes, and by
extension, multiple plugins in one assembly.

Each plugin descriptor must inherit the `ExtensibleFramework.Core.Plugin` 
class and implement properties and methods that specify among other things, the 
name, ID and activities present within the plugin.


#### Creating a plugin descriptor class
1. Click on `Project > Add Class...` and add a new class to your project.
2. Add the following line after the class definition line to inherit the base 
   plugin descriptor class.  
        `Inherits ExtensibleFramework.Core.Plugin`  
3. Complete the code that was automatically generated when you inherited the
   `Plugin` class.  
   Optionally, you can override additional methods and properties to fine-tune
   the way your plugin descriptor (and hence your plugin) works.

The sample implementation of a plugin descriptor class is as shown below.

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
                If launchers.Length = 0 Then
                    ' create two launchers for the two main activities in the plugin

                    Dim launchers(2) As ActivityLauncher

                    launchers(0) = New ActivityLauncher("Demo.Plugin.NotepadActivity",
                                                        "Notepad", My.Resources.TextEditor)
                    launchers(1) = New ActivityLauncher("Demo.Plugin.SampleActivity",
                                                        "Timer", My.Resources.Clock)
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
                Case "Demo.Plugin.SampleActivity"
                    Return New SampleActivity()

                Case "Demo.Plugin.NotepadActivity"
                    Return New TextEditorActivity()

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
                Return "Demo.Plugin.SampleActivity"
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


### Activities
An activity is a container control hosting one or more controls to perform a 
specific task. 

Activities inherit the `ExtensibleFramework.Core.ActivityControl` class.
This base class implements a set of events and properties that can be used to
interact with the user, other activities in the plugin, as well as activities in
other plugins.

#### Creating an Activity
1. Click on `Project > Add New Item...`. 
2. Select **Inherited User Control** (Installed > Common Items > Windows Forms),
   enter the name of the activity and click on **Add**.
3. In the Inheritance Picker dialog box that opens, select **ActivityControl**
   component (namespace **ExtensibleFramework.Core**) and then click on **OK**.
4. Add controls in the design view to suit your purpose.
5. Switch to the code view and handle the events you want.

The code below outlines the basic events that can be implemented by an activity
in order to interact with the Extensible Application Framework program.

    Public Class SampleActivity
        Inherits ExtensibleFramework.Core.ActivityControl
    
        Private Sub SampleActivity_Initializing(sender As Object, 
                                                e As ExtensibleFramework.Core.InitializingEventArgs) Handles Me.Initializing
            ' the Initializing event is raised when the control is being prepared to be displayed.
            ' it is recommended to load and set default values of controls here.
            ' resource intensive operations should be reserved for the ActivityControl.Started event
    
            ' initialize the activity text in code; 
            ' it cannot be set in design mode
            Me.Text = "Sample Activity"
        End Sub
    
        Private Sub SampleActivity_Paused(sender As Object, e As EventArgs) Handles Me.Paused
            ' the Paused event is raised when the control's window is minimized or loses focus
            ' pause activities such as rendering and screen updates here 
            '   since the user is not likely to see or interact with them
        End Sub
    
        Private Sub SampleActivity_Restart(sender As Object, e As ExtensibleFramework.Core.RestartEventArgs) Handles Me.Restart
            ' The Restart event is raised when the activity is started after being previously stopped.
            ' The most likely way this event will be raised is when the user navigates backwards to
            '   ActivityControl.Stopped event will be returned in the RestartedEventArgs.State property.
            '   You can use this to restore the state of your activity.
        End Sub
    
        Private Sub SampleActivity_Resumed(sender As Object, e As EventArgs) Handles Me.Resumed
            ' the Resumed event is raised when the control's window regains focus
            '   or is restored after being minimized
            ' Resume rendering and screen updates here for user interaction
        End Sub
    
        Private Sub SampleActivity_Started(sender As Object, e As EventArgs) Handles Me.Started
            ' The Started event is raised when the control has been initialized and is displayed to the 
            '   user for interaction.
        End Sub
    
        Private Sub SampleActivity_Stopped(sender As Object, e As ExtensibleFramework.Core.StoppedEventArgs) Handles Me.Stopped
            ' This event is raised when this activity has been stopped and removed from the window.
            ' A stopped activity may or may not be resumed so it is recommended to save volatile values 
            '   here. You should also stop all processing in anticipation of termination.
            ' The state of some controls such as caret position, scroll position and focused controls
            '   can be persisted in the StoppedEventArgs.State property in the event of a restart.
            ' The StoppedEventArgs.CleanUp property indicates whether large objects should be disposed of
            '   or not. It is set to True when the user navigates backwards and may not return to this
            '   activity.
        End Sub
    
        Private Sub TimerActivity_Stopping(sender As Object, e As ExtensibleFramework.Core.StoppingEventArgs) Handles Me.Stopping
            ' Raised just before the activity is stopped. It allows you to prompt user to save open documents, etc.
            ' Setting the StoppingEventArgs.Cancel property to True prevents the activity from being stopped.
        End Sub
    End Class
