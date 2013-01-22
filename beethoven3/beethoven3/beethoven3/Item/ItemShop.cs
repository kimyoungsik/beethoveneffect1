using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using System.IO;
namespace beethoven3
{
    class ItemShop
    {
        protected ItemManager itemManager;
        protected ScoreManager scoreManager;
        protected ReportManager reportManager;
        protected bool darkBackground;
        
        //버튼띄울때 장착or구입 여부 확인하기위해서.
        protected bool buyOne;
        protected bool wearOne;
        protected bool sellOrWearOne;
        protected bool sellOne;

        //protected SpriteFont pericles36Font;

        protected Texture2D darkBackgroundImage;
        protected Texture2D yesButton;
        protected Texture2D noButton;
      
        protected Texture2D buyPanel;
        protected Texture2D sellPanel;

        protected Texture2D sellorwearPanel;


        protected Texture2D sWearButton;
        protected Texture2D sSellButton;
        protected Texture2D sCancelButton;      
            
        protected Texture2D hoverYesButton;
        protected Texture2D hoverNoButton;
        protected Texture2D hoverSWearButton;
        protected Texture2D hoverSSellButton;
        protected Texture2D hoverSCancelButton;      

        protected Rectangle recYesButton;
        protected Rectangle recNoButton;
        protected Rectangle recWearButton;
        protected Rectangle recSellButton;
        protected Rectangle recCancelButton;

        protected bool isHoverYesButton;
        protected bool isHoverNoButton;

        protected bool isHoverSellYesButton;
        protected bool isHoverSellNoButton;


        protected bool isHoverWearButton;
        protected bool isHoverSellButton;
        protected bool isHoverCancelButton;

        protected Texture2D buyItemBackground;
        protected Texture2D ItemBackground;


        protected Texture2D noGoldBackground;
        protected Texture2D hoverNoGoldButton;
        protected Texture2D noGoldButton;


        protected Texture2D handInItemBackground;
        protected Texture2D hoverHandInItemButton;
        protected Texture2D handInItemButton;

        protected bool noGold;
        protected bool isHoverNoGoldButton;


        protected bool handInItem;
        protected bool isHoverHandInItem;
        
        

        protected Rectangle recNoGoldButton;
        protected Rectangle recHandInButton;

    
     
        protected bool clickPreviousButton;

        protected Texture2D wearItemMark;

        protected Vector2 wearItemLocation;




        protected Rectangle recPreviousButton = new Rectangle(36, 35, 220, 170);

        private Vector2 locMoney = new Vector2(870, 55);

        
        public ItemShop(ItemManager itemManager, ScoreManager scoreManager, ReportManager reportManager)
        {
            this.itemManager = itemManager;
            this.scoreManager = scoreManager;
            this.reportManager = reportManager;
            darkBackground = false;
            wearOne = false;
            buyOne = false;
            noGold = false;
        }

        public virtual void LoadContent(ContentManager cm)
        {

           // ItemShopBackground = cm.Load<Texture2D>(@"shopButton\itemShopBackground");

            darkBackgroundImage = cm.Load<Texture2D>(@"Textures\darkBackground");

            yesButton = cm.Load<Texture2D>(@"shopButton\yesButton");
            noButton = cm.Load<Texture2D>(@"shopButton\noButton");
        
            buyPanel = cm.Load<Texture2D>(@"shopButton\buyPanel");
          //  sellPanel = cm.Load<Texture2D>(@"shopdoor\sellPanel");

            hoverYesButton = cm.Load<Texture2D>(@"shopButton\hoverYesButton");
            hoverNoButton = cm.Load<Texture2D>(@"shopButton\hoverNoButton");


            buyItemBackground = cm.Load<Texture2D>(@"shopButton\itemBack");
            ItemBackground = cm.Load<Texture2D>(@"shopButton\itemBack");


            noGoldBackground = cm.Load<Texture2D>(@"shopButton\noGoldBackground");
            noGoldButton = cm.Load<Texture2D>(@"shopButton\noGoldButton");
            hoverNoGoldButton = cm.Load<Texture2D>(@"shopButton\hoverNoGoldButton");

            sellorwearPanel = cm.Load<Texture2D>(@"shopButton\sellorwearPanel");

            sWearButton = cm.Load<Texture2D>(@"shopButton\sWearButton");
            sSellButton = cm.Load<Texture2D>(@"shopButton\sSellButton");
            sCancelButton = cm.Load<Texture2D>(@"shopButton\sCancelButton");


            hoverSWearButton = cm.Load<Texture2D>(@"shopButton\hoverSWearButton");
            hoverSSellButton = cm.Load<Texture2D>(@"shopButton\hoverSSellButton");
            hoverSCancelButton = cm.Load<Texture2D>(@"shopButton\hoverSCancelButton");


            handInItemBackground = cm.Load<Texture2D>(@"shopButton\handInItemBackground");
            hoverHandInItemButton = cm.Load<Texture2D>(@"shopButton\hoverNoGoldButton");
            handInItemButton = cm.Load<Texture2D>(@"shopButton\noGoldButton");

            wearItemMark = cm.Load<Texture2D>(@"rightItem\wearItem");

        }

