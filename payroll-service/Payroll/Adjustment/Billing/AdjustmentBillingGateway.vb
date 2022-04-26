Imports MySql.Data.MySqlClient
Imports utility_service

Namespace Payroll
    Public Class AdjustmentBillingGateway

        Public Shared Function Find(databaseManager As Manager.Mysql, billing_name As String) As AdjustmentBillingModel
            Dim billing As New AdjustmentBillingModel
            Try
                Using reader As MySqlDataReader = databaseManager.ExecuteDataReader(String.Format("SELECT * FROM payroll_db.adjustment_billing_complete WHERE billing_name='{0}' LIMIT 1", billing_name))
                    While reader.Read
                        billing = New AdjustmentBillingModel(reader)
                    End While
                End Using
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Return billing
        End Function

        Public Shared Function Collect(databaseManager As Manager.Mysql) As List(Of AdjustmentBillingModel)
            Dim billings As New List(Of AdjustmentBillingModel)
            Try
                Using reader As MySqlDataReader = databaseManager.ExecuteDataReader("SELECT * FROM payroll_db.adjustment_billing_complete;")
                    While reader.Read
                        billings.Add(New AdjustmentBillingModel(reader))
                    End While
                End Using
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Return billings
        End Function
        Public Shared Function Collect(databaseManager As Manager.Mysql, payroll_name As String) As List(Of AdjustmentBillingModel)
            Dim billings As New List(Of AdjustmentBillingModel)
            Try
                Using reader As MySqlDataReader = databaseManager.ExecuteDataReader(String.Format("SELECT * FROM payroll_db.adjustment_billing_complete WHERE payroll_name='{0}'", payroll_name))
                    While reader.Read
                        billings.Add(New AdjustmentBillingModel(reader))
                    End While
                End Using
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Return billings
        End Function

        Public Shared Function Filter(databaseManager As Manager.Mysql, filterString As String, payrollCode As String, payrollDate As Date) As List(Of AdjustmentBillingModel)
            Dim billings As New List(Of AdjustmentBillingModel)
            Try
                Using reader As MySqlDataReader = databaseManager.ExecuteDataReader(String.Format("SELECT * FROM payroll_db.adjustment_billing_complete WHERE (ee_id LIKE '{0}%' OR last_name LIKE '{0}%' OR first_name LIKE '{0}%' OR middle_name LIKE '{0}%') AND payroll_code LIKE '{1}' AND payroll_date LIKE '{2:yyyy-MM-dd}' ORDER BY date_created DESC;", filterString, payrollCode, payrollDate))
                    While reader.Read
                        billings.Add(New AdjustmentBillingModel(reader))
                    End While
                End Using
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try
            Return billings
        End Function


        Public Shared Function Save(databaseManager As Manager.Mysql, adjustmentBillng As AdjustmentBillingModel) As AdjustmentBillingModel
            Try
                Dim command As New MySqlCommand("REPLACE INTO payroll_db.adjustment_billing (billing_name,adjustment_name,ee_id,payroll_name,amount,adjustment_type)VALUES(?,?,?,?,?,?)", databaseManager.Connection)
                command.Parameters.AddWithValue("p1", adjustmentBillng.GetBilling_Name)
                command.Parameters.AddWithValue("p1b", adjustmentBillng.Adjustment_Name)
                command.Parameters.AddWithValue("p2", adjustmentBillng.ee_id)
                command.Parameters.AddWithValue("p3", adjustmentBillng.Payroll_Name)
                command.Parameters.AddWithValue("p4", adjustmentBillng.Amount)
                command.Parameters.AddWithValue("pas", adjustmentBillng.Adjustment_Type)
                command.ExecuteNonQuery()
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try
            Return find(databaseManager, adjustmentBillng.GetBilling_Name)
        End Function
    End Class
End Namespace
