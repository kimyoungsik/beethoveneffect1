﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
namespace beethoven3
{
    class ItemManager
    {
        //상점 아이템
        private List<Item> rightHandItem = new List<Item>();
        private List<Item> leftHandItem = new List<Item>();
        private List<Item> effectItem = new List<Item>();
        private List<Item> rightNoteItem = new List<Item>();
        private List<Item> leftNoteItem = new List<Item>();
        private List<Item> dragNoteItem = new List<Item>();
        private List<Item> longNoteItem = new List<Item>();
                
        private List<Item> backgroundItem = new List<Item>();


        //내가 가지고 있는 아이템
        private List<Item> myRightHandItem = new List<Item>();
        private List<Item> myLeftHandItem = new List<Item>();
        private List<Item> myEffectItem = new List<Item>();
        private List<Item> myNoteItem = new List<Item>();
        private List<Item> myBackgroundItem = new List<Item>();


        //아이템 텍스쳐
        private Texture2D[] rightHandTexture = new Texture2D[6];
        private Texture2D[] leftHandTexture = new Texture2D[6];
        private Texture2D[] effectTexture = new Texture2D[6];
        private Texture2D[] rightNoteTexture = new Texture2D[6];
        private Texture2D[] leftNoteTexture = new Texture2D[6];
        private Texture2D[] dragNoteTexture = new Texture2D[6];
        private Texture2D[] longNoteTexture = new Texture2D[6];
        private Texture2D[] backgroundTexture = new Texture2D[6];
        
        //섬네일

        private Texture2D[] effectThumnail = new Texture2D[6];
        private Texture2D[] backgroundThumnail = new Texture2D[6];


        //팔지는 않지만 텍스쳐만 가지고 있음
        private Texture2D[] markTexture = new Texture2D[6];
        
        //Great의 이펙트 말고 다른 이펙트는 숨겨서 표현
        private Texture2D[] goodEffectTexture = new Texture2D[6];
        private Texture2D[] badEffectTexture = new Texture2D[6];
        private Texture2D[] missEffectTexture = new Texture2D[6];
        
        


        
        //롱노트라 왼손노트를 따로 만들 때 쓰임
        //***  private Texture2D[] longNoteTexture = new Texture2D[5];
        //***  private Texture2D[] leftNoteTexture = new Texture2D[5];

        
        //내가 착용한 아이템-착용함수 제작
        //이걸 베이스를 자기가 산 아이템만 해당하는 것이 아니라 전체를 베이스로 

        private int rightHandIndex = -1;
        private int leftHandIndex = -1;
        private int effectIndex = -1;
        private int noteIndex =-1;
        private int backgroundIndex = -1;




        //이펙트 특성  -start

        //각 이펙트마다 rect init
        private Rectangle[] effectInitFrams = new Rectangle[6];
        //각 이펙트 마다 frameCount
        private int[] effectFrameCount = new int[6];
        //각 이펙트 마다 scale
        private float[] effecScale = new float[6];
        //지속기간
        private int[] effectDulation = new int[6];

        //이펙트 특성  -end

        //노트 특성
        float[] rightNoteScale = new float[6];

        //마커특성
        private float[] markersScale = new float[6];
                

        public ItemManager()
        {
                      
        }

