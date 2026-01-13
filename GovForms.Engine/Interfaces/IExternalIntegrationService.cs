namespace GovForms.Engine.Interfaces
{
    public interface IExternalIntegrationService
    {
        // בדיקה האם לאזרח יש חובות פתוחים
        Task<bool> HasOutstandingDebtsAsync(string Email);
        
        // בדיקה האם פרטי המשתמש מאומתים מול מרשם האוכלוסין
        Task<bool> IsIdentityVerifiedAsync(int userId);
    }
}