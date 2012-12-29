using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace beethoven3
{
    class PictureInfo
    {
        Texture2D picture;
        String name;

        public PictureInfo(Texture2D picture, String name)
        {
            this.picture = picture;
            this.name = name;

        }

        public Texture2D Picture
        {
            get { return picture; }
            set { picture = value; }
        }

        public String Name
        {
            get { return name; }
            set { name = value; }

        }
    }
    class ReportManager
    {
        //최고 5개만 가지고 있는 reportManager을 위한 class
        private List<ScoreInfoManager> scoreInfoManagers = new List<ScoreInfoManager>();

        private List<PictureInfo> userPictures = new List<PictureInfo>();

        private ScoreManager scoreManager;


        public ReportManager(ScoreManager scoreManager )
        {
            this.scoreManager = scoreManager;
        }
        
        public void SaveReport()
        {
            TextWriter tw = new StreamWriter("c:\\beethovenRecord\\record.txt");
            int i, j;
            for (i = 0; i < scoreInfoManagers.Count; i++)
            {
                tw.WriteLine("%%");
                tw.WriteLine(scoreInfoManagers[i].SongName);
                List<ScoreInfo> scoreInfos = scoreInfoManagers[i].GetScoreInfos();
                for (j = 0; j < scoreInfos.Count; j++)
                {
                    tw.WriteLine(scoreInfos[j].UserPicture + '$' + scoreInfos[j].Score);
                }   
            }
            tw.WriteLine("**");
            tw.WriteLine(scoreManager.TotalGold);
                       
            tw.Close();
        }


        public void LoadReport()
        {

            StreamReader sr = new StreamReader("c:\\beethovenRecord\\record.txt");
            String line;
            String songTitle=null;
            String[] contents;
            line = sr.ReadLine();
            while (line != "**")//처음
               {
                   if (line == "%%")
                   {
                       //노래 제목
                       songTitle = sr.ReadLine();
                   }
                   else
                   {
                       contents = ((String)line).Split('$');
                       this.AddSongInfoManager(songTitle, Int32.Parse(contents[1]), contents[0]); // contents[0] 그림 , contents[1]  점수
                   }
                   line = sr.ReadLine();          
            }
            line = sr.ReadLine();

            scoreManager.TotalGold = Int32.Parse(line);
            
            sr.Close();
        }


        //곡 이름에 따라서 점수 가져오기

        //public List<int> GetHighScore(String songName)
        //{
        //    List<int> highScores = new List<int>();
        //    int i;
        //    for (i = 0; i < scoreInfoManagers.Count; i++)
        //    {
        //        if (scoreInfoManagers[i].SongName == songName)
        //        {
        //            highScores = scoreInfoManagers[i].GetFiveScore();
        //        }
        //    }
        //    return highScores;
        //}


        public List<ScoreInfo> GetHighScore(String songName)
        {
            List<ScoreInfo> highScores = new List<ScoreInfo>();
            int i;
            for (i = 0; i < scoreInfoManagers.Count; i++)
            {
                if (scoreInfoManagers[i].SongName == songName)
                {
                    highScores = scoreInfoManagers[i].GetFiveScore();
                }
            }
            return highScores;
        }


        //곡이름에 따라 새로 등록된 것인지 체크
        public bool IsHighScore(String songName, int score)
        {
            bool ret = false;

            int i;

          
                for (i = 0; i < scoreInfoManagers.Count; i++)
                {
                    if (scoreInfoManagers[i].SongName == songName)
                    {
                        ret = scoreInfoManagers[i].IsHighScore(score);
                        i = scoreInfoManagers.Count;
                    }

                }
          
            return ret;


        }
        // 곡 이름에 따라 찾아서 더하기

        public void AddSongInfoManager(String songName, int score, String userPicture)
        {
            int j = -1;
            int i;

            for (i = 0; i < scoreInfoManagers.Count; i++)
            {
                if (scoreInfoManagers[i].SongName == songName)
                {
                    j = i;
                    i = scoreInfoManagers.Count;
                }
            }
            //곡이름이 같은것이 있으면
            if (j != -1)
            {
                scoreInfoManagers[j].AddScoreInfo(new ScoreInfo(userPicture, score));
            }
            //없으면
            else
            {
                ScoreInfoManager newScoreInfoManager = new ScoreInfoManager();
                newScoreInfoManager.SongName = songName;
                newScoreInfoManager.AddScoreInfo(new ScoreInfo(userPicture,score));

                scoreInfoManagers.Add(newScoreInfoManager);
            }
        }

        //하이스코어에 대한 그림들을 가지고 있는다음에

        //요청이 오면 그냥 보내주기만 한다.

        public void MakePictures(String songName, GraphicsDevice graphicsdevice)
        {
           List<ScoreInfo> scoreInfos =  GetHighScore(songName);

           int i;

           for (i = 0; i < scoreInfos.Count; i++)
           {

               Texture2D tex = FindPicture(scoreInfos[i].UserPicture);

               //이미 있으면 또 만들지 않는다.
               if (tex == null)
               {
                   FileStream fileStream = new FileStream(@"C:\\beethovenRecord\\userPicture\\" + scoreInfos[i].UserPicture, FileMode.Open);


                   Texture2D texture = Texture2D.FromStream(graphicsdevice, fileStream);

                   userPictures.Add(new PictureInfo(texture, scoreInfos[i].UserPicture));

               }
           }
        }


        public Texture2D FindPicture(String name)
        {
            int i;
            Texture2D texture= null;
            for (i = 0; i < userPictures.Count; i++)
            {
                if (userPictures[i].Name == name)
                {
                    texture = userPictures[i].Picture;
                    i = userPictures.Count;
                }
            }
            return texture;
        }

        //public bool IsInAbulm(String name)
        //{
        //    bool ret = false;
        //    for (i = 0; i < userPictures.Count; i++)
        //    {
        //        if (userPictures[i].Name == name)
        //        {
        //            texture = userPictures[i].Picture;
        //            i = userPictures.Count;
        //        }
        //    }

        //    return ret;
        //}



    }
}
