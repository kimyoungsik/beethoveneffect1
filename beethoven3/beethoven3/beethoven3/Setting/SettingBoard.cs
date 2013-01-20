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
        
        private Texture2D UpButton;
        private Texture2D hoverUpButton;
      
        private Texture2D DownButton;
        private Texture2D hoverDownButton;
       
        private Rectangle recScaleUpButton;
        private Rectangle recScaleDownButton;
        private Rectangle recAngleUpButton;
        private Rectangle recAngleDownButton;

        private bool clickNextButton;
        private bool clickScaleUpButton;
        private bool clickScaleDownButton;
        private bool clickAngleUpButton;
        private bool clickAngleDownButton;




        private Rectangle recBackground;
        private Rectangle recNextButton;

        private SpriteFont pericles36Font;

        private Game1 game1;
        public SettingBoard(Game1 game1)
        {
            clickNextButton = false;
            clickScaleUpButton = false;
            clickScaleDownButton = false;
            clickAngleUpButton = false;
            clickAngleDownButton = false;
            this.game1 = game1;

        }

        public void LoadContent(ContentManager cm)
        {
            //   background = cm.Load<Texture2D>(@"result\background");
            nextButton = cm.Load<Texture2D>(@"result\nextButton");
            hoverNextButton = cm.Load<Texture2D>(@"result\hoverNextButton");
            
            UpButton = cm.Load<Texture2D>(@"settingBoard\up");
            hoverUpButton = cm.Load<Texture2D>(@"settingBoard\upHover");
            
            DownButton = cm.Load<Texture2D>(@"settingBoard\down");
            hoverDownButton = cm.Load<Texture2D>(@"settingBoard\downHover");
                        
       //     pericles36Font = cm.Load<SpriteFont>(@"Fonts\Pericles36");
                       
        }

        

        public void Draw(SpriteBatch spriteBatch, int width, int height)
        {
            //   recBackground = new Rectangle(0, 0, width, height);
            //    spriteBatch.Draw(background, recBackground, Color.White);

            recNextButton = new Rectangle(width - 400, height - 200, 356, 215);
            spriteBatch.Draw(nextButton, recNextButton, Color.White);

            recScaleUpButton = new Rectangle(800, 100, 101, 66);
            spriteBatch.Draw(UpButton, recScaleUpButton, Color.White);

            recScaleDownButton = new Rectangle(800, 200, 101, 66);
            spriteBatch.Draw(DownButton, recScaleDownButton, Color.White);

            recAngleUpButton = new Rectangle(800, 400, 101, 66);
            spriteBatch.Draw(UpButton, recAngleUpButton, Color.White);

            recAngleDownButton = new Rectangle(800, 500, 101, 66);
            spriteBatch.Draw(DownButton, recAngleDownButton, Color.White);

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



            if (clickScaleUpButton)
            {
                spriteBatch.Draw(hoverUpButton, recScaleUpButton, Color.White);
            }


            if (clickScaleDownButton)
            {
                spriteBatch.Draw(hoverDownButton, recScaleDownButton, Color.White);
            }

            if (clickAngleUpButton)
            {
                spriteBatch.Draw(hoverUpButton, recAngleUpButton, Color.White);
            }


            if (clickAngleDownButton)
            {
                spriteBatch.Draw(hoverDownButton, recAngleDownButton, Color.White);
            }

        }

        //public void setClickNextButton(bool value)
        //{
        //    this.clickNextButton = value;
        //}



        public Rectangle RectNextButton
        {
            get { return recNextButton; }
        }

        public Rectangle RecScaleUpButton
        {
            get { return recScaleUpButton; }
        }

        public Rectangle RecScaleDownButton
        {
            get { return recScaleDownButton; }
        }

        public Rectangle RecAngleUpButton
        {
            get { return recAngleUpButton; }
        }

        public Rectangle RecAngleDownButton
        {
            get { return recAngleDownButton; }
        }

        public bool ClickNextButton
        {
            set { clickNextButton = value; }
        }


        public bool ClickScaleUpButton
        {
            set { clickScaleUpButton = value; }
        }


        public bool ClickScaleDownButton
        {
            set { clickScaleDownButton = value; }
        }


        public bool ClickAngleUpButton
        {
            set { clickAngleUpButton = value; }
        }


        public bool ClickAngleDownButton
        {
            set { clickAngleDownButton = value; }
        }

     
    }
}
