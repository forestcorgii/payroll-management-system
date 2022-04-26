Imports System.Net.Http
Imports Newtonsoft.Json
Imports time_module.Model

Namespace Manager
    Namespace API
        Public Class TimeDownloader
            Private Client As New HttpClient

            Public info As String
            Public api_token As String
            Public Url As String

            Public Class PostData
                Public info As String
                Public api_token As String
                Public date_from As String
                Public date_to As String
                Public page As String
                Public payroll_code As String
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
            Public Async Function GetSummary(date_from As Date, date_to As Date, payroll_code As String) As Task(Of TimeResponseData)
                While True
                    Try
                        Dim postData As New PostData
                        postData.info = info
                        postData.api_token = api_token

                        postData.payroll_code = payroll_code
                        postData.page = -1
                        postData.date_from = date_from.ToString("yyyy-MM-dd")
                        postData.date_to = date_to.ToString("yyyy-MM-dd")

                        Dim dicc As New Dictionary(Of String, String)
                        dicc.Add("postData", JsonConvert.SerializeObject(postData))

                        Dim content As New FormUrlEncodedContent(dicc)

                        Dim responseMessage As HttpResponseMessage = Await Client.PostAsync(String.Format("{0}", Url), content)
                        Dim responseMessageContent As String = Await responseMessage.Content.ReadAsStringAsync()
                        Dim responseDeserialized As TimeResponseData = JsonConvert.DeserializeObject(Of TimeResponseData)(responseMessageContent)

                        Return responseDeserialized
                    Catch ex As Exception
                        Console.WriteLine(ex.Message)
                        'MessageBox.Show(ex.Message, "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                End While
                Return Nothing
            End Function

            Public Async Function GetPageContent(date_from As Date, date_to As Date, page As Integer, payroll_code As String) As Task(Of Model.PayrollTime())
                Try
                    Dim postData As New PostData
                    postData.info = info
                    postData.api_token = api_token

                    postData.payroll_code = payroll_code
                    postData.page = page
                    postData.date_from = date_from.ToString("yyyy-MM-dd")
                    postData.date_to = date_to.ToString("yyyy-MM-dd")

                    Dim dicc As New Dictionary(Of String, String)
                    dicc.Add("postData", JsonConvert.SerializeObject(postData))

                    Dim content As New FormUrlEncodedContent(dicc)

                    Dim responseMessage As HttpResponseMessage = Await Client.PostAsync(String.Format("{0}", Url), content)
                    Dim responseMessageContent As String = Await responseMessage.Content.ReadAsStringAsync()
                    Dim responseDeserialized As TimeResponseData = JsonConvert.DeserializeObject(Of TimeResponseData)(responseMessageContent)

                    Return responseDeserialized.message
                Catch ex As Exception
                    Console.WriteLine(ex.Message)
                    'MessageBox.Show(ex.Message, "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
                Return Nothing
            End Function

        End Class

    End Namespace
End Namespace
