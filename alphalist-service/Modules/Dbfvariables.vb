Module Dbfvariables
    Public masterConnection As OleDb.OleDbConnection
    Public masterCommand As New OleDb.OleDbCommand
    Public masterDA As New OleDb.OleDbDataAdapter
    Public masterds As New DataSet
    Public mdbconstring As String = "PROVIDER=Microsoft.Jet.OLEDB.4.0;Data Source =" & Environment.CurrentDirectory & "\Masterdb.mdb"

    Public dbfconnection As OleDb.OleDbConnection
    Public dbfds As New DataSet
    Public dbfComm As New OleDb.OleDbCommand
    Public dbfda As New OleDb.OleDbDataAdapter
    Public dbfdt As DataTable

    Public noeedatatxt As String = Nothing
    Public notintext As String = Nothing
    Public alphatextactive As String = Nothing
    Public alphatextnonactive As String = Nothing
    Public alphatextmin As String = Nothing

    Public id As String = Nothing
    Public last As String = Nothing
    Public first As String = Nothing
    Public mid As String = Nothing
    Public ext As String = Nothing

    Public dbf13filename As String = Nothing
    Public dbf13fileloc As String = Nothing

End Module
