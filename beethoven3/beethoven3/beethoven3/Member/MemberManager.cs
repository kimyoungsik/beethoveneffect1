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

        //스트로크 1단계
        private Texture2D violinMemberPlay1;
        private Texture2D fluteMemberPlay1;
        private Texture2D timpaniMemberPlay1;
        private Texture2D hornMemberPlay1;
        private Texture2D contrabaseMemberPlay1;
        private Texture2D celloMemberPlay1;

        //스트로크 2단계
        private Texture2D violinMemberPlay2;
        private Texture2D fluteMemberPlay2;
        private Texture2D timpaniMemberPlay2;
        private Texture2D hornMemberPlay2;
        private Texture2D contrabaseMemberPlay2;
        private Texture2D celloMemberPlay2;

        //스트로크 3단계
        private Texture2D violinMemberPlay3;
        private Texture2D fluteMemberPlay3;
        private Texture2D timpaniMemberPlay3;
        private Texture2D hornMemberPlay3;
        private Texture2D contrabaseMemberPlay3;
        private Texture2D celloMemberPlay3;
        /// <summary>
        //*** 스트로트 포함된 member
        /// </summary>
        private int violinMemberState;
        private int fluteMemberState;
        private int timpaniMemberState;
        private int hornMemberState;
        private int contrabaseMemberState;
        private int celloMemberState;

        private ScoreManager scoreManager;
        #endregion
        public MemberManager(ScoreManager scoreManager)
        {

            violinMemberState = 0;
            fluteMemberState = 0;
            timpaniMemberState = 0;
            hornMemberState = 0;
            contrabaseMemberState = 0;
            celloMemberState = 0;

            this.scoreManager = scoreManager;

        }
        public void LoadContent(ContentManager cm)
        {

            //일반
            violinMemberPlay = cm.Load<Texture2D>(@"character\violin");
            fluteMemberPlay = cm.Load<Texture2D>(@"character\flute");
            timpaniMemberPlay = cm.Load<Texture2D>(@"character\timpani");
            hornMemberPlay = cm.Load<Texture2D>(@"character\horn");//
            contrabaseMemberPlay = cm.Load<Texture2D>(@"character\contrabase");//
            celloMemberPlay = cm.Load<Texture2D>(@"character\cello");


            //스토로크 1
            violinMemberPlay1 = cm.Load<Texture2D>(@"character\violin1");
            fluteMemberPlay1 = cm.Load<Texture2D>(@"character\flute");
            timpaniMemberPlay1 = cm.Load<Texture2D>(@"character\timpani");
            hornMemberPlay1 = cm.Load<Texture2D>(@"character\horn1");
            contrabaseMemberPlay1 = cm.Load<Texture2D>(@"character\contrabase1");
            celloMemberPlay1 = cm.Load<Texture2D>(@"character\cello");

            //스트로크2
            violinMemberPlay2 = cm.Load<Texture2D>(@"character\violin2");
            fluteMemberPlay2 = cm.Load<Texture2D>(@"character\flute");
            timpaniMemberPlay2 = cm.Load<Texture2D>(@"character\timpani");
            hornMemberPlay2 = cm.Load<Texture2D>(@"character\horn2");
            contrabaseMemberPlay2 = cm.Load<Texture2D>(@"character\contrabase2");
            celloMemberPlay2 = cm.Load<Texture2D>(@"character\cello");


            //스트로크 3
            violinMemberPlay3 = cm.Load<Texture2D>(@"character\violin3");
            fluteMemberPlay3 = cm.Load<Texture2D>(@"character\flute");
            timpaniMemberPlay3 = cm.Load<Texture2D>(@"character\timpani");
            hornMemberPlay3 = cm.Load<Texture2D>(@"character\horn3");
            contrabaseMemberPlay3 = cm.Load<Texture2D>(@"character\contrabase3");
            celloMemberPlay3 = cm.Load<Texture2D>(@"character\cello");


            violinMemberMiss = cm.Load<Texture2D>(@"character\violin");
            fluteMemberMiss = cm.Load<Texture2D>(@"character\flute");
            timpaniMemberMiss = cm.Load<Texture2D>(@"character\timpani");
            hornMemberMiss = cm.Load<Texture2D>(@"character\horn");
            contrabaseMemberMiss = cm.Load<Texture2D>(@"character\contrabase");
            celloMemberMiss = cm.Load<Texture2D>(@"character\cello");
        }

        
        public void init() 
        {

            //NO STROKE
            //바이올린
            MakeMember(violinMemberPlay, new Rectangle(0, 0, 114, 158), new Vector2(170, 70),/*velocity*/ Vector2.Zero,/*speed*/ 0f,/*collisionRadius*/ 15,/*프레임*/ 14,/*memberNumber*/ 0,/*scale*/ 1.2f);

            

            //플룻 
            MakeMember(fluteMemberPlay, new Rectangle(0, 0, 253, 595), new Vector2(270, 20), Vector2.Zero, 0f, 15, 5, 1, 0.4f);
             //팀파니 
            MakeMember(timpaniMemberPlay, new Rectangle(0, 0, 196, 238), new Vector2(350, 10), Vector2.Zero, 0f, 15, 5, 2, 1f);
             // 호른
            MakeMember(hornMemberPlay, new Rectangle(0, 0, 280, 595), new Vector2(540, 30), Vector2.Zero, 0f, 15, 5, 3, 0.32f);
             //콘타라 베이스
            MakeMember(contrabaseMemberPlay, new Rectangle(0, 0, 172, 254), new Vector2(610, 20), Vector2.Zero, 0f, 15, 8, 4, 0.9f);
            //첼로  
            MakeMember(celloMemberPlay, new Rectangle(0, 0, 325, 595), new Vector2(730, 55), Vector2.Zero, 0f, 15, 5, 5, 0.40f);

            //STROKE1
            //바이올린
            MakeMember(violinMemberPlay1, new Rectangle(0, 0, 114, 158), new Vector2(170, 70),/*velocity*/ Vector2.Zero,/*speed*/ 0f,/*collisionRadius*/ 15,/*프레임*/ 14,/*memberNumber*/ 0,/*scale*/ 1.2f);
            //플룻 
            MakeMember(fluteMemberPlay1, new Rectangle(0, 0, 253, 595), new Vector2(270, 20), Vector2.Zero, 0f, 15, 5, 1, 0.4f);
            //팀파니 
            MakeMember(timpaniMemberPlay1, new Rectangle(0, 0, 196, 238), new Vector2(350, 10), Vector2.Zero, 0f, 15, 5, 2, 1f);
            // 호른
            MakeMember(hornMemberPlay1, new Rectangle(0, 0, 280, 595), new Vector2(540, 30), Vector2.Zero, 0f, 15, 5, 3, 0.32f);
            //콘타라 베이스
            MakeMember(contrabaseMemberPlay1, new Rectangle(0, 0, 172, 254), new Vector2(610, 20), Vector2.Zero, 0f, 15, 8, 4, 0.9f);
            //첼로  
            MakeMember(celloMemberPlay1, new Rectangle(0, 0, 325, 595), new Vector2(730, 55), Vector2.Zero, 0f, 15, 5, 5, 0.40f);


            //STROKE2
            //바이올린
            MakeMember(violinMemberPlay2, new Rectangle(0, 0, 114, 158), new Vector2(170, 70),/*velocity*/ Vector2.Zero,/*speed*/ 0f,/*collisionRadius*/ 15,/*프레임*/ 14,/*memberNumber*/ 0,/*scale*/ 1.2f);
            //플룻 
            MakeMember(fluteMemberPlay2, new Rectangle(0, 0, 253, 595), new Vector2(270, 20), Vector2.Zero, 0f, 15, 5, 1, 0.4f);
            //팀파니 
            MakeMember(timpaniMemberPlay2, new Rectangle(0, 0, 196, 238), new Vector2(350, 10), Vector2.Zero, 0f, 15, 5, 2, 1f);
            // 호른
            MakeMember(hornMemberPlay2, new Rectangle(0, 0, 280, 595), new Vector2(540, 30), Vector2.Zero, 0f, 15, 5, 3, 0.32f);
            //콘타라 베이스
            MakeMember(contrabaseMemberPlay2, new Rectangle(0, 0, 172, 254), new Vector2(610, 20), Vector2.Zero, 0f, 15, 8, 4, 0.9f);
            //첼로  
            MakeMember(celloMemberPlay2, new Rectangle(0, 0, 325, 595), new Vector2(730, 55), Vector2.Zero, 0f, 15, 5, 5, 0.40f);


            // STROKE3
            //바이올린
            MakeMember(violinMemberPlay3, new Rectangle(0, 0, 114, 158), new Vector2(170, 70),/*velocity*/ Vector2.Zero,/*speed*/ 0f,/*collisionRadius*/ 15,/*프레임*/ 14,/*memberNumber*/ 0,/*scale*/ 1.2f);
            //플룻 
            MakeMember(fluteMemberPlay3, new Rectangle(0, 0, 253, 595), new Vector2(270, 20), Vector2.Zero, 0f, 15, 5, 1, 0.4f);
            //팀파니 
            MakeMember(timpaniMemberPlay3, new Rectangle(0, 0, 196, 238), new Vector2(350, 10), Vector2.Zero, 0f, 15, 5, 2, 1f);
            // 호른
            MakeMember(hornMemberPlay3, new Rectangle(0, 0, 280, 595), new Vector2(540, 30), Vector2.Zero, 0f, 15, 5, 3, 0.32f);
            //콘타라 베이스
            MakeMember(contrabaseMemberPlay3, new Rectangle(0, 0, 172, 254), new Vector2(610, 20), Vector2.Zero, 0f, 15, 8, 4, 0.9f);
            //첼로  
            MakeMember(celloMemberPlay3, new Rectangle(0, 0, 325, 595), new Vector2(730, 55), Vector2.Zero, 0f, 15, 5, 5, 0.40f);



            //실수 했을 때
            //바이올린
            MakeMember(violinMemberMiss, new Rectangle(0, 0, 114, 158), new Vector2(170, 70),/*velocity*/ Vector2.Zero,/*speed*/ 0f,/*collisionRadius*/ 15,/*프레임*/ 1,/*memberNumber*/ 0,/*scale*/ 1.2f);
            //플룻 
            MakeMember(fluteMemberMiss, new Rectangle(0, 0, 253, 595), new Vector2(270, 20), Vector2.Zero, 0f, 15, 1, 1, 0.4f);
            //팀파니 
            MakeMember(timpaniMemberMiss, new Rectangle(0, 0, 196, 238), new Vector2(350, 10), Vector2.Zero, 0f, 15, 1, 2, 1f);
            // 호른
            MakeMember(hornMemberMiss, new Rectangle(0, 0, 280, 595), new Vector2(540, 30), Vector2.Zero, 0f, 15, 1, 3, 0.32f);
            //콘타라 베이스
            MakeMember(contrabaseMemberMiss, new Rectangle(0, 0, 172, 254), new Vector2(610, 20), Vector2.Zero, 0f, 1, 8, 4, 0.9f);
            //첼로  
            MakeMember(celloMemberMiss, new Rectangle(0, 0, 325, 595), new Vector2(730, 55), Vector2.Zero, 0f, 15, 1, 5, 0.40f);

         
            foreach (Sprite flute in fluteMembers)
            {
                flute.FrameTime = 0.15f;
            }
            
            foreach (Sprite cello in celloMembers)
            {
                cello.FrameTime = 0.15f;
            }

         
        }

        //멤버 생성
        //멤버 생성할 때 각 LIST를 차곡차곡 들어가게 된다. 즉 스트로크 1은 배열 2번째에들어감
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

        //콘트라베이스의 경우에는 0.1이정상

        //멤버들의 프레임 속도 변화
        public void SetMembersFrameTime(float speed)
        {
            foreach (Sprite violin in violinMembers)
            {
                violin.FrameTime = speed;
            }
            foreach (Sprite flute in fluteMembers)
            {
                flute.FrameTime = speed + 0.05f;
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
                cello.FrameTime = speed + 0.05f;
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

            
            if (scoreManager.Combo == 0 && scoreManager.ComboChanged)
            {
                for (int q = 0; q < 6; q++)
                {
                    SetMemberState(q, 0);
                }
                scoreManager.ComboChanged = false;
            }
            else if(((int)scoreManager.Combo == 10  || (int)scoreManager.Combo == 11 )&& scoreManager.ComboChanged)
            {
                for (int q = 0; q < 6; q++)
                {
                    SetMemberState(q, 1);
                }
                SetMembersFrameTime(0.07f);
                scoreManager.ComboChanged = false;
            }
            else if (((int)scoreManager.Combo == 30 || (int)scoreManager.Combo == 30)  && scoreManager.ComboChanged)
            {
                for (int q = 0; q < 6; q++)
                {
                    SetMemberState(q, 2);
                }
                //속도 빨라짐
                SetMembersFrameTime(0.04f);
                scoreManager.ComboChanged = false;


            }
            else if (((int)scoreManager.Combo == 50 || (int)scoreManager.Combo == 50 )&& scoreManager.ComboChanged)
            {
                for (int q = 0; q < 6; q++)
                {
                    SetMemberState(q, 3);
                }
                //속도 빨라짐
                SetMembersFrameTime(0.01f);
                scoreManager.ComboChanged = false;
            }

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

        //멤버의 상태를 바꾼다 , 노말 스트로크 등, 
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