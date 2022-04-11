Imports System.ComponentModel
Imports System.IO
Imports System.Windows.Forms
Imports NPOI.HSSF.UserModel
Imports NPOI.SS.UserModel
Imports payroll_module.Payroll
Class PayrollTimeComparer
    Private Sub btnRun_Click(sender As Object, e As RoutedEventArgs)
        Dim emps As New List(Of timesheetmodel)
        If tbFilePath.Text <> "" Then
            Dim nWorkbook As New HSSFWorkbook()
            Using nEFile As IO.FileStream = New FileStream(tbFilePath.Text, FileMode.OpenOrCreate, FileAccess.Read)
                nWorkbook = New HSSFWorkbook(nEFile)
            End Using

            Dim nSheet As ISheet = nWorkbook.GetSheetAt(0)
            DatabaseManager.Connection.Open()
            Try
                For j As Integer = 4 To nSheet.LastRowNum
                    Dim nRow As IRow = nSheet.GetRow(j)
                    If nRow IsNot Nothing AndAlso nRow.GetCell(1) IsNot Nothing Then
                        Dim ee_id As String = nRow.GetCell(1).StringCellValue.Trim
                        Dim newPayroll As TimesheetModel = TimesheetGateway.Find(DatabaseManager, ee_id & Date.Parse(dtPayrollDate.Text).ToString("_yyyyMMdd"), True)
                        If newPayroll IsNot Nothing Then
                            emps.Add(newPayroll)
                        Else
                            Dim payrollTime As New TimesheetModel With {.EE_Id = ee_id}
                            TimesheetController.CompleteDetail(DatabaseManager, payrollTime)
                            payrollTime.Remarks = "NO TIMESHEET FOUND;"
                            emps.Add(payrollTime)
                        End If
                    End If
                Next
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try
            DatabaseManager.Connection.Close()

        End If

        lstPayrollTimes.ItemsSource = emps
    End Sub

    Private Sub btnBrowse_Click(sender As Object, e As RoutedEventArgs)
        Using openFile As New OpenFileDialog
            If openFile.ShowDialog = DialogResult.OK Then
                tbFilePath.Text = openFile.FileName
            End If
        End Using
    End Sub
End Class
