Imports System.Windows.Forms
Imports MySql.Data.MySqlClient
Imports utility_service

Namespace Controller
    Public Class PayrollTime
        Public Shared Function GetPayrollTime(databaseManager As Manager.Mysql, Optional id As Integer = 0, Optional employee_id As String = "") As Model.PayrollTime
            Dim payrollTime As Model.PayrollTime = Nothing
            Using reader As MySqlDataReader = databaseManager.ExecuteDataReader(
                String.Format("SELECT * FROM payroll_management.payroll_time; where id={0} or employee_id='{1}' LIMIT 1;", id, employee_id))
                If reader.HasRows Then
                    reader.Read()
                    payrollTime = New Model.PayrollTime(reader)
                End If
            End Using

            Return payrollTime
        End Function

        Public Shared Function LoadPayrollTimes(databaseManager As Manager.Mysql, Optional location As String = "", Optional payroll_code As String = "", Optional bank_category As String = "", Optional payroll_date As String = "", Optional job_title As String = "", Optional completeDetail As Boolean = False) As List(Of Model.PayrollTime)
            Dim payrollTimes As New List(Of Model.PayrollTime)
            Using reader As MySqlDataReader = databaseManager.ExecuteDataReader(
                String.Format("SELECT * FROM payroll_management.payroll_time where (payroll_code='{1}' AND payroll_date='{3}');", location, payroll_code, bank_category, payroll_date, job_title))

                While reader.Read()
                    payrollTimes.Add(New Model.PayrollTime(reader))
                End While
            End Using

            If completeDetail Then
                payrollTimes.ForEach(Sub(item As Model.PayrollTime) CompletePayrollTimeDetail(databaseManager, item))
            End If

            Return payrollTimes
        End Function

        Public Shared Function CompletePayrollTimeDetail(databaseManager As Manager.Mysql, payrollTime As Model.PayrollTime) As Model.PayrollTime
            Try
                payrollTime.EE = Employee.GetEmployees(databaseManager, payrollTime.EE_Id)
            Catch ex As Exception
                MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
            Return payrollTime
        End Function

    End Class
End Namespace
