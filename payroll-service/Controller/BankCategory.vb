﻿Imports System.Windows.Forms
Imports MySql.Data.MySqlClient
Imports utility_service

Namespace Controller
    Public Class BankCategory
        Public Shared Sub SaveBankCategory(databaseManager As Manager.Mysql, bankCategory As String, ee_id As Integer)
            Try
                Dim command As New MySqlCommand("INSERT INTO payroll_management.bank_category (ee_id, bank_category)VALUES(?,?)", databaseManager.Connection)
                command.Parameters.AddWithValue("p1", ee_id)
                command.Parameters.AddWithValue("p2", bankCategory)
                command.ExecuteNonQuery()
            Catch ex As Exception
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Public Shared Function GetHistory(databaseManager As Manager.Mysql, ee_id As Integer) As Model.BankCategoryHistory
            Dim bankCategoryHistory As New Model.BankCategoryHistory
            Try
                Using reader As MySqlDataReader = databaseManager.ExecuteDataReader(String.Format("SELECT * FROM payroll_management.bank_category WHERE ee_id={0} ORDER BY date_created DESC", ee_id))
                    While reader.Read
                        bankCategoryHistory.History.Add(New Model.BankCategory(reader))
                    End While
                End Using
            Catch ex As Exception
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
            Return bankCategoryHistory
        End Function
    End Class

End Namespace