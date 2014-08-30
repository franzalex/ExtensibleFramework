Imports Image = System.Drawing.Image

''' <summary>
''' This class represents items that will be displayed on the 'home screen' that a user can use to 
''' directly launch an activity control.
''' </summary>
Public Class ActivityLauncher

    Dim _caption As String
    Dim _icon As Image
    Dim _description As String
    Dim _activityID As String
    Dim _initCommand As String

    ''' <summary>Initializes a new instance of the <see cref="ActivityLauncher" /> class.</summary>
    ''' <param name="caption">The caption displayed for the <see cref="ActivityLauncher" />.</param>
    ''' <param name="icon">The icon displayed for the <see cref="ActivityLauncher" />.</param>
    ''' <param name="description">Additional descriptive text for the <see cref="ActivityLauncher" />.</param>
    ''' <remarks>The <paramref name="icon" /> should preferably be 32x32 pixels in size.</remarks>
    Public Sub New(caption As String, icon As Image, Optional description As String = "")
        _caption = caption
        _icon = icon
        _description = description
    End Sub

    ''' <summary>Gets the caption of the <see cref="ActivityLauncher"/>.</summary>
    Public ReadOnly Property Caption As String
        Get
            Return _caption
        End Get
    End Property

    ''' <summary>Gets the additional descriptive text of the <see cref="ActivityLauncher"/>.</summary>
    Public ReadOnly Property Description As String
        Get
            Return _description
        End Get
    End Property

    ''' <summary>Gets the icon displayed for this <see cref="ActivityLauncher"/>.</summary>
    Public ReadOnly Property Icon As Image
        Get
            Return _icon
        End Get
    End Property

    ''' <summary>Gets or sets the activity ID.</summary>
    ''' <value>
    ''' The command that will be used to launch the associated <see cref="ActivityControl" />.
    ''' </value>
    ''' <exception cref="System.InvalidOperationException">The ActivityID property cannot be set more than once.</exception>
    ''' <remarks>
    ''' The ActivityID is passed to the <see cref="Plugin.CreateActivity" /> method to create an 
    ''' instance of the associated <seealso cref="ActivityControl" />.
    ''' </remarks>
    Public Property ActivityID As String
        Get
            Return _activityID
        End Get
        Set(value As String)
            If _activityID Is Nothing OrElse String.IsNullOrEmpty(_activityID) Then
                _activityID = value
            Else
                Throw New InvalidOperationException("The ActivityID property cannot be set more than once.")
            End If
        End Set
    End Property

    ''' <summary>Gets or sets the activity initialization command.</summary>
    ''' <value>
    ''' The command that will be used to initialize the associated <see cref="ActivityControl" /> on launch.
    ''' </value>
    ''' <exception cref="System.InvalidOperationException">The InitializationCommand property cannot be set more than once.</exception>
    ''' <remarks>
    ''' The value of this property passed sent to the <see cref="ActivityControl"/> in the 
    ''' <see cref="ActivityControl.Start"/> event via the <see cref="StartEventArgs.InitializationCommand"/>.
    ''' </remarks>
    Public Property InitializationCommand As String
        Get
            Return _initCommand
        End Get
        Set(value As String)
            If _initCommand Is Nothing OrElse String.IsNullOrEmpty(_initCommand) Then
                _initCommand = value
            Else
                Throw New InvalidOperationException("The InitializationCommand property cannot be set more than once.")
            End If
        End Set
    End Property

    ''' <summary>Returns a <see cref="System.String" /> that represents this instance.</summary>
    ''' <returns>A <see cref="System.String" /> that represents this instance.</returns>
    Public Overrides Function ToString() As String
        Return _caption & "[" & ActivityID & "]"
    End Function

    ''' <summary>
    ''' Determines whether the specified <see cref="System.Object" />, is equal to this instance.
    ''' </summary>
    ''' <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
    ''' <returns>
    '''   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
    ''' </returns>
    Public Overrides Function Equals(obj As Object) As Boolean
        If Not TypeOf obj Is ActivityLauncher Then Return False

        ' cast the object passed to the same type as this one and compare the string properties
        ' image properties cannot be reliably compared so it is excluded
        Dim o1 = DirectCast(obj, ActivityLauncher)
        Return _caption = o1._caption AndAlso
               _description = o1._description AndAlso
               _activityID = o1._activityID
    End Function
End Class
