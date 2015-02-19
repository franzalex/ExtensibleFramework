Imports System.Linq

''' <summary>Class for counting the number of frames per second.</summary>
Public Class FpsCounter
    Dim _fps() As Double
    Dim fpsIdx As Integer
    Dim last_ms As Long

    ''' <summary>Initializes a new instance of the <see cref="FpsCounter"/> class.</summary>
    Public Sub New()
        Me.New(30)
    End Sub

    ''' <summary>Initializes a new instance of the <see cref="FpsCounter" /> class.</summary>
    ''' <param name="fpsBufferSize">
    ''' The size of the FPS buffer. Higher values reduces jumpiness of the 
    ''' <see cref="FpsCounter.FPS" /> value.
    ''' </param>
    Public Sub New(fpsBufferSize As Integer)
        _fps = New Double(fpsBufferSize - 1) {}
        fpsIdx = -1
        last_ms = Environment.TickCount
    End Sub

    ''' <summary>Gets the current frames per second.</summary>
    Public ReadOnly Property FPS As Double
        Get
            Return _fps.Average()
        End Get
    End Property

    ''' <summary>Gets the size of the FPS averaging buffer.</summary>
    Private ReadOnly Property FpsBufferSize As Integer
        Get
            Return _fps.Length
        End Get
    End Property

    ''' <summary>Marks the end of a frame.</summary>
    ''' <returns>The FPS of the frame that was just marked.</returns>
    Public Function MarkFrame() As Double
        Dim now_ms = Environment.TickCount ' number of elapsed milliseconds now

        fpsIdx = (fpsIdx + 1) Mod _fps.Length
        _fps(fpsIdx) = 1 / TimeSpan.FromMilliseconds(now_ms - last_ms).TotalSeconds

        last_ms = Environment.TickCount

        Return _fps(fpsIdx)
    End Function

    ''' <summary>Returns a <see cref="System.String" /> that represents this instance.</summary>
    ''' <returns>A <see cref="System.String" /> that represents this instance.</returns>
    Public Overrides Function ToString() As String
        Return String.Format("{0:#,##0.0} FPS (From last {1} frames)", Me.FPS, Me.FpsBufferSize)
    End Function
End Class
