using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace catchSongs
{
    public class catchSongsInfo
    {
        public static void Main(string[] args) {
            DAL.getSongList getsongslist = new DAL.getSongList();
            getsongslist.getSongs();
        }
    }
}
