using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
namespace beethoven3
{
    static class SoundManager
    {
        public static Song[] soundEngine = new Song[100];
     //   public static SoundEffectInstance[] soundEngineInstance = new SoundEffectInstance[100];
        public static List<String> sndFiles = new List<String>();

        static public void Init()
        {
          AddSndFile("snd/ka");
          AddSndFile("snd/jo");
          AddSndFile("snd/maid");

        }

       

        static public int AddSndFile(String name)
        {

            sndFiles.Add(name);

            return sndFiles.Count + 1;

        }

        static public int FindSound(string name)
        
        {
            //List<String> names = new List<String>();
            //names.Add(name);
           ///String index = sndFiles.Find(delegate(String p) { return p == name; });
           int index = -1;
           int i;
           for (i = 0; i < sndFiles.Count(); i++)
           {
               if (sndFiles[i] == name)
               {
                   index = i;
                   i = sndFiles.Count();
               }

           }
           

           // String index = sndFiles.Find(System.Predicate<String>name);
            return index;

        }

        static public void SndPlay(string name)
        {
            int index = FindSound(name);
            MediaPlayer.Play(soundEngine[index]);

        }

        static public void SndStop()
        {
         //   int index = FindSound(name);
           // soundEngine[index].Stop();
            MediaPlayer.Stop();

        }


    }
}
