using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace Imageprocessing
{
    class SqliteDataAccess
    {
        public  SqliteDataAccess() 
        {
            if (!File.Exists("./Faces.db"))
            {
                SQLiteConnection.CreateFile("Faces.db");
                using(SQLiteConnection con = new SQLiteConnection(LoadConnectionString()))
                {
                    SQLiteCommand command = new SQLiteCommand();
                    con.Open();
                    command.CommandText= @"CREATE TABLE Faces(id INTEGER NOT NULL UNIQUE,face  BLOB NOT NULL,PRIMARY KEY(id AUTOINCREMENT))";
                    command.Connection = con;
                    command.ExecuteNonQuery();
                    con.Close();
                    Console.WriteLine("database created");
                }
            }
        }
        public static List<FaceModel> LoadFaces()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<FaceModel>("select * from Faces", new DynamicParameters());
                return output.ToList();
            }
        }

        public static void SaveFace(FaceModel face)
        {
            if (face!=null)
            {
                using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
                {
                    cnn.Execute("insert into Faces (face) values (@Face)", face);
                }
            }
            else
            {
                Console.WriteLine("something gone wrong while saving new face");
            }
        }

        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}
