namespace GovForms.Engine.Interfaces
{
    public interface IPermissionService
    {
        Task<bool> CanUserApprove(int userId);
    }
}