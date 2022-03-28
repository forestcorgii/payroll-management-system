Imports utility_service
Imports hrms_api_service
Imports Newtonsoft.Json

Class MainWindow
    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        HRMSAPIManager = New Manager.API.HRMS()

        SetupConfiguration()

        '   SetupUserAuthentication()
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
            HRMSAPIManager = JsonConvert.DeserializeObject(Of hrms_api_service.Manager.API.HRMS)(settings.JSON_Arguments)
        End If

        settings = Controller.Settings.GetSettings(DatabaseManager, "TimeDownloaderAPIManager", "payroll_management")
        If settings IsNot Nothing Then
            TimeDownloaderAPIManager = JsonConvert.DeserializeObject(Of payroll_time_service.Manager.API.TimeDownloader)(settings.JSON_Arguments)
        End If

        Return True
    End Function
    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        frmMain.Navigate(New ProcessPayreg)
    End Sub

    Private Sub btnTimeDownloader_Click(sender As Object, e As RoutedEventArgs)
        Dim da As New TimeDownloader
        da.Show()

    End Sub

    Private Sub btnGenerateDBF_Click(sender As Object, e As RoutedEventArgs)
        'Dim da As New GenerateDBF
        'da.ShowDialog()
        frmMain.Navigate(New GenerateDBFPage)

    End Sub

    Private Sub btnSettings_Click(sender As Object, e As RoutedEventArgs)
        'Dim da As New Settings
        'da.ShowDialog()

        frmMain.Navigate(New Settings)
    End Sub
End Class
