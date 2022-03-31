Imports MySql.Data.MySqlClient
Imports Newtonsoft.Json
Imports NPOI.SS.UserModel
Imports payroll
Namespace Payroll

    Namespace Time
        Public Class Model
            Inherits payroll_time_service.Model.PayrollTime
            Public EE As Employee.Model

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
End Namespace
