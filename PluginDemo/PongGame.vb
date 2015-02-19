Imports System.Drawing
Imports System.Windows.Forms

Public Class PongGame
    Private Enum GameStates
        Intro
        InGame
        Intermission
        GameOver
    End Enum
    Private Enum Direction
        Up = -1
        None = 0
        Down = 1
    End Enum

    Private Const BoardHeight As Integer = 360
    Private Const BoardWidth As Integer = 480
    Private Const ballRadius As Integer = 8
    Private ReadOnly pad As SizeF = New SizeF(8, BoardHeight * 0.25F)
    Private ReadOnly elemsColor As Color = Color.FromArgb(&HFFA2FF26)

    Dim gameState As GameStates

    Dim ballPos As PointF
    Dim ballSpeed As PointF
    Dim cpuPad As Single
    Dim plyrPad As Single
    Dim cpuScore As Integer
    Dim plyrScore As Integer
    Dim plyrPadDirxn As Direction

    Dim cpuRxnSpd As Double     ' accepted range: 0.0 to 1.0  [0.0=Beginner; 1.0=Expert]


    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        ' set the parameters to speed up drawing
        Me.SetStyle(ControlStyles.UserPaint, True)
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        Me.SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
    End Sub

    Private Sub AdjustBoardParameters()
        ' drawing rectangle
        Dim cr = New RectangleF(Me.ClientRectangle.Location, Me.ClientRectangle.Size)
        cr = New RectangleF(cr.X, cr.Y, Math.Max(cr.Width, cr.Height), Math.Max(cr.Width, cr.Height))
        cr.Offset((cr.Width - Me.ClientRectangle.Width) / -2.0F,
                  (cr.Height - Me.ClientRectangle.Height) / -2.0F)


        ' set the graphics path and gradient brush
        Dim brdPath = New Drawing2D.GraphicsPath()
        brdPath.AddEllipse(cr)
        Dim boardColorLight As Color = Color.FromArgb(&HFF0F5A82)
        Dim boardColorDark As Color = Color.FromArgb(&HFF071112)
        Dim brdSolidBrush = New SolidBrush(boardColorDark)
        Dim brdGradientBrush = New Drawing2D.PathGradientBrush(brdPath) With {
            .CenterColor = boardColorLight,
            .CenterPoint = New PointF((cr.Width / 2.0F) + cr.Left, (cr.Height / 2.0F) + cr.Top),
            .SurroundColors = New Color() {boardColorDark}
        }


        ' render the background image
        Dim background As New Bitmap(Me.ClientRectangle.Width, Me.ClientRectangle.Height)
        Using g = Graphics.FromImage(background)
            g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBilinear
            g.PixelOffsetMode = Drawing2D.PixelOffsetMode.Half
            g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
            g.FillRectangle(brdSolidBrush, cr)
            g.FillPath(brdGradientBrush, brdPath)
        End Using

        ' clean up objects
        brdPath.Dispose()
        brdSolidBrush.Dispose()
        brdGradientBrush.Dispose()

        ' display the background image
        If Me.BackgroundImage IsNot Nothing Then Me.BackgroundImage.Dispose()
        Me.BackgroundImage = background
        Me.BackgroundImageLayout = ImageLayout.None
    End Sub

    Private Sub PongGame_Initializing(sender As Object, e As ExtensibleFramework.Core.InitializingEventArgs) Handles Me.Initializing
        AdjustBoardParameters()
    End Sub

    Private Sub PongGame_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.Up
                plyrPadDirxn = Direction.Up
            Case Keys.Down
                plyrPadDirxn = Direction.Down
            Case Else
                plyrPadDirxn = Direction.None
        End Select
    End Sub

    Private Sub PongGame_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.Enter AndAlso
            (gameState = GameStates.Intro OrElse gameState = GameStates.Intermission) Then
            StartGame(resetAI:=gameState = GameStates.Intro)

        ElseIf e.KeyCode = Keys.Pause AndAlso gameState = GameStates.InGame Then
            tmrUpdate.Enabled = Not tmrUpdate.Enabled

        ElseIf {Keys.Up, Keys.Down}.Contains(e.KeyCode) Then
            plyrPadDirxn = Direction.None
        End If
    End Sub

    Private Sub PongGame_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint
        Static fpsCounter As FpsCounter = New FpsCounter()
        Static fpsFont As Font = New Font("Consolas", 9, FontStyle.Bold)

        ' board rectangle variable; used to offset other drawing elements
        Dim brdRect = New RectangleF(0, 0, BoardWidth, BoardHeight)
        brdRect.Offset((Me.ClientRectangle.Width - BoardWidth) / 2.0F,
                       (Me.ClientRectangle.Height - BoardHeight) / 2.0F)


        Dim g = e.Graphics
        g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBilinear
        g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality

        Select Case gameState
            Case GameStates.Intro
                DrawIntro(g)
            Case GameStates.InGame
                DrawState(g, brdRect)
            Case GameStates.Intermission
                DrawIntermission(g, brdRect)
            Case GameStates.GameOver
                ' DrawEnd(g)
        End Select

        ' calculate and draw FPS
        g.DrawString("Real FPS: " & fpsCounter.MarkFrame().ToString("0.0") & vbCrLf &
                     "Avg FPS:  " & fpsCounter.FPS.ToString("0.0"),
                     fpsFont, Brushes.LightYellow, New Point(5, 5))

    End Sub

    Private Sub PongGame_PreviewKeyDown(sender As Object, e As PreviewKeyDownEventArgs) Handles Me.PreviewKeyDown
        ' allow arrow keys and ENTER to be used for input
        e.IsInputKey = {Keys.Left, Keys.Up, Keys.Right, Keys.Down, Keys.Enter}.Contains(e.KeyCode)
    End Sub

    Private Sub PongGame_SizeChanged(sender As Object, e As EventArgs) Handles Me.Resize
        Dim myForm = Me.FindForm()
        If myForm IsNot Nothing AndAlso myForm.WindowState <> FormWindowState.Minimized Then
            Me.AdjustBoardParameters()
            Me.Invalidate() ' force a redraw of the board
        End If
    End Sub

    Private Sub PongGame_Started(sender As Object, e As EventArgs) Handles Me.Started
        '
        gameState = GameStates.Intro
    End Sub

    Private Sub tmrUpdate_Tick(sender As Object, e As EventArgs) Handles tmrUpdate.Tick
        If Me.State <> ExtensibleFramework.Core.ActivityState.Running Then Return

        tmrUpdate.Stop()
        Me.Invalidate() ' force the game to redraw itself
        tmrUpdate.Interval = CInt(1000 / If(gameState = GameStates.InGame, 40, 2) * 1.1)
        tmrUpdate.Start()
    End Sub

    Private Sub DrawIntro(g As Graphics)
        Dim title = "Pong"
        Dim intro = {"Welcome to Pong.",
                     "Move your paddle (on the right) with UP/DOWN to keep the ball in play.",
                     "Press ENTER to start."}
        Dim titleFont = New Font("Segoe UI", 24, FontStyle.Bold)
        Dim introFont = New Font("Segoe UI", 8, FontStyle.Bold)
        Dim titleSz = TextRenderer.MeasureText(title, titleFont)
        Dim introSz = TextRenderer.MeasureText(String.Join(vbCrLf, intro), introFont)

        Dim titlePt = New PointF((Me.ClientRectangle.Width - titleSz.Width) / 2.0F,
                                 (Me.ClientRectangle.Height - (titleSz.Height * 1.25F + introSz.Height * 1.1F)) / 2)

        TextRenderer.DrawText(g, title, titleFont, titlePt.ToPoint(), elemsColor)
        Dim nextY = titlePt.Y + (titleSz.Height * 1.25F)
        For Each line In intro
            Dim sz = TextRenderer.MeasureText(line, introFont)
            Dim x = (Me.ClientSize.Width - sz.Width) / 2.0F
            TextRenderer.DrawText(g, line, introFont, New PointF(x, nextY).ToPoint(), elemsColor)
            nextY += sz.Height * 1.15F
        Next

        titleFont.Dispose()
        introFont.Dispose()
    End Sub

    Private Sub DrawState(g As Graphics, board As RectangleF)
        Static scoreFont As Font = New Font("Segoe UI", 32, FontStyle.Bold)
        Static skillFont As Font = New Font("Consolas", 9, FontStyle.Bold)


        ' board rectangle & center variables; used to offset other drawing elements
        Dim brdCenter = New PointF(board.Left + board.Width / 2.0F,
                                   board.Top + board.Height / 2.0F)


        ' draw the paddles and ball
        If gameState = GameStates.InGame Then Me.MoveElements()
        Me.DrawElements(g, board)


        ' draw the scores
        Dim scoreText = cpuScore & "     " & plyrScore
        Dim scoreW = TextRenderer.MeasureText(scoreText, scoreFont).Width
        g.DrawString(scoreText, scoreFont, Brushes.GreenYellow,
                     New PointF(brdCenter.X - scoreW * 0.5F, board.Top))

        ' draw AI level
        g.DrawString("AI: " & (cpuRxnSpd * 10).ToString("#0"), skillFont,
                     Brushes.White, New PointF(board.Left + 5, board.Bottom - 16))

        ' check if ball is still in game
        If Not BallInPlay() Then
            If ballPos.X < 0 Then
                ' player has beaten CPU
                plyrScore += 1
            Else
                ' CPU has beaten player
                cpuScore += 1
            End If

            gameState = If(Math.Max(plyrScore, cpuScore) >= 10,
                           GameStates.GameOver, GameStates.Intermission)
        End If
    End Sub

    Private Sub DrawIntermission(g As Graphics, bounds As RectangleF)
        Static textFont As Font = New Font("Segoe UI", 16, FontStyle.Bold)

        ' draw the board and its elements
        DrawElements(g, bounds)


        ' draw the intermission text
        Dim text As String
        If Math.Max(plyrScore, cpuScore) < 10 Then
            text = If(ballPos.X < 0, "Player ", "Computer ") & "scores!"

        Else
            text = If(plyrScore > cpuScore, "Player ", "Computer ") & "wins!"
        End If

        text &= vbCrLf & vbCrLf & "Press ENTER to continue."

        Dim textSize = TextRenderer.MeasureText(text, textFont)
        g.DrawString(text, textFont, Brushes.Gold,
                     New PointF(bounds.Left + bounds.Width / 2.0F,
                                bounds.Top + bounds.Height / 2.0F),
                     New StringFormat() With {.Alignment = StringAlignment.Center,
                                             .LineAlignment = StringAlignment.Center})
    End Sub

    ''' <summary>Starts the game.</summary>
    Private Sub StartGame(Optional resetAI As Boolean = True)
        Dim rand = New Random()

        ' set paddle and ball positions
        plyrPad = 0
        cpuPad = 0
        ballPos = New PointF(0, 0)
        plyrPadDirxn = Direction.None

        If resetAI Then
            ' set the CPU intelligence
            cpuRxnSpd = rand.NextDouble()
        Else
            ' jitter CPU intelligence
            cpuRxnSpd = (cpuRxnSpd + rand.NextDouble(-0.2, 0.2)).FitToRange(0, 1)
        End If

        ballSpeed = New PointF(rand.Next(40, 80) / 10.0F * If(rand.NextDouble() < 0.5, -1, 1),
                               rand.Next(30, 60) / 10.0F)

        gameState = GameStates.InGame

        tmrUpdate.Start()
    End Sub

    ''' <summary>Moves the ball and paddles.</summary>
    Private Sub MoveElements()
        ' move pads
        Dim padSpeed = 6
        plyrPad += padSpeed * plyrPadDirxn
        cpuPad += padSpeed * GetCpuPadDirection()

        ' keep pads on board
        Dim padMax = (BoardHeight / 2.0F) - (pad.Height / 2)
        Dim padMin = -padMax
        plyrPad = Math.Min(padMax, Math.Max(padMin, plyrPad))
        cpuPad = Math.Min(padMax, Math.Max(padMin, cpuPad))

        ' move the ball
        ballPos.X += ballSpeed.X
        ballPos.Y += ballSpeed.Y

        ' reverse ball direction when ball reaches edges
        Dim ballXBound = (BoardWidth / 2.0F) - ballRadius
        Dim ballYBound = (BoardHeight / 2.0F) - ballRadius
        If -ballYBound >= ballPos.Y OrElse ballYBound <= ballPos.Y Then ballSpeed.Y *= -1
        If -ballXBound >= ballPos.X OrElse ballXBound <= ballPos.X Then ballSpeed.X *= -1.1F

        ' prevent super high speed balls
        Dim maxBallSpeed = 8
        ballSpeed.X = ballSpeed.X.FitToRange(-maxBallSpeed, maxBallSpeed)
    End Sub

    Private Function GetCpuPadDirection() As Direction
        Static cpuMover As Random = New Random()

        ' fit CPU reaction threshold range 0.25 - 1.0
        Dim cpuSkill = (Math.Max(0.25F, Math.Min(1.0F, cpuRxnSpd)) / 1)
        ' factor in the distance of the ball from CPU pad
        Dim urgency = (-(ballPos.X - 240) / 480)


        Dim qtrpad = pad.Height / 4.0F
        Dim padTop = ballPos.Y.CompareTo(cpuPad - qtrpad)
        Dim padBtm = ballPos.Y.CompareTo(cpuPad + qtrpad)
        Dim direction = DirectCast(Math.Sign(padTop + padBtm), Direction)

        If direction <> PongGame.Direction.None AndAlso
            (cpuMover.NextDouble() < Math.Max(cpuSkill, urgency)) Then
            Return direction
        Else
            Return direction.None
        End If
    End Function

    Private Sub DrawElements(g As Graphics, board As RectangleF)
        Dim brdCenter = New PointF(board.Left + board.Width / 2.0F,
                                   board.Top + board.Height / 2.0F)
        Dim halfPad = pad.Height / 2.0F

        ' create pad rectangles
        Dim cpuPadRect = New RectangleF(board.Left, brdCenter.Y - halfPad, pad.Width, pad.Height)
        Dim plyrPadRect = New RectangleF(board.Right, brdCenter.Y - halfPad, pad.Width, pad.Height)
        cpuPadRect.Offset(0, cpuPad)
        plyrPadRect.Offset(-pad.Width, plyrPad)

        ' get ball rectangle
        Dim ballRect = New RectangleF(brdCenter, New SizeF(ballRadius * 2, ballRadius * 2))
        ballRect.Offset(-ballRadius, -ballRadius)
        ballRect.Offset(ballPos)


        ' draw board outline and center line
        g.DrawRectangle(New Pen(Color.GreenYellow, 5), board)
        g.DrawLine(New Pen(Color.GreenYellow, 5), brdCenter.X, board.Top,
                                                  brdCenter.X, board.Bottom)
        ' draw pads and ball
        g.FillRectangle(Brushes.Red, cpuPadRect)
        g.FillRectangle(Brushes.Aqua, plyrPadRect)
        g.FillEllipse(Brushes.Yellow, ballRect)
    End Sub

    ''' <summary>Determines if the ball is in the game.</summary>
    ''' <returns><c>true</c> if the ball is still in play; otherwise <c>false</c>.</returns>
    Private Function BallInPlay() As Boolean
        Dim brdRight = (BoardWidth / 2.0F) - (1.5F * ballRadius)
        Dim brdLeft = -brdRight

        If ballPos.X.IsInRange(brdLeft, brdRight) Then
            Return True
        Else
            Dim padTop = If(ballPos.X < 0, cpuPad, plyrPad) - pad.Height / 2
            Dim padBottom = padTop + pad.Height

            'Return padTop <= ballPos.Y AndAlso ballPos.Y <= padBottom
            Return ballPos.Y.IsInRange(padTop, padBottom)
        End If
    End Function

