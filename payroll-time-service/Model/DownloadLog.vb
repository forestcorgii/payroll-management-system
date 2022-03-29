Imports MySql.Data.MySqlClient

Namespace Model
    Public Class DownloadLog
        Public id As Integer

        Public Payroll_Date As Date
        Public Payroll_Code As String
        Public TotalPage As Integer
        Public Last_Page_Downloaded As Integer = 0

        Public Status As DownloadStatusChoices
        Public DateTimeCreated As Date
        Public ReadOnly Property Name As String
            Get
                Return Payroll_Date.ToString("yyyyMMdd")
            End Get
        End Property
        Public Overrides Function ToString() As String
            Return Name
        End Function
        Sub New()

        End Sub

        Sub New(reader As MySqlDataReader)
            id = reader.Item("id")
            Payroll_Date = reader.Item("payroll_date")
            Payroll_Code = reader.Item("payroll_code")
            TotalPage = reader.Item("total_page")
            Last_Page_Downloaded = reader.Item("last_page_downloaded")
            Status = reader.Item("status")
            DateTimeCreated = reader.Item("log_created")
        End Sub

        Public Enum DownloadStatusChoices
            PENDING
            DONE
        End Enum
    End Class

End Namespace
