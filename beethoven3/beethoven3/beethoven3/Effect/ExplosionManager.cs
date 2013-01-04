using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace beethoven3
{
    class ExplosionManager
    {

        #region declarations
        
        private Texture2D texture;
        private Rectangle initialFrame;
        private float scale;
        private int frameCount;
        private int duration;
        public List<Explosion> Explosions = new List<Explosion>();
        
       
        #endregion
         

        #region constructor

        public void ExplosionInit(Texture2D texture, Rectangle initialFrame,int frameCount, float scale, int duration)
        {
            this.texture = texture;
            this.initialFrame = initialFrame;
            this.frameCount = frameCount;
            this.scale = scale;
            this.duration = duration;

        }
        public void AddExplosions(Vector2 location)
        {
            Explosion thisExplotion = new Explosion(
                texture,
                location,
                initialFrame,
                duration,
                frameCount,
                scale);
            Explosions.Add(thisExplotion);
        }
        public void deleteAllMarks()
        {
            
            for (int i = 0; i < Explosions.Count; i++)
            {
                Explosions.RemoveAt(i);
            }
        }



        #endregion
        
        #region properties
        public Rectangle InitialFrame
        {
            get { return initialFrame; }
            set { initialFrame = value; }
        }
        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public int FrameCount
        {
            get { return frameCount; }
            set { frameCount = value; }
        }

        #endregion

        #region method

        ////각 노트의 모양을 바꾸다.
        ////노래 선택하고 시작하기전에 설정한다.
        //public void changeRightNoteImage(Texture2D texture, Rectangle rect,float scale)
        //{
        //    rightNoteManager.TextureChange = texture;
        //    rightNoteManager.InitFrameChange = rect;
        //    rightNoteManager.ScaleChange = scale;
        //}



        //public void changeLeftNoteImage(Texture2D texture, Rectangle rect, float scale)
        //{
        //    leftNoteManager.TextureChange = texture;
        //    leftNoteManager.InitFrameChange = rect;
        //    leftNoteManager.ScaleChange = scale;
        //}


        //public void changeLongNoteImage(Texture2D texture, Rectangle rect, float scale)
        //{
        //    longNoteManager.TextureChange = texture;
        //    longNoteManager.InitFrameChange = rect;
        //    longNoteManager.ScaleChange = scale;
        //}

       



        //public void addStartNote(Vector2 location)
        //{
        //    StartNote thisStartNote = new StartNote(
        //        texture,
        //        location,
        //        initialFrame,
        //        frameCount);
        //    StartNotes.Add(thisStartNote);
        //}
        //public void deleteAllMarks()
        //{
        //    // for (int i = 0; i < 6; i++)
        //    for (int i = 0; i < StartNotes.Count; i++)
        //    {
        //        StartNotes.RemoveAt(i);
        //    }
        //}
        
        ////오른손 노트
        ////매개변수로 들어오는 markNumber가 (1베이스) 시작 노트의 위치이다. 

        ////반환 : 노트의 객체
        //public RightNoteInfo MakeRightNote(int markNumber)
        //{
            
        //    Vector2 location = StartNotes[markNumber-1].StartNoteSprite.Location;

        //    Vector2 direction =
        //                    MarkManager.Marks[markNumber-1].MarkSprite.Location -
        //                    location;
        //    //속도 1로 맞추기 
        //    direction.Normalize();

        //    RightNoteInfo rightNoteInfo = rightNoteManager.MakeNote(location, direction,/*시작 노트의 위치*/markNumber);

        //    return rightNoteInfo;

        //}

        ////왼손노트
        //public void MakeLeftNote(int markNumber)
        //{
        //    //노트시작점의 위치
        //    Vector2 location = StartNotes[markNumber-1].StartNoteSprite.Location;

        //    //노트시작점에서 마크의 방향
        //    Vector2 direction =
        //                    MarkManager.Marks[markNumber-1].MarkSprite.Location -
        //                    location;
        //    direction.Normalize();
        //    leftNoteManager.MakeNote(location, direction,/*시작 노트의 위치*/markNumber);
        //}

        ////public void MakeDoubleNote(int markNumber)
        ////{
        ////    //노트시작점의 위치
        ////    Vector2 location = StartNotes[markNumber].StartNoteSprite.Location;

        ////    //노트시작점에서 마크의 방향
        ////    Vector2 direction =
        ////                    MarkManager.Marks[markNumber].MarkSprite.Location -
        ////                    location;
        ////    direction.Normalize();
        ////    doubleNoteManager.MakeNote(location, direction);
        ////}

        //public void MakeLongNote(int markNumber)
        //{
        //    //노트시작점의 위치
        //    Vector2 location = StartNotes[markNumber-1].StartNoteSprite.Location;

        //    //노트시작점에서 마크의 방향
        //    Vector2 direction =
        //                    MarkManager.Marks[markNumber-1].MarkSprite.Location -
        //                    location;
        //    direction.Normalize();
        //    longNoteManager.MakeNote(location, direction,/*시작 노트의 위치*/markNumber);
        //}
        

        #endregion

        #region update and draw
        public void Update(GameTime gameTime)
        {
       
            for (int i = 0; i < Explosions.Count; i++)
            {
                Explosions[i].Update(gameTime);
              
            }
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
        

            for (int i = 0; i < Explosions.Count; i++)
            {
                Explosions[i].Draw(spriteBatch);
               
            }

        }


        #endregion

    }


    //    private Texture2D texture;
    //    //폭발 조각 텍스쳐 저장
    //    private List<Rectangle> pieceRectangles = new List<Rectangle>();
    //    private Rectangle pointRectangle;

    //    //조각
    //    private int minPieceCount = 9;
    //    private int maxPieceCount = 9;
        
    //    //포인트 스프라이트
    //    private int minPointCount = 20;
    //    private int maxPointCount = 30;

    //    private int durationCount = 20;
    //    private float explosionMaxSpeed = 1f;


    //    //조각과 포인트가 얼마나 빨리 퍼져나가는지 
    //    private float pieceSpeedScale = 6f;
    //    private int pointSpeedMin = 15;
    //    private int pointSpeedMax = 30;


    //    private Color initialColor = new Color(1.0f, 0.3f, 0f) * 0.5f;
    //    private Color finalColor = new Color(0f, 0f, 0f, 0f);

    //    private float scale;

    //    Random rand = new Random();

    //    //활성화 폭발에서 생성한 모든 입자 저장 리스트//입자 생명이 다하면 리스트에 서 제외
    //    private List<Sprite> ExplosionParticles = new List<Sprite>();

    //    public ExplosionManager(
    //        Texture2D texture,
    //        Rectangle initialFrame,
    //        int pieceCount,
    //        Rectangle pointRectangle,
    //        Color initialColor,
    //        Color finalColor,
    //        float scale)
    //    {
    //        this.initialColor = initialColor;
    //        this.finalColor = finalColor;
    //        this.texture = texture;
    //        this.scale = scale;
    //        for (int x = 0; x < pieceCount; x++)
    //        {
    //            pieceRectangles.Add(new Rectangle(
    //                initialFrame.X + (initialFrame.Width * x),
    //                initialFrame.Y,
    //                initialFrame.Width,
    //                initialFrame.Height));
    //        }
    //        this.pointRectangle = pointRectangle;
    //    }


    //    public Vector2 randomDirection(float scale)
    //    {
    //        Vector2 direction;
    //        do
    //        {
    //            direction = new Vector2(
    //            rand.Next(0, 101) - 50,
    //            rand.Next(0, 101) - 50);
    //        } while (direction.Length() == 0);
    //        direction.Normalize();
    //        direction *= scale;

    //        return direction;
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="location">중점백터</param>
    //    /// <param name="momentum"></param>
    //    public void AddExplosion(Vector2 location, Vector2 momentum)
    //    {
    //        //중점구하기 
    //        Vector2 pieceLocation = location -
    //            new Vector2(pieceRectangles[0].Width / 2,
    //                pieceRectangles[0].Height * scale / 2);
    //        /*scale 포함 */

            

    //        int pieces = rand.Next(minPieceCount, maxPieceCount + 1);


    //        for (int x = 0; x < pieceRectangles.Count; x++)
    //        {
    //            ExplosionParticles.Add(new Sprite(
    //                pieceLocation,
    //                texture,
    //                pieceRectangles[x],
    //                randomDirection(pieceSpeedScale),
    //                //piecespeedscale로 속도값을 조절해 폭발의 중심점에서 서서히 멀어지게함
    //                //모멤텀: 전체폭발 움직이게 함
                   
                   
    //                scale));
    //        }

    //        int points = rand.Next(minPointCount, maxPointCount + 1);
    //        for (int x = 0; x < points; x++)
    //        {
    //            ExplosionParticles.Add(new Particle(
                   
    //                location,
    //                texture,
    //                pointRectangle,
    //                randomDirection((float)rand.Next(
    //                    pointSpeedMin, pointSpeedMax)) + momentum,
    //                Vector2.Zero,
    //                explosionMaxSpeed,
    //                durationCount,
    //                initialColor,
    //                finalColor,
    //                scale));

    //        }
    //        //SoundManager.PlayExplosion();
    //    }

    //    public void Update(GameTime gameTime)
    //    {
    //        for (int x = ExplosionParticles.Count - 1; x >= 0; x--)
    //        {
    //         //   if (ExplosionParticles[x].IsActive)
    //          //  {
    //                ExplosionParticles[x].Update(gameTime);
    //          //  }
    //          //  else
    //           // {
    //          //      ExplosionParticles.RemoveAt(x);
    //          //  }
    //        }
    //    }

    //    public void Draw(SpriteBatch spriteBatch)
    //    {
    //        ExplosionParticles[0].Draw(spriteBatch);
    //    }
    //}
}
