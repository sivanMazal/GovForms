using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using GovForms.Engine.Models;
using GovForms.Engine.Models.Enums;

namespace GovForms.Engine.Data
{
    public class AppRepository : IAppRepository
    {
        private readonly string _connectionString;

        public AppRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Application> GetApplicationsByStatus(int statusId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                // שימוש ב-Dapper לשליפה מהירה ומקצועית
                return db.Query<Application>(
                    "SELECT * FROM Applications WHERE StatusId = @StatusId", 
                    new { StatusId = statusId }
                ).AsList();
            }
        }

        public void UpdateStatus(int appId, int newStatusId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                db.Execute(
                    "UPDATE Applications SET StatusId = @StatusId WHERE Id = @Id", 
                    new { StatusId = newStatusId, Id = appId }
                );
            }
        }

        public void LogHistory(ApplicationHistory history)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string sql = @"INSERT INTO ApplicationHistory (ApplicationId, Action, Status, Remarks, UserId, Timestamp) 
                               VALUES (@ApplicationId, @Action, @Status, @Remarks, @UserId, @Timestamp)";
                db.Execute(sql, history);
            }
        }

        // מימושים הנדרשים על ידי ה-Interface (נשלים בהמשך הפרויקט)
public Application AddApplication(Application app)
{
    using (IDbConnection db = new SqlConnection(_connectionString))
    {
        app.SubmissionDate = DateTime.Now;
        // הוספנו את עמודת Status (הטקסטואלית) לשאילתה
        string sql = @"
            INSERT INTO Applications (Title, Type, Amount, UserEmail, UserId, StatusId, Status, SubmissionDate) 
            VALUES (@Title, @Type, @Amount, @UserEmail, @UserId, @StatusId, @Status, GETDATE());
            SELECT CAST(SCOPE_IDENTITY() as int);";

        // כאן אנחנו משתמשים ב-app.StatusId.ToString() כדי לשלוח את שם הסטטוס (למשל "InProcess")
        int newId = db.QuerySingle<int>(sql, new { 
            app.Title, 
            app.Type,
            app.Amount, 
            app.UserEmail, 
            app.UserId,
            app.StatusId,
            Status = app.StatusId.ToString() // המרת ה-Enum למחרוזת עבור ה-SQL
        });

        app.Id = newId;
        return app;
    }
}       public Application GetApplicationById(int id) => throw new NotImplementedException();
    }
}