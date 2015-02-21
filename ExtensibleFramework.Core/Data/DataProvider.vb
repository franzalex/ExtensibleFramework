Imports System.Data

Namespace Data
    Public Class DataProvider

        Dim _lastExc As Exception
        Dim _connxn As DataConnection

        ''' <summary>Initializes a new instance of the <see cref="DataProvider"/> class.</summary>
        ''' <param name="connection">The connection to the database.</param>
        Public Sub New(connection As DataConnection)
            _connxn = connection
        End Sub

        ''' <summary>Gets the connection string used by this <see cref="DataProvider"/>.</summary>
        Public ReadOnly Property Connection As DataConnection
            Get
                Return _connxn
            End Get
        End Property

        ''' <summary>Gets or sets the last exception encountered.</summary>
        ''' <value>The last exception.</value>
        ''' <remarks>
        ''' Inheritor's Note: Clear the last exception before calling 
        ''' <see cref="DataProvider.ExecuteSelect"/> or <see cref="DataProvider.ExecuteSelect"/>.
        ''' </remarks>
        Public Property LastException As Exception
            Get
                Return _lastExc
            End Get
            Protected Set(value As Exception)
                _lastExc = value
            End Set
        End Property

        ''' <summary>Executes an SQL SELECT statement against the data provider.</summary>
        ''' <param name="statement">The SELECT statement to be executed.</param>
        ''' <returns>The rows in the data provider returned by the execution of the statement.</returns>
        Public Overridable Function ExecuteSelect(statement As System.Data.Common.DbCommand) As DataTable
            Dim dt As New DataTable()

            _lastExc = Nothing

            Try
                statement.Connection = _connxn.CreateDbConnection()
                dt.Load(statement.ExecuteReader(CommandBehavior.CloseConnection))
            Catch ex As Exception
                _lastExc = ex
            End Try

            Return dt
        End Function

        ''' <summary>Executes an SQL SELECT statement against the data provider.</summary>
        ''' <param name="statement">The SELECT statement to be executed.</param>
        ''' <returns>The rows in the data provider returned by the execution of the statement.</returns>
        <Obsolete("Risk of SQL injection when command is passed as String. " &
                  "Use the overload that uses the DbCommand parameter instead.")> _
        Public Overridable Function ExecuteSelect(statement As String) As DataTable
            Return Me.ExecuteSelect(_connxn.CreateDbCommand(statement))
        End Function

        ''' <summary>Executes an SQL UPDATE, DELETE or other non-selecting statement against the data provider.</summary>
        ''' <param name="statement">The non-selecting statement to be executed.</param>
        ''' <returns>The rows in the data provider returned by the execution of the statement.</returns>
        Public Overridable Function ExecuteNonSelect(statement As System.Data.Common.DbCommand) As Integer
            Dim result As Integer

            _lastExc = Nothing

            Try
                statement.Connection = _connxn.CreateDbConnection()
                result = statement.ExecuteNonQuery()
                statement.Connection.Close()
            Catch ex As Exception
                _lastExc = ex
            End Try

            Return result
        End Function

        ''' <summary>Executes an SQL UPDATE, DELETE or other non-selecting statement against the data provider.</summary>
        ''' <param name="statement">The non-selecting statement to be executed.</param>
        ''' <returns>The rows in the data provider returned by the execution of the statement.</returns>
        <Obsolete("Risk of SQL injection when command is passed as String. " &
                  "Use the overload that uses the DbCommand parameter instead.")> _
        Public Overridable Function ExecuteNonSelect(statement As String) As Integer
            Return Me.ExecuteNonSelect(_connxn.CreateDbCommand(statement))
        End Function

        ''' <summary>Updates the database using data from the specified <see cref="DataTable"/>.</summary>
        ''' <param name="selectCommand">The select statement which will be used to generate the other commands required for updating the database.</param>
        ''' <param name="table">The <see cref="DataTable"/> of rows to update the database with.</param>
        ''' <returns><c>true</c> if the database was successfully updated; otherwise <c>false</c>.</returns>
        Public Overridable Function Update(selectCommand As System.Data.Common.DbCommand, table As DataTable) As Integer
            Dim adapter = _connxn.CreateDataAdapter()
            Dim cmdBldr = _connxn.CreateCommandBuilder(adapter)

            Try
                _lastExc = Nothing

                'TODO: Investigate order of updates 
                'x Dim delRows = table.Select(Nothing, Nothing, DataViewRowState.Deleted).Any()
                'x Dim modRows = table.Select(Nothing, Nothing, DataViewRowState.ModifiedCurrent).Any()
                'x Dim newRows = table.Select(Nothing, Nothing, DataViewRowState.Added).Any()
                'x 
                'x ' set the adapter.SelectCommand and use the CommandBuilder to generate the rest
                'x adapter.SelectCommand = selectCommand
                'x If (delRows) Then adapter.DeleteCommand = cmdBldr.GetDeleteCommand()
                'x If (newRows) Then adapter.InsertCommand = cmdBldr.GetInsertCommand()
                'x If (modRows) Then adapter.UpdateCommand = cmdBldr.GetUpdateCommand()
                'x 
                'x ' update the database using the DataTable
                'x Dim updateFlags = DataViewRowState.Added Or
                'x                   DataViewRowState.ModifiedCurrent Or
                'x                   DataViewRowState.Deleted
                'x Return adapter.Update(table.Select(Nothing, Nothing, updateFlags))


                ' set the adapter.SelectCommand and use the CommandBuilder to generate the rest
                adapter.SelectCommand = selectCommand
                adapter.DeleteCommand = cmdBldr.GetDeleteCommand()
                adapter.InsertCommand = cmdBldr.GetInsertCommand()
                adapter.UpdateCommand = cmdBldr.GetUpdateCommand()

                ' update the database using the DataTable
                Return adapter.Update(table)
            Catch ex As Exception
                _lastExc = ex
                Return False
            End Try
        End Function

        ''' <summary>Updates the database using data from the specified <see cref="DataTable"/>.</summary>
        ''' <param name="selectCommand">The select statement which will be used to generate the other commands required for updating the database.</param>
        ''' <param name="table">The <see cref="DataTable"/> of rows to update the database with.</param>
        ''' <returns><c>true</c> if the database was successfully updated; otherwise <c>false</c>.</returns>
        <Obsolete("Risk of SQL injection when command is passed as String. " &
                  "Use the overload that uses the DbCommand parameter instead.")> _
        Public Overridable Function Update(selectCommand As String, table As DataTable) As Integer
            Return Me.Update(_connxn.CreateDbCommand(selectCommand), table)
        End Function
    End Class
End Namespace
