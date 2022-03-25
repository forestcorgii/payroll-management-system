Imports System.Net.Http
Imports Newtonsoft.Json

Namespace Manager
    Namespace API
        Public Class TimeDownloader
            Private Client As New HttpClient

            Public info As String = "getTimeLogs"
            Public api_token As String = "40jhwWlphorjv40"

            Public Class PostData
                Public info As String
                Public api_token As String
                Public date_from As String
                Public date_to As String
                Public page As String
            End Class

            Public Url As String

            Sub New()

                Client = New HttpClient
                Client.Timeout = TimeSpan.FromSeconds(10)
            End Sub


            Public Async Function GetTotalPage(date_from As Date, date_to As Date) As Task
                Try
                    Dim postData As New PostData
                    postData.info = info
                    postData.api_token = api_token

                    postData.page = 0
                    postData.date_from = date_from
                    postData.date_to = date_to

                    Dim dicc As New Dictionary(Of String, String)
                    dicc.Add("postData", JsonConvert.SerializeObject(postData))

                    Dim content As New FormUrlEncodedContent(dicc)

                    Dim response As HttpResponseMessage = Await Client.PostAsync(String.Format("{0}", Url), content)

                    Dim responseString As String = Await response.Content.ReadAsStringAsync()

                Catch ex As Exception
                    Console.WriteLine(ex.Message)
                    'MessageBox.Show(ex.Message, "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End Function

            Public Async Function GetPageContent(date_from As Date, date_to As Date, page As Integer) As Task
                Try
                    Dim postData As New PostData
                    postData.info = info
                    postData.api_token = api_token

                    postData.page = 0
                    postData.date_from = date_from
                    postData.date_to = date_to

                    Dim dicc As New Dictionary(Of String, String)
                    dicc.Add("postData", JsonConvert.SerializeObject(postData))

                    Dim content As New FormUrlEncodedContent(dicc)

                    Dim response As HttpResponseMessage = Await Client.PostAsync(String.Format("{0}", Url), content)
                    Dim responseString As String = Await response.Content.ReadAsStringAsync()


                Catch ex As Exception
                    Console.WriteLine(ex.Message)
                    'MessageBox.Show(ex.Message, "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End Function

        End Class

    End Namespace
End Namespace
