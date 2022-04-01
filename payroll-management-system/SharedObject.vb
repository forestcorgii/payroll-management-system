Imports monitoring_module
Imports utility_service
Module SharedObject
    Public PageRedirecting As Boolean

    Public DatabaseConfiguration As Configuration.Mysql
    Public DatabaseManager As Manager.Mysql

    Public HRMSAPIManager As hrms_api_service.Manager.API.HRMS

    Public TimeDownloaderAPIManager As time_module.Manager.API.TimeDownloader

    Public LoggingService As Logging.LoggingService

    Public User As Authentication.User
    Public Function SetupUserAuthentication() As Boolean
        Dim password As String = Environment.GetEnvironmentVariable("ACCOUNTING_AUTH_TOKEN")
        Dim loginRequired As Boolean = True
        Dim authenticated As Boolean = False
        User = New Authentication.User
        User.Password = password

        DatabaseManager.Connection.Open()
        Try
            If password <> "" Then
                loginRequired = Not User.Login(DatabaseManager)
            End If

            If loginRequired Then
                Dim login As New Login
                If login.ShowDialog() Then
                    User.EE_Id = login.Username
                    User.Password = login.Password

                    authenticated = User.Login(login.Remember, DatabaseManager)
                End If
            End If
        Catch ex As Exception
            LoggingService.LogError(DatabaseManager, ex.Message, "SetupUserAuthentication")
        End Try

        If authenticated Or loginRequired = False Then
            LoggingService.AuthenticatedUser = User
            LoggingService.LogAccess(DatabaseManager, Logging.LoggingGateway.AccessChoices.LOGGED_IN)
        End If

        DatabaseManager.Connection.Close()

        Return authenticated Or loginRequired = False
    End Function
End Module
