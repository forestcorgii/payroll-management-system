Imports System.ComponentModel
Imports payroll_module.Payroll

Public Class AdjustmentRecordBinding
    Implements INotifyPropertyChanged
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Private _filter As String
    Private _PayrollCode As String
    Private _AdjustmentRecords As List(Of AdjustmentRecordModel)
    Private _PayrollCodes As List(Of String)

    Public Property Filter As String
        Get
            Return _filter
        End Get
        Set(value As String)
            _filter = value
            RefreshItems()
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Filter"))
        End Set
    End Property

    Public Property PayrollCode As String
        Get
            Return _PayrollCode
        End Get
        Set(value As String)
            _PayrollCode = value
            RefreshItems()
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("PayrollCode"))
        End Set
    End Property

    Public Property PayrollCodes As List(Of String)
        Get
            Return _PayrollCodes
        End Get
        Set(value As List(Of String))
            _PayrollCodes = value
            RefreshItems()
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("PayrollCodes"))
        End Set
    End Property

    Public Property AdjustmentRecords As List(Of AdjustmentRecordModel)
        Get
            Return _AdjustmentRecords
        End Get
        Set(value As List(Of AdjustmentRecordModel))
            _AdjustmentRecords = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("AdjustmentRecords"))
        End Set
    End Property

    Private Sub RefreshItems()
        DatabaseManager.Connection.Open()
        AdjustmentRecords = AdjustmentRecordGateway.Filter(DatabaseManager, _filter, _PayrollCode)
        DatabaseManager.Connection.Close()
    End Sub

End Class