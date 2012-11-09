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
        private int judgment;
        private ExplosionManager perfectManager;
        private ExplosionManager  goodManager;
        #endregion

        #region constructor
        public CollisionManager(ExplosionManager perfectManager, ExplosionManager goodManager)
        {
            this.perfectManager = perfectManager;
            this.goodManager = goodManager;
        }
        #endregion

        #region method

        public void checkDragNote(Vector2 mousePoint)
        {
            for (int i = 0; i < DragNoteManager.DragNotes.Count(); i++ )
            {
                Sprite dragNote = DragNoteManager.DragNotes[i];
                judgment = dragNote.JudgedNote(mousePoint, 15.0f);
                if (judgment == 2)
                {
                    
                    DragNoteManager.DragNotes.RemoveAt(i);
                }

                        //good
                else if (judgment == 1)
                {
                    
                    DragNoteManager.DragNotes.RemoveAt(i);
                }
                else
                {

                }
            }
        }

        private void checkRightNoteToMarker(int number)
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
                    judgment = MarkManager.Marks[number].MarkSprite.JudgedNote(
                        littleNote.Center,
                        littleNote.CollisionRadius);
                    //perfect
                    if(judgment == 2)
                    {

                      perfectManager.AddExplosion(littleNote.Center, Vector2.Zero);
                      StartNoteManager.rightNoteManager.LittleNotes.RemoveAt(x);
                      
                    }

                    //good
                    else if (judgment == 1)
                    {
                        goodManager.AddExplosion(littleNote.Center, Vector2.Zero);
                       
                        StartNoteManager.rightNoteManager.LittleNotes.RemoveAt(x);
                    }
                    else
                    {

                    }
            }
        }

        private void checkLeftNoteToMarker(int number)
        {

            for (int x = 0; x < StartNoteManager.leftNoteManager.LittleNotes.Count; x++)
            {
                Sprite littleNote = StartNoteManager.leftNoteManager.LittleNotes[x];

                //0:bad 1:good 2:perfect

                ///노트의 반지름으로 
                //judgment = littleNote.JudgedNote(
                //    mark.MarkSprite.Center,
                //    mark.MarkSprite.CollisionRadius);

                //마커의 반지름으로
                judgment = MarkManager.Marks[number].MarkSprite.JudgedNote(
                    littleNote.Center,
                    littleNote.CollisionRadius);
                //perfect
                if (judgment == 2)
                {
                    perfectManager.AddExplosion(littleNote.Center, Vector2.Zero);
                    StartNoteManager.leftNoteManager.LittleNotes.RemoveAt(x);
                }

                //good
                else if (judgment == 1)
                {
                    goodManager.AddExplosion(littleNote.Center, Vector2.Zero);
                     StartNoteManager.leftNoteManager.LittleNotes.RemoveAt(x);
                }
                else
                {

                }
            
            }
        }


        private void checkDoubleNoteToMarker(int number)
        {

            for (int x = 0; x < StartNoteManager.doubleNoteManager.LittleNotes.Count; x++)
            {
                Sprite littleNote = StartNoteManager.doubleNoteManager.LittleNotes[x];

                //0:bad 1:good 2:perfect

                ///노트의 반지름으로 
                //judgment = littleNote.JudgedNote(
                //    mark.MarkSprite.Center,
                //    mark.MarkSprite.CollisionRadius);

                //마커의 반지름으로
                judgment = MarkManager.Marks[number].MarkSprite.JudgedNote(
                    littleNote.Center,
                    littleNote.CollisionRadius);
                //perfect
                if (judgment == 2)
                {
                    perfectManager.AddExplosion(littleNote.Center, Vector2.Zero);
                    StartNoteManager.doubleNoteManager.LittleNotes.RemoveAt(x);
                }

                //good
                else if (judgment == 1)
                {
                    goodManager.AddExplosion(littleNote.Center, Vector2.Zero);
                    StartNoteManager.doubleNoteManager.LittleNotes.RemoveAt(x);
                }
                else
                {

                }

            }
        }

        public void checkLongNoteToMarker(int number)
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
                judgment = MarkManager.Marks[number].MarkSprite.JudgedNote(
                    littleNote.Center,
                    littleNote.CollisionRadius);
                //perfect
                if (judgment == 2)
                {
                    
                    StartNoteManager.longNoteManager.LittleNotes.RemoveAt(x);
                }

                //good
                else if (judgment == 1)
                {

                    StartNoteManager.longNoteManager.LittleNotes.RemoveAt(x);
                }
                else
                {

                }
            }
        }


        public void CheckCollisions(int number)
        {
            checkRightNoteToMarker(number);
            checkLeftNoteToMarker(number);
            checkDoubleNoteToMarker(number);
        }

        #endregion

    }
}
