Imports System.IO
Imports employee_module
Imports MySql.Data.MySqlClient
Imports NPOI.HSSF.UserModel
Imports NPOI.SS.UserModel
Imports utility_service

Namespace Payroll
    Public Class AdjustmentRecordController

        Public Shared Function CollectOrGenerateBillings(databaseManager As Manager.Mysql, EE_Id As String, payroll_name As String) As List(Of AdjustmentBillingModel)
            Dim billings As New List(Of AdjustmentBillingModel)
            Try
                billings = AdjustmentBillingGateway.Collect(databaseManager, payroll_name)
                If billings.Count = 0 Then
                    Dim activeRecords As List(Of AdjustmentRecordModel) = AdjustmentRecordGateway.Filter(databaseManager, EE_Id, True)
                    If activeRecords.Count > 0 Then
                        For Each record As AdjustmentRecordModel In activeRecords
                            Dim newBilling As New AdjustmentBillingModel
                            newBilling.EE_Id = EE_Id
                            newBilling.Adjustment_Name = record.Adjustment_Name
                            newBilling.Payroll_Name = payroll_name
                            'newBilling.Record_Name = record.Record_Name
                            newBilling.Amount = record.Monthly_Deduction
                            newBilling.Adjustment_Type = record.Adjustment_Type

                            billings.Add(AdjustmentBillingGateway.Save(databaseManager, newBilling))
                        Next
                    End If
                End If
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Return billings
        End Function

        Public Shared Function ImportRecords(databaseManager As Manager.Mysql, filePath As String) As List(Of AdjustmentRecordModel)
            Dim records As New List(Of AdjustmentRecordModel)

            Dim nWorkBook As IWorkbook
            Using nNewPayreg As IO.FileStream = New FileStream(filePath, FileMode.Open, FileAccess.Read)
                nWorkBook = New HSSFWorkbook(nNewPayreg)
            End Using

            Dim nSheet As ISheet = nWorkBook.GetSheetAt(0)
            Dim nRow As IRow = nSheet.GetRow(0)
            Dim rIdx As Integer = 1
            Try
                While nRow IsNot Nothing OrElse nRow.GetCell(rIdx) IsNot Nothing
                    Try
                        nRow = nSheet.GetRow(rIdx)
                        If nRow Is Nothing Then Exit While

                        Dim newRecord As New AdjustmentRecordModel
                        newRecord.Adjustment_Type = AdjustmentChoices.ADJUST2
                        newRecord.Request_Type = ParseRequestType(nRow.GetCell(0))
                        newRecord.Adjustment_Name = nRow.GetCell(1).StringCellValue
                        'if EE ID is nothing, get EE ID using the fullname
                        If nRow.GetCell(2) IsNot Nothing AndAlso nRow.GetCell(2).StringCellValue <> "" Then
                            newRecord.EE_Id = nRow.GetCell(2).StringCellValue
                        Else
                            newRecord.EE_Id = FindEEId(databaseManager, nRow.GetCell(3))
                            If newRecord.EE_Id <> "" Then
                                nRow.CreateCell(2).SetCellValue(newRecord.EE_Id)
                            Else
                                nRow.CreateCell(8).SetCellValue("Unknown Employee Name")
                                rIdx += 1
                                Continue While
                            End If
                        End If
                        newRecord.Total_Advances = nRow.GetCell(4).NumericCellValue
                        newRecord.Date_Effective = nRow.GetCell(5).DateCellValue
                        newRecord.Monthly_Deduction = -nRow.GetCell(6).NumericCellValue
                        newRecord.Remaining_Balance = nRow.GetCell(7).NumericCellValue
                        records.Add(AdjustmentRecordGateway.Save(databaseManager, newRecord))
                        rIdx += 1
                        nRow.CreateCell(8).SetCellValue("OKAY")
                        nRow.CreateCell(10).SetCellValue("")
                    Catch ex As Exception
                        nRow.CreateCell(8).SetCellValue("ERROR!")
                        nRow.CreateCell(10).SetCellValue(ex.Message)
                        Console.WriteLine(ex.Message)
                        rIdx += 1
                    End Try
                End While

                Using nImportFile As FileStream = New FileStream(filePath, FileMode.Open, FileAccess.Write)
                    nWorkBook.Write(nImportFile)
                End Using
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Return records
        End Function

        Private Shared Function FindEEId(databaseManager As Manager.Mysql, cell As ICell) As String
            If cell IsNot Nothing Then
                Dim name As String = cell.StringCellValue
                Dim name_args As String() = name.Split(",")
                Dim lastname As String = name_args(0).Trim.Split(" ").First
                Dim firstname_args As String() = name_args(1).Trim.Split(" ")

                Dim middlename As String = firstname_args.Last
                Dim firstname As String = firstname_args.First

                Dim ee As EmployeeModel = EmployeeGateway.Find(databaseManager, lastname, firstname, middlename)
                If ee IsNot Nothing Then
                    Return ee.EE_Id
                Else
                    Console.WriteLine("No User Found.")
                End If
            Else
                Throw New Exception("Name Field is also empty.")
            End If
            Return ""
        End Function

        Private Shared Function ParseRequestType(cell As ICell) As RequestTypeChoices
            If cell IsNot Nothing Then
                Select Case cell.StringCellValue
                    Case "REQ"
                        Return RequestTypeChoices.BY_REQUEST
                    Case "30TH"
                        Return RequestTypeChoices.ONLY_30TH
                    Case ""
                        Return RequestTypeChoices.NORMAL
                    Case Else
                        Console.WriteLine("Unknown Request Type: " & cell.StringCellValue)
                        Return RequestTypeChoices.NORMAL
                End Select
            End If

            Return RequestTypeChoices.NORMAL
        End Function

    End Class
End Namespace
