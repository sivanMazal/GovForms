namespace GovForms.Engine.Models.Enums
{
    public enum ApplicationStatus
    {
   NotSubmitted = 1,
        WaitingForTreatment = 2,
        InTreatment = 3,
        Treated = 4,              // זה יהיה ה"מאושר" הסופי
        ReturnedForCompletion = 5,
        PendingManualReview = 6,   // נשתמש בזה ל"אישור בכיר" (דרישה 7)
        MissingDocuments = 7,
        Rejected = 8     // חסרים מסמכים
    }
}