        public void setDarkBackground(bool value)
        {
            this.darkBackground = value;
        }

        public void setBuyOne(bool value)
        {
            this.buyOne = value;
           // Trace.WriteLine(value);
        }

        public bool getBuyOne()
        {
            return this.buyOne;
        }

        //사기, 팔기 메시지 

        public void setSellOrWearOne(bool value)
        {
            this.sellOrWearOne = value;
           // Trace.WriteLine(value);
        }
               
        public bool getSellOrWearOne()
        {
            return this.sellOrWearOne;
        }




        public void setSellOne(bool value)
        {
            this.sellOne = value;
        }

        public bool getSellOne()
        {
            return this.sellOne;
        }

        
        public void setWearOne(bool value)
        {
            this.wearOne = value;
        }

        public bool getWearOne()
        {
            return this.wearOne;
        }


        public void setNoGold(bool value)
        {
            this.noGold = value;
       
        }

        public bool getNoGold()
        {
            return this.noGold;
        }

        public void setHandInItem(bool value)
        {
            this.handInItem = value;
       
        }

        public bool getHandInItem()
        {
            return this.handInItem;
        }
        
        

        public Rectangle getRectWearButton()
        {
            return this.recWearButton;
        }

        public Rectangle getRectSellButton()
        {
            return this.recSellButton;
        }

        public Rectangle getRectCancelButton()
        {
            return this.recCancelButton;
        }
        

        public Rectangle getRectYesButton()
        {
            return this.recYesButton;
        }

        public Rectangle getRectNoButton()
        {
            return this.recNoButton;
        }
        public Rectangle getRectNoGoldButton()
        {
            return this.recNoGoldButton;
        }

        public Rectangle getRectHandInItemButton()
        {
            return this.recHandInButton;
        }

        

        
        public void setHoverWearButton(bool value)
        {

            this.isHoverWearButton = value;
        }

        public void setHoverSellButton(bool value)
        {

            this.isHoverSellButton = value;
        }
        public void setHoverCancelButton(bool value)
        {

            this.isHoverCancelButton = value;
        }


        public void setHoverSellYesButton(bool value)
        {

            this.isHoverSellYesButton = value;
        }


        public void setHoverSellNoButton(bool value)
        {

            this.isHoverSellNoButton = value;
        }


       
        
        public void setHoverYesButton(bool value)
        {

            this.isHoverYesButton = value;
        }

        

        public void setHoverNoButton(bool value)
        {

            this.isHoverNoButton = value;
        }

        
        public void setHoverNoGoldButton(bool value)
        {

            this.isHoverNoGoldButton = value;
        }



        public void setHoverHandInItemButton(bool value)
        {

            this.isHoverHandInItem = value;
        }


        public void setClickPreviousButton(bool value)
        {
            this.clickPreviousButton = value;
        }

        public Rectangle getRectPreviousButton()
        {
            return this.recPreviousButton;
        }
       
  

        public virtual void Update(GameTime gameTime, Rectangle rightHandPosition)
        {


        }

        public virtual void Draw(SpriteBatch spriteBatch, int width, int height)
        {

           
            spriteBatch.Draw(Game1.previousButton, recPreviousButton, Color.White);

            if (clickPreviousButton)
            {
                spriteBatch.Draw(Game1.hoverPreviousButton, recPreviousButton, Color.White);
            }

             spriteBatch.DrawString(Game1.georgia, scoreManager.TotalGold.ToString(), locMoney, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);


        }
    
    }
}
