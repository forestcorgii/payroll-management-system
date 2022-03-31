﻿Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports payroll_service
Imports System.Windows.Forms

Class EmployeePage

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Employees As New ObservableCollection(Of Employee.Model)
    Private Sub EmployeePage_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        DatabaseManager.Connection.Open()
        Employees = New ObservableCollection(Of Employee.Model)(Employee.Gateway.Collect(DatabaseManager))
        DatabaseManager.Connection.Close()

        lstEmployees.ItemsSource = Employees
    End Sub

    Private Sub lstEmployees_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles lstEmployees.SelectionChanged
        If lstEmployees.SelectedItem IsNot Nothing Then Fill(lstEmployees.SelectedItem)

    End Sub

    Public Sub Fill(selectedItem As Employee.Model)
        With selectedItem
            tbEEId.Text = .EE_Id
            tbLastName.Text = .Last_Name
            tbFirstName.Text = .First_Name
            tbMiddleName.Text = .Middle_Name
            tbTIN.Text = .TIN
            tbPayrollCode.Text = .Payroll_Code
            tbBankCategory.Text = .Bank_Category
            tbBankName.Text = .Bank_Name
            tbAccountNumber.Text = .Account_Number
            tbCardNumber.Text = .Card_Number
        End With

    End Sub



    Private Sub btnSync_Click(sender As Object, e As RoutedEventArgs)
        bgSyncAll.RunWorkerAsync()
    End Sub



    Public WithEvents bgSyncAll As New BackgroundWorker
    Private Async Sub bgSyncAll_DoWork(sender As Object, e As DoWorkEventArgs) Handles bgSyncAll.DoWork
        Try
            DatabaseManager.Connection.Open()
            Dim employees As List(Of Employee.Model) = Employee.Gateway.Collect(DatabaseManager)
            If employees.Count > 0 Then
                Dispatcher.Invoke(Sub()
                                      pb.Value = 0
                                      pb.Maximum = employees.Count
                                  End Sub)
                Dim i As Integer = 0
                While i < employees.Count
                    Dim _employee As Employee.Model = employees(i)
                    Try
                        i += 1
                        Await Employee.Gateway.SyncEmployeeFromHRMS(DatabaseManager, HRMSAPIManager, _employee.EE_Id, _employee)
                        Dispatcher.Invoke(Sub()
                                              pb.Value += 1
                                              lbStatus.Text = String.Format("Found {0} Employees... Syncing {1}", employees.Count, _employee.EE_Id)
                                          End Sub)
                    Catch ex As Exception
                        If ex.Message = "Employee not found in HRMS." Then
                            'archive employee
                        End If
                        Console.WriteLine(ex.Message)
                    End Try
                End While
            End If
            DatabaseManager.Connection.Close()
        Catch ex As Exception
            DatabaseManager.Connection.Close()
            MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub



End Class