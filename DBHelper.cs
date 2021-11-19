using System.Collections.Generic;
using System.Linq;
using LiteDB;

namespace Project_Manager_V2
{
    public class DBHelper
    {
        private LiteDatabase db;
        private ILiteCollection<ProjectInfo> table;

        public DBHelper()
        {
            // Open database or create database if it does not exist
            db = new LiteDatabase(@".\DB.db");
            table = db.GetCollection<ProjectInfo>("ProjectInfos");
        }

        // Get Project Info by name
        public ProjectInfo GetProjectInfo(string Name)
        {
            int count = table.FindAll().Count();
            return table.FindOne(x => x.Name == Name);
        }

        // Insert or Update Project Info
        public void SetProjectInfo(ProjectInfo projectInfo)
        {
            if (!table.Update(projectInfo))
                table.Insert(projectInfo);
        }
    }
}
