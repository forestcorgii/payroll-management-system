Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports payroll_module

Public Class FilterBinding
    Implements INotifyPropertyChanged
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Private _filter As String

    Public Property Filter As String
        Get
            Return _filter
        End Get
        Set(value As String)
            _filter = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Filter"))
        End Set
    End Property

End Class