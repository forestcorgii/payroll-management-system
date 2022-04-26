Imports System.ComponentModel
Imports payroll_module.Payroll

Public Class AdjustmentBillingBinding
    Implements INotifyPropertyChanged
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Private _filter As String
    Private _PayrollCode As String
    Private _PayrollDate As Date
    Private _AdjustmentBillings As List(Of AdjustmentBillingModel)
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

    Public Property PayrollDate As Date
        Get
            Return _PayrollDate
        End Get
        Set(value As Date)
            _PayrollDate = value
            RefreshItems()
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("PayrollDate"))
        End Set
    End Property

    Public Property AdjustmentBillings As List(Of AdjustmentBillingModel)
        Get
            Return _AdjustmentBillings
        End Get
        Set(value As List(Of AdjustmentBillingModel))
            _AdjustmentBillings = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("AdjustmentBillings"))
        End Set
    End Property

    Private Sub RefreshItems()
        DatabaseManager.Connection.Open()
        AdjustmentBillings = AdjustmentBillingGateway.Filter(DatabaseManager, _filter, _PayrollCode, _PayrollDate)
        DatabaseManager.Connection.Close()
    End Sub

End Class