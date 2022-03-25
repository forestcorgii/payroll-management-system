Imports utility_service
Imports hrms_api_service
Imports Newtonsoft.Json

Class MainWindow
    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        HRMSAPIManager = New Manager.API.HRMS()


        DatabaseConfiguration = New Configuration.Mysql()
        DatabaseConfiguration.Setup("ACCOUNTING_DB_URL")

        DatabaseManager = New Manager.Mysql
        DatabaseManager.Connection = DatabaseConfiguration.ToMysqlConnection

        '   SetupUserAuthentication()
    End Sub


    Private Sub SetupConfiguration()
        DatabaseConfiguration = New Configuration.Mysql()
        DatabaseConfiguration.Setup("TIME_RECORDER_DB_URL_2")

        DatabaseManager = New Manager.Mysql(DatabaseConfiguration)
        DatabaseManager.Connection.Open()
        SetupManager()
        DatabaseManager.Connection.Close()
    End Sub
    Private Function SetupManager()

        Dim settings = Controller.Settings.GetSettings(DatabaseManager, "HRMSAPIManager")
        If settings IsNot Nothing Then
            HRMSAPIManager = JsonConvert.DeserializeObject(Of hrms_api_service.Manager.API.HRMS)(settings.JSON_Arguments)
        End If

        Return True
    End Function
    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Dim da As New GenerateDBF
        da.ShowDialog()
        frmMain.Navigate(New ProcessPayreg)
    End Sub
End Class
