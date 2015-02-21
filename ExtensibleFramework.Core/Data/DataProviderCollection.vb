Namespace Data
    Public Class DataProviderCollection
        Implements IEnumerable(Of DataProvider)

        Dim innerList As List(Of DataProvider)

        ''' <summary>
        ''' Initializes a new instance of the <see cref="DataProviderCollection"/> class.
        ''' </summary>
        Public Sub New()
            innerList = New List(Of DataProvider)()
        End Sub

        ''' <summary>Returns an enumerator that iterates through the <see cref="DataProvider"/>.</summary>
        Public Function GetEnumerator() As IEnumerator(Of DataProvider) Implements IEnumerable(Of DataProvider).GetEnumerator
            Return innerList.GetEnumerator()
        End Function

        ''' <summary>Returns an enumerator that iterates through the <see cref="DataProvider"/>.</summary>
        Private Function NonGenericEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return Me.GetEnumerator()
        End Function
    End Class
End Namespace