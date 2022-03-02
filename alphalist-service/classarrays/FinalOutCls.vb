Public Class FinalOutCls
    Public ID As String = Nothing
    Public FirstName As String = ""
    Public MiddlesName As String = ""
    Public LastName As String = ""
    Public DateFrom As String = Nothing
    Public DateEnd As String = Nothing
    Public Tin As String = Nothing
    Public company As String = Nothing

    Public Rate As Double = 0
    Public GrossComp As Double = 0
    Public Nontax13th As Double = 0
    Public Benefits As Double = 0
    Public TotalIncome As Double = 0
    Public TaxDue As Double = 0
    Public JanNov As Double = 0
    Public Advances As Double = 0
    Public Refund As Double = 0
    Public WithHeld As Double = 0
    Public December As Double = 0
    Public final As Double = 0

    Public perday As Double = 0
    Public permonth As Double = 0
    Public peryear As Double = 0
    Public holiday As Double = 0
    Public Overtime As Double = 0
    Public RD_OT As Double = 0
    Public NDiff As Double = 0
    Public NonTaxSalary As Double = 0

    Public ReadOnly Property Fullname As String
        Get
            Return String.Format("{0} {1} {2}", FirstName, LastName, MiddlesName)
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return ID
    End Function


End Class
