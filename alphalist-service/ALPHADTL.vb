
Imports System.IO

Public Class ALPHADTL
    'Inherits EmployeeList
    Implements IComparable(Of ALPHADTL)

    Public id As String


    Public FORM_TYPE As String
    Public EMPLOYER_TIN As String 'from converter
    Public EMPLOYER_BRANCH_CODE As String 'from converter
    Public RETRN_PERIOD As String
    Public SCHEDULE_NUM As String 'from converter
    Public SEQUENCE_NUM As Integer
    Public REGISTERED_NAME As String
    Public FIRST_NAME As String 'from alpha
    Public LAST_NAME As String 'from alpha
    Public MIDDLE_NAME As String 'from alpha
    Public TIN As String 'from alpha
    Public BRANCH_CODE As String 'from converter
    Public EMPLOYMENT_FROM As String 'from alpha
    Public EMPLOYMENT_TO As String 'from alpha
    Public ATC_CODE As String
    Public STATUS_CODE As String
    Public REGION_NUM As String 'from converter (roman numeral)
    Public SUBS_FILING As String 'Y?
    Public EXMPN_CODE As String
    Public FACTOR_USED As Double '313 if D1 then blank
    Public ACTUAL_AMT_WTHLD As Double 'x.TaxDue
    Public INCOME_PAYMENT As Double
    Public PRES_TAXABLE_SALARIES As Double
    Public PRES_TAXABLE_13TH_MONTH As Double
    Public PRES_TAX_WTHLD As Double 'u.JanNov
    Public PRES_NONTAX_SALARIES As Double 'AJ Val(.NonTaxSalary).ToString("0.00")
    Public PRES_NONTAX_13TH_MONTH As Double 'AG.Nontax13th
    Public PREV_TAXABLE_SALARIES As Double
    Public PREV_TAXABLE_13TH_MONTH As Double
    Public PREV_TAX_WTHLD As Double
    Public PREV_NONTAX_SALARIES As Double
    Public PREV_NONTAX_13TH_MONTH As Double
    Public PRES_NONTAX_SSS_GSIS_OTH_CONT As Double 'AI.Benefits & i
    Public PREV_NONTAX_SSS_GSIS_OTH_CONT As Double
    Public TAX_RATE As Double
    Public OVER_WTHLD As Double
    Public AMT_WTHLD_DEC As Double 'v.Advances
    Public EXMPN_AMT As Double
    Public TAX_DUE As Double 't.TaxDue
    Public HEATH_PREMIUM As Double
    Public FRINGE_BENEFIT As Double
    Public MONETARY_VALUE As Double
    Public NET_TAXABLE_COMP_INCOME As Double 'lnos.TotalIncome blank if not taxdue
    Public GROSS_COMP_INCOME As Double 'f.GrossComp
    Public PREV_NONTAX_DE_MINIMIS As Double
    Public PREV_TOTAL_NONTAX_COMP_INCOME As Double
    Public PREV_TAXABLE_BASIC_SALARY As Double
    Public PRES_NONTAX_DE_MINIMIS As Double
    Public PRES_TAXABLE_BASIC_SALARY As Double 'lnos.TotalIncome
    Public PRES_TOTAL_COMP As Double 'f.GrossComp
    Public PREV_PRES_TOTAL_TAXABLE As Double
    Public PRES_TOTAL_NONTAX_COMP_INCOME As Double 'i
    Public PREV_NONTAX_GROSS_COMP_INCOME As Double
    Public PREV_NONTAX_BASIC_SMW As Double
    Public PREV_NONTAX_HOLIDAY_PAY As Double
    Public PREV_NONTAX_OVERTIME_PAY As Double
    Public PREV_NONTAX_NIGHT_DIFF As Double
    Public PREV_NONTAX_HAZARD_PAY As Double
    Public PRES_NONTAX_GROSS_COMP_INCOME As Double 'XGrossComp & grosscomp
    Public PRES_NONTAX_BASIC_SMW_DAY As Double 'Yperday
    Public PRES_NONTAX_BASIC_SMW_MONTH As Double 'Zpermonth PRES_NONT7
    Public PRES_NONTAX_BASIC_SMW_YEAR As Double 'AAperyear
    Public PRES_NONTAX_HOLIDAY_PAY As Double 'AC.holiday.ToString("0.00")
    Public PRES_NONTAX_OVERTIME_PAY As Double 'AD Val(.Overtime + .RD_OT).ToString("0.00")
    Public PRES_NONTAX_NIGHT_DIFF As Double 'AE Val(.NDiff).ToString("0.00")
    Public PREV_PRES_TOTAL_COMP_INCOME As Double
    Public PRES_NONTAX_HAZARD_PAY As Double
    Public TOTAL_NONTAX_COMP_INCOME As Double '??????
    Public TOTAL_TAXABLE_COMP_INCOME As Double 'lnos.TotalIncome
    Public PREV_TOTAL_TAXABLE As Double
    Public NONTAX_BASIC_SAL As Double 'AJ
    Public TAX_BASIC_SAL As Double
    Public QRT_NUM As Integer
    Public QUARTERDATE As String
    Public NATIONALITY As String
    Public REASON_SEPARATION As String
    Public EMPLOYMENT_STATUS As String
    Public ADDRESS1 As String
    Public ADDRESS2 As String
    Public ATC_DESC As String
    Public DATE_DEATH As String
    Public DATE_WTHELD As String
    'Public _NULLFLAGS

    Public December As String
    Public Final As String

    Public Overrides Function ToString() As String
        Return SCHEDULE_NUM & LAST_NAME & FIRST_NAME
    End Function

    Public Sub Compute()


        'PRES_NONTAX_13TH_MONTH = NONTAX13TH
        'GROSS_COMP_INCOME = GROSS_PAY + NONTAX13TH

        'PRES_NONTAX_SSS_GSIS_OTH_CONT = SSS_EE + PHIC + ADJUST1



        'PRES_NONTAX_BASIC_SMW_DAY = RATE * 8
        'PRES_NONTAX_BASIC_SMW_MONTH = PRES_NONTAX_BASIC_SMW_DAY * 24
        'PRES_NONTAX_BASIC_SMW_YEAR = PRES_NONTAX_BASIC_SMW_MONTH * 12
        'PRES_NONTAX_HOLIDAY_PAY = RATE * 2.0
        'PRES_NONTAX_HOLIDAY_PAY *= HOL_OT
        'PRES_NONTAX_OVERTIME_PAY = RATE * 1.25
        'PRES_NONTAX_OVERTIME_PAY *= R_OT
        'PRES_NONTAX_NIGHT_DIFF = RATE * 0.1
        'PRES_NONTAX_NIGHT_DIFF *= ND
        'PRES_NONTAX_HAZARD_PAY = 0


        'RD_OT = RATE * 1.3
        'RD_OT *= RD_OT
    End Sub


    Public Function toCSVRow() As String
        Return String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30},{31},{32},{33},{34},{35},{36},{37},{38},{39},{40},{41},{42},{43},{44},{45},{46},{47},{48},{49},{50},{51},{52},{53},{54},{55},{56},{57},{58},{59},{60},{61},{62},{63},{64},{65},{66},{67},{68},{69},{70},{71},{72},{73},{74},{75},{76},{77},{78},{79},{80},{81}",
