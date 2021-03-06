﻿using System;
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
    class NoteItemShop : ItemShop
    {


        private List<Item> noteItems;
        private List<Item> myNoteItems;
        private List<Rectangle> rectNoteItems = new List<Rectangle>();

        private Texture2D noteItemShopBackground;//
        MouseState mouseStatePrevious;//
        private Item selectedItem;//

        public NoteItemShop(ItemManager itemManager, ScoreManager scoreManager, ReportManager reportManager)
            : base(itemManager, scoreManager, reportManager)
        {

        }

        public override void LoadContent(ContentManager cm)
        {
            base.LoadContent(cm);
            noteItems = itemManager.getShopRightNoteItem(); ////////////////// getShopRightNoteItem이 아니라 getShopNoteItem 인듯
            myNoteItems = itemManager.getMyNoteItem();
            noteItemShopBackground = cm.Load<Texture2D>(@"noteItem\noteItemBackground");//
            setLocationItems();
        }

        public void addItemtoMyItem(Item item)
        {
            itemManager.addMyNoteItem(item);
            myNoteItems = itemManager.getMyNoteItem();

        }

        public void removeItemtoMyItem(Item item)
        {
            itemManager.removeMyNoteItem(item);

            //더하고 다시 갱신
            myNoteItems = itemManager.getMyNoteItem();

        }

        public List<Rectangle> getRectNoteItem()
        {

            return rectNoteItems;
        }

        public List<Item> getShopNoteItem()
        {

            return noteItems;
        }
        public bool haveOne(Item shopItem)
        {
            bool ret = false;
            int i;
            for (i = 0; i < myNoteItems.Count; i++)
            {
                if (shopItem == myNoteItems[i])
                {
                    ret = true;
                    i = myNoteItems.Count;
                }
            }
            return ret;

        }

        public void setLocationItems()
        {
            int i;
            int j = 0;
            if (noteItems.Count < 4)
            {
                for (i = 0; i < noteItems.Count; i++)
                {
                    Rectangle rectNoteHand = new Rectangle(i * 255 + 160, 200, 240, 240);
                    rectNoteItems.Add(rectNoteHand);
                }
            }
            else
            {
                for (i = 0; i < 3; i++)
                {
                    Rectangle rectNoteHand = new Rectangle(i * 255 + 160, 200, 240, 240);
                    rectNoteItems.Add(rectNoteHand);
                }

                for (i = 3; i < noteItems.Count; i++)
                {

                    Rectangle rectNoteHand = new Rectangle(j * 255 + 160, 450, 240, 240);
                    rectNoteItems.Add(rectNoteHand);
                    j++;
                }

            }

        }

        public override void Update(GameTime gameTime, Rectangle rightHandPosition)//
        {

            base.Update(gameTime, rightHandPosition);
            MouseState mouseStateCurrent = Mouse.GetState();
            mouseStatePrevious = Game1.mouseStatePrevious;

            Rectangle mouseRect = new Rectangle(mouseStateCurrent.X, mouseStateCurrent.Y, 5, 5);


            int i;
            //아이템 rect
            List<Rectangle> rectNoteItems = getRectNoteItem();

            //아이템/
            List<Item> shopNoteItems = getShopNoteItem();

            //다이얼로그를 띄웠을 때 이것이 중복실행되지 않도록 
            if (!getSellOrWearOne() && !getBuyOne() && !getNoGold() && !getHandInItem())
            {
                int count = 0;
                for (i = 0; i < rectNoteItems.Count; i++)
                {
                    //Trace.WriteLine(finalClick + " - " + pastClick);
                    //아이템을 선택 했을때
                    if (mouseRect.Intersects(rectNoteItems[i]) || rightHandPosition.Intersects(rectNoteItems[i]))
                    {

                        Game1.nearButton = true;
                        Game1.GetCenterOfButton(rectNoteItems[i]);
                        //if ((mouseRect.Intersects(rectRightItems[i]) && mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) ||
                        //    (drawrec1.Intersects(rectRightItems[i]) && finalClick && !pastClick))

                        if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (Game1.finalClick && !Game1.pastClick))
                        {

                            //nearButton = false;
                            //어두어짐
                            setDarkBackground(true);

                            selectedItem = shopNoteItems[i];

                            //이미 산거이면 true
                            if (haveOne(shopNoteItems[i]))
                            {
                                //장착 메시지 or 팔기 메시지 박스 띄우기 
                                setSellOrWearOne(true);
                                //반복 없애기
                                i = rectNoteItems.Count;
                            }
                            else
                            {
                                //구입 메시지 박스 띄우기 
                                setBuyOne(true);
                                //반복 없애기
                                i = rectNoteItems.Count;
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

                if (count == rectNoteItems.Count)
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
                            int index = itemManager.getIndexOfAllNoteItem(selectedItem);

                            //아이템을 찾았으면
                            if (index != -1)
                            {
                                //change index
                                //장착
                                itemManager.setNoteIndex(index);

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
                            int index = itemManager.getIndexOfAllNoteItem(selectedItem);

                            //찾은 아이템이 장착하고 있는 아이템이면 팔 수 없다.
                            if (index != itemManager.getNoteIndex())
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
            spriteBatch.Draw(noteItemShopBackground, new Rectangle(0, 0, 1024, 769), Color.White);//
            base.Draw(spriteBatch, width, height);

            int i;


            //보유아이템
            for (i = 0; i < noteItems.Count; i++)
            {
                spriteBatch.Draw(ItemBackground, rectNoteItems[i], Color.White);//

                if (haveOne(noteItems[i]))//
                {

                    spriteBatch.Draw(noteItems[i].ItemSprite.Texture, new Vector2(rectNoteItems[i].Center.X, rectNoteItems[i].Center.Y), new Rectangle(0, 0, noteItems[i].ItemSprite.Texture.Width, noteItems[i].ItemSprite.Texture.Height), Color.White, 0f, new Vector2(noteItems[i].ItemSprite.Texture.Width / 2, noteItems[i].ItemSprite.Texture.Height / 2), 0.7f, SpriteEffects.None, 1.0f);

                }

                else
                {
                    spriteBatch.Draw(noteItems[i].ItemSprite.Texture, new Vector2(rectNoteItems[i].Center.X, rectNoteItems[i].Center.Y), new Rectangle(0, 0, noteItems[i].ItemSprite.Texture.Width, noteItems[i].ItemSprite.Texture.Height), Color.Black, 0f, new Vector2(noteItems[i].ItemSprite.Texture.Width / 2, noteItems[i].ItemSprite.Texture.Height / 2), 0.7f, SpriteEffects.None, 1.0f);


                }


                spriteBatch.Draw(Game1.menuGold, new Vector2(rectNoteItems[i].X + 5, rectNoteItems[i].Y + rectNoteItems[i].Height - rectNoteItems[i].Height / 6), new Rectangle(0, 0, 25, 31), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

                spriteBatch.DrawString(Game1.georgia, noteItems[i].GetCost().ToString(), new Vector2(rectNoteItems[i].X + 40, rectNoteItems[i].Y + rectNoteItems[i].Height - rectNoteItems[i].Height / 4), Color.White);


                //rectRightItem.Add(rectRightHand);
            }

            if (itemManager.getNoteIndex() != -1)
            {

                wearItemLocation = new Vector2(rectNoteItems[itemManager.getNoteIndex()].X, rectNoteItems[itemManager.getNoteIndex()].Y);
                spriteBatch.Draw(wearItemMark, new Rectangle((int)wearItemLocation.X, (int)wearItemLocation.Y, wearItemMark.Width, wearItemMark.Height), Color.White);
            }
            spriteBatch.Draw(Game1.goldPlusEffect10, forthItemLocation, Color.White);
            spriteBatch.Draw(Game1.lifePlusEffect10, thirdItemLocation, Color.White);
            if (darkBackground)
            {
                Color color = Color.White;
                color.A = 50;
                spriteBatch.Draw(Game1.darkBackgroundImage, new Rectangle(0, 0, width, height), color);

                //Rectangle rectBuyPanel = new Rectangle(width / 2 - (buyPanel.Width / 2) - 100, height / 2 - (buyPanel.Height / 2) - 100, 200, 101);
                //spriteBatch.Draw(buyPanel,new Vector2(100,100), rectBuyPanel, Color.White,0f,new Vector2(0,0),1f,SpriteEffects.None,1f);
                Vector2 middle = new Vector2(width / 2, height / 2);


                //돈이 부족 하면 팝업 띄운다.
                //show up message if there is not enough money
                if (noGold)
                {
                    spriteBatch.Draw(noGoldBackground, new Vector2(0, 0), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

                    spriteBatch.Draw(noGoldButton, new Vector2(middle.X - 369, middle.Y), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

                    //마우스 커서가 버튼위에 올라가면
                    //mouse cursor on noGoldButton
                    if (isHoverNoGoldButton)
                    {
                        spriteBatch.Draw(hoverNoGoldButton, new Vector2(middle.X - 369, middle.Y), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

                    }


                    recNoGoldButton = new Rectangle((int)(middle.X - 369), (int)middle.Y, 738, 241);
                }


                if (handInItem)
                {
                    //   spriteBatch.Draw(handInItemBackground, middle, null, Color.White, 0f, new Vector2(350, 200), 1f, SpriteEffects.None, 1f);
                    spriteBatch.Draw(handInItemBackground, new Vector2(0, 0), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

                    //텍스쳐만 가져옴
                    spriteBatch.Draw(noGoldButton, new Vector2(middle.X - 369, middle.Y), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

                    //마우스 커서가 버튼위에 올라가면
                    //mouse cursor on noGoldButton
                    if (isHoverHandInItem)
                    {
                        spriteBatch.Draw(hoverNoGoldButton, new Vector2(middle.X - 369, middle.Y), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

                    }


                    recHandInButton = new Rectangle((int)(middle.X - 369), (int)middle.Y, 738, 241);
                }


                if (sellOrWearOne)
                {
                    spriteBatch.Draw(sellorwearPanel, new Vector2(0, 0), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 1f);

                    //장착 버튼 
                    Vector2 wearButtonLocation = new Vector2(width / 2 - 270, height / 2 + 40);

                    // mouse cursor on YES button
                    if (!isHoverWearButton)
                    {

                        spriteBatch.Draw(sWearButton, wearButtonLocation, null, Color.White, 0f, new Vector2(100, 60), 1f, SpriteEffects.None, 1f);

                    }
                    else
                    {

                        spriteBatch.Draw(hoverSWearButton, wearButtonLocation, null, Color.White, 0f, new Vector2(100, 60), 1f, SpriteEffects.None, 1f);

                    }

                    //팔기버튼
                    Vector2 sellButtonLocation = new Vector2(width / 2, height / 2 + 40);

                    // mouse cursor on YES button
                    if (!isHoverSellButton)
                    {

                        spriteBatch.Draw(sSellButton, sellButtonLocation, null, Color.White, 0f, new Vector2(100, 60), 1f, SpriteEffects.None, 1f);

                    }
                    else
                    {

                        spriteBatch.Draw(hoverSSellButton, sellButtonLocation, null, Color.White, 0f, new Vector2(100, 60), 1f, SpriteEffects.None, 1f);

                    }


                    //취소
                    Vector2 cancelButtonLocation = new Vector2(width / 2 + 270, height / 2 + 40);

                    // mouse cursor on YES button
                    if (!isHoverCancelButton)
                    {

                        spriteBatch.Draw(sCancelButton, cancelButtonLocation, null, Color.White, 0f, new Vector2(100, 60), 1f, SpriteEffects.None, 1f);

                    }
                    else
                    {

                        spriteBatch.Draw(hoverSCancelButton, cancelButtonLocation, null, Color.White, 0f, new Vector2(100, 60), 1f, SpriteEffects.None, 1f);

                    }



                    recWearButton = new Rectangle((int)wearButtonLocation.X - 100, (int)wearButtonLocation.Y - 60, 200, 120);
                    recSellButton = new Rectangle((int)sellButtonLocation.X - 100, (int)sellButtonLocation.Y - 60, 200, 120);
                    recCancelButton = new Rectangle((int)cancelButtonLocation.X - 100, (int)cancelButtonLocation.Y - 60, 200, 120);


                }
                //to BUY item
                if (buyOne)
                {
                    spriteBatch.Draw(buyPanel, new Vector2(0, 0), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 1f);

                    Vector2 yesButtonLocation = new Vector2(width / 2 - 190, height / 2 + 100);

                    // mouse cursor on YES button
                    if (!isHoverYesButton)
                    {

                        spriteBatch.Draw(yesButton, yesButtonLocation, null, Color.White, 0f, new Vector2(yesButton.Width / 2, yesButton.Height / 2), 1f, SpriteEffects.None, 1f);

                    }
                    else
                    {

                        spriteBatch.Draw(hoverYesButton, yesButtonLocation, null, Color.White, 0f, new Vector2(hoverYesButton.Width / 2, hoverYesButton.Height / 2), 1f, SpriteEffects.None, 1f);

                    }


                    Vector2 noButtonLocation = new Vector2(width / 2 + 190, height / 2 + 100);


                    // mouse cursor on NO bUTTON
                    if (!isHoverNoButton)
                    {
                        spriteBatch.Draw(noButton, noButtonLocation, null, Color.White, 0f, new Vector2(noButton.Width / 2, noButton.Height / 2), 1f, SpriteEffects.None, 1f);

                    }

                    else
                    {
                        spriteBatch.Draw(hoverNoButton, noButtonLocation, null, Color.White, 0f, new Vector2(hoverNoButton.Width / 2, hoverNoButton.Height / 2), 1f, SpriteEffects.None, 1f);

                    }

                    recYesButton = new Rectangle((int)yesButtonLocation.X - yesButton.Width / 2, (int)yesButtonLocation.Y - yesButton.Height / 2, yesButton.Width, yesButton.Height);

                    recNoButton = new Rectangle((int)noButtonLocation.X - noButton.Width / 2, (int)noButtonLocation.Y - noButton.Height / 2, noButton.Width, noButton.Height);

                }


              
            }

        }



    }
}
