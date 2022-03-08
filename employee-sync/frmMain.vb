Imports utility_service
Imports payroll_service
Public Class frmMain
    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = Application.ProductName & " v" & Application.ProductVersion

        HRMSAPIManager = New Manager.API.HRMS("HRMS_API_URL")

        DatabaseConfiguration = New Configuration.MysqlConfiguration()
        DatabaseConfiguration.Setup("ACCOUNTING_DB_URL")

        DatabaseManager = New Manager.Mysql
        DatabaseManager.Connection = DatabaseConfiguration.ToMysqlConnection

        'Re-Runs Every 30 minutes.
        tmLister.Interval = TimeSpan.FromMinutes(30).TotalMilliseconds
        tmLister_Tick(Nothing, Nothing)
    End Sub

    Private Async Sub bgwSync_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgwSync.DoWork
        Try
            DatabaseManager.Connection.Open()
            Dim employees As List(Of Model.Employee) = Controller.Employee.CollectEmployeeForSyncing(DatabaseManager)
            If employees.Count > 0 Then
                Invoke(Sub()
                           pb.Value = 0
                           pb.Maximum = employees.Count
                       End Sub)
                For Each employee As Model.Employee In employees
                    Try
                        Invoke(Sub()
                                   pb.Value += 1
                                   lbStatus.Text = String.Format("Found {0} Employees... Currently Syncing {1}", employees.Count, employee.Employee_Id)
                               End Sub)
                        Await Controller.Employee.SyncEmployeeFromHRMSAsync(DatabaseManager, HRMSAPIManager, employee.Employee_Id, employee)
                    Catch ex As Exception
                        MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                Next
            End If
            DatabaseManager.Connection.Close()
        Catch ex As Exception
            DatabaseManager.Connection.Close()
            MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub tmLister_Tick(sender As Object, e As EventArgs) Handles tmLister.Tick
        bgwSync.RunWorkerAsync()
    End Sub
End Class
