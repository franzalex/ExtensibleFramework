Imports System.Diagnostics
Imports System.ComponentModel

Namespace Data
    ''' <summary>
    ''' Creates a connection string for accessing a database.
    ''' </summary>
    <DebuggerDisplay("{ToString()}"), DebuggerStepThrough()> _
    Public MustInherit Class DataConnection

        Private Const ProviderKey As String = "Provider"
        Private Const DataSourceKey As String = "DataSource"
        Private params As Dictionary(Of String, String)

        ''' <summary>
        ''' Creates a new instance of the ConnectionString class.
        ''' </summary>
        Public Sub New()
            params = New Dictionary(Of String, String)(StringComparer.CurrentCultureIgnoreCase)
            params.Add(ProviderKey, "Microsoft.ACE.OLEDB.12.0")
            params.Add(DataSourceKey, "")
        End Sub

        ''' <summary>
        ''' Creates a new instance of the ConnectionString class with the parameters specified.
        ''' </summary>
        ''' <param name="DataSource">Data source for the connection.</param>
        ''' <param name="DataProvider">Data provider for the connection.</param>
        Public Sub New(ByVal DataProvider As String, ByVal DataSource As String)
            Me.New()

            Me.DataSource = DataSource
            Me.DataProvider = DataProvider
        End Sub

        ''' <summary>
        ''' Creates a new instance of the ConnectionString class from the System.String specified.
        ''' </summary>
        ''' <param name="connString">System.String to be used to create the ConnectionString.</param>
        Public Sub New(ByVal connString As String)
            Me.New()
            For Each kvp In ParseParameters(connString)
                Me.params(kvp.Key) = kvp.Value
            Next
        End Sub

        ''' <summary>Get or set the data provider of the connection.</summary>
        Public Overridable Property DataProvider() As String
            Get
                Return params(ProviderKey)
            End Get
            Set(ByVal value As String)
                params(ProviderKey) = value
            End Set
        End Property

        ''' <summary>Get or set the data source for the connection.</summary>
        Public Overridable Property DataSource() As String
            Get
                Return params(DataSourceKey)
            End Get
            Set(ByVal value As String)
                'append inverted commas at beginning and end
                If (value.Contains(" ")) And Not (value.StartsWith("""") AndAlso value.EndsWith("""")) Then
                    params(DataSourceKey) = """" & value & """"
                Else
                    params(DataSourceKey) = value
                End If
            End Set
        End Property

        ''' <summary>Get or set the parameters of the connection string.</summary>
        Public Overridable ReadOnly Property Parameters() As Dictionary(Of String, String)
            Get
                Return params
            End Get
        End Property

        ''' <summary>
        ''' Returns a System.String that represents  the current ConnectionString
        ''' </summary>
        <EditorBrowsable(EditorBrowsableState.Advanced)> _
        Public Overrides Function ToString() As String
            Dim result As New List(Of String)()

            For Each kvp In params
                If Not kvp.Value.IsNullOrEmpty() Then
                    result.Add(kvp.Key & "=" & kvp.Value)
                End If
            Next

            Return String.Join(";", result.ToArray())
        End Function

        ''' <summary>Determines whether the specified System.Object is equal to the current Dinofage.Data.ConnectionStrng.</summary>
        ''' <param name="obj">The System.Object to compare with the current Dinofage.Data.ConnectionStrng.</param>
        ''' <returns>true if the specified System.Object is equal to the current Dinofage.Data.ConnectionStrng; otherwise, false.</returns>
        <EditorBrowsable(EditorBrowsableState.Advanced)> _
        Public Overrides Function Equals(ByVal obj As Object) As Boolean
            Return Me.ToString().ToLower() = obj.ToString().ToLower()
        End Function

        ''' <summary>Determines whether the specified Dinofage.Data.ConnectionStrng is equal to the current Dinofage.Data.ConnectionStrng.</summary>
        ''' <param name="connStr">The Dinofage.Data.ConnectionStrng to compare with the current Dinofage.Data.ConnectionStrng.</param>
        ''' <returns>true if the specified Dinofage.Data.ConnectionStrng is equal to the current Dinofage.Data.ConnectionStrng; otherwise, false.</returns>
        <EditorBrowsable(EditorBrowsableState.Advanced)> _
        Public Overloads Function Equals(ByVal connStr As DataConnection) As Boolean
            Return Me.ToString().ToLower() = connStr.ToString().ToLower()
        End Function

        ''' <summary>Parses the parameters in the given connection string.</summary>
        ''' <param name="ConnStr">The connection string.</param>
        ''' <returns>
        ''' A collection of key-value pairs containing the parameters parsed from the connection string.
        ''' </returns>
        Protected Friend Shared Function ParseParameters(ByVal ConnStr As String) As IEnumerable(Of KeyValuePair(Of String, String))
            Dim params As New Dictionary(Of String, String)(StringComparer.CurrentCultureIgnoreCase)


            For Each param In ConnStr.Split(";"c)   ' split individual parameters
                Dim splt = param.Split({"="c}, 2)   ' split parameters into key, value
                If splt.Length > 1 Then
                    params(splt(0)) = splt(1)
                End If
            Next


            'return the new connection string
            Return params.ToArray()

        End Function

        ''' <summary>Equality comparer for the ConnectionString class.</summary>
        ''' <param name="val1">First ConnectionString to be compared.</param>
        ''' <param name="val2">Second ConnectionString to be compared.</param>
        ''' <returns>Boolean. True if both ConnectinStrings are equal; false if otherwise.</returns>
        Public Shared Operator =(ByVal val1 As DataConnection, ByVal val2 As DataConnection) As Boolean
            Return val1.ToString() = val2.ToString()
        End Operator

        ''' <summary>Inequality comparer for the ConnectionString class.</summary>
        ''' <param name="val1">First ConnectionString to be compared.</param>
        ''' <param name="val2">Second ConnectionString to be compared.</param>
        ''' <returns>Boolean. True if both ConnectinStrings are not equal; false if otherwise.</returns>	
        Public Shared Operator <>(ByVal val1 As DataConnection, ByVal val2 As DataConnection) As Boolean
            Return Not val1 = val2
        End Operator

        ''' <summary>Equality comparer for the ConnectionString class.</summary>
        ''' <param name="val1">ConnectionString to be compared with a String.</param>
        ''' <param name="val2">String to be compared with a ConnectionString.</param>
        ''' <returns>Boolean. True if both are equal; false if otherwise.</returns>
        Public Shared Operator =(ByVal val1 As DataConnection, ByVal val2 As String) As Boolean
            Return val1.ToString() = val2
        End Operator

        ''' <summary>Inequality comparer for the ConnectionString class.</summary>
        ''' <param name="val1">ConnectionString to be compared with a String.</param>
        ''' <param name="val2">String to be compared with a ConnectionString.</param>
        ''' <returns>Boolean. True if both are not equal; false if otherwise.</returns>
        Public Shared Operator <>(ByVal val1 As DataConnection, ByVal val2 As String) As Boolean
            Return Not val1 = val2
        End Operator

        ''' <summary>
        ''' Creates a command builder to be used in generating statements for updating the database.
        ''' </summary>
        ''' <param name="dataAdapter">The data adapter from which the CommandBuilder will generate the commands.</param>
        ''' <returns>An instance of a <see cref="System.Data.Common.DbCommandBuilder" />.</returns>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        Public MustOverride Function CreateCommandBuilder(dataAdapter As System.Data.Common.DataAdapter) As System.Data.Common.DbCommandBuilder

        ''' <summary>
        ''' Creates an instance of a <see cref="System.Data.Common.DbDataAdapter" /> to be used in updating the database.
        ''' </summary>
        ''' <returns>An instance of a <see cref="System.Data.Common.DbDataAdapter" />.</returns>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        Public MustOverride Function CreateDataAdapter() As System.Data.Common.DbDataAdapter

        ''' <summary>
        ''' Creates a command to execute an SQL statement or stored procedure against the <see cref="DataSource" />.
        ''' </summary>
        ''' <param name="cmdText">The command text.</param>
        ''' <returns>
        ''' An instance of a class that inherits <seealso cref="System.Data.Common.DbCommand" />.
        ''' </returns>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        Public MustOverride Function CreateDbCommand(cmdText) As System.Data.Common.DbCommand

        ''' <summary>
        ''' Creates a suitable instance of a class inheriting 
        ''' <seealso cref="System.Data.Common.DbConnection"/> from this <see cref="DataConnection"/>.
        ''' </summary>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        Public MustOverride Function CreateDbConnection() As System.Data.Common.DbConnection

    End Class
End Namespace