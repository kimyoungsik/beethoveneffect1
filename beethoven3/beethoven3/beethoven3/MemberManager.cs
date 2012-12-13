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
            firstMemberPlay = cm.Load<Texture2D>(@"members\member1Run");
            secondMemberPlay = cm.Load<Texture2D>(@"members\member2Run");
            thirdMemberPlay = cm.Load<Texture2D>(@"members\member3Run");
            forthMemberPlay = cm.Load<Texture2D>(@"members\member4Run");
            fifthMemberPlay = cm.Load<Texture2D>(@"members\member5Run");
            sixthMemberPlay = cm.Load<Texture2D>(@"members\member6Run");

            firstMemberMiss = cm.Load<Texture2D>(@"members\member1Die");
            secondMemberMiss = cm.Load<Texture2D>(@"members\member2Die");
            thirdMemberMiss = cm.Load<Texture2D>(@"members\member3Die");
            forthMemberMiss = cm.Load<Texture2D>(@"members\member4Die");
            fifthMemberMiss = cm.Load<Texture2D>(@"members\member5Die");
            sixthMemberMiss = cm.Load<Texture2D>(@"members\member6Die");
          
        }

        
        public void init()
        {
            //first Run
            MakeMember(firstMemberPlay, new Rectangle(0,0,47,47),new Vector2(100,100),Vector2.Zero,0f,15,8,0);
            //second Run
            MakeMember(secondMemberPlay, new Rectangle(0, 0, 47, 47), new Vector2(200, 100), Vector2.Zero, 0f, 15, 8, 1);
            //third Run
            MakeMember(thirdMemberPlay, new Rectangle(0, 0, 47, 47), new Vector2(300, 100), Vector2.Zero, 0f, 15, 8, 2);
            //forth Run
            MakeMember(forthMemberPlay, new Rectangle(0, 0, 47, 47), new Vector2(400, 100), Vector2.Zero, 0f, 15, 8, 3);
            //fifth Run
            MakeMember(fifthMemberPlay, new Rectangle(0, 0, 47, 47), new Vector2(500, 100), Vector2.Zero, 0f, 15, 8, 4);
            //sixth Run
            MakeMember(sixthMemberPlay, new Rectangle(0, 0, 47, 47), new Vector2(600, 100), Vector2.Zero, 0f, 15, 8, 5);

            //first Miss
            MakeMember(firstMemberMiss, new Rectangle(0, 0, 47, 47), new Vector2(100, 100), Vector2.Zero, 0f, 15, 8, 0);
            //second Miss
            MakeMember(secondMemberMiss, new Rectangle(0, 0, 47, 47), new Vector2(200, 100), Vector2.Zero, 0f, 15, 8, 1);
            //third Miss
            MakeMember(thirdMemberMiss, new Rectangle(0, 0, 47, 47), new Vector2(300, 100), Vector2.Zero, 0f, 15, 8, 2);
            //forth Miss
            MakeMember(forthMemberMiss, new Rectangle(0, 0, 47, 47), new Vector2(400, 100), Vector2.Zero, 0f, 15, 8, 3);
            //fifth Miss
            MakeMember(fifthMemberMiss, new Rectangle(0, 0, 47, 47), new Vector2(500, 100), Vector2.Zero, 0f, 15, 8, 4);
            //sixth Miss
            MakeMember(sixthMemberMiss, new Rectangle(0, 0, 47, 47), new Vector2(600, 100), Vector2.Zero, 0f, 15, 8, 5);
        }

        public void MakeMember(
          Texture2D texture, 
          Rectangle InitialFrame,
          Vector2 location,
          Vector2 velocity,
          float speed,
          int collisionRadius,
          int frameCount,
          int memberNumber
          )
        {
            Sprite thisMember = new Sprite(
                location,
                texture,
                InitialFrame,
                velocity);

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
            firstMembers[firstMemberState].Draw(spriteBatch);
            secondMembers[secondMemberState].Draw(spriteBatch);
            thirdMembers[thirdMemberState].Draw(spriteBatch);
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