Imports MySql.Data.MySqlClient
Imports Newtonsoft.Json

Namespace Model
    Public Class PayrollTime
        Public DATER As Integer
        Public CODE As Integer
        Public Payroll_Date As Date

        <JsonProperty("employee_id")>
        Public Property EE_Id As String
        Public Property Total_Hours As Double
        Public Property Total_OTs As Double
        Public Property Total_RD_OT As Double
        Public Property Total_H_OT As Double
        Public Property Total_ND As Double
        Public Property Total_Tardy As Double
        Public Property Allowance As Double
        Public Property Has_PCV As String
        Public Property Is_Confirmed As Boolean
        Public Property Page As Integer
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
            Is_Confirmed = reader.Item("is_confirmed")
            Page = reader.Item("page")


            If IsDBNull(reader.Item("allowance")) = False Then
                Allowance = reader.Item("allowance")
            Else Allowance = 0
            End If
            'Has_PCV = reader.Item("has_pcv")
        End Sub

    End Class

End Namespace
