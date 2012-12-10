using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
namespace beethoven3
{
    class NoteItemShop : ItemShop
    {


        private List<Item> noteItems;
        private List<Item> myNoteItems;
        private List<Rectangle> rectNoteItems = new List<Rectangle>();



        public NoteItemShop(ItemManager itemManager)
            : base(itemManager)
        {

        }

        public override void LoadContent(ContentManager cm)
        {
            base.LoadContent(cm);
            noteItems = itemManager.getShopNoteItem();
            myNoteItems = itemManager.getMyNoteItem();
            setLocationItems();
        }

        public void addItemtoMyItem(Item item)
        {
            itemManager.addMyNoteItem(item);
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
            for (i = 0; i < noteItems.Count; i++)
            {

                Rectangle rectNote = new Rectangle(300, i * 150, 100, 100);
                rectNoteItems.Add(rectNote);
            }

        }



        public override void Draw(SpriteBatch spriteBatch, int width, int height)
        {
            int i;


            //보유아이템
            for (i = 0; i < noteItems.Count; i++)
            {
                //Rectangle rectRightHand = new Rectangle(300, i * 150, 100, 100);

                // bool isHave = false;

                //Item I already have
                if (haveOne(noteItems[i]))
                {
                    spriteBatch.Draw(noteItems[i].ItemSprite.Texture, rectNoteItems[i], Color.White);
                }

                else
                {
                    spriteBatch.Draw(noteItems[i].ItemSprite.Texture, rectNoteItems[i], Color.Black);
                }


                //rectRightItem.Add(rectRightHand);
            }

            //장착아이템
            Rectangle usedItemRect = new Rectangle(50, 50, 100, 100);
            // Color myColor = Color.White;
            //  myColor.A = 50;

            spriteBatch.Draw(myNoteItems[itemManager.getLeftHandIndex()].ItemSprite.Texture, usedItemRect, Color.White);

            if (darkBackground)
            {
                Color color = Color.White;
                color.A = 50;
                spriteBatch.Draw(darkBackgroundImage, new Rectangle(0, 0, width, height), color);

                //Rectangle rectBuyPanel = new Rectangle(width / 2 - (buyPanel.Width / 2) - 100, height / 2 - (buyPanel.Height / 2) - 100, 200, 101);
                //spriteBatch.Draw(buyPanel,new Vector2(100,100), rectBuyPanel, Color.White,0f,new Vector2(0,0),1f,SpriteEffects.None,1f);


                //to BUY item
                if (buyOne)
                {

                    Vector2 middle = new Vector2(width / 2, height / 2);
                    spriteBatch.Draw(buyPanel, middle, null, Color.White, 0f, new Vector2(310, 150), 1.5f, SpriteEffects.None, 1f);


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

                    // mouse cursor on NO bUTTON
                    if (!isHoverNoButton)
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


                // to WEAR item
                if (wearOne)
                {
                    Vector2 middle = new Vector2(width / 2, height / 2);
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

                    recYesButton = new Rectangle((int)yesButtonLocation.X - 100, (int)yesButtonLocation.Y - 60, 200, 120);
                    recNoButton = new Rectangle((int)noButtonLocation.X - 100, (int)noButtonLocation.Y - 60, 200, 120);

                }
            }
        }



    }
}
