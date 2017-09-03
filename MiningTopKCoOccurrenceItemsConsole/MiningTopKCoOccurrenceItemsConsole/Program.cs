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
            List<List<string>> randomPatternList;
            int k;
            string algorithmName;
            double runningTime = 0;
            // Load database
            Console.Write("Database: ");
            string dbName=Console.ReadLine().Trim().ToLower();
            db = Database.getDatabase(dbName);
            Console.WriteLine("Total rows: {0}",db.Count);            
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
            foreach (var p in randomPatternList)
            {
                switch (algorithmName)
                {
                    case "nt":
                        runningTime= runningTime+algorithm.nt(db,p,k);
                        break;
                    case "nti":
                        runningTime = runningTime + algorithm.nti(db, p, k);
                        break;
                    case "ntta":
                        runningTime = runningTime + algorithm.ntta(db, p, k);
                        break;
                    case "ntita":
                        runningTime = runningTime + algorithm.ntita(db, p, k);
                        break;
                    case "pt":
                        runningTime = runningTime + algorithm.pt(db, p, k);
                        break;
                    case "bt":
                        runningTime = runningTime + algorithm.bt(db, p, k);
                        break;
                    case "bti":
                        runningTime = runningTime + algorithm.bti(db, p, k);
                        break;
                    case "btiv":
                        runningTime = runningTime + algorithm.btiv(db, p, k);
                        break;
                }
            }
            Console.WriteLine("Total Running time: {0}ms",runningTime);
            Console.ReadLine();

        }
    }
}
