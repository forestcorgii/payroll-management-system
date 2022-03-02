Public Class EmployeeList
    Public id As String = Nothing
    Public name As String = Nothing
    Public Code As String = Nothing
    Public Reg_Hrs As New Double
    Public R_OT As New Double
    Public RD_8 As New Double
    Public RD_OT As New Double
    Public HOL_OT As New Double
    Public HOL_OT8 As New Double
    Public ND As New Double
    Public ADJUST1 As New Double
    Public GROSS_PAY As New Double
    Public ADJUST2 As New Double
    Public TAX As New Double
    Public SSS_EE As New Double
    Public SSS_ER As New Double
    Public PHIC As New Double
    Public NETPAY As New Double
    Public REG_PAY As New Double
    Public RATE As New Double
    Public STATUS As String = Nothing
    Public STARTDATE As String = Nothing
    Public ENDDATE As String = Nothing
    Public NONTAX13TH As New Double
    Public JANNOV As New Double
    Public DECEMBER As New Double
    Public EXT As String = Nothing
    Public TIN As String = Nothing
    Public MIDDLENAME As String = Nothing

    'KS
    Public KSSTARTDATE As String = Nothing
    Public KSENDDATE As String = Nothing
    Public KSID As String = Nothing
    Public KSRECS1 As New Double
    Public KSKS1 As New Double
    Public KSHOURS1 As New Double
    Public KSGROSSPAY As New Double
    Public KSND1 As New Double
    Public KSNETPAY1 As New Double
    Public KSRECS2 As New Double
    Public KSKS2 As New Double
    Public KSHOURS2 As New Double
    Public KSND2 As New Double
    Public KSNETPAY2 As New Double
    Public KSPAGIBIGEE As New Double
    Public KSPAGIBIGER As New Double
    Public KSSSSEE As New Double
    Public KSSSSER As New Double
    Public KSNONTAX13TH As New Double
    Public KSRATE As New Double
    Public KSJANNOV As New Double
    Public KSDECEMBER As New Double

    Public Overrides Function ToString() As String
        Return id & " - " & name
    End Function
End Class
