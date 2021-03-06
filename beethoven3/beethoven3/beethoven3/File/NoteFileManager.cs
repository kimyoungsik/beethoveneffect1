﻿using System;
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
        private String fileName;
        
        private int level;
        private double startTime;
        private double endTime;
        private int bpm;
        

        public NoteFile(String fileName, String version,
            int level,
        String name,
        String artist,
        String mp3,
        String picture,
        double startTime,
        double endTime,
            int bpm
            )
        {
            this.level = level;

            this.version = version; ;
            this.name = name;
            this.artist = artist;
            this.mp3 = mp3;
            this.picture = picture;
            this.startTime = startTime;
            this.endTime = endTime;
            this.bpm = bpm;
            this.fileName = fileName;
        }
        public String FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        public String Version
        {
            get { return version; }
            set { version = value; }
        }
        public int Level
        {
            get { return level; }
            set { level = value; }
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
        public int Bpm
        {
            get { return bpm; }
            set { bpm = value; }
        }

    }
    class NoteFileManager
    {

        public List<NoteFile> noteFiles = new List<NoteFile>();

        public NoteFileManager()
        {

        }

        public void Add(
            String fileName,
        String version,
            int level,
        String name,
        String artist,
        String mp3,
        String picture,
        double startTime,
        double endTime,
        int bpm)
        
        {
            NoteFile noteFile = new NoteFile(fileName,version, level, name, artist, mp3, picture, startTime, endTime, bpm);
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
