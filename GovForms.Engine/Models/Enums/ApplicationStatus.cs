using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GovForms.Engine.Models.Enums
{
    public enum ApplicationStatus
    {
        NotSubmitted = 1,      // טרם הוגש
        WaitingForReview = 2,  // ממתין לטיפול
        InProcess = 3,         // בטיפול
        Handled = 4,           // טופל
        ReturnedForDocs = 5    // הוחזר להשלמת מסמכים
    }
}
