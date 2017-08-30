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
                }
                algorithm.nt(db, p, k);
            }
            Console.WriteLine("Total Running time: {0}ms",runningTime);
            Console.ReadLine();

        }
    }
}
