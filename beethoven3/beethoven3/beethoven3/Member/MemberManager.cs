﻿using System;
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

        private List<Sprite> violinMembers = new List<Sprite>();
        private List<Sprite> fluteMembers = new List<Sprite>();
        private List<Sprite> timpaniMembers = new List<Sprite>();
        private List<Sprite> hornMembers = new List<Sprite>();
        private List<Sprite> contrabaseMembers = new List<Sprite>();
        private List<Sprite> celloMembers = new List<Sprite>();

        private Texture2D violinMemberPlay;
        private Texture2D fluteMemberPlay;
        private Texture2D timpaniMemberPlay;
        private Texture2D hornMemberPlay;
        private Texture2D contrabaseMemberPlay;
        private Texture2D celloMemberPlay;

        private Texture2D violinMemberMiss;
        private Texture2D fluteMemberMiss;
        private Texture2D timpaniMemberMiss;
        private Texture2D hornMemberMiss;
        private Texture2D contrabaseMemberMiss;
        private Texture2D celloMemberMiss;
        
        /// <summary>
        //*** 스트로트 포함된 member
        /// </summary>
        private int violinMemberState;
        private int fluteMemberState;
        private int timpaniMemberState;
        private int hornMemberState;
        private int contrabaseMemberState;
        private int celloMemberState;

        #endregion
        public MemberManager()
        {

            violinMemberState = 0;
            fluteMemberState = 0;
            timpaniMemberState = 0;
            hornMemberState = 0;
            contrabaseMemberState = 0;
            celloMemberState = 0; 

        }
        public void LoadContent(ContentManager cm)
        {
            violinMemberPlay = cm.Load<Texture2D>(@"character\violin");
            fluteMemberPlay = cm.Load<Texture2D>(@"character\flute");
            timpaniMemberPlay = cm.Load<Texture2D>(@"character\timpani");
            hornMemberPlay = cm.Load<Texture2D>(@"character\horn");
            contrabaseMemberPlay = cm.Load<Texture2D>(@"character\contrabase");
            celloMemberPlay = cm.Load<Texture2D>(@"character\cello");

            violinMemberMiss = cm.Load<Texture2D>(@"character\violin");
            fluteMemberMiss = cm.Load<Texture2D>(@"character\flute");
            timpaniMemberMiss = cm.Load<Texture2D>(@"character\timpani");
            hornMemberMiss = cm.Load<Texture2D>(@"character\horn");
            contrabaseMemberMiss = cm.Load<Texture2D>(@"character\contrabase");
            celloMemberMiss = cm.Load<Texture2D>(@"character\cello");
        }

        
        public void init() 
        {

            
            //바이올린
            MakeMember(violinMemberPlay, new Rectangle(0, 0, 114, 158), new Vector2(280, 170),/*velocity*/ Vector2.Zero,/*speed*/ 0f,/*collisionRadius*/ 15,/*프레임*/ 15,/*memberNumber*/ 0,/*scale*/ 1.2f);
             //플룻 
            MakeMember(fluteMemberPlay, new Rectangle(0, 0, 253, 595), new Vector2(230, -120), Vector2.Zero, 0f, 15, 5, 1, 0.4f);
             //팀파니 
            MakeMember(timpaniMemberPlay, new Rectangle(0, 0, 196, 238), new Vector2(370, 0), Vector2.Zero, 0f, 15, 5, 2, 1f);
             // 호른
            MakeMember(hornMemberPlay, new Rectangle(0, 0, 280, 595), new Vector2(450, -180), Vector2.Zero, 0f, 15, 5, 3, 0.32f);
             //콘타라 베이스
            MakeMember(contrabaseMemberPlay, new Rectangle(0, 0, 172, 254), new Vector2(570, 45), Vector2.Zero, 0f, 15, 8, 4, 1f);
            //첼로 
            MakeMember(celloMemberPlay, new Rectangle(0, 0, 325, 595), new Vector2(580, -40), Vector2.Zero, 0f, 15, 5, 5, 0.42f);


            //실수했을때
            //바이올린
            MakeMember(violinMemberMiss, new Rectangle(0, 0, 114, 158), new Vector2(280, 170),/*velocity*/ Vector2.Zero,/*speed*/ 0f,/*collisionRadius*/ 15,/*프레임*/ 15,/*memberNumber*/ 0,/*scale*/ 1.2f);
            //플룻 
            MakeMember(fluteMemberMiss, new Rectangle(0, 0, 253, 595), new Vector2(230, -120), Vector2.Zero, 0f, 15, 5, 1, 0.4f);
            //팀파니 
            MakeMember(timpaniMemberMiss, new Rectangle(0, 0, 196, 238), new Vector2(370, 0), Vector2.Zero, 0f, 15, 5, 2, 1f);
            // 호른
            MakeMember(hornMemberMiss, new Rectangle(0, 0, 280, 595), new Vector2(450, -180), Vector2.Zero, 0f, 15, 5, 3, 0.32f);
            //콘타라 베이스
            MakeMember(contrabaseMemberMiss, new Rectangle(0, 0, 172, 254), new Vector2(570, 45), Vector2.Zero, 0f, 15, 8, 4, 1f);
            //첼로 
            MakeMember(celloMemberMiss, new Rectangle(0, 0, 325, 595), new Vector2(580, -40), Vector2.Zero, 0f, 15, 5, 5, 0.42f);


           
       
        }

        //멤버 생성
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
                velocity, 
                scale);

            thisMember.Velocity *= speed;

           

            // 처음부터 마지막 프레임까지 연주
            for (int x = 0; x < frameCount; x++)
            {
                thisMember.AddFrame(new Rectangle(
                    InitialFrame.X + (InitialFrame.Width * x),
                    InitialFrame.Y,
                    InitialFrame.Width,
                    InitialFrame.Height));
            }

            //마지막 프레임을 빼고 그 앞에서부터 처음까지 다시 연주
            for (int x = frameCount - 2; x >= 0; x--)
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
                    violinMembers.Add(thisMember);
                    break;

                case 1:
                    fluteMembers.Add(thisMember);
                    break;

                case 2:
                    timpaniMembers.Add(thisMember);
                    break;

                case 3:
                    hornMembers.Add(thisMember);
                    break;

                case 4:
                    contrabaseMembers.Add(thisMember);
                    break;

                case 5:
                    celloMembers.Add(thisMember);
                    break;

            }
        }

        //멤버들의 프레임 속도 변화
        public void SetMembersFrameTime(float speed)
        {
            foreach (Sprite violin in violinMembers)
            {
                violin.FrameTime = speed;
            }
            foreach (Sprite flute in fluteMembers)
            {
                flute.FrameTime = speed;
            }
            foreach (Sprite timpani in timpaniMembers)
            {
                timpani.FrameTime = speed;
            }
            foreach (Sprite horn in hornMembers)
            {
                horn.FrameTime = speed;
            }
            foreach (Sprite contrabase in contrabaseMembers)
            {
                contrabase.FrameTime = speed;
            }
            foreach (Sprite cello in celloMembers)
            {
                cello.FrameTime = speed;
            }
        }

        #region update and draw
        
        public void Update(GameTime gameTime)
        {
            violinMembers[violinMemberState].Update(gameTime);
            fluteMembers[fluteMemberState].Update(gameTime);
            timpaniMembers[timpaniMemberState].Update(gameTime);
            hornMembers[hornMemberState].Update(gameTime);
            contrabaseMembers[contrabaseMemberState].Update(gameTime);
            celloMembers[celloMemberState].Update(gameTime);
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            timpaniMembers[violinMemberState].Draw(spriteBatch);
            fluteMembers[fluteMemberState].Draw(spriteBatch);
            violinMembers[timpaniMemberState].Draw(spriteBatch);
            hornMembers[hornMemberState].Draw(spriteBatch);
            contrabaseMembers[contrabaseMemberState].Draw(spriteBatch);
            celloMembers[celloMemberState].Draw(spriteBatch);
        }


        public void SetMemberState(int member, int state)
        {                 
            switch (member)
            {
                case 0:
                    violinMemberState = state;
                    break;
                    
                case 1:
                    fluteMemberState = state;
                    break;

                case 2:
                    timpaniMemberState = state;
                    break;

                case 3:
                    hornMemberState = state;
                    break;
                    
                case 4:
                    contrabaseMemberState = state;
                    break;

                case 5:
                    celloMemberState = state; 
                    break;   
            }

        }
        #endregion
    }
}