﻿Imports MySql.Data.MySqlClient
Imports utility_service

Namespace Controller
    Public Class Employee
        Public Shared Function GetEmployees(databaseManager As Manager.Mysql, Optional id As Integer = 0, Optional employee_id As String = "") As Model.Employee
            Dim employee As Model.Employee = Nothing
            Using reader As MySqlDataReader = databaseManager.ExecuteDataReader(
                String.Format("SELECT * FROM payroll_management.employee where id={0} or employee_id='{1}' LIMIT 1;", id, employee_id))
                If reader.HasRows Then
                    reader.Read()
                    employee = New Model.Employee(reader)
                End If
            End Using

            Return employee
        End Function

        Public Shared Function LoadEmployees(databaseManager As Manager.Mysql, Optional location As String = "", Optional first_name As String = "", Optional last_name As String = "", Optional middle_name As String = "", Optional job_title As String = "") As List(Of Model.Employee)
            Dim employees As List(Of Model.Employee) = Nothing
            Using reader As MySqlDataReader = databaseManager.ExecuteDataReader(
                String.Format("SELECT * FROM payroll_management.employee; where location='{0}' or first_name='{1}' or last_name='{2}' or middle_name='{3}' or job_title='{4}';", location, first_name, last_name, middle_name, job_title))
                If reader.HasRows Then
                    reader.Read()
                    employees.Add(New Model.Employee(reader))
                End If
            End Using

            Return employees
        End Function

        'Public Shared Function CompleteEmployeDetail(databaseManager As Manager.Mysql) As Model.Employee

        'End Function
    End Class
End Namespace
