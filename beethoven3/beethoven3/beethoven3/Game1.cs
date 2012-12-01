#define Kinect

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
using Microsoft.Kinect;
using Coding4Fun.Kinect.Wpf;
using System.IO;
using System.Threading;
using Microsoft.Speech.Recognition;
using Microsoft.Speech.AudioFormat;
 
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

#if Kinect
         //키넥트
         KinectSensor nui = null;
         Skeleton[] Skeletons = null;

         //음성인식
         SpeechRecognitionEngine sre;
         RecognizerInfo ri;
         KinectAudioSource source;
         Stream audioStream;

         //쓰레드
         ThreadStart ts;
         Thread th;

         //폰트
         SpriteFont messageFont;
         string message = "start";




         Texture2D KinectVideoTexture;
         Rectangle VideoDisplayRectangle;
         Texture2D idot1;
         Texture2D idot2;

         Rectangle drawrec1;
         Rectangle drawrec2;

#endif



         enum GameStates { Menu, Playing, SongMenu, GameOver };
        GameStates gameState = GameStates.Menu;
        

        public static Texture2D spriteSheet;
        public static Texture2D menu;
        public static Texture2D heart;
        public static Texture2D dot;

    //    private Texture2D tileSprite;
        private Texture2D uiBackground;
        private Texture2D uiHeart;



        private Rectangle removeAreaRec = new Rectangle(0, 0, 0, 0);
      
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
          //  content = new ContentManager(Services);


        
        }



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
#if Kinect
            
            //KINECT
             VideoDisplayRectangle = new Rectangle(0, 0, 1200, 900);

            drawrec1 = new Rectangle(0, 0, GraphicsDevice.Viewport.Width / 20, GraphicsDevice.Viewport.Height / 20);
            drawrec2 = new Rectangle(0, 0, GraphicsDevice.Viewport.Width / 20, GraphicsDevice.Viewport.Height / 20);
#endif
            base.Initialize();

        }
          
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

            Item.LoadContent(Content);
            SoundManager.Init();
            LoadSound();
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
           
            
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

            //Vector2 mark1Location = new Vector2(400, 70);
            //Vector2 mark2Location = new Vector2(500, 170);
            //Vector2 mark3Location = new Vector2(500, 270);
            //Vector2 mark4Location = new Vector2(400, 370);
            //Vector2 mark5Location = new Vector2(300, 270);
            //Vector2 mark6Location = new Vector2(300, 170);


            Vector2 mark1Location = new Vector2(200, 140);
            Vector2 mark2Location = new Vector2(400, 270);
            Vector2 mark3Location = new Vector2(300, 370);
            Vector2 mark4Location = new Vector2(200, 570);
            Vector2 mark5Location = new Vector2(100, 370);
            Vector2 mark6Location = new Vector2(100, 270);
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

#if Kinect
            idot1 = Content.Load<Texture2D>("Bitmap1");
            idot2 = Content.Load<Texture2D>("Bitmap2");
            messageFont = Content.Load<SpriteFont>("MessageFont");

            ////키넥트 셋업
            setupKinect();
#endif
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

