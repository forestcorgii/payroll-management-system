

Namespace Payroll

    Public Class PayRegisterController
        Public Shared Function GetTotalAmount(payrolls As List(Of PayrollModel)) As Double
            Dim amount As Double = 0
            For i As Integer = 0 To payrolls.Count - 1
                amount += payrolls(i).Gross_Pay
            Next
            Return amount
        End Function

        Public Shared Function GetBankEECount(payrolls As List(Of PayrollModel), bankName As String, bankCategory As String) As Integer
            Dim count As Integer = 0
            For i As Integer = 0 To payrolls.Count - 1
                If bankName = payrolls(i).Bank_Name And bankCategory = payrolls(i).Bank_Category Then
                    count += 1
                End If
            Next
            Return count
        End Function

    End Class
End Namespace
