Imports MySql.Data.MySqlClient

Namespace Payroll

    Public Class AdjustmentRecordModel
        Public Property Record_Name As String

        Public Property EE_Id As String
        Public Property Name As String

        Public Property Date_Effective As Date
        Public Property Date_Expires As Date

        Public Property Monthly_Deduction As Double
        Public Property Total_Balance As Double
        Public Property Is_Active As Boolean

        Public Property Adjust_Type As AdjustmentChoices

        Public Date_Created As Date
        Public Date_Modified As Date

        Sub New()
        End Sub

        Sub New(reader As MySqlDataReader)
            EE_Id = reader("ee_id")
            Name = reader("name")
            Date_Effective = reader("date_effective")
            Date_Expires = reader("date_expires")
            Monthly_Deduction = reader("monthly_deduction")
            Total_Balance = reader("total_balance")
            Is_Active = reader("is_active")
            Adjust_Type = reader("adjust_type")
            Date_Created = reader("date_created")
            Date_Modified = reader("date_modified")
        End Sub

        Public ReadOnly Property GetRecordName As String
            Get
                Return String.Format("{0}_{1}_{2:yyMM}", EE_Id, Name, Date_Effective)
            End Get
        End Property
    End Class

    Public Enum AdjustmentChoices
        ADJUST1 = 1
        ADJUST2 = 2
    End Enum

End Namespace
