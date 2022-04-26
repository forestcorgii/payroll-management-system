Imports employee_module
Imports MySql.Data.MySqlClient

Namespace Payroll
    Public Class PayrollModel

        Public Property Payroll_Date As Date

        Public Property EE_Id As String
        Public Property EE As EmployeeModel
        'Identifier
        Public Property Government As GovernmentModel

        Public AdjustmentLogs As List(Of AdjustmentBillingModel)

        Public Property Payroll_Code As String
        Public Property Bank_Category As String
        Public Property Bank_Name As String

        Public Property Gross_Pay As Double
        Public Property Reg_Pay As Double

        Public Property Net_Pay_Preview As Double
        Public Property Adjust1_Preview As Double
        Public Property Adjust2_Preview As Double

        Public ReadOnly Property Payroll_Name As String
            Get
                Return String.Format("{0}_{1:yyyyMMdd}", EE_Id, Payroll_Date)
            End Get
        End Property

        Public ReadOnly Property Adjust1 As Double
            Get
                Dim adj As Double = 0
                If AdjustmentLogs IsNot Nothing Then
                    For Each log As AdjustmentBillingModel In AdjustmentLogs
                        If log.Adjustment_Type = AdjustmentChoices.ADJUST1 Then
                            adj += log.Amount
                        End If
                    Next
                End If
                Return adj
            End Get
        End Property
        Public ReadOnly Property Adjust2 As Double
            Get
                Dim adj As Double = 0
                If AdjustmentLogs IsNot Nothing Then
                    For Each log As AdjustmentBillingModel In AdjustmentLogs
                        If log.Adjustment_Type = AdjustmentChoices.ADJUST2 Then
                            adj += log.Amount
                        End If
                    Next
                End If
                Return adj
            End Get
        End Property

        Public ReadOnly Property Net_Pay As Double
            Get
                Dim governmentDeduction As Double = 0
                If Government IsNot Nothing Then
                    governmentDeduction = Government.TotalDeduction
                End If
                Return Gross_Pay + Adjust1 + Adjust2 + governmentDeduction
            End Get
        End Property

        Sub New()

        End Sub
        Sub New(reader As MySqlDataReader)
            EE_Id = reader("ee_id")
            Payroll_Date = reader("payroll_date")
            Gross_Pay = reader("gross_pay")
            Reg_Pay = reader("reg_pay")
            Net_Pay_Preview = reader("net_pay")
            Adjust1_Preview = reader("adjust1")
            Adjust2_Preview = reader("adjust2")

            Payroll_Code = reader("payroll_code")
            Bank_Category = reader("bank_category")
            Bank_Name = reader("bank_name")
        End Sub

    End Class

End Namespace
