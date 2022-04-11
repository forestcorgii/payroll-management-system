Imports MySql.Data.MySqlClient
Imports utility_service

Namespace Payroll
    Public Class AdjustmentRecordGateway
        Public Shared Function Collect(databaseManager As Manager.Mysql) As List(Of AdjustmentRecordModel)
            Dim records As New List(Of AdjustmentRecordModel)
            Try
                Using reader As MySqlDataReader = databaseManager.ExecuteDataReader("SELECT * FROM payroll_db.adjustment_record ORDER BY Date_modified DESC;")
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
                Using reader As MySqlDataReader = databaseManager.ExecuteDataReader(String.Format("SELECT * FROM payroll_db.adjustment_record WHERE record_name='{0}' LIMIT 1;", record_name))
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
                Dim isActiveSnip As String = ""
                If is_active <> Nothing Then
                    isActiveSnip = String.Format("AND is_active={0}", is_active)
                End If

                Using reader As MySqlDataReader = databaseManager.ExecuteDataReader(String.Format("SELECT * FROM payroll_db.adjustment_record WHERE ee_id='{0}' {1} ORDER BY Date_modified DESC;", ee_id, isActiveSnip))
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
                Dim command As New MySqlCommand("INSERT INTO payroll_db.adjustment_record (record_name,ee_id,name,date_effective,date_expires,monthly_deduction,total_balance,adjust_type)VALUES(?,?,?,?,?,?,?,?)", databaseManager.Connection)
                command.Parameters.AddWithValue("rec", record.GetRecordName)
                command.Parameters.AddWithValue("ee", record.EE_Id)
                command.Parameters.AddWithValue("nm", record.Name)
                command.Parameters.AddWithValue("eff", record.Date_Effective)
                command.Parameters.AddWithValue("ex", record.Date_Expires)
                command.Parameters.AddWithValue("ded", record.Monthly_Deduction)
                command.Parameters.AddWithValue("bal", record.Total_Balance)
                command.Parameters.AddWithValue("aty", record.Adjust_Type)
                command.ExecuteNonQuery()
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try
            Return Find(databaseManager, record.Record_Name)
        End Function
    End Class
End Namespace
