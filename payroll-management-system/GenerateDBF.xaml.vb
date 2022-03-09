Imports utility_service

Class GenerateDBF
    Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub btnSettings_Click(sender As Object, e As RoutedEventArgs) Handles btnSettings.Click

    End Sub

    Private Sub btnGenerateDBF_Click(sender As Object, e As RoutedEventArgs)
        Dim payDay As Integer = Date.Parse(dtPayrollDate.SelectedDate).Day
        Dim payMonth As Integer = Date.Parse(dtPayrollDate.SelectedDate).Month
        If {15, 30}.Contains(payDay) Or
            ({28, 29}.Contains(payDay) And payMonth = 2) Or
            (payDay = 13 And payMonth = 12) Then

            Dim startupPath As String = String.Format("{0}/DBF", AppDomain.CurrentDomain.BaseDirectory)
            IO.Directory.CreateDirectory(startupPath)
            For Each pp As String In {"P11A", "P10A", "P7A", "P1A", ""}
                payroll_service.DBF.SavePayrollTimeToDBF(DatabaseManager, dtPayrollDate.SelectedDate, pp.ToUpper, startupPath, IIf(payDay = 15, 1, 2))
            Next
        End If
    End Sub
End Class
