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

        //아이템 텍스쳐
        private Texture2D[] rightHandTexture = new Texture2D[5];
        private Texture2D[] leftHandTexture = new Texture2D[5];

        //내가 착용한 아이템-착용함수 제작
        private int rightHandIndex = 0;
        private int leftHandIndex = 0;

        //내가 가지고 있는 아이템
        public List<Item> myRightHandItem = new List<Item>();
        private List<Item> myLeftHandItem = new List<Item>();

        public ItemManager()
        {
                      

        }

        public void Init()
        {
            int i;
            for (i = 0; i < 2; i++)
            {
                addItem(rightHandItem, new Vector2(100, 100), rightHandTexture[i], new Rectangle(0, 0, rightHandTexture[i].Width, rightHandTexture[i].Height), 1);
            }
            for (i = 0; i < 2; i++)
            {
                addItem(leftHandItem, new Vector2(100, 100), leftHandTexture[i], new Rectangle(0, 0, leftHandTexture[i].Width, leftHandTexture[i].Height), 1);
            }

            //test
            buyItem(myRightHandItem, rightHandItem[0]);
          //  buyItem(myRightHandItem, rightHandItem[1]);
        }

        public void LoadContent(ContentManager cm)
        {

            rightHandTexture[0] = cm.Load<Texture2D>(@"Textures\red");
            rightHandTexture[1] = cm.Load<Texture2D>(@"Textures\heart");

            leftHandTexture[0] = cm.Load<Texture2D>(@"Bitmap1");
            leftHandTexture[1] = cm.Load<Texture2D>(@"Bitmap2");

            //노트는 좌표값
        }

      
        private void addItem(
          List<Item> itemList,
          Vector2 location,
          Texture2D texture,
          Rectangle initalFrame,
          int frameCount
            )
        {
            Item thisItem = new Item(
                texture,
                location,
                initalFrame,
                frameCount);
            itemList.Add(thisItem);
        }

        public void buyItem( List<Item> itemArray ,Item item)
        {
            itemArray.Add(item);
        }

        public void sellItem(List<Item> itemArray, Item item)
        {
            itemArray.Remove(item);
        }
        
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
    

        public List<Item> getMyRightHandItem()
        {
            return this.myRightHandItem;
        }

        public List<Item> getMyLeftHandItem()
        {
            return this.myLeftHandItem;
        }
        public void addMyRightHandItem(Item item)
        {

            this.myRightHandItem.Add(item);

        }
        public void addMyLeftHandItem(Item item)
        {
            this.myLeftHandItem.Add(item);


        }


        public List<Item> getShopRightHandItem()
        {
            return this.rightHandItem;
        }

        public List<Item> getShopLeftHandItem()
        {
            return this.leftHandItem;
        }



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
    }
}
