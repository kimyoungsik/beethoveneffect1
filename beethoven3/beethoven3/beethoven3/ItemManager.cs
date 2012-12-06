using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
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
        private int rightHandIndex = 1;
        private int leftHandIndex = 1;

        //내가 가지고 있는 아이템
        private List<Item> myRightHandItem = new List<Item>();
        private List<Item> myLeftHandItem = new List<Item>();

        public ItemManager()
        {
            int i;
            for (i = 0; i < 1; i++)
            {
                addItem(rightHandItem, new Vector2(100, 100), rightHandTexture[i], new Rectangle(0, 0, rightHandTexture[i].Width, rightHandTexture[i].Height), 1);
            }
            for (i = 0; i < 1; i++)
            {
                addItem(leftHandItem, new Vector2(100, 100), leftHandTexture[i], new Rectangle(0, 0, leftHandTexture[i].Width, leftHandTexture[i].Height), 1);
            }
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

        public void setLeftHandIndex(int index)
        {
            this.leftHandIndex = index;
        }

    }
}
