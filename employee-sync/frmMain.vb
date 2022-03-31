Imports hrms_api_service
Imports Newtonsoft.Json
Imports payroll_service
Imports utility_service

Public Class frmMain
    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = Application.ProductName & " v" & Application.ProductVersion

        SetupConfiguration()

        'Re-Runs Every 30 minutes.
        tmLister.Interval = TimeSpan.FromMinutes(30).TotalMilliseconds
        tmLister_Tick(Nothing, Nothing)
    End Sub

    Private Sub SetupConfiguration()
        DatabaseConfiguration = New Configuration.Mysql()
        DatabaseConfiguration.Setup("ACCOUNTING_DB_URL")

        DatabaseManager = New Manager.Mysql(DatabaseConfiguration)
        DatabaseManager.Connection.Open()
        SetupManager()
        DatabaseManager.Connection.Close()
    End Sub
    Private Function SetupManager()
        Dim settings As Model.Settings = Controller.Settings.GetSettings(DatabaseManager, "HRMSAPIManager", databaseName:="payroll_management")
        If settings IsNot Nothing Then
            HRMSAPIManager = JsonConvert.DeserializeObject(Of hrms_api_service.Manager.API.HRMS)(settings.JSON_Arguments)
        End If

        Return True
    End Function


    Private Async Sub bgwSync_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgwSync.DoWork
        Try
            DatabaseManager.Connection.Open()
            Dim employees As List(Of Employee.Model) = Employee.Gateway.Collect(DatabaseManager)
            If employees.Count > 0 Then
                Invoke(Sub()
                           pb.Value = 0
                           pb.Maximum = employees.Count
                       End Sub)
                Dim i As Integer = 0
                While i < employees.Count
                    Dim _employee As Employee.Model = employees(i)
                    Try
                        i += 1
                        Await Employee.Gateway.SyncEmployeeFromHRMS(DatabaseManager, HRMSAPIManager, _employee.EE_Id, _employee)
                        Invoke(Sub()
                                   pb.Value += 1
                                   lbStatus.Text = String.Format("Found {0} Employees... Currently Syncing {1}", employees.Count, _employee.EE_Id)
                               End Sub)
                    Catch ex As Exception
                        If ex.Message = "Employee not found in HRMS." Then
                            'archive employee
                        End If
                        Console.WriteLine(ex.Message)
                    End Try
                End While
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
