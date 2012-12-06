using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;

namespace beethoven3
{
    class ShopDoor
    {

        private Texture2D rightHand;
        private Texture2D leftHand;
        private Texture2D note;
        private Texture2D effect;
        private Texture2D background;

        private Texture2D hoverRightHand;
        private Texture2D hoverLeftHand;
        private Texture2D hoverNote;
        private Texture2D hoverEffect;
        private Texture2D hoverBackground;

        private Rectangle recRightHand;
        private Rectangle recLeftHand;
        private Rectangle recNote;
        private Rectangle recEffect;
        private Rectangle recBackground;


        private bool clickRightHand;
        private bool clickLeftHand;
        private bool clickNote;
        private bool clickEffect;
        private bool clickBackground;

        public ShopDoor()
        {
          //  button1 = false;
             clickRightHand=false;
             clickLeftHand = false;
             clickNote = false;
             clickEffect = false;
             clickBackground = false;

        }

        public void LoadContent(ContentManager cm)
        {
         rightHand= cm.Load<Texture2D>(@"shopdoor\shopDoor1");
         leftHand= cm.Load<Texture2D>(@"shopdoor\shopDoor2");
         note = cm.Load<Texture2D>(@"shopdoor\shopDoor3");
         effect= cm.Load<Texture2D>(@"shopdoor\shopDoor4");
         background = cm.Load<Texture2D>(@"shopdoor\shopDoor5");

         hoverRightHand = cm.Load<Texture2D>(@"shopdoor\changed1");
         hoverLeftHand = cm.Load<Texture2D>(@"shopdoor\changed2");
         hoverNote = cm.Load<Texture2D>(@"shopdoor\changed3");
         hoverEffect = cm.Load<Texture2D>(@"shopdoor\changed4");
         hoverBackground = cm.Load<Texture2D>(@"shopdoor\changed5");
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch,int width,int height)
        {
            recRightHand = new Rectangle(width / 2 - (rightHand.Width / 2) - 100, height / 2 - (rightHand.Height / 2) - 100, 208, 233);
            spriteBatch.Draw(rightHand,
                 recRightHand,
                     Color.White);
            
           recLeftHand =  new Rectangle(width / 2 - (leftHand.Width / 2) + 100, height / 2 - (leftHand.Height / 2) - 150, 273,
                     172);         
            spriteBatch.Draw(leftHand,
                 recLeftHand,
                     Color.White);


            recNote = new Rectangle(width / 2 - (note.Width / 2) + 220, height / 2 - (note.Height / 2)+25, 172,
                     344);
            spriteBatch.Draw(note,
                 recNote,
                     Color.White);

            recEffect = new Rectangle(width / 2 - (effect.Width / 2) + 90, height / 2 - (effect.Height / 2) + 185, 237,
                     161);
            spriteBatch.Draw(effect,
                 recEffect,
                     Color.White);

            recBackground = new Rectangle(width / 2 - (background.Width / 2) - 100, height / 2 - (background.Height / 2) + 140, 213,
                     229);
            spriteBatch.Draw(background,
                 recBackground,
                     Color.White);


            if (clickRightHand)
            {
                spriteBatch.Draw(hoverRightHand,
                new Rectangle(width / 2 - (hoverRightHand.Width / 2) - 100, height / 2 - (hoverRightHand.Height / 2) - 100, 208,
                    233),
                    Color.White);
            }

            if (clickLeftHand)
            {

                spriteBatch.Draw(hoverLeftHand,
                 new Rectangle(width / 2 - (hoverLeftHand.Width / 2) + 100, height / 2 - (hoverLeftHand.Height / 2) - 150, 273,
                     172),
                     Color.White);
            }
            if (clickNote)
            {
                spriteBatch.Draw(hoverNote,
                     new Rectangle(width / 2 - (hoverNote.Width / 2) + 220, height / 2 - (hoverNote.Height / 2) + 25, 172,
                         344),
                         Color.White);
            }
            if (clickEffect)
            {
                spriteBatch.Draw(hoverEffect,
                     new Rectangle(width / 2 - (hoverEffect.Width / 2) + 90, height / 2 - (hoverEffect.Height / 2) + 185, 237,
                         161),
                         Color.White);
            }
            if (clickBackground)
            {
                spriteBatch.Draw(hoverBackground,
                 new Rectangle(width / 2 - (hoverBackground.Width / 2) - 100, height / 2 - (hoverBackground.Height / 2) + 140, 213,
                     229),
                     Color.White);
            }

            //if (button1)
            //{
            //    spriteBatch.Draw(heart,
            //    new Rectangle(0, 0, 100,s
            //        100),
            //        Color.White);
            //}

        }

        //public void setButton1()
        //{
        //    this.button1 = true;
        //}

        public void setClickLeftHand()
        {
            this.clickLeftHand = true;
        }

        public void setClickNote()
        {
            this.clickNote = true;
        }

        public void setClickEffect()
        {
            this.clickEffect = true;
        }

        public void setClickBackground()
        {
            this.clickBackground= true;
        }

        public void setClickRightHand()
        {
            this.clickRightHand = true;
        }


        public Rectangle getRectRightHand()
        {
            return this.recRightHand;
        }

        public Rectangle getRectLeftRightHand()
        {
            return this.recLeftHand;
        }

        public Rectangle getRectNote()
        {
            return this.recNote;
        }

        public Rectangle getRectEffect()
        {
            return this.recEffect;
        }

        public Rectangle getRectBackground()
        {
            return this.recBackground;
        }
       
    }
}
