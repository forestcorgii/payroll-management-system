using MySql.Data.MySqlClient;
using System;

namespace employee_module
{
    public class EmployeeModel
    {
        public string EE_Id { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Middle_Name { get; set; }
        public string Location { get; set; }

        public string Card_Number { get; set; }
        public string Account_Number { get; set; }
        public string Payroll_Code { get; set; }
        public string Bank_Category { get; set; }
        public string Bank_Name { get; set; }

        public string Pagibig { get; set; }
        public string PhilHealth { get; set; }
        public string SSS { get; set; }
        public string TIN { get; set; }

        public DateTime Date_Modified { get; set; }

        public string Fullname
        {
            get
            {
                if (First_Name is null || Last_Name is null) { return ""; }
                string _fullName = $"{Last_Name}, {First_Name}";
                if (Middle_Name == "" || Middle_Name is null) { _fullName = $"{_fullName}."; } else { _fullName = $"{_fullName} {Middle_Name.Substring(0, 1)}."; }
                return _fullName;
            }
        }

        public EmployeeModel() { }

        public EmployeeModel(MySqlDataReader reader)
        {

            EE_Id = reader.GetString("ee_id");
            First_Name = reader.GetString("first_name");
            Last_Name = reader.GetString("last_name");
            Middle_Name = reader.GetString("middle_name");
            Account_Number = reader.GetString("account_number");
            Card_Number = reader.GetString("card_number");
            Bank_Category = reader.GetString("bank_category");
            Bank_Name = reader.GetString("bank_name");
            Payroll_Code = reader.GetString("payroll_code");
            Location = reader.GetString("location");
            TIN = reader.GetString("tin");
            Pagibig = reader.GetString("pagibig");
            SSS = reader.GetString("sss");
            PhilHealth = reader.GetString("philhealth");

            Date_Modified = reader.GetDateTime("date_modified");
        }
    }
}
