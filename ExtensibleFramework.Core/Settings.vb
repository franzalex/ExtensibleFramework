Imports Newtonsoft.Json
Imports System.Text.RegularExpressions
Imports ThreadPool = System.Threading.ThreadPool

''' <summary>
''' Dictionary based class for storing settings.
''' </summary>
''' <remarks></remarks>
<DebuggerStepThrough(), DebuggerDisplay("")> _
Public Class Settings
    Inherits Dictionary(Of String, Object)

    Private Const fileDescriptor As String = "Extensible Application Framework Settings File, Format Version "
    Private Const metadataTag As String = "| Metadata |"

    Dim _autoSave As Boolean = True
    Dim _fileName As String = ""

    ''' <summary>
    ''' Creates a new instance of the Settings class.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        'set the base class to ignore case
        MyBase.New(StringComparer.CurrentCultureIgnoreCase)
    End Sub

    ''' <summary>
    ''' Creates a new instance of the Settings class.
    ''' </summary>
    ''' <param name="fileName">String. File to populate the Settings from.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal fileName As String)
        'set the base class to ignore case
        MyBase.New(StringComparer.CurrentCultureIgnoreCase)
        'set the filename and then try to open the file for settings.
        'if opening the file fails, create a new one.
        Me._fileName = fileName
        If FileExists(fileName) Then
            Me.Open(fileName)
            '	If Not Me.Open(fileName) Then
            '		Me.SaveAs(fileName)
            '	End If
            'Else
            '	Me.SaveAs(fileName)
        End If
    End Sub

    ''' <summary>
    ''' Creates a new instance of the Settings class.
    ''' </summary>
    ''' <param name="fileName">String. File to populate the Settings from.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal fileName As String, ByVal comparer As IEqualityComparer(Of String))
        'initialize the base dictionary with the equality comparer
        MyBase.New(comparer)

        'set the filename and then try to open the file for settings.
        'if opening the file fails, create a new one.
        Me._fileName = fileName
        If FileExists(fileName) Then
            Me.Open(fileName)
            '	If Not Me.Open(fileName) Then
            '		Me.SaveAs(fileName)
            '	End If
            'Else
            '	Me.SaveAs(fileName)
        End If
    End Sub

    ''' <summary>Initializes a new instance of the <see cref="Settings"/> class.</summary>
    ''' <param name="collection">The settings collection o use to initialize this instance.</param>
    Protected Friend Sub New(collection As IDictionary(Of String, Object))
        ' Initialize our settings with the specified collection and set the 
        ' key comparer to ignore case
        MyBase.New(collection, StringComparer.CurrentCultureIgnoreCase)
    End Sub

    ''' <summary>Initializes a new instance of the <see cref="Settings"/> class.</summary>
    ''' <param name="settings">The settings collection o use to initialize this instance.</param>
    Protected Friend Sub New(ParamArray settings() As KeyValuePair(Of String, Object))
        Me.New(settings.ToDictionary(Function(s) s.Key, Function(s) s.Value))
    End Sub

    'public properties

    ''' <summary>
    ''' Get or set a value indicating whether or not the Settings is saved automatically.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AutoSave As Boolean
        Get
            Return _autoSave
        End Get
        Set(ByVal value As Boolean)
            _autoSave = value
        End Set
    End Property

    ''' <summary>
    ''' Get or set the filename of the file to which 
    ''' this Settings is saved or opened from.
    ''' </summary>
    ''' <value>String. File name of the file to save to or open from.</value>
    Public Property FileName As String
        Get
            Return _fileName
        End Get
        Protected Friend Set(ByVal value As String)
            _fileName = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets a setting in the Object.
    ''' </summary>
    ''' <param name="key">Key of the setting to be set or retrieved.</param>
    ''' <value>Setting to be added.</value>
    Default Public Shadows Property Item(ByVal key As String) As Object
        Get
            If MyBase.ContainsKey(key) Then
                Return MyBase.Item(key)
            Else
                Return New Object
            End If
        End Get
        Set(ByVal value As Object)
            If Not MyBase.ContainsKey(key) Then
                MyBase.Add(key, value)
            Else
                MyBase.Item(key) = value
            End If
        End Set
    End Property

    ''' <summary>Gets or sets metadata written to the settings file on save.</summary>
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never)> _
    Public Property Metadata As String = ""

    'public methods

    ''' <summary>Clears the value of the setting with the specified name.</summary>
    ''' <param name="name">The name of the setting whose value is to be cleared.</param>
    Public Sub ClearValue(name As String)
        If MyBase.ContainsKey(name) Then
            MyBase.Remove(name)
            If Me.AutoSave Then Me.Save()
        End If
    End Sub

    ''' <summary>
    ''' Gets the value of the setting with the name specified.
    ''' </summary>
    ''' <typeparam name="T">System.Type of the return value.</typeparam>
    ''' <param name="name">Name of the setting to be retrieved.</param>
    ''' <param name="defaultValue">Default value to be returned if the setting does not exist.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetValue(Of T)(ByVal name As String, Optional ByVal defaultValue As T = Nothing) As T
        If Me.ContainsKey(name) Then
            Try
                Return CType(Me.Item(name), T)
            Catch ex As Exception
                Return defaultValue
            End Try
        Else
            Return defaultValue
        End If
    End Function

    ''' <summary>
    ''' Gets the value of the setting with the name specified.
    ''' </summary>
    ''' <param name="name">Name of the setting to be retrieved.</param>
    ''' <param name="defaultValue">Default value to be returned if the setting does not exist.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetValue(ByVal name As String, Optional ByVal defaultValue As Object = Nothing) As Object
        If Me.ContainsKey(name) Then
            Return Me.Item(name)
        Else
            Return defaultValue
        End If
    End Function

    ''' <summary>
    ''' Opens this instance of Settings from the file 
    ''' specified in the Settings.FileName property.
    ''' </summary>
    ''' <returns>Boolean. True if successful; false if otherwise.</returns>
    ''' <remarks></remarks>
    Public Function Open() As Boolean
        If FileName.Trim <> "" Then
            Return Open(_fileName)
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Opens the Settings from the specified file.
    ''' </summary>
    ''' <param name="fileName">String. File to retrieve the Settings from.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Open(ByVal fileName As String) As Boolean
        Try
            Using fileStream As New IO.FileStream(fileName, IO.FileMode.Open)
                Using sr As New IO.StreamReader(fileStream)
                    ' ensure we're deserializing a compatible settings file

                    Dim fileText = sr.ReadToEnd()
                    Dim pattern = "(?<head>/\*)(?<body>(?>(?:(?>[^*]+)|\*(?!/))*))(?<tail>\*/)"
                    Dim descriptor = Regex.Match(fileText, pattern, RegexOptions.Compiled)

                    ' proceed with deserialization only if we've got a good file
                    If IsValidFile(descriptor.Value) Then

                        Dim jset = New JsonSerializerSettings() With {
                                    .DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
                                    .NullValueHandling = NullValueHandling.Ignore}

                        ' copy entries from the deserialized dictionary into ours
                        Me.Clear()
                        For Each kvp In JsonConvert.DeserializeObject(Of Dictionary(Of String, Object)) _
                                                                     (fileText, jset)
                            MyBase.Add(kvp.Key, kvp.Value)
                        Next

                        ' load the saved metadata
                        Me.ReadMetadata(descriptor.Value)

                        'all operations successful
                        Return True
                    Else
                        Return False ' The settings file is not in the correct format.
                    End If

                End Using
            End Using
        Catch ex As Exception
            'an exception occurred
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Adds a setting to the settings.
    ''' </summary>
    ''' <param name="name">Name of the setting. 
    ''' If a setting with the same name exists, it will be overwritten.</param>
    ''' <param name="value">Value of the setting.</param>
    ''' <remarks></remarks>
    Public Sub SetValue(ByVal name As String, ByVal value As Object)
        Me.SetValues({name.CreateKVP(value)})
    End Sub

    ''' <summary>Adds multiple items to the settings.</summary>
    ''' <param name="items">The items to be added to the settings.</param>
    Public Sub SetValues(ParamArray items As KeyValuePair(Of String, Object)())
        For Each kvp In items
            Me.Item(kvp.Key) = kvp.Value
        Next
        If Me.AutoSave Then Me.Save()
    End Sub

    ''' <summary>
    ''' Saves this instance of Settings to the file
    ''' specified in the Settings.FileName property.
    ''' </summary>
    Public Sub Save()
        If _fileName.Trim <> "" Then Me.SaveAs(_fileName)
    End Sub

    ''' <summary>Saves the Settings to the specified file.</summary>
    ''' <param name="fileName">String. File name of the file to save the <see cref="Settings"/> to.</param>
    ''' <param name="saveEmptyFile">
    ''' If set to <c>true</c> the settings will be saved whether or not there are values in the collection.
    ''' </param>
    Public Sub SaveAs(ByVal fileName As String, Optional saveEmptyFile As Boolean = False)
        Try
            ' save if user wants to save an empty file or if data exists in the dictionary
            If saveEmptyFile OrElse Me.Keys.Any() Then
                ' do save on a worker thread. prevents application from freezing during saves
                ThreadPool.QueueUserWorkItem(Sub(o As Object)
                                                 With System.Threading.Thread.CurrentThread
                                                     Dim bgState = .IsBackground
                                                     .IsBackground = False
                                                     Me.SaveInternal(fileName)
                                                     .IsBackground = bgState
                                                 End With
                                             End Sub)
            End If

        Catch ex As Exception
            'save was not successful.
        End Try
    End Sub

    'private methods

    Private Function FileExists(ByVal fileName As String) As Boolean
        Return FileIO.FileSystem.FileExists(fileName)
    End Function

    Private Function IsValidFile(ByRef descriptor As String) As Boolean

        Dim result = Not descriptor.IsNullOrEmpty() AndAlso descriptor.Contains(fileDescriptor)

        If result Then
            Dim verLine = (From l In descriptor.SplitLines()
                           Let line = l.TrimStart(" /*".ToCharArray())
                           Where line.StartsWith(fileDescriptor)
                           Select line).FirstOrDefault()

            ' extract the version information
            If Not verLine.IsNullOrEmpty() Then
                Dim fileVersion = New Version(verLine.Remove(0, fileDescriptor.Length).Trim())
                result = result AndAlso fileVersion.Major = 1 ' we can only handle v1.x
            End If
        End If

        Return result
    End Function

    Private Sub SaveInternal(fileName As String)
        SyncLock Me
            Using sw As New System.IO.StreamWriter(fileName)

                Dim fileInfo = My.Application.Info
                Dim descriptor = New List(Of String) From {
                    fileDescriptor & "1.0",
                    "{0} v{1}".FormatWith(fileInfo.ProductName, fileInfo.Version),
                    ""
                }

                If Not Me.Metadata.IsNullOrEmpty() Then
                    ' maximum metadata line length
                    Dim maxLen = Me.Metadata.SplitLines().Max(Function(l) l.Length)
                    maxLen = Math.Max(maxLen - metadataTag.Length, 12)

                    descriptor.Add(metadataTag & New String(">"c, maxLen))
                    descriptor.AddRange(Me.Metadata.SplitLines(maxEmptyLines:=2))
                    descriptor.Add(New String("<"c, maxLen) & metadataTag)
                End If


                ' add descriptor in an escaped comment (/* ... */)
                sw.WriteLine("/*")
                For Each line In descriptor
                    sw.WriteLine(" * " & line)
                Next
                sw.WriteLine(" */")


                ' add serialized dictionary
                Dim jset = New JsonSerializerSettings() With {
                            .DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
                            .Formatting = Formatting.Indented,
                            .NullValueHandling = NullValueHandling.Ignore
                        }

                Dim fileText = JsonConvert.SerializeObject(Me, jset)

                sw.Write(fileText)

                Return
            End Using
        End SyncLock
    End Sub

    ''' <summary>Reads the metadata.</summary>
    ''' <param name="descriptor">The file descriptor from which the metadata will be read.</param>
    Private Sub ReadMetadata(descriptor As String)
        Dim lines = descriptor.SplitLines(removeEmptyLines:=False).
                               Select(Function(l) l.TrimStart(" */".ToCharArray())).
                               SkipWhile(Function(l) Not l.StartsWith(metadataTag)).Skip(1).
                               TakeWhile(Function(l) Not l.EndsWith(metadataTag))

        Me.Metadata = String.Join(vbCrLf, lines.ToArray())
    End Sub

End Class