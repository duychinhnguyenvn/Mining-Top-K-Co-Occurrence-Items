using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiningTopKCoOccurrenceItemsConsole
{
    class PiTreeNode
    {
        private string label;
        private int count;
        private List<PiTreeNode> childrenNodes;
        private PiTreeNode parentNode;
        public PiTreeNode(string label)
        { 
            this.label = label;
            this.count = 1;
            childrenNodes = new List<PiTreeNode>();
            parentNode = null;
        }
        public void increaseCount()
        {
            this.count++;
        }
        public void setParentLink(PiTreeNode parentNode)
        {
            this.parentNode = parentNode;
        }
        public void addChildrenNode(PiTreeNode childrenNode)
        {
            this.childrenNodes.Add(childrenNode);
        }
        public PiTreeNode getParentNode()
        {
            return parentNode;
        }
        public List<PiTreeNode> getChildrenNodes()
        {
            return this.childrenNodes;
        }
        public int getCount()
        {
            return this.count;
        }
        public string getLabel()
        {
            return this.label;
        }
    }
}
