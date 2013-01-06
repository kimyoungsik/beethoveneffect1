﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;
namespace beethoven3
{
    class CollisionManager
    {
        #region declarations

        private PerfectExplosionManager perfectManager;
        private GoodExplosionManager goodManager;
        private BadExplosionManager badManager;
        private ExplosionManager goldGetManager;

        private ScoreManager scoreManager;

        private MemberManager memberManager;

        private ItemManager itemManager;
        private float roundPoint = 15.0f;
        #endregion 

        #region constructor
        public CollisionManager(PerfectExplosionManager perfectManager, GoodExplosionManager goodManager, BadExplosionManager badManager, ExplosionManager goldGetManager, ScoreManager scoreManager, MemberManager memberManager, ItemManager itemManager)
        {
            this.perfectManager = perfectManager;
            this.goodManager = goodManager;
            this.badManager = badManager;
            this.goldGetManager = goldGetManager;

            this.scoreManager = scoreManager;


            this.memberManager = memberManager;
            this.itemManager = itemManager;
        }
        #endregion

        #region method

       /// <summary>
       /// 드래그노트 와 마우스 만. (이건 마크가 없다)
       /// </summary>
       /// <param name="mousePoint"></param>
        public void checkDragNote(Vector2 mousePoint)
        {
            for (int i = 0; i < DragNoteManager.DragNotes.Count(); i++ )
            {
                Sprite dragNote = DragNoteManager.DragNotes[i];
                int judgment = dragNote.JudgedNote(mousePoint);
                if (judgment == 2)
                {

                    perfectManager.AddExplosions(new Vector2(dragNote.Center.X - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Width / 2, dragNote.Center.Y - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Height / 2));
                    DragNoteManager.DragNotes.RemoveAt(i);

                    scoreManager.DragNoteScore = scoreManager.DragNoteScore + 1;
                    scoreManager.Combo = scoreManager.Combo + 1;
                }
                //good
                else if (judgment == 1)
                {

                    perfectManager.AddExplosions(new Vector2(dragNote.Center.X - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Width / 2, dragNote.Center.Y - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Height / 2));
                    DragNoteManager.DragNotes.RemoveAt(i);
                //    ScoreManager.otherScore += 1;
                    scoreManager.DragNoteScore = scoreManager.DragNoteScore + 1;
                    scoreManager.Combo = scoreManager.Combo + 1;
                }
                else
                {
                  
                }
            }
        }
        
        private void checkRightNoteToMarker(int number,Vector2 mousePoint)
        {
            //오른손 노트(움직이는것 반복)
            for (int x = 0; x < StartNoteManager.rightNoteManager.LittleNotes.Count;  x++)
            {
                //오른손 노트들 중 하나를 가져옴
                Sprite littleNote = StartNoteManager.rightNoteManager.LittleNotes[x];
               
                //이중에서 마커의 시작위치에서 출발한 노트만 가려낸다. 

                if (littleNote.StartNoteLoation == number)
                {
                    //0:bad 1:good 2:perfect

                    ///노트의 반지름으로 
                    //judgment = littleNote.JudgedNote(
                    //    mark.MarkSprite.Center,
                    //    mark.MarkSprite.CollisionRadius);

                    //마커의 반지름으로

                    //스파리이트 클래스에는 거리 판단하는것이 있음.
                    //이상한점. 0번 마커일지라도 모든 노트를 다 검사하고 있다. 
                    //0번 마커는 0번 스타토 노트에서 출발한 것만  검사하면 되지 않느가?
                    //0번 스타트 노트에서 출발한것은 알 수가 있는가?

                    //노트와 마커의 센터 위치 파악 하기 위한 것.
                 //   Trace.WriteLine("noteCenter:"+littleNote.Center+" markcndter:"+MarkManager.Marks[number].MarkSprite.Center);
                 //   Vector2 notecenter = littleNote.Center;
                 //   Vector2 markcenter = MarkManager.Marks[number].MarkSprite.Center;
                    int judgment = MarkManager.Marks[number].MarkSprite.JudgedNote(
                        littleNote.Center
                        );
                    //perfect
                    if (judgment == 2)
                    {

                        int mouseJudgment = MarkManager.Marks[number].MarkSprite.JudgedNote(mousePoint);
                        if (mouseJudgment != 0)
                        {

                            perfectManager.AddExplosions(new Vector2(littleNote.Center.X - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Width / 2, littleNote.Center.Y - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Height / 2));
                            StartNoteManager.rightNoteManager.LittleNotes.RemoveAt(x);

                            scoreManager.Perfect = scoreManager.Perfect + 1;
                            scoreManager.Combo = scoreManager.Combo + 1;
                            scoreManager.Gage = scoreManager.Gage + 10;
                        }

                    }

                    //good
                    else if (judgment == 1)
                    {
                        int mouseJudgment = MarkManager.Marks[number].MarkSprite.JudgedNote(mousePoint);
                        if (mouseJudgment != 0)
                        {

                            goodManager.AddExplosions(new Vector2(littleNote.Center.X - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Width / 2, littleNote.Center.Y - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Height / 2));

                            StartNoteManager.rightNoteManager.LittleNotes.RemoveAt(x);

                            scoreManager.Good = scoreManager.Good + 1;

                            scoreManager.Combo = scoreManager.Combo + 1;
                            scoreManager.Gage = scoreManager.Gage + 10;

                            memberManager.SetMemberState(1, 1);
                        }
                    }
                    else
                    {

                    }
                }
            }
        }
       


