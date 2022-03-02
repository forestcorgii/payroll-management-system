Namespace Model
    Public Class PayrollRecord
        Public _idNo As String = ""
        Public _amount As String = "0.0"
        Public _accountNumber As String = ""
        Public _accountType As String = "SA"
        Public _fullName As String = ""
        Public _firstName As String = ""
        Public _lastName As String = ""
        Public _middleName As String = ""
        Public _extension As String = ""
        Public _bankName As String = ""
        Public _regPay As String = ""
        Public _regHrs As String = ""
        Public _rate As String = ""
        Public _withOT As String = ""

        Public ReadOnly Property Fullname As String
            Get
                Dim _fullname As String = _firstName
                If Trim(_middleName).Length > 0 Then
                    _fullname = _fullname & " " & _middleName.Substring(0, 1) & "."
                End If
                _fullname = _fullname & " " & _lastName
                If Trim(_extension).Length > 0 Then
                    _fullname = _fullname & " " & _extension
                End If

                Return _fullname.ToUpper
            End Get
        End Property
    End Class

    Public Class NewRecord
        Public _idNo As String = ""
        Public _accountNumber As String = ""
        Public _fullName As String = ""
        Public _bankName As String = ""
        Public _payrollCode As String = ""
        Public _remarks As String = ""
        Public _lastReported As String = ""
    End Class

    Public Class ChangesRecord
        Public _idNo As String = ""
        Public _fullName As String = ""
        Public _bankName As String = ""
        Public _payrollCode As String = ""
        Public _remarks As String = ""
    End Class

    Public Class RecordCompare
        Public _idNo As String = ""
        Public _nameInHR As String = ""
        Public _nameInCashCardDatabase As String = ""
        Public _site As String = ""
        Public _bank As String = ""
        Public _hrFilename As String = ""
    End Class

    Public Class manHoursCompare
        Public _idNo As String = ""
        Public _fullname As String = ""
        Public _HrManHour As String = ""
        Public _PayRegManHour As String = ""
        Public _site As String = ""
        Public _bank As String = ""
        Public _hrFilename As String = ""
    End Class

    Public Class MnameRecord
        Public _idNo As String = ""
        Public _lastName As String = ""
        Public _firstName As String = ""
        Public _middleName As String = ""
        Public _ext As String = ""
    End Class
End Namespace
