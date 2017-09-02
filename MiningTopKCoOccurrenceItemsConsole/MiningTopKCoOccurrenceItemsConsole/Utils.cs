using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MiningTopKCoOccurrenceItemsConsole
{
    class Utils
    {
        public static string ROOTFOLDER = "E:\\Cao hoc\\Codes\\Mining-Top-K-Co-Occurrence-Items\\MiningTopKCoOccurrenceItemsConsole\\";
        public static List<List<string>> getRandomPatternList(List<List<string>> db, int length)
        {
            List<List<string>> randomList = new List<List<string>>();
            Random rnd = new Random();
            int n;
            int index;
            for (int i=0;i<100;i++)
            {
                n = rnd.Next(db.Count-1);
                List<string> rdTran = db[n];
                List<string> patternList = new List<string>();
                for (int j=0;j<length;j++)
                {
                    index = rnd.Next(rdTran.Count - length - 1);
                    patternList.Add(rdTran[index]);
                    rdTran.RemoveAt(index);
                }
                
                randomList.Add(patternList);
            }        
            return randomList;
        }
        public static void genQueryItemsetsToFile(string dbName, int length)
        {
            List<List<string>> db = Database.getDatabase(dbName);
            List<List<string>> randomList = getRandomPatternList(db, length);
            StreamWriter file = new StreamWriter(ROOTFOLDER+dbName+"_"+length+".txt");
            foreach (var item in randomList)
            {
                file.WriteLine(string.Join(" ",item));
            }
            file.Close();
        }
        public static List<List<string>> loadQueryItemsets(string dbName,int length)
        {
            List<List<string>> db;
            switch (dbName)
            {
                case "connect":
                    db = Database.loadDBFromFile(Utils.ROOTFOLDER + "connect_"+length+".txt");
                    break;
                case "accidents":
                    db = Database.loadDBFromFile(Utils.ROOTFOLDER + "accidents_"+length+".txt");
                    break;
                default:
                    db = new List<List<string>>();
                    db.Add(new List<string>(new string[] {"a","c" }));
                    break;
            }
            return db;
        }
        public static void travelDownSetCo(PiTreeNode childrenNode,Dictionary<string,int> CO)
        {
            if (!CO.ContainsKey(childrenNode.getLabel())) {
                CO[childrenNode.getLabel()] = childrenNode.getCount();
            }
            else
            {
                CO[childrenNode.getLabel()] = CO[childrenNode.getLabel()] + childrenNode.getCount();
            }            
            foreach (var n in childrenNode.getChildrenNodes())
            {
                travelDownSetCo(n, CO);
            }
        }
        public static bool IsSubSetOf(List<string> a, List<string> b)
        {
            bool result = true;
            foreach (var item in a)
            {
                result = b.Contains(item);
                if (result != true)
                {
                    break;
                }
            }
            return result;
        }
        public static List<string> GetCoItems(List<string> a, List<string> b)
        {
            List<string> result = new List<string>();
            foreach (var item in b)
            {
                if (!a.Contains(item))
                {
                    result.Add(item);
                }
            }
            return result;
        }
    }
}
