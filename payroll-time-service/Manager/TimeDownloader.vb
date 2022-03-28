Imports System.Net.Http
Imports Newtonsoft.Json

Namespace Manager
    Namespace API
        Public Class TimeDownloader
            Private Client As New HttpClient

            Public info As String = "getTimeLogs"
            Public api_token As String = "40jhwWlphorjv40"
            Public Url As String

            Public Class PostData
                Public info As String
                Public api_token As String
                Public date_from As String
                Public date_to As String
                Public page As String
            End Class


            Sub New()

                Client = New HttpClient
                Client.Timeout = TimeSpan.FromSeconds(30)
            End Sub

            ''' <summary>
            ''' 
            ''' </summary>
            ''' <param name="date_from"></param>
            ''' <param name="date_to"></param>
            ''' <returns>Total Page</returns>
            Public Async Function GetTotalPage(date_from As Date, date_to As Date) As Task(Of Integer)
                Try
                    Dim postData As New PostData
                    postData.info = info
                    postData.api_token = api_token

                    postData.page = -1
                    postData.date_from = date_from.ToString("yyyy-MM-dd")
                    postData.date_to = date_to.ToString("yyyy-MM-dd")

                    Dim dicc As New Dictionary(Of String, String)
                    dicc.Add("postData", JsonConvert.SerializeObject(postData))

                    Dim content As New FormUrlEncodedContent(dicc)

                    Dim responseMessage As HttpResponseMessage = Await Client.PostAsync(String.Format("{0}", Url), content)

                    Dim responseMessageContent As String = Await responseMessage.Content.ReadAsStringAsync()

                    Dim responseDeserialized As resp = JsonConvert.DeserializeObject(Of resp)(responseMessageContent)

                    Return responseDeserialized.totalPage
                Catch ex As Exception
                    Console.WriteLine(ex.Message)
                    'MessageBox.Show(ex.Message, "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
                Return Nothing
            End Function

            Public Async Function GetPageContent(date_from As Date, date_to As Date, page As Integer) As Task(Of Model.PayrollTime())
                Try
                    Dim postData As New PostData
                    postData.info = info
                    postData.api_token = api_token

                    postData.page = page
                    postData.date_from = date_from.ToString("yyyy-MM-dd")
                    postData.date_to = date_to.ToString("yyyy-MM-dd")

                    Dim dicc As New Dictionary(Of String, String)
                    dicc.Add("postData", JsonConvert.SerializeObject(postData))

                    Dim content As New FormUrlEncodedContent(dicc)

                    Dim responseMessage As HttpResponseMessage = Await Client.PostAsync(String.Format("{0}", Url), content)
                    Dim responseMessageContent As String = Await responseMessage.Content.ReadAsStringAsync()
                    Dim responseDeserialized As resp = JsonConvert.DeserializeObject(Of resp)(responseMessageContent)

                    Return responseDeserialized.message
                Catch ex As Exception
                    Console.WriteLine(ex.Message)
                    'MessageBox.Show(ex.Message, "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
                Return Nothing
            End Function

            Public Class resp
                Public status As String
                Public totalPage As String
                Public message As Model.PayrollTime()
            End Class

        End Class

    End Namespace
End Namespace
