using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using utility_service.Manager;
namespace employee_module
{
    public class EmployeeController
    {
        public static bool Import(Mysql databaseManager, string filePath)
        {
            IWorkbook nWorkbook = null;
            using (FileStream eeFile = new FileStream(filePath,FileMode.Open,FileAccess.Read))
            {
                nWorkbook = new HSSFWorkbook(eeFile);
            }

            ISheet nSheet = nWorkbook.GetSheetAt(0);
            
            Int32 index = 0;
            IRow nRow = nSheet.GetRow(index);


            return false;
        }
    }
}
