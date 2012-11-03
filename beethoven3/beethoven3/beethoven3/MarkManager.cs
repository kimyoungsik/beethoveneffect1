using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace beethoven3
{
    static class MarkManager
    {
        #region declarations
        private static Texture2D markTexture;
        private static Rectangle markInitialFrame;
        private static int markFrameCount;
        public static List<Mark> Marks = new List<Mark>();

        private static Vector2 mark1Location;
        private static Vector2 mark2Location;
        private static Vector2 mark3Location;
        private static Vector2 mark4Location;
        private static Vector2 mark5Location;
        private static Vector2 mark6Location;

        private static StartNoteManager startNoteManager;

        private static int distance = 100;
        #endregion


        #region initialization

        public static void initialize(
            Texture2D texture,
            Rectangle initialFrame,
            int frameCount,
            Vector2 mark1Loc,
            Vector2 mark2Loc,
            Vector2 mark3Loc,
            Vector2 mark4Loc,
            Vector2 mark5Loc,
            Vector2 mark6Loc,
            StartNoteManager startNoteMana
            )
        {
            markTexture = texture;
            markInitialFrame = initialFrame;
            markFrameCount = frameCount;

            startNoteManager = startNoteMana;
            
            mark1Location=mark1Loc;
            mark2Location=mark2Loc;
            mark3Location=mark3Loc;
            mark4Location=mark4Loc;
            mark5Location=mark5Loc;
            mark6Location=mark6Loc;



            addMark(mark1Loc);
            addMark(mark2Loc);
            addMark(mark3Loc);
            addMark(mark4Loc);
            addMark(mark5Loc);
            addMark(mark6Loc);
            
            //startNoteManager = new StartNoteManager(texture, initialFrame, frameCount);

            startNoteManager.addStartNote(new Vector2(mark1Loc.X, mark1Loc.Y - distance));
            startNoteManager.addStartNote(new Vector2(mark2Loc.X + distance, mark2Loc.Y - distance));
            startNoteManager.addStartNote(new Vector2(mark3Loc.X + distance, mark3Loc.Y + distance));
            startNoteManager.addStartNote(new Vector2(mark4Loc.X, mark4Loc.Y + distance));
            startNoteManager.addStartNote(new Vector2(mark5Loc.X - distance, mark5Loc.Y + distance));
            startNoteManager.addStartNote(new Vector2(mark6Loc.X - distance, mark6Loc.Y - distance));

        }

        #endregion

        
        
        #region method
        public static void addMark(Vector2 location)
        {
            Mark thisMark = new Mark(
                markTexture,
                location,
                markInitialFrame,
                markFrameCount);
            Marks.Add(thisMark);
        }
        public static void deleteAllMarks()
        {
           // for (int i = 0; i < 6; i++)
            for (int i = 0; i < Marks.Count; i++)
            {
                Marks.RemoveAt(i);
            }
        }
        #endregion
       
        #region update and draw
        public static void Update(GameTime gameTime)
        {
      
            foreach (Mark mark in Marks)
            {
            
                mark.Update(gameTime);
            }

        }

        public static void Draw(SpriteBatch spriteBatch)
        {

            foreach (Mark mark in Marks)
            {
                mark.Draw(spriteBatch);
            }
        }
        #endregion
    }
}
