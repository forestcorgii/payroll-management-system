Class MainWindow

    '    CD/P7A_FINAL
    'USE PAY7A-CASHCARD-01302022
    'COPY TO DETAIL
    'USE DETAIL
    '    Do IDCHECK
    'REINDEX
    'Do IDCHECK
    'Do COMPUTE WITH 2,1
    'Do ALL WITH "30 JANUARY 2021",2,2
    'USE
    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        frmMain.Navigate(New ProcessPayreg)
    End Sub
End Class
