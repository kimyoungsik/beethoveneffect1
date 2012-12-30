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

        public void Add(
        String version,
        String name,
        String artist,
        String mp3,
        String picture)
        
        {
            NoteFile noteFile = new NoteFile(version, name, artist, mp3,picture);
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
