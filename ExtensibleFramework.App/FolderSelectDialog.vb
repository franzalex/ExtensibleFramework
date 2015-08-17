Imports System.Reflection

'' ------------------------------------------------------------------ 
''
'' Wraps System.Windows.Forms.OpenFileDialog to make it present a vista-style dialog.
''
'' ------------------------------------------------------------------

Namespace Global.System.Windows.Forms
    ''' <summary>
    '''   Wraps System.Windows.Forms.OpenFileDialog to make it present a vista-style dialog.
    ''' </summary>
    Public Class FolderSelectDialog
        Implements IDisposable

        ' Wrapped dialog
        Private ofd As System.Windows.Forms.OpenFileDialog = Nothing

        ''' <summary>Default constructor</summary>
        Public Sub New()
            ofd = New System.Windows.Forms.OpenFileDialog()

            ofd.Filter = "Folders|" & vbLf
            ofd.AddExtension = False
            ofd.CheckFileExists = False
            ofd.DereferenceLinks = True
            ofd.Multiselect = False
        End Sub

#Region "Properties"

        ''' <summary>
        '''   Gets/Sets the initial folder to be selected. A null value selects the current directory.
        ''' </summary>
        Public Property InitialDirectory As String
            Get
                Return ofd.InitialDirectory
            End Get
            Set(value As String)
                ofd.InitialDirectory = If(value Is Nothing OrElse value.Length = 0, Environment.CurrentDirectory, value)
            End Set
        End Property

        ''' <summary>Gets/Sets the title to show in the dialog</summary>
        Public Property Title As String
            Get
                Return ofd.Title
            End Get
            Set(value As String)
                ofd.Title = If(value Is Nothing, "Select a folder", value)
            End Set
        End Property

        ''' <summary>Gets the path selected by the user.</summary>
        Public ReadOnly Property SelectedPath As String
            Get
                Return ofd.FileName
            End Get
        End Property

#End Region

#Region "Methods"

        ''' <summary>Shows the dialog</summary>
        ''' <returns>True if the user presses OK else false</returns>
        Public Function ShowDialog() As DialogResult
            Return ShowDialog(IntPtr.Zero)
        End Function

        ''' <summary>Shows the dialog</summary>
        ''' <param name="hWndOwner">Handle of the control to be parent</param>
        ''' <returns>True if the user presses OK else false</returns>
        Public Function ShowDialog(hWndOwner As IntPtr) As DialogResult
            Dim flag As Boolean = False

            If Environment.OSVersion.Version.Major >= 6 Then
                Dim r = New Reflector("System.Windows.Forms")

                Dim num As UInteger = 0
                Dim typeIFileDialog As Type = r.GetType_("FileDialogNative.IFileDialog")
                Dim dialog As Object = r.CallMethod(ofd, "CreateVistaDialog")
                r.CallMethod(ofd, "OnBeforeVistaDialog", dialog)

                Dim options As UInteger = CUInt(r.CallAs(GetType(System.Windows.Forms.FileDialog), ofd, "GetOptions"))
                options = options Or CUInt(r.GetEnum("FileDialogNative.FOS", "FOS_PICKFOLDERS"))
                r.CallAs(typeIFileDialog, dialog, "SetOptions", options)

                Dim pfde As Object = r.CreateNew("FileDialog.VistaDialogEvents", ofd)
                Dim parameters As Object() = New Object() {pfde, num}
                r.CallAs2(typeIFileDialog, dialog, "Advise", parameters)
                num = CUInt(parameters(1))
                Try
                    Dim num2 As Integer = CInt(r.CallAs(typeIFileDialog, dialog, "Show", hWndOwner))
                    flag = (0 = num2)
                Finally
                    r.CallAs(typeIFileDialog, dialog, "Unadvise", num)
                    GC.KeepAlive(pfde)
                End Try
            Else
                Dim fbd = New FolderBrowserDialog()
                fbd.Description = Me.Title
                fbd.SelectedPath = Me.InitialDirectory
                fbd.ShowNewFolderButton = False
                If fbd.ShowDialog(New WindowWrapper(hWndOwner)) <> DialogResult.OK Then
                    Return False
                End If
                ofd.FileName = fbd.SelectedPath
                flag = True
            End If

            Return If(flag, DialogResult.OK, DialogResult.Cancel)
        End Function

