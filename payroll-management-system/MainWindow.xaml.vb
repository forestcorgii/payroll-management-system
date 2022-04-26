Imports utility_service
Imports hrms_api_service
Imports Newtonsoft.Json
Imports System.ComponentModel
Imports employee_module

Class MainWindow

    Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        HRMSAPIManager = New Manager.API.HRMS()

        SetupConfiguration()

        LoggingService = New monitoring_module.Logging.LoggingService()
        SetupUserAuthentication()

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
        Dim settings As Model.Settings = Controller.Settings.GetSettings(DatabaseManager, "HRMSAPIManager", "accounting_monitoring")
        If settings IsNot Nothing Then
            HRMSAPIManager = JsonConvert.DeserializeObject(Of Manager.API.HRMS)(settings.JSON_Arguments)
        End If

        settings = Controller.Settings.GetSettings(DatabaseManager, "TimeDownloaderAPIManager", "accounting_monitoring")
        If settings IsNot Nothing Then
            TimeDownloaderAPIManager = JsonConvert.DeserializeObject(Of time_module.Manager.API.TimeDownloader)(settings.JSON_Arguments)
        End If

        Return True
    End Function

    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        DatabaseManager.Connection.Open()
        cbPayrollCode.ItemsSource = EmployeeGateway.CollectPayrollCodes(DatabaseManager)
        DatabaseManager.Connection.Close()

        frmMain.Navigate(New Payroll)
        btnProcessPayroll.IsChecked = True
        TimesheetPage = New Timesheet
    End Sub

    Private Sub btnProcessPayroll_Click(sender As Object, e As RoutedEventArgs)
        frmMain.Navigate(New Payroll)
    End Sub

    Private TimesheetPage As Timesheet
    Private Sub btnTimesheet_Click(sender As Object, e As RoutedEventArgs)
        frmMain.Navigate(TimesheetPage)
    End Sub

    Private LastTimeModified As Date
    Private EmployeePage As Employee
    Private Sub btnEmployee_Click(sender As Object, e As RoutedEventArgs)
        DatabaseManager.Connection.Open()
        Dim latestModifiedTime As Date = employee_module.EmployeeGateway.TimeHasChange(DatabaseManager)
        DatabaseManager.Connection.Close()

        If latestModifiedTime > LastTimeModified Then
            EmployeePage = New Employee
            LastTimeModified = latestModifiedTime
        End If
        frmMain.Navigate(EmployeePage)
    End Sub

    Private Sub MainWindow_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If User IsNot Nothing Then
            If DatabaseManager.Connection.State = System.Data.ConnectionState.Closed Then DatabaseManager.Connection.Open()
            LoggingService.LogAccess(DatabaseManager, monitoring_module.Logging.LoggingGateway.AccessChoices.LOGGED_OUT)
            DatabaseManager.Connection.Close()
        End If
    End Sub

    Private Sub btnReport_Click(sender As Object, e As RoutedEventArgs)
        frmMain.Navigate(New Report)
    End Sub

    Private Sub btnAlphalist_Click(sender As Object, e As RoutedEventArgs)
        frmMain.Navigate(New Alphalist)
    End Sub


    Private Sub dtPayrollDate_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        Try
            If sender IsNot Nothing Then
                If sender.name = "dtPayrollDate" Then
                    DefaultPayrollDate = e.AddedItems.Item(0)
                    frmMain.Refresh()
                ElseIf sender.name = "cbPayrollCode" Then
                    DefaultPayrollCode = e.AddedItems.Item(0)
                    frmMain.Refresh()
                End If
            End If
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
    End Sub


End Class