        public void Init()
        {
            int i;

            //아이템 상점에 올림
            
            //오른손 아이템 
            for (i = 0; i < 6; i++)
            {
                addItem(rightHandItem, new Vector2(100, 100), rightHandTexture[i], new Rectangle(0, 0, rightHandTexture[i].Width, rightHandTexture[i].Height), 1,/*cost*/ (i+1)*5);
            }

            //노트 아이템
            for (i = 0; i < 4; i++)
            {
                if (i != 3)
                {
                    addItem(rightNoteItem, new Vector2(100, 100), rightNoteTexture[i], new Rectangle(0, 0, rightNoteTexture[i].Width, rightNoteTexture[i].Height), 1,/*cost*/ 25);
                }
                //고스톱이면 보이는 텍스쳐 롱노트로 하려고

                else if (i == 3)
                {
                    addItem(rightNoteItem, new Vector2(100, 100), longNoteTexture[i], new Rectangle(0, 0, longNoteTexture[i].Width, longNoteTexture[i].Height), 1,/*cost*/ 25);
                }

            }



            //왼손 아이템
            for (i = 0; i < 4; i++)
            {
                addItem(leftHandItem, new Vector2(100, 100), leftHandTexture[i], new Rectangle(0, 0, leftHandTexture[i].Width, leftHandTexture[i].Height), 1,/*cost*/ 7);
                
                //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                //have to additem, effect, node , background
            }

            //배경아이템
            for (i = 0; i < 4; i++)
            {
                addItem(backgroundItem, new Vector2(100, 100), backgroundTexture[i], new Rectangle(0, 0, backgroundTexture[i].Width, backgroundTexture[i].Height), 1,/*cost*/ 45,backgroundThumnail[i]);
            }


            //이펙트 아이템
            for (i = 0; i < 4; i++)
            {
                addItem(effectItem, new Vector2(100, 100), effectTexture[i], new Rectangle(0, 0, effectTexture[i].Width, effectTexture[i].Height), 1,/*cost*/ 16, effectThumnail[i]);
              
            }




            //GREAT-BAD-MISS-GOOD모든 이펙트가 다음과 같이 따라감
            //이펙트 특성 -start
            
            
            effectInitFrams[0] = new Rectangle(0, 0, 166, 162);//고쳐야 함 //기본
                        
            effectInitFrams[1] = new Rectangle(0, 0, 166, 162);//우주 
            
            effectInitFrams[2] = new Rectangle(0, 0, 166, 162);

            effectInitFrams[3] = new Rectangle(0, 0, 166, 162);

            effectFrameCount[0] = 9;//기본
            effectFrameCount[1] = 9;//우주 
            effectFrameCount[2] = 9;//숲
            effectFrameCount[3] = 9;//숲
            
           
            effecScale[0] = 1.0f;//기본
            effecScale[1] = 1.0f;//우주 
            effecScale[2] = 1.0f;//숲
            effecScale[3] = 1.0f;//숲


            //지속시간
            effectDulation[0] =  45;
            effectDulation[1] =  45;//우주 
            effectDulation[2] =  45;//숲
            effectDulation[3] = 45;//숲
           //이펙트 특성 -end



            //마커 특성 -
            markersScale[0] = 1.0f;

            //노트 특성
            
            rightNoteScale[0] = 1f;

            //test

            //구입한 아이템으로 설정
            //buyItem(myRightHandItem, rightHandItem[0]);
            //buyItem(myLeftHandItem, leftHandItem[0]);
            //buyItem(myEffectItem, effectItem[0]);
            //buyItem(myNoteItem, noteItem[0]);
            //buyItem(myBackgroundItem, backgroundItem[0]);
        }

