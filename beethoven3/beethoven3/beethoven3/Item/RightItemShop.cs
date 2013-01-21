using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace beethoven3
{
    class RightItemShop : ItemShop
    {

        private List<Item> rightItems;
        private List<Item> myRightItems;
        private List<Rectangle> rectRightItems = new List<Rectangle>();
      //  private Texture2D title;
     //   private Vector2 locationTitle = new Vector2(352,123);
        private Texture2D rightItemShopBackground;

        MouseState mouseStatePrevious;
        private Item selectedItem;

        public RightItemShop(ItemManager itemManager, ScoreManager scoreManager, ReportManager reportManager)
            : base(itemManager, scoreManager, reportManager)
        {
          
        }

        public override void LoadContent(ContentManager cm)
        {
            base.LoadContent(cm);
            rightItems = itemManager.getShopRightHandItem();
            myRightItems = itemManager.getMyRightHandItem();
            rightItemShopBackground = cm.Load<Texture2D>(@"rightItem\rightItemBackground");
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
        

        //배치 위치
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
                    
                    Rectangle rectRightHand = new Rectangle(j * 255+160, 450, 240, 240);
                    rectRightItems.Add(rectRightHand);
                    j++;
                }

            }

        }
        public override void Update(GameTime gameTime, Rectangle rightHandPosition)
        {
            
            base.Update(gameTime, rightHandPosition);
            MouseState mouseStateCurrent = Mouse.GetState();
            mouseStatePrevious = Game1.mouseStatePrevious;
            Rectangle mouseRect = new Rectangle(mouseStateCurrent.X, mouseStateCurrent.Y, 5, 5);
            
            
            int i;
            //아이템 rect
            List<Rectangle> rectRightItems = getRectRightItem();

            //아이템/
            List<Item> shopRightItems = getShopRightItem();

            //다이얼로그를 띄웠을 때 이것이 중복실행되지 않도록 
            if (!getSellOrWearOne() && !getBuyOne() && !getNoGold() && !getHandInItem())
            {
                int count = 0;
                for (i = 0; i < rectRightItems.Count; i++)
                {
                    //Trace.WriteLine(finalClick + " - " + pastClick);
                    //아이템을 선택 했을때
                    if (mouseRect.Intersects(rectRightItems[i]) || rightHandPosition.Intersects(rectRightItems[i]))
                    {

                        Game1.nearButton = true;
                        Game1.GetCenterOfButton(rectRightItems[i]);
                        //if ((mouseRect.Intersects(rectRightItems[i]) && mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) ||
                        //    (drawrec1.Intersects(rectRightItems[i]) && finalClick && !pastClick))

                        if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (Game1.finalClick && !Game1.pastClick))
                        {

                            //nearButton = false;
                            //어두어짐
                            setDarkBackground(true);

                            selectedItem = shopRightItems[i];

                            //이미 산거이면 true
                            if (haveOne(shopRightItems[i]))
                            {
                                //장착 메시지 or 팔기 메시지 박스 띄우기 
                                setSellOrWearOne(true);
                                //반복 없애기
                                i = rectRightItems.Count;
                            }
                            else
                            {
                                //구입 메시지 박스 띄우기 
                                setBuyOne(true);
                                //반복 없애기
                                i = rectRightItems.Count;
                            }

                            //이게 있어야 중복해서 안된다.
                            //중복 구입 막음

                            mouseStatePrevious = mouseStateCurrent;
                            Game1.pastClick = Game1.finalClick;
                        }
                    }
                    else
                    {
                        count++;
                    }



                }

                if (count == rectRightItems.Count)
                {
                    Game1.nearButton = false;
                }

            }


            //돈 부족 메시지 띄우기
            if (getNoGold())
            {
                //버튼 Hover
                if (mouseRect.Intersects(getRectNoGoldButton()) || rightHandPosition.Intersects(getRectNoGoldButton()))
                {
                    //눌린모양
                    setHoverNoGoldButton(true);
                    Game1.nearButton = true;
                    Game1.GetCenterOfButton(getRectNoGoldButton());
                    //버튼 누르면
                    if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (Game1.finalClick && !Game1.pastClick))
                    {
                        Game1.nearButton = false;
                        //다시 밝게
                        setDarkBackground(false);
                        //메시지 없애기
                        setNoGold(false);

                    }

                }
                //버튼 not hover
                else
                {
                    Game1.nearButton = false;
                    setHoverNoGoldButton(false);
                }
            }




            //장착한 아이템 팔 수없게 
            if (getHandInItem())
            {
                //버튼 Hover
                if (mouseRect.Intersects(getRectHandInItemButton()) || rightHandPosition.Intersects(getRectHandInItemButton()))
                {
                    //눌린모양
                    Game1.nearButton = true;
                    Game1.GetCenterOfButton(getRectHandInItemButton());

                    setHoverHandInItemButton(true);
                    //버튼 누르면
                    if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (Game1.finalClick && !Game1.pastClick))
                    {
                        Game1.nearButton = false;
                        //다시 밝게
                        setDarkBackground(false);
                        //메시지 없애기
                        setHandInItem(false);


                    }
                }
                //버튼 not hover
                else
                {
                    Game1.nearButton = false;
                    setHoverHandInItemButton(false);
                }
                //mouseStatePrevious = mouseStateCurrent;
                //pastClick = finalClick;
            }

            //장착할것인지 팔것인지 묻는 메시지

            if (getSellOrWearOne())
            {
                //마우스가 장착 버튼에 올려졌을 때
                if (mouseRect.Intersects(getRectWearButton()) || rightHandPosition.Intersects(getRectWearButton()))
                {
                    setHoverWearButton(true);
                    Game1.nearButton = true;
                    Game1.GetCenterOfButton(getRectWearButton());

                    if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (Game1.finalClick && !Game1.pastClick))
                    {
                        //장착 메시지 박스 띄우기 
                        // rightItemShop.setWearOne(true);
                        //선택된 아이템이 있으면
                        if (selectedItem != null)
                        {
                            //find the index of item in myrightitem list
                            //아이템 찾기
                            int index = itemManager.getIndexOfAllRightItem(selectedItem);

                            //아이템을 찾았으면
                            if (index != -1)
                            {
                                //change index
                                //장착
                                itemManager.setRightHandIndex(index);

                                //return to normal , remove message box
                                //메시지 박스 지우기
                                // rightItemShop.setWearOne(false);
                                //밝게 하기
                                setDarkBackground(false);
                                setSellOrWearOne(false);
                                Game1.nearButton = false;

                                //파일로저장
                                itemManager.SaveFileItem();
                            }
                            //아이템을 찾지 못했으면
                            else
                            {
                                //Trace.WriteLine("get wrong index( no item in list)");
                            }
                        }
                        //선택된 아이템이 없으면
                        else
                        {
                           // Trace.WriteLine("nothing is selected");
                        }
                    }
                }
                else
                {



                    setHoverWearButton(false);

                }


                //마우스가 팔기 버튼에 눌러졌을 때
                if (mouseRect.Intersects(getRectSellButton()) || rightHandPosition.Intersects(getRectSellButton()))
                {

                    setHoverSellButton(true);
                    Game1.nearButton = true;
                    Game1.GetCenterOfButton(getRectSellButton());

                    if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (Game1.finalClick && !Game1.pastClick))
                    {
                        //팔기 메시지 박스 띄우기 
                        // rightItemShop.setSellOne(true);
                        //선택된 아이템이 있으면
                        if (selectedItem != null)
                        {
                            //find the index of item in myrightitem list
                            //아이템 찾기
                            int index = itemManager.getIndexOfAllRightItem(selectedItem);

                            //찾은 아이템이 장착하고 있는 아이템이면 팔 수 없다.
                            if (index != itemManager.getRightHandIndex())
                            {
                                //아이템을 찾았으면
                                if (index != -1)
                                {
                                    Game1.nearButton = true;
                                    //전체 돈에서 판 비용 더해줌
                                    scoreManager.TotalGold += selectedItem.GetCost() / 2;

                                    //자신의 인벤토리에서 빼기
                                    removeItemtoMyItem(selectedItem);


                                    setDarkBackground(false);

                                    //돈을 파일에 저장
                                         reportManager.SaveGoldToFile();

                                    //추가적으로 sellorwear 버튼도 false로 해야 한다.
                                    setSellOrWearOne(false);

                                    //파일로저장
                                    itemManager.SaveFileItem();


                                }
                                //아이템을 찾지 못했으면
                                else
                                {
                                  //  Trace.WriteLine("get wrong index( no item in list)");
                                }
                            }
                            else
                            {
                                setHandInItem(true);

                                setSellOrWearOne(false);
                                setSellOne(false);


                            }


                        }
                        //선택된 아이템이 없으면
                        else
                        {
                           // Trace.WriteLine("nothing is selected");
                        }
                    }
                }
                else
                {
                    //nearButton = true;
                    setHoverSellButton(false);
                }


                //취소버튼 눌렀을 때
                if (mouseRect.Intersects(getRectCancelButton()) || rightHandPosition.Intersects(getRectCancelButton()))
                {
                    setHoverCancelButton(true);
                    Game1.nearButton = true;
                    Game1.GetCenterOfButton(getRectCancelButton());

                    if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (Game1.finalClick && !Game1.pastClick))
                    {
                        //return to normal , remove message box
                        setSellOrWearOne(false);
                        setDarkBackground(false);
                        Game1.nearButton = false;
                        //mouseStatePrevious = mouseStateCurrent;
                        //pastClick = finalClick;
                    }
                }
                else
                {
                    // nearButton = false;
                    setHoverCancelButton(false);
                }

                if (
                 !(mouseRect.Intersects(getRectWearButton()) || rightHandPosition.Intersects(getRectWearButton()))
                 && !(mouseRect.Intersects(getRectCancelButton()) || rightHandPosition.Intersects(getRectCancelButton()))
                 && !(mouseRect.Intersects(getRectSellButton()) || rightHandPosition.Intersects(getRectSellButton()))
                )
                {
                    Game1.nearButton = false;
                }

            }



            //구입 메시지
            //message box about buying item
            if (getBuyOne())
            {
                //mouse cursor on right button
                if (mouseRect.Intersects(getRectYesButton()) || rightHandPosition.Intersects(getRectYesButton()))
                {
                    Game1.nearButton = true;
                    Game1.GetCenterOfButton(getRectYesButton());
                    setHoverYesButton(true);
                    if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (Game1.finalClick && !Game1.pastClick))
                    {
                        //add item to my item
                        if (selectedItem != null)
                        {
                            //돈으로 물건사기 
                            //buy item with money

                            Game1.nearButton = false;
                            //돈이 충분히 있다.
                            if (scoreManager.TotalGold >= selectedItem.GetCost())
                            {
                                //전체 돈에서 구입비용 차감
                                scoreManager.TotalGold -= selectedItem.GetCost();
                                addItemtoMyItem(selectedItem);
                                setBuyOne(false);
                                setDarkBackground(false);
                                //돈을 파일에 저장
                               reportManager.SaveGoldToFile();

                                //파일로저장
                                itemManager.SaveFileItem();
                            }
                            //돈이 없다.
                            else
                            {
                                setBuyOne(false);
                                //"돈 부족" 메시지 띄운다.
                                setNoGold(true);
                            }
                        }
                        else
                        {
                          //  Trace.WriteLine("nothing is selected");
                        }
                    }
                }
                else
                {
                    setHoverYesButton(false);
                }

                if (mouseRect.Intersects(getRectNoButton()) || rightHandPosition.Intersects(getRectNoButton()))
                {
                    Game1.nearButton = true;
                    Game1.GetCenterOfButton(getRectNoButton());
                    setHoverNoButton(true);
                    if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (Game1.finalClick && !Game1.pastClick))
                    {
                        //return to normal , remove message box
                        setBuyOne(false);
                        setDarkBackground(false);
                        Game1.nearButton = false;
                    }
                }
                else
                {
                    setHoverNoButton(false);
                }

                if (
                !(mouseRect.Intersects(getRectYesButton()) || rightHandPosition.Intersects(getRectYesButton()))
                && !(mouseRect.Intersects(getRectNoButton()) || rightHandPosition.Intersects(getRectNoButton()))
                )
                {

                    Game1.nearButton = false;
                }


            }




            //다른메시지 창이 안떳을 때

            if (!(getBuyOne()) && !(getSellOrWearOne()) && !getHandInItem() && !getNoGold())
            {
                if (mouseRect.Intersects(getRectPreviousButton()) || rightHandPosition.Intersects(getRectPreviousButton()))
                {
                    Game1.nearButton = true;
                    Game1.GetCenterOfButton(getRectPreviousButton());
                    setClickPreviousButton(true);
                    //click the right hand item section
                    if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (Game1.finalClick && !Game1.pastClick))
                    {
                        Game1.nearButton = false;
                        Game1.gameState = Game1.GameStates.ShopDoor;
                    }
                }
                else
                {
                    //nearButton = false;
                    setClickPreviousButton(false);
                }
            }

            mouseStatePrevious = mouseStateCurrent;
               
            
        }
        public override void Draw(SpriteBatch spriteBatch, int width, int height)
        {
            
            //배경
            spriteBatch.Draw(rightItemShopBackground, new Rectangle(0, 0, 1024, 769), Color.White);


            base.Draw(spriteBatch, width, height);

           int i;
           

            //보유아이템
            for (i = 0; i < rightItems.Count; i++)
            {
              
                spriteBatch.Draw(ItemBackground, rectRightItems[i], Color.White);
                //Item I already have
                if (haveOne(rightItems[i]))
                {

                    spriteBatch.Draw(rightItems[i].ItemSprite.Texture, new Vector2(rectRightItems[i].Center.X, rectRightItems[i].Center.Y), new Rectangle(0, 0, rightItems[i].ItemSprite.Texture.Width, rightItems[i].ItemSprite.Texture.Height), Color.White, 0f, new Vector2( rightItems[i].ItemSprite.Texture.Width/2, rightItems[i].ItemSprite.Texture.Height/2), 0.7f, SpriteEffects.None, 1.0f);
                 
                }

                else
                {
                    spriteBatch.Draw(rightItems[i].ItemSprite.Texture, new Vector2(rectRightItems[i].Center.X, rectRightItems[i].Center.Y), new Rectangle(0, 0, rightItems[i].ItemSprite.Texture.Width, rightItems[i].ItemSprite.Texture.Height), Color.Black, 0f, new Vector2(rightItems[i].ItemSprite.Texture.Width / 2, rightItems[i].ItemSprite.Texture.Height / 2), 0.7f, SpriteEffects.None, 1.0f);
             
                 
                }


               // spriteBatch.Draw(Game1.menuGold, new Vector2(rectRightItems[i].Center.X, rectRightItems[i].Center.Y), new Rectangle(0, 0, rightItems[i].ItemSprite.Texture.Width, rightItems[i].ItemSprite.Texture.Height), Color.Black, 0f, new Vector2(rightItems[i].ItemSprite.Texture.Width / 2, rightItems[i].ItemSprite.Texture.Height / 2), 0.7f, SpriteEffects.None, 1.0f);
                spriteBatch.Draw(Game1.menuGold, new Vector2(rectRightItems[i].X + 5, rectRightItems[i].Y + rectRightItems[i].Height - rectRightItems[i].Height / 6), new Rectangle(0,0,25,31),Color.White,0f,Vector2.Zero,1f,SpriteEffects.None,0f);
           
                //아이템 가격 표시 
                spriteBatch.DrawString(Game1.georgia, rightItems[i].GetCost().ToString(), new Vector2(rectRightItems[i].X + 40, rectRightItems[i].Y + rectRightItems[i].Height - rectRightItems[i].Height / 4), Color.White);
           
             
            }

                    
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
                

                //// to WEAR item
                //if(wearOne)
                //{
                //  //  Vector2 middle = new Vector2(width / 2, height / 2);
                //    spriteBatch.Draw(putPanel, middle, null, Color.White, 0f, new Vector2(310, 150), 1.5f, SpriteEffects.None, 1f);

                //    Vector2 yesButtonLocation = new Vector2(width / 2 - 180, height / 2 + 50);

                //    // mouse cursor on YES button
                //    if (!isHoverYesButton)
                //    {
                //        spriteBatch.Draw(yesButton, yesButtonLocation, null, Color.White, 0f, new Vector2(100, 60), 1.5f, SpriteEffects.None, 1f);
                //    }
                //    else
                //    {
                //        spriteBatch.Draw(hoverYesButton, yesButtonLocation, null, Color.White, 0f, new Vector2(100, 60), 1.5f, SpriteEffects.None, 1f);

                //    }
                //    Vector2 noButtonLocation = new Vector2(width / 2 + 180, height / 2 + 50);

                //    // mouse cursor on No button
                //    if (!isHoverNoButton)
                //    {
                //        spriteBatch.Draw(noButton, noButtonLocation, null, Color.White, 0f, new Vector2(100, 60), 1.5f, SpriteEffects.None, 1f);
                //    }
                //    else
                //    {
                //        spriteBatch.Draw(hoverNoButton, noButtonLocation, null, Color.White, 0f, new Vector2(100, 60), 1.5f, SpriteEffects.None, 1f);
                //    }

                //    recYesButton = new Rectangle((int)yesButtonLocation.X-100, (int)yesButtonLocation.Y-60, 200, 120);
                //    recNoButton = new Rectangle((int)noButtonLocation.X-100, (int)noButtonLocation.Y-60, 200, 120);

                //}


                //// 팔기 위해서
                //if (sellOne)
                //{
                //    //  Vector2 middle = new Vector2(width / 2, height / 2);
                //    spriteBatch.Draw(sellPanel, middle, null, Color.White, 0f, new Vector2(310, 150), 1.5f, SpriteEffects.None, 1f);

                //    Vector2 yesButtonLocation = new Vector2(width / 2 - 180, height / 2 + 50);

                //    // mouse cursor on YES button
                //    if (!isHoverSellYesButton)
                //    {
                //        spriteBatch.Draw(yesButton, yesButtonLocation, null, Color.White, 0f, new Vector2(100, 60), 1.5f, SpriteEffects.None, 1f);
                //    }
                //    else
                //    {
                //        spriteBatch.Draw(hoverYesButton, yesButtonLocation, null, Color.White, 0f, new Vector2(100, 60), 1.5f, SpriteEffects.None, 1f);

                //    }
                //    Vector2 noButtonLocation = new Vector2(width / 2 + 180, height / 2 + 50);

                //    // mouse cursor on No button
                //    if (!isHoverSellNoButton)
                //    {
                //        spriteBatch.Draw(noButton, noButtonLocation, null, Color.White, 0f, new Vector2(100, 60), 1.5f, SpriteEffects.None, 1f);
                //    }
                //    else
                //    {
                //        spriteBatch.Draw(hoverNoButton, noButtonLocation, null, Color.White, 0f, new Vector2(100, 60), 1.5f, SpriteEffects.None, 1f);
                //    }

                //    recYesButton = new Rectangle((int)yesButtonLocation.X - 100, (int)yesButtonLocation.Y - 60, 200, 120);
                //    recNoButton = new Rectangle((int)noButtonLocation.X - 100, (int)noButtonLocation.Y - 60, 200, 120);

                //}
            }
        }
    }
}