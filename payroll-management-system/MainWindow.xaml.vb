Imports utility_service
Imports hrms_api_service
Imports Newtonsoft.Json
Imports System.ComponentModel

Class MainWindow
    Dim TimeDownloaderPage As TimeDownloaderPage

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
        Dim settings As Model.Settings = Controller.Settings.GetSettings(DatabaseManager, "HRMSAPIManager", "payroll_management")
        If settings IsNot Nothing Then
            HRMSAPIManager = JsonConvert.DeserializeObject(Of Manager.API.HRMS)(settings.JSON_Arguments)
        End If

        settings = Controller.Settings.GetSettings(DatabaseManager, "TimeDownloaderAPIManager", "payroll_management")
        If settings IsNot Nothing Then
            TimeDownloaderAPIManager = JsonConvert.DeserializeObject(Of time_module.Manager.API.TimeDownloader)(settings.JSON_Arguments)
        End If

        Return True
    End Function

    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        TimeDownloaderPage = New TimeDownloaderPage
        frmMain.Navigate(New EmployeePage)
    End Sub

    Private Sub btnTimeDownloader_Click(sender As Object, e As RoutedEventArgs)
        frmMain.Navigate(TimeDownloaderPage)
    End Sub

    Private Sub btnGenerateDBF_Click(sender As Object, e As RoutedEventArgs)
        frmMain.Navigate(New GenerateDBFPage)
    End Sub

    Private Sub btnSettings_Click(sender As Object, e As RoutedEventArgs)
        frmMain.Navigate(New Settings)
    End Sub

    Private LastTimeModified As Date
    Private EmployeePage As EmployeePage
    Private Sub btnEmployee_Click(sender As Object, e As RoutedEventArgs)
        DatabaseManager.Connection.Open()
        Dim latestModifiedTime As Date = employee_module.EmployeeGateway.TimeHasChange(DatabaseManager)
        DatabaseManager.Connection.Close()

        If latestModifiedTime > LastTimeModified Then
            EmployeePage = New EmployeePage
            LastTimeModified = latestModifiedTime
        End If
        frmMain.Navigate(EmployeePage)
    End Sub

    Private Sub btnProcessPayroll_Click(sender As Object, e As RoutedEventArgs)
        frmMain.Navigate(New ProcessPayreg)
    End Sub

    Private Sub MainWindow_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If User IsNot Nothing Then
            DatabaseManager.Connection.Open()
            LoggingService.LogAccess(DatabaseManager, monitoring_module.Logging.LoggingGateway.AccessChoices.LOGGED_OUT)
            DatabaseManager.Connection.Close()
        End If
    End Sub
End Class