        public void SaveFileItem()
        {
            //자기 아이템이 전체 아이템의 몇번 인덱스 인지 파악한후에 저장
            int i;
            int index;
           
            String dir = System.Environment.CurrentDirectory + "\\beethovenRecord\\itemManager.txt";
            if (!System.IO.File.Exists(dir))
            {
                var myFile = System.IO.File.Create(dir);
                myFile.Close();
            }

            TextWriter tw = new StreamWriter(dir);
            tw.WriteLine("**");
            tw.WriteLine("rightHand");
            
            for(i=0; i<myRightHandItem.Count; i++)
            {
                index = getIndexOfAllRightItem(myRightHandItem[i]);
                tw.WriteLine(index.ToString());
            }
            tw.WriteLine("$$");
            tw.WriteLine(getRightHandIndex());


         //   tw.WriteLine("**");
            tw.WriteLine("leftHand");
            for (i = 0; i < myLeftHandItem.Count; i++)
            {
                index = getIndexOfAllLeftItem(myLeftHandItem[i]);
                tw.WriteLine(index.ToString());
            }
            tw.WriteLine("$$");
            tw.WriteLine(getLeftHandIndex());

        //    tw.WriteLine("**");
            tw.WriteLine("effect");
            for (i = 0; i < myEffectItem.Count; i++)
            {
                index = getIndexOfAllEffectItem(myEffectItem[i]);
                tw.WriteLine(index.ToString());
            }
            tw.WriteLine("$$");
            tw.WriteLine(getEffectIndex());

        //    tw.WriteLine("**");
            tw.WriteLine("note");
            for (i = 0; i < myNoteItem.Count; i++)
            {
                index = getIndexOfAllNoteItem(myNoteItem[i]);
                tw.WriteLine(index.ToString());
            }
            tw.WriteLine("$$");
            tw.WriteLine(getNoteIndex());
        //    tw.WriteLine("**");
            tw.WriteLine("background");
            for (i = 0; i < myBackgroundItem.Count; i++)
            {
                index = getIndexOfAllBackgroundItem(myBackgroundItem[i]);
                tw.WriteLine(index.ToString());
            }
            tw.WriteLine("$$");
            tw.WriteLine(getBackgroundIndex());
         //   tw.WriteLine("**");
            tw.WriteLine("!!");
            tw.Close();
        }


        public void LoadFileItem()
        {
          
            

            String dir = System.Environment.CurrentDirectory + "\\beethovenRecord\\itemManager.txt";
            if (!System.IO.File.Exists(dir))
            {
                var myFile = System.IO.File.Create(dir);
                myFile.Close();
                buyItem(myRightHandItem, rightHandItem[0]);
                buyItem(myLeftHandItem, leftHandItem[0]);
                buyItem(myEffectItem, effectItem[0]);
                buyItem(myNoteItem, rightNoteItem[0]);
                buyItem(myBackgroundItem, backgroundItem[0]);
                setRightHandIndex(0);
                setLeftHandIndex(0);
                setEffectIndex(0);
                setNoteIndex(0);
                setBackgroundIndex(0);
                
                //변화가 있으니깐 저장
                SaveFileItem();


            }
            else
            {
                StreamReader sr = new StreamReader(dir);
                String line;

                line = sr.ReadLine();

                if (line != null)
                {
                    while (line != "!!")
                    {
                        if (line == "rightHand")
                        {

                            line = sr.ReadLine();
                            while (line != "$$")
                            {

                                buyItem(myRightHandItem, rightHandItem[Int32.Parse(line)]);
                                line = sr.ReadLine();

                            }
                            line = sr.ReadLine();
                            setRightHandIndex(Int32.Parse(line));

                        }

                        if (line == "leftHand")
                        {

                            line = sr.ReadLine();
                            while (line != "$$")
                            {

                                buyItem(myLeftHandItem, leftHandItem[Int32.Parse(line)]);
                                line = sr.ReadLine();

                            }
                            line = sr.ReadLine();
                            setLeftHandIndex(Int32.Parse(line));
                        }


                        if (line == "effect")
                        {

                            line = sr.ReadLine();
                            while (line != "$$")
                            {

                                buyItem(myEffectItem, effectItem[Int32.Parse(line)]);

                                line = sr.ReadLine();

                            }
                            line = sr.ReadLine();
                            setEffectIndex(Int32.Parse(line));
                        }


                        if (line == "note")
                        {

                            line = sr.ReadLine();
                            while (line != "$$")
                            {

                                buyItem(myNoteItem, rightNoteItem[Int32.Parse(line)]);
                                line = sr.ReadLine();

                            }
                            line = sr.ReadLine();
                            setNoteIndex(Int32.Parse(line));
                        }


                        if (line == "background")
                        {

                            line = sr.ReadLine();
                            while (line != "$$")
                            {

                                buyItem(myBackgroundItem, backgroundItem[Int32.Parse(line)]);
                                line = sr.ReadLine();

                            }
                            line = sr.ReadLine();
                            setBackgroundIndex(Int32.Parse(line));
                        }



                        line = sr.ReadLine();
                    }
                    sr.Close();
                }
            }

           

        }






