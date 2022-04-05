using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using monitoring_module.LogDetail;
using monitoring_module.Logging;

using utility_service.Manager;

using hrms_api_service.Manager.API;
using hrms_api_service.IInterface;

namespace employee_module
{
    public class EmployeeGateway
    {
        public static List<EmployeeModel> Collect(Mysql databaseManager)
        {
            var employees = new List<EmployeeModel>();

            using (MySqlDataReader reader = databaseManager.ExecuteDataReader("SELECT * FROM employee_db.employee;"))
            {
                while (reader.Read())
                {
                    try
                    {
                        employees.Add(new EmployeeModel(reader));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        throw;
                    }
                }
            }
            return employees;
        }

        public static async Task<EmployeeModel> SyncEmployeeFromHRMS(Mysql databaseManager, HRMS hrmsAPIManager, String ee_id, EmployeeModel employee, LoggingService loggingService)
        {
            IEmployee employeeFound = await hrmsAPIManager.GetEmployeeFromServer_NoPrompt(ee_id);
            if (!(employeeFound is null))
            {
                if (!(employee is null))
                {
                    employee = Update(databaseManager, employeeFound, loggingService);
                }
                else
                {
                    employee = Save(databaseManager, employeeFound, loggingService);
                }
            }
            return employee;
        }

        public static EmployeeModel Find(Mysql databaseManager, string ee_id)
        {
            EmployeeModel employee = null;
            using (MySqlDataReader reader = databaseManager.ExecuteDataReader($"SELECT * FROM employee_db.employee where ee_id='{ee_id}' LIMIT 1;"))
            {
                while (reader.Read())
                {
                    employee = new EmployeeModel(reader);
                }
            }
            return employee;
        }
        public static List<EmployeeModel> Filter(Mysql databaseManager, string filterString)
        {
            var employees = new List<EmployeeModel>();
            using (MySqlDataReader reader = databaseManager.ExecuteDataReader($"SELECT * FROM employee_db.employee where location like '%{filterString}%' or first_name like '%{filterString}%' or last_name like '%{filterString}%' or middle_name like '%{filterString}%';"))
            {
                while (reader.Read())
                {
                    employees.Add(new EmployeeModel(reader));
                }
            }
            return employees;
        }

