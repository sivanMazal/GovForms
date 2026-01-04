using System;
using System.Threading.Tasks;

namespace GovForms.API.Integrations
{
    public class MinistryOfInteriorSimulator : ICitizenService
    {
        public async Task<CitizenStatus> ValidateCitizenAsync(int userId)
        {
            // סימולציה של זמן המתנה (כאילו פונים לשרת מרוחק)
            // זה קריטי להראות שאת יודעת לעבוד עם Async/Await
            await Task.Delay(2000); // ממתין 2 שניות

            // חוקים מומצאים (Hardcoded) לצורך ההדגמה:
            
            // משתמש 999 הוא "האזרח הבעייתי" - יש לו חובות
            if (userId == 999)
            {
                return new CitizenStatus 
                { 
                    CanSubmitRequest = false, 
                    RejectionReason = "Citizen has outstanding debts via Hotsaa LaPoal" 
                };
            }

            // משתמש 0 הוא בכלל לא קיים
            if (userId == 0)
            {
                return new CitizenStatus 
                { 
                    CanSubmitRequest = false, 
                    RejectionReason = "Citizen ID not found in registry" 
                };
            }

            // כל שאר המשתמשים תקינים
            return new CitizenStatus 
            { 
                CanSubmitRequest = true, 
                RejectionReason = null 
            };
        }
    }
}