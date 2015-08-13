Imports System.ComponentModel
Imports Size = System.Drawing.Size
Imports Regex = System.Text.RegularExpressions.Regex

Public Class ActivityHostControl
    Inherits System.Windows.Forms.Panel

    Dim parentForm As System.Windows.Forms.Form
    Dim _nav As Navigator
    Dim _plugins As Dictionary(Of String, Plugin)
    Dim activities As Dictionary(Of String, ActivityControl)
    Dim _currentActivity As ActivityControl
    Dim _activityResult As Object
    Dim _dataDir As String
    Dim settingsDir As String

    Public Sub New()
        Me.Size = Me.GetPreferredSize(New Size(0, 0))
        Me.Font = System.Drawing.SystemFonts.MessageBoxFont

        parentForm = Nothing
        _nav = New Navigator()
        _plugins = New Dictionary(Of String, Plugin)(StringComparer.InvariantCultureIgnoreCase)
        activities = New Dictionary(Of String, ActivityControl)(StringComparer.InvariantCultureIgnoreCase)
        _currentActivity = Nothing
        _activityResult = Nothing


        AddHandler _nav.IsNavigating, AddressOf Navigator_IsNavigating
        AddHandler _nav.HasNavigated, AddressOf Navigator_HasNavigated
    End Sub

    ''' <summary>
    ''' Gets the current <see cref="ActivityControl"/> being hosted in this <see cref="ActivityHostControl"/>.
    ''' </summary>
    <System.ComponentModel.Browsable(False)>
    Public ReadOnly Property CurrentActivity As ActivityControl
        Get
            Return _currentActivity
        End Get
    End Property

    ''' <summary>Gets the <see cref="Navigator"/> for this <see cref="ActivityHostControl"/>.</summary>
    Public ReadOnly Property Navigator As Navigator
        Get
            Return _nav
        End Get
    End Property

    ''' <summary>Gets the plugins detected by the control.</summary>
    Public ReadOnly Property Plugins As IEnumerable(Of Plugin)
        Get
            Return _plugins.Values.ToList().AsReadOnly()
        End Get
    End Property

    ''' <summary>Gets the result produced by running the last <see cref="ActivityControl"/>.</summary>
    <ComponentModel.Browsable(False)>
    Public ReadOnly Property RunActivityResult As Object
        Get
            Return _activityResult
        End Get
    End Property

    ''' <summary>Gets or sets the height and width of the control.</summary>
    ''' <returns>The <see cref="T:System.Drawing.Size" /> that represents the height and width of the control in pixels.</returns>
    '''   <PermissionSet>
    '''   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
    '''   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
    '''   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
    '''   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
    '''   </PermissionSet>
    <DefaultValue(GetType(Size), "240, 180")>
    Public Shadows Property Size As System.Drawing.Size
        Get
            Return MyBase.Size
        End Get
        Set(value As System.Drawing.Size)
            MyBase.Size = value
        End Set
    End Property

    ' methods

    ''' <summary>Adds the plugin version information to metadata.</summary>
    ''' <param name="plugin">The plugin whose version info will be added to <paramref name="metadata"/>.</param>
    ''' <param name="metadata">The metadata to be updated with the plugin version info.</param>
    Private Function AddVersionInfoToMetadata(plugin As Plugin, metadata As String) As String
        Dim verInfo = "{0} v{1}".FormatWith(plugin.Name, plugin.Version)
        Dim verLine = Regex.Match(metadata, plugin.Name & " v\d+.\d+.\d+.\d+")

        If verLine.Success Then
            Dim lastVer = Regex.Match(verLine.Value, "v\d+.\d+.\d+.\d+")

            If lastVer.Value.Substring(1) <> plugin.Version.ToString() Then
                ' matched version info line but last version is different from plugin version
                Return metadata.Replace(verLine.Value, verInfo)
            Else
                ' everything is okay
                Return metadata
            End If

        Else
            ' version info line is not present
            Return verInfo & vbCrLf & vbCrLf & metadata
        End If


    End Function

    ''' <summary>Finds the <see cref="ActivityControl" /> with the specified ActivityID.</summary>
    ''' <param name="activityID">The ID of the <seealso cref="ActivityControl" /> to return.</param>
    ''' <param name="searchAllPlugins">
    ''' If set to <c>true</c> all plugins will be searched for a match. Otherwise just cached 
    ''' activities will be searched for a match.
    ''' </param>
    ''' <returns></returns>
    Private Function FindActivityByID(activityID As String, Optional searchAllPlugins As Boolean = True) As ActivityControl
        ' create an instance of the activity if one hasn't been created already
        If Not activities.ContainsKey(activityID) AndAlso searchAllPlugins Then
            Dim foundActivites = From plugin In _plugins.Values
                                 Let activityCtrl = plugin.CreateActivity(activityID)
                                 Where activityCtrl IsNot Nothing
                                 Select New With {.Control = activityCtrl, .PluginID = plugin.UniqueID}

            If foundActivites.Any() Then
                Dim result1 = foundActivites.First()
                result1.Control.pluginID = result1.PluginID
                result1.Control.Settings = _plugins(result1.PluginID).Settings
                activities.Add(activityID, result1.Control)
            End If
        End If

        ' return activity with the specified ID. 
        Return If(activities.ContainsKey(activityID), activities(activityID), Nothing)

    End Function

    ''' <summary>Finds an <see cref="ActivityControl"/> that can run the specified command.</summary>
    ''' <param name="command">The command for which an <see cref="ActivityControl"/> is to be found.</param>
    ''' <returns>
    ''' The first found <see cref="ActivityControl"/> that can run the <paramref name="command"/> or
    ''' <c>null</c> if none was found
    ''' </returns>
    Private Function FindActivityByCommand(command As String) As String
        ' create an instance of the activity if one hasn't been created already
        Dim foundActivites = From plugin In _plugins.Values
                             Where plugin.SupportsCommand(command)
                             Select plugin.GetActivityForCommand(command)


        Return foundActivites.FirstOrDefault()
    End Function

    ''' <summary>Loads the plugin settings.</summary>
    Private Sub LoadPluginSettings()
        ' ensure the settings directory exists
        If _dataDir.IsNullOrEmpty() Then Return
        If Not System.IO.Directory.Exists(settingsDir) Then System.IO.Directory.CreateDirectory(settingsDir)

        ' load the settings for the plugin if it exists otherwise create a new settings file
        For Each pluginID In _plugins.Keys
            Dim filename = System.IO.Path.Combine(settingsDir, pluginID & ".json")
            Dim plugin = _plugins(pluginID)
            plugin.Settings = New Settings(filename) With {.Metadata = AddVersionInfoToMetadata(plugin, .Metadata)}
        Next

    End Sub

    ''' <summary>
    ''' Retrieves the size of a rectangular area into which a control can be fitted.
    ''' </summary>
    ''' <param name="proposedSize">The custom-sized area for a control.</param>
    ''' <returns>
    ''' An ordered pair of type <see cref="T:System.Drawing.Size" /> representing the width and height of a rectangle.
    ''' </returns>
    Public Overrides Function GetPreferredSize(proposedSize As Drawing.Size) As Drawing.Size
        Dim w = Math.Max(proposedSize.Width, 240)
        Dim h = Math.Max(proposedSize.Height, 180)
        Return New System.Drawing.Size(w, h)
    End Function

    ''' <summary>Runs the <see cref="ActivityControl" /> with the specified ID.</summary>
    ''' <param name="activityId">The ID of the <see cref="ActivityControl"/> to run.</param>
    ''' <param name="initializationCommand">
    ''' The command to initialize the <see cref="ActivityControl"/> with.
    ''' </param>
    ''' <returns>
    '''   <c>true</c> if an <see cref="ActivityControl" /> with the specified ID was found; otherwise <c>false</c>.
    ''' </returns>
    Public Function RunActivity(activityId As String, Optional initializationCommand As String = "") As Boolean
        Return RunActivityInternal(activityId, initializationCommand, False)
    End Function

    ''' <summary>Runs the <see cref="ActivityControl"/> with the specified ID and captures the result.</summary>
    ''' <param name="activityId">The ID of the <see cref="ActivityControl"/> to run.</param>
    ''' <param name="initializationCommand">
    ''' The command to initialize the <see cref="ActivityControl"/> with.
    ''' </param>
    ''' <returns>
    '''   <c>true</c> if an <see cref="ActivityControl" /> with the specified ID was found; otherwise <c>false</c>.
    ''' </returns>
    Public Function RunActivityForResult(activityId As String, Optional initializationCommand As String = "") As Boolean
        Return RunActivityInternal(activityId, initializationCommand, True)
    End Function

    Private Function RunActivityInternal(activityId As String, initializationCommand As String, getResult As Boolean) As Boolean
        If FindActivityByID(activityId) IsNot Nothing Then
            Dim locInfo = New LocationInfo() With {.ActivityID = activityId,
                                                   .InitCmd = initializationCommand,
                                                   .GetResult = getResult}

            Me.Navigator.GoTo(New Location() With {.Tag = locInfo})
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>Runs an <see cref="ActivityControl"/> for the specified command.</summary>
    ''' <param name="command">The command for which an <see cref="ActivityControl"/> will be run.</param>
    ''' <returns>
    ''' <c>true</c> if an <see cref="ActivityControl"/> was found for the command; otherwise <c>false</c>.
    ''' </returns>
    Public Function RunCommand(command As String) As Boolean
        Dim activityId = FindActivityByCommand(command)

        If Not activityId.IsNullOrEmpty() Then
            Return RunActivity(activityId, command)

        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Runs an <see cref="ActivityControl"/> for the specified command and captures the result.
    ''' </summary>
    ''' <param name="command">The command for which an <see cref="ActivityControl"/> will be run.</param>
    ''' <returns>
    ''' <c>true</c> if an <see cref="ActivityControl"/> was found for the command; otherwise <c>false</c>.
    ''' </returns>
    Public Function RunCommandForResult(command As String) As Boolean
        Dim activityId = FindActivityByCommand(command)

        If Not activityId.IsNullOrEmpty() Then
            Return RunActivityForResult(activityId, command)

        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Saves the settings of plugins loaded in this <see cref="ActivityHostControl"/>.
    ''' </summary>
    Public Sub SavePluginSettings()
        If Not System.IO.Directory.Exists(settingsDir) Then
            System.IO.Directory.CreateDirectory(settingsDir)
        End If

        For Each plugin In _plugins.Values
            ' save only those plugins whose autoSave has been turned off
            If plugin.Settings.AutoSave = False Then
                plugin.Settings.Metadata = AddVersionInfoToMetadata(plugin, plugin.Settings.Metadata)
                plugin.Settings.Save()
            End If
        Next
    End Sub

    ''' <summary>Scans the specified directories for plugins.</summary>
    ''' <param name="directories">The directories to scan for plugins.</param>
    Public Sub ScanForPlugins(ParamArray directories As String())
        ' scan logic:
        '  1. go through each directory
        '  2. get all files whose names end with .info.txt
        '  3. read each line from the file (typically 1 line only indicating which DLL is the plugin)
        '  4. strip quotation marks and spaces from the line
        '  5. filter non blanks and assign it as the plugin file name
        '  6. set the plugin directory (the same directory as the current .info.txt file)
        '  7. set the plugin file name (full file name)
        '  8. filter for files that exist only
        '  9. filter distinct plugin files
        ' 10. search for and retrieve the plugins from the file
        Dim scanResult = From directory In directories
                         Where System.IO.Directory.Exists(directory)
                         From file In IO.Directory.GetFiles(directory, "*.info.txt", IO.SearchOption.AllDirectories)
                         From line In System.IO.File.ReadAllLines(file)
                         Let pluginName = line.Trim(" '""".ToCharArray())
                         Where Not String.IsNullOrEmpty(pluginName)
                         Let pluginDir = System.IO.Directory.GetParent(file).FullName
                         Let pluginFile = System.IO.Path.Combine(pluginDir, pluginName)
                         Where System.IO.File.Exists(pluginFile)
                         Select pluginFile Distinct
                         From plugin In ExtensibleFramework.Core.Plugin.FromFile(pluginFile)
                         Select plugin Distinct


        ' if there were any plugins found, update our plugin dictionary to those ones found
        If scanResult.Any() Then
            _plugins = scanResult.ToDictionary(Function(k) k.UniqueID)

            ' clear the cached activities
            For Each aID In (From a In activities
                             Where a.Key <> CurrentActivity.ID
                             Select a.Key).ToArray()

                activities(aID).StopActivity(True)
                activities(aID).Dispose()
                activities.Remove(aID)
            Next
        End If

        Me.LoadPluginSettings()
    End Sub

    ''' <summary>Sets the data directory.</summary>
    ''' <param name="dataDir">The data directory.</param>
    Public Sub SetDataDirectory(dataDir As String)
        _dataDir = dataDir
        settingsDir = System.IO.Path.Combine(_dataDir, "PluginPrefs")

        If _plugins.Any() Then Me.LoadPluginSettings()
    End Sub

    ' event handlers

    Private Sub Navigator_IsNavigating(sender As Navigator, e As Navigator.LocationChangingEventArgs)
        If _currentActivity IsNot Nothing Then
            If Not _currentActivity.CanStopActivity() Then
                ' cancel navigation because current activity does not want to be stopped
                e.Cancel = True
            End If
        End If

        If Not e.Cancel AndAlso Me.Navigator.CurrentLocation IsNot Nothing Then
            ' get the current location info or create one if none exists
            Dim locInfo = If(TryCast(Me.Navigator.CurrentLocation.Tag, LocationInfo),
                             New LocationInfo() With {.ActivityID = _currentActivity.ID})

            ' save the result if current activity was run for a result
            _activityResult = If(locInfo.GetResult, _currentActivity.Result, Nothing)

            'save the state of the activity
            'a cleanup is performed when navigation direction is backwards
            Dim cleanup = e.Direction = Core.Navigator.NavigationDirection.Back
            locInfo.State = _currentActivity.StopActivity(cleanup)

            ' save the activity's state
            Me.Navigator.CurrentLocation.Tag = locInfo
        End If
    End Sub

    Private Sub Navigator_HasNavigated(sender As Navigator, e As Navigator.LocationChangedEventArgs)
        If e.NewLocation.Tag Is Nothing Then Return

        Dim locInfo As LocationInfo = Nothing
        Dim newActivity As ActivityControl = Nothing

        ' try to get the location info and activity for the new location.
        ' if either of them returns null, go back
        If TryCast(e.NewLocation.Tag, LocationInfo).AssignToAndReturn(locInfo) IsNot Nothing AndAlso
            FindActivityByID(locInfo.ActivityID).AssignToAndReturn(newActivity) IsNot Nothing Then

            ' initialize the new activity
            newActivity.Initialize(locInfo.InitCmd)

            ' pause all drawing and replace the current activity with the new one
            Me.SuspendLayout()
            Me.Controls.Clear()
            Me.Controls.Add(newActivity)
            newActivity.Dock = Windows.Forms.DockStyle.Fill

            ' resume drawing the control
            Me.ResumeLayout()
            Me.PerformLayout()

            ' start or restart the activity control based on the navigation direction
            If e.Direction = Core.Navigator.NavigationDirection.Back Then
                newActivity.RestartActivity(locInfo.State)
            Else
                newActivity.StartActivity()
            End If
            newActivity.Focus()

            'set the text and of the new location
            e.NewLocation.Text = newActivity.Text
            e.NewLocation.Icon = newActivity.Icon

            _currentActivity = newActivity
        Else
            ' invalid location info or no matching activity found.
            Me.Navigator.GoBack()
        End If
    End Sub

    Private Sub ActivityHostControl_ParentChanged(sender As Object, e As EventArgs) Handles Me.ParentChanged
        If parentForm IsNot Nothing Then
            'remove existing event handlers
            RemoveHandler parentForm.Activated, AddressOf ParentForm_Active
            RemoveHandler parentForm.Deactivate, AddressOf ParentForm_Inactive
            RemoveHandler parentForm.FormClosing, AddressOf ParentForm_FormClosing
        End If

        ' set the parent form
        parentForm = Me.FindForm()

        If Me.Parent IsNot Nothing And parentForm IsNot Nothing Then
            ' add event handlers to control the activities in sync with the parent form's events
            AddHandler parentForm.Activated, AddressOf ParentForm_Active
            AddHandler parentForm.Deactivate, AddressOf ParentForm_Inactive
            AddHandler parentForm.FormClosing, AddressOf ParentForm_FormClosing
        End If
    End Sub

    Private Sub ParentForm_Active(sender As Object, e As EventArgs)
        ' resume activity when parent form gets focus
        If _currentActivity IsNot Nothing AndAlso
           _currentActivity.State <> ActivityState.Running Then
            _currentActivity.ResumeActivity()
        End If
    End Sub

    Private Sub ParentForm_Inactive(sender As Object, e As EventArgs)
        ' pause activity when parent form loses focus
        If _currentActivity IsNot Nothing AndAlso
           _currentActivity.State <> ActivityState.Paused Then
            _currentActivity.PauseActivity()
        End If
    End Sub

    Private Sub ParentForm_FormClosing(sender As Object, e As Windows.Forms.FormClosingEventArgs)
        If _currentActivity IsNot Nothing Then
            ' determine if the current activity can be stopped 
            If _currentActivity.CanStopActivity() Then
                ' stop activity and perform cleanup
                _currentActivity.StopActivity(True)
            Else
                ' set cancel to true to prevent form from closing
                e.Cancel = True
            End If
        End If

        ' form is going to close. save plugin settings
        If e.Cancel = False Then Me.SavePluginSettings()
    End Sub

End Class
