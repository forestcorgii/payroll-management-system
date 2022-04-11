Imports MySql.Data.MySqlClient
Imports utility_service

Namespace Payroll
    Public Class AdjustmentRecordController
        Public Shared Function GenerateBillings(databaseManager As Manager.Mysql, EE_Id As String, payroll_name As String)
            Dim billings As New List(Of AdjustmentBillingModel)
            Try
                Dim activeRecords As List(Of AdjustmentRecordModel) = AdjustmentRecordGateway.Filter(databaseManager, EE_Id, True)
                If activeRecords.Count > 0 Then
                    For Each record As AdjustmentRecordModel In activeRecords
                        Dim newBilling As New AdjustmentBillingModel
                        newBilling.ee_id = EE_Id
                        newBilling.Name = record.Name
                        newBilling.Record_Name = record.Record_Name
                        newBilling.Payroll_Name = payroll_name

                        newBilling.Amount = record.Monthly_Deduction
                        newBilling.Adjust_Type = AdjustmentChoices.ADJUST2

                        billings.Add(newBilling)
                    Next
                End If
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Return billings
        End Function
    End Class
End Namespace
