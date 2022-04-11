Namespace Payroll
    Public Class PayrollController

        Public Shared Function GetCutoffRange(payrollDate As Date) As Date()
            If {28, 29, 30}.Contains(payrollDate.Day) Then
                Return {New Date(payrollDate.Year, payrollDate.Month, 5), New Date(payrollDate.Year, payrollDate.Month, 19)}
            ElseIf 15 = (payrollDate.Day) Then
                Dim previousMonth As Date = payrollDate.AddMonths(-1)
                Return {New Date(previousMonth.Year, previousMonth.Month, 20), New Date(payrollDate.Year, payrollDate.Month, 4)}
                'Return {New Date(previousMonth.Year, previousMonth.Month, 20), New Date(payrollDate.Year, payrollDate.Month, 11)}
            End If
            Return Nothing
        End Function

        Public Shared Function GetPreviousPayrollDate(currentPayrollDate As Date) As Date
            Dim previousPayrollDate As Date
            If {28, 29, 30}.Contains(currentPayrollDate.Day) Then
                previousPayrollDate = String.Format("{0}-{1:00}-15", currentPayrollDate.Year, currentPayrollDate.Month)
            ElseIf 15 = (currentPayrollDate.Day) Then
                Dim day As String = "30"
                Dim _date = currentPayrollDate.AddMonths(-1)
                If _date.Month = 2 Then
                    day = IIf(Date.IsLeapYear(_date.Year), 29, 28)
                End If

                previousPayrollDate = String.Format("{0}-{1:00}-{2}", _date.Year, _date.Month, day)
            End If

            Return Date.Parse(previousPayrollDate)
        End Function
    End Class

End Namespace
