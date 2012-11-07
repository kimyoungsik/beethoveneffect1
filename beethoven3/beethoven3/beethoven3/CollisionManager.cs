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
        #endregion

        #region constructor
        public CollisionManager()
        {
         
        }
        #endregion

        #region method
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
                       
                      StartNoteManager.rightNoteManager.LittleNotes.RemoveAt(x);
                    }

                    //good
                    else if (judgment == 1)
                    {
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

                    StartNoteManager.leftNoteManager.LittleNotes.RemoveAt(x);
                }

                //good
                else if (judgment == 1)
                {
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

                    StartNoteManager.doubleNoteManager.LittleNotes.RemoveAt(x);
                }

                //good
                else if (judgment == 1)
                {
                    StartNoteManager.doubleNoteManager.LittleNotes.RemoveAt(x);
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
