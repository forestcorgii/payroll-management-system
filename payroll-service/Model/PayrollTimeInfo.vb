Namespace Model
    Public Class PayrollTimeInfo
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
        Public Timesheet_Guide_Remarks As String

        Public Function ToDBFRecordFormat() As String()
            Return {DATER, CODER, EE.Employee_Id, Total_Hours, Total_OTs, Total_RD_OT, 0, Total_H_OT, 0, Total_ND, Total_Tardy, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        End Function
    End Class

End Namespace
