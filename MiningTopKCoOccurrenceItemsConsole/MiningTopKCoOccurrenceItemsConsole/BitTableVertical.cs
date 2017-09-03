using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiningTopKCoOccurrenceItemsConsole
{
    class BitTableVertical
    {
        private Dictionary<string, BitArray> data;
        public BitTableVertical(List<List<string>> db)
        {
            this.data = new Dictionary<string, BitArray>();
            // Scan list items
            List<string> items = new List<string>();
            foreach (var tran in db)
            {
                foreach (var i in tran)
                {
                    if (!items.Contains(i)) items.Add(i);
                }
            }
            // Build null data
            foreach (var item in items)
            {
                this.data[item] = new BitArray(db.Count);
            }
            // Put data value
            for(int i = 0; i < db.Count; i++)
            {
                List<string> tran = db[i];
                foreach (var item in tran)
                {
                    this.data[item][i] = true;
                }
            }
        }
        public Dictionary<string,BitArray> getData()
        {
            return this.data;
        }
        public void show()
        {
            foreach (var item in this.data)
            {
                Console.Write("{0}: ",item.Key);
                for(int i = 0; i < item.Value.Length; i++)
                {
                    if (i > 0) Console.Write(" ");
                    if (item.Value[i]) Console.Write("1");
                    if (!item.Value[i]) Console.Write("0");
                    if (i == item.Value.Length - 1) Console.WriteLine(); 
                }
            }
        }
    }
}