        //각 아이템 텍스쳐 저장
        public void LoadContent(ContentManager cm)
        {


            //오른손 아이템
            //주의! 가득찬걸로 해야 함.


            rightHandTexture[0] = cm.Load<Texture2D>(@"rightItem\Baton_1");
            rightHandTexture[1] = cm.Load<Texture2D>(@"rightItem\Baton_2");
            rightHandTexture[2] = cm.Load<Texture2D>(@"rightItem\Baton_3");
            rightHandTexture[3] = cm.Load<Texture2D>(@"rightItem\Baton_4");
            rightHandTexture[4] = cm.Load<Texture2D>(@"rightItem\Baton_7");
            rightHandTexture[5] = cm.Load<Texture2D>(@"rightItem\Baton_6");



            //왼손
            //가운데를 중점으로 맞추어야 함.
            leftHandTexture[0] = cm.Load<Texture2D>(@"LeftHand\Left_1");
            leftHandTexture[1] = cm.Load<Texture2D>(@"LeftHand\Left_2");
            leftHandTexture[2] = cm.Load<Texture2D>(@"LeftHand\Left_3");
            leftHandTexture[3] = cm.Load<Texture2D>(@"LeftHand\Left_4");



            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //have to additem, effect, node , background
            
            //이펙트 섬네일

            //기본
            effectThumnail[0] = cm.Load<Texture2D>(@"Explosion\starThumnail");
            
            //우주느낌
            effectThumnail[1] = cm.Load<Texture2D>(@"Explosion\starThumnail");
            
            //초원느낌
            effectThumnail[2] = cm.Load<Texture2D>(@"Explosion\treeThumnail");

            //고스톱느낌
            effectThumnail[3] = cm.Load<Texture2D>(@"Explosion\sakuraThumnail");

            //퍼펙트 이펙트 

            effectTexture[0] = cm.Load<Texture2D>(@"Explosion\starPerfectEffect");//기본
            effectTexture[1] = cm.Load<Texture2D>(@"Explosion\starPerfectEffect");//우주
            effectTexture[2] = cm.Load<Texture2D>(@"Explosion\Effect_Guilty_3");//숲
            effectTexture[3] = cm.Load<Texture2D>(@"Explosion\Effect_Sakura_1");//고스톱


            //good
            goodEffectTexture[0] = cm.Load<Texture2D>(@"Explosion\starGoodEffect");//기본
            goodEffectTexture[1] = cm.Load<Texture2D>(@"Explosion\starGoodEffect");//우주
            goodEffectTexture[2] = cm.Load<Texture2D>(@"Explosion\Effect_Guilty_3");//숲
            goodEffectTexture[3] = cm.Load<Texture2D>(@"Explosion\Effect_Sakura_2");//숲

            //롱노트 //드래그 ( 일단 똑같이)
            badEffectTexture[0] = cm.Load<Texture2D>(@"Explosion\starLongEffect");//기본
            badEffectTexture[1] = cm.Load<Texture2D>(@"Explosion\starLongEffect");//우주
            badEffectTexture[2] = cm.Load<Texture2D>(@"Explosion\colorPencil3");//숲
            badEffectTexture[3] = cm.Load<Texture2D>(@"Explosion\colorPencil2");//숲
           
            //missEffectTexture[0] = cm.Load<Texture2D>(@"Explosion\starLongEffect2");
            //missEffectTexture[1] = cm.Load<Texture2D>(@"Explosion\starLongEffect2");
            //missEffectTexture[2] = cm.Load<Texture2D>(@"Explosion\windExplosion2");



            rightNoteTexture[0] = cm.Load<Texture2D>(@"notes\starRightNote_yell");//기본
            rightNoteTexture[1] = cm.Load<Texture2D>(@"notes\starRightNote_yell");//우주
            rightNoteTexture[2] = cm.Load<Texture2D>(@"notes\leafRightNote");//숲
            rightNoteTexture[3] = cm.Load<Texture2D>(@"notes\Note_Gostop_1");//고스톱


            leftNoteTexture[0] = cm.Load<Texture2D>(@"notes\starLeftNote_moon");//기본
            leftNoteTexture[1] = cm.Load<Texture2D>(@"notes\starLeftNote_moon");//우주
            leftNoteTexture[2] = cm.Load<Texture2D>(@"notes\leafLeftNote");//숲
            leftNoteTexture[3] = cm.Load<Texture2D>(@"notes\Note_Gostop_2");//고스톱


            dragNoteTexture[0] = cm.Load<Texture2D>(@"notes\starDragNote");//기본
            dragNoteTexture[1] = cm.Load<Texture2D>(@"notes\starDragNote");//우주
            dragNoteTexture[2] = cm.Load<Texture2D>(@"notes\leafDragNote");//숲
            dragNoteTexture[3] = cm.Load<Texture2D>(@"notes\Note_Gostop_3");//고스톱

            


            longNoteTexture[0] = cm.Load<Texture2D>(@"notes\planetNote4");//기본
            longNoteTexture[1] = cm.Load<Texture2D>(@"notes\planetNote4");//우주
            longNoteTexture[2] = cm.Load<Texture2D>(@"notes\leafLeftNote");//숲
            longNoteTexture[3] = cm.Load<Texture2D>(@"notes\Note_Gostop_3");//고스톱



            //노트랑 한쌍이다.//기본
            markTexture[0] = cm.Load<Texture2D>(@"markers\whiteMarker");//기본
            markTexture[1] = cm.Load<Texture2D>(@"markers\whiteMarker");//우주
            markTexture[2] = cm.Load<Texture2D>(@"markers\whiteMarker");//숲
            markTexture[3] = cm.Load<Texture2D>(@"markers\whiteMarker");//고스톱


            backgroundTexture[0] = cm.Load<Texture2D>(@"background\ConcertHall_2");//기본
            backgroundTexture[1] = cm.Load<Texture2D>(@"background\uniBackground");//우주
            backgroundTexture[2] = cm.Load<Texture2D>(@"background\ConcertHall_2");//숲
            backgroundTexture[3] = cm.Load<Texture2D>(@"background\BackG_2");//기본
            //backgroundTexture[0] = cm.Load<Texture2D>(@"background\BackG_2");//기본
            //backgroundTexture[1] = cm.Load<Texture2D>(@"background\BackG_3");//우주
            //backgroundTexture[2] = cm.Load<Texture2D>(@"background\BackG_4");//숲

            //배경 섬네일

            //기본
            backgroundThumnail[0] = cm.Load<Texture2D>(@"background\Back_ssum_2");

            //우주느낌
            backgroundThumnail[1] = cm.Load<Texture2D>(@"background\Back_ssum_4");

            //초원느낌
            backgroundThumnail[2] = cm.Load<Texture2D>(@"background\Back_ssum_2");
            //고스톱
        
            backgroundThumnail[3] = cm.Load<Texture2D>(@"background\Back_ssum_5");


           
        }

        private void addItem(
          List<Item> itemList,
          Vector2 location,
          Texture2D texture,
          Rectangle initalFrame,
          int frameCount,
          int cost,
         Texture2D thumnail = null
            )
        {
            Item thisItem = new Item(
                texture,
                location,
                initalFrame,
                frameCount,
                cost,
                thumnail);
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

        public Texture2D[] GetRightNoteTexture()
        {
            return this.rightNoteTexture;
        }

        public Texture2D[] GetLeftNoteTexture()
        {
            return this.leftNoteTexture;
        }

        public Texture2D[] GetDragNoteTexture()
        {
            return this.dragNoteTexture;
        }

        public Texture2D[] GetLongNoteTexture()
        {
            return this.longNoteTexture;
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

        public List<Item> getShopRightNoteItem()
        {
            return this.rightNoteItem;
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
            for (i = 0; i < rightNoteItem.Count; i++)
            {
                if (item == rightNoteItem[i])
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
