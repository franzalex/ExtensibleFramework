Option Strict On

Imports System.ComponentModel
Imports System.Linq

''' <summary>Class used to represent a series of navigator.</summary>
<DebuggerStepThrough()>
Public Class Navigator
    Inherits CollectionBase

#Region "Internal Classes"
    Public Enum NavigationDirection
        Undefined
        Forward
        Back
    End Enum
    Public Class LocationChangedEventArgs
        Inherits EventArgs

        Dim _steps As Integer
        Private _newLocation As Location
        Dim _oldLocation As Location

        ''' <summary>Creates a new instance of the LocationChangedEventArgs class.</summary>
        ''' <param name="destination">New location the navigator has moved to.</param>
        ''' <param name="steps">Number of steps the <paramref name="destination"/> is from the current location.</param>
        Public Sub New(source As Location, ByVal destination As Location, ByVal steps As Integer)
            _newLocation = destination
            _oldLocation = source
            _steps = steps
        End Sub

        ''' <summary>Gets the new Location the navigator has moved to.</summary>
        Public ReadOnly Property NewLocation As Location
            Get
                Return _newLocation
            End Get
        End Property

        ''' <summary>Gets the old Location the navigator moved from.</summary>
        Public ReadOnly Property OldLocation As Location
            Get
                Return _oldLocation
            End Get
        End Property

        ''' <summary>Gets the steps the <see cref="NewLocation"/> is from the previous location.</summary>
        Public ReadOnly Property Steps As Integer
            Get
                Return _steps
            End Get
        End Property

        ''' <summary>Gets the general direction the <see cref="NewLocation"/> is from the previous location.</summary>
        Public ReadOnly Property Direction As NavigationDirection
            Get
                Select Case Math.Sign(_steps)
                    Case Is < 0
                        Return NavigationDirection.Back
                    Case Is > 0
                        Return NavigationDirection.Forward
                    Case Else
                        Return NavigationDirection.Undefined
                End Select
            End Get
        End Property
    End Class
    Public Class LocationChangingEventArgs
        Inherits CancelEventArgs

        Dim _steps As Integer
        Private _newLocation As Location

        ''' <summary>Creates a new instance of the LocationChangingEventArgs class.</summary>
        ''' <param name="destination">New location the navigator is moving to.</param>
        ''' <param name="steps">Number of steps the <paramref name="destination"/> is from the current location.</param>
        Public Sub New(ByVal destination As Location, ByVal steps As Integer)
            _newLocation = destination
        End Sub

        ''' <summary>Gets the new Location the navigator is moving to.</summary>
        Public ReadOnly Property NewLocation As Location
            Get
                Return _newLocation
            End Get
        End Property

        ''' <summary>Gets the steps the <see cref="NewLocation"/> is from the previous location.</summary>
        Public ReadOnly Property Steps As Integer
            Get
                Return _steps
            End Get
        End Property

        ''' <summary>Gets the general direction the <see cref="NewLocation"/> is from the previous location.</summary>
        Public ReadOnly Property Direction As NavigationDirection
            Get
                Select Case Math.Sign(_steps)
                    Case Is < 0
                        Return NavigationDirection.Back
                    Case Is > 0
                        Return NavigationDirection.Forward
                    Case Else
                        Return NavigationDirection.Undefined
                End Select
            End Get
        End Property
    End Class
