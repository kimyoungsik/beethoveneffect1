using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using System.IO;
namespace beethoven3
{
    class ItemShop
    {
        protected ItemManager itemManager;
        protected ScoreManager scoreManager;

        protected bool darkBackground;
        
        //버튼띄울때 장착or구입 여부 확인하기위해서.
        protected bool buyOne;
        protected bool wearOne;

        protected Texture2D darkBackgroundImage;
        protected Texture2D yesButton;
        protected Texture2D noButton;
        protected Texture2D putPanel;
        protected Texture2D buyPanel;
        protected Texture2D hoverYesButton;
        protected Texture2D hoverNoButton;

        protected Rectangle recYesButton;
        protected Rectangle recNoButton;

        protected bool isHoverYesButton;
        protected bool isHoverNoButton;

        protected Texture2D usedItemBackground;
        protected Texture2D ItemBackground;

        protected Texture2D hoverNoGoldButton;
        protected Texture2D noGoldButton;

        protected bool noGold;
        protected bool isHoverNoGoldButton;


        protected Rectangle recNoGoldButton;

   
        public ItemShop(ItemManager itemManager, ScoreManager scoreManager)
        {
            this.itemManager = itemManager;
            this.scoreManager = scoreManager;
            darkBackground = false;
            wearOne = false;
            buyOne = false;
            noGold = false;
        }

        public virtual void LoadContent(ContentManager cm)
        {
            darkBackgroundImage = cm.Load<Texture2D>(@"Textures\darkBackground");

            yesButton = cm.Load<Texture2D>(@"shopdoor\yesButton");
            noButton = cm.Load<Texture2D>(@"shopdoor\noButton");
            putPanel = cm.Load<Texture2D>(@"shopdoor\putPanel");
            buyPanel = cm.Load<Texture2D>(@"shopdoor\buyPanel");
            hoverYesButton = cm.Load<Texture2D>(@"shopdoor\hoverYesButton");
            hoverNoButton = cm.Load<Texture2D>(@"shopdoor\hoverNoButton");


            usedItemBackground = cm.Load<Texture2D>(@"background\itemBackground");
            ItemBackground = cm.Load<Texture2D>(@"background\itemBackground2");

            noGoldButton = cm.Load<Texture2D>(@"shopdoor\nogold");
            hoverNoGoldButton = cm.Load<Texture2D>(@"shopdoor\nogoldhover");

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


        //public List<Rectangle> getRectRightItem()
        //{

        //    return rectRightItems;
        //}

        //public List<Item> getShopRightItem()
        //{

        //    return rightItems;
        //}

        //public bool haveOne(Item shopItem)
        //{
        //    bool ret = false;
        //    int i;
        //    for (i = 0; i < myRightItems.Count; i++)
        //    {
        //        if (shopItem == myRightItems[i])
        //        {
        //            ret = true;
        //            i = myRightItems.Count;
        //        }
        //    }
        //    return ret;
        //}

        //public virtual void setLocationItems()
        //{
        //    int i;
        //    for (i = 0; i < rightItems.Count; i++)
        //    {

        //        Rectangle rectRightHand = new Rectangle(300, i * 150, 100, 100);
        //        rectRightItems.Add(rectRightHand);
        //    }

        //}

        //public void SaveReport()
        //{
        //    TextWriter tw = new StreamWriter("c:\\beethovenRecord\\record.txt");
        //    int i, j;
        //    for (i = 0; i < scoreInfoManagers.Count; i++)
        //    {
        //        tw.WriteLine("%%");
        //        tw.WriteLine(scoreInfoManagers[i].SongName);
        //        List<ScoreInfo> scoreInfos = scoreInfoManagers[i].GetScoreInfos();
        //        for (j = 0; j < scoreInfos.Count; j++)
        //        {
        //            tw.WriteLine(scoreInfos[j].UserPicture + '$' + scoreInfos[j].Score);
        //        }
        //    }

            
        //    tw.WriteLine("**");
        //    tw.WriteLine(scoreManager.TotalGold);

        //    tw.Close();
        //}

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

        public void setHoverYesButton(bool value)
        {

            this.isHoverYesButton = value;
        }
        
        public void setHoverNoGoldButton(bool value)
        {

            this.isHoverNoGoldButton = value;
        }

        public void setHoverNoButton(bool value)
        {

            this.isHoverNoButton = value;
        }

        public virtual void Draw(SpriteBatch spriteBatch, int width, int height)
        {
          
        }
    
    }
}
