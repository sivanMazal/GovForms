namespace GovForms.Engine.Models.Enums
{
    public enum UserRole
    {
        Guest = 0,      // אזרח רגיל
        Clerk = 1,      // פקיד זוטר (יכול לבדוק מסמכים)
        Manager = 2,    // מנהל (יכול לאשר סכומים גדולים)
        Admin = 99      // מנהל מערכת (יכול הכל)
    }
}