using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiningTopKCoOccurrenceItemsConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            List<List<string>> db;            
            int[] listk=new int[] { 1,5,10,15};
            int[] listQueryLength = new int[] { 3, 4, 5, 6, 7 };
            //int[] listk = new int[] { 1 };
            //int[] listQueryLength = new int[] { 3};
            string algorithmName;
            // Load database
            Console.Write("Database: ");
            string dbName=Console.ReadLine().Trim().ToLower();
            db = Database.getDatabase(dbName);
            
                        // Generate test queries
                       // foreach (var length in listQueryLength)
                       //{
                       //     Utils.genQueryItemsetsToFile(dbName, length);
                       // }
                       // return;     
            // Algorithm
            Console.Write("Algorithm: ");
            algorithmName = Console.ReadLine();
            foreach(var k in listk)
            {
                foreach(var length in listQueryLength)
                {
                    runAlgorithm(k, length, algorithmName, dbName, db);
                }
            }

            
            Console.ReadLine();
        }
        private static void runAlgorithm(int k, int queryLength, string algorithmName,string dbName,List<List<string>> db)
        {
            List<List<string>> randomPatternList = Utils.loadQueryItemsets(dbName, queryLength);
            Algorithm algorithm = new Algorithm();
            List<AlgorithmResult> listTestResults = new List<AlgorithmResult>();
            string outPutFileName = Utils.ROOTFOLDER + "testResults\\" + algorithmName + "_" + dbName + "_" + queryLength + "_" + k + "_" + System.DateTime.Now.ToString("dd-MM-yyyy_HH_mm_ss") + ".txt";
            System.IO.StreamWriter fileOutput = new System.IO.StreamWriter(outPutFileName);
            int i = 0;
            foreach (var p in randomPatternList)
            {
                i++;
                if (algorithmName.Equals("nt"))
                {
                    Console.WriteLine("{0}. {1}", i, string.Join(",", p));
                    AlgorithmResult result = algorithm.nt(db, p, k);
                    listTestResults.Add(result);
                }
                else if (algorithmName.Equals("nti"))
                {
                    Console.WriteLine("{0}. {1}", i, string.Join(",", p));
                    AlgorithmResult result = algorithm.nti(db, p, k);
                    listTestResults.Add(result);
                }
                else if (algorithmName.Equals("ntta"))
                {
                    Console.WriteLine("{0}. {1}", i, string.Join(",", p));
                    AlgorithmResult result = algorithm.ntta(db, p, k);
                    listTestResults.Add(result);
                }
                else if (algorithmName.Equals("ntita"))
                {
                    Console.WriteLine("{0}. {1}", i, string.Join(",", p));
                    AlgorithmResult result = algorithm.ntita(db, p, k);
                    listTestResults.Add(result);
                }
                else if (algorithmName.Equals("pt"))
                {
                    Console.WriteLine("{0}. {1}", i, string.Join(",", p));
                    AlgorithmResult result = algorithm.pt(db, p, k);
                    listTestResults.Add(result);
                }
                else if (algorithmName.Equals("bt"))
                {
                    Console.WriteLine("{0}. {1}", i, string.Join(",", p));
                    AlgorithmResult result = algorithm.bt(db, p, k);
                    listTestResults.Add(result);
                }
                else if (algorithmName.Equals("bti"))
                {
                    Console.WriteLine("{0}. {1}", i, string.Join(",", p));
                    AlgorithmResult result = algorithm.bti(db, p, k);
                    listTestResults.Add(result);
                }
                else if (algorithmName.Equals("btiv"))
                {
                    Console.WriteLine("{0}. {1}", i, string.Join(",", p));
                    AlgorithmResult result = algorithm.btiv(db, p, k);
                    listTestResults.Add(result);
                }
            }
            // write result to file
            double totalProcessingTime = 0;
            double totalTimeToBuildData = 0;
            long totalPreprocessingMemUsage = 0;
            foreach (var result in listTestResults)
            {
                fileOutput.WriteLine("--------------------");
                IEnumerable<KeyValuePair<string, int>> lk = result.getListTopK();
                foreach (var item in lk)
                {
                    fileOutput.WriteLine("{0}:{1}", item.Key, item.Value);
                }
                totalProcessingTime = totalProcessingTime + result.getRunningTime();
                totalTimeToBuildData = totalTimeToBuildData + result.getTimeToBuildDatbase();
                totalPreprocessingMemUsage = totalPreprocessingMemUsage + result.getMemDatabase();
                fileOutput.WriteLine("Time to build data: {0}ms", result.getTimeToBuildDatbase());
                fileOutput.WriteLine("Mem Usage to preprocessing phase: {0}MB", result.getMemDatabase()/ 1048576);
                fileOutput.WriteLine("Time to processing: {0}ms", result.getRunningTime());
                
            }
            fileOutput.WriteLine("===========================================");
            fileOutput.WriteLine("Avg time to build Pitree or Bittable: {0}ms", totalTimeToBuildData / listTestResults.Count);
            fileOutput.WriteLine("Avg preprocessing Mem Usage to build Pitree or Bittable: {0}MB", totalPreprocessingMemUsage/ 1048576 / listTestResults.Count);
            fileOutput.WriteLine("Total Running time: {0}ms", totalProcessingTime);            
            fileOutput.Close();
            Console.WriteLine("===========================================");
            Console.WriteLine("Avg time to build Pitree or Bittable: {0}ms", totalTimeToBuildData / listTestResults.Count);
            Console.WriteLine("Avg preprocessing Mem Usage to build Pitree or Bittable: {0}MB", totalPreprocessingMemUsage / 1048576 / listTestResults.Count);
            Console.WriteLine("Total Running time: {0}ms", totalProcessingTime);
            Utils.WriteTestResult(dbName, algorithmName, k, queryLength, (int)totalProcessingTime, (int)(totalTimeToBuildData / listTestResults.Count),(int)(totalPreprocessingMemUsage/ 1048576 / listTestResults.Count));
        }
    }
}
