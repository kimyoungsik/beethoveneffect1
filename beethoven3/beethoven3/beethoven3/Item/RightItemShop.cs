using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
namespace beethoven3
{
    class RightItemShop : ItemShop
    {

        private List<Item> rightItems;
        private List<Item> myRightItems;
        private List<Rectangle> rectRightItems = new List<Rectangle>();
        private Texture2D rightItemBackground;


        public RightItemShop(ItemManager itemManager,ScoreManager scoreManager)
            : base(itemManager, scoreManager)
        {
          
        }

        public override void LoadContent(ContentManager cm)
        {
            base.LoadContent(cm);
            rightItems = itemManager.getShopRightHandItem();
            myRightItems = itemManager.getMyRightHandItem();
            rightItemBackground = cm.Load<Texture2D>(@"rightItem\rightItemBackground");
            setLocationItems();
        }


        public void addItemtoMyItem(Item item)
        {
            itemManager.addMyRightHandItem(item);

            //더하고 다시 갱신
            myRightItems = itemManager.getMyRightHandItem();
            
        }



        public void removeItemtoMyItem(Item item)
        {
            itemManager.removeMyRightHandItem(item);

            //더하고 다시 갱신
            myRightItems = itemManager.getMyRightHandItem();

        }



               

        public List<Rectangle> getRectRightItem()
        {

            return rectRightItems;
        }

        public List<Item> getShopRightItem()
        {

            return rightItems;
        }

        public bool haveOne(Item shopItem)
        {
            bool ret = false;
            int i;
            for (i = 0; i < myRightItems.Count; i++)
            {
                if (shopItem == myRightItems[i])
                {
                    ret = true;
                    i = myRightItems.Count;
                }
            }
            return ret;
        }

        public void setLocationItems()
        {
            int i;
            int j = 0;
            if (rightItems.Count < 4)
            {
                for (i = 0; i < rightItems.Count; i++)
                {
                    Rectangle rectRightHand = new Rectangle(i * 255 + 160, 200, 240, 240);
                    rectRightItems.Add(rectRightHand);
                }
            }
            else
            {
                for (i = 0; i < 3; i++)
                {
                    Rectangle rectRightHand = new Rectangle(i * 255+160, 200, 240, 240);
                    rectRightItems.Add(rectRightHand);
                }
                
                for (i = 3; i < rightItems.Count; i++)
                {
                    
                    Rectangle rectRightHand = new Rectangle(j * 255+160, 480, 240, 240);
                    rectRightItems.Add(rectRightHand);
                    j++;
                }


            }

        }

