
Imports NPOI.HSSF.UserModel
Imports NPOI.SS.UserModel
Imports System.IO
Namespace Model
    Namespace KS
        Public Class PayregInfo
            Implements IPayregInfo

            Private _list As New List(Of IPayrollInfo)

#Region "Interface Fields"
            Default Public Property Item(index As Integer) As IPayrollInfo Implements IList(Of IPayrollInfo).Item
                Get
                    Return _list(index)
                End Get
                Set(value As IPayrollInfo)
                    _list(index) = value
                End Set
            End Property

            Public ReadOnly Property Count As Integer Implements ICollection(Of IPayrollInfo).Count
                Get
                    Return _list.Count
                End Get
            End Property

            Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of IPayrollInfo).IsReadOnly
                Get
                    Throw New NotImplementedException()
                End Get
            End Property

            Public Property PayregPath As String Implements IPayregInfo.PayregPath

            Public Sub Insert(index As Integer, item As IPayrollInfo) Implements IList(Of IPayrollInfo).Insert
                _list.Insert(index, item)
            End Sub

            Public Sub RemoveAt(index As Integer) Implements IList(Of IPayrollInfo).RemoveAt
                _list.RemoveAt(index)
            End Sub

            Public Sub Add(item As IPayrollInfo) Implements ICollection(Of IPayrollInfo).Add
                _list.Add(item)
            End Sub

            Public Sub Clear() Implements ICollection(Of IPayrollInfo).Clear
                _list.Clear()
            End Sub

            Public Sub CopyTo(array() As IPayrollInfo, arrayIndex As Integer) Implements ICollection(Of IPayrollInfo).CopyTo
                Throw New NotImplementedException()
            End Sub


            Public Function IndexOf(item As IPayrollInfo) As Integer Implements IList(Of IPayrollInfo).IndexOf
                Throw New NotImplementedException()
            End Function

            Public Function Contains(item As IPayrollInfo) As Boolean Implements ICollection(Of IPayrollInfo).Contains
                Throw New NotImplementedException()
            End Function

            Public Function Remove(item As IPayrollInfo) As Boolean Implements ICollection(Of IPayrollInfo).Remove
                Throw New NotImplementedException()
            End Function

            Public Function GetEnumerator() As IEnumerator(Of IPayrollInfo) Implements IEnumerable(Of IPayrollInfo).GetEnumerator
                Return _list.GetEnumerator()
            End Function

            Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
                Throw New NotImplementedException()
            End Function


#End Region

            Public Function GetPayrollDate(nSheet As ISheet) As String Implements IPayregInfo.GetPayrollDate
                If nSheet.GetRow(3) Is Nothing Then
                    MsgBox("Cannot Find Payroll Date")
                    Return Nothing
                End If
                Return nSheet.GetRow(3).GetCell(1).StringCellValue.Trim.Replace("*", "").Trim
            End Function

            Public Function CreatePayroll() As IPayrollInfo Implements IPayregInfo.CreatePayroll
                Return New PayrollInfo
            End Function

            Public Function CreatePayreg() As IPayregInfo Implements IPayregInfo.CreatePayreg
                Return New PayregInfo
            End Function

            Public Sub PopulatePayslips(nSheet As ISheet) Implements IPayregInfo.PopulatePayslips
                Dim pageIdx As Integer = 0

                Dim pageItemIdx As Integer = 0
                LayoutPage(pageIdx, nSheet)
                For idx As Integer = 0 To Count - 1
                    _list(idx).PastePayslip(nSheet, pageIdx, pageItemIdx)

                    If pageItemIdx = 3 Then
                        pageIdx += 1
                        LayoutPage(pageIdx, nSheet)
                        pageItemIdx = 0
                    Else
                        pageItemIdx += 1
                    End If
                Next
            End Sub

            Public Function Find(id As String) As IPayrollInfo Implements IPayregInfo.Find
                For Each p As PayrollInfo In Me
                    If p.Employee_Id = id Then
                        Return p
                    End If
                Next
                Return Nothing
            End Function

            Public Function SyncToSheet(nSheet As ISheet) As Boolean Implements IPayregInfo.SyncToSheet
                For j As Integer = 2 To nSheet.LastRowNum
                    Dim nRow As IRow = nSheet.GetRow(j)
                    If nRow IsNot Nothing AndAlso nRow.GetCell(0) IsNot Nothing Then
                        Dim employeeDetail As String() = GetEmployeeDetail(nRow)
                        Find(employeeDetail(1)).WriteGovernment(nRow)
                    End If
                Next
            End Function

            Public Function GetPayroll() Implements IPayregInfo.GetPayroll
                Dim sourceFileInfo As New IO.FileInfo(PayregPath)

                '     Dim payrolls As New List(Of IPayrollInfo)
                Dim nWorkBook_Payreg As IWorkbook
                Using nNewPayreg As IO.FileStream = New FileStream(sourceFileInfo.FullName, FileMode.Open, FileAccess.Read)
                    nWorkBook_Payreg = New HSSFWorkbook(nNewPayreg)
                End Using

                Dim nSheet As ISheet = nWorkBook_Payreg.GetSheetAt(0)
                'Get Payroll Date 

                Dim rawPayrollDate As String = GetPayrollDate(nSheet) 'nSheet.GetRow(3).GetCell(1).StringCellValue.Trim.Replace("*", "").Trim

                For j As Integer = 4 To nSheet.LastRowNum
                    Dim nRow As IRow = nSheet.GetRow(j)
                    If nRow IsNot Nothing AndAlso nRow.GetCell(1) IsNot Nothing Then
                        Dim newPayroll As New PayrollInfo()
                        With newPayroll
                            .PayrollDate = rawPayrollDate
                            If .GetPayrollDetail(nRow) Is Nothing Then Continue For
                        End With
                        Add(newPayroll)
                    End If
                Next

                Return Me
            End Function

            Public Shared Function GetEmployeeDetail(row As IRow) As String()
                Dim fullname_raw As String() = row.GetCell(1).StringCellValue.Trim(")").Split("(")
                If fullname_raw.Length < 2 Then Return Nothing

                Return {fullname_raw(0).Trim, fullname_raw(1).Trim}
            End Function
        End Class

    End Namespace
End Namespace