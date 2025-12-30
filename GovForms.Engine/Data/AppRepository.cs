using System.Collections.Generic;
using System.Data.SqlClient;
using GovForms.Engine.Models;
using GovForms.Engine.Models.Enums;

namespace GovForms.Engine.Data
{
    public class AppRepository
    {
        private string _connString = @"Server=DESKTOP-5CVCILM;Database=GovFormsDB;Trusted_Connection=True;TrustServerCertificate=True;";

        public List<Application> GetInProcessApplications()
        {
            var list = new List<Application>();
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                // שימוש בערך המספרי של ה-Enum (3) בתוך השאילתה
                var cmd = new SqlCommand("SELECT ApplicationID, Title, StatusID, UserID, Amount FROM Applications WHERE StatusID = @status", conn);
                cmd.Parameters.AddWithValue("@status", (int)ApplicationStatus.InProcess);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Application
                        {
                            Id = (int)reader["ApplicationID"],
                            Title = reader["Title"].ToString(),
                            Status = (ApplicationStatus)reader["StatusID"], // שימוש ב-Enum מהצילום שלך
                            UserId = (int)reader["UserID"],
                            Amount = reader["Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Amount"])
                        });
                    }
                }
            }
            return list;
        }

        public void UpdateApplicationStatus(int appId, ApplicationStatus nextStatus, string comment, int actingUserId)
        {
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();

                // 1. עדכון הסטטוס בטבלת Applications
                var cmd = new SqlCommand("UPDATE Applications SET StatusID = @status WHERE ApplicationID = @id", conn);
                cmd.Parameters.AddWithValue("@status", (int)nextStatus);
                cmd.Parameters.AddWithValue("@id", appId);
                cmd.ExecuteNonQuery();

                // 2. רישום בהיסטוריה כולל ActionDescription ו-Remarks (פותר את השגיאה!)
                string historySql = @"INSERT INTO ApplicationHistory 
                              (ApplicationID, StatusID, ChangeDate, Remarks, UserID, ActionDescription) 
                              VALUES 
                              (@appId, @status, GETDATE(), @remarks, @userId, @description)";

                var historyCmd = new SqlCommand(historySql, conn);
                historyCmd.Parameters.AddWithValue("@appId", appId);
                historyCmd.Parameters.AddWithValue("@status", (int)nextStatus);
                historyCmd.Parameters.AddWithValue("@remarks", comment); // הערה כללית
                historyCmd.Parameters.AddWithValue("@userId", actingUserId);
                historyCmd.Parameters.AddWithValue("@description", "Automated status update by Engine"); // תיאור הפעולה
                historyCmd.ExecuteNonQuery();
            }
        }
    }
}