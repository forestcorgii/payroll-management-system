
Imports System.ComponentModel
Imports System.Windows.Forms
Imports employee_module
Imports Newtonsoft.Json
Imports payroll_module
Imports payroll_module.Payroll
Imports utility_service

Class ProcessPayreg
    Sub New(payreg As PayRegisterModel)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        dtPayrollDate.SelectedDate = DefaultPayrollDate
        cbPayrollCode.SelectedItem = DefaultPayrollCode
    End Sub
    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        dtPayrollDate.SelectedDate = DefaultPayrollDate
        cbPayrollCode.SelectedItem = DefaultPayrollCode
    End Sub

    Private Sub ProcessPayreg_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        DatabaseManager.Connection.Open()
        cbPayrollCode.ItemsSource = EmployeeGateway.CollectPayrollCodes(DatabaseManager)
        DatabaseManager.Connection.Close()

    End Sub

    Private Sub btnStartProcess_Click(sender As Object, e As RoutedEventArgs)
        If bgProcess.IsBusy = False Then
            'bgProcess.RunWorkerAsync(lstPayreg.Items)
        End If
    End Sub

    Public WithEvents bgProcess As New BackgroundWorker
    Private Sub bgProcess_DoWork(sender As Object, e As DoWorkEventArgs) Handles bgProcess.DoWork
        Dim _databaseManager As New Manager.Mysql(DatabaseConfiguration)
        _databaseManager.Connection.Open()
        Try
            Dim payRegisters As String() = e.Argument
            Dispatcher.Invoke(Sub()
                                  pb.Maximum = payRegisters.Count
                                  pb.Value = 0
                              End Sub)
            For i As Integer = 0 To payRegisters.Count - 1
                PayRegisterController.ProcessPayRegister(_databaseManager, payRegisters(i), LoggingService)
                Dispatcher.Invoke(Sub() pb.Value += 1)
            Next
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
        _databaseManager.Connection.Close()
    End Sub

    Private Sub dtPayrollDate_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        Dim selectedDate As Date = dtPayrollDate.SelectedDate
        Dim selectedPayrollCode As String = cbPayrollCode.Text
        Try
            If sender IsNot Nothing Then
                If sender.name = "dtPayrollDate" Then
                    selectedDate = e.AddedItems.Item(0)
                ElseIf sender.name = "cbPayrollCode" Then
                    selectedPayrollCode = e.AddedItems.Item(0)
                End If
            End If

            If selectedPayrollCode = "" Or selectedDate = Nothing Then Exit Sub

            DatabaseManager.Connection.Open()
            lstPayrolls.ItemsSource = PayrollGateway.CollectPayrolls(DatabaseManager, selectedDate, selectedPayrollCode, True)
            DatabaseManager.Connection.Close()
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
    End Sub

    Private Sub btnUploadPayreg_Click(sender As Object, e As RoutedEventArgs)
        Using openFile As New OpenFileDialog
            openFile.Filter = "Pay Register Files(*.xls)|*.xls"
            openFile.Multiselect = True
            If openFile.ShowDialog = DialogResult.OK Then
                bgProcess.RunWorkerAsync(openFile.FileNames)
            End If
        End Using
    End Sub

    Private Sub btnExport_Click(sender As Object, e As RoutedEventArgs)

    End Sub
End Class
