Imports System.ComponentModel

Public Class TimeDownloader

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub dtPayrollDate_SelectedDateChanged(sender As Object, e As SelectionChangedEventArgs)

    End Sub

    Private Sub btnDownload_Click(sender As Object, e As RoutedEventArgs)

    End Sub


    Private WithEvents bgProcessor As BackgroundWorker
    Private Sub bgProcessor_DoWork(sender As Object, e As DoWorkEventArgs) Handles bgProcessor.DoWork
        'GET TOTAL PAGE

        'RUN THROUGH PAGES


    End Sub

End Class
