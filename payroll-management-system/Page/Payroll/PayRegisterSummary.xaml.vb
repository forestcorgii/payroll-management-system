Imports System.Collections.ObjectModel

Class PayRegisterSummary
    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub PayRegisterSummary_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        DatabaseManager.Connection.Open()
        Dim summaries As New ObservableCollection(Of payroll_module.Payroll.PayRegisterModel)(payroll_module.Payroll.PayRegisterGateway.Collect(DatabaseManager))
        DatabaseManager.Connection.Close()

        lstPayRegisters.ItemsSource = summaries
    End Sub

    Private Sub btnUploadPayreg_Click(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub btnStartProcess_Click(sender As Object, e As RoutedEventArgs)

    End Sub
End Class
