Imports System.Windows.Forms
Imports MySql.Data.MySqlClient
Imports utility_service

Namespace Controller
    Public Class PayrollCode
        Public Shared Sub SavePayrollCode(databaseManager As Manager.Mysql, payrollCode As String, ee_id As Integer)
            Try
                Dim command As New MySqlCommand("INSERT INTO payroll_management.payroll_code (ee_id, payroll_code)VALUES(?,?)", databaseManager.Connection)
                command.Parameters.AddWithValue("p1", ee_id)
                command.Parameters.AddWithValue("p2", payrollCode)
                command.ExecuteNonQuery()
            Catch ex As Exception
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Public Shared Function GetHistory(databaseManager As Manager.Mysql, ee_id As Integer) As Model.PayrollCodeHistory
            Dim payrollCodeHistory As New Model.PayrollCodeHistory
            Try
                Using reader As MySqlDataReader = databaseManager.ExecuteDataReader(String.Format("SELECT * FROM payroll_management.payroll_code WHERE ee_id={0} ORDER BY date_created DESC", ee_id))
                    While reader.Read
                        payrollCodeHistory.History.Add(New Model.PayrollCode(reader))
                    End While
                End Using
            Catch ex As Exception
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
            Return payrollCodeHistory
        End Function
    End Class

End Namespace