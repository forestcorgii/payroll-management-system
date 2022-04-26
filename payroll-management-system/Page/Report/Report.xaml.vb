Class Report
    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub Report_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        frmAlphalist.Navigate(New BankReport)
        btnBankReport.IsChecked = True
    End Sub

    Private Sub btn13thMonth_Click(sender As Object, e As RoutedEventArgs) Handles btn13thMonth.Checked
        'frmAlphalist.Navigate(New ProcessPayreg)
    End Sub

    Private Sub btnBankReport_Click(sender As Object, e As RoutedEventArgs) Handles btnBankReport.Checked
        frmAlphalist.Navigate(New BankReport)
    End Sub

End Class
