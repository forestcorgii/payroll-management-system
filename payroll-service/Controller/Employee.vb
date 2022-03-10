Imports MySql.Data.MySqlClient
Imports Newtonsoft.Json
Imports utility_service

Namespace Controller
    Public Class Employee
        Public Shared Function CollectEmployeeForSyncing(databaseManager As Manager.Mysql)
            'Get Cutoff Date
            Dim cutOffDate As Date = Now
            If cutOffDate.Day >= 8 Then
                cutOffDate = New Date(cutOffDate.Year, cutOffDate.Month, 10)
            ElseIf cutOffDate.Day >= 24 Then
                cutOffDate = New Date(cutOffDate.Year, cutOffDate.Month, 24)
            End If

            Dim employees As New List(Of Model.Employee)
            Using reader As MySqlDataReader = databaseManager.ExecuteDataReader(
                String.Format("SELECT * FROM payroll_management.employee where date_modified <= '{0}';", cutOffDate.ToString("yyyy-MM-dd")))
                While reader.Read()
                    employees.Add(New Model.Employee(reader))
                End While
            End Using

            Return employees
        End Function

        Public Shared Async Function SyncEmployeeFromHRMSAsync(databaseManager As Manager.Mysql, hrmsAPIManager As Manager.API.HRMS, ee_id As String, Optional employee As Model.Employee = Nothing) As Task(Of Model.Employee)
            Dim response As Object() = Await hrmsAPIManager.SendPOSTRequest(ee_id)
            If response(0) Then
                Dim hrms_employee As Manager.API.HRMS.ResponseArgument = JsonConvert.DeserializeObject(Of Manager.API.HRMS.ResponseArgument)(response(1))
                If employee IsNot Nothing Then
                    employee = UpdateEmployee(databaseManager, hrms_employee.message(0), employee)
                Else
                    employee = SaveEmployee(databaseManager, hrms_employee.message(0))
                End If
                Return employee
            Else
                Throw New Exception("Employee not found in HRMS.")
            End If

            Return Nothing
        End Function

        Public Shared Function GetEmployee(databaseManager As Manager.Mysql, Optional ee_id As String = "") As Model.Employee
            Dim employee As Model.Employee = Nothing
            Using reader As MySqlDataReader = databaseManager.ExecuteDataReader(
                String.Format("SELECT * FROM payroll_management.employee where ee_id='{0}' LIMIT 1;", ee_id))
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

        Public Shared Function UpdateEmployee(databaseManager As Manager.Mysql, newEmployee As Manager.API.HRMS.ResponseArgument.MessageArgument, oldEmployee As Model.Employee) As Model.Employee
            oldEmployee.First_Name = newEmployee.first_name
            oldEmployee.Last_Name = newEmployee.last_name
            oldEmployee.Middle_Name = newEmployee.middle_name
            oldEmployee.Location = newEmployee.department
            oldEmployee.TIN = newEmployee.tin

            SaveEmployee(databaseManager, oldEmployee)


            If ParseBankCategory(newEmployee.bank_category & "") <> oldEmployee.Bank_Category Then
                BankCategory.SaveBankCategory(databaseManager, ParseBankCategory(newEmployee.bank_category & ""), oldEmployee.EE_Id)
            End If
            If ParsePayrollCode(newEmployee.payroll_code & "") <> oldEmployee.Payroll_Code Then
                PayrollCode.SavePayrollCode(databaseManager, ParsePayrollCode(newEmployee.payroll_code & ""), oldEmployee.EE_Id)
            End If
            If newEmployee.card_number <> "" AndAlso newEmployee.card_number <> oldEmployee.Card_Number Then
                CardNumber.SaveCardNumber(databaseManager, newEmployee.card_number, oldEmployee.EE_Id)
            End If
            If newEmployee.account_number <> "" AndAlso newEmployee.account_number <> oldEmployee.Account_Number Then
                AccountNumber.SaveAccountNumber(databaseManager, newEmployee.account_number, oldEmployee.EE_Id)
            End If
            If newEmployee.bank_name <> "" AndAlso newEmployee.bank_name <> oldEmployee.Bank_Name Then
                BankName.SaveBankName(databaseManager, newEmployee.bank_name, oldEmployee.EE_Id)
            End If

            Dim ee As Model.Employee = GetEmployee(databaseManager, ee_id:=newEmployee.idno)
            Return ee
        End Function

        Public Shared Function SaveEmployee(databaseManager As Manager.Mysql, newEmployee As Manager.API.HRMS.ResponseArgument.MessageArgument) As Model.Employee
            Dim command As New MySqlCommand("REPLACE INTO payroll_management.employee (ee_id, first_name, last_name,middle_name,location,tin)VALUES(?,?,?,?,?,?)", databaseManager.Connection)
            command.Parameters.AddWithValue("p1", newEmployee.idno)
            command.Parameters.AddWithValue("p2", newEmployee.first_name)
            command.Parameters.AddWithValue("p3", newEmployee.last_name)
            command.Parameters.AddWithValue("p4", newEmployee.middle_name)
            command.Parameters.AddWithValue("p5", newEmployee.department)
            command.Parameters.AddWithValue("p6", newEmployee.tin)
            command.ExecuteNonQuery()

            Dim ee As Model.Employee = GetEmployee(databaseManager, ee_id:=newEmployee.idno)

            If Not newEmployee.card_number = "" Then CardNumber.SaveCardNumber(databaseManager, newEmployee.card_number,ee.ee_id)
            If Not newEmployee.account_number = "" Then AccountNumber.SaveAccountNumber(databaseManager, newEmployee.account_number,ee.ee_id)
            If Not newEmployee.bank_category = "" Then BankCategory.SaveBankCategory(databaseManager, ParseBankCategory(newEmployee.bank_category),ee.ee_id)
            If Not newEmployee.bank_name = "" Then BankName.SaveBankName(databaseManager, newEmployee.bank_name,ee.ee_id)
            If Not newEmployee.payroll_code = "" Then PayrollCode.SavePayrollCode(databaseManager, ParsePayrollCode(newEmployee.payroll_code),ee.ee_id)

            Return ee
        End Function
        Public Shared Function SaveEmployee(databaseManager As Manager.Mysql, newEmployee As Model.Employee) As Model.Employee
            Dim command As New MySqlCommand("REPLACE INTO payroll_management.employee (ee_id, first_name, last_name,middle_name,location,tin,card_number,account_number,bank_category,bank_name,payroll_code)VALUES(?,?,?,?,?,?,?,?,?,?,?)", databaseManager.Connection)
            command.Parameters.AddWithValue("p1", newEmployee.EE_Id)
            command.Parameters.AddWithValue("p2", newEmployee.First_Name)
            command.Parameters.AddWithValue("p3", newEmployee.Last_Name)
            command.Parameters.AddWithValue("p4", newEmployee.Middle_Name)
            command.Parameters.AddWithValue("p5", newEmployee.Location)
            command.Parameters.AddWithValue("p6", newEmployee.TIN)
            command.Parameters.AddWithValue("p7", newEmployee.Card_Number)
            command.Parameters.AddWithValue("p8", newEmployee.Account_Number)
            command.Parameters.AddWithValue("p9", newEmployee.Bank_Category)
            command.Parameters.AddWithValue("p10", newEmployee.Bank_Name)
            command.Parameters.AddWithValue("p11", newEmployee.Payroll_Code)
            command.ExecuteNonQuery()

            Dim employee As Model.Employee = GetEmployee(databaseManager, ee_id:=newEmployee.EE_Id)
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
