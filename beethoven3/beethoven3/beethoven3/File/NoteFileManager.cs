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
        private double startTime;
        private double endTime;

        public NoteFile(String version,
        String name,
        String artist,
        String mp3,
        String picture,
        double startTime,
        double endTime
            )
        {
            this.version = version; ;
            this.name = name;
            this.artist = artist;
            this.mp3 = mp3;
            this.picture = picture;
            this.startTime = startTime;
            this.endTime = endTime;
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
        public double StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }
        public double EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }

    }
    class NoteFileManager
    {

        public List<NoteFile> noteFiles = new List<NoteFile>();

        public NoteFileManager()
        {

        }

        public void Add(
        String version,
        String name,
        String artist,
        String mp3,
        String picture,
        double startTime,
        double endTime)
        
        {
            NoteFile noteFile = new NoteFile(version, name, artist, mp3,picture,startTime,endTime);
            noteFiles.Add(noteFile);  
        }

        //노트파일 곡제목이 있으면 노트파일을 찾을 수 있다.

        public NoteFile FindNoteFile(String name)
        {
            NoteFile noteFile= null;
            int i;
            for (i = 0; i < noteFiles.Count(); i++)
            {
                if (name == noteFiles[i].Name)
                {
                    noteFile = noteFiles[i];
                    i = noteFiles.Count();
                }
            }
            return noteFile;

        }




    }
}
