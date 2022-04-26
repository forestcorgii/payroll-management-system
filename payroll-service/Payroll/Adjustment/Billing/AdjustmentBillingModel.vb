Imports MySql.Data.MySqlClient

Namespace Payroll

    Public Class AdjustmentBillingModel
        Public Property Billing_Name As String
        Public ReadOnly Property GetBilling_Name As String
            Get
                Return String.Format("{0}_{1}", Adjustment_Name, Payroll_Name)
            End Get
        End Property

        Public Property Adjustment_Name As String
        Public Property Record_Name As String
        Public Property Record As AdjustmentRecordModel

        Public Property EE_Id As String
        Public Property EE As employee_module.EmployeeModel

        Public Property Payroll_Name As String
        Public Property Payroll As PayrollModel

        Public Property Amount As Double
        Public Property Remarks As String
        Public Property Deduct As Boolean

        Public Property Adjustment_Type As AdjustmentChoices

        Public Property Date_Created As Date

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

            Payroll = New PayrollModel
            Payroll.Payroll_Date = reader("payroll_date")

            Billing_Name = reader("billing_name")
            Adjustment_Name = reader("adjustment_name")
            Record_Name = reader("record_name")
            Payroll_Name = reader("payroll_name")
            Amount = reader("amount")
            Adjustment_Type = reader("adjustment_type")
            Remarks = reader("remarks")
            Deduct = reader("deduct")
            Date_Created = reader("date_created")
        End Sub

    End Class

End Namespace