        #region "Update"
        public static EmployeeModel Update(Mysql databaseManager, EmployeeModel nEmployee, LoggingService loggingService)
        {
            try
            {
                EmployeeModel oldEmployee = Find(databaseManager, nEmployee.EE_Id);
                if (!(oldEmployee is null))
                {
                    var command = new MySqlCommand("UPDATE employee_db.employee SET  first_name=?, last_name=?, middle_name=?, location=?, card_number=?, account_number=?, bank_category=?, bank_name=?, payroll_code=?, tin=?, pagibig=?, sss=?, philhealth=? WHERE ee_id =?;", databaseManager.Connection);
                    command.Parameters.AddWithValue("p2", nEmployee.First_Name);
                    command.Parameters.AddWithValue("p3", nEmployee.Last_Name);
                    command.Parameters.AddWithValue("p4", nEmployee.Middle_Name);
                    command.Parameters.AddWithValue("p5", nEmployee.Location + "");
                    command.Parameters.AddWithValue("p7", nEmployee.Card_Number);
                    command.Parameters.AddWithValue("p8", nEmployee.Account_Number);
                    command.Parameters.AddWithValue("p9", ParseBankCategory(nEmployee.Bank_Category + ""));
                    command.Parameters.AddWithValue("p10", nEmployee.Bank_Name);
                    command.Parameters.AddWithValue("p11", ParsePayrollCode(nEmployee.Payroll_Code + ""));
                    command.Parameters.AddWithValue("p6", nEmployee.TIN);
                    command.Parameters.AddWithValue("p11b", nEmployee.Pagibig);
                    command.Parameters.AddWithValue("p11c", nEmployee.SSS);
                    command.Parameters.AddWithValue("p11d", nEmployee.PhilHealth);
                    command.Parameters.AddWithValue("p1", nEmployee.EE_Id);
                    command.ExecuteNonQuery();

                    if (!(oldEmployee.Pagibig == nEmployee.Pagibig))
                    {
                        loggingService.LogActivity(databaseManager, nEmployee.EE_Id, new ChangeLog("Pagibig", oldEmployee.Pagibig, nEmployee.Pagibig), "HRMS");
                    }
                    if (!(oldEmployee.SSS == nEmployee.SSS))
                    {
                        loggingService.LogActivity(databaseManager, nEmployee.EE_Id, new ChangeLog("SSS", oldEmployee.SSS, nEmployee.SSS), "HRMS");
                    }
                    if (!(oldEmployee.PhilHealth == nEmployee.PhilHealth))
                    {
                        loggingService.LogActivity(databaseManager, nEmployee.EE_Id, new ChangeLog("PhilHealth", oldEmployee.PhilHealth, nEmployee.PhilHealth), "HRMS");
                    }
                    if (!(oldEmployee.First_Name == nEmployee.First_Name))
                    {
                        loggingService.LogActivity(databaseManager, nEmployee.EE_Id, new ChangeLog("First_Name", oldEmployee.First_Name, nEmployee.First_Name), "");
                    }
                    if (!(oldEmployee.Last_Name == nEmployee.Last_Name))
                    {
                        loggingService.LogActivity(databaseManager, nEmployee.EE_Id, new ChangeLog("Last_Name", oldEmployee.Last_Name, nEmployee.Last_Name), "");
                    }
                    if (!(oldEmployee.Middle_Name == nEmployee.Middle_Name))
                    {
                        loggingService.LogActivity(databaseManager, nEmployee.EE_Id, new ChangeLog("Middle_Name", oldEmployee.Middle_Name, nEmployee.Middle_Name), "");
                    }
                    if (!(oldEmployee.Location == nEmployee.Location))
                    {
                        loggingService.LogActivity(databaseManager, nEmployee.EE_Id, new ChangeLog("Location", oldEmployee.Location, nEmployee.Location), "");
                    }
                    if (!(oldEmployee.TIN == nEmployee.TIN))
                    {
                        loggingService.LogActivity(databaseManager, nEmployee.EE_Id, new ChangeLog("TIN", oldEmployee.TIN, nEmployee.TIN), "");
                    }
                    if (!(ParseBankCategory(nEmployee.Bank_Category + "") == oldEmployee.Bank_Category))
                    {
                        loggingService.LogActivity(databaseManager, nEmployee.EE_Id, new ChangeLog("bank_category", oldEmployee.Bank_Category, nEmployee.Bank_Category), "");
                    }
                    if (!(ParsePayrollCode(nEmployee.Payroll_Code + "") == oldEmployee.Payroll_Code))
                    {
                        loggingService.LogActivity(databaseManager, nEmployee.EE_Id, new ChangeLog("Payroll_Code", oldEmployee.Payroll_Code, nEmployee.Payroll_Code), "");
                    }
                    if (nEmployee.Card_Number != "" && (!(nEmployee.Card_Number == oldEmployee.Card_Number)))
                    {
                        loggingService.LogActivity(databaseManager, nEmployee.EE_Id, new ChangeLog("Card_Number", oldEmployee.Card_Number, nEmployee.Card_Number), "");
                    }
                    if (nEmployee.Account_Number != "" && (!(nEmployee.Account_Number == oldEmployee.Account_Number)))
                    {
                        loggingService.LogActivity(databaseManager, nEmployee.EE_Id, new ChangeLog("Account_Number", oldEmployee.Account_Number, nEmployee.Account_Number), "");
                    }
                    if (nEmployee.Bank_Name != "" && (!(nEmployee.Bank_Name == oldEmployee.Bank_Name)))
                    {
                        loggingService.LogActivity(databaseManager, nEmployee.EE_Id, new ChangeLog("Bank_Name", oldEmployee.Bank_Name, nEmployee.Bank_Name), "");
                    }
                    else
                    {
                        return Save(databaseManager, nEmployee, loggingService);
                    }

                }
            }
            catch (Exception ex)
            {
                loggingService.LogError(databaseManager, ex.Message, "bank_category - Save");
            }

            return Find(databaseManager, nEmployee.EE_Id);
        }
        public static EmployeeModel Update(Mysql databaseManager, IEmployee nEmployee, LoggingService loggingService)
        {
            try
            {
                EmployeeModel oldEmployee = Find(databaseManager, nEmployee.ee_id);
                if (!(oldEmployee is null))
                {
                    var command = new MySqlCommand("UPDATE employee_db.employee SET  first_name=?, last_name=?, middle_name=?, location=?, card_number=?, account_number=?, bank_category=?, bank_name=?, payroll_code=?, tin=?, pagibig=?, sss=?, philhealth=? WHERE ee_id =?;", databaseManager.Connection);
                    command.Parameters.AddWithValue("p2", nEmployee.first_name);
                    command.Parameters.AddWithValue("p3", nEmployee.last_name);
                    command.Parameters.AddWithValue("p4", nEmployee.middle_name);
                    command.Parameters.AddWithValue("p5", nEmployee.department + "");
                    command.Parameters.AddWithValue("p7", nEmployee.card_number);
                    command.Parameters.AddWithValue("p8", nEmployee.account_number);
                    command.Parameters.AddWithValue("p9", ParseBankCategory(nEmployee.bank_category + ""));
                    command.Parameters.AddWithValue("p10", nEmployee.bank_name);
                    command.Parameters.AddWithValue("p11", ParsePayrollCode(nEmployee.payroll_code + ""));
                    command.Parameters.AddWithValue("p6", nEmployee.tin);
                    command.Parameters.AddWithValue("p11b", nEmployee.pagibig);
                    command.Parameters.AddWithValue("p11c", nEmployee.sss);
                    command.Parameters.AddWithValue("p11d", nEmployee.philhealth);
                    command.Parameters.AddWithValue("p1", nEmployee.ee_id);
                    command.ExecuteNonQuery();

                    if (!(oldEmployee.Pagibig == nEmployee.pagibig))
                    {
                        loggingService.LogActivity(databaseManager, nEmployee.ee_id, new ChangeLog("Pagibig", oldEmployee.Pagibig, nEmployee.pagibig), "HRMS");
                    }
                    if (!(oldEmployee.SSS== nEmployee.sss))
                    {
                        loggingService.LogActivity(databaseManager, nEmployee.ee_id, new ChangeLog("SSS", oldEmployee.SSS, nEmployee.sss), "HRMS");
                    }
                    if (!(oldEmployee.PhilHealth== nEmployee.philhealth))
                    {
                        loggingService.LogActivity(databaseManager, nEmployee.ee_id, new ChangeLog("PhilHealth", oldEmployee.PhilHealth, nEmployee.philhealth), "HRMS");
                    }
                    if (!(oldEmployee.First_Name == nEmployee.first_name))
                    {
                        loggingService.LogActivity(databaseManager, nEmployee.ee_id, new ChangeLog("First_Name", oldEmployee.First_Name, nEmployee.first_name), "HRMS");
                    }
                    if (!(oldEmployee.Last_Name == nEmployee.last_name))
                    {
                        loggingService.LogActivity(databaseManager, nEmployee.ee_id, new ChangeLog("Last_Name", oldEmployee.Last_Name, nEmployee.last_name), "HRMS");
                    }
                    if (!(oldEmployee.Middle_Name == nEmployee.middle_name))
                    {
                        loggingService.LogActivity(databaseManager, nEmployee.ee_id, new ChangeLog("Middle_Name", oldEmployee.Middle_Name, nEmployee.middle_name), "HRMS");
                    }
                    if (!(oldEmployee.Location == nEmployee.department))
                    {
                        loggingService.LogActivity(databaseManager, nEmployee.ee_id, new ChangeLog("Location", oldEmployee.Location, nEmployee.job_location), "HRMS");
                    }
                    if (!(oldEmployee.TIN == nEmployee.tin))
                    {
                        loggingService.LogActivity(databaseManager, nEmployee.ee_id, new ChangeLog("TIN", oldEmployee.TIN, nEmployee.tin), "HRMS");
                    }
                    if (!(ParseBankCategory(nEmployee.bank_category + "") == oldEmployee.Bank_Category))
                    {
                        loggingService.LogActivity(databaseManager, nEmployee.ee_id, new ChangeLog("bank_category", oldEmployee.Bank_Category, ParseBankCategory(nEmployee.bank_category + "")), "HRMS");
                    }
                    if (!(ParsePayrollCode(nEmployee.payroll_code + "") == oldEmployee.Payroll_Code))
                    {
                        loggingService.LogActivity(databaseManager, nEmployee.ee_id, new ChangeLog("Payroll_Code", oldEmployee.Payroll_Code, ParsePayrollCode(nEmployee.payroll_code + "")), "HRMS");
                    }
                    if (nEmployee.card_number != "" && (!(nEmployee.card_number == oldEmployee.Card_Number)))
                    {
                        loggingService.LogActivity(databaseManager, nEmployee.ee_id, new ChangeLog("Card_Number", oldEmployee.Card_Number, nEmployee.card_number), "HRMS");
                    }
                    if (nEmployee.account_number != "" && (!(nEmployee.account_number == oldEmployee.Account_Number)))
                    {
                        loggingService.LogActivity(databaseManager, nEmployee.ee_id, new ChangeLog("Account_Number", oldEmployee.Account_Number, nEmployee.account_number), "HRMS");
                    }
                    if (nEmployee.bank_name != "" && (!(nEmployee.bank_name == oldEmployee.Bank_Name)))
                    {
                        loggingService.LogActivity(databaseManager, nEmployee.ee_id, new ChangeLog("Bank_Name", oldEmployee.Bank_Name, nEmployee.bank_name), "HRMS");
                    }
                    else
                    {
                        return Save(databaseManager, nEmployee, loggingService);
                    }

                }
            }
            catch (Exception ex)
            {
                loggingService.LogError(databaseManager, ex.Message, "bank_category - Save");
            }

            return Find(databaseManager, nEmployee.ee_id);
        }
        #endregion

