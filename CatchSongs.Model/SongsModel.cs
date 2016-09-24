using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatchSongs.Model
{
    public class Song
    {
        private int songId = 0;

        public int SongId
        {
            get { return songId; }
            set { songId = value; }
        }
        private string songName = string.Empty;

        public string SongName
        {
            get { return songName; }
            set { songName = value; }
        }
        private string artist = string.Empty;

        public string Artist
        {
            get { return artist; }
            set { artist = value; }
        }
        private int count = 0;

        public int Count
        {
            get { return count; }
            set { count = value; }
        }
    }
   

}
