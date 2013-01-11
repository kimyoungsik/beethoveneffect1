﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace beethoven3
{
    static class GoldManager
    {
        #region declarations
        public static List<Sprite> Golds = new List<Sprite>();

        private static Texture2D Texture;
        private static Rectangle InitialFrame;
        private static int FrameCount;
        private static float NoteSpeed;
        private static int CollisionRadius;
        private static float Scale;
        #endregion


        #region initialize

        public static void initialize(

            Texture2D texture,
            Rectangle initialFrame,
            int frameCount,
            int collisionRadius,
            float noteSpeed,
            float scale

            )
        {
            Texture = texture;
            InitialFrame = initialFrame;
            FrameCount = frameCount;
            CollisionRadius = collisionRadius;
            NoteSpeed = noteSpeed;
            Scale = scale;
        }
        #endregion

        #region method
        public static void MakeGold(
            Vector2 location,
            Vector2 velocity
            )
        {
            Sprite gold = new Sprite(
                location,
                Texture,
                InitialFrame,
                velocity,
                Scale);

            gold.Velocity *= NoteSpeed;

            for (int x = 1; x < FrameCount; x++)
            {
                gold.AddFrame(new Rectangle(
                    InitialFrame.X + (InitialFrame.Width * x),
                    InitialFrame.Y,
                    InitialFrame.Width,
                    InitialFrame.Height));
            }
            gold.CollisionRadius = CollisionRadius;
            Golds.Add(gold);
        }

        public static void DeleteAll()
        {
           
            int count = Golds.Count;
           
            while (Golds.Count > 0)
            {
                Golds.RemoveAt(0);
                    
            }
          
        }


        #endregion


        #region update and draw

        public static void Update(GameTime gameTime)
        {
            for (int x = Golds.Count - 1; x >= 0; x--)
            {
                Golds[x].Update(gameTime);

            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (Sprite gold in Golds)
            {
                gold.Draw(spriteBatch);
            }
        }

        #endregion

    }
}
