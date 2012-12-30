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
    class ItemManager
    {
        //상점 아이템
        private List<Item> rightHandItem = new List<Item>();
        private List<Item> leftHandItem = new List<Item>();
        private List<Item> effectItem = new List<Item>();
        private List<Item> noteItem = new List<Item>();
        private List<Item> backgroundItem = new List<Item>();
      
       
        //아이템 텍스쳐
        private Texture2D[] rightHandTexture = new Texture2D[5];
        private Texture2D[] leftHandTexture = new Texture2D[5];
        private Texture2D[] effectTexture = new Texture2D[5];
        private Texture2D[] noteTexture = new Texture2D[5];
        private Texture2D[] backgroundTexture = new Texture2D[5];

   
        //내가 착용한 아이템-착용함수 제작
        private int rightHandIndex = 0;
        private int leftHandIndex = 0;
        private int effectIndex = 0;
        private int noteIndex = 0;
        private int backgroundIndex = 0;

        //내가 가지고 있는 아이템
        public List<Item> myRightHandItem = new List<Item>();
        private List<Item> myLeftHandItem = new List<Item>();
        public List<Item> myEffectItem = new List<Item>();
        private List<Item> myNoteItem = new List<Item>();
        public List<Item> myBackgroundItem = new List<Item>();

        public ItemManager()
        {
                      

        }

        public void Init()
        {
            int i;
            for (i = 0; i < 4; i++)
            {
                addItem(rightHandItem, new Vector2(100, 100), rightHandTexture[i], new Rectangle(0, 0, rightHandTexture[i].Width, rightHandTexture[i].Height), 1,/*cost*/ (i+1)*5);
            }
                for (i = 0; i < 2; i++)
            {
                addItem(leftHandItem, new Vector2(100, 100), leftHandTexture[i], new Rectangle(0, 0, leftHandTexture[i].Width, leftHandTexture[i].Height), 1,/*cost*/ 7);
                
                //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                //have to additem, effect, node , background
                addItem(effectItem, new Vector2(100, 100), effectTexture[i], new Rectangle(0, 0, effectTexture[i].Width, effectTexture[i].Height), 1,/*cost*/ 16);

                addItem(noteItem, new Vector2(100, 100), noteTexture[i], new Rectangle(0, 0, noteTexture[i].Width, noteTexture[i].Height), 1,/*cost*/ 25);

                addItem(backgroundItem, new Vector2(100, 100), backgroundTexture[i], new Rectangle(0, 0, backgroundTexture[i].Width, backgroundTexture[i].Height), 1,/*cost*/ 45);
            
            }

         

            //test
            buyItem(myRightHandItem, rightHandItem[0]);
            buyItem(myLeftHandItem, leftHandItem[0]);
            buyItem(myEffectItem, effectItem[0]);
            buyItem(myNoteItem, noteItem[0]);
            buyItem(myBackgroundItem, backgroundItem[0]);
            
        
        }

        public void LoadContent(ContentManager cm)
        {

            rightHandTexture[0] = cm.Load<Texture2D>(@"rightItem\s1");
            rightHandTexture[1] = cm.Load<Texture2D>(@"rightItem\s2");
            rightHandTexture[2] = cm.Load<Texture2D>(@"rightItem\s3");
            rightHandTexture[3] = cm.Load<Texture2D>(@"rightItem\s4");
            rightHandTexture[4] = cm.Load<Texture2D>(@"rightItem\s5");

            leftHandTexture[0] = cm.Load<Texture2D>(@"Bitmap1");
            leftHandTexture[1] = cm.Load<Texture2D>(@"Bitmap2");

            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //have to additem, effect, node , background

            effectTexture[0] = cm.Load<Texture2D>(@"Textures\red");
            effectTexture[1] = cm.Load<Texture2D>(@"Textures\heart");

            noteTexture[0] = cm.Load<Texture2D>(@"Bitmap1");
            noteTexture[1] = cm.Load<Texture2D>(@"Bitmap2");


            backgroundTexture[0] = cm.Load<Texture2D>(@"Textures\red");
            backgroundTexture[1] = cm.Load<Texture2D>(@"Textures\heart");



        }

      
        private void addItem(
          List<Item> itemList,
          Vector2 location,
          Texture2D texture,
          Rectangle initalFrame,
          int frameCount,
          int cost
            )
        {
            Item thisItem = new Item(
                texture,
                location,
                initalFrame,
                frameCount,
                cost);
            itemList.Add(thisItem);
        }

        public void buyItem( List<Item> itemArray ,Item item)
        {
            itemArray.Add(item);
        }

        //maybe no function sell item
        //public void sellItem(List<Item> itemArray, Item item)
        //{
        //    itemArray.Remove(item);
        //}
        

        ///////////////////index
        public void setRightHandIndex(int index)
        {
            this.rightHandIndex = index;
        }

        public int getRightHandIndex()
        {
            return this.rightHandIndex;
        }

        public void setLeftHandIndex(int index)
        {
            this.leftHandIndex = index;
        }

        public int getLeftHandIndex()
        {
            return this.leftHandIndex;
        }

        public void setEffectIndex(int index)
        {
            this.effectIndex = index;
        }

        public int getEffectIndex()
        {
            return this.effectIndex;
        }

        public void setNoteIndex(int index)
        {
            this.noteIndex = index;
        }

        public int getNoteIndex()
        {
            return this.noteIndex;
        }


        public void setBackgroundIndex(int index)
        {
            this.backgroundIndex = index;
        }

        public int getBackgroundIndex()
        {
            return this.backgroundIndex;
        }
     
        ////////////////get MY item list////////////////////////////
 

        public List<Item> getMyRightHandItem()
        {
            return this.myRightHandItem;
        }

        public List<Item> getMyLeftHandItem()
        {
            return this.myLeftHandItem;
        }

        public List<Item> getMyNoteItem()
        {
            return this.myNoteItem;
        }

        public List<Item> getMyEffectItem()
        {
            return this.myEffectItem;
        }
        public List<Item> getMyBackgroundItem()
        {
            return this.myBackgroundItem;
        }

        ///////////////////add item //////////////////////
        
        public void addMyRightHandItem(Item item)
        {
            this.myRightHandItem.Add(item);
        }
        public void addMyLeftHandItem(Item item)
        {
            this.myLeftHandItem.Add(item);
        }
        public void addMyEffectItem(Item item)
        {
            this.myEffectItem.Add(item);
        }
        public void addMyNoteItem(Item item)
        {
            this.myNoteItem.Add(item);
        }
        public void addMyBackgroundItem(Item item)
        {
            this.myBackgroundItem.Add(item);
        }


        //GET SHOP ITEM LIST

        public List<Item> getShopRightHandItem()
        {
            return this.rightHandItem;
        }

        public List<Item> getShopLeftHandItem()
        {
            return this.leftHandItem;
        }

        public List<Item> getShopEffectItem()
        {
            return this.effectItem;
        }

        public List<Item> getShopNoteItem()
        {
            return this.noteItem;
        }

        public List<Item> getShopBackgroundItem()
        {
            return this.backgroundItem;
        }

        //get index 

        public int getIndexOfMyRightItem(Item item)
        {
            int i;
            int index = -1;
            for (i = 0; i < myRightHandItem.Count; i++)
            {
                if (item == myRightHandItem[i])
                {
                    index = i;
                }   
            }
            return index;
        }


        public int getIndexOfMyLeftItem(Item item)
        {
            int i;
            int index = -1;
            for (i = 0; i < myLeftHandItem.Count; i++)
            {
                if (item == myLeftHandItem[i])
                {
                    index = i;
                }
            }
            return index;
        }

        public int getIndexOfMyEffectItem(Item item)
        {
            int i;
            int index = -1;
            for (i = 0; i < myEffectItem.Count; i++)
            {
                if (item == myEffectItem[i])
                {
                    index = i;
                }
            }
            return index;
        }

        public int getIndexOfMyNoteItem(Item item)
        {
            int i;
            int index = -1;
            for (i = 0; i < myNoteItem.Count; i++)
            {
                if (item == myNoteItem[i])
                {
                    index = i;
                }

            }

            return index;

        }

        public int getIndexOfMyBackgroundItem(Item item)
        {
            int i;
            int index = -1;
            for (i = 0; i < myBackgroundItem.Count; i++)
            {
                if (item == myBackgroundItem[i])
                {
                    index = i;
                }

            }

            return index;

        }
    }
}
