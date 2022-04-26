using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace employee_module
{
    public class PayrollInfoModel
    {
        public string Id { get; set; }
        public string EE_Id { get; set; }
        public DateTime Payroll_Date { get; set; }
        public string Payroll_Code { get; set; }
        public string Bank_Category { get; set; }
        public string Bank_Name { get; set; }
        
        public DateTime Date_Created { get; set; }

        public PayrollInfoModel()
        {
        }
        public PayrollInfoModel(MySqlDataReader reader)
        {
            Id = reader.GetString("id");
            EE_Id = reader.GetString("ee_id");

            Payroll_Date = reader.GetDateTime("payroll_date");
            Payroll_Code = reader.GetString("payroll_code");
            Bank_Category = reader.GetString("bank_category");
            Bank_Name = reader.GetString("bank_name");

            Date_Created = reader.GetDateTime("date_created");
        }
    }
}