        private void checkLeftNoteToMarker(int number, Vector2 mousePoint)
        {
            int x;
            for (x = 0; x < StartNoteManager.leftNoteManager.LittleNotes.Count; x++)
            {
                Sprite littleNote = StartNoteManager.leftNoteManager.LittleNotes[x];
                if (littleNote.StartNoteLoation == number)
                {
                    //0:bad 1:good 2:perfect

                    ///노트의 반지름으로 
                    //judgment = littleNote.JudgedNote(
                    //    mark.MarkSprite.Center,
                    //    mark.MarkSprite.CollisionRadius);

                    //마커의 반지름으로
                    int judgment = MarkManager.Marks[number].MarkSprite.JudgedNote(
                        littleNote.Center
                        );
                    //perfect
                    if (judgment == 2)
                    {
                        int mouseJudgment = MarkManager.Marks[number].MarkSprite.JudgedNote(mousePoint);
                        if (mouseJudgment != 0)
                        {

                            perfectManager.AddExplosions(new Vector2(littleNote.Center.X - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Width / 2, littleNote.Center.Y - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Height / 2));
                            StartNoteManager.leftNoteManager.LittleNotes.RemoveAt(x);

                            scoreManager.Perfect = scoreManager.Perfect + 1;
                            scoreManager.Combo = scoreManager.Combo + 1;
                            scoreManager.Gage = scoreManager.Gage + 10;
                        }
                    }

                    //good
                    else if (judgment == 1)
                    {
                        int mouseJudgment = MarkManager.Marks[number].MarkSprite.JudgedNote(mousePoint);
                        if (mouseJudgment != 0)
                        {
                            goodManager.AddExplosions(new Vector2(littleNote.Center.X - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Width / 2, littleNote.Center.Y - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Height / 2));
                            StartNoteManager.leftNoteManager.LittleNotes.RemoveAt(x);

                            scoreManager.Good = scoreManager.Good + 1;
                            scoreManager.Combo = scoreManager.Combo + 1;
                            scoreManager.Gage = scoreManager.Gage + 10;
                        }
                    }
                    else
                    {
                    }
                }
            
            }
        }

