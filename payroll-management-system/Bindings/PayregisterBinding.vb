Imports System.ComponentModel
Imports payroll_module.Payroll

Public Class PayregisterBinding
    Implements INotifyPropertyChanged
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Private _payrolls As New List(Of PayrollModel)
    Public Property Payrolls As List(Of PayrollModel)
        Get
            Return _payrolls
        End Get
        Set(value As List(Of PayrollModel))
            TotalAmount = PayRegisterController.GetTotalAmount(value)
            TotalEE = value.Count
            TotalUCPB = PayRegisterController.GetBankEECount(value, "UCPB", "CCARD")

            TotalChinaBank = 0
            TotalChinaBank += PayRegisterController.GetBankEECount(value, "CHINABANK", "CCARD")

            TotalCheck = 0
            TotalCheck += PayRegisterController.GetBankEECount(value, "UCPB", "CHK")
            TotalCheck += PayRegisterController.GetBankEECount(value, "CHINABANK", "CHK")
            TotalCheck += PayRegisterController.GetBankEECount(value, "", "CHK")

            _payrolls = value
        End Set
    End Property

    Private _payrollCodes As New List(Of String)
    Public Property PayrollCodes As List(Of String)
        Get
            Return _payrollCodes
        End Get
        Set(value As List(Of String))
            PayrollCodesAll = String.Join("-", value.ToArray)
            _payrollCodes = value
        End Set
    End Property

    Private _PayrollCodesAll As String
    Public Property PayrollCodesAll As String
        Get
            Return _PayrollCodesAll
        End Get
        Set(value As String)
            _PayrollCodesAll = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("PayrollCodesAll"))
        End Set
    End Property

    Private _TotalEE As Integer
    Public Property TotalEE As Integer
        Get
            Return _TotalEE
        End Get
        Set(value As Integer)
            _TotalEE = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TotalEE"))
        End Set
    End Property

    Private _TotalAmount As Double
    Public Property TotalAmount As Double
        Get
            Return _TotalAmount
        End Get
        Set(value As Double)
            _TotalAmount = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TotalAmount"))
        End Set
    End Property


    Private _TotalUCPB As Integer
    Public Property TotalUCPB As Integer
        Get
            Return _TotalUCPB
        End Get
        Set(value As Integer)
            _TotalUCPB = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TotalUCPB"))
        End Set
    End Property


    Private _TotalChinaBank As Integer
    Public Property TotalChinaBank As Integer
        Get
            Return _TotalChinaBank
        End Get
        Set(value As Integer)
            _TotalChinaBank = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TotalChinaBank"))
        End Set
    End Property


    Private _TotalCheck As Integer
    Public Property TotalCheck As Integer
        Get
            Return _TotalCheck
        End Get
        Set(value As Integer)
            _TotalCheck = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TotalCheck"))
        End Set
    End Property


    Private _TotalMetroBank_Palo As Integer
    Public Property TotalMetroBank_Palo As Integer
        Get
            Return _TotalMetroBank_Palo
        End Get
        Set(value As Integer)
            _TotalMetroBank_Palo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TotalMetroBank_Palo"))
        End Set
    End Property


    Private _TotalMetroBank_Tac As Integer
    Public Property TotalMetroBank_Tac As Integer
        Get
            Return _TotalMetroBank_Tac
        End Get
        Set(value As Integer)
            _TotalMetroBank_Tac = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TotalMetroBank_Tac"))
        End Set
    End Property


End Class