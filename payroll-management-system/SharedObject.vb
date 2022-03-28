Imports monitoring_service
Imports utility_service
Module SharedObject
    Public PageRedirecting As Boolean

    Public DatabaseConfiguration As Configuration.Mysql
    Public DatabaseManager As Manager.Mysql

    Public HRMSAPIManager As hrms_api_service.Manager.API.HRMS

    Public TimeDownloaderAPIManager As payroll_time_service.Manager.API.TimeDownloader

    Public User As monitoring_service.Model.User
    Public Function SetupUserAuthentication() As Boolean
        Dim password As String = Environment.GetEnvironmentVariable("PAYABLE_SYSTEM_AUTH_TOKEN")
        Dim loginRequired As Boolean = True

        User = New monitoring_service.Model.User
        User.Password = password

        If password <> "" Then
            loginRequired = Not User.Login(DatabaseManager)
        End If

        If loginRequired Then
            Dim login As New Login
            If login.ShowDialog() Then
                User.Username = login.Username
                User.Password = login.Password

                Return User.Login(login.Remember, DatabaseManager)
            End If
        End If
        Return Not loginRequired
    End Function
End Module