#End Region

    ''' <summary>Index of the current location.</summary>
    Dim curLocIndex As Integer
    Dim _delimiter As Char

    ''' <summary>Occurs when the Navigator navigates to a new location.</summary>
    Public Event HasNavigated As GenericEventHandler(Of Navigator, LocationChangedEventArgs)
    ''' <summary>Occurs before the Navigator moves to a new location.</summary>
    Public Event IsNavigating As GenericEventHandler(Of Navigator, LocationChangingEventArgs)

    ''' <summary>Creates a new instance of the Navigator class.</summary>
    Public Sub New()
        _delimiter = "\"c
        curLocIndex = -1
    End Sub


    'properties


    ''' <summary>Determines if we can move backward in the navigator.</summary>
    Public ReadOnly Property CanGoBack As Boolean
        Get
            Return curLocIndex > 0
        End Get
    End Property

    ''' <summary>Determines if we can move forward in the navigator.</summary>
    Protected Friend ReadOnly Property CanGoForward As Boolean
        Get
            Return curLocIndex < List.Count - 1
        End Get
    End Property

    ''' <summary>Gets the current location in the navigator list.</summary>
    Public ReadOnly Property CurrentLocation As Location
        Get
            If CurrentLocationIndex < 0 OrElse CurrentLocationIndex > List.Count - 1 Then
                Return Nothing
            Else
                Return Me.Item(curLocIndex)
            End If
        End Get
    End Property

    ''' <summary>Gets the index of the current Location.</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public ReadOnly Property CurrentLocationIndex As Integer
        Get
            Return curLocIndex
        End Get
    End Property

    ''' <summary>Get/set the character to delimit the location for the full path.</summary>
    Protected Friend Property Delimiter As Char
        Get
            Return _delimiter
        End Get
        Set(ByVal value As Char)
            _delimiter = value
        End Set
    End Property

    '''<summary>Gets the Locations that precede the current in the Navigator list.</summary>
    Public ReadOnly Property BackItems As Location()
        Get
            Dim arr = Me.InnerList.ToArray
            'convert the array [of object] to Location and then
            '  take the elements that precede the current item.
            Return arr.Select(Function(x) DirectCast(x, Location)).Take(curLocIndex).Reverse.ToArray
        End Get
    End Property

    '''<summary>Gets the Locations that follow the current in the Navigator list.</summary>
    Public ReadOnly Property ForwardItems As Location()
        Get
            Dim arr = Me.InnerList.ToArray
            'convert the array [of object] to Location and then
            '  skip the elements up to the current item and return the rest.
            Return arr.Select(Function(x) DirectCast(x, Location)).Skip(curLocIndex + 1).ToArray
        End Get
    End Property

    ''' <summary>Gets the Location at the specified index.</summary>
    ''' <param name="index">Index of the location to be returned.</param>
    Default Public Property Item(ByVal index As Integer) As Location
        Get
            If index < 0 OrElse index > List.Count - 1 Then
                Throw New ArgumentOutOfRangeException("Index must be from 0 to " & List.Count - 1 & ".")
            Else
                Return CType(List(index), Location)
            End If
        End Get
        Set(ByVal value As Location)
            If index < 0 OrElse index > List.Count - 1 Then
                Throw New ArgumentOutOfRangeException("Index must be from 0 to " & List.Count - 1 & ".")
            Else
                List(index) = value
            End If
        End Set
    End Property

    ''' <summary>Gets the Location that corresponds to the specified path.</summary>
    ''' <param name="path">Path of the location to be returned.</param>
    Default Public Property Item(ByVal path As String) As Location
        Get
            Return GetLocationByPath(path)
        End Get
        Set(ByVal value As Location)
            Dim l = GetLocationByPath(path)

            If l IsNot Nothing Then
                l = value
            End If
        End Set
    End Property

    ''' <summary>Returns the number of Locations in the Navigator.</summary>
    Public ReadOnly Property LocationCount As Integer
        Get
            Return List.Count
        End Get
    End Property

    ''' <summary>Gets the next Location in the Navigator list.</summary>
    Protected Friend ReadOnly Property NextLocation As Location
        Get
            If CanGoForward Then
                Return Item(curLocIndex + 1)
            Else
                Return Nothing
            End If
        End Get
    End Property

    ''' <summary>Gets the previous Location in the Navigator list.</summary>
    Public ReadOnly Property PreviousLocation As Location
        Get
            If CanGoBack Then
                Return Item(curLocIndex - 1)
            Else
                Return Nothing
            End If
        End Get
    End Property


    'methods


    ''' <summary>
    ''' Determines whether the Navigator contains the specified location.
    ''' </summary>
    ''' <param name="location">Location to be looked up.</param>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Function Contains(ByVal location As Location) As Boolean
        Return List.Contains(location)
    End Function

    ''' <summary>
    ''' Determines if there is a Location with the specified path is in the current Navigator.
    ''' </summary>
    ''' <param name="path">Path to be evaluated.</param>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Function Contains(ByVal path As String) As Boolean
        Return Me.IndexOf(path) <> -1
    End Function

    Protected Function GetLocationByPath(ByVal path As String) As Location
        Dim l As Location = Nothing

        For i = 0 To List.Count - 1
            If (DirectCast(l, Location).FullPath = path) Then
                l = DirectCast(l, Location)
                Exit For
            End If
        Next

        Return l
    End Function

    ''' <summary>Goes back one step.</summary>
    ''' <returns>
    ''' The location one steps behind of the current location if navigation was successful; 
    ''' otherwise, the current location is returned
    ''' </returns>
    Public Function GoBack() As Location
        If CanGoBack Then
            Return Me.GoBack(1)
        Else
            Return Item(curLocIndex)
        End If
    End Function

    ''' <summary>Goes back by the specified number of steps.</summary>
    ''' <param name="steps">Number of steps to go backwards.</param>
    ''' <returns>
    ''' The location at the specified number of steps behind of the current location if navigation was successful; 
    ''' otherwise, the current location is returned
    ''' </returns>
    Public Function GoBack(ByVal steps As Integer) As Location
        If Me.List.Count = 0 Then
            'don't navigate an empty list
            Throw New Exception("Navigation list is empty!")

        Else
            'go backward by the specified number of steps, ensuring we don't exceed the min # of items
            Dim newLocIndex = curLocIndex - Math.Abs(steps)
            If newLocIndex < 0 Then newLocIndex = 0

            'allow the user to cancel the navigation by raising the IsNavigating event
            Dim navArgs As New LocationChangingEventArgs(Item(newLocIndex), -Math.Abs(steps))
            RaiseEvent IsNavigating(Me, navArgs)

            If navArgs.Cancel = True Then
                'navigation was canceled
                Return CurrentLocation
            Else
                'navigation was not canceled

                'raise the navigation event
                Dim curLoc = If(curLocIndex.IsInRange(0, List.Count - 1), Item(curLocIndex), Nothing)
                Dim newLoc = If(newLocIndex.IsInRange(0, List.Count - 1), Item(newLocIndex), Nothing)
                Dim e = New LocationChangedEventArgs(curLoc, newLoc, -Math.Abs(steps))

                curLocIndex = newLocIndex
                RaiseEvent HasNavigated(Me, e)

            End If

            Return Item(curLocIndex)
        End If
    End Function

    ''' <summary>Goes forward one step.</summary>
    ''' <returns>
    ''' The location one steps ahead of the current location if navigation was successful; 
    ''' otherwise, the current location is returned
    ''' </returns>
    Protected Friend Function GoForward() As Location
        If CanGoForward Then
            Return Me.GoForward(1)
        Else
            Return Item(curLocIndex)
        End If
    End Function

    ''' <summary>Goes forward by the specified number of steps.</summary>
    ''' <param name="steps">Number of steps to go forward.</param>
    ''' <returns>
    ''' The location at the specified number of steps ahead of the current location if navigation was successful; 
    ''' otherwise, the current location is returned
    ''' </returns>
    Protected Friend Function GoForward(ByVal steps As Integer) As Location
        If Me.List.Count = 0 Then
            'don't navigate an empty list
            Throw New Exception("Navigation list is empty!")

        Else
            'go forward by the specified number of steps, ensuring we don't exceed the max # of items
            Dim newLocIndex = curLocIndex + Math.Abs(steps)
            If newLocIndex > List.Count - 1 Then newLocIndex = List.Count - 1

            'allow the user to cancel the navigation by raising the IsNavigating event
            Dim navArgs As New LocationChangingEventArgs(Item(newLocIndex), Math.Abs(steps))
            RaiseEvent IsNavigating(Me, navArgs)

            If navArgs.Cancel = True Then
                'navigation was canceled
                Return CurrentLocation
            Else
                'navigation was not canceled

                ' raise the navigation event
                Dim curLoc = If(curLocIndex.IsInRange(0, List.Count - 1), Item(curLocIndex), Nothing)
                Dim newLoc = If(newLocIndex.IsInRange(0, List.Count - 1), Item(newLocIndex), Nothing)
                Dim e = New LocationChangedEventArgs(curLoc, newLoc, Math.Abs(steps))

                curLocIndex = newLocIndex
                RaiseEvent HasNavigated(Me, e)

            End If

            Return Item(curLocIndex)
        End If
    End Function

    ''' <summary>Goes to the specified location.</summary>
    ''' <param name="destination">Location to go to.</param>
    ''' <returns>
    ''' The <paramref name="destination"/> if navigation was successful; otherwise, the current location is returned
    ''' </returns>
    Protected Friend Function [GoTo](ByVal destination As Location) As Location
        'check if the specified location is the next in the list
        If CanGoForward AndAlso destination.Equals(NextLocation) Then
            'if the location is the next in the list, go forward
            Return GoForward()
        Else
            Dim cancelArgs = New LocationChangingEventArgs(destination, 1)
            RaiseEvent IsNavigating(Me, cancelArgs)

            If cancelArgs.Cancel = True Then
                Return CurrentLocation
            Else
                'otherwise clear the locations after the current and append the specified location.

                'clear each item after the current location
                Do While List.Count - 1 > curLocIndex
                    List.RemoveAt(curLocIndex + 1)
                Loop

                'add the new location to the end of the list and go to it
                Dim newLocIndex = List.Add(destination)
                Dim curLoc = If(curLocIndex.IsInRange(0, List.Count - 1), Item(curLocIndex), Nothing)
                Dim newLoc = If(newLocIndex.IsInRange(0, List.Count - 1), Item(newLocIndex), Nothing)

                'raise the navigation event
                curLocIndex = newLocIndex
                RaiseEvent HasNavigated(Me, New LocationChangedEventArgs(curLoc, newLoc, 1))

                'return the current location (which happens to be the destination passed)
                Return Item(curLocIndex)
            End If
        End If
    End Function

    ''' <summary>
    ''' Returns the index of the specified location in the Navigator.
    ''' </summary>
    ''' <param name="location">Location to be looked up.</param>
    ''' <param name="exactLocation">
    ''' If set to <c>true</c> the index of the location in the Navigator is returned; 
    ''' otherwise the index of the first location which evaluates true for specified location's 
    ''' <see cref="Location.Equals"/>.
    ''' </param>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Function IndexOf(ByVal location As Location, Optional ByVal exactLocation As Boolean = True) As Integer
        If exactLocation Then
            Return Me.IndexOf(Function(l) l Is location)
        Else
            Return List.IndexOf(location)
        End If
    End Function

    ''' <summary>
    ''' Returns the index of the Location with the specified path in the Navigator.
    ''' </summary>
    ''' <param name="path">Path to be evaluated.</param>
    <EditorBrowsable(EditorBrowsableState.Advanced), DebuggerStepperBoundary()> _
    Public Function IndexOf(ByVal path As String) As Integer
        Dim returnVal = -1

        'look through each of the locations in the navigator list
        For i = 0 To List.Count - 1
            If DirectCast(List(i), Location).FullPath = path Then
                returnVal = i
                Exit For
            End If
        Next

        Return returnVal
    End Function

    ''' <summary>Returns the index of the Location which satisfies a specified condition.</summary>
    ''' <param name="predicate">A function to test each element for a condition.</param>
    <EditorBrowsable(EditorBrowsableState.Advanced), DebuggerStepperBoundary()> _
    Public Function IndexOf(ByVal predicate As Predicate(Of Location)) As Integer
        For i = 0 To List.Count - 1
            If predicate.Invoke(DirectCast(List(i), Location)) Then Return i
        Next

        Return -1
    End Function

    Protected Overrides Sub OnInsertComplete(ByVal index As Integer, ByVal value As Object)
        MyBase.OnInsertComplete(index, value)
        DirectCast(value, Location).SetNavigator(Me)
    End Sub

    Protected Overrides Sub OnClearComplete()
        MyBase.OnClearComplete()
        curLocIndex = -1
    End Sub

    Protected Overrides Sub OnValidate(ByVal value As Object)
        If Not GetType(Location).IsAssignableFrom(value.GetType) Then
            Throw New Exception("Value must be a Location.")
        End If
    End Sub
End Class