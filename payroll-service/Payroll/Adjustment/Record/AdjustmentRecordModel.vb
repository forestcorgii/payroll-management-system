Imports MySql.Data.MySqlClient

Namespace Payroll

    Public Class AdjustmentRecordModel
        Public Property Record_Name As String

        Public Property EE_Id As String
        Public Property EE As employee_module.EmployeeModel

        Public Property Adjustment_Name As String
        Public Property Request_Type As RequestTypeChoices

        Public Property Date_Expires As Date

        Public Property Total_Advances As Double
        Public Property Date_Effective As Date
        Public Property Monthly_Deduction As Double
        Public Property Remaining_Balance As Double


        Public Property Adjustment_Type As AdjustmentChoices

        Public Date_Created As Date
        Public Date_Modified As Date

        Sub New()
        End Sub

        Sub New(reader As MySqlDataReader)
            EE_Id = reader("ee_id")
            EE = New employee_module.EmployeeModel
            EE.EE_Id = EE_Id
            EE.Payroll_Code = reader("payroll_code")
            EE.First_Name = reader("first_name")
            EE.Last_Name = reader("last_name")
            EE.Middle_Name = reader("middle_name")

            Record_Name = reader("record_name")

            Adjustment_Name = reader("adjustment_name")
            Date_Effective = reader("date_effective")
            Date_Expires = reader("date_expires")
            Monthly_Deduction = reader("monthly_deduction")
            Total_Advances = reader("total_advances")
            Remaining_Balance = reader("remaining_balance")

            Adjustment_Type = reader("adjustment_type")
            Request_Type = reader("request_type")

            Date_Created = reader("date_created")
            Date_Modified = reader("date_modified")
        End Sub

        Public ReadOnly Property GetRecordName As String
            Get
                Return String.Format("{0}_{1}_{2:yyMM}", EE_Id, Adjustment_Name, Date_Effective)
            End Get
        End Property
    End Class

    Public Enum AdjustmentChoices
        ADJUST1 = 1
        ADJUST2 = 2
    End Enum

    Public Enum RequestTypeChoices
        NORMAL = 0
        ONLY_30TH = 1
        BY_REQUEST = 2
    End Enum


End Namespace
