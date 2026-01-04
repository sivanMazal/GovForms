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

                // כאן הייתה הטעות: תיקנו את השם ל-TaxRefundValidator
                case ApplicationType.TaxRefund:
                    return new TaxRefundValidator(); 

                default:
                    return null;
            }
        }
    }
}