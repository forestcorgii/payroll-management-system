
Imports NPOI.SS.UserModel

Public Interface IPayregInfo
    Inherits IList(Of IPayrollInfo)
    Property PayregPath As String


    Function GetPayrollDate(nSheet As ISheet) As String
    Function GetPayroll()

    Function CreatePayroll() As IPayrollInfo
    Function CreatePayreg() As IPayregInfo
    Sub PopulatePayslips(nSheet As ISheet)

    Function Find(id As String) As IPayrollInfo

    Function SyncToSheet(nSheet As ISheet) As Boolean


End Interface