FORM_TYPE, EMPLOYER_TIN, EMPLOYER_BRANCH_CODE, RETRN_PERIOD, SCHEDULE_NUM, SEQUENCE_NUM, REGISTERED_NAME, FIRST_NAME, LAST_NAME, MIDDLE_NAME, TIN, BRANCH_CODE, EMPLOYMENT_FROM, EMPLOYMENT_TO, ATC_CODE, STATUS_CODE, REGION_NUM, SUBS_FILING, EXMPN_CODE, FACTOR_USED, ACTUAL_AMT_WTHLD,
        INCOME_PAYMENT,
        PRES_TAXABLE_SALARIES,
        PRES_TAXABLE_13TH_MONTH,
        PRES_TAX_WTHLD,
        PRES_NONTAX_SALARIES,
        PRES_NONTAX_13TH_MONTH,
        PREV_TAXABLE_SALARIES,
        PREV_TAXABLE_13TH_MONTH,
        PREV_TAX_WTHLD,
        PREV_NONTAX_SALARIES,
        PREV_NONTAX_13TH_MONTH,
        PRES_NONTAX_SSS_GSIS_OTH_CONT,
        PREV_NONTAX_SSS_GSIS_OTH_CONT,
        TAX_RATE,
        OVER_WTHLD,
        AMT_WTHLD_DEC,
        EXMPN_AMT,
        TAX_DUE,
        HEATH_PREMIUM,
        FRINGE_BENEFIT,
        MONETARY_VALUE,
        NET_TAXABLE_COMP_INCOME,
        GROSS_COMP_INCOME,
        PREV_NONTAX_DE_MINIMIS,
        PREV_TOTAL_NONTAX_COMP_INCOME,
        PREV_TAXABLE_BASIC_SALARY,
        PRES_NONTAX_DE_MINIMIS,
        PRES_TAXABLE_BASIC_SALARY,
        PRES_TOTAL_COMP,
        PREV_PRES_TOTAL_TAXABLE,
        PRES_TOTAL_NONTAX_COMP_INCOME,
        PREV_NONTAX_GROSS_COMP_INCOME,
        PREV_NONTAX_BASIC_SMW,
        PREV_NONTAX_HOLIDAY_PAY,
        PREV_NONTAX_OVERTIME_PAY,
        PREV_NONTAX_NIGHT_DIFF,
        PREV_NONTAX_HAZARD_PAY,
        PRES_NONTAX_GROSS_COMP_INCOME,
        PRES_NONTAX_BASIC_SMW_DAY,
        PRES_NONTAX_BASIC_SMW_MONTH,
        PRES_NONTAX_BASIC_SMW_YEAR,
        PRES_NONTAX_HOLIDAY_PAY,
        PRES_NONTAX_OVERTIME_PAY,
        PRES_NONTAX_NIGHT_DIFF,
        PREV_PRES_TOTAL_COMP_INCOME,
        PRES_NONTAX_HAZARD_PAY,
        TOTAL_NONTAX_COMP_INCOME,
        TOTAL_TAXABLE_COMP_INCOME,
        PREV_TOTAL_TAXABLE,
        NONTAX_BASIC_SAL,
        TAX_BASIC_SAL,
        QRT_NUM,
        QUARTERDATE,
        NATIONALITY,
        REASON_SEPARATION,
        EMPLOYMENT_STATUS,
        ADDRESS1,
        ADDRESS2,
        ATC_DESC,
        DATE_DEATH,
        DATE_WTHELD)
        '_NULLFLAGS)
    End Function

    Public Shared Function toCSVRow2Header() As String
        Return "ID," &
       "FIRST_NAME," &
       "LAST_NAME," &
       "MIDDLE_NAME," &
       "TIN," &
       "EMPLOYMENT_FROM," &
       "EMPLOYMENT_TO," &
       "ACTUAL_AMT_WTHLD," &
       "FACTOR_USED," &
       "PRES_TAXABLE_SALARIES," &
       "PRES_TAXABLE_13TH_MONTH," &
       "PRES_TAX_WTHLD," &
       "PRES_NONTAX_SALARIES," &
       "PRES_NONTAX_13TH_MONTH," &
       "PRES_NONTAX_SSS_GSIS_OTH_CONT," &
       "OVER_WTHLD," &
       "AMT_WTHLD_DEC," &
       "TAX_DUE," &
       "NET_TAXABLE_COMP_INCOME," &
       "GROSS_COMP_INCOME," &
       "PRES_NONTAX_DE_MINIMIS," &
       "PRES_TOTAL_COMP," &
       "PRES_TOTAL_NONTAX_COMP_INCOME," &
       "PRES_NONTAX_GROSS_COMP_INCOME," &
       "PRES_NONTAX_BASIC_SMW_DAY," &
       "PRES_NONTAX_BASIC_SMW_MONTH," &
       "PRES_NONTAX_BASIC_SMW_YEAR," &
       "PRES_NONTAX_HOLIDAY_PAY," &
       "PRES_NONTAX_OVERTIME_PAY," &
       "PRES_NONTAX_NIGHT_DIFF," &
       "PRES_NONTAX_HAZARD_PAY," &
       "NONTAX_BASIC_SAL," &
       "TAX_BASIC_SAL," &
       "NATIONALITY," &
       "REASON_SEPARATION," &
       "EMPLOYMENT_STATUS," &
       "December," &
       "Final"
    End Function

    'Public Sub d()
    '    .id = cls(0)
    '    .FIRST_NAME = cls(1)
    '    .LAST_NAME = cls(2)
    '    .MIDDLE_NAME = cls(3)
    '    .TIN = cls(4)
    '    .EMPLOYMENT_FROM = cls(5)
    '    .EMPLOYMENT_TO = cls(6)
    '    .ACTUAL_AMT_WTHLD = cls(7)
    '    .FACTOR_USED = cls(8)
    '    .PRES_TAXABLE_SALARIES = cls(9)
    '    .PRES_TAXABLE_13TH_MONTH = cls(10)
    '    .PRES_TAX_WTHLD = cls(11)
    '    .PRES_NONTAX_SALARIES = cls(12)
    '    .PRES_NONTAX_13TH_MONTH = cls(13)
    '    .PRES_NONTAX_SSS_GSIS_OTH_CONT = cls(14)
    '    .OVER_WTHLD = cls(15)
    '    .AMT_WTHLD_DEC = cls(16)
    '    .TAX_DUE = cls(17)
    '    .NET_TAXABLE_COMP_INCOME = cls(18)
    '    .GROSS_COMP_INCOME = cls(19)
    '    .PRES_NONTAX_DE_MINIMIS = cls(20)
    '    .PRES_TOTAL_COMP = cls(21)
    '    .PRES_TOTAL_NONTAX_COMP_INCOME = cls(22)
    '    .PRES_NONTAX_GROSS_COMP_INCOME = cls(23)
    '    .PRES_NONTAX_BASIC_SMW_DAY = cls(24)
    '    .PRES_NONTAX_BASIC_SMW_MONTH = cls(25)
    '    .PRES_NONTAX_BASIC_SMW_YEAR = cls(26)
    '    .PRES_NONTAX_HOLIDAY_PAY = cls(27)
    '    .PRES_NONTAX_OVERTIME_PAY = cls(28)
    '    .PRES_NONTAX_NIGHT_DIFF = cls(29)
    '    .PRES_NONTAX_HAZARD_PAY = cls(30)
    '    .NONTAX_BASIC_SAL = cls(31)
    '    .TAX_BASIC_SAL = cls(32)
    '    .NATIONALITY = cls(33)
    '    .REASON_SEPARATION = cls(34)
    '    .EMPLOYMENT_STATUS = cls(35)

    'End Sub

    Public Sub toCSVRow2Item(wrtr As StreamWriter)
        wrtr.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30},{31},{32},{33},{34},{35},{36},{37}",
       id,
       FIRST_NAME,
       LAST_NAME,
       MIDDLE_NAME,
       TIN,
       EMPLOYMENT_FROM,
       EMPLOYMENT_TO,
       ACTUAL_AMT_WTHLD,
       FACTOR_USED,
       PRES_TAXABLE_SALARIES,
       PRES_TAXABLE_13TH_MONTH,
       PRES_TAX_WTHLD,
       PRES_NONTAX_SALARIES,
       PRES_NONTAX_13TH_MONTH,
       PRES_NONTAX_SSS_GSIS_OTH_CONT,
       OVER_WTHLD,
       AMT_WTHLD_DEC,
       TAX_DUE,
       NET_TAXABLE_COMP_INCOME,
       GROSS_COMP_INCOME,
       PRES_NONTAX_DE_MINIMIS,
       PRES_TOTAL_COMP,
       PRES_TOTAL_NONTAX_COMP_INCOME,
       PRES_NONTAX_GROSS_COMP_INCOME,
       PRES_NONTAX_BASIC_SMW_DAY,
       PRES_NONTAX_BASIC_SMW_MONTH,
       PRES_NONTAX_BASIC_SMW_YEAR,
       PRES_NONTAX_HOLIDAY_PAY,
       PRES_NONTAX_OVERTIME_PAY,
       PRES_NONTAX_NIGHT_DIFF,
       PRES_NONTAX_HAZARD_PAY,
       NONTAX_BASIC_SAL,
       TAX_BASIC_SAL,
       NATIONALITY,
       REASON_SEPARATION,
       EMPLOYMENT_STATUS,
       December,
       Final
       )
    End Sub

    Public Function ToInsertStatement(tablename As String) As String

        Dim flds As String = "FORM_TYPE ," &
         "EMPLOYER_TIN," &
         "EMPLOYER_BRANCH_CODE," &
         "RETRN_PERIOD," &
         "SCHEDULE_NUM," &
         "SEQUENCE_NUM," &
         "REGISTERED_NAME," &
         "FIRST_NAME," &
         "LAST_NAME," &
         "MIDDLE_NAME," &
         "TIN," &
         "BRANCH_CODE," &
         "EMPLOYMENT_FROM," &
         "EMPLOYMENT_TO," &
         "ATC_CODE," &
         "STATUS_CODE," &
         "REGION_NUM," &
         "SUBS_FILING," &
         "EXMPN_CODE," &
         "FACTOR_USED," &
         "ACTUAL_AMT_WTHLD," &
         "INCOME_PAYMENT," &
         "PRES_TAXABLE_SALARIES," &
         "PRES_TAXABLE_13TH_MONTH," &
         "PRES_TAX_WTHLD," &
         "PRES_NONTAX_SALARIES," &
         "PRES_NONTAX_13TH_MONTH," &
         "PREV_TAXABLE_SALARIES," &
         "PREV_TAXABLE_13TH_MONTH," &
         "PREV_TAX_WTHLD," &
         "PREV_NONTAX_SALARIES," &
         "PREV_NONTAX_13TH_MONTH," &
         "PRES_NONTAX_SSS_GSIS_OTH_CONT," &
         "PREV_NONTAX_SSS_GSIS_OTH_CONT," &
         "TAX_RATE," &
         "OVER_WTHLD," &
         "AMT_WTHLD_DEC," &
         "EXMPN_AMT," &
         "TAX_DUE," &
         "HEATH_PREMIUM," &
         "FRINGE_BENEFIT," &
         "MONETARY_VALUE," &
         "NET_TAXABLE_COMP_INCOME," &
         "GROSS_COMP_INCOME," &
         "PREV_NONTAX_DE_MINIMIS," &
         "PREV_TOTAL_NONTAX_COMP_INCOME," &
         "PREV_TAXABLE_BASIC_SALARY," &
         "PRES_NONTAX_DE_MINIMIS," &
         "PRES_TAXABLE_BASIC_SALARY," &
         "PRES_TOTAL_COMP," &
         "PREV_PRES_TOTAL_TAXABLE," &
         "PRES_TOTAL_NONTAX_COMP_INCOME," &
         "PREV_NONTAX_GROSS_COMP_INCOME," &
         "PREV_NONTAX_BASIC_SMW," &
         "PREV_NONTAX_HOLIDAY_PAY," &
         "PREV_NONTAX_OVERTIME_PAY," &
         "PREV_NONTAX_NIGHT_DIFF," &
         "PREV_NONTAX_HAZARD_PAY," &
         "PRES_NONTAX_GROSS_COMP_INCOME," &
         "PRES_NONTAX_BASIC_SMW_DAY," &
         "PRES_NONTAX_BASIC_SMW_MONTH," &
         "PRES_NONTAX_BASIC_SMW_YEAR," &
         "PRES_NONTAX_HOLIDAY_PAY," &
         "PRES_NONTAX_OVERTIME_PAY," &
         "PRES_NONTAX_NIGHT_DIFF," &
         "PREV_PRES_TOTAL_COMP_INCOME," &
         "PRES_NONTAX_HAZARD_PAY," &
         "TOTAL_NONTAX_COMP_INCOME," &
         "TOTAL_TAXABLE_COMP_INCOME," &
         "PREV_TOTAL_TAXABLE," &
         "NONTAX_BASIC_SAL," &
         "TAX_BASIC_SAL," &
         "QRT_NUM," &
         "QUARTERDATE," &
         "NATIONALITY," &
         "REASON_SEPARATION," &
         "EMPLOYMENT_STATUS," &
         "ADDRESS1," &
         "ADDRESS2," &
         "ATC_DESC," &
         "DATE_DEATH," &
         "DATE_WTHELD"

        Dim values As String = String.Format("'{0}','{1}','{2}',{{{3}}},'{4}',{5},'{6}','{7}','{8}','{9}','{10}','{11}',{{{12}}},{{{13}}},'{14}','{15}','{16}','{17}','{18}',{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30},{31},{32},{33},{34},{35},{36},{37},{38},{39},{40},{41},{42},{43},{44},{45},{46},{47},{48},{49},{50},{51},{52},{53},{54},{55},{56},{57},{58},{59},{60},{61},{62},{63},{64},{65},{66},{67},{68},{69},{70},{71},{72},{73},'{74}','{75}','{76}','{77}','{78}','{79}',{80},{81}",
         FORM_TYPE,
         EMPLOYER_TIN,
         EMPLOYER_BRANCH_CODE,
         RETRN_PERIOD,
         SCHEDULE_NUM,
         SEQUENCE_NUM,
         REGISTERED_NAME,
         FIRST_NAME,
         LAST_NAME,
         MIDDLE_NAME,
         TIN,
         BRANCH_CODE,
         EMPLOYMENT_FROM,
         EMPLOYMENT_TO,
         ATC_CODE,
         STATUS_CODE,
         REGION_NUM,
         SUBS_FILING,
         EXMPN_CODE,
         FACTOR_USED,
         ACTUAL_AMT_WTHLD,
         INCOME_PAYMENT,
         PRES_TAXABLE_SALARIES,
         PRES_TAXABLE_13TH_MONTH,
         PRES_TAX_WTHLD,
         PRES_NONTAX_SALARIES,
         PRES_NONTAX_13TH_MONTH,
         PREV_TAXABLE_SALARIES,
         PREV_TAXABLE_13TH_MONTH,
         PREV_TAX_WTHLD,
         PREV_NONTAX_SALARIES,
         PREV_NONTAX_13TH_MONTH,
         PRES_NONTAX_SSS_GSIS_OTH_CONT,
         PREV_NONTAX_SSS_GSIS_OTH_CONT,
         TAX_RATE,
         OVER_WTHLD,
         AMT_WTHLD_DEC,
         EXMPN_AMT,
         TAX_DUE,
         HEATH_PREMIUM,
         FRINGE_BENEFIT,
         MONETARY_VALUE,
         NET_TAXABLE_COMP_INCOME,
         GROSS_COMP_INCOME,
         PREV_NONTAX_DE_MINIMIS,
         PREV_TOTAL_NONTAX_COMP_INCOME,
         PREV_TAXABLE_BASIC_SALARY,
         PRES_NONTAX_DE_MINIMIS,
         PRES_TAXABLE_BASIC_SALARY,
         PRES_TOTAL_COMP,
         PREV_PRES_TOTAL_TAXABLE,
         PRES_TOTAL_NONTAX_COMP_INCOME,
         PREV_NONTAX_GROSS_COMP_INCOME,
         PREV_NONTAX_BASIC_SMW,
         PREV_NONTAX_HOLIDAY_PAY,
         PREV_NONTAX_OVERTIME_PAY,
         PREV_NONTAX_NIGHT_DIFF,
         PREV_NONTAX_HAZARD_PAY,
         PRES_NONTAX_GROSS_COMP_INCOME,
         PRES_NONTAX_BASIC_SMW_DAY,
         PRES_NONTAX_BASIC_SMW_MONTH,
         PRES_NONTAX_BASIC_SMW_YEAR,
         PRES_NONTAX_HOLIDAY_PAY,
         PRES_NONTAX_OVERTIME_PAY,
         PRES_NONTAX_NIGHT_DIFF,
         PREV_PRES_TOTAL_COMP_INCOME,
         PRES_NONTAX_HAZARD_PAY,
         TOTAL_NONTAX_COMP_INCOME,
         TOTAL_TAXABLE_COMP_INCOME,
         PREV_TOTAL_TAXABLE,
         NONTAX_BASIC_SAL,
         TAX_BASIC_SAL,
         QRT_NUM,
         "{//}",
         NATIONALITY,
         REASON_SEPARATION,
         EMPLOYMENT_STATUS,
         ADDRESS1,
         ADDRESS2,
         ATC_DESC,
         "{//}",
         "{//}"
)

        Return String.Format("INSERT INTO {0} ({1})VALUES({2});", tablename, flds, values)
    End Function

    Public Function CompareTo(other As ALPHADTL) As Integer Implements IComparable(Of ALPHADTL).CompareTo
        Return ToString.CompareTo(other.ToString)
    End Function
End Class