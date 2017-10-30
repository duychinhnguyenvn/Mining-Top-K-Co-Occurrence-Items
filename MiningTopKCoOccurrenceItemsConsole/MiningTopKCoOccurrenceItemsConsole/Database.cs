using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiningTopKCoOccurrenceItemsConsole
{
    class Database
    {
        public static List<List<string>> getDatabase(string dbName)
        {
            List<List<string>> db;
            switch (dbName)
            {               
                case "connect":
                    db = Database.loadDBFromFile(Utils.ROOTFOLDER+"connect.dat");
                    break;
                case "accidents":
                    db = Database.loadDBFromFile(Utils.ROOTFOLDER+"accidents.dat");
                    break;
                case "syn_data1":
                    db = Database.loadDBFromFile(Utils.ROOTFOLDER + "syn_data1.dat");
                    break;
                case "syn_data2":
                    db = Database.loadDBFromFile(Utils.ROOTFOLDER + "syn_data2.dat");
                    break;
                default:
                    db = exampleDB();
                    break;
            }
            return db;
        }
        private static List<List<string>> exampleDB()
        {
            List<List<string>> db = new List<List<string>>();
            List<string> tran = new List<string>(new string[] { "b","f","g"});
            db.Add(tran);
            tran = new List<string>(new string[] { "a", "b", "c", "f" });
            db.Add(tran);
            tran = new List<string>(new string[] { "a", "c", "d", "f" });
            db.Add(tran);
            tran = new List<string>(new string[] { "b", "c", "e" });
            db.Add(tran);
            tran = new List<string>(new string[] { "a", "c", "d", "e", "f" });
            db.Add(tran);
            return db;
        }

        public static List<List<string>> loadDBFromFile(string path)
        {
            List<List<string>> db = new List<List<string>>();
            string line;
            List<string> listItems = new List<string>();
            // Read the file and display it line by line.
            System.IO.StreamReader file = new System.IO.StreamReader(path);
            while ((line = file.ReadLine()) != null)
            {
                List<string> lineItems = line.Trim().Split(' ').ToList<string>();
                //foreach (var item in lineItems) {
                 //   if (!listItems.Contains(item)) listItems.Add(item);
                //}
                db.Add(lineItems);
            }

            file.Close();
            Console.WriteLine("Loaded row count: {0}", db.Count);
            //Console.WriteLine("Total Items: {0}",listItems.Count);
            return db;
        }
    }
}
