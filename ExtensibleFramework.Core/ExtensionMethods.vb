Imports System.Runtime.CompilerServices
Imports System.Linq

''' <summary>Contains extension methods for extending classes. (.NET 3.5+ only)</summary>
Friend Module ExtensionMethods

    ''' <summary>
    ''' Indicates whether the specified string is a null reference 
    ''' (Nothing in Visual Basic) or an Empty string.
    ''' </summary>
    ''' <param name="value">The string to test.</param>
    ''' <returns>
    ''' true if the value parameter is a null reference (Nothing in Visual Basic) 
    ''' or an empty string (""); otherwise, false.
    ''' </returns>
    <Extension(), DebuggerStepThrough()> Function IsNullOrEmpty(ByVal value As String) As Boolean
        Return String.IsNullOrEmpty(value)
    End Function

    ''' <summary>
    ''' Determines whether two COM types have the same identity and are eligible for type equivalence.
    ''' </summary>
    ''' <param name="type">The type to compare another with.</param>
    ''' <param name="other">The COM type that is tested for equivalence with the current type.</param>
    <Extension(), DebuggerStepThrough()> Public Function IsEquivalentTo(ByVal [type] As Type, ByVal other As Type) As Boolean
        Return ([type] Is other)
    End Function

    ''' <summary>Determines whether the specified value lies within a given range.</summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="value">The value to be evaluated.</param>
    ''' <param name="lower">The lower inclusive boundary.</param>
    ''' <param name="upper">The upper inclusive boundary.</param>
    ''' <returns>
    ''' <c>true</c> if <paramref name="value"/> lies within the range specified; otherwise <c>false</c>.
    ''' </returns>
    <Extension()>
    Public Function IsInRange(Of T As IComparable)(ByVal value As T, lower As T, upper As T) As Boolean
        Dim comp = Comparer(Of T).Default
        ' return values for Comparer<T>.Compare(x, y)
        ' x <  y = -1
        ' x >  y =  1
        ' x == y =  0
        Return comp.Compare(value, lower) >= 0 AndAlso comp.Compare(value, upper) <= 0
    End Function

    ''' <summary>
    ''' Determines whether one or more bit fields are set in the current instance.
    ''' </summary>
    ''' <param name="value">Enumeration to be evaluated.</param>
    ''' <param name="flag">An enumeration value.</param>
    <Extension(), DebuggerStepThrough()> Function HasFlag(Of T As Structure)(ByVal value As T, ByVal flag As T) As Boolean
        If Not GetType(T).IsEnum Then
            Throw New InvalidOperationException("Generic type argument is not a System.Enum.")
        End If
        If Not value.GetType.IsEquivalentTo(flag.GetType) Then
            Throw New ArgumentException("The argument Enum type does not match")
        End If

        Dim num = Convert.ToInt64(flag)
        Return (Convert.ToInt64(value) And num) = num

    End Function

    ''' <summary>
    ''' Gets the name of the specified enum value.
    ''' </summary>
    ''' <param name="value">The enum value.</param>
    <ExtensionAttribute()>
    Public Function GetName(ByVal value As [Enum]) As String
        Return [Enum].GetName(value.GetType, value)
    End Function

    ''' <summary>
    ''' Compares two specified System.String objects, ignoring or honoring their case.
    ''' </summary>
    ''' <param name="strA">The first System.String.</param>
    ''' <param name="strB">The second System.String.</param>
    ''' <param name="ignoreCase">
    ''' A System.Boolean indicating a case-sensitive or insensitive comparison.
    ''' (true indicates a case-insensitive comparison.</param>
    <Extension(), DebuggerStepThrough()> _
    Function Compare(ByVal strA As String, ByVal strB As String, Optional ByVal ignoreCase As Boolean = True) As Integer
        Return String.Compare(strA, strB, ignoreCase)
    End Function

    ''' <summary>
    ''' Splits the specified string at uppercase letters, and optionally, at non-letter characters.
    ''' </summary>
    ''' <param name="text">Text to be split.</param>
    ''' <param name="splitAtNonLetters">Option to split text at non-letter characters.</param>
    <Extension()> Public Function SplitAtCap(ByVal text As String, Optional ByVal splitAtNonLetters As Boolean = False) As String
        Dim returnVal As String = ""
        Dim chars() As Char = text.ToCharArray
        Dim prevChar As Char

        For x As Integer = 0 To chars.Length - 1
            If x > 0 Then prevChar = prevChar
            If x = 0 Then
                returnVal &= chars(x).ToString
                Continue For
            End If

            'current char is not letter
            If (Not Char.IsLetter(chars(x))) Then
                If (Not Char.IsLetter(prevChar)) Then   'if previous is not letter, append
                    returnVal &= chars(x).ToString
                Else                                    'if previous is a letter, append with space
                    returnVal &= " " & chars(x).ToString
                End If
                Continue For
            End If

            'current char is uppercase
            If Char.IsUpper(chars(x)) Then
                If (Char.IsUpper(prevChar)) Then    'if previous char is also upper, append
                    returnVal &= chars(x).ToString
                Else                                'if previous char is lower, append with space
                    returnVal &= " " & chars(x).ToString

                End If
                Continue For
            End If

            'current char is lowercase
            If Char.IsLower(chars(x)) Then
                If Not Char.IsLetter(prevChar) Then 'prev char is not a letter
                    If splitAtNonLetters Then   'append with space if split at nonletter
                        returnVal &= " " & chars(x).ToString
                        Continue For
                    Else                        'append w/o space if not split at nonletter
                        returnVal &= chars(x).ToString
                        Continue For
                    End If

                Else
                    returnVal &= chars(x).ToString
                End If
            End If

        Next

        Return returnVal.Replace("  ", " ")
    End Function

    ''' <summary>
    ''' Partitions the specified source into units of the specified size.
    ''' </summary>
    ''' <typeparam name="T">The type of elements of <i>source</i>.</typeparam>
    ''' <param name="source">The IEnumerable to partition.</param>
    ''' <param name="partSize">Size of each partition.</param>
    ''' <returns></returns>
    <System.Runtime.CompilerServices.Extension(), DebuggerStepThrough()> _
    Public Function InGroupsOf(Of T)(ByVal source As IEnumerable(Of T), ByVal partSize As Integer) As IEnumerable(Of IEnumerable(Of T))
        Return source.Where(Function(x, i) i Mod partSize = 0).Select(Function(x, i) source.Skip(i * partSize).Take(partSize))
    End Function

    ''' <summary>Performs the specified action on each element of the specified enumerable collection.</summary>
    ''' <typeparam name="T">The type of the elements of the collection.</typeparam>
    ''' <param name="source">An object implementing the IEnumerable(Of T) interface.</param>
    ''' <param name="action">The action to perform on each item in the collection.</param>
    <System.Runtime.CompilerServices.Extension(), DebuggerStepThrough()> _
    Public Sub ForEach(Of T)(ByVal source As IEnumerable(Of T), ByVal action As Action(Of T))
        For Each item In source
            action(item)
        Next
    End Sub

    ''' <summary>Assigns the value of the source to the target and returns the target value.</summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="source">The source of the assignment.</param>
    ''' <param name="target">The target of the assignment.</param>
    ''' <returns>The result from the source.</returns>
    <Extension(), DebuggerStepThrough()>
    Public Function AssignToAndReturn(Of T)(ByVal source As T, ByRef target As T) As T
        target = source
        Return target
    End Function

    ''' <summary>Determines if the <paramref name="value"/> passed is found in the <paramref name="set"/>.</summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="value">The value to be evaluated.</param>
    ''' <param name="set">The set of possible values.</param>
    ''' <returns>
    ''' <b>True</b> if the value is found in the set, otherwise <b>False</b>.
    ''' </returns>
    <Extension(), DebuggerStepThrough()>
    Public Function IsOneOf(Of T)(ByVal value As T, ByVal ParamArray [set]() As T) As Boolean
        Return [set].Contains(value)
    End Function

    ''' <summary>Determines if the <paramref name="value"/> passed is found in the <paramref name="set"/>.</summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="value">The value to be evaluated.</param>
    ''' <param name="set">The set of possible values.</param>
    ''' <returns>
    ''' <b>True</b> if the value is found in the set, otherwise <b>False</b>.
    ''' </returns>
    <Extension(), DebuggerStepThrough()>
    Public Function IsOneOf(Of T)(ByVal value As T, ByVal [set] As IEnumerable(Of T)) As Boolean
        Return [set].Contains(value)
    End Function

#Region "Array Extension Methods"
    ''' <summary>
    ''' Removes the item at the specified index in an array.
    ''' </summary>
    ''' <typeparam name="T">Type. Type of the array.</typeparam>
    ''' <param name="arr">Array of  T to remove an element from.</param>
    ''' <param name="index">Index of the item to be removed.</param>
    <Extension(), DebuggerStepThrough()> Function RemoveAt(Of T)(ByVal arr As T(), ByVal index As Integer) As T()
        Dim uBound = arr.GetUpperBound(0)
        Dim lBound = arr.GetLowerBound(0)
        Dim arrLen = uBound - lBound

        If index < lBound OrElse index > uBound Then
            Throw New ArgumentOutOfRangeException( _
            String.Format("Index must be from {0} to {1}.", lBound, uBound))

        Else
            'create an array 1 element less than the input array
            Dim outArr(arrLen - 1) As T
            'copy the first part of the input array
            Array.Copy(arr, 0, outArr, 0, index)
            'then copy the second part of the input array
            Array.Copy(arr, index + 1, outArr, index, uBound - index)

            Return outArr
        End If
    End Function

    ''' <summary>
    ''' Adds an item to the end of an array.
    ''' </summary>
    ''' <typeparam name="T">Type. Type of the array.</typeparam>
    ''' <param name="arr">Array of  T to add an element to.</param>
    <Extension(), DebuggerStepThrough()> Sub Add(Of T)(ByRef arr As T(), ByVal item As T)
        ReDim Preserve arr(arr.Length)
        arr(arr.Length - 1) = item
    End Sub
#End Region

#Region "String.Format Extension"
    ''' <summary>
    ''' Replaces the format item in a specified string with the string 
    ''' representation of a corresponding object in a specified array.
    ''' </summary>
    ''' <param name="s"> A composite format string.</param>
    ''' <param name="arg0">The first Object to format.</param>
    ''' <returns>A copy of format in which the format items have been replaced by the 
    ''' String equivalent of the corresponding instances of Object in args.</returns>
    <DebuggerStepThrough(), Extension()> _
    Public Function FormatWith(ByVal s As String, ByVal arg0 As Object) As String
        Return String.Format(s, arg0)
    End Function

    ''' <summary>
    ''' Replaces the format item in a specified string with the string 
    ''' representation of a corresponding object in a specified array.
    ''' </summary>
    ''' <param name="s"> A composite format string.</param>
    ''' <param name="arg0">The first Object to format.</param>
    ''' <param name="arg1">The second Object to format.</param>
    ''' <returns>A copy of format in which the format items have been replaced by the 
    ''' String equivalent of the corresponding instances of Object in args.</returns>
    <DebuggerStepThrough(), Extension()> _
    Public Function FormatWith(ByVal s As String, ByVal arg0 As Object, ByVal arg1 As Object) As String
        Return String.Format(s, arg0, arg1)
    End Function

    ''' <summary>
    ''' Replaces the format item in a specified string with the string 
    ''' representation of a corresponding object in a specified array.
    ''' </summary>
    ''' <param name="s"> A composite format string.</param>
    ''' <param name="arg0">The first Object to format.</param>
    ''' <param name="arg1">The second Object to format.</param>
    ''' <param name="arg2">The third Object to format.</param>
    ''' <returns>A copy of format in which the format items have been replaced by the 
    ''' String equivalent of the corresponding instances of Object in args.</returns>
    <DebuggerStepThrough(), Extension()> _
    Public Function FormatWith(ByVal s As String, ByVal arg0 As Object, ByVal arg1 As Object, ByVal arg2 As Object) As String
        Return String.Format(s, arg0, arg1, arg2)
    End Function

    ''' <summary>
    ''' Replaces the format item in a specified string with the string 
    ''' representation of a corresponding object in a specified array.
    ''' </summary>
    ''' <param name="s"> A composite format string.</param>
    ''' <param name="args">An Object array containing zero or more objects to format.</param>
    ''' <returns>A copy of format in which the format items have been replaced by the 
    ''' String equivalent of the corresponding instances of Object in args.</returns>
    <DebuggerStepThrough(), Extension()> _
    Public Function FormatWith(ByVal s As String, ByVal ParamArray args As Object()) As String
        Return String.Format(s, args)
    End Function
#End Region
End Module