#End Region

#Region "Internal Classes"
        ''' <summary>Creates IWin32Window around an IntPtr</summary>
        Friend Class WindowWrapper
            Implements System.Windows.Forms.IWin32Window
            ''' <summary>Constructor</summary>
            ''' <param name="handle">Handle to wrap</param>
            Public Sub New(handle As IntPtr)
                _hwnd = handle
            End Sub

            ''' <summary>Gets the handle to the window represented by the implementer.</summary>
            Public ReadOnly Property Handle As IntPtr Implements IWin32Window.Handle
                Get
                    Return _hwnd
                End Get
            End Property

            Private _hwnd As IntPtr
        End Class

        ''' <summary>
        '''   This class is from the Front-End for DosBox and is used to present a 'vista' dialog box to
        '''   select folders. Being able to use a vista style dialog box to select folders is much
        '''   better then using the shell folder browser. http://code.google.com/p/fed/
        ''' 
        '''   Example:    Dim r = new Reflector("System.Windows.Forms");
        ''' </summary>
        Friend Class Reflector
#Region "variables"

            Private m_ns As String
            Private m_asmb As Assembly

#End Region

#Region "Constructors"

            ''' <summary>Constructor</summary>
            ''' <param name="ns">The namespace containing types to be used</param>
            Public Sub New(ns As String)
                Me.New(ns, ns)
            End Sub

            ''' <summary>Constructor</summary>
            ''' <param name="an">
            '''   A specific assembly name (used if the assembly name does not tie exactly with the namespace)
            ''' </param>
            ''' <param name="ns">The namespace containing types to be used</param>
            Public Sub New(an As String, ns As String)
                m_ns = ns
                m_asmb = Nothing
                For Each aName As AssemblyName In Assembly.GetExecutingAssembly().GetReferencedAssemblies()
                    If aName.FullName.StartsWith(an) Then
                        m_asmb = Assembly.Load(aName)
                        Exit For
                    End If
                Next
            End Sub

#End Region

