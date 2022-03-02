Imports NPOI.SS.UserModel
Imports payroll_service.Util

Public Interface IPayrollInfo
    'Property PayrollDate As String

    'Property Employee_Id As String
    'Property Fullname As String

    'Property REG_HRS As Double
    'Property ADJUST1 As Double
    'Property GROSS_PAY As Double
    'Property ADJUST2 As Double
    'Property WITHHOLDING_TAX As Double
    'Property Pagibig_EE As Double
    'Property Pagibig_ER As Double
    'Property SSS_EE As Double
    'Property SSS_ER As Double
    'Property PHIC As Double

    'ReadOnly Property SSS_PHIC_EE As Double

    'ReadOnly Property SSS_PHIC_ER As Double

    'Property NET_PAY As Double

    Function GetPayrollDetail(row As IRow)
    Sub PastePayslip(nSheet As ISheet, pageIdx As Integer, payslipPosition As PayslipPositionChoices)
    Function WriteGovernment(row As IRow)
    Sub Compute30thPayroll(Payroll15th_GROSS_PAY As Double)
End Interface
