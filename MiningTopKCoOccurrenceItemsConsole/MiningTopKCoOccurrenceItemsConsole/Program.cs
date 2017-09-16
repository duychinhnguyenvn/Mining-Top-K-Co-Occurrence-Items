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
            string[][] db;
            List<List<string>> randomPatternList;
            int k;
            string algorithmName;
            // Load database
            Console.Write("Database: ");
            string dbName=Console.ReadLine().Trim().ToLower();
            db = Database.getDatabase(dbName);
            Console.WriteLine("Total rows: {0}",db.Length);            
            // Generate random queries
            Console.Write("Query length: ");
            int queryLength = int.Parse(Console.ReadLine());
            // Generate test queries
            //Utils.genQueryItemsetsToFile(dbName, queryLength);
            randomPatternList = Utils.loadQueryItemsets(dbName,queryLength);
            // Get top k input
            Console.Write("Top-k: ");
            k = int.Parse(Console.ReadLine());
            // Algo
            Console.Write("Algorithm: ");
            algorithmName = Console.ReadLine();
            Algorithm algorithm = new Algorithm();
            List<AlgorithmResult> listTestResults = new List<AlgorithmResult>();
            string outPutFileName = Utils.ROOTFOLDER + "testResults\\"+algorithmName+"_"+dbName+"_"+queryLength+"_"+k+"_"+ System.DateTime.Now.ToString("dd-MM-yyyy_HH_mm_ss")+".txt";
            System.IO.StreamWriter fileOutput= new System.IO.StreamWriter(outPutFileName);
            int i = 0;
            foreach (var p in randomPatternList)
            {
                i++;
                if (algorithmName.Equals("nt"))
                {
                    Console.WriteLine("{0}. {1}",i,string.Join(",",p));
                    //AlgorithmResult result = algorithm.nt(db, p, k);
                    //listTestResults.Add(result);                   
                }
                else if (algorithmName.Equals("nti"))
                {
                    Console.WriteLine("{0}. {1}", i, string.Join(",", p));
                    //AlgorithmResult result = algorithm.nti(db, p, k);
                    //listTestResults.Add(result);
                }
                else if (algorithmName.Equals("ntta"))
                {
                    Console.WriteLine("{0}. {1}", i, string.Join(",", p));
                    //AlgorithmResult result = algorithm.ntta(db, p, k);
                    //listTestResults.Add(result);
                }
                else if (algorithmName.Equals("ntita"))
                {
                    Console.WriteLine("{0}. {1}", i, string.Join(",", p));
                    //AlgorithmResult result = algorithm.ntita(db, p, k);
                    //listTestResults.Add(result);
                }
                else if (algorithmName.Equals("pt"))
                {
                    Console.WriteLine("{0}. {1}", i, string.Join(",", p));
                    //AlgorithmResult result = algorithm.pt(db, p, k);
                    //listTestResults.Add(result);
                }
                else if (algorithmName.Equals("btiv"))
                {
                    Console.WriteLine("{0}. {1}", i, string.Join(",", p));
                    //AlgorithmResult result = algorithm.btiv(db, p, k);
                    //listTestResults.Add(result);
                }
            }
            // write result to file
            int totalProcessingTime = 0;
            int totalTimeToBuildData = 0;
            foreach (var result in  listTestResults)
            {
                fileOutput.WriteLine("--------------------");
                IEnumerable<KeyValuePair<string, int>> lk = result.getListTopK();
                foreach (var item in lk)
                {
                    fileOutput.WriteLine("{0}:{1}",item.Key,item.Value);
                }
                totalProcessingTime = totalProcessingTime + result.getRunningTime();
                totalTimeToBuildData = totalTimeToBuildData + result.getTimeToBuildDatbase();
                fileOutput.WriteLine("Time to build data: {0}ms",result.getTimeToBuildDatbase());
                fileOutput.WriteLine("Time to processing: {0}ms", result.getRunningTime());
            }
            fileOutput.WriteLine("===========================================");            
            fileOutput.WriteLine("Avg time to build Pitree or Bittable: {0}ms",totalTimeToBuildData/listTestResults.Count);
            fileOutput.WriteLine("Total Running time: {0}ms", totalProcessingTime);
            fileOutput.Close();
            Console.WriteLine("===========================================");
            Console.WriteLine("Avg time to build Pitree or Bittable: {0}ms", totalTimeToBuildData / listTestResults.Count);
            Console.WriteLine("Total Running time: {0}ms", totalProcessingTime);
            Console.ReadLine();
        }
    }
}
