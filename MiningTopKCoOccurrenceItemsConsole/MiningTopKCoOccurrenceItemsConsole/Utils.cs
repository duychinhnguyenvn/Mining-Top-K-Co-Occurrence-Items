using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

namespace MiningTopKCoOccurrenceItemsConsole
{
    class Utils
    {
        public static string ROOTFOLDER = "E:\\Cao hoc\\Codes\\Mining-Top-K-Co-Occurrence-Items\\MiningTopKCoOccurrenceItemsConsole\\";
        public static List<List<string>> getRandomPatternList(string[][] db, int length)
        {
            List<List<string>> randomList = new List<List<string>>();
            Random rnd = new Random();
            int n;
            int index;
            for (int i=0;i<100;i++)
            {
                n = rnd.Next(db.Length-1);
                string[] rdTran = db[n];
                List<string> patternList = new List<string>();
                for (int j=0;j<length;j++)
                {
                    index = rnd.Next(rdTran.Length - length - 1);
                    patternList.Add(rdTran[index]);
                    rdTran = rdTran.Except(new string[] { rdTran[index] }).ToArray();
                }
                
                randomList.Add(patternList);
            }        
            return randomList;
        }
        public static void genQueryItemsetsToFile(string dbName, int length)
        {
            string[][] db = Database.getDatabase(dbName);
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
        public static Int32 GetCardinality(BitArray bitArray)
        {

            Int32[] ints = new Int32[(bitArray.Count >> 5) + 1];

            bitArray.CopyTo(ints, 0);

            Int32 count = 0;

            // fix for not truncated bits in last integer that may have been set to true with SetAll()
            ints[ints.Length - 1] &= ~(-1 << (bitArray.Count % 32));

            for (Int32 i = 0; i < ints.Length; i++)
            {

                Int32 c = ints[i];
                unchecked
                {
                    c = c - ((c >> 1) & 0x55555555);
                    c = (c & 0x33333333) + ((c >> 2) & 0x33333333);
                    c = ((c + (c >> 4) & 0xF0F0F0F) * 0x1010101) >> 24;
                }

                count += c;

            }

            return count;
    
        }
        public static Int32 GetCountBitArray(BitArray bitArray)
        {
            Int32 count = 0;
            foreach(bool i in bitArray)
            {
                if (i) count++;
            }
            return count;
        }
        public static int CountLines(string filename)
        {
            int result = 0;

            using (var input = File.OpenText(filename))
            {
                while (input.ReadLine() != null)
                {
                    ++result;
                }
            }

            return result;
        }

    }
}
