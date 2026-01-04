using GovForms.Engine.Models;

namespace GovForms.Engine.Validators
{
    public interface IValidator
    {
        bool Validate(Application app);
    }
}