        #region "Save"
        public static EmployeeModel Save(Mysql databaseManager, EmployeeModel nEmployee, LoggingService loggingService)
        {
            try
            {
                var command = new MySqlCommand("REPLACE INTO employee_db.employee (ee_id, first_name, last_name,middle_name,location,tin,card_number,account_number,bank_category,bank_name,payroll_code,pagibig,sss,philhealth)VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?)", databaseManager.Connection);
                command.Parameters.AddWithValue("p1", nEmployee.EE_Id);
                command.Parameters.AddWithValue("p2", nEmployee.First_Name);
                command.Parameters.AddWithValue("p3", nEmployee.Last_Name);
                command.Parameters.AddWithValue("p4", nEmployee.Middle_Name);
                command.Parameters.AddWithValue("p5", nEmployee.Location + "");
                command.Parameters.AddWithValue("p6", nEmployee.TIN);
                command.Parameters.AddWithValue("p7", nEmployee.Card_Number);
                command.Parameters.AddWithValue("p8", nEmployee.Account_Number);
                command.Parameters.AddWithValue("p9", nEmployee.Bank_Category);
                command.Parameters.AddWithValue("p10", nEmployee.Bank_Name);
                command.Parameters.AddWithValue("p11a", nEmployee.Payroll_Code);
                command.Parameters.AddWithValue("p11b", nEmployee.Pagibig);
                command.Parameters.AddWithValue("p11c", nEmployee.SSS);
                command.Parameters.AddWithValue("p11d", nEmployee.PhilHealth);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                loggingService.LogError(databaseManager, ex.Message, "bank_category - Save");
            }
            return Find(databaseManager, nEmployee.EE_Id);
        }
        public static EmployeeModel Save(Mysql databaseManager, IEmployee nEmployee, LoggingService loggingService)
        {
            try
            {
                var command = new MySqlCommand("REPLACE INTO employee_db.employee (ee_id, first_name, last_name,middle_name,location,tin,card_number,account_number,bank_category,bank_name,payroll_code,pagibig,sss,philhealth)VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?)", databaseManager.Connection);
                command.Parameters.AddWithValue("p1", nEmployee.ee_id);
                command.Parameters.AddWithValue("p2", nEmployee.first_name);
                command.Parameters.AddWithValue("p3", nEmployee.last_name);
                command.Parameters.AddWithValue("p4", nEmployee.middle_name);
                command.Parameters.AddWithValue("p5", nEmployee.department + "");
                command.Parameters.AddWithValue("p6", nEmployee.tin);
                command.Parameters.AddWithValue("p7", nEmployee.card_number);
                command.Parameters.AddWithValue("p8", nEmployee.account_number);
                command.Parameters.AddWithValue("p9", ParseBankCategory(nEmployee.bank_category + ""));
                command.Parameters.AddWithValue("p10", nEmployee.bank_name);
                command.Parameters.AddWithValue("p11", ParsePayrollCode(nEmployee.payroll_code + ""));
                command.Parameters.AddWithValue("p11b", nEmployee.pagibig);
                command.Parameters.AddWithValue("p11c", nEmployee.sss);
                command.Parameters.AddWithValue("p11d", nEmployee.philhealth);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                loggingService.LogError(databaseManager, ex.Message, "bank_category - Save");
            }
            return Find(databaseManager, nEmployee.ee_id);
        }
        #endregion


