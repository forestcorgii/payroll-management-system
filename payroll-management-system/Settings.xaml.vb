Imports Newtonsoft.Json
Imports utility_service
Public Class Settings
    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        If HRMSAPIManager IsNot Nothing Then
            tbHRMSUrl.Text = HRMSAPIManager.Url
            tbHRMSToken.Text = HRMSAPIManager.APIToken
            tbHRMSWhat.Text = HRMSAPIManager.What
            tbHRMSSearch.Text = HRMSAPIManager.Search
            tbHRMSField.Text = HRMSAPIManager.Field
        Else : HRMSAPIManager = New hrms_api_service.Manager.API.HRMS
        End If

        If TimeDownloaderAPIManager IsNot Nothing Then
            With TimeDownloaderAPIManager
                tbTimeDownloaderUrl.Text = .Url
                tbTimeDownloaderInfo.Text = .info
                tbTimeDownloaderToken.Text = .api_token
            End With
        Else
            TimeDownloaderAPIManager = New time_module.Manager.API.TimeDownloader
        End If
    End Sub

    Private Sub Settings_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded

    End Sub

    Private Sub btnTimeDownloaderSave_Click(sender As Object, e As RoutedEventArgs)
        TimeDownloaderAPIManager.Url = tbTimeDownloaderUrl.Text
        TimeDownloaderAPIManager.info = tbTimeDownloaderInfo.Text
        TimeDownloaderAPIManager.api_token = tbTimeDownloaderToken.Text
        Dim Settings As Model.Settings = New Model.Settings() With {
                .Name = "TimeDownloaderAPIManager",
                .JSON_Arguments = JsonConvert.SerializeObject(TimeDownloaderAPIManager)
            }
        DatabaseManager.Connection.Open()
        Controller.Settings.SaveSettings(DatabaseManager, Settings, "payroll_management")
        DatabaseManager.Connection.Close()
    End Sub

    Private Sub btnHRMSSave_Click(sender As Object, e As RoutedEventArgs)
        HRMSAPIManager.Url = tbHRMSUrl.Text
        HRMSAPIManager.What = tbHRMSWhat.Text
        HRMSAPIManager.Field = tbHRMSField.Text
        HRMSAPIManager.Search = tbHRMSSearch.Text
        HRMSAPIManager.APIToken = tbHRMSToken.Text
        Dim Settings As Model.Settings = New Model.Settings() With {
                .Name = "HRMSAPIManager",
                .JSON_Arguments = JsonConvert.SerializeObject(HRMSAPIManager)
            }
        DatabaseManager.Connection.Open()
        Controller.Settings.SaveSettings(DatabaseManager, Settings, "payroll_management")
        DatabaseManager.Connection.Close()
    End Sub
End Class
