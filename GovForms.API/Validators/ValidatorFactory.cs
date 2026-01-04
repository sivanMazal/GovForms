using GovForms.Engine.Models.Enums;

namespace GovForms.Engine.Validators
{
    public static class ValidatorFactory
    {
        public static IValidator GetValidator(ApplicationType type)
        {
            switch (type)
            {
                case ApplicationType.BuildingPermit:
                    return new BuildingPermitValidator();
                
                case ApplicationType.BusinessLicense:
                    return new BusinessLicenseValidator();
                
                case ApplicationType.TaxDiscount:
                    return new TaxDiscountValidator();
                
                default:
                    return null; // אם אין מומחה, נחזיר כלום (והמנהל יטפל בזה)
            }
        }
    }
}