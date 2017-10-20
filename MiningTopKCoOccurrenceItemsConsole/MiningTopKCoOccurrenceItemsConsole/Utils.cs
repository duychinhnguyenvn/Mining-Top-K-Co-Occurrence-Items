using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using Microsoft.Office.Interop;
using Microsoft.Office.Interop.Excel;

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
                while (rdTran.Count<30)
                {
                    n = rnd.Next(db.Count - 1);
                    rdTran = db[n];
                }
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
        public static void WriteTestResult(string dbName,string algorithmName,int k, int queryLength, int totalProcessingTime,int avgPreprocessingTime,int avgPreprocessingMemUsage)
        {
            string RunningTimepath = Utils.ROOTFOLDER + "testResults\\RunningTimeResult.xlsx";
            string PreprocessingTimepath = Utils.ROOTFOLDER + "testResults\\PreprocessingTimeResult.xlsx";
            string PreprocessingMemUsage = Utils.ROOTFOLDER + "testResults\\PreprocessingMemUsage.xlsx";
            string[] cols = new string[] {"a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z","aa","ab","ac","ad","ae","af","ag","ah","ai","aj","ak","al","am","an","ao","ap","aq" };
            Dictionary<int, string[]> kLoc = new Dictionary<int, string[]>();
            kLoc[1] = new string[] { "b","3"};
            kLoc[5] = new string[] { "m", "3" };
            kLoc[10] = new string[] { "x", "3" };
            kLoc[15] = new string[] { "ai", "3" };
            // get location
            Dictionary<string, int> algorimPlus = new Dictionary<string, int>();
            algorimPlus["nt"] = 0;
            algorimPlus["nti"] = 1;
            algorimPlus["ntta"] = 2;
            algorimPlus["ntita"] = 3;
            algorimPlus["pt"] = 4;
            algorimPlus["ptta"] = 5;
            algorimPlus["bt"] = 6;
            algorimPlus["bti"] = 7;
            algorimPlus["btiv"] = 8;
            int col = Array.FindIndex(cols,item => item.Equals(kLoc[k][0]))+algorimPlus[algorithmName]+1;
            int row = queryLength;
            Console.WriteLine("Write test result {0}:{1}",row,col);
            // Write running time result
            Application excel = new Application();
            Workbook workbook = excel.Workbooks.Open(RunningTimepath, ReadOnly: false, Editable: true);
            Worksheet worksheet;
            foreach (Worksheet item in workbook.Worksheets)
            {
                if (item.Name.Equals("connect") && dbName.Equals("connect"))
                {
                    worksheet = item;
                    Range cell = worksheet.Rows.Cells[row, col];
                    cell.Value = totalProcessingTime;
                    break;
                }else
                    if (item.Name.Equals("accidents") && dbName.Equals("accidents"))
                {
                    worksheet = item;
                    Range cell = worksheet.Rows.Cells[row, col];
                    cell.Value = totalProcessingTime;
                    break;
                }
            }
            excel.Application.ActiveWorkbook.Save();
            excel.Application.Quit();
            excel.Quit();

            // Write preprocessing time result
            excel = new Application();
            workbook = excel.Workbooks.Open(PreprocessingTimepath, ReadOnly: false, Editable: true);            
            foreach (Worksheet item in workbook.Worksheets)
            {
                if (item.Name.Equals("connect") && dbName.Equals("connect"))
                {
                    worksheet = item;
                    Range cell = worksheet.Rows.Cells[row, col];
                    cell.Value = avgPreprocessingTime;
                    break;
                }
                else
                    if (item.Name.Equals("accidents") && dbName.Equals("accidents"))
                {
                    worksheet = item;
                    Range cell = worksheet.Rows.Cells[row, col];
                    cell.Value = avgPreprocessingTime;
                    break;
                }
            }
            excel.Application.ActiveWorkbook.Save();
            excel.Application.Quit();
            excel.Quit();
            // Write preprocessing mem usage
            excel = new Application();
            workbook = excel.Workbooks.Open(PreprocessingMemUsage, ReadOnly: false, Editable: true);
            foreach (Worksheet item in workbook.Worksheets)
            {
                if (item.Name.Equals("connect") && dbName.Equals("connect"))
                {
                    worksheet = item;
                    Range cell = worksheet.Rows.Cells[row, col];
                    cell.Value = avgPreprocessingMemUsage;
                    break;
                }
                else
                    if (item.Name.Equals("accidents") && dbName.Equals("accidents"))
                {
                    worksheet = item;
                    Range cell = worksheet.Rows.Cells[row, col];
                    cell.Value = avgPreprocessingMemUsage;
                    break;
                }
            }
            excel.Application.ActiveWorkbook.Save();
            excel.Application.Quit();
            excel.Quit();
        }

    }
}
