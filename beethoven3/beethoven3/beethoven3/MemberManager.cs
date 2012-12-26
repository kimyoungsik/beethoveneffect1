using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
namespace beethoven3
{
    class MemberManager
    {

        #region declarations
        private List<Sprite> firstMembers = new List<Sprite>();
        private List<Sprite> secondMembers = new List<Sprite>();
        private List<Sprite> thirdMembers = new List<Sprite>();
        private List<Sprite> forthMembers = new List<Sprite>();
        private List<Sprite> fifthMembers = new List<Sprite>();
        private List<Sprite> sixthMembers = new List<Sprite>();
        
        private Texture2D firstMemberPlay;
        private Texture2D secondMemberPlay;
        private Texture2D thirdMemberPlay;
        private Texture2D forthMemberPlay;
        private Texture2D fifthMemberPlay;
        private Texture2D sixthMemberPlay;
        
        private Texture2D firstMemberMiss;
        private Texture2D secondMemberMiss;
        private Texture2D thirdMemberMiss;
        private Texture2D forthMemberMiss;
        private Texture2D fifthMemberMiss;
        private Texture2D sixthMemberMiss;
        
        private int firstMemberState;
        private int secondMemberState;
        private int thirdMemberState;
        private int forthMemberState;
        private int fifthMemberState;
        private int sixthMemberState;

        #endregion
        public MemberManager()
        {
              
         firstMemberState = 0;
         secondMemberState = 0;
         thirdMemberState = 0;
         forthMemberState = 0;
         fifthMemberState = 0;
         sixthMemberState = 0; 

        }
        public void LoadContent(ContentManager cm)
        {
            firstMemberPlay = cm.Load<Texture2D>(@"character\violin");
            secondMemberPlay = cm.Load<Texture2D>(@"character\flute");
            thirdMemberPlay = cm.Load<Texture2D>(@"character\timpani");
            forthMemberPlay = cm.Load<Texture2D>(@"character\horn");
            fifthMemberPlay = cm.Load<Texture2D>(@"character\contrabase");
            sixthMemberPlay = cm.Load<Texture2D>(@"character\cello");

            firstMemberMiss = cm.Load<Texture2D>(@"character\violin");
            secondMemberMiss = cm.Load<Texture2D>(@"character\flute");
            thirdMemberMiss = cm.Load<Texture2D>(@"character\timpani");
            forthMemberMiss = cm.Load<Texture2D>(@"character\horn");
            fifthMemberMiss = cm.Load<Texture2D>(@"character\contrabase");
            sixthMemberMiss = cm.Load<Texture2D>(@"character\cello");
          
        }

        
        public void init() 
        {

            ///프레임카운트 일단 1
            //first Run
            MakeMember(firstMemberPlay, new Rectangle(0,0,490,800),new Vector2(20,-150),Vector2.Zero,0f,15,1,0,0.3f);
            //second Run
            MakeMember(secondMemberPlay, new Rectangle(0, 0, 258, 595), new Vector2(170, -110), Vector2.Zero, 0f, 15,1, 1, 0.4f);
            //third Run
            MakeMember(thirdMemberPlay, new Rectangle(0, 0, 471, 595), new Vector2(170, -170), Vector2.Zero, 0f, 15, 1, 2, 0.4f);
            //forth Run
            MakeMember(forthMemberPlay, new Rectangle(0, 0, 312, 800), new Vector2(370, -290), Vector2.Zero, 0f, 15, 1, 3, 0.28f);
            //fifth Run
            MakeMember(fifthMemberPlay, new Rectangle(0, 0, 372, 800), new Vector2(420, -220), Vector2.Zero, 0f, 15, 1, 4, 0.3f);
            //sixth Run
            MakeMember(sixthMemberPlay, new Rectangle(0, 0, 320, 595), new Vector2(510, -40), Vector2.Zero, 0f, 15, 1, 5, 0.42f);


            //first Run
            MakeMember(firstMemberMiss, new Rectangle(0, 0, 490, 800), new Vector2(20, -150), Vector2.Zero, 0f, 15, 1, 0, 0.3f);
            //second Run
            MakeMember(secondMemberMiss, new Rectangle(0, 0, 258, 595), new Vector2(170, -110), Vector2.Zero, 0f, 15, 1, 1, 0.4f);
            //third Run
            MakeMember(thirdMemberMiss, new Rectangle(0, 0, 471, 595), new Vector2(170, -170), Vector2.Zero, 0f, 15, 1, 2, 0.4f);
            //forth Run
            MakeMember(forthMemberMiss, new Rectangle(0, 0, 312, 800), new Vector2(370, -290), Vector2.Zero, 0f, 15, 1, 3, 0.28f);
            //fifth Run
            MakeMember(fifthMemberMiss, new Rectangle(0, 0, 372, 800), new Vector2(420, -220), Vector2.Zero, 0f, 15, 1, 4, 0.3f);
            //sixth Run
            MakeMember(sixthMemberMiss, new Rectangle(0, 0, 320, 595), new Vector2(510, -40), Vector2.Zero, 0f, 15, 1, 5, 0.42f);

       
        }

