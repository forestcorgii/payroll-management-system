Imports NPOI.SS.UserModel

Namespace Model

    Namespace Perday

        Public Class PayregInfo
            Implements IPayregInfo

            Public Property PayregPath As String Implements IPayregInfo.PayregPath
                Get
                    Throw New NotImplementedException()
                End Get
                Set(value As String)
                    Throw New NotImplementedException()
                End Set
            End Property

            Default Public Property Item(index As Integer) As IPayrollInfo Implements IList(Of IPayrollInfo).Item
                Get
                    Throw New NotImplementedException()
                End Get
                Set(value As IPayrollInfo)
                    Throw New NotImplementedException()
                End Set
            End Property

            Public ReadOnly Property Count As Integer Implements ICollection(Of IPayrollInfo).Count
                Get
                    Throw New NotImplementedException()
                End Get
            End Property

            Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of IPayrollInfo).IsReadOnly
                Get
                    Throw New NotImplementedException()
                End Get
            End Property

            Public Sub PopulatePayslips(nSheet As ISheet) Implements IPayregInfo.PopulatePayslips
                Throw New NotImplementedException()
            End Sub


            Public Sub Insert(index As Integer, item As IPayrollInfo) Implements IList(Of IPayrollInfo).Insert
                Throw New NotImplementedException()
            End Sub

            Public Sub RemoveAt(index As Integer) Implements IList(Of IPayrollInfo).RemoveAt
                Throw New NotImplementedException()
            End Sub

            Public Sub Add(item As IPayrollInfo) Implements ICollection(Of IPayrollInfo).Add
                Throw New NotImplementedException()
            End Sub

            Public Sub Clear() Implements ICollection(Of IPayrollInfo).Clear
                Throw New NotImplementedException()
            End Sub

            Public Sub CopyTo(array() As IPayrollInfo, arrayIndex As Integer) Implements ICollection(Of IPayrollInfo).CopyTo
                Throw New NotImplementedException()
            End Sub

            Public Function GetPayrollDate(nSheet As ISheet) As String Implements IPayregInfo.GetPayrollDate
                Dim rawPayrollDate As String = nSheet.GetRow(3).GetCell(1).StringCellValue
                Return rawPayrollDate.Split(":")(1).Trim
            End Function

            Public Function CreatePayroll() As IPayrollInfo Implements IPayregInfo.CreatePayroll
                Return New PayrollInfo
            End Function

            Public Function Find(id As String) As IPayrollInfo Implements IPayregInfo.Find
                Throw New NotImplementedException()
            End Function

            Public Function SyncToSheet(nSheet As ISheet) As Boolean Implements IPayregInfo.SyncToSheet
                Throw New NotImplementedException()
            End Function

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
                Throw New NotImplementedException()
            End Function

            Public Function GetPayroll() Implements IPayregInfo.GetPayroll
                Throw New NotImplementedException()
            End Function

            Public Function CreatePayreg() As IPayregInfo Implements IPayregInfo.CreatePayreg
                Throw New NotImplementedException()
            End Function

            Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
                Throw New NotImplementedException()
            End Function

        End Class
    End Namespace
End Namespace
