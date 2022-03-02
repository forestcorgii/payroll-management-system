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
End Class