#if Kinect
        //키넥트 셋업
        #region Kinect Setup
        protected bool setupKinect()
        {
            if (KinectSensor.KinectSensors.Count == 0)
            {
                return false;
            }

            //파라미터
            var parameters = new TransformSmoothParameters
            {
                Smoothing = 0.3f,
                Correction = 0.0f,
                Prediction = 0.0f,
                JitterRadius = 1.0f,
                MaxDeviationRadius = 0.5f


                //Smoothing = 0.5f,
                //Correction = 0.5f,
                //Prediction = 0.5f,
                //JitterRadius = 0.05f,
                //MaxDeviationRadius = 0.04f

                //Smoothing = 0.5f,
                //Correction = 0.1f,
                //Prediction = 0.5f,
                //JitterRadius = 0.1f,
                //MaxDeviationRadius = 0.1f

                //Smoothing = 0.7f,
                //Correction = 0.3f,
                //Prediction = 1.0f,
                //JitterRadius = 1.0f,
                //MaxDeviationRadius = 1.0f

                //Smoothing = 0.05f,
                //Correction = 0.5f,
                //Prediction = 0.5f,
                //JitterRadius = 0.8f,
                //MaxDeviationRadius = 0.2f
            };

            ////키넥트 센서
            nui = KinectSensor.KinectSensors[0];

            try
            {
                //컬러스트림 
                //nui.ColorStream.Enable();
                //nui.ColorFrameReady += new EventHandler<ColorImageFrameReadyEventArgs>(nui_ColorFrameReady);

                //스켈레톤 스트림
                nui.SkeletonStream.Enable(parameters);
                //nui.SkeletonStream.Enable();
                nui.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(nui_SkeletonFrameReady);

                //뎁스스트림
                //nui.DepthStream.Enable();
                //nui.DepthFrameReady += new EventHandler<DepthImageFrameReadyEventArgs>(nui_DepthFrameReady);

                //키넥트 시작
                nui.Start();

                //음성인식
                setupAudio();
            }
            catch
            {
                return false;
            }

            return true;
        }
        #endregion

        //음성인식
        #region Speech Recognition

        private void setupAudio()
        {

            foreach (RecognizerInfo reinfo in SpeechRecognitionEngine.InstalledRecognizers())//인스톨된 모든 스피치 엔진 불러온다.
            {
                if (reinfo.Id == "SR_MS_en-US_Kinect_11.0")
                {
                    ri = reinfo;
                    break;
                }
            }

            if (ri == null)
            {
                return;
            }


            try
            {
                sre = new SpeechRecognitionEngine(ri.Id);
            }
            catch
            {
                return;
            }

            //원하는 단어 입력
            var choices = new Choices();
            //choices.Add("Yes");
            choices.Add("green");
            choices.Add("kinect");
            choices.Add("go");
            choices.Add("next");
            choices.Add("stop");
            choices.Add("wiro");
            choices.Add("wero");


            var gb = new GrammarBuilder { Culture = ri.Culture };
            gb.Append(choices);
            var g = new Grammar(gb);

            sre.LoadGrammar(g);
            sre.SpeechHypothesized += new EventHandler<SpeechHypothesizedEventArgs>(sre_SpeechHypothesized);
            sre.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(sre_SpeechRecognized);

            //쓰레드 시작
            ts = new ThreadStart(UserFunc);
            th = new Thread(ts);
            th.Start();
        }


        private void UserFunc()
        {

            //오디오 스트림
            try
            {
                source = nui.AudioSource;
                source.BeamAngleMode = BeamAngleMode.Adaptive;
                audioStream = source.Start();

                sre.SetInputToAudioStream(audioStream, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                sre.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch
            {
                return;
            }
        }

        void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence < 0.5) return;//신뢰도 0.5미만일땐 리턴
            message = e.Result.Text + " " + e.Result.Confidence.ToString();
            switch (e.Result.Text)
            {
                case "stop":
                    nui.ElevationAngle += 3;//앵글 올리기
                    break;


                case "next":
                case "nest":
                case "naxt":
                    nui.ElevationAngle -= 3;
                    break;
            }
        }

        void sre_SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
            //if (e.Result.Confidence < 0.5) return;
            //message = e.Result.Text + " " + e.Result.Confidence.ToString();

            //switch (e.Result.Text)
            //{
            //    case "Red":
            //        nui.ElevationAngle -= 3;
            //        break;
            //    case "green":
            //        nui.ElevationAngle += 3;
            //        break;

            //}

        }


        #endregion
        //디스플레이
        #region Color display

     

        //컬러 프레임
        void nui_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            byte[] ColorData = null;

            using (ColorImageFrame ImageParam = e.OpenColorImageFrame())
            {
                if (ImageParam == null) return;
                if (ColorData == null)
                    ColorData = new byte[ImageParam.Width * ImageParam.Height * 4];
                ImageParam.CopyPixelDataTo(ColorData);
                KinectVideoTexture = new Texture2D(GraphicsDevice, ImageParam.Width, ImageParam.Height);
                Color[] bitmap = new Color[ImageParam.Width * ImageParam.Height];
                bitmap[0] = new Color(ColorData[2], ColorData[1], ColorData[0], 255);

                int sourceOffset = 0;
                for (int i = 0; i < bitmap.Length; i++)
                {
                    bitmap[i] = new Color(ColorData[sourceOffset + 2], ColorData[sourceOffset + 1], ColorData[sourceOffset], 255);
                    sourceOffset += 4;
                }
                KinectVideoTexture.SetData(bitmap);


            }
        }
        #endregion
        //스켈레톤 프레임
        #region Skeleton
        void nui_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (SkeletonFrame frame = e.OpenSkeletonFrame())
            {
                if (frame != null)
                {
                    Skeletons = new Skeleton[frame.SkeletonArrayLength];
                    frame.CopySkeletonDataTo(Skeletons);
                }
            }
        }
        #endregion

#endif
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
            collisionManager.checkDragNote(new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y));

            collisionManager.CheckCollisions(0, new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y));

            collisionManager.CheckCollisions(1, new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y));

            collisionManager.CheckCollisions(2, new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y));

            collisionManager.CheckCollisions(3, new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y));

            collisionManager.CheckCollisions(4, new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y));

            collisionManager.CheckCollisions(5, new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y));

        }