        public void MakeMember(
          Texture2D texture, 
          Rectangle InitialFrame,
          Vector2 location,
          Vector2 velocity,
          float speed,
          int collisionRadius,
          int frameCount,
          int memberNumber,
            float scale
          )
        {
            Sprite thisMember = new Sprite(
                location,
                texture,
                InitialFrame,
                velocity, scale);

            thisMember.Velocity *= speed;

            for (int x = 1; x < frameCount; x++)
            {
                thisMember.AddFrame(new Rectangle(
                    InitialFrame.X + (InitialFrame.Width * x),
                    InitialFrame.Y,
                    InitialFrame.Width,
                    InitialFrame.Height));
            }

            thisMember.CollisionRadius = collisionRadius;

            switch (memberNumber)
            {
                case 0:
                    firstMembers.Add(thisMember);
                    break;

                case 1:
                    secondMembers.Add(thisMember);
                    break;

                case 2:
                    thirdMembers.Add(thisMember);
                    break;

                case 3:
                    forthMembers.Add(thisMember);
                    break;

                case 4:
                    fifthMembers.Add(thisMember);
                    break;

                case 5:
                    sixthMembers.Add(thisMember);
                    break;

            }
        }

        #region update and draw
        
        public void Update(GameTime gameTime)
        {
            firstMembers[firstMemberState].Update(gameTime);
            secondMembers[secondMemberState].Update(gameTime);
            thirdMembers[thirdMemberState].Update(gameTime);
            forthMembers[forthMemberState].Update(gameTime);
            fifthMembers[fifthMemberState].Update(gameTime);
            sixthMembers[sixthMemberState].Update(gameTime);
        }
        

        public void Draw(SpriteBatch spriteBatch)
        {
            
            
            thirdMembers[thirdMemberState].Draw(spriteBatch);
            secondMembers[secondMemberState].Draw(spriteBatch);
            firstMembers[firstMemberState].Draw(spriteBatch);
            forthMembers[forthMemberState].Draw(spriteBatch);
            fifthMembers[fifthMemberState].Draw(spriteBatch);
            sixthMembers[sixthMemberState].Draw(spriteBatch);
        }


        public void SetMemberState(int member, int state)
        {                 
            switch (member)
            {
                case 0:
                    firstMemberState = state;
                    break;
                    
                case 1:
                    secondMemberState = state;
                    break;

                case 2:
                    thirdMemberState = state;
                    break;

                case 3:
                    forthMemberState = state;
                    break;
                    
                case 4:
                    fifthMemberState = state;
                    break;

                case 5:
                    sixthMemberState = state; 
                    break;   
            }

        }
        #endregion
    }
}