#Region "Methods"

            ''' <summary>Return a Type instance for a type 'typeName'</summary>
            ''' <param name="typeName">The name of the type</param>
            ''' <returns>A type instance</returns>
            Public Function GetType_(typeName As String) As Type
                Dim type As Type = Nothing
                Dim names As String() = typeName.Split("."c)

                If names.Length > 0 Then
                    type = m_asmb.GetType((m_ns & Convert.ToString(".")) + names(0))
                End If

                For i As Integer = 1 To names.Length - 1
                    type = type.GetNestedType(names(i), BindingFlags.NonPublic)
                Next
                Return type
            End Function

            ''' <summary>Create a new object of a named type passing along any parameters</summary>
            ''' <param name="name">The name of the type to create</param>
            ''' <param name="parameters"></param>
            ''' <returns>An instantiated type</returns>
            Public Function CreateNew(name As String, ParamArray parameters As Object()) As Object
                Dim type As Type = GetType_(name)

                Dim ctorInfos As ConstructorInfo() = type.GetConstructors()
                For Each ci As ConstructorInfo In ctorInfos
                    Try
                        Return ci.Invoke(parameters)
                    Catch
                    End Try
                Next

                Return Nothing
            End Function

            ''' <summary>
            '''   Calls method <paramref name="func" /> on object <paramref name="obj" /> passing
            '''   parameters <paramref name="parameters" />
            ''' </summary>
            ''' <param name="obj">The object on which to execute function <paramref name="func" /></param>
            ''' <param name="func">The function to execute</param>
            ''' <param name="parameters">The parameters to pass to function <paramref name="func" /></param>
            ''' <returns>The result of the function invocation</returns>
            Public Function CallMethod(obj As Object, func As String, ParamArray parameters As Object()) As Object
                Return Call2(obj, func, parameters)
            End Function

            ''' <summary>
            '''   Calls method <paramref name="func" /> on object <paramref name="obj" /> passing
            '''   parameters <paramref name="parameters" />
            ''' </summary>
            ''' <param name="obj">The object on which to execute function <paramref name="func" /></param>
            ''' <param name="func">The function to execute</param>
            ''' <param name="parameters">The parameters to pass to function <paramref name="func" /></param>
            ''' <returns>The result of the function invocation</returns>
            Public Function Call2(obj As Object, func As String, parameters As Object()) As Object
                Return CallAs2(obj.GetType(), obj, func, parameters)
            End Function

            ''' <summary>
            '''   Calls method <paramref name="func" /> on object <paramref name="obj" /> which is of
            '''   type <paramref name="type" /> passing parameters <paramref name="parameters" />
            ''' </summary>
            ''' <param name="type">The type of <paramref name="obj" /></param>
            ''' <param name="obj">The object on which to execute function <paramref name="func" /></param>
            ''' <param name="func">The function to execute</param>
            ''' <param name="parameters">The parameters to pass to function <paramref name="func" /></param>
            ''' <returns>The result of the function invocation</returns>
            Public Function CallAs(type As Type, obj As Object, func As String, ParamArray parameters As Object()) As Object
                Return CallAs2(type, obj, func, parameters)
            End Function

            ''' <summary>
            '''   Calls method <paramref name="func" /> on object <paramref name="obj" /> which is of
            '''   type <paramref name="type" /> passing parameters <paramref name="parameters" />
            ''' </summary>
            ''' <param name="type">The type of <paramref name="obj" /></param>
            ''' <param name="obj">The object on which to execute function <paramref name="func" /></param>
            ''' <param name="func">The function to execute</param>
            ''' <param name="parameters">The parameters to pass to function <paramref name="func" /></param>
            ''' <returns>The result of the function invocation</returns>
            Public Function CallAs2(type As Type, obj As Object, func As String, parameters As Object()) As Object
                Dim methInfo As MethodInfo = type.GetMethod(func, BindingFlags.Instance Or BindingFlags.[Public] Or BindingFlags.NonPublic)
                Return methInfo.Invoke(obj, parameters)
            End Function

            ''' <summary>Returns the value of property <paramref name="prop" /> of object <paramref name="obj" /></summary>
            ''' <param name="obj">The object containing <paramref name="prop" /></param>
            ''' <param name="prop">The property name</param>
            ''' <returns>The property value</returns>
            Public Function GetProperty(obj As Object, prop As String) As Object
                Return GetAs(obj.GetType(), obj, prop)
            End Function

            ''' <summary>
            '''   Returns the value of property <paramref name="prop" /> of object <paramref name="obj" /> which
            '''   has type <paramref name="type" />
            ''' </summary>
            ''' <param name="type">The type of <paramref name="obj" /></param>
            ''' <param name="obj">The object containing <paramref name="prop" /></param>
            ''' <param name="prop">The property name</param>
            ''' <returns>The property value</returns>
            Public Function GetAs(type As Type, obj As Object, prop As String) As Object
                Dim propInfo As PropertyInfo = type.GetProperty(prop, BindingFlags.Instance Or BindingFlags.[Public] Or BindingFlags.NonPublic)
                Return propInfo.GetValue(obj, Nothing)
            End Function

            ''' <summary>Returns an enum value</summary>
            ''' <param name="typeName">The name of enum type</param>
            ''' <param name="name">The name of the value</param>
            ''' <returns>The enum value</returns>
            Public Function GetEnum(typeName As String, name As String) As Object
                Dim type As Type = GetType_(typeName)
                Dim fieldInfo As FieldInfo = type.GetField(name)
                Return fieldInfo.GetValue(Nothing)
            End Function

#End Region

        End Class
#End Region

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    If ofd IsNot Nothing Then ofd.Dispose()
                End If


            End If
            Me.disposedValue = True
        End Sub

        ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
        'Protected Overrides Sub Finalize()
        '    ' Do not change this code.  Put clean-up code in Dispose(ByVal disposing As Boolean) above.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put clean-up code in Dispose(disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

    End Class
End Namespace