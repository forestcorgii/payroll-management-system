Imports MySql.Data.MySqlClient
Imports utility_service

Namespace Payroll
    Public Class AdjustmentRecordGateway
        Public Shared Function Collect(databaseManager As Manager.Mysql) As List(Of AdjustmentRecordModel)
            Dim records As New List(Of AdjustmentRecordModel)
            Try
                Using reader As MySqlDataReader = databaseManager.ExecuteDataReader("SELECT * FROM payroll_db.adjustment_record_complete ORDER BY Date_modified DESC;")
                    While reader.Read
                        records.Add(New AdjustmentRecordModel(reader))
                    End While
                End Using
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try
            Return records
        End Function

        Public Shared Function Find(databaseManager As Manager.Mysql, record_name As String) As AdjustmentRecordModel
            Dim records As New AdjustmentRecordModel
            Try
                Using reader As MySqlDataReader = databaseManager.ExecuteDataReader(String.Format("SELECT * FROM payroll_db.adjustment_record_complete WHERE record_name='{0}' LIMIT 1;", record_name))
                    While reader.Read
                        records = New AdjustmentRecordModel(reader)
                    End While
                End Using
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try
            Return records
        End Function

        Public Shared Function Filter(databaseManager As Manager.Mysql, ee_id As String, Optional is_active As Boolean = Nothing) As List(Of AdjustmentRecordModel)
            Dim records As New List(Of AdjustmentRecordModel)
            Try
                'Dim conjunction
                Dim isActiveSnip As String = ""
                If is_active <> Nothing Then
                    isActiveSnip = String.Format("AND is_active={0}", is_active)
                End If

                Using reader As MySqlDataReader = databaseManager.ExecuteDataReader(String.Format("SELECT * FROM payroll_db.adjustment_record_complete WHERE ee_id='{0}' ORDER BY Date_modified DESC;", ee_id))
                    While reader.Read
                        records.Add(New AdjustmentRecordModel(reader))
                    End While
                End Using
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try
            Return records
        End Function

        Public Shared Function Filter(databaseManager As Manager.Mysql, filterString As String, payrollCode As String) As List(Of AdjustmentRecordModel)
            Dim records As New List(Of AdjustmentRecordModel)
            Try
                Using reader As MySqlDataReader = databaseManager.ExecuteDataReader(String.Format("SELECT * FROM payroll_db.adjustment_record_complete WHERE (ee_id LIKE '{0}%' OR last_name LIKE '{0}%' OR first_name LIKE '{0}%' OR middle_name LIKE '{0}%') AND payroll_code='{1}' ORDER BY Date_modified DESC;", filterString, payrollCode))
                    While reader.Read
                        records.Add(New AdjustmentRecordModel(reader))
                    End While
                End Using
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try
            Return records
        End Function


        Public Shared Function Save(databaseManager As Manager.Mysql, record As AdjustmentRecordModel)
            Try
                Dim command As New MySqlCommand("REPLACE INTO payroll_db.adjustment_record (record_name,ee_id,adjustment_name,date_effective,date_expires,monthly_deduction,total_advances,remaining_balance,adjustment_type,request_type)VALUES(?,?,?,?,?,?,?,?,?,?)", databaseManager.Connection)
                command.Parameters.AddWithValue("rec", record.GetRecordName)
                command.Parameters.AddWithValue("ee", record.EE_Id)
                command.Parameters.AddWithValue("nm", record.Adjustment_Name)
                command.Parameters.AddWithValue("eff", record.Date_Effective)
                command.Parameters.AddWithValue("ex", record.Date_Expires)
                command.Parameters.AddWithValue("ded", record.Monthly_Deduction)
                command.Parameters.AddWithValue("bal", record.Total_Advances)
                command.Parameters.AddWithValue("baal", record.Remaining_Balance)
                command.Parameters.AddWithValue("aty", record.Adjustment_Type)
                command.Parameters.AddWithValue("atyy", record.Request_Type)
                command.ExecuteNonQuery()
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try
            Return Find(databaseManager, record.Record_Name)
        End Function
    End Class
End Namespace