        public static string ParsePayrollCode(string payroll_code)
        {
            string pCode = payroll_code.Split('-')[0].Replace("PAY", "P").Trim();

            if (pCode.Contains("K12AA")) { return "K12A"; }
            if (pCode.Contains("K12AT")) { return "K12"; }

            if (pCode.Contains("K12A")) { return "K12A"; }
            if (pCode.Contains("K12")) { return "K12"; }

            if (pCode.Contains("K13")) { return "K13"; }
            if (pCode.Contains("P1A")) { return "P1A"; }
            if (pCode.Contains("P4A")) { return "P4A"; }
            if (pCode.Contains("P7A")) { return "P7A"; }
            if (pCode.Contains("P10A")) { return "P10A"; }
            if (pCode.Contains("P11A")) { return "P11A"; }

            if (pCode == "") { pCode = "NOCODE"; }

            return pCode;
        }

        public static string ParseBankCategory(string bank_category)
        {
            string bankCat = bank_category;
            switch (bankCat)
            {
                case "ATM":
                case "ATM1":
                    bankCat = "ATM1";
                    break;
                case "ATM2":
                case "CHECK":
                case "CHEQUE":
                case "":
                case "NO BANK":
                    bankCat = "CHK";
                    break;
                case "CASHCARD":
                case "CCARD":
                    bankCat = "CCARD";
                    break;
                case "CASH":
                    bankCat = "CASH";
                    break;
                default:
                    //MsgBox(bankCat)
                    break;
            }
            return bankCat;
        }


        public static List<string> CollectPayrollCodes(Mysql databaseManager)
        {
            var payrollCodes = new List<string>();

            using (MySqlDataReader reader = databaseManager.ExecuteDataReader("SELECT payroll_code FROM employee_db.employee GROUP BY payroll_code;"))
            {
                while (reader.Read())
                {
                    payrollCodes.Add(reader.GetString("payroll_code"));
                }
            }
            return payrollCodes;
        }

        public static List<string> CollectBankCategories(Mysql databaseManager)
        {
            var bankCategories = new List<string>();
            using (MySqlDataReader reader = databaseManager.ExecuteDataReader("SELECT bank_category FROM employee_db.employee GROUP BY bank_category;"))
            {
                while (reader.Read())
                {
                    bankCategories.Add(reader.GetString("bank_category"));
                }
            }
            return bankCategories;
        }

        public static DateTime TimeHasChange(Mysql databaseManager)
        {
            DateTime modifiedDate = new DateTime();
            using (MySqlDataReader reader = databaseManager.ExecuteDataReader("SELECT date_modified FROM employee_db.employee GROUP BY date_modified;"))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    modifiedDate = reader.GetDateTime("date_modified");
                }
            }
            return modifiedDate;
        }
    }
}
