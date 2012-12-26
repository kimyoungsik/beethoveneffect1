using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 
namespace beethoven3
{
    class NoteFile
    {

        private String version;
        private String name;
        private String artist;
        private String mp3;
        private String picture;

        public NoteFile( String version,
        String name,
        String artist,
        String mp3,
        String picture
            )
        {
        this.version = version;;
        this.name = name;
        this.artist = artist;
        this.mp3 = mp3;
        this.picture = picture;


        }


        public String Version
        {
            get { return version; }
            set { version = value; }
        }

        public String Name
        {
            get { return name; }
            set { name = value; }
        }
        public String Mp3
        {
            get { return mp3; }
            set { mp3 = value; }
        }
        public String Picture
        {
            get { return picture; }
            set { picture = value; }
        }
        public String Artist
        {
            get { return artist; }
            set { artist = value; }
        }

    }
}
