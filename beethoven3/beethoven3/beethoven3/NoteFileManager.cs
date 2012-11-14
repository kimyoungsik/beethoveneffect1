using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace beethoven3
{
    class NoteFileManager
    {

        public List<NoteFile> noteFiles = new List<NoteFile>();

        public NoteFileManager()
        {

        }

        public void Add(String version,
        String name,
        String artist,
        String mp3,
        String picture)
        
        {
            NoteFile noteFile = new NoteFile(version, name, artist, mp3,picture);
            noteFiles.Add(noteFile);  
        }





    }
}
