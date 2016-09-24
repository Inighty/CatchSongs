using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatchSongs.model
{
    public class ParamModel
    {
        private int startId = 0;

        public int StartId
        {
            get { return startId; }
            set { startId = value; }
        }
        private int endId = 0;

        public int EndId
        {
            get { return endId; }
            set { endId = value; }
        }
    }
}
