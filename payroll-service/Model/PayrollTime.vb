Imports MySql.Data.MySqlClient

Namespace Model
    Public Class PayrollTime
        Public Id As Integer
        Public DATER As Integer
        Public CODER As Integer
        Public Payroll_Date As Date
        Public EE_Id As Integer
        Public EE As Employee
        Public Location As String
        Public Job_Title As String
        Public Payroll_Code As String
        Public Bank_Category As String
        Public Total_Hours As Double
        Public Total_OTs As Double
        Public Total_RD_OT As Double
        Public Total_H_OT As Double
        Public Total_ND As Double
        Public Total_Tardy As Double
        Public Allowance As Double
        Public Incentive As Double
        Public Has_PCV As String
        Public Timesheet_Guide As String

        Sub New()

        End Sub
        Sub New(reader As MySqlDataReader)
            Id = reader.Item("id")
            Payroll_Date = reader.Item("payroll_date")
            EE_Id = reader.Item("ee_id")
            Location = reader.Item("location")
            Job_Title = reader.Item("job_title")
            Payroll_Code = reader.Item("payroll_code")
            Bank_Category = reader.Item("bank_category")
            Total_Hours = reader.Item("total_hours")
            Total_OTs = reader.Item("total_ots")
            Total_RD_OT = reader.Item("total_rd_ot")
            Total_H_OT = reader.Item("total_h_ot")
            Total_ND = reader.Item("total_nd")
            Total_Tardy = reader.Item("total_tardy")
            Allowance = reader.Item("allowance")
            Incentive = reader.Item("incentive")
            Has_PCV = reader.Item("has_pcv")
            Timesheet_Guide = reader.Item("timesheet_guide_remarks")
        End Sub


        Public Function ToDBFRecordFormat() As String()
            Return {DATER, CODER, EE.Employee_Id, Total_Hours, Total_OTs, Total_RD_OT, 0, Total_H_OT, 0, Total_ND, Total_Tardy, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        End Function
    End Class

End Namespace
