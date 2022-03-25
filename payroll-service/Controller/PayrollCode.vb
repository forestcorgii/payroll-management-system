Imports System.Windows.Forms
Imports MySql.Data.MySqlClient
Imports utility_service

Namespace Controller
    Public Class PayrollCode
        Public Shared Function CollectFromPreviousCutOff(databaseManager As Manager.Mysql, payrollCode As String, previousCutoffRange As Date()) As List(Of Model.PayrollCode)
            Dim payrollCodes As New List(Of Model.PayrollCode)

            'NOTE: COLLECT BANK CATEGORIES FROM THE PREVIOUS CUT OFF
            Using reader As MySqlDataReader = databaseManager.ExecuteDataReader(
                String.Format("SELECT * FROM payroll_management.payroll_code WHERE payroll_code='{0}' AND date_created <= '{1:yyyy-MM-dd}';", payrollCode, previousCutoffRange(1)))
                While reader.Read
                    payrollCodes.Add(New Model.PayrollCode(reader))
                End While
            End Using

            Return payrollCodes
        End Function

        Public Shared Sub SavePayrollCode(databaseManager As Manager.Mysql, payrollCode As String, ee_id As String)
            Try
                Dim command As New MySqlCommand("INSERT INTO payroll_management.payroll_code (ee_id, payroll_code)VALUES(?,?)", databaseManager.Connection)
                command.Parameters.AddWithValue("p1", ee_id)
                command.Parameters.AddWithValue("p2", payrollCode)
                command.ExecuteNonQuery()
            Catch ex As Exception
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub


        Public Shared Function GetAllPayrollCodes(databaseManager As Manager.Mysql) As List(Of String)
            Dim payrollCodes As New List(Of String)
            Using reader As MySqlDataReader = databaseManager.ExecuteDataReader("SELECT payroll_code FROM payroll_management.payroll_code GROUP BY payroll_code;")

                While reader.Read()
                    payrollCodes.Add(reader.Item("payroll_code"))
                End While
            End Using

            Return payrollCodes
        End Function


        Public Shared Function GetHistory(databaseManager As Manager.Mysql, ee_id As String) As Model.PayrollCodeHistory
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