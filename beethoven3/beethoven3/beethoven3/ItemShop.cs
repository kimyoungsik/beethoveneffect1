using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
namespace beethoven3
{
    class ItemShop
    {
       protected ItemManager itemManager;
    

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

   
        public ItemShop(ItemManager itemManager)
        {
            this.itemManager = itemManager;
            darkBackground = false;
            wearOne = false;
            buyOne = false;
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

        }

        //public void addItemtoMyItem(Item item)
        //{
        //    itemManager.addMyRightHandItem(item);

        //    myRightItems = itemManager.getMyRightHandItem();
        //}
       
        public void setDarkBackground(bool value)
        {
            this.darkBackground = value;
        }

        public void setBuyOne(bool value)
        {
            this.buyOne = value;
            Trace.WriteLine(value);
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


        public Rectangle getRectYesButton()
        {
            return this.recYesButton;
        }

        public Rectangle getRectNoButton()
        {
            return this.recNoButton;
        }


        public void setHoverYesButton(bool value)
        {

            this.isHoverYesButton = value;
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
