
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


        DatabaseManager.Connection.Open()
        For i As Integer = 0 To lstPayreg.Items.Count - 1
            Controller.PayRegister.ProcessPayRegister(DatabaseManager, lstPayreg.Items(i))
        Next
        DatabaseManager.Connection.Close()
    End Sub

    Private Sub lstPayreg_KeyDown(sender As Object, e As KeyEventArgs)
        If e.Key = Key.Delete Then
            For i As Integer = lstPayreg.SelectedItems.Count - 1 To 0 Step -1
                lstPayreg.Items.RemoveAt(lstPayreg.Items.IndexOf(lstPayreg.SelectedItems(i)))
            Next
        End If
    End Sub
End Class
