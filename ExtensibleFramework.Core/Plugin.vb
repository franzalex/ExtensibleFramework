Imports GuidAttribute = System.Runtime.InteropServices.GuidAttribute

''' <summary>Base class for all plugins.</summary>
''' <remarks>
''' This is the base class that all plugins must inherit from in order to be correctly detected.
''' </remarks>
Public MustInherit Class Plugin
    ' The MustInherit keyword is used to prevent 'accidental' creation of instances of this class.

    Dim _fileName As String
    Dim _settings As Core.Settings

    ''' <summary>Initializes a new instance of the <see cref="Plugin"/> class.</summary>
    Protected Sub New()
        _settings = New Settings()
    End Sub

    ' Plugin Descriptor properties

    ''' <summary>Gets the description of the plugin.</summary>
    Public MustOverride ReadOnly Property Description As String

    ''' <summary>Gets the file name of the file this plugin was created from.</summary>
    Public ReadOnly Property FileName As String
        Get
            Return _fileName
        End Get
    End Property

    ''' <summary>Gets the GUID (Globally Universal ID) of the plugin.</summary>
    ''' <remarks>
    ''' This property was included so classes inheriting this class can quickly return this value as the plugin ID.
    ''' </remarks>
    <System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Advanced)>
    Public ReadOnly Property Guid As Guid
        Get
            Dim value = Me.GetType().Assembly.GetCustomAttributes(GetType(GuidAttribute), False)(0)
            Return New Guid(DirectCast(value, GuidAttribute).Value)
        End Get
    End Property

    ''' <summary>Gets the plugin icon.</summary>
    Public Overridable ReadOnly Property Icon As System.Drawing.Image
        Get
            Return My.Resources.PluginIcon_32
        End Get
    End Property

    ''' <summary>Gets the name of the plugin.</summary>
    Public MustOverride ReadOnly Property Name As String

    ''' <summary>Gets the unique identifier.</summary>
    ''' <value>The unique identifier.</value>
    Public Overridable ReadOnly Property UniqueID As String
        Get
            ' the unique id of each plugin should in theory be different to distinguish between two
            ' or more plugins with the same name.
            ' it is recommended not to change the Unique ID of plugins as the version changes
            Return Guid.ToString()
        End Get
    End Property

    ''' <summary>Gets the plugin version.</summary>
    Public Overridable ReadOnly Property Version As String
        Get
            ' the return value is a string here so that you can return values like
            ' 1.2 beta, 0.0.1 trial, etc.
            Return Me.GetType().Assembly.GetName().Version.ToString()
        End Get
    End Property

    ' other properties

    ''' <summary>Gets the launchers for launching activities directly for this plugin.</summary>
    Public MustOverride ReadOnly Property ActivityLaunchers As IEnumerable(Of ActivityLauncher)

    ''' <summary>Gets or sets the plugin's settings.</summary>
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

    ''' <summary>Gets a value indicating whether the plugin supports the specified command.</summary>
    ''' <param name="command">The command to be evaluated.</param>
    ''' <returns><c>true</c> if the plugin supports the specified command; else <c>false</c>.</returns>
    ''' <remarks>
    ''' If this method returns <c>true</c>, the <see cref="Plugin.GetActivityForCommand" /> method
    ''' will be called to determine which activity should be used to run the command.
    ''' </remarks>
    Public MustOverride Function SupportsCommand(command As String) As Boolean

    ''' <summary>
    ''' Gets the ID of the <see cref="ActivityControl" /> to use for executing the specified command.
    ''' </summary>
    ''' <param name="command">The command to be evaluated.</param>
    ''' <returns>
    ''' The ID of the <see cref="ActivityControl" /> to use for executing <paramref name="command" />.
    ''' </returns>
    ''' <remarks>
    ''' The <paramref name="command"/> parameter will be will be passed to the 
    ''' <see cref="Plugin.RunCommand"/> method if a <c>null</c> or empty string is returned.
    ''' </remarks>
    Public MustOverride Function GetActivityForCommand(command As String) As String

    ''' <summary>Runs the specified command.</summary>
    ''' <param name="command">The command to be run.</param>
    ''' <returns>The result produced from running the command.</returns>
    Public MustOverride Function RunCommand(command As String) As Object

    ' plugin methods

    ''' <summary>Creates a <see cref="Plugin"/> from the specified file.</summary>
    ''' <param name="fileName">Name of the file.</param>
    Public Shared Function FromFile(fileName As String) As IEnumerable(Of Plugin)
        'exit if the file does not exist.
        If Not IO.File.Exists(fileName) Then Return Nothing

        Dim plugins As New List(Of Plugin)
        Try
            Dim assembly As Reflection.Assembly = Reflection.Assembly.LoadFrom(fileName)
            Dim foundTypes = From t In assembly.GetTypes() Where t.IsSubclassOf(GetType(Plugin))

            For Each foundType In foundTypes
                Dim plugin = DirectCast(assembly.CreateInstance(foundType.FullName), Plugin)
                plugin._fileName = fileName
                plugins.Add(plugin)
            Next
        Catch ex As Exception
        End Try

        Return plugins
    End Function

    ''' <summary>Creates a <see cref="Plugin"/> from the specified object.</summary>
    ''' <param name="obj">Object to attempt creating a plugin from.</param>
    Public Shared Function FromFile(obj As Object) As Plugin
        If TypeOf obj Is String Then
            Return Plugin.FromFile(obj.ToString())
        Else
            Return TryCast(obj, Plugin)
        End If

    End Function

    ' overridden methods

    ''' <summary>Determines whether the specified <see cref="System.Object" />, is equal to this instance.</summary>
    ''' <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
    ''' <returns>
    '''   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
    ''' </returns>
    Public Overrides Function Equals(obj As Object) As Boolean
        ' ensure both objects aren't null and the object being compared is a Plugin
        Return (Me IsNot Nothing) AndAlso (obj IsNot Nothing) AndAlso
               (TypeOf obj Is Plugin) AndAlso
               (DirectCast(obj, Plugin).UniqueID.Equals(Me.UniqueID))
    End Function

    ''' <summary>Returns a hash code for this instance.</summary>
    ''' <returns>
    '''   A hash code for this instance, suitable for use in hashing algorithms and data structures like a
    '''   hash table.
    ''' </returns>
    Public Overrides Function GetHashCode() As Integer
        Return Me.UniqueID.GetHashCode()
    End Function

End Class