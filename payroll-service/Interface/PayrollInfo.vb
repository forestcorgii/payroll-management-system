Public Class PayrollInfo
    Public Property PayrollDate As String

    Public Property Employee_Id As String
    Public Property Fullname As String

    Public Property REG_HRS As Double
    Public Property ADJUST1 As Double
    Public Property GROSS_PAY As Double
    Public Property ADJUST2 As Double
    Public Property WITHHOLDING_TAX As Double
    Public Property Pagibig_EE As Double
    Public Property Pagibig_ER As Double
    Public Property SSS_EE As Double
    Public Property SSS_ER As Double
    Public Property PHIC As Double

    Public ReadOnly Property SSS_PHIC_EE As Double
        Get
            Return SSS_EE + PHIC
        End Get
    End Property
    Public ReadOnly Property SSS_PHIC_ER As Double
        Get
            Return SSS_ER + PHIC
        End Get
    End Property
    Public Property NET_PAY As Double
End Class
