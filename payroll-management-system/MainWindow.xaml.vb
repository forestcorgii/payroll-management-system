Imports utility_service

Class MainWindow
    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        HRMSAPIManager = New Manager.API.HRMS("HRMS_API_URL")


        DatabaseConfiguration = New Configuration.MysqlConfiguration()
        DatabaseConfiguration.Setup("ACCOUNTING_DB_URL")

        DatabaseManager = New Manager.Mysql
        DatabaseManager.Connection = DatabaseConfiguration.ToMysqlConnection

        '   SetupUserAuthentication()
    End Sub
    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        frmMain.Navigate(New ProcessPayreg)
    End Sub
End Class
