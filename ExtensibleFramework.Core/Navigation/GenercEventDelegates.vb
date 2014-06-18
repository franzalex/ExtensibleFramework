Imports System.ComponentModel

Public Module GenercEventDelegates
    ''' <summary>
    '''Represents the method that handles a generic event.
    ''' </summary>
    <SerializableAttribute()> _
    Public Delegate Sub GenericEventHandler(Of TSender, TEventArgs As EventArgs) _
        (ByVal sender As TSender, ByVal e As TEventArgs)

    ''' <summary>
    ''' Represents the method that handles a generic cancelable event.
    ''' </summary>
    Public Delegate Sub GenericCancelEventHandler(Of TSender, TEventArgs As CancelEventArgs) _
        (ByVal sender As TSender, ByVal e As TEventArgs)
End Module
