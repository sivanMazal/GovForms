namespace GovForms.Engine.Models.Enums
{
    public enum ApplicationStatus
    {
        NotSubmitted = 1,
        InProcess = 2,
        Approved = 3,
        Rejected = 4,
        PendingManualReview = 5, 
        ReturnedForDocs = 6,     // ודאי שזה מופיע רק פעם אחת כאן
        MissingDocuments = 7
    }
}