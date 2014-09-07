Namespace My

    Partial Friend Class MyApplication
        Private _exeFile As String = Nothing
        Dim _appDataDir As String

        ''' <summary>Gets the application data directory.</summary>
        ''' <value>The application data directory.</value>
        Public ReadOnly Property AppDataDir As String
            Get
                Return _appDataDir
            End Get
        End Property

        ''' <summary>Gets the application path.</summary>
        ''' <value>The application path.</value>
        Public ReadOnly Property Path As String
            Get
                If _exeFile Is Nothing Then
                    _exeFile = System.Reflection.Assembly.GetExecutingAssembly().Location
                End If

                Return _exeFile
            End Get
        End Property

        ''' <summary>Gets the application directory.</summary>
        ''' <value>The application directory.</value>
        Public ReadOnly Property Directory As String
            Get
                Return New System.IO.FileInfo(_exeFile).Directory.FullName
            End Get
        End Property

        Private Sub MyApplication_Startup(sender As Object, e As ApplicationServices.StartupEventArgs) Handles Me.Startup
            ' set the AppData path
            _appDataDir = GetAppDataPath()
        End Sub

        ''' <summary>Gets the application data path.</summary>
        ''' <param name="autoCreate">
        ''' If set to <c>true</c>, the application's data path will be created automatically if it 
        ''' does not exist.
        ''' </param>
        ''' <returns>
        ''' A directory path with read and write access for use as the application's data path.
        ''' </returns>
        Private Function GetAppDataPath(Optional autoCreate As Boolean = True)
            Dim appDir = New System.IO.FileInfo(_exeFile).Directory
            Dim appDrive = New System.IO.DriveInfo(appDir.Root.FullName)
            Dim localAppData = IO.Path.Combine(
                               IO.Path.Combine(System.Environment.GetEnvironmentVariable("appdata"),
                                               Me.Info.CompanyName),
                                               Me.Info.ProductName)
            Dim programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)
            Dim windir = Environment.GetEnvironmentVariable("windir")

            Dim appDataPath = ""
            Try
                ' try to create a temporary file in the application's path to confirm read/write access
                Dim tmp = IO.Path.Combine(appDir.FullName, System.IO.Path.GetRandomFileName())
                Dim fs = System.IO.File.Open(tmp, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)
                fs.Close()
                System.IO.File.Delete(tmp)

                ' read/write access confirmed. Ensure we're not in WinDir or ProgramFiles
                If (appDir.FullName.Contains(programFiles) OrElse appDir.FullName.Contains(windir)) Then
                    appDataPath = localAppData
                Else
                    appDataPath = appDir.FullName
                End If

            Catch ex As Exception
                ' we don't have read/write access to the application directory
                appDataPath = localAppData
            End Try

            If autoCreate AndAlso Not System.IO.Directory.Exists(appDataPath) Then
                System.IO.Directory.CreateDirectory(appDataPath)
            End If

            Return appDataPath
        End Function
    End Class
End Namespace