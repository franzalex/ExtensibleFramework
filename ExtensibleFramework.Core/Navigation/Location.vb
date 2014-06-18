Imports System.ComponentModel

Public Class Location

    Dim _navigator As Navigator
    Dim _text As String
    Dim _tag As Object
    Dim _icon As Drawing.Image

    ''' <summary>Creates a new instance of the Location class.</summary>
    <DebuggerStepThrough()> Public Sub New()
        Tag = New Object
        Text = ""
        Icon = New System.Drawing.Bitmap(1, 1)
    End Sub

    ''' <summary>Creates a new instance of the Location class.</summary>
    ''' <param name="text">The Text to be displayed for the Location.</param>
    <DebuggerStepThrough()> Public Sub New(ByVal text As String)
        Tag = New Object
        text = text
        Icon = New System.Drawing.Bitmap(1, 1)
    End Sub

    ''' <summary>Creates a new instance of the Location class.</summary>
    ''' <param name="text">The Text to be displayed for the Location.</param>
    ''' <param name="icon">Icon to represent the Location.</param>
    <DebuggerStepThrough()> Public Sub New(ByVal text As String, ByVal icon As System.Drawing.Image)
        MyBase.New()
        Tag = New Object
        text = text
        icon = icon
    End Sub

    ''' <summary>Creates a new instance of the Location class.</summary>
    ''' <param name="text">The Text to be displayed for the Location.</param>
    ''' <param name="icon">Icon to represent the Location.</param>
    ''' <param name="tag">Additional data associated with the location.</param>
    <DebuggerStepThrough()> Public Sub New(ByVal text As String, ByVal icon As System.Drawing.Image, ByVal tag As Object)
        MyBase.New()
        Me.Tag = tag
        Me.Text = text
        Me.Icon = icon
    End Sub

    ''' <summary>Creates a new instance of the Location class.</summary>
    ''' <param name="text">The Text to be displayed for the Location.</param>
    ''' <param name="tag">Additional data associated with the location.</param>
    <DebuggerStepThrough()> Public Sub New(ByVal text As String, ByVal tag As Object)
        MyBase.New()
        Me.Tag = tag
        Me.Text = text
        Me.Icon = New System.Drawing.Bitmap(1, 1)
    End Sub


    'properties

    ''' <summary>Icon to represent the Location.</summary>
    Public Property Icon As System.Drawing.Image
        Get
            Return _icon
        End Get
        Set(ByVal value As System.Drawing.Image)
            _icon = value
        End Set
    End Property

    ''' <summary>Gets the full path from the root Location to this Location.</summary>
    Public ReadOnly Property FullPath As String
        Get
            If IsNothing(Me._navigator) Then Return Me.Text

            Dim sep = Me.Navigator.Delimiter
            Dim sb As New Text.StringBuilder
            Dim l = Me.Parent

            ' iteratively append each ancestor's text
            For i As Integer = 0 To Me.Level - 1
                sb.Append(_navigator(i).Text & sep)
            Next

            'then append this location's text
            sb.Append(Me.Text)

            Return sb.ToString
        End Get
    End Property

    ''' <summary>
    ''' Returns the depth of this Location in the location hierarchy.
    ''' </summary>
    Public ReadOnly Property Level As Integer
        Get
            If _navigator Is Nothing Then
                Return 0
            Else
                Return _navigator.IndexOf(Me, True)
            End If

        End Get
    End Property

    ''' <summary>Gets a value indicating whether the item is the first in the series.</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public ReadOnly Property IsRoot As Boolean
        Get
            Return IsNothing(Me.Parent)
        End Get
    End Property

    ''' <summary>Navigation which contains this Location.</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public ReadOnly Property Navigator As Navigator
        Get
            Return _navigator
        End Get
    End Property

    ''' <summary>Gets the parent of this location.</summary>
    Public ReadOnly Property Parent As Location
        Get
            Dim idx = Me.Level
            Return If(idx = 0, Nothing, _navigator(idx - 1))
        End Get
    End Property

    ''' <summary>Text to be displayed for the Location.</summary>
    Public Property Text As String
        Get
            Return _text
        End Get
        Set(ByVal value As String)
            _text = value
        End Set
    End Property

    ''' <summary>Additional data associated with the Location.</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Property Tag As Object
        Get
            Return _tag
        End Get
        Set(ByVal value As Object)
            _tag = value
        End Set
    End Property


    'methods


    ''' <summary>Determines whether the specified <see cref="System.Object" /> is equal to this instance.</summary>
    ''' <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
    ''' <returns>
    ''' <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
    ''' </returns>
    ''' <exception cref="T:System.NullReferenceException">The <paramref name="obj" /> parameter is null.</exception>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If GetType(Location).IsAssignableFrom(GetType(Object)) Then
            Dim l As Location = DirectCast(obj, Location)
            Dim val As Boolean

            val = Me.Text = l.Text
            val = val AndAlso Me.Icon.Equals(l.Icon)
            val = val AndAlso Me.Tag.Equals(l.Tag)

            Return val
        Else
            Return False
        End If
    End Function

    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Protected Friend Sub SetNavigator(ByVal navigator As Navigator)
        _navigator = navigator
    End Sub
End Class
