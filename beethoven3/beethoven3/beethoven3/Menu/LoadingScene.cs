using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
namespace beethoven3
{
    class LoadingScene
    {


        private Texture2D Background;
        private Rectangle recBackground = new Rectangle(0, 0, 1024, 769);




        public LoadingScene()
        {
        
        
        }

        public void LoadContent(ContentManager cm)
        {
          //  Background = cm.Load<Texture2D>(@"Menu\firstMenuBackground");

        }


        public void Draw(SpriteBatch spriteBatch)
        {

          //  spriteBatch.Draw(Background, recBackground, Color.White);

        }

    }
}
