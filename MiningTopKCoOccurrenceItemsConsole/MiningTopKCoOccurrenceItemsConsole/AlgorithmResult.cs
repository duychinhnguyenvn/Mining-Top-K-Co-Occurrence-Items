using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiningTopKCoOccurrenceItemsConsole
{
    class AlgorithmResult
    {
        private double runningTime;
        private long memDatabase;
        private double timeToBuildDatabase;
        private IEnumerable<KeyValuePair<string,int>>  lk;
        public AlgorithmResult(double runningTime,long memDatabase, double timeToBuildDatabase, IEnumerable<KeyValuePair<string, int>> lk)
        {
            this.runningTime = runningTime; // ms
            this.memDatabase = memDatabase; //mb
            this.timeToBuildDatabase = timeToBuildDatabase; //ms
            this.lk = lk;
            
        }
        public double getRunningTime()
        {
            return this.runningTime;
        }
        public long getMemDatabase()
        {
            return this.memDatabase;
        }
        public double getTimeToBuildDatbase()
        {
            return this.timeToBuildDatabase;
        }
        public IEnumerable<KeyValuePair<string, int>> getListTopK()
        {
            return this.lk;
        }
    }
}