End Class

Friend Module ExtensionMethods
    <Runtime.CompilerServices.Extension(), DebuggerStepThrough()>
    Public Function ToPoint(pt As PointF) As Point
        Return New Point(CInt(pt.X), CInt(pt.Y))
    End Function

    <Runtime.CompilerServices.Extension(), DebuggerStepThrough()>
    Public Sub DrawRectangle(g As Graphics, pen As Pen, rect As RectangleF)
        ' custom implementation of Graphics.DrawRectangle for RectangleF 
        ' breaks rectangle into borders and draws them 

        Using sb = New SolidBrush(pen.Color)
            ' save the smoothing mode; multiple rectangles draw best only with SmoothingMode.None
            Dim sm = g.SmoothingMode
            g.SmoothingMode = Drawing2D.SmoothingMode.None

            ' rectangle creation variables
            Dim halfW = pen.Width / 2.0F
            Dim rectT = rect.Top - halfW
            Dim rectL = rect.Left - halfW
            Dim topRect = New RectangleF(New PointF(rectL, rectT), New SizeF(rect.Width + pen.Width, pen.Width))
            Dim leftRect = New RectangleF(New PointF(rectL, rectT), New SizeF(pen.Width, rect.Height + pen.Width))
            Dim rightRect = New RectangleF(leftRect.Location, leftRect.Size)
            Dim bottomRect = New RectangleF(topRect.Location, topRect.Size)
            rightRect.Offset(rect.Width, 0)
            bottomRect.Offset(0, rect.Height)

            ' draw the rectangles
            g.FillRectangles(sb, {topRect, bottomRect, leftRect, rightRect})

            ' restore old smoothing mode
            g.SmoothingMode = sm
        End Using
    End Sub

    <Runtime.CompilerServices.Extension(), DebuggerStepThrough()>
    Public Function FitToRange(Of T As IComparable(Of T))(value As T, min As T, max As T) As T
        Dim bounds = min.CompareTo(max)


        Select Case bounds
            Case -1     ' min is less than max
                If value.CompareTo(min) <= 0 Then Return min ' value is less or equal min
                If value.CompareTo(max) >= 0 Then Return max ' value greater or equal max
                Return value

            Case 0  ' min equals max
                Return min

            Case 1  ' min greater than max
                Return value.FitToRange(max, min) ' interchange position of min & max

        End Select
    End Function

    <Runtime.CompilerServices.Extension(), DebuggerStepThrough()>
    Public Function IsInRange(Of T As IComparable(Of T))(value As T, min As T, max As T) As Boolean
        Dim bounds = min.CompareTo(max)

        Select Case bounds
            Case -1     ' min is less than max
                If value.CompareTo(min) < 0 Then Return False ' value is less than min
                If value.CompareTo(max) > 0 Then Return False ' value greater than max
                Return True

            Case 0  ' min equals max
                Return value.CompareTo(min) = 0

            Case 1  ' min greater than max
                Return value.IsInRange(max, min) ' interchange position of min & max

        End Select

        ' should we ever get here, then IComparer<T>.CompareTo() has failed & .NET is broken  ;-)
        Return False
    End Function

    <Runtime.CompilerServices.Extension()>
    Public Function NextDouble(value As Random, min As Double, max As Double) As Double
        Return (value.NextDouble() * (max - min)) + min
    End Function
End Module
