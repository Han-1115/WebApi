using Confluent.Kafka;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using System.Collections.Generic;

public static class PropertyKeyMap
{
#pragma warning disable S2386 // Mutable fields should not be "public static"
    public static readonly Dictionary<string, string> Map = new Dictionary<string, string>
    {
        { "StaffNo", "person.number" },
        { "Date", "dateSet.date" },
        { "RequiredAttendanceHours", "ATS_RESULT.S16" },
        { "ActualAttendanceHours", "ATS_RESULT.S17" },
        { "RequiredAttendanceDays", "ATS_RESULT.S50" },
        { "ActualAttendanceDays", "ATS_RESULT.S51" },
        { "Absenteeism", "ATS_RESULT.S38" },
        { "AbsenteeismHours", "ATS_RESULT.S23" },
        { "LeaveNumbers", "ATS_RESULT.S26" },
        { "LeaveDays", "ATS_RESULT.S28" },
        { "SickLeaveHours", "ATS_RESULT.S43" },
        { "SickLeaveDays", "ATS_RESULT.S42" },
        { "AnnualLeaveHours", "ATS_RESULT.S46" },
        { "AnnualLeaveDays", "ATS_RESULT.S45" },
        { "PersonalLeaveHours", "ATS_RESULT.S49" },
        { "PersonalLeaveDays", "ATS_RESULT.S48" },
        { "CompensatoryLeaveHours", "ATS_RESULT.S53" },
        { "CompensatoryLeaveDays", "ATS_RESULT.S52" },
        { "MarriageLeave", "ATS_RESULT.S54" },
        { "MaternityLeave", "ATS_RESULT.S55" },
        { "PaternityLeave", "ATS_RESULT.S56" },
        { "MaternityProtectionLeave", "ATS_RESULT.S57" },
        { "PrenatalLeave", "ATS_RESULT.S58" },
        { "AbortionLeave", "ATS_RESULT.S59" },
        { "BereavementLeave", "ATS_RESULT.S60" },
        { "OnlyChildCareLeave", "ATS_RESULT.S61" },
        { "MedicalPeriod", "ATS_RESULT.S62" },
        { "MaternalExaminationLeave", "ATS_RESULT.S63" },
        { "BreastfeedingLeaveDays", "ATS_RESULT.S67" },
        { "ParentalLeave", "ATS_RESULT.S101" },
        { "Outgoing", "ATS_RESULT.S102" },
        { "LateNumbers", "ATS_RESULT.S18" },
        { "LateMinutes", "ATS_RESULT.S19" },
        { "LeaveEarlyNumbers", "ATS_RESULT.S20" },
        { "LeaveEarlyMinutes", "ATS_RESULT.S21" }
    };
#pragma warning restore S2386 // Mutable fields should not be "public static"
}
