using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiningTopKCoOccurrenceItemsConsole
{
    class Database
    {
        public static string[][] getDatabase(string dbName)
        {
            string[][] db;
            switch (dbName)
            {               
                case "connect":
                    db = Database.loadDBFromFileToArray(Utils.ROOTFOLDER+"connect.dat");
                    break;
                case "accidents":
                    db = Database.loadDBFromFileToArray(Utils.ROOTFOLDER+"accidents.dat");
                    break;
                default:
                    db = exampleDB();
                    break;
            }
            return db;
        }
        private static string[][] exampleDB()
        {
            string[][] db = new string[5][];
            db[0] = new string[] { "b", "f", "g" };
            db[1] = new string[] { "a", "b", "c", "f" };
            db[2] = new string[] { "a", "c", "d", "f" };
            db[3] = new string[] { "b", "c", "e" };
            db[4] = new string[] { "a", "c", "d", "e", "f" };          
            return db;
        }

        public static List<List<string>> loadDBFromFile(string path)
        {
            List<List<string>> db = new List<List<string>>();
            string line;

            // Read the file and display it line by line.
            System.IO.StreamReader file = new System.IO.StreamReader(path);
            while ((line = file.ReadLine()) != null)
            {
                db.Add(line.Trim().Split(' ').ToList<string>());
            }

            file.Close();
            return db;
        }
        public static string[][] loadDBFromFileToArray(string path)
        {
            int rowCount = Utils.CountLines(path);
            string[][] db = new string[rowCount][];
            string line;

            // Read the file and display it line by line.
            System.IO.StreamReader file = new System.IO.StreamReader(path);
            int i = 0;
            while ((line = file.ReadLine()) != null)
            {
                db[i]=line.Trim().Split(' ');
                i++;
            }

            file.Close();
            return db;
        }
    }
}