        public void checkLongNoteToMarker(int number, Vector2 mousePoint)
        {

            for (int x = 0; x < StartNoteManager.longNoteManager.LittleNotes.Count; x++)
            {
                Sprite littleNote = StartNoteManager.longNoteManager.LittleNotes[x];
                if (littleNote.StartNoteLoation == number)
                {
                    //0:bad 1:good 2:perfect
                    ///노트의 반지름으로 
                    //judgment = littleNote.JudgedNote(
                    //    mark.MarkSprite.Center,
                    //    mark.MarkSprite.CollisionRadius);
                    //마커의 반지름으로
                    int judgment = MarkManager.Marks[number].MarkSprite.JudgedNote(
                        littleNote.Center
                        );
                    //perfect
                    if (judgment == 2)
                    {
                        int mouseJudgment = MarkManager.Marks[number].MarkSprite.JudgedNote(mousePoint);
                        if (mouseJudgment != 0)
                        {
                            perfectManager.AddExplosions(new Vector2(littleNote.Center.X - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Width / 2, littleNote.Center.Y - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Height / 2));
                            StartNoteManager.longNoteManager.LittleNotes.RemoveAt(x);

                            scoreManager.LongNoteScore = scoreManager.LongNoteScore + 1;
                            scoreManager.Combo = scoreManager.Combo + 1;
                        }
                    }
                    //good
                    else if (judgment == 1)
                    {
                        int mouseJudgment = MarkManager.Marks[number].MarkSprite.JudgedNote(mousePoint);
                        if (mouseJudgment != 0)
                        {
                            //롱노트 효과를 바꾸던지 아니면 하나의 효과만 나오게 하던지
                            perfectManager.AddExplosions(new Vector2(littleNote.Center.X - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Width / 2, littleNote.Center.Y - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Height / 2));
                            StartNoteManager.longNoteManager.LittleNotes.RemoveAt(x);

                            scoreManager.LongNoteScore = scoreManager.LongNoteScore + 1;
                            scoreManager.Combo = scoreManager.Combo + 1;
                        }
                    }
                    else
                    {

                    }
                }
            }
        }

        private void checkGold(Vector2 mousePoint)
        {
            int i;
            for (i = 0; i < GoldManager.Golds.Count; i++)
            { 
                Sprite gold = GoldManager.Golds[i];

                int judgment = gold.JudgedNote(mousePoint);

                if (judgment != 0)
                {
                    goldGetManager.AddExplosions(new Vector2(gold.Center.X - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Width / 2, gold.Center.Y - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Height / 2));
                    GoldManager.Golds.RemoveAt(i);
                  
                    scoreManager.Gold = scoreManager.Gold + 1;
                }
            }
        }
        public void CheckCollisions(int number,Vector2 mousePoint)
        {
            checkRightNoteToMarker(number, mousePoint);
            checkLeftNoteToMarker(number, mousePoint);
       
            checkLongNoteToMarker(number, mousePoint);
            checkGold(mousePoint);
        }


        /// <summary>
        /// 오른 손 노트 사각형 범위 들어가면 삭제 , 반복문 돌 필요가 없는지 다시 검토
        /// </summary>
        public void CheckRightNoteInCenterArea()
        {
            int i;
            for (i = 0; i < StartNoteManager.rightNoteManager.LittleNotes.Count; i++)
            {
                Sprite littleNote = StartNoteManager.rightNoteManager.LittleNotes[i];


                if (littleNote.IsBoxColliding(MarkManager.centerArea))
                {


                    badManager.AddExplosions(new Vector2(littleNote.Center.X - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Width / 2, littleNote.Center.Y - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Height / 2));

                    StartNoteManager.rightNoteManager.LittleNotes.RemoveAt(i);
                    scoreManager.Bad = scoreManager.Bad + 1;
                    if (scoreManager.Combo > scoreManager.Max)
                    {
                        scoreManager.Max = scoreManager.Combo;
                    }


                    scoreManager.Combo = 0;
                    scoreManager.Gage = scoreManager.Gage - 1;
                }

            }


        }

        public void CheckLeftNoteInCenterArea()
        {
            int i;
            for (i = 0; i < StartNoteManager.leftNoteManager.LittleNotes.Count; i++)
            {
                Sprite littleNote = StartNoteManager.leftNoteManager.LittleNotes[i];


                if (littleNote.IsBoxColliding(MarkManager.centerArea))
                {
                    badManager.AddExplosions(littleNote.Center);
                    StartNoteManager.leftNoteManager.LittleNotes.RemoveAt(i);
                    scoreManager.Bad = scoreManager.Bad + 1;
                    if (scoreManager.Combo > scoreManager.Max)
                    {
                        scoreManager.Max = scoreManager.Combo;
                    }
                    scoreManager.Combo = 0;

                }
            }
        }



        #endregion

    }
}
