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


        //내가 가지고 있는 아이템
        private List<Item> myRightHandItem = new List<Item>();
        private List<Item> myLeftHandItem = new List<Item>();
        private List<Item> myEffectItem = new List<Item>();
        private List<Item> myNoteItem = new List<Item>();
        private List<Item> myBackgroundItem = new List<Item>();


        //아이템 텍스쳐
        private Texture2D[] rightHandTexture = new Texture2D[5];
        private Texture2D[] leftHandTexture = new Texture2D[5];
        private Texture2D[] effectTexture = new Texture2D[5];
        private Texture2D[] noteTexture = new Texture2D[5];
        private Texture2D[] backgroundTexture = new Texture2D[5];
        
        //팔지는 않지만 텍스쳐만 가지고 있음
        private Texture2D[] markTexture = new Texture2D[5];
        
        //Great의 이펙트 말고 다른 이펙트는 숨겨서 표현
        private Texture2D[] goodEffectTexture = new Texture2D[5];
        private Texture2D[] badEffectTexture = new Texture2D[5];
        private Texture2D[] missEffectTexture = new Texture2D[5];
        
        
        
        //롱노트라 왼손노트를 따로 만들 때 쓰임
        //***  private Texture2D[] longNoteTexture = new Texture2D[5];
        //***  private Texture2D[] leftNoteTexture = new Texture2D[5];

        
        //내가 착용한 아이템-착용함수 제작
        //이걸 베이스를 자기가 산 아이템만 해당하는 것이 아니라 전체를 베이스로 

        private int rightHandIndex = 0;
        private int leftHandIndex = 0;
        private int effectIndex = 0;
        private int noteIndex =0;
        private int backgroundIndex = 0;




        //이펙트 특성  -start

        //각 이펙트마다 rect init
        private Rectangle[] effectInitFrams = new Rectangle[5];
        //각 이펙트 마다 frameCount
        private int[] effectFrameCount = new int[5];
        //각 이펙트 마다 scale
        private float[] effecScale = new float[5];
        //지속기간
        private int[] effectDulation = new int[5];

        //이펙트 특성  -end

        //노트 특성
        float[] rightNoteScale = new float[5];

        //마커특성
        private float[] markersScale = new float[5];
                

        public ItemManager()
        {
                      
        }

        public void Init()
        {
            int i;

            //아이템 상점에 올림
            
            //오른손 아이템 
            for (i = 0; i < 4; i++)
            {
                addItem(rightHandItem, new Vector2(100, 100), rightHandTexture[i], new Rectangle(0, 0, rightHandTexture[i].Width, rightHandTexture[i].Height), 1,/*cost*/ (i+1)*5);
            }

            //노트 아이템
            for (i = 0; i < 2; i++)
            {
                addItem(noteItem, new Vector2(100, 100), noteTexture[i], new Rectangle(0, 0, noteTexture[i].Width, noteTexture[i].Height), 1,/*cost*/ 25);
            }

            //왼손 아이템
            for (i = 0; i < 2; i++)
            {
                addItem(leftHandItem, new Vector2(100, 100), leftHandTexture[i], new Rectangle(0, 0, leftHandTexture[i].Width, leftHandTexture[i].Height), 1,/*cost*/ 7);
                
                //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                //have to additem, effect, node , background
            }

            //배경아이템
            for (i = 0; i < 2; i++)
            {
                addItem(backgroundItem, new Vector2(100, 100), backgroundTexture[i], new Rectangle(0, 0, backgroundTexture[i].Width, backgroundTexture[i].Height), 1,/*cost*/ 45);
            }


            //이펙트 아이템
            for (i = 0; i < 3; i++)
            {
                addItem(effectItem, new Vector2(100, 100), effectTexture[i], new Rectangle(0, 0, effectTexture[i].Width, effectTexture[i].Height), 1,/*cost*/ 16);
              
            }


            //GREAT-BAD-MISS-GOOD모든 이펙트가 다음과 같이 따라감
            //이펙트 특성 -start
            
            
            effectInitFrams[0] = new Rectangle(0, 0, 166, 162);
            effectInitFrams[1] = new Rectangle(0, 0, 130, 122);
            effectInitFrams[2] = new Rectangle(0, 0, 156, 152);

            effectFrameCount[0] = 9;
            effectFrameCount[1] = 8;
            effectFrameCount[2] = 6;
           
            
           
            effecScale[0] = 1.0f;
            effecScale[1] = 1.0f;
            effecScale[2] = 1.0f;

            //지속시간
            effectDulation[0] =  45;
            effectDulation[1] =  45;
            effectDulation[2] =  45;
           //이펙트 특성 -end



            //마커 특성 -
            markersScale[0] = 1.0f;

            //노트 특성
            
            rightNoteScale[0] = 0.5f;

            //test

            //구입한 아이템으로 설정
            buyItem(myRightHandItem, rightHandItem[0]);
            buyItem(myLeftHandItem, leftHandItem[0]);
            buyItem(myEffectItem, effectItem[0]);
            buyItem(myNoteItem, noteItem[0]);
            buyItem(myBackgroundItem, backgroundItem[0]);
        }

        //각 아이템 텍스쳐 저장
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
            effectTexture[0] = cm.Load<Texture2D>(@"Explosion\starExplosion2");
            effectTexture[1] = cm.Load<Texture2D>(@"Explosion\needleExplosion2");
            effectTexture[2] = cm.Load<Texture2D>(@"Explosion\windExplosion2");

            goodEffectTexture[0] = cm.Load<Texture2D>(@"Explosion\starExplosion2");
            goodEffectTexture[1] = cm.Load<Texture2D>(@"Explosion\needleExplosion2");
            goodEffectTexture[2] = cm.Load<Texture2D>(@"Explosion\windExplosion2");

            badEffectTexture[0] = cm.Load<Texture2D>(@"Explosion\starExplosion");
            badEffectTexture[1] = cm.Load<Texture2D>(@"Explosion\needleExplosion2");
            badEffectTexture[2] = cm.Load<Texture2D>(@"Explosion\windExplosion2");

            missEffectTexture[0] = cm.Load<Texture2D>(@"Explosion\starExplosion");
            missEffectTexture[1] = cm.Load<Texture2D>(@"Explosion\needleExplosion2");
            missEffectTexture[2] = cm.Load<Texture2D>(@"Explosion\windExplosion2");
         
            noteTexture[0] = cm.Load<Texture2D>(@"notes\starNote");
            noteTexture[1] = cm.Load<Texture2D>(@"notes\turnNote2");
            
            //노트랑 한쌍이다.
            markTexture[0] = cm.Load<Texture2D>(@"markers\whiteMarker");
            markTexture[1] = cm.Load<Texture2D>(@"markers\turnMarker");
           // markTexture[2] = cm.Load<Texture2D>(@"markers\whiteMarker");

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
        
        /////////////////////////////////
        //버튼을 눌러서 구입 ,팔기는 add~, remove를 이용하고
        //이것은 코드상에서 할 수 있도록 함

        //아이템 구입
        public void buyItem( List<Item> itemArray ,Item item)
        {
            itemArray.Add(item);
        }

        //아이템 팔기
        public void sellItem(List<Item> itemArray, Item item)
        {
            itemArray.Remove(item);
        }
        //////////////////////////////////////

        //노트특성 Get하기
        public float[] GetRightNoteScale()
        {
            return this.rightNoteScale;
        }
        
        //마커특성 Get하기
        public float[] GetMarkersScale()
        {
            return this.markersScale;
        }
        
        //effect특성 Get하기 - start
        public  Rectangle[] GetEffectInitFrame()
        {
            return this.effectInitFrams;
        }

        public  int[] GetEffectFrameCount()
        {
            return this.effectFrameCount;
        }

        public  float[] GetEffectScale()
        {
            return this.effecScale;
        }

        public int[] GetEffectDulation()
        {
            return this.effectDulation;
        }

        //텍스쳐 반환
        //오른손텍스쳐 반환 , 게임 로딩전에 각 노트 배경 등을 변경
        public Texture2D[] GetRightHandTexture()
        {
            return this.rightHandTexture;
        }

        public Texture2D[] GetLeftHandTexture()
        {
            return this.leftHandTexture;
        }

        public Texture2D[] GetNoteTexture()
        {
            return this.noteTexture;
        }

        public Texture2D[] GetEffectTexture()
        {
            return this.effectTexture;
        }


        public Texture2D[] GetBackgroundTexture()
        {
            return this.backgroundTexture;
        }

        public Texture2D[] GetMarkerTexture()
        {
            return this.markTexture;
        }

        public Texture2D[] GetGoodEffectTexture()
        {
            return this.goodEffectTexture;
        }


        public Texture2D[] GetBadEffectTexture()
        {
            return this.badEffectTexture;
        }

        public Texture2D[] GetMissEffectTexture()
        {
            return this.missEffectTexture;
        }


        //현재 자기가 장착한 인덱스 set , get
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
 
        //내가 가지고 있는 아이템 리스트가져오기
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


        //물건 사게 되면 내 아이템리스트에 추가하기 
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

        //물건 팔게되면 내 아이템에서 빼기
        public void removeMyRightHandItem(Item item)
        {
            this.myRightHandItem.Remove(item);
        }
        public void removeMyLeftHandItem(Item item)
        {
            this.myLeftHandItem.Remove(item);
        }
        public void removeMyEffectItem(Item item)
        {
            this.myEffectItem.Remove(item);
        }
        public void removeMyNoteItem(Item item)
        {
            this.myNoteItem.Remove(item);
        }
        public void removeMyBackgroundItem(Item item)
        {
            this.myBackgroundItem.Remove(item);
        }


        // 샵에 있는 전체 아이템 리스트 가져오기
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

        //아이템을 넣으면 그 인덱스가 나타난다.
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


        //자신의 장착아이템이 아니라 전체에서 찾는다. 
        //get index 

        //아이템을 넣으면 그 인덱스가 나타난다.
        public int getIndexOfAllRightItem(Item item)
        {
            int i;
            int index = -1;
            for (i = 0; i < rightHandItem.Count; i++)
            {
                if (item == rightHandItem[i])
                {
                    index = i;
                }
            }
            return index;
        }


        public int getIndexOfAllLeftItem(Item item)
        {
            int i;
            int index = -1;
            for (i = 0; i < leftHandItem.Count; i++)
            {
                if (item == leftHandItem[i])
                {
                    index = i;
                }
            }
            return index;
        }


        public int getIndexOfAllEffectItem(Item item)
        {
            int i;
            int index = -1;
            for (i = 0; i < effectItem.Count; i++)
            {
                if (item == effectItem[i])
                {
                    index = i;
                }
            }
            return index;
        }

     


        public int getIndexOfAllNoteItem(Item item)
        {
            int i;
            int index = -1;
            for (i = 0; i < noteItem.Count; i++)
            {
                if (item == noteItem[i])
                {
                    index = i;
                }

            }

            return index;

        }

        public int getIndexOfAllBackgroundItem(Item item)
        {
            int i;
            int index = -1;
            for (i = 0; i < backgroundItem.Count; i++)
            {
                if (item == backgroundItem[i])
                {
                    index = i;
                }

            }

            return index;

        }


    }
}
