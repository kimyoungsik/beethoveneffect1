using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
//using Microsoft.Kinect;
/*
 타입 0-오른손 1-왼손 2-양손 3-롱노트 4-드래그노트 
 
 */
namespace beethoven3
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SpriteFont pericles36Font;
         private ContentManager content;

        //스프라이즈 정렬 모드 테스트
      //  private SpriteSortMode sortMode = SpriteSortMode.Deferred;
      //  private BlendState blendMode = BlendState.AlphaBlend;
        
        //////////////////FPS, 성능
        //private float fps;
        //private float updateInterval = 1.0f;
        //private float timeSinceLastUpdate = 0.0f;
        //private float framecourt = 0;

        //상태바데모
       //  private ContentManager content;

        //private readonly Vector2 initializationVector = new Vector2(-99, -99);
        //private Vector2 currentPosition;
        //private Vector2 originalPosition;
        //private Vector2 position = new Vector2(300,300);
        ////텍스쳐의 배경 영역 (256-63 = 193)
        //private Rectangle progressBarBackground = new Rectangle(63, 0, 193, 32);
        ////텍스쳐 전경 영역
        //private Rectangle progressBarForeground = new Rectangle(0, 0, 63, 20);
        ////배경위에 전경을 그릴 위치
        //private Vector2 progressBarOffset = new Vector2(12, 6);
        //public float MoveRate = 90.0f;
        //private Texture2D progressBar;





        enum GameStates { Menu, Playing, SongMenu, GameOver };
        GameStates gameState = GameStates.Menu;
        

        public static Texture2D spriteSheet;
        public static Texture2D menu;
        public static Texture2D heart;
        public static Texture2D dot;

    //    private Texture2D tileSprite;
        private Texture2D uiBackground;
        private Texture2D uiHeart;



        private Rectangle removeAreaRec = new Rectangle(360, 130, 130, 230);
        SongMenu songMenu;
        int result;

        StartNoteManager startNoteManager;
        CollisionManager collisionManager;
        File file;
        ExplosionManager perfectManager;
        ExplosionManager goodManager;
        ExplosionManager badManager;
        ExplosionManager goldGetManager;
        MouseState mouseStateCurrent;

        ScoreManager scoreManager;
        static public int mousex = 100; //X좌표
        static public int mousey = 100; //Y좌표

        const int SCR_W = 1024;
        const int SCR_H = 768;

        //점수위치
        Vector2 scorePosition = new Vector2(705, 5);


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferHeight = SCR_H;
            graphics.PreferredBackBufferWidth = SCR_W;
            ///test
            ///
            content = new ContentManager(Services);
            
        }


            ////////////////FPS, 성능
        //    //Draw 메서드를 모니터의 수직회귀(vertical retrace) 속도에 동기화 하지 말것
        //    //fps를 60으로 제한 하지 않기
        //    graphics.SynchronizeWithVerticalRetrace = false;

        //    //update 메서드는 여전히 기본값인 1/60초의 속도로 호출할 것
        //    //현재 targetElapsedTime 이 1/60으로 되어 있고, 이것을 바꾸고 싶으면 바꿔도 됨.
        //    IsFixedTimeStep = true;
        //}

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.IsMouseVisible = true;
            //KINECT
    //DepthDisplayRectangle = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

            
            SoundManager.Init();
            LoadSound();
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //KINECT
        //nui = KinectSensor.KinectSensors[0];
        //nui.DepthStream.Enable();
        //nui.DepthFrameReady += new EventHandler<DepthImageFrameReadyEventArgs>(nui_DepthFrameReady);
        //nui.Start();
        //////
            ////test
            //progressBar = Content.Load<Texture2D>(@"Textures\progressbar");
           
            
            spriteSheet = Content.Load<Texture2D>(@"Textures\SpriteSheet8");
            menu = Content.Load<Texture2D>(@"Textures\menu");
            heart = Content.Load<Texture2D>(@"Textures\heart");
            dot = Content.Load<Texture2D>(@"Textures\dot");
           
            //test 텍스쳐 순서
            uiBackground = Content.Load<Texture2D>(@"ui\background");
            uiHeart = Content.Load<Texture2D>(@"ui\heart");
        //    tileSprite = Content.Load<Texture2D>(@"Textures\shapes");
            
            pericles36Font = Content.Load<SpriteFont>(@"Fonts\Pericles36");

            // TODO: use this.Content to load your game content here

          //  LineRenderer.LoadContent(content);
            
            Vector2 mark1Location = new Vector2(400, 70);
            Vector2 mark2Location = new Vector2(500, 170);
            Vector2 mark3Location = new Vector2(500, 270);
            Vector2 mark4Location = new Vector2(400, 370);
            Vector2 mark5Location = new Vector2(300, 270);
            Vector2 mark6Location = new Vector2(300, 170);
            scoreManager = new ScoreManager();
            
            startNoteManager = new StartNoteManager(
                spriteSheet,
                new Rectangle(0, 200, 52, 55),
                1);
           // ScoreManager.initialize();


            MarkManager.initialize(
           // markManager = new MarkManager(
                spriteSheet,
                new Rectangle(0, 200, 50, 55),
                1,
                mark1Location,
                mark2Location,
                mark3Location,
                mark4Location,
                mark5Location,
                mark6Location,
                startNoteManager,
                removeAreaRec
                
                );
            perfectManager = new ExplosionManager(
                 spriteSheet,
                 new Rectangle(0, 100, 50, 50),
                 3,
                 new Rectangle(0, 450, 2, 2),
                 new Color(1.0f, 0.3f, 0f) * 0.5f,
                 new Color(0f, 0f, 0f, 0f));

            goodManager = new ExplosionManager(
                 spriteSheet,
                 new Rectangle(0, 100, 50, 50),
                 3,
                 new Rectangle(0, 450, 2, 2),
                 new Color(0f, 0f, 1.0f) * 0.5f,
                 new Color(0f, 0f, 0f, 0f));

            badManager = new ExplosionManager(
                 spriteSheet,
                 new Rectangle(0, 100, 50, 50),
                 3,
                 new Rectangle(0, 450, 2, 2),
                 new Color(0f, 1.0f, 0f) * 0.5f,
                 new Color(0f, 0f, 0f, 0f));

            goldGetManager = new ExplosionManager(
              spriteSheet,
              new Rectangle(0, 100, 50, 50),
              3,
              new Rectangle(0, 450, 2, 2),
              new Color(1f, 0.5f, 0.5f) * 0.5f,
              new Color(0f, 0f, 0f, 0f));
            collisionManager = new CollisionManager(perfectManager, goodManager, badManager, goldGetManager, scoreManager);
            
            
            NoteFileManager noteFileManager = new NoteFileManager();
            file = new File(startNoteManager, noteFileManager, badManager, scoreManager);
            //곡선택화면에서
            //file.Loading("a.txt");
            String dir = "c:\\beethoven\\";
            file.FileLoading(dir, "*.txt");
            
            DragNoteManager.initialize(
                 spriteSheet,
                 new Rectangle(0, 100, 50, 50),
                 6,
                 15,
                 0,
                 badManager,
                 scoreManager);

            //골드로 변경해야 함
            GoldManager.initialize(
                spriteSheet,
                new Rectangle(0, 100, 20, 20),
                1,
                15,
                0);
            songMenu = new SongMenu(noteFileManager);
            songMenu.Load(Content,graphics.GraphicsDevice);
            
        }

     
        public void LoadSound()
        {
            int count = SoundManager.sndFiles.Count();
            for (int i = 0; i < count; i++)
            {
                SoundManager.soundEngine[i] = Content.Load<Song>(SoundManager.sndFiles[i]);
           //     SoundManager.soundEngineInstance[i] = SoundManager.soundEngine[i].CreateInstance();
            }
        }
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        private void HandleMouseInput(MouseState mouseState)
        {
             collisionManager.checkDragNote(new Vector2(mouseStateCurrent.X,mouseStateCurrent.Y));

                collisionManager.CheckCollisions(0, new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y));

                collisionManager.CheckCollisions(1, new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y));

                collisionManager.CheckCollisions(2, new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y));

                collisionManager.CheckCollisions(3, new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y));

                collisionManager.CheckCollisions(4, new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y));

                collisionManager.CheckCollisions(5, new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y));
         
        }


        private void HandleKeyboardInput(KeyboardState keyState)
        
        {
            if (keyState.IsKeyDown(Keys.NumPad1))
            {
             //   SoundManager.SndPlay("snd/ka");
             //   collisionManager.CheckCollisions(0);

         //       sortMode = SpriteSortMode.BackToFront;
            }
            if (keyState.IsKeyDown(Keys.NumPad2))
            {
             //   SoundManager.SndPlay("snd/maid");
              //  collisionManager.CheckCollisions(1);
         //       sortMode = SpriteSortMode.FrontToBack;
            }
            if (keyState.IsKeyDown(Keys.NumPad3))
            {
              //  SoundManager.SndPlay("snd/jo");
               // collisionManager.CheckCollisions(2);
          //      sortMode = SpriteSortMode.Deferred;
            }
            if (keyState.IsKeyDown(Keys.NumPad4))
            {
              //  SoundManager.SndStop();
              //  collisionManager.CheckCollisions(3);
         //       sortMode = SpriteSortMode.Immediate;
            }
            if (keyState.IsKeyDown(Keys.NumPad5))
            {
              //  collisionManager.CheckCollisions(4);
          //      sortMode = SpriteSortMode.Texture;
            }
            if (keyState.IsKeyDown(Keys.NumPad6))
            {
                //  collisionManager.CheckCollisions(4);
          //      blendMode = BlendState.AlphaBlend;
            }

            if (keyState.IsKeyDown(Keys.NumPad7))
            {
                //  collisionManager.CheckCollisions(4);
          //      blendMode = BlendState.Additive;
            }

            if (keyState.IsKeyDown(Keys.NumPad8))
            {
           //     //  collisionManager.CheckCollisions(4);
//blendMode = BlendState.NonPremultiplied;
            }

            if (keyState.IsKeyDown(Keys.NumPad9))
            {
                //  collisionManager.CheckCollisions(4);
           //     blendMode = BlendState.Opaque;
                scoreManager.Gage = scoreManager.Gage + 10;
            }
      
      
          
        }
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            mouseStateCurrent = Mouse.GetState();

           switch (gameState)
            {
                case GameStates.Menu:


                    if ((Keyboard.GetState().IsKeyDown(Keys.Space))                    )
                    {
                        gameState = GameStates.SongMenu;
                    }

                    break;
                case GameStates.Playing:
                      MarkManager.Update(gameTime);
                    startNoteManager.Update(gameTime);
                    HandleKeyboardInput(Keyboard.GetState());
                    HandleMouseInput(Mouse.GetState());

                    file.Update(spriteBatch, gameTime);
                    DragNoteManager.Update(gameTime);
                    GoldManager.Update(gameTime);
                    perfectManager.Update(gameTime);
                    goodManager.Update(gameTime);
                    badManager.Update(gameTime);
                    goldGetManager.Update(gameTime);
                    scoreManager.Update(gameTime);


                   //TEST

                    //if (currentPosition == initializationVector)//처음 호출
                    //{
                    //    currentPosition = originalPosition = position;
                    //}
                    //else
                    //{
                    //    currentPosition += new Vector2(MoveRate *(float)gameTime.ElapsedGameTime.TotalSeconds,0);

                    //    //영역의 마지막(또는 시작)에 도달했는지 검사
                    //    // 만약 그렇다면 방향을 뒤집는다.

                    //    if(currentPosition.X> originalPosition.X + (progressBarBackground.Width - progressBarForeground.Width - 15) || currentPosition.X < position.X)
                    //    {
                    //        MoveRate = -MoveRate;
                    //    }
                    //}
                break;

                case GameStates.SongMenu:
                    result = songMenu.Update();
                    if (result == -1)
                    {
                        gameState = GameStates.Menu;

                    }

                    else if(result != -2)
                    {
                        gameState = GameStates.Playing;
                        file.Loading(result);
                    }

                    break;  
            }
        
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            ////////////////FPS, 성능
            //float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            //framecourt++;

            ////새로운 프레임 속도를 표시할 만큼 충분한 시간이 경과했는지 검사(지금은 updateInterval 1초로 설정됨)
            
            //timeSinceLastUpdate += elapsed;
            //if (timeSinceLastUpdate > updateInterval)
            //{
            //    fps = framecourt / timeSinceLastUpdate; //실질적으로 fps/updateInterval (프레임카운트를 지난번 계산으로부터 경과한 시간으로 나눔)
            //    Window.Title = "FPS: " + fps.ToString() +
            //        " - 게임시간: " + gameTime.ElapsedGameTime.TotalSeconds.ToString();
            //    framecourt = 0;
            //    timeSinceLastUpdate -= updateInterval;
            //}


            GraphicsDevice.Clear(Color.White);
           // spriteBatch.Begin(sortMode,blendMode);
           
            // graphics.GraphicsDevice.
            spriteBatch.Begin();

            if (gameState == GameStates.Menu)
            {
                spriteBatch.Draw(menu,
                    new Rectangle(0, 0, this.Window.ClientBounds.Width,
                        this.Window.ClientBounds.Height),
                        Color.White);
            }

            if ((gameState == GameStates.Playing))
            {
                MarkManager.Draw(spriteBatch);
                startNoteManager.Draw(spriteBatch);
                CurveManager.Draw(gameTime, spriteBatch);
                GuideLineManager.Draw(gameTime, spriteBatch);
                //이걸 주석하면 드래그노트 체크하는거 안보임 하지만 체크는 됨
                //DragNoteManager.Draw(spriteBatch);
               
                
                GoldManager.Draw(spriteBatch);

                file.Draw(spriteBatch, gameTime);
                perfectManager.Draw(spriteBatch);
                goodManager.Draw(spriteBatch);
                badManager.Draw(spriteBatch);
                goldGetManager.Draw(spriteBatch);
                //가운데 빨간 사각형 주석하면 보이지않는다.
           //     spriteBatch.Draw(dot, removeAreaRec, Color.Red);
                int combo =  scoreManager.Combo;
                int max = scoreManager.Max;
                int total = scoreManager.TotalScore;
                spriteBatch.DrawString(pericles36Font, combo.ToString(), scorePosition, Color.Black);
                spriteBatch.DrawString(pericles36Font, max.ToString(), new Vector2(scorePosition.X + 120, scorePosition.Y), Color.Black);
                spriteBatch.DrawString(pericles36Font, total.ToString(), new Vector2(scorePosition.X + 240, scorePosition.Y), Color.Black);

                
               // int heartgage = heart.Width * gage ;

                ////test

                spriteBatch.Draw(uiBackground, new Vector2(0, 0), Color.White);
                int gage = scoreManager.Gage; 
                //0이하이거나 넘어가지 않게 
                
                spriteBatch.Draw(uiHeart, new Vector2(0, 6), new Rectangle(0, 0, gage, 50), Color.White);
                //spriteBatch.Draw(progressBar, originalPosition, progressBarBackground, Color.White);
                //spriteBatch.Draw(progressBar, currentPosition + progressBarOffset, progressBarForeground, Color.Blue);

                ////test
                ////하트
                //spriteBatch.Draw(tileSprite, new Rectangle(64, 64, 256, 256), new Rectangle(256, 256, 256, 256), Color.White, 0, Vector2.Zero, SpriteEffects.None,.10f);


                ////원
                //spriteBatch.Draw(tileSprite, new Rectangle(0, 0, 256, 256), new Rectangle(256, 0, 256, 256), Color.White, 0, Vector2.Zero, SpriteEffects.None, .15f);


                ////모양
                //spriteBatch.Draw(tileSprite, new Rectangle(128, 128, 256, 256), new Rectangle(0, 0, 256, 256), Color.White, 0, Vector2.Zero, SpriteEffects.None, .05f);


                ////별
                //spriteBatch.Draw(tileSprite, new Rectangle(192, 192, 256, 256), new Rectangle(0, 256, 256, 256), Color.White, 0, Vector2.Zero, SpriteEffects.None, .01f);

                //Window.Title = "정렬데모순서 - " +blendMode.ToString()+" : "+ sortMode.ToString();
            
            
            }

            if (gameState == GameStates.SongMenu)
            {
                songMenu.Draw(spriteBatch);

            }
            spriteBatch.End();
            
            base.Draw(gameTime);
        }

       
    }
}