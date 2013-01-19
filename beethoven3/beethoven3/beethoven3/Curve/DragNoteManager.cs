﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Diagnostics;
namespace beethoven3
{
    //충돌로 판별하는 마크르 만드는 것
    static class DragNoteManager
    {
        #region declarations
        public static List<Sprite> DragNotes = new List<Sprite>();

        public static Texture2D Texture;
        public static Rectangle InitialFrame;
        private static int FrameCount;

        //노트 스피드로서 역할을 제대로 하지 않는다.
        public static float NoteSpeed;
        private static int CollisionRadius;
      
        private static ScoreManager ScoreManager;
        private static MissBannerManager MissBanner;
        #endregion


        #region initialize

        public static void initialize(
        
            Texture2D texture,
            Rectangle initialFrame,
            int frameCount,
            int collisionRadius,
            float noteSpeed,
            MissBannerManager missBanner,
            ScoreManager scoreManager
            )
        {
            Texture = texture;
            InitialFrame = initialFrame;
            FrameCount = frameCount;
            CollisionRadius = collisionRadius;
            NoteSpeed = noteSpeed;
             MissBanner = missBanner;
            ScoreManager = scoreManager;
        }
        #endregion

        #region method
        public static void MakeDragNote(
            Vector2 location,
            Vector2 velocity
            )
        {
            Sprite thisNote = new Sprite(
                location,
                Texture,
                InitialFrame,
                velocity);

            thisNote.Velocity *= NoteSpeed;

            for (int x = 1; x < FrameCount; x++)
            {
                thisNote.AddFrame(new Rectangle(
                    InitialFrame.X + (InitialFrame.Width * x),
                    InitialFrame.Y,
                    InitialFrame.Width,
                    InitialFrame.Height));
            }
            thisNote.CollisionRadius = CollisionRadius;
            DragNotes.Add(thisNote);
        }

        public static void DeleteDragNoteFromFront()
        {
            if (DragNotes.Count > 0)
            {
                try
                {

                    DragNotes.RemoveAt(0);
                }
                catch (ArgumentOutOfRangeException)
                {
                    //Trace.WriteLine("out of range in dragnote");
                }
            }
        }
        public static void DeleteDragNotes()
        {
            int i;
            for (i = 0; i < DragNotes.Count; i++ )
            {
             
                //미스 띄워주기 
                MissBanner.AddBanners(new Vector2(1024 / 2 - 975 / 4, 769 / 2 - 412 / 4));

                DragNotes.RemoveAt(i);
                //***이거 잘못된듯 
                //드래그 노트실패하면 깍아야 할듯  
               // ScoreManager.DragNoteScore = ScoreManager.DragNoteScore + 1;
                
                
                if (ScoreManager.Combo > ScoreManager.Max)
                {
                    ScoreManager.Max = ScoreManager.Combo;
                }
                ScoreManager.Combo = 0;
                ScoreManager.Gage = ScoreManager.Gage - 1;
   
            }
        }

        #endregion


        #region update and draw
        
        public static void Update(GameTime gameTime)
        {
            for (int x = DragNotes.Count - 1; x >= 0; x--)
            {
                DragNotes[x].Update(gameTime);
                
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (Sprite dragNote in DragNotes)
            {
                dragNote.Draw(spriteBatch);
            }
        }

        #endregion
      
    }
}
