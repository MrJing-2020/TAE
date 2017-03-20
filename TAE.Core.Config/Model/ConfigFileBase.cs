using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Core.Config
{
    public abstract class ConfigFileBase
    {
        public ConfigFileBase() { }
        public int Id { get; set; }

        public virtual bool ClusteredByIndex
        {
            get { return false; }
        }
        internal virtual void Save() { }

        internal virtual void UpdateNodeList<T>(List<T> nodeList) where T : ConfigNodeBase
        {
            foreach (var node in nodeList)
            {
                if (node.Id > 0)
                    continue;
                node.Id = nodeList.Max(n => n.Id) + 1;
            }
        }
    }
}
