using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace beethoven3
{
    class CollisionManager
    {
        #region declarations
        //MarkManager
       // private StartNoteManager startNoteManager;
         //   private Vector2 offScreen = new Vector2(-500, -500);
      //  private int judgment;
        private ExplosionManager perfectManager;
        private ExplosionManager  goodManager;
        private ExplosionManager badManager;
        private ExplosionManager goldGetManager;
        #endregion

        #region constructor
        public CollisionManager(ExplosionManager perfectManager, ExplosionManager goodManager, ExplosionManager badManager,ExplosionManager goldGetManager)
        {
            this.perfectManager = perfectManager;
            this.goodManager = goodManager;
            this.badManager = badManager;
            this.goldGetManager = goldGetManager;
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
                int judgment = dragNote.JudgedNote(mousePoint, 15.0f);
                if (judgment == 2)
                {
                    perfectManager.AddExplosion(dragNote.Center, Vector2.Zero);
                    DragNoteManager.DragNotes.RemoveAt(i);
                }

                //good
                else if (judgment == 1)
                {
                    perfectManager.AddExplosion(dragNote.Center, Vector2.Zero);
                    DragNoteManager.DragNotes.RemoveAt(i);
                }
                else
                {
                  
                }
            }
        }
        
        private void checkRightNoteToMarker(int number,Vector2 mousePoint)
        {
            for (int x = 0; x < StartNoteManager.rightNoteManager.LittleNotes.Count;  x++)
            {
                Sprite littleNote = StartNoteManager.rightNoteManager.LittleNotes[x];
          
                    //0:bad 1:good 2:perfect
                    
                    ///노트의 반지름으로 
                    //judgment = littleNote.JudgedNote(
                    //    mark.MarkSprite.Center,
                    //    mark.MarkSprite.CollisionRadius);
                    
                    //마커의 반지름으로
                    int judgment = MarkManager.Marks[number].MarkSprite.JudgedNote(
                        littleNote.Center,
                        littleNote.CollisionRadius);
                    //perfect
                    if(judgment == 2)
                    {
                        int mouseJudgment = MarkManager.Marks[number].MarkSprite.JudgedNote(mousePoint, 15.0f);
                        if (mouseJudgment != 0)
                        {
                            perfectManager.AddExplosion(littleNote.Center, Vector2.Zero);
                            StartNoteManager.rightNoteManager.LittleNotes.RemoveAt(x);
                        }
                
                    }

                    //good
                    else if (judgment == 1)
                    {
                        int mouseJudgment = MarkManager.Marks[number].MarkSprite.JudgedNote(mousePoint, 15.0f);
                        if (mouseJudgment != 0)
                        {
                            goodManager.AddExplosion(littleNote.Center, Vector2.Zero);

                            StartNoteManager.rightNoteManager.LittleNotes.RemoveAt(x);
                        }
                    }
                    else
                    {

                    }
            }
        }
       


        private void checkLeftNoteToMarker(int number, Vector2 mousePoint)
        {
            int x;
            for (x = 0; x < StartNoteManager.leftNoteManager.LittleNotes.Count; x++)
            {
                Sprite littleNote = StartNoteManager.leftNoteManager.LittleNotes[x];

                //0:bad 1:good 2:perfect

                ///노트의 반지름으로 
                //judgment = littleNote.JudgedNote(
                //    mark.MarkSprite.Center,
                //    mark.MarkSprite.CollisionRadius);

                //마커의 반지름으로
                int judgment = MarkManager.Marks[number].MarkSprite.JudgedNote(
                    littleNote.Center,
                    littleNote.CollisionRadius);
                //perfect
                if (judgment == 2)
                {
                    int mouseJudgment = MarkManager.Marks[number].MarkSprite.JudgedNote(mousePoint, 15.0f);
                    if (mouseJudgment != 0)
                    {
                        perfectManager.AddExplosion(littleNote.Center, Vector2.Zero);
                        StartNoteManager.leftNoteManager.LittleNotes.RemoveAt(x);
                    }
                }

                //good
                else if (judgment == 1)
                {
                    int mouseJudgment = MarkManager.Marks[number].MarkSprite.JudgedNote(mousePoint, 15.0f);
                    if (mouseJudgment != 0)
                    {
                        goodManager.AddExplosion(littleNote.Center, Vector2.Zero);
                        StartNoteManager.leftNoteManager.LittleNotes.RemoveAt(x);
                    }
                }
                else
                {
                }
            
            }
        }


        //private void checkDoubleNoteToMarker(int number)
        //{

        //    for (int x = 0; x < StartNoteManager.doubleNoteManager.LittleNotes.Count; x++)
        //    {
        //        Sprite littleNote = StartNoteManager.doubleNoteManager.LittleNotes[x];

        //        //0:bad 1:good 2:perfect

        //        ///노트의 반지름으로 
        //        //judgment = littleNote.JudgedNote(
        //        //    mark.MarkSprite.Center,
        //        //    mark.MarkSprite.CollisionRadius);

        //        //마커의 반지름으로
        //        int judgment = MarkManager.Marks[number].MarkSprite.JudgedNote(
        //            littleNote.Center,
        //            littleNote.CollisionRadius);
        //        //perfect
        //        if (judgment == 2)
        //        {
        //            perfectManager.AddExplosion(littleNote.Center, Vector2.Zero);
        //            StartNoteManager.doubleNoteManager.LittleNotes.RemoveAt(x);
        //        }

        //        //good
        //        else if (judgment == 1)
        //        {
        //            goodManager.AddExplosion(littleNote.Center, Vector2.Zero);
        //            StartNoteManager.doubleNoteManager.LittleNotes.RemoveAt(x);
        //        }
        //        else
        //        {

        //        }

        //    }
        //}

        public void checkLongNoteToMarker(int number, Vector2 mousePoint)
        {

            for (int x = 0; x < StartNoteManager.longNoteManager.LittleNotes.Count; x++)
            {
                Sprite littleNote = StartNoteManager.longNoteManager.LittleNotes[x];

                //0:bad 1:good 2:perfect

                ///노트의 반지름으로 
                //judgment = littleNote.JudgedNote(
                //    mark.MarkSprite.Center,
                //    mark.MarkSprite.CollisionRadius);

                //마커의 반지름으로
                int judgment = MarkManager.Marks[number].MarkSprite.JudgedNote(
                    littleNote.Center,
                    littleNote.CollisionRadius);
                //perfect
                if (judgment == 2)
                {
                     int mouseJudgment = MarkManager.Marks[number].MarkSprite.JudgedNote(mousePoint, 15.0f);
                     if (mouseJudgment != 0)
                     {
                         perfectManager.AddExplosion(littleNote.Center, Vector2.Zero);
                         StartNoteManager.longNoteManager.LittleNotes.RemoveAt(x);
                     }
                }

                //good
                else if (judgment == 1)
                {
                     int mouseJudgment = MarkManager.Marks[number].MarkSprite.JudgedNote(mousePoint, 15.0f);
                     if (mouseJudgment != 0)
                     {
                         //롱노트 효과를 바꾸던지 아니면 하나의 효과만 나오게 하던지
                         perfectManager.AddExplosion(littleNote.Center, Vector2.Zero);
                         StartNoteManager.longNoteManager.LittleNotes.RemoveAt(x);
                     }
                }
                else
                {

                }
            }
        }

        private void checkGold(Vector2 mousePoint)
        {
            int i;
            for (i = 0; i < GoldManager.Golds.Count; i++)
            {
                Sprite gold = GoldManager.Golds[i];

                int judgment = gold.JudgedNote(mousePoint, 15.0f);

                if (judgment != 0)
                {
                    goldGetManager.AddExplosion(gold.Center, Vector2.Zero);
                    GoldManager.Golds.RemoveAt(i);

                }



            }


        }
        public void CheckCollisions(int number,Vector2 mousePoint)
        {
            checkRightNoteToMarker(number, mousePoint);
            checkLeftNoteToMarker(number, mousePoint);
         //   checkDoubleNoteToMarker(number);
            checkLongNoteToMarker(number, mousePoint);
            checkGold(mousePoint);
        }

        #endregion

    }
}