        public override void Draw(SpriteBatch spriteBatch, int width, int height)
        {
            
            //배경
            spriteBatch.Draw(rightItemBackground, new Vector2(0, 0), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 1f);

            base.Draw(spriteBatch, width, height);

            spriteBatch.DrawString(Game1.georgia, "Gold : ", new Vector2(10, 200), Color.Black);


            //전체 돈 표시
            spriteBatch.DrawString(Game1.georgia, scoreManager.TotalGold.ToString(), new Vector2(190, 200), Color.Black);

            int i;
           

            //보유아이템
            for (i = 0; i < rightItems.Count; i++)
            {
              
                spriteBatch.Draw(ItemBackground, rectRightItems[i], Color.White);
                //Item I already have
                if (haveOne(rightItems[i]))
                {
                    spriteBatch.Draw(rightItems[i].ItemSprite.Texture, rectRightItems[i], Color.White);
                }

                else
                {
                    spriteBatch.Draw(rightItems[i].ItemSprite.Texture, rectRightItems[i], Color.Black);
                }

                //아이템 가격 표시 
                spriteBatch.DrawString(Game1.georgia, rightItems[i].GetCost().ToString(), new Vector2(rectRightItems[i].X + (rectRightItems[i].Width / 2), rectRightItems[i].Y + rectRightItems[i].Height - rectRightItems[i].Height / 10), Color.Black);
           
                //rectRightItem.Add(rectRightHand);
            }

            ////장착아이템
            //Rectangle usedItemRect = new Rectangle(50,50,100,100);

            //spriteBatch.Draw(usedItemBackground, usedItemRect, Color.White);

           
            if (itemManager.getRightHandIndex()  !=  -1)
            {

                wearItemLocation = new Vector2(rectRightItems[itemManager.getRightHandIndex()].X , rectRightItems[itemManager.getRightHandIndex()].Y);
                spriteBatch.Draw(wearItemMark, new Rectangle((int)wearItemLocation.X, (int)wearItemLocation.Y, wearItemMark.Width, wearItemMark.Height), Color.White);
            }



            //장착아이템 텍스쳐
            //리스트에서 인덱스만 가져와서 표시
            //spriteBatch.Draw(rightItems[itemManager.getRightHandIndex()].ItemSprite.Texture, usedItemRect, Color.White);

            
            if (darkBackground)
            {
                Color color = Color.White;
                color.A = 50;
                spriteBatch.Draw(darkBackgroundImage, new Rectangle(0, 0, width, height), color);
                Vector2 middle = new Vector2(width / 2, height / 2);
                //Rectangle rectBuyPanel = new Rectangle(width / 2 - (buyPanel.Width / 2) - 100, height / 2 - (buyPanel.Height / 2) - 100, 200, 101);
                //spriteBatch.Draw(buyPanel,new Vector2(100,100), rectBuyPanel, Color.White,0f,new Vector2(0,0),1f,SpriteEffects.None,1f);

                //돈이 부족 하면 팝업 띄운다.
                //show up message if there is not enough money
                if (noGold)
                {
                    
                    spriteBatch.Draw(noGoldButton, middle, null, Color.White, 0f, new Vector2(350, 200), 1f, SpriteEffects.None, 1f);

                    //마우스 커서가 버튼위에 올라가면
                    //mouse cursor on noGoldButton
                    if(isHoverNoGoldButton)
                    {
                        spriteBatch.Draw(hoverNoGoldButton, middle, null, Color.White, 0f, new Vector2(350, 200), 1f, SpriteEffects.None, 1f);

                    }


                    recNoGoldButton = new Rectangle((int)middle.X-350, (int)middle.Y-200, 700, 300);
                }

                if (handInItem)
                {

                    spriteBatch.Draw(handInItemButton, middle, null, Color.White, 0f, new Vector2(350, 200), 1f, SpriteEffects.None, 1f);

                    //마우스 커서가 버튼위에 올라가면
                    //mouse cursor on noGoldButton
                    if (isHoverHandInItem)
                    {
                        spriteBatch.Draw(hoverHandInItemButton, middle, null, Color.White, 0f, new Vector2(350, 200), 1f, SpriteEffects.None, 1f);

                    }


                    recHandInButton = new Rectangle((int)middle.X - 350, (int)middle.Y - 200, 700, 300);
                }


                if(sellOrWearOne)
                {
                    spriteBatch.Draw(sellorwearPanel, new Vector2(0, 0), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 1f);

                    //장착 버튼 
                    Vector2 wearButtonLocation = new Vector2(width / 2 - 270, height / 2 + 90);
                    
                     // mouse cursor on YES button
                    if (!isHoverWearButton)
                    {

                        spriteBatch.Draw(sWearButton, wearButtonLocation, null, Color.White, 0f, new Vector2(sWearButton.Width / 2, sWearButton.Height/2), 1f, SpriteEffects.None, 1f);
                    
                    }
                    else
                    {

                        spriteBatch.Draw(hoverSWearButton, wearButtonLocation, null, Color.White, 0f, new Vector2(hoverSWearButton.Width / 2, hoverSWearButton.Height/2), 1f, SpriteEffects.None, 1f);
                        
                    }

                    //팔기버튼
                    Vector2 sellButtonLocation = new Vector2(width / 2, height / 2 +90);
                     // mouse cursor on YES button
                    if (!isHoverSellButton)
                    {

                        spriteBatch.Draw(sSellButton, sellButtonLocation, null, Color.White, 0f, new Vector2(sSellButton.Width / 2, sSellButton.Height / 2), 1f, SpriteEffects.None, 1f);
                    
                    }
                    else
                    {

                        spriteBatch.Draw(hoverSSellButton, sellButtonLocation, null, Color.White, 0f, new Vector2(hoverSSellButton.Width / 2, hoverSSellButton.Height/2), 1f, SpriteEffects.None, 1f);
                        
                    }

                    
                    //취소
                    Vector2 cancelButtonLocation = new Vector2(width / 2 + 270, height / 2 + 90);
                    
                     // mouse cursor on YES button
                    if (!isHoverCancelButton)
                    {

                        spriteBatch.Draw(sCancelButton, cancelButtonLocation, null, Color.White, 0f, new Vector2(sCancelButton.Width / 2, sCancelButton.Height/2), 1f, SpriteEffects.None, 1f);
                    
                    }
                    else
                    {

                        spriteBatch.Draw(hoverSCancelButton, cancelButtonLocation, null, Color.White, 0f, new Vector2(hoverSCancelButton.Width / 2, hoverSCancelButton.Height/2), 1f, SpriteEffects.None, 1f);
                        
                    }



                    recWearButton = new Rectangle((int)wearButtonLocation.X - sWearButton.Width / 2, (int)wearButtonLocation.Y - sWearButton.Height / 2, sWearButton.Width, sWearButton.Height);
                    recSellButton = new Rectangle((int)sellButtonLocation.X - sSellButton.Width / 2, (int)sellButtonLocation.Y - sSellButton.Height / 2, sSellButton.Width, sSellButton.Height);
                    recCancelButton = new Rectangle((int)cancelButtonLocation.X - sCancelButton.Width / 2, (int)cancelButtonLocation.Y - sCancelButton.Height / 2, sCancelButton.Width, sCancelButton.Height);
                
                
                }

                //to BUY item
                if (buyOne)
                {
                    spriteBatch.Draw(buyPanel, new Vector2(0, 0), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 1f);

                    Vector2 yesButtonLocation = new Vector2(width / 2 -190 , height / 2 + 100 );
                    
                    // mouse cursor on YES button
                    if (!isHoverYesButton)
                    {

                        spriteBatch.Draw(yesButton, yesButtonLocation, null, Color.White, 0f, new Vector2(yesButton.Width/2, yesButton.Height/2), 1f, SpriteEffects.None, 1f);
                    
                    }
                    else
                    {

                        spriteBatch.Draw(hoverYesButton, yesButtonLocation, null, Color.White, 0f, new Vector2(hoverYesButton.Width/2, hoverYesButton.Height/2), 1f, SpriteEffects.None, 1f);
                        
                    }


                    Vector2 noButtonLocation = new Vector2(width / 2 + 190, height / 2 + 100);
                 

                    // mouse cursor on NO bUTTON
                    if (!isHoverNoButton)
                    {
                        spriteBatch.Draw(noButton, noButtonLocation, null, Color.White, 0f, new Vector2(noButton.Width / 2,noButton.Height / 2), 1f, SpriteEffects.None, 1f);

                    }

                    else
                    {
                        spriteBatch.Draw(hoverNoButton, noButtonLocation, null, Color.White, 0f, new Vector2(hoverNoButton.Width / 2,hoverNoButton.Height / 2), 1f, SpriteEffects.None, 1f);

                    }

                    recYesButton = new Rectangle((int)yesButtonLocation.X - yesButton.Width / 2, (int)yesButtonLocation.Y - yesButton.Height / 2, yesButton.Width, yesButton.Height);

                    recNoButton = new Rectangle((int)noButtonLocation.X - noButton.Width / 2, (int)noButtonLocation.Y - noButton.Height / 2, noButton.Width, noButton.Height);
                
                }
                

                // to WEAR item
                if(wearOne)
                {
                  //  Vector2 middle = new Vector2(width / 2, height / 2);
                    spriteBatch.Draw(putPanel, middle, null, Color.White, 0f, new Vector2(310, 150), 1.5f, SpriteEffects.None, 1f);

                    Vector2 yesButtonLocation = new Vector2(width / 2 - 180, height / 2 + 50);

                    // mouse cursor on YES button
                    if (!isHoverYesButton)
                    {
                        spriteBatch.Draw(yesButton, yesButtonLocation, null, Color.White, 0f, new Vector2(100, 60), 1.5f, SpriteEffects.None, 1f);
                    }
                    else
                    {
                        spriteBatch.Draw(hoverYesButton, yesButtonLocation, null, Color.White, 0f, new Vector2(100, 60), 1.5f, SpriteEffects.None, 1f);

                    }
                    Vector2 noButtonLocation = new Vector2(width / 2 + 180, height / 2 + 50);

                    // mouse cursor on No button
                    if (!isHoverNoButton)
                    {
                        spriteBatch.Draw(noButton, noButtonLocation, null, Color.White, 0f, new Vector2(100, 60), 1.5f, SpriteEffects.None, 1f);
                    }
                    else
                    {
                        spriteBatch.Draw(hoverNoButton, noButtonLocation, null, Color.White, 0f, new Vector2(100, 60), 1.5f, SpriteEffects.None, 1f);
                    }

                    recYesButton = new Rectangle((int)yesButtonLocation.X-100, (int)yesButtonLocation.Y-60, 200, 120);
                    recNoButton = new Rectangle((int)noButtonLocation.X-100, (int)noButtonLocation.Y-60, 200, 120);

                }


                // 팔기 위해서
                if (sellOne)
                {
                    //  Vector2 middle = new Vector2(width / 2, height / 2);
                    spriteBatch.Draw(sellPanel, middle, null, Color.White, 0f, new Vector2(310, 150), 1.5f, SpriteEffects.None, 1f);

                    Vector2 yesButtonLocation = new Vector2(width / 2 - 180, height / 2 + 50);

                    // mouse cursor on YES button
                    if (!isHoverSellYesButton)
                    {
                        spriteBatch.Draw(yesButton, yesButtonLocation, null, Color.White, 0f, new Vector2(100, 60), 1.5f, SpriteEffects.None, 1f);
                    }
                    else
                    {
                        spriteBatch.Draw(hoverYesButton, yesButtonLocation, null, Color.White, 0f, new Vector2(100, 60), 1.5f, SpriteEffects.None, 1f);

                    }
                    Vector2 noButtonLocation = new Vector2(width / 2 + 180, height / 2 + 50);

                    // mouse cursor on No button
                    if (!isHoverSellNoButton)
                    {
                        spriteBatch.Draw(noButton, noButtonLocation, null, Color.White, 0f, new Vector2(100, 60), 1.5f, SpriteEffects.None, 1f);
                    }
                    else
                    {
                        spriteBatch.Draw(hoverNoButton, noButtonLocation, null, Color.White, 0f, new Vector2(100, 60), 1.5f, SpriteEffects.None, 1f);
                    }

                    recYesButton = new Rectangle((int)yesButtonLocation.X - 100, (int)yesButtonLocation.Y - 60, 200, 120);
                    recNoButton = new Rectangle((int)noButtonLocation.X - 100, (int)noButtonLocation.Y - 60, 200, 120);

                }
            }
        }
    }
}