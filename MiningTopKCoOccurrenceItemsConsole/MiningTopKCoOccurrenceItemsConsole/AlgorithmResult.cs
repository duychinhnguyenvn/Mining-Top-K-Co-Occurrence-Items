using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiningTopKCoOccurrenceItemsConsole
{
    class AlgorithmResult
    {
        private int runningTime;
        private int memDatabase;
        private int timeToBuildDatabase;
        private IEnumerable<KeyValuePair<string,int>>  lk;
        public AlgorithmResult(int runningTime,int memDatabase,int timeToBuildDatabase, IEnumerable<KeyValuePair<string, int>> lk)
        {
            this.runningTime = runningTime; // ms
            this.memDatabase = memDatabase; //mb
            this.timeToBuildDatabase = timeToBuildDatabase; //ms
            this.lk = lk;
            
        }
        public int getRunningTime()
        {
            return this.runningTime;
        }
        public int getMemDatabase()
        {
            return this.memDatabase;
        }
        public int getTimeToBuildDatbase()
        {
            return this.timeToBuildDatabase;
        }
        public IEnumerable<KeyValuePair<string, int>> getListTopK()
        {
            return this.lk;
        }
    }
}
