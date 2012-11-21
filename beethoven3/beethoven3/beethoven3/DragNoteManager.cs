﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace beethoven3
{
    static class DragNoteManager
    {
        #region declarations
        public static List<Sprite> DragNotes = new List<Sprite>();

        private static Texture2D Texture;
        private static Rectangle InitialFrame;
        private static int FrameCount;
        private static float NoteSpeed;
        private static int CollisionRadius;
        private static ExplosionManager BadManager;
        private static ScoreManager ScoreManager;
        
        #endregion


        #region initialize

        public static void initialize(
        
            Texture2D texture,
            Rectangle initialFrame,
            int frameCount,
            int collisionRadius,
            float noteSpeed,
            ExplosionManager badManager,
            ScoreManager scoreManager
            )
        {
            Texture = texture;
            InitialFrame = initialFrame;
            FrameCount = frameCount;
            CollisionRadius = collisionRadius;
            NoteSpeed = noteSpeed;
            BadManager = badManager;
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

                }
            }
        }
        public static void DeleteDragNotes()
        {
            int i;
            for (i = 0; i < DragNotes.Count; i++ )
            {
               // try
               // {
                    BadManager.AddExplosion(DragNotes[i].Center, Vector2.Zero);
                    DragNotes.RemoveAt(i);
                    ScoreManager.DragNoteScore = ScoreManager.DragNoteScore + 1;
                    if (ScoreManager.Combo > ScoreManager.Max)
                    {
                        ScoreManager.Max = ScoreManager.Combo;
                    }
    

                    ScoreManager.Combo = 0;
                    


               // }
               // catch (ArgumentOutOfRangeException)
              //  {

               // }
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
