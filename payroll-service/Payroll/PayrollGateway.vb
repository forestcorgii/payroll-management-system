Imports System.Windows.Forms
Imports MySql.Data.MySqlClient
Imports utility_service

Namespace Payroll
    Public Class PayrollGateway

        Public Shared Function Find(databaseManager As Manager.Mysql, ee_id As String, payrollDate As String) As PayrollModel
            Dim payroll As PayrollModel = Nothing
            Using reader As MySqlDataReader = databaseManager.ExecuteDataReader(
                String.Format("SELECT * FROM payroll_db.payroll_complete where ee_id='{0}' AND payroll_date='{1}' LIMIT 1;", ee_id, payrollDate))
                If reader.HasRows Then
                    reader.Read()
                    payroll = New PayrollModel(reader)
                End If
            End Using

            Return payroll
        End Function

        Public Shared Function CollectPayrolls(databaseManager As Manager.Mysql, payrollDate As Date, payrollCode As String) As List(Of PayrollModel)
            Dim payrolls As New List(Of PayrollModel)
            Try
                Using reader As MySqlDataReader = databaseManager.ExecuteDataReader(
                String.Format("SELECT * FROM payroll_db.payroll_complete where payroll_date='{0:yyyy-MM-dd}' AND payroll_code='{1}';", payrollDate, payrollCode))
                    While reader.Read()
                        payrolls.Add(New PayrollModel(reader))
                    End While
                End Using
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Return payrolls
        End Function

        Public Shared Function CollectPayrollCodes(databaseManager As Manager.Mysql, payrollDate As Date) As List(Of String)
            Dim payrollCodes As New List(Of String)
            Using reader As MySqlDataReader = databaseManager.ExecuteDataReader(
                String.Format("SELECT payroll_code FROM payroll_db.payroll_complete WHERE payroll_date='{0:yyyy-MM-dd}' GROUP BY payroll_code;", payrollDate))
                While reader.Read()
                    payrollCodes.Add(reader("payroll_code"))
                End While
            End Using

            Return payrollCodes
        End Function

        Public Shared Sub Save(databaseManager As Manager.Mysql, payroll As PayrollModel)
            Try
                Dim command As New MySqlCommand("REPLACE INTO payroll_db.payroll (ee_id,payroll_date,gross_pay,payroll_name,adjust1,adjust2,net_pay)VALUES(?,?,?,?,?,?,?)", databaseManager.Connection)
                command.Parameters.AddWithValue("p1", payroll.EE_Id)
                command.Parameters.AddWithValue("p2", payroll.Payroll_Date)
                command.Parameters.AddWithValue("p3", payroll.Gross_Pay)
                command.Parameters.AddWithValue("p4", payroll.Payroll_Name)
                command.Parameters.AddWithValue("p5", payroll.Adjust1)
                command.Parameters.AddWithValue("p6", payroll.Adjust2)
                command.Parameters.AddWithValue("p7", payroll.Net_Pay)
                command.ExecuteNonQuery()
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Error Saving Payroll.", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub
    End Class
End Namespace
