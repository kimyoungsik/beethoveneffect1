using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Media;


namespace beethoven3
{
    class MenuScene
    {
        MouseState pastmouse;
        KeyboardState pastkey;
        private Texture2D menu;

        //test
      //  private Texture2D heart;

        bool button1;

        public MenuScene()
        {
            button1 = false;
        }

        public void LoadContent(ContentManager cm)
        {
            menu = cm.Load<Texture2D>(@"Menu\firstMenuBackground");
          //  heart = cm.Load<Texture2D>(@"Textures\heart");   
        }

        public void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();
            KeyboardState key = Keyboard.GetState();

            Rectangle mouseRectangle = new Rectangle(mouse.X, mouse.Y, 1, 1);

            //Start
            if ((mouseRectangle.Intersects(new Rectangle(160, 310, 160, 50)) && mouse.LeftButton == ButtonState.Pressed && pastmouse.LeftButton == ButtonState.Released)
                || (key.IsKeyDown(Keys.Space) && !pastkey.IsKeyDown(Keys.Space)) || Game1.drawrec1.Intersects(new Rectangle(160, 310, 160, 50)) && Game1.finalClick)
            {
                Game1.gameState = Game1.GameStates.SongMenu;


            }


            //Shop
            if ((mouseRectangle.Intersects(new Rectangle(720, 300, 160, 70)) && mouse.LeftButton == ButtonState.Pressed && pastmouse.LeftButton == ButtonState.Released)
                || (key.IsKeyDown(Keys.S) && !pastkey.IsKeyDown(Keys.S)) || Game1.drawrec1.Intersects(new Rectangle(720, 300, 160, 70)) && Game1.finalClick)
            {
                Game1.gameState = Game1.GameStates.ShopDoor;


            }

            pastmouse = mouse;
            pastkey = key;
         
            
        }

        public void Draw(SpriteBatch spriteBatch,int width,int height)
        {

            spriteBatch.Draw(menu,
                 new Rectangle(0, 0, width,
                     height),
                     Color.White);

            //if (button1)
            //{
            //    spriteBatch.Draw(heart,
            //    new Rectangle(0, 0, 100,
            //        100),
            //        Color.White);
            //}
        }

        //public void setButton1()
        //{
        //    this.button1 = true;
        //}
    }
}
