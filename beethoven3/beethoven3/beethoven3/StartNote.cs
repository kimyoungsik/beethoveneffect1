using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace beethoven3
{
    class StartNote
    {

        #region declarations
        public Sprite StartNoteSprite;
        private float speed = 120f;

        //변환시 사라짐
        public bool Disappear = false;

        private int startNoteRadius = 15;

        private Vector2 location;
        #endregion

        #region constructor
        public StartNote(
            Texture2D texture,
            Vector2 location,
            Rectangle initialFrame,
            int frameCount)
        {
            StartNoteSprite = new Sprite(
                location,
                texture,
                initialFrame,
                Vector2.Zero);

            for (int x = 1; x < frameCount; x++)
            {
                StartNoteSprite.AddFrame(
                    new Rectangle(
                        initialFrame.X = (initialFrame.Width * x),
                        initialFrame.Y,
                        initialFrame.Width,
                        initialFrame.Height));
            }
            this.location = location;
            StartNoteSprite.CollisionRadius = startNoteRadius;
        }
        #endregion


        #region update and draw
        public void Update(GameTime gameTime)
        {
            StartNoteSprite.Update(gameTime);


        }

        public void Draw(SpriteBatch spriteBatch)
        {

            StartNoteSprite.Draw(spriteBatch);

        }

        #endregion
    }
}
