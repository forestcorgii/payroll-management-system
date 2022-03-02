Public Class clsCompanyTemplate
    Public SiteIdx As Integer
    Public CompanyIdx As Integer

    Public RegisteredName As String
    Public RegionNumber As String
    Public TIN As String
    Public BranchCode As String

    Public Overrides Function ToString() As String
        Return String.Format("{0} - {1}-{2} - {3}", RegionNumber, TIN, BranchCode, RegisteredName)
    End Function
End Class
Public Class clsCompanyTemplates
    Public Items As New List(Of clsCompanyTemplate)
    Public DestinationFolder As String
End Class