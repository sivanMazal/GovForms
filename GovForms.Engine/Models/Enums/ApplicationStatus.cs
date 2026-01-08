namespace GovForms.Engine.Models.Enums
{
    public enum ApplicationStatus
    {
        NotSubmitted = 1,          // טרם הוגש
        WaitingForTreatment = 2,   // ממתין לטיפול
        InTreatment = 3,           // בטיפול
        Treated = 4,               // טופל
        ReturnedForCompletion = 5, // הוחזר להשלמת מסמכים
        PendingManualReview = 6,   // ממתין לבדיקה ידנית
        MissingDocuments = 7       // חסרים מסמכים
    }
}