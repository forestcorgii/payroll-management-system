Imports MySql.Data.MySqlClient
Imports Newtonsoft.Json
Imports NPOI.SS.UserModel
Imports payroll_service.Model

Namespace Model
    Public Class PayrollTime
        Public Id As Integer
        Public DATER As Integer
        Public CODE As Integer
        Public Payroll_Date As Date

        'Public WriteOnly Property employee_id As String
        '    Set(value As String)
        '        EE_Id = value
        '    End Set
        'End Property

        <JsonProperty("employee_id")>
        Public EE_Id As String
        Public EE As Employee
        Public Total_Hours As Double
        Public Total_OTs As Double
        Public Total_RD_OT As Double
        Public Total_H_OT As Double
        Public Total_ND As Double
        Public Total_Tardy As Double
        Public Allowance As Double
        'Public Incentive As Double
        Public Has_PCV As String
        Public ReadOnly Property Payroll_Name As String
            Get
                Return String.Format("{0}_{1:yyyyMMdd}", EE_Id, Payroll_Date)
            End Get
        End Property

        Sub New()

        End Sub
        Sub New(reader As MySqlDataReader)
            Id = reader.Item("id")
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
            'Incentive = reader.Item("incentive")
            'Has_PCV = reader.Item("has_pcv")
        End Sub

        Public Function ToDBFRecordFormat() As String()
            Return {DATER, CODE, EE_Id, Total_Hours, Total_OTs, Total_RD_OT, 0, Total_H_OT, 0, Total_ND, Total_Tardy, Allowance, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        End Function
        Public Sub ToEERowFormat(row As IRow)
            row.CreateCell(1).SetCellValue(EE_Id)
            row.CreateCell(2).SetCellValue(EE.Fullname)
            row.CreateCell(3).SetCellValue(Total_Hours)
            row.CreateCell(4).SetCellValue(Total_OTs)
            row.CreateCell(5).SetCellValue(Total_RD_OT)
            row.CreateCell(6).SetCellValue(Total_H_OT)
            row.CreateCell(7).SetCellValue(Total_ND)
            row.CreateCell(8).SetCellValue(Total_Tardy)
        End Sub
    End Class

End Namespace
