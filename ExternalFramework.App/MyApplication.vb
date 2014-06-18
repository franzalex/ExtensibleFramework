Namespace My

    Partial Friend Class MyApplication
        Private _exeFile As String = Nothing

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
                Return System.IO.Directory.GetParent(Path).FullName
            End Get
        End Property
    End Class
End Namespace