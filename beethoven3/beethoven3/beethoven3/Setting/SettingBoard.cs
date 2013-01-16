using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Kinect;
// 기록판
namespace beethoven3
{
    class SettingBoard
    {

  
        private Texture2D background;
        private Texture2D nextButton;
        private Texture2D hoverNextButton;
        private bool clickNextButton;
        private Rectangle recBackground;
        private Rectangle recNextButton;

        private SpriteFont pericles36Font;

        private Game1 game1;
        public SettingBoard(Game1 game1)
        {
            clickNextButton = false;
            this.game1 = game1;

        }

        public void LoadContent(ContentManager cm)
        {
            //   background = cm.Load<Texture2D>(@"result\background");
            nextButton = cm.Load<Texture2D>(@"result\nextButton");
            hoverNextButton = cm.Load<Texture2D>(@"result\hoverNextButton");
            pericles36Font = cm.Load<SpriteFont>(@"Fonts\Pericles36");

           
        }

        

        public void Draw(SpriteBatch spriteBatch, int width, int height)
        {
            //   recBackground = new Rectangle(0, 0, width, height);
            //    spriteBatch.Draw(background, recBackground, Color.White);

            recNextButton = new Rectangle(width - 400, height - 200, 356, 215);
            spriteBatch.Draw(nextButton, recNextButton, Color.White);

            //float fCurrentVolume = SoundFmod.GetVolume();
            //int iCurrentVolume = (int)(fCurrentVolume * 10);

            float userParam = game1.UserParam;

            KinectSensor nui = game1.Nui;
            int elevationAngle  = nui.ElevationAngle;

            spriteBatch.DrawString(pericles36Font, "scaling", new Vector2(100, 100), Color.Black);

            spriteBatch.DrawString(pericles36Font, userParam.ToString(), new Vector2(500, 100), Color.Black);

            spriteBatch.DrawString(pericles36Font, "angle", new Vector2(100, 300), Color.Black);

            spriteBatch.DrawString(pericles36Font, elevationAngle.ToString(), new Vector2(500, 300), Color.Black);




            if (clickNextButton)
            {
                spriteBatch.Draw(hoverNextButton, recNextButton, Color.White);
            }

        }

        public void setClickNextButton(bool value)
        {
            this.clickNextButton = value;
        }

        public Rectangle getRectNextButton()
        {
            return this.recNextButton;
        }
    }
}
