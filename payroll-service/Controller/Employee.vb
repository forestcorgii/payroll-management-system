Imports MySql.Data.MySqlClient
Imports Newtonsoft.Json
Imports utility_service

Namespace Controller
    Public Class Employee
        Public Shared Async Function SyncEmployeeFromHRMSAsync(databaseManager As Manager.Mysql, hrmsAPIManager As Manager.API.HRMS, employee_id As String) As Task(Of Model.Employee)
            Dim employee As Model.Employee
            Dim response As Object() = Await hrmsAPIManager.SendPOSTRequest(employee_id)
            If response(0) Then
                Dim hrms_employee As Manager.API.HRMS.ResponseArgument = JsonConvert.DeserializeObject(Of Manager.API.HRMS.ResponseArgument)(response(1))
                employee = SaveEmployee(databaseManager, hrms_employee.message(0))
                Return employee
            Else
                Throw New Exception("Employee not found in HRMS.")
            End If

            Return Nothing
        End Function

        Public Shared Function GetEmployee(databaseManager As Manager.Mysql, Optional id As Integer = 0, Optional employee_id As String = "") As Model.Employee
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

        Public Shared Sub UpdateEmployee(databaseManager As Manager.Mysql, newEmployee As Model.Employee, oldEmployee As Model.Employee)
            If newEmployee.Payroll_Code <> "" AndAlso newEmployee.Payroll_Code <> oldEmployee.Payroll_Code Then
                PayrollCode.SavePayrollCode(databaseManager, ParsePayrollCode(newEmployee.Payroll_Code), oldEmployee.Id)
            End If
            If newEmployee.Card_Number <> "" AndAlso newEmployee.Card_Number <> oldEmployee.Card_Number Then
                CardNumber.SaveCardNumber(databaseManager, newEmployee.Card_Number, oldEmployee.Id)
            End If
            If newEmployee.Account_Number <> "" AndAlso newEmployee.Account_Number <> oldEmployee.Account_Number Then
                AccountNumber.SaveAccountNumber(databaseManager, newEmployee.Account_Number, oldEmployee.Id)
            End If
            If newEmployee.Bank_Category <> "" AndAlso newEmployee.Bank_Category <> oldEmployee.Bank_Category Then
                BankCategory.SaveBankCategory(databaseManager, ParseBankCategory(newEmployee.Bank_Category), oldEmployee.Id)
            End If
            If newEmployee.Bank_Name <> "" AndAlso newEmployee.Bank_Name <> oldEmployee.Bank_Name Then
                BankName.SaveBankName(databaseManager, newEmployee.Bank_Name, oldEmployee.Id)
            End If
        End Sub

        Public Shared Function SaveEmployee(databaseManager As Manager.Mysql, newEmployee As Manager.API.HRMS.ResponseArgument.MessageArgument) As Model.Employee
            Dim command As New MySqlCommand("REPLACE INTO payroll_management.employee (employee_id, first_name, last_name,middle_name,location,tin)VALUES(?,?,?,?,?,?)", databaseManager.Connection)
            command.Parameters.AddWithValue("p1", newEmployee.idno)
            command.Parameters.AddWithValue("p2", newEmployee.first_name)
            command.Parameters.AddWithValue("p3", newEmployee.last_name)
            command.Parameters.AddWithValue("p4", newEmployee.middle_name)
            command.Parameters.AddWithValue("p5", newEmployee.department)
            command.Parameters.AddWithValue("p6", newEmployee.tin)
            command.ExecuteNonQuery()

            Dim ee As Model.Employee = GetEmployee(databaseManager, employee_id:=newEmployee.idno)
            CardNumber.SaveCardNumber(databaseManager, newEmployee.card_number, ee.Id)
            AccountNumber.SaveAccountNumber(databaseManager, newEmployee.account_number, ee.Id)
            BankCategory.SaveBankCategory(databaseManager, ParseBankCategory(newEmployee.bank_category), ee.Id)
            BankName.SaveBankName(databaseManager, newEmployee.bank_name, ee.Id)
            PayrollCode.SavePayrollCode(databaseManager, ParsePayrollCode(newEmployee.payroll_code), ee.Id)

            Return ee
        End Function
        Public Shared Function SaveEmployee(databaseManager As Manager.Mysql, newEmployee As Model.Employee) As Model.Employee
            Dim command As New MySqlCommand("REPLACE INTO payroll_management.employee (employee_id, first_name, last_name,middle_name)VALUES(?,?,?,?)", databaseManager.Connection)
            command.Parameters.AddWithValue("p1", newEmployee.Employee_Id)
            command.Parameters.AddWithValue("p2", newEmployee.First_Name)
            command.Parameters.AddWithValue("p3", newEmployee.Last_Name)
            command.Parameters.AddWithValue("p4", newEmployee.Middle_Name)
            command.ExecuteNonQuery()

            Dim oldEmployee As Model.Employee = GetEmployee(databaseManager, employee_id:=newEmployee.Employee_Id) 'OLD
            UpdateEmployee(databaseManager, newEmployee, oldEmployee)

            Return oldEmployee
        End Function

        Public Shared Function ParsePayrollCode(payroll_code As String) As String
            Dim pCode As String = payroll_code.Split("-")(0).Replace("PAY", "P")
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
