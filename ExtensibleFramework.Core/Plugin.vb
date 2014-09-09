Imports GuidAttribute = System.Runtime.InteropServices.GuidAttribute

''' <summary>Base class for all plug-ins.</summary>
''' <remarks>
''' This is the base class that all plug-ins must inherit from in order to be correctly detected.
''' </remarks>
Public MustInherit Class Plugin
    ' The MustInherit keyword is used to prevent 'accidental' creation of instances of this class.

    Dim _fileName As String
    Dim _settings As Core.Settings

    ''' <summary>Initializes a new instance of the <see cref="Plugin"/> class.</summary>
    Protected Sub New()
        _settings = New Settings()
    End Sub

    ' Plug-in Descriptor properties

    ''' <summary>Gets the description of the plug-in.</summary>
    Public MustOverride ReadOnly Property Description As String

    ''' <summary>Gets the file name of the file this plug-in was created from.</summary>
    Public ReadOnly Property FileName As String
        Get
            Return _fileName
        End Get
    End Property

    ''' <summary>Gets the GUID (Globally Universal ID) of the plug-in.</summary>
    ''' <remarks>
    ''' This property was included so classes inheriting this class can quickly return this value as the plug-in ID.
    ''' </remarks>
    <System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Advanced)>
    Public ReadOnly Property Guid As Guid
        Get
            Dim value = Me.GetType().Assembly.GetCustomAttributes(GetType(GuidAttribute), False)(0)
            Return New Guid(DirectCast(value, GuidAttribute).Value)
        End Get
    End Property

    ''' <summary>Gets the plug-in icon.</summary>
    Public Overridable ReadOnly Property Icon As System.Drawing.Image
        Get
            Return My.Resources.PluginIcon_32
        End Get
    End Property

    ''' <summary>Gets the name of the plug-in.</summary>
    Public MustOverride ReadOnly Property Name As String

    ''' <summary>Gets the unique identifier.</summary>
    ''' <value>The unique identifier.</value>
    Public Overridable ReadOnly Property UniqueID As String
        Get
            ' the unique id of each plug-in should in theory be different to distinguish between two
            ' or more plug-ins with the same name.
            ' it is recommended not to change the Unique ID of plug-ins as the version changes
            Return Guid.ToString()
        End Get
    End Property

    ''' <summary>Gets the plug-in version.</summary>
    Public Overridable ReadOnly Property Version As String
        Get
            ' the return value is a string here so that you can return values like
            ' 1.2 beta, 0.0.1 trial, etc.
            Return Me.GetType().Assembly.GetName().Version.ToString()
        End Get
    End Property

    ' other properties

    ''' <summary>Gets the launchers for launching activities directly for this plug-in.</summary>
    Public MustOverride ReadOnly Property ActivityLaunchers As IEnumerable(Of ActivityLauncher)

    ''' <summary>Gets or sets the plug-in's settings.</summary>
    Public Property Settings As Settings
        Get
            Return _settings
        End Get
        Protected Friend Set(value As Settings)
            _settings = value
        End Set
    End Property

    ' Activity and command invocation functions

    ''' <summary>
    ''' Creates the activity associated with the specified <paramref name="activityID" />.</summary>
    ''' <param name="activityID">The activity ID of the activity to be created.</param>
    ''' <returns>An instance of <seealso cref="ActivityControl"/>.</returns>
    Public MustOverride Function CreateActivity(activityID As String) As ActivityControl

    ''' <summary>Gets a value indicating whether the plug-in supports the specified command.</summary>
    ''' <param name="command">The command to be evaluated.</param>
    ''' <returns><c>true</c> if the plug-in supports the specified command; else <c>false</c>.</returns>
    Public MustOverride Function SupportsCommand(command As String) As Boolean

    ''' <summary>
    ''' Gets the ID of the <see cref="ActivityControl" /> to use for executing the specified command.
    ''' </summary>
    ''' <param name="command">The command to be evaluated.</param>
    ''' <returns>
    ''' The ID of the <see cref="ActivityControl" /> to use for executing <paramref name="command" />.
    ''' </returns>
    ''' <remarks>
    ''' If a non-empty string is returned and <seealso cref="Plugin.SupportsCommand"/> is <c>true</c>,
    ''' the activity whose ID is returned will be initialized with <paramref name="command"/>.
    ''' On the other hand, if an empty string is returned, <paramref name="command"/> will be passed
    ''' to <see cref="Plugin.RunCommand"/>.</remarks>
    Public MustOverride Function GetActivityForCommand(command As String) As String

    ''' <summary>Runs the specified command.</summary>
    ''' <param name="command">The command to be run.</param>
    ''' <returns>The result produced from running the command.</returns>
    Public MustOverride Function RunCommand(command As String) As Object

    ' plug-in methods

    ''' <summary>Creates a <see cref="Plugin"/> from the specified file.</summary>
    ''' <param name="fileName">Name of the file.</param>
    Public Shared Function FromFile(fileName As String) As Plugin
        'exit if the file does not exist.
        If Not IO.File.Exists(fileName) Then Return Nothing

        Dim plugin As Plugin = Nothing
        Try
            Dim assembly As Reflection.Assembly = Reflection.Assembly.LoadFrom(fileName)
            Dim foundTypes = From t In assembly.GetTypes() Where t.IsSubclassOf(GetType(Plugin))

            If foundTypes.Any() Then
                plugin = CType(assembly.CreateInstance(foundTypes.First.FullName), Plugin)
                plugin._fileName = fileName
            End If
        Catch ex As Exception
        End Try

        Return plugin
    End Function

    ''' <summary>Creates a <see cref="Plugin"/> from the specified object.</summary>
    ''' <param name="obj">Object to attempt creating a plug-in from.</param>
    Public Shared Function FromFile(obj As Object) As Plugin
        If TypeOf obj Is String Then
            Return Plugin.FromFile(obj.ToString())
        Else
            Return TryCast(obj, Plugin)
        End If

    End Function

End Class