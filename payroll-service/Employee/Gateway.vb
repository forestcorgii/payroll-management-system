Imports MySql.Data.MySqlClient
Imports utility_service
Imports hrms_api_service

Namespace Employee
    Public Class Gateway
        Public Shared Function Collect(databaseManager As Manager.Mysql) As List(Of Employee.Model)
            Dim employees As New List(Of Employee.Model)
            Using reader As MySqlDataReader = databaseManager.ExecuteDataReader("SELECT * FROM payroll_management.employee;")
                While reader.Read()
                    Try
                        employees.Add(New Employee.Model(reader))
                    Catch ex As Exception
                        Console.WriteLine(ex.Message)
                    End Try
                End While
            End Using

            Return employees
        End Function

        Public Shared Async Function SyncEmployeeFromHRMS(databaseManager As Manager.Mysql, hrmsAPIManager As Manager.API.HRMS, ee_id As String, Optional employee As Employee.Model = Nothing) As Task(Of Employee.Model)
            Dim employeeFound As IInterface.IEmployee = Await hrmsAPIManager.GetEmployeeFromServer_NoPrompt(ee_id)
            If employeeFound IsNot Nothing Then
                If employee IsNot Nothing Then
                    employee = Update(databaseManager, employeeFound, employee)
                Else
                    employee = Save(databaseManager, employeeFound)
                End If
                Return employee
            Else
                'Throw New Exception("Employee not found in HRMS.")
            End If

            Return Nothing
        End Function

        Public Shared Function Find(databaseManager As Manager.Mysql, Optional ee_id As String = "") As Employee.Model
            Dim employee As Employee.Model = Nothing
            Using reader As MySqlDataReader = databaseManager.ExecuteDataReader(
                String.Format("SELECT * FROM payroll_management.employee where ee_id='{0}' LIMIT 1;", ee_id))
                If reader.HasRows Then
                    reader.Read()
                    employee = New Employee.Model(reader)
                End If
            End Using

            Return employee
        End Function

        Public Shared Function Filter(databaseManager As Manager.Mysql, Optional location As String = "", Optional first_name As String = "", Optional last_name As String = "", Optional middle_name As String = "", Optional job_title As String = "") As List(Of Employee.Model)
            Dim employees As List(Of Employee.Model) = Nothing
            Using reader As MySqlDataReader = databaseManager.ExecuteDataReader(
                String.Format("SELECT * FROM payroll_management.employee; where location='{0}' or first_name='{1}' or last_name='{2}' or middle_name='{3}' or job_title='{4}';", location, first_name, last_name, middle_name, job_title))
                If reader.HasRows Then
                    reader.Read()
                    employees.Add(New Employee.Model(reader))
                End If
            End Using

            Return employees
        End Function

        Public Shared Function Update(databaseManager As Manager.Mysql, newEmployee As Manager.API.HRMS.ResponseArgument.MessageArgument, oldEmployee As Employee.Model) As Employee.Model
            oldEmployee.First_Name = newEmployee.first_name
            oldEmployee.Last_Name = newEmployee.last_name
            oldEmployee.Middle_Name = newEmployee.middle_name
            oldEmployee.Location = newEmployee.department
            oldEmployee.TIN = newEmployee.tin

            Save(databaseManager, oldEmployee)

            If ParseBankCategory(newEmployee.bank_category & "") <> oldEmployee.Bank_Category Then
                BankCategory.Gateway.Save(databaseManager, ParseBankCategory(newEmployee.bank_category & ""), oldEmployee.EE_Id)
            End If
            If ParsePayrollCode(newEmployee.payroll_code & "") <> oldEmployee.Payroll_Code Then
                Payroll.Code.Gateway.SavePayrollCode(databaseManager, ParsePayrollCode(newEmployee.payroll_code & ""), oldEmployee.EE_Id)
            End If
            If newEmployee.card_number <> "" AndAlso newEmployee.card_number <> oldEmployee.Card_Number Then
                CardNumber.Gateway.Save(databaseManager, newEmployee.card_number, oldEmployee.EE_Id)
            End If
            If newEmployee.account_number <> "" AndAlso newEmployee.account_number <> oldEmployee.Account_Number Then
                AccountNumber.Gateway.SaveAccountNumber(databaseManager, newEmployee.account_number, oldEmployee.EE_Id)
            End If
            If newEmployee.bank_name <> "" AndAlso newEmployee.bank_name <> oldEmployee.Bank_Name Then
                BankName.Gateway.Save(databaseManager, newEmployee.bank_name, oldEmployee.EE_Id)
            End If

            Dim ee As Employee.Model = Find(databaseManager, ee_id:=newEmployee.idno)
            Return ee
        End Function

        Public Shared Function Save(databaseManager As Manager.Mysql, newEmployee As Employee.Model) As Employee.Model
            Dim command As New MySqlCommand("REPLACE INTO payroll_management.employee (ee_id, first_name, last_name,middle_name,location,tin,card_number,account_number,bank_category,bank_name,payroll_code)VALUES(?,?,?,?,?,?,?,?,?,?,?)", databaseManager.Connection)
            command.Parameters.AddWithValue("p1", newEmployee.EE_Id)
            command.Parameters.AddWithValue("p2", newEmployee.First_Name)
            command.Parameters.AddWithValue("p3", newEmployee.Last_Name)
            command.Parameters.AddWithValue("p4", newEmployee.Middle_Name)
            command.Parameters.AddWithValue("p5", newEmployee.Location & "")
            command.Parameters.AddWithValue("p6", newEmployee.TIN)
            command.Parameters.AddWithValue("p7", newEmployee.Card_Number)
            command.Parameters.AddWithValue("p8", newEmployee.Account_Number)
            command.Parameters.AddWithValue("p9", newEmployee.Bank_Category)
            command.Parameters.AddWithValue("p10", newEmployee.Bank_Name)
            command.Parameters.AddWithValue("p11", newEmployee.Payroll_Code)
            command.ExecuteNonQuery()

            Dim employee As Employee.Model = Find(databaseManager, ee_id:=newEmployee.EE_Id)
            Return employee
        End Function

        Public Shared Function ParsePayrollCode(payroll_code As String) As String
            Dim pCode As String = payroll_code.Split("-")(0).Replace("PAY", "P").Trim
            If pCode.Contains("K12AA") Then Return "K12A"
            If pCode.Contains("K12AT") Then Return "K12"

            If pCode.Contains("K12A") Then Return "K12A"
            If pCode.Contains("K12") Then Return "K12"

            If pCode.Contains("K13") Then Return "K13"
            If pCode.Contains("P1A") Then Return "P1A"
            If pCode.Contains("P4A") Then Return "P4A"
            If pCode.Contains("P7A") Then Return "P7A"
            If pCode.Contains("P10A") Then Return "P10A"
            If pCode.Contains("P11A") Then Return "P11A"

            If pCode = "" Then pCode = "NOCODE"
            Return pCode
        End Function

        Public Shared Function ParseBankCategory(bank_category As String) As String
            Dim bankCat As String = bank_category

            Select Case bankCat
                Case "ATM", "ATM1"
                    bankCat = "ATM1"
                Case "ATM2"
                Case "CHECK", "CHEQUE", "", "NO BANK"
                    bankCat = "CHK"
                Case "CASHCARD", "CCARD"
                    bankCat = "CCARD"
                Case "CASH"
                    bankCat = "CASH"
                Case Else
                    MsgBox(bankCat)
            End Select
            Return bankCat
        End Function
    End Class
End Namespace
