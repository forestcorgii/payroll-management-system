using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using utility_service;

namespace employee_module
{
    public class PayrollSnapshotGateway
    {
        public PayrollSnapshotModel Find(utility_service.Manager.Mysql databaseManager, string EEId, DateTime PayrollDate)
        {
            List<PayrollSnapshotModel> payrolls = new List<PayrollSnapshotModel>();
            using (MySqlDataReader reader = databaseManager.ExecuteDataReader($"SELECT * FROM employee_db.payroll_info WHERE ee_id='{EEId}' AND payroll_date='{PayrollDate}';"))
            {
                if (reader.HasRows)
                {
                    while (reader.Read()) { payrolls.Add(new PayrollSnapshotModel(reader)); }
                    return payrolls[0];
                }
            }
            return null;
        }
        public List<PayrollSnapshotModel> Collect(utility_service.Manager.Mysql databaseManager)
        {
            List<PayrollSnapshotModel> payrolls = new List<PayrollSnapshotModel>();
            using (MySqlDataReader reader = databaseManager.ExecuteDataReader($"SELECT * FROM employee_db.payroll_info;"))
            {
                if (reader.HasRows)
                {
                    while (reader.Read()) { payrolls.Add(new PayrollSnapshotModel(reader)); }
                    return payrolls;
                }
            }
            return null;
        }
        public List<PayrollSnapshotModel> Filter(utility_service.Manager.Mysql databaseManager,string EEId)
        {

            List<PayrollSnapshotModel> payrolls = new List<PayrollSnapshotModel>();
            using (MySqlDataReader reader = databaseManager.ExecuteDataReader($"SELECT * FROM employee_db.payroll_info WHERE ee_id='{EEId}';"))
            {
                if (reader.HasRows)
                {
                    while (reader.Read()) { payrolls.Add(new PayrollSnapshotModel(reader)); }
                    return payrolls;
                }
            }
            return null;
        }
        public PayrollSnapshotModel Save(utility_service.Manager.Mysql databaseManager, PayrollSnapshotModel payrollInfo)
        {
            MySqlCommand command = new MySqlCommand("INSERT INTO employee_db.payroll_info (id,ee_id,payroll_date,payroll_code,bank_category,bank_name) VALUES(?,?,?,?,?,?);", databaseManager.Connection);
            command.Parameters.AddWithValue("p0", payrollInfo.Id);
            command.Parameters.AddWithValue("p1", payrollInfo.EE_Id);
            command.Parameters.AddWithValue("p2", payrollInfo.Payroll_Date);
            command.Parameters.AddWithValue("p3", payrollInfo.Payroll_Code);
            command.Parameters.AddWithValue("p4", payrollInfo.Bank_Category);
            command.Parameters.AddWithValue("p5", payrollInfo.Bank_Name);
            command.ExecuteNonQuery();

            return Find(databaseManager, payrollInfo.EE_Id, payrollInfo.Payroll_Date);
        }
        public PayrollSnapshotModel Update(utility_service.Manager.Mysql databaseManager, PayrollSnapshotModel payrollInfo)
        {
            MySqlCommand command = new MySqlCommand("UPDATE employee_db.payroll_info SET id=?, ee_id=?, payroll_date=?, payroll_code=?, bank_category=?, bank_name=?;", databaseManager.Connection);
            command.Parameters.AddWithValue("p0", payrollInfo.Id);
            command.Parameters.AddWithValue("p1", payrollInfo.EE_Id);
            command.Parameters.AddWithValue("p2", payrollInfo.Payroll_Date);
            command.Parameters.AddWithValue("p3", payrollInfo.Payroll_Code);
            command.Parameters.AddWithValue("p4", payrollInfo.Bank_Category);
            command.Parameters.AddWithValue("p5", payrollInfo.Bank_Name);
            command.ExecuteNonQuery();

            return Find(databaseManager, payrollInfo.EE_Id, payrollInfo.Payroll_Date);
        }
    }
}