#if Kinect
        private void HandleInput()
        {
            collisionManager.checkDragNote(new Vector2(drawrec1.X, drawrec1.Y));

            collisionManager.CheckCollisions(0, new Vector2(drawrec1.X, drawrec1.Y));

            collisionManager.CheckCollisions(1, new Vector2(drawrec1.X, drawrec1.Y));

            collisionManager.CheckCollisions(2, new Vector2(drawrec1.X, drawrec1.Y));

            collisionManager.CheckCollisions(3, new Vector2(drawrec1.X, drawrec1.Y));

            collisionManager.CheckCollisions(4, new Vector2(drawrec1.X, drawrec1.Y));

            collisionManager.CheckCollisions(5, new Vector2(drawrec1.X, drawrec1.Y));

        }
#endif
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
                MarkManager.changeMarkPattern(0);
            }

            if (keyState.IsKeyDown(Keys.NumPad7))
            {
                //  collisionManager.CheckCollisions(4);
          //      blendMode = BlendState.Additive;
                MarkManager.changeMarkPattern(1);
            }

            if (keyState.IsKeyDown(Keys.NumPad8))
            {
           //     //  collisionManager.CheckCollisions(4);
//blendMode = BlendState.NonPremultiplied;

                MarkManager.changeMarkPattern(2);
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
#if Kinect
                    HandleInput();  
#endif
                    file.Update(spriteBatch, gameTime);
                    DragNoteManager.Update(gameTime);
                    GoldManager.Update(gameTime);
                    perfectManager.Update(gameTime);
                    goodManager.Update(gameTime);
                    badManager.Update(gameTime);
                    goldGetManager.Update(gameTime);
                    scoreManager.Update(gameTime);



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
           
#if Kineck
            Window.Title = drawrec1.X.ToString() + " - " + drawrec1.Y.ToString() + "마우스" + mouseStateCurrent.X.ToString() + "-" + mouseStateCurrent.Y.ToString();
#endif
            //Window.Title = ;

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
                int combo = scoreManager.Combo;
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
#if Kinect
                //컬러 디스플레이
                if (KinectVideoTexture != null)
                {
                    spriteBatch.Draw(KinectVideoTexture, VideoDisplayRectangle, Color.White);

                }

                //손에 따라 사각형 그리기
                if (Skeletons != null)
                {
                    foreach (Skeleton s in Skeletons)
                        if (s.TrackingState == SkeletonTrackingState.Tracked)
                        {
                            drawpoint(s.Joints[JointType.HandRight], s.Joints[JointType.HandLeft]);
                        }
                }

                //음성인식 메시지
                if (message.Length > 0)
                {
                    spriteBatch.DrawString(messageFont, message, Vector2.Zero, Color.White);
                }
            
#endif
            }
            if (gameState == GameStates.SongMenu)
            {
                songMenu.Draw(spriteBatch);

            }
            spriteBatch.End();
            
            base.Draw(gameTime);
        }

#if Kinect
        //손동작 스케일 변환
        #region Hand scale
        void drawpoint(Joint j1, Joint j2)
        {
            ////실질적인 스케일 변환
            Joint j1r = j1.ScaleTo(1024, 768, .3f, .3f);

            //그리기
            drawrec1.X = (int)j1r.Position.X - drawrec1.Width / 2;
            drawrec1.Y = (int)j1r.Position.Y - drawrec1.Height / 2;

            //손 그대로에 그릴때(대신 화면 사이즈를 640*480으로 맞춰야함)
            //ColorImagePoint jp1 = nui.MapSkeletonPointToColor(j1.Position, ColorImageFormat.RgbResolution640x480Fps30);
            //drawrec1.X = jp1.X - drawrec1.Width / 2;
            //drawrec1.Y = jp1.Y - drawrec1.Height / 2;



            spriteBatch.Draw(heart, drawrec1, Color.Red);
            

            Joint j2r = j2.ScaleTo(1024, 768, .3f, .3f);

            drawrec2.X = (int)j2r.Position.X - drawrec2.Width / 2;
            drawrec2.Y = (int)j2r.Position.Y - drawrec2.Height / 2;

            //ColorImagePoint jp2 = nui.MapSkeletonPointToColor(j2.Position, ColorImageFormat.RgbResolution640x480Fps30);
            //drawrec2.X = jp2.X - drawrec2.Width / 2;
            //drawrec2.Y = jp2.Y - drawrec2.Height / 2;

            spriteBatch.Draw(idot2, drawrec2, Color.Blue);
        }
        #endregion
#endif
       
    }
}