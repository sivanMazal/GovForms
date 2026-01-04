using System.Collections.Generic;
using System.Data.SqlClient;
using GovForms.Engine.Models;
using GovForms.Engine.Models.Enums;

namespace GovForms.Engine.Data
{
// הוספנו את : IAppRepository
public class AppRepository : IAppRepository    {
        private string _connString ;
public AppRepository(string connString)
{
    _connString = connString;
}
       // שינינו את השם ואת מה שהיא מקבלת כדי להתאים לאינטרפייס
public List<Application> GetApplicationsByStatus(int statusId)
{
    var list = new List<Application>();
    using (var conn = new SqlConnection(_connString))
    {
        conn.Open();
        var cmd = new SqlCommand("SELECT ApplicationID, Title, StatusID, UserID, Amount FROM Applications WHERE StatusID = @status", conn);
        
        // כאן השינוי הגדול! במקום Enum קבוע, אנחנו משתמשים במספר שקיבלנו
        cmd.Parameters.AddWithValue("@status", statusId);

        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                list.Add(new Application
                {
                    Id = (int)reader["ApplicationID"],
                    Title = reader["Title"].ToString(),
                    // המרה למספר ואז ל-Enum
                    Status = (ApplicationStatus)reader["StatusID"], 
                    UserId = (int)reader["UserID"],
                    Amount = reader["Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Amount"])
                });
            }
        }
    }
    return list;
}
       // שינינו את החתימה שתתאים בדיוק לאינטרפייס
public void UpdateStatus(int appId, int newStatus)
{
    // נתונים דיפולטיביים למערכת (כי האינטרפייס לא מבקש אותם כרגע)
    int actingUserId = 0; // 0 מסמל "מערכת"thl
    string comment = "Automated status update by Workflow Engine";

    using (var conn = new SqlConnection(_connString))
    {
        conn.Open();

        // 1. עדכון סטטוס
        var cmd = new SqlCommand("UPDATE Applications SET StatusID = @status WHERE ApplicationID = @id", conn);
        cmd.Parameters.AddWithValue("@status", newStatus);
        cmd.Parameters.AddWithValue("@id", appId);
        cmd.ExecuteNonQuery();

        // 2. תיעוד בהיסטוריה (הקוד המעולה שלך נשאר!)
        string historySql = @"INSERT INTO ApplicationHistory 
                              (ApplicationID, StatusID, ChangeDate, Remarks, UserID, ActionDescription) 
                              VALUES 
                              (@appId, @status, GETDATE(), @remarks, @userId, @description)";

        var historyCmd = new SqlCommand(historySql, conn);
        historyCmd.Parameters.AddWithValue("@appId", appId);
        historyCmd.Parameters.AddWithValue("@status", newStatus);
        historyCmd.Parameters.AddWithValue("@remarks", comment);
        historyCmd.Parameters.AddWithValue("@userId", actingUserId);
        historyCmd.Parameters.AddWithValue("@description", "Status Change");
        historyCmd.ExecuteNonQuery();
    }
}
// מימוש הפונקציה לכתיבת היסטוריה ב-SQL האמיתי
        public void LogHistory(ApplicationHistory history)
        {
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();

                string sql = @"INSERT INTO ApplicationHistory 
                               (ApplicationID, StatusID, ChangeDate, Remarks, UserID, ActionDescription) 
                               VALUES 
                               (@appId, @status, @date, @remarks, @userId, @action)";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@appId", history.ApplicationId);
                    // המרה ל-INT כדי שה-SQL יבין את ה-Enum
                    cmd.Parameters.AddWithValue("@status", (int)history.Status); 
                    cmd.Parameters.AddWithValue("@date", history.Date);
                    cmd.Parameters.AddWithValue("@remarks", history.Remarks ?? (object)DBNull.Value); // טיפול ב-Null
                    cmd.Parameters.AddWithValue("@userId", history.UserId);
                    cmd.Parameters.AddWithValue("@action", history.Action);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}