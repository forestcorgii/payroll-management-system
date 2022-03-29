
Imports System.ComponentModel
Imports Newtonsoft.Json
Imports payroll_service
Imports utility_service

Class ProcessPayreg
    Private Sub lstPayreg_DragEnter(sender As Object, e As DragEventArgs)
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effects = DragDropEffects.Copy
        End If
    End Sub

    Private Sub lstPayreg_Drop(sender As Object, e As DragEventArgs)
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            Dim paths() As String
            paths = e.Data.GetData(DataFormats.FileDrop)

            For Each p As String In paths
                lstPayreg.Items.Add(p)
            Next
        End If
    End Sub

    Private Sub btnStartProcess_Click(sender As Object, e As RoutedEventArgs)
        If bgProcess.IsBusy = False Then
            bgProcess.RunWorkerAsync(lstPayreg.Items)
        End If
    End Sub

    Private Sub lstPayreg_KeyDown(sender As Object, e As KeyEventArgs)
        If e.Key = Key.Delete Then
            For i As Integer = lstPayreg.SelectedItems.Count - 1 To 0 Step -1
                lstPayreg.Items.RemoveAt(lstPayreg.Items.IndexOf(lstPayreg.SelectedItems(i)))
            Next
        End If
    End Sub

    Public WithEvents bgProcess As New BackgroundWorker
    Private Sub bgProcess_DoWork(sender As Object, e As DoWorkEventArgs) Handles bgProcess.DoWork
        Dim _databaseManager As New Manager.Mysql(DatabaseConfiguration)
        _databaseManager.Connection.Open()
        Try
            Dim payRegisters As ItemCollection = e.Argument
            Dispatcher.Invoke(Sub()
                                  pb.Maximum = payRegisters.Count
                                  pb.Value = 0
                              End Sub)
            For i As Integer = 0 To payRegisters.Count - 1
                Controller.PayRegister.ProcessPayRegister(_databaseManager, payRegisters(i))
                Dispatcher.Invoke(Sub() pb.Value += 1)
            Next
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
        _databaseManager.Connection.Close()
    End Sub


End Class
