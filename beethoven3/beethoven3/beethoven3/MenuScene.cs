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
    class MenuScene
    {
        
        private Texture2D menu;

        //test
        private Texture2D heart;

        bool button1;

        public MenuScene()
        {
            button1 = false;
        }

        public void LoadContent(ContentManager cm)
        {
            menu = cm.Load<Texture2D>(@"Textures\menu");
            heart = cm.Load<Texture2D>(@"Textures\heart");   
        }

        public void Update(GameTime gameTime)
        {
          
            
        }

        public void Draw(SpriteBatch spriteBatch,int width,int height)
        {

            spriteBatch.Draw(menu,
                 new Rectangle(0, 0, width,
                     height),
                     Color.White);

            if (button1)
            {
                spriteBatch.Draw(heart,
                new Rectangle(0, 0, 100,
                    100),
                    Color.White);
            }
        }

        public void setButton1()
        {
            this.button1 = true;
        }
    }
}
