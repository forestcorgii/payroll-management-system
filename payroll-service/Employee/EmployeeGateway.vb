Imports MySql.Data.MySqlClient
Imports utility_service
Imports hrms_api_service
Imports monitoring_module

Namespace Employee_
    Public Class EmployeeGateway
        Public Shared Function Collect(databaseManager As Manager.Mysql) As List(Of EmployeeModel)
            Dim employees As New List(Of EmployeeModel)
            Using reader As MySqlDataReader = databaseManager.ExecuteDataReader("SELECT * FROM payroll_management.employee;")
                While reader.Read()
                    Try
                        employees.Add(New EmployeeModel(reader))
                    Catch ex As Exception
                        Console.WriteLine(ex.Message)
                    End Try
                End While
            End Using

            Return employees
        End Function

        Public Shared Async Function SyncEmployeeFromHRMS(databaseManager As Manager.Mysql, hrmsAPIManager As Manager.API.HRMS, ee_id As String, employee As EmployeeModel, loggingService As Logging.LoggingService) As Task(Of EmployeeModel)
            Dim employeeFound As IInterface.IEmployee = Await hrmsAPIManager.GetEmployeeFromServer_NoPrompt(ee_id)
            If employeeFound IsNot Nothing Then
                If employee IsNot Nothing Then
                    'employee = Update(databaseManager, employeeFound, employee, loggingService)
                    employee = Update(databaseManager, employeeFound, loggingService)
                Else
                    employee = Save(databaseManager, employeeFound, loggingService)
                End If
                Return employee
            Else
                'Throw New Exception("Employee not found in HRMS.")
            End If

            Return Nothing
        End Function

        Public Shared Function Find(databaseManager As Manager.Mysql, Optional ee_id As String = "") As EmployeeModel
            Dim employee As EmployeeModel = Nothing
            Using reader As MySqlDataReader = databaseManager.ExecuteDataReader(
                String.Format("SELECT * FROM payroll_management.employee where ee_id='{0}' LIMIT 1;", ee_id))
                If reader.HasRows Then
                    reader.Read()
                    employee = New EmployeeModel(reader)
                End If
            End Using

            Return employee
        End Function

        Public Shared Function Filter(databaseManager As Manager.Mysql, filterString As String) As List(Of EmployeeModel)
            Dim employees As New List(Of EmployeeModel)
            Using reader As MySqlDataReader = databaseManager.ExecuteDataReader(
                String.Format("SELECT * FROM payroll_management.employee where location like '%{0}%' or first_name like '%{0}%' or last_name like '%{0}%' or middle_name like '%{0}%';", filterString))
                While reader.Read()
                    employees.Add(New EmployeeModel(reader))
                End While
            End Using

            Return employees
        End Function

        'Public Shared Function Update(databaseManager As Manager.Mysql, newEmployee As Manager.API.HRMS.ResponseArgument.MessageArgument, oldEmployee As EmployeeModel, loggingService As Logging.LoggingService) As EmployeeModel
        '    oldEmployee.First_Name = newEmployee.first_name
        '    oldEmployee.Last_Name = newEmployee.last_name
        '    oldEmployee.Middle_Name = newEmployee.middle_name
        '    oldEmployee.Location = newEmployee.department
        '    oldEmployee.TIN = newEmployee.tin

        '    Save(databaseManager, oldEmployee, loggingService)

        '    If ParseBankCategory(newEmployee.bank_category & "") <> oldEmployee.Bank_Category Then
        '        BankCategory.Gateway.Save(databaseManager, ParseBankCategory(newEmployee.bank_category & ""), oldEmployee, loggingService)
        '    End If
        '    If ParsePayrollCode(newEmployee.payroll_code & "") <> oldEmployee.Payroll_Code Then
        '        PayrollCode.Gateway.Save(databaseManager, ParsePayrollCode(newEmployee.payroll_code & ""), oldEmployee.EE_Id)
        '    End If
        '    If newEmployee.card_number <> "" AndAlso newEmployee.card_number <> oldEmployee.Card_Number Then
        '        CardNumber.Gateway.Save(databaseManager, newEmployee.card_number, oldEmployee.EE_Id)
        '    End If
        '    If newEmployee.account_number <> "" AndAlso newEmployee.account_number <> oldEmployee.Account_Number Then
        '        AccountNumber.Gateway.SaveAccountNumber(databaseManager, newEmployee.account_number, oldEmployee.EE_Id)
        '    End If
        '    If newEmployee.bank_name <> "" AndAlso newEmployee.bank_name <> oldEmployee.Bank_Name Then
        '        BankName.Gateway.Save(databaseManager, newEmployee.bank_name, oldEmployee.EE_Id)
        '    End If


        '    Dim ee As EmployeeModel = Find(databaseManager, ee_id:=newEmployee.idno)
        '    Return ee
        'End Function

        Public Shared Function Update(databaseManager As Manager.Mysql, newEmployee As EmployeeModel, loggingService As Logging.LoggingService) As EmployeeModel
            Try
                Dim oldEmployee As EmployeeModel = Find(databaseManager, ee_id:=newEmployee.EE_Id)
                If oldEmployee IsNot Nothing Then
                    Dim command As New MySqlCommand("UPDATE payroll_management.employee SET card_number=? WHERE ee_id =?;", databaseManager.Connection)
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

                    If Not oldEmployee.First_Name = newEmployee.First_Name Then
                        loggingService.LogActivity(databaseManager, newEmployee.EE_Id, New LogDetail.ChangeLog("First_Name", oldEmployee.First_Name, newEmployee.First_Name), "")
                    End If
                    If Not oldEmployee.Last_Name = newEmployee.Last_Name Then
                        loggingService.LogActivity(databaseManager, newEmployee.EE_Id, New LogDetail.ChangeLog("Last_Name", oldEmployee.Last_Name, newEmployee.Last_Name), "")
                    End If
                    If Not oldEmployee.Middle_Name = newEmployee.Middle_Name Then
                        loggingService.LogActivity(databaseManager, newEmployee.EE_Id, New LogDetail.ChangeLog("Middle_Name", oldEmployee.Middle_Name, newEmployee.Middle_Name), "")
                    End If
                    If Not oldEmployee.Location = newEmployee.Location Then
                        loggingService.LogActivity(databaseManager, newEmployee.EE_Id, New LogDetail.ChangeLog("Location", oldEmployee.Location, newEmployee.Location), "")
                    End If
                    If Not oldEmployee.TIN = newEmployee.TIN Then
                        loggingService.LogActivity(databaseManager, newEmployee.EE_Id, New LogDetail.ChangeLog("TIN", oldEmployee.TIN, newEmployee.TIN), "")
                    End If
                    If Not ParseBankCategory(newEmployee.Bank_Category & "") = oldEmployee.Bank_Category Then
                        loggingService.LogActivity(databaseManager, newEmployee.EE_Id, New LogDetail.ChangeLog("bank_category", oldEmployee.Bank_Category, newEmployee.Bank_Category), "")
                    End If
                    If Not ParsePayrollCode(newEmployee.Payroll_Code & "") = oldEmployee.Payroll_Code Then
                        loggingService.LogActivity(databaseManager, newEmployee.EE_Id, New LogDetail.ChangeLog("Payroll_Code", oldEmployee.Payroll_Code, newEmployee.Payroll_Code), "")
                    End If
                    If newEmployee.Card_Number <> "" AndAlso Not newEmployee.Card_Number = oldEmployee.Card_Number Then
                        loggingService.LogActivity(databaseManager, newEmployee.EE_Id, New LogDetail.ChangeLog("Card_Number", oldEmployee.Card_Number, newEmployee.Card_Number), "")
                    End If
                    If newEmployee.Account_Number <> "" AndAlso Not newEmployee.Account_Number = oldEmployee.Account_Number Then
                        loggingService.LogActivity(databaseManager, newEmployee.EE_Id, New LogDetail.ChangeLog("Account_Number", oldEmployee.Account_Number, newEmployee.Account_Number), "")
                    End If
                    If newEmployee.Bank_Name <> "" AndAlso Not newEmployee.Bank_Name = oldEmployee.Bank_Name Then
                        loggingService.LogActivity(databaseManager, newEmployee.EE_Id, New LogDetail.ChangeLog("Bank_Name", oldEmployee.Bank_Name, newEmployee.Bank_Name), "")
                    End If
                Else
                    Return Save(databaseManager, newEmployee, loggingService)
                End If
            Catch ex As Exception
                loggingService.LogError(databaseManager, ex.Message, "bank_category - Save")
            End Try

            Return Find(databaseManager, ee_id:=newEmployee.EE_Id)
        End Function

        Public Shared Function Save(databaseManager As Manager.Mysql, newEmployee As EmployeeModel, loggingService As Logging.LoggingService) As EmployeeModel
            Try
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
            Catch ex As Exception
                loggingService.LogError(databaseManager, ex.Message, "bank_category - Save")
            End Try

            Return Find(databaseManager, ee_id:=newEmployee.EE_Id)
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


        Public Shared Function CollectPayrollCodes(databaseManager As Manager.Mysql) As List(Of String)
            Dim payrollCodes As New List(Of String)
            Using reader As MySqlDataReader = databaseManager.ExecuteDataReader("SELECT payroll_code FROM payroll_management.employee GROUP BY payroll_code;")
                While reader.Read
                    payrollCodes.Add(reader("payroll_code"))
                End While
            End Using

            Return payrollCodes
        End Function

        Public Shared Function CollectBankCategories(databaseManager As Manager.Mysql) As List(Of String)
            Dim bankCategories As New List(Of String)
            Using reader As MySqlDataReader = databaseManager.ExecuteDataReader("SELECT bank_category FROM payroll_management.employee GROUP BY bank_category;")
                While reader.Read
                    bankCategories.Add(reader("bank_category"))
                End While
            End Using

            Return bankCategories
        End Function

        Public Shared Function TimeHasChange(databaseManager As Manager.Mysql) As Date
            Dim modifiedDate As Date = Nothing
            Using reader As MySqlDataReader = databaseManager.ExecuteDataReader("SELECT date_modified FROM payroll_management.employee GROUP BY date_modified;")
                If reader.HasRows Then
                    reader.Read()
                    modifiedDate = reader("date_modified")
                End If
            End Using
            Return modifiedDate
        End Function
    End Class
End Namespace
