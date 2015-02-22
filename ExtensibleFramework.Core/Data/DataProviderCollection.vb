Namespace Data
    Public Class DataProviderCollection
        Implements IEnumerable(Of DataProvider), ICollection(Of DataProvider)

        Dim innerList As List(Of DataProvider)

        ''' <summary>
        ''' Initializes a new instance of the <see cref="DataProviderCollection"/> class.
        ''' </summary>
        Public Sub New()
            innerList = New List(Of DataProvider)()
        End Sub

        Public ReadOnly Property Item(index As Integer) As DataProvider
            Get
                Return innerList(index)
            End Get
        End Property

        ''' <summary>Returns an enumerator that iterates through the <see cref="DataProvider"/>.</summary>
        Public Function GetEnumerator() As IEnumerator(Of DataProvider) Implements IEnumerable(Of DataProvider).GetEnumerator
            Return innerList.GetEnumerator()
        End Function

        ''' <summary>Returns an enumerator that iterates through the <see cref="DataProvider"/>.</summary>
        Private Function NonGenericEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return Me.GetEnumerator()
        End Function

#Region "ICollection(Of DataProvider) Implementation"
        ''' <summary>
        ''' Adds the specified <see cref="DataProvider" /> to the <see cref="DataProviderCollection" />.
        ''' </summary>
        ''' <param name="item">The <see cref="DataProvider" /> to be added.</param>
        Protected Friend Sub Add(item As DataProvider) Implements ICollection(Of DataProvider).Add
            innerList.Add(item)
        End Sub

        ''' <summary>Removes all items from the <see cref="DataProviderCollection" />.</summary>
        Protected Friend Sub Clear() Implements ICollection(Of DataProvider).Clear
            innerList.Clear()
        End Sub

        ''' <summary>
        ''' Determines whether the <see cref="DataProviderCollection" /> contains the specified <see cref="DataProvider" />.
        ''' </summary>
        ''' <param name="item">The <see cref="DataProvider" /> to locate in the <see cref="DataProviderCollection" />.</param>
        ''' <returns>
        '''   <c>true</c> if the <see cref="DataProvider" /> is found in the <see cref="DataProviderCollection" /> otherwise <c>false</c>.
        ''' </returns>
        Public Function Contains(item As DataProvider) As Boolean Implements ICollection(Of DataProvider).Contains
            Return innerList.Contains(item)
        End Function

        ''' <summary>Copies the elements of the to an <see cref="Array"/> starting at the specified index.</summary>
        ''' <param name="array">The one-dimensional array that is the destination of the elements to be copied.</param>
        ''' <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        Public Sub CopyTo(array() As DataProvider, arrayIndex As Integer) Implements ICollection(Of DataProvider).CopyTo
            innerList.CopyTo(array, arrayIndex)
        End Sub

        ''' <summary>
        ''' Gets the number of elements contained in the <see cref="DataProviderCollection" />.
        ''' </summary>
        ''' <returns>The number of elements contained in the <see cref="DataProviderCollection" />.</returns>
        Public ReadOnly Property Count As Integer Implements ICollection(Of DataProvider).Count
            Get
                Return innerList.Count
            End Get
        End Property

        ''' <summary>
        ''' Gets a value indicating whether the <see cref="DataProviderCollection" /> is read-only.
        ''' </summary>
        ''' <returns>true if the <see cref="DataProviderCollection" /> is read-only; otherwise, false.</returns>
        Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of DataProvider).IsReadOnly
            Get
                Return False
            End Get
        End Property

        ''' <summary>
        ''' Removes the specified <see cref="DataProvider" /> from the <see cref="DataProviderCollection" />.
        ''' </summary>
        ''' <param name="item">The <see cref="DataProvider" /> to be removed.</param>
        ''' <returns>
        ''' true if <paramref name="item" /> was successfully removed; otherwise, false.
        ''' </returns>
        Protected Friend Function Remove(item As DataProvider) As Boolean Implements ICollection(Of DataProvider).Remove
            Return innerList.Remove(item)
        End Function
#End Region
    End Class
End Namespace