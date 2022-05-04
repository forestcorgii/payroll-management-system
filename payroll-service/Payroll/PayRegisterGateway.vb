

Imports System.Globalization
Imports System.IO
Imports employee_module
Imports utility_service
Imports NPOI.HSSF.UserModel
Imports NPOI.SS.UserModel
Imports MySql.Data.MySqlClient

Namespace Payroll

    Public Class PayRegisterGateway
        Public Shared Function Collect(databaseManager As Manager.Mysql) As List(Of PayRegisterModel)
            Dim summaries As New List(Of PayRegisterModel)
            Try
                Using reader As MySqlDataReader = databaseManager.ExecuteDataReader("SELECT * FROM payroll_db.payregister_summary;")
                    While reader.Read
                        summaries.Add(New PayRegisterModel(reader))
                    End While
                End Using
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try
            Return summaries
        End Function

    End Class
End Namespace
