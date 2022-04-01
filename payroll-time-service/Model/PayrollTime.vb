Imports MySql.Data.MySqlClient
Imports Newtonsoft.Json

Namespace Model
    Public Class PayrollTime
        Public DATER As Integer
        Public CODE As Integer
        Public Payroll_Date As Date

        <JsonProperty("employee_id")>
        Public EE_Id As String
        Public Total_Hours As Double
        Public Total_OTs As Double
        Public Total_RD_OT As Double
        Public Total_H_OT As Double
        Public Total_ND As Double
        Public Total_Tardy As Double
        Public Allowance As Double
        Public Has_PCV As String
        Public ReadOnly Property Payroll_Name As String
            Get
                Return String.Format("{0}_{1:yyyyMMdd}", EE_Id, Payroll_Date)
            End Get
        End Property

        Sub New()

        End Sub
        Sub New(reader As MySqlDataReader)
            Payroll_Date = reader.Item("payroll_date")
            EE_Id = reader.Item("ee_id")
            Total_Hours = reader.Item("total_hours")
            Total_OTs = reader.Item("total_ots")
            Total_RD_OT = reader.Item("total_rd_ot")
            Total_H_OT = reader.Item("total_h_ot")
            Total_ND = reader.Item("total_nd")
            Total_Tardy = reader.Item("total_tardy")


            If IsDBNull(reader.Item("allowance")) = False Then
                Allowance = reader.Item("allowance")
            Else Allowance = 0
            End If
            'Has_PCV = reader.Item("has_pcv")
        End Sub

    End Class

End Namespace
