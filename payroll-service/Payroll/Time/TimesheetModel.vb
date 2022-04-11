Imports employee_module
Imports MySql.Data.MySqlClient

Namespace Payroll

    Public Class TimesheetModel
        Inherits time_module.Model.PayrollTime
        Public Property EE As EmployeeModel
        Public Property Remarks As String
        Public ReadOnly Property Evaluation As String
            Get
                Dim eval As String = Remarks
                If Is_Confirmed = False Then eval &= "NOT CONFIRMED; "
                If Total_Hours = 0 Then eval &= "ZERO TOTAL HOURS; "
                Return eval
            End Get
        End Property
        Sub New()
            EE = New EmployeeModel
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
            Is_Confirmed = reader("is_confirmed")
        End Sub


    End Class

End Namespace
