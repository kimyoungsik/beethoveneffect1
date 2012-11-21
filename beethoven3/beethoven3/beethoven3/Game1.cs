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

        //KINECT
        //KinectSensor nui = null;
        //Texture2D KinectDepthTexture;
        //Rectangle DepthDisplayRectangle;

        enum GameStates { Menu, Playing, SongMenu, GameOver };
        GameStates gameState = GameStates.Menu;
        
        public static Texture2D spriteSheet;
        public static Texture2D menu;
        public static Texture2D heart;
        public static Texture2D dot;
        private Rectangle removeAreaRec = new Rectangle(360, 130, 130, 230);
        SongMenu songMenu;
        int result;

        StartNoteManager startNoteManager;
        CollisionManager collisionManager;
        File file;
        ExplosionManager perfectManager;
        ExplosionManager goodManager;
        ExplosionManager badManager;
        MouseState mouseStateCurrent;   

        static public int mousex = 100; //X좌표
        static public int mousey = 100; //Y좌표

        const int SCR_W = 1024;
        const int SCR_H = 768;
        //test
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferHeight = SCR_H;
            graphics.PreferredBackBufferWidth = SCR_W;

        
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
            spriteSheet = Content.Load<Texture2D>(@"Textures\SpriteSheet8");
            menu = Content.Load<Texture2D>(@"Textures\menu");
            heart = Content.Load<Texture2D>(@"Textures\heart");
            dot = Content.Load<Texture2D>(@"Textures\dot");
            // TODO: use this.Content to load your game content here
           
          //  LineRenderer.LoadContent(content);
            
            Vector2 mark1Location = new Vector2(400, 70);
            Vector2 mark2Location = new Vector2(500, 170);
            Vector2 mark3Location = new Vector2(500, 270);
            Vector2 mark4Location = new Vector2(400, 370);
            Vector2 mark5Location = new Vector2(300, 270);
            Vector2 mark6Location = new Vector2(300, 170);

            
            startNoteManager = new StartNoteManager(
                spriteSheet,
                new Rectangle(0, 200, 52, 55),
                1);

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
            collisionManager = new CollisionManager(perfectManager, goodManager, badManager);
            
            
            NoteFileManager noteFileManager = new NoteFileManager();
            file = new File(startNoteManager, noteFileManager, badManager);
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
                 badManager);

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

        //void nui_DepthFrameReady(object sender, DepthImageFrameReadyEventArgs e)
        //{

        //    short[] depthData = null;

        //    using (DepthImageFrame ImageParam = e.OpenDepthImageFrame())
        //    {
        //        if (ImageParam == null) return;

        //        if (depthData == null)
        //            depthData = new short[ImageParam.Width * ImageParam.Height];

        //        ImageParam.CopyPixelDataTo(depthData);




        //        Color[] bitmap = new Color[ImageParam.Width * ImageParam.Height];

        //        for (int i = 0; i < bitmap.Length; i++)
        //        {

        //            int depth = depthData[i] >> 3;
        //            if (depth == nui.DepthStream.UnknownDepth)
        //                bitmap[i] = Color.Red;
        //            else
        //                if (depth == nui.DepthStream.TooFarDepth)
        //                    bitmap[i] = Color.Blue;
        //                else
        //                    if (depth == nui.DepthStream.TooNearDepth)
        //                        bitmap[i] = Color.Green;
        //                    else
        //                    {
        //                        byte depthByte = (byte)(255 - (depth >> 5));
        //                        bitmap[i] = new Color(depthByte, depthByte, depthByte, 255);
        //                    }
        //        }
        //        KinectDepthTexture = new Texture2D(GraphicsDevice, ImageParam.Width, ImageParam.Height);
        //        KinectDepthTexture.SetData(bitmap);
                
        //    }
        //}
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
            //눌렀을때만.
           // if (mouseState.LeftButton == ButtonState.Pressed)
          //  {
                collisionManager.checkDragNote(new Vector2(mouseStateCurrent.X,mouseStateCurrent.Y));

                collisionManager.CheckCollisions(0, new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y));

                collisionManager.CheckCollisions(1, new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y));

                collisionManager.CheckCollisions(2, new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y));

                collisionManager.CheckCollisions(3, new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y));

                collisionManager.CheckCollisions(4, new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y));

                collisionManager.CheckCollisions(5, new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y));
               
          //  }

        }


        private void HandleKeyboardInput(KeyboardState keyState)
        {
        //{
        //    if (keyState.IsKeyDown(Keys.NumPad8))
        //    {
        //     //   SoundManager.SndPlay("snd/ka");
        //        collisionManager.CheckCollisions(0);
        //    }
        //    if (keyState.IsKeyDown(Keys.NumPad9))
        //    {
        //     //   SoundManager.SndPlay("snd/maid");
        //        collisionManager.CheckCollisions(1);
        //    }
        //    if (keyState.IsKeyDown(Keys.NumPad6))
        //    {
        //      //  SoundManager.SndPlay("snd/jo");
        //        collisionManager.CheckCollisions(2);
        //    }
        //    if (keyState.IsKeyDown(Keys.NumPad5))
        //    {
        //      //  SoundManager.SndStop();
        //        collisionManager.CheckCollisions(3);
        //    }
        //    if (keyState.IsKeyDown(Keys.NumPad4))
        //    {
        //        collisionManager.CheckCollisions(4);
        //    }
        //    if (keyState.IsKeyDown(Keys.NumPad7))
        //    {
        //        collisionManager.CheckCollisions(5);
        //    }
          
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

     //       mousex = mouseStateCurrent.X;
     //       mousey = mouseStateCurrent.Y;
     //          Window.Title = "|"+mousex + "|"+ mousey;

     //       mouseRect = new Rectangle(mouseStateCurrent.X, mouseStateCurrent.Y, 5, 5);
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
                    //HandleKeyboardInput(Keyboard.GetState());
                    HandleMouseInput(Mouse.GetState());

                    file.Update(spriteBatch, gameTime);
                    DragNoteManager.Update(gameTime);
                    GoldManager.Update(gameTime);
                    perfectManager.Update(gameTime);
                    goodManager.Update(gameTime);
                    badManager.Update(gameTime);
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
            // TODO: Add your update logic here
           
            //if (gameState == GameStates.SongMenu)
            //{

            //}
            //if (gameState == GameStates.Playing) 

            //{
            //    MarkManager.Update(gameTime);
            //    startNoteManager.Update(gameTime);
            //    HandleKeyboardInput(Keyboard.GetState());
            //    HandleMouseInput(Mouse.GetState());

            //    file.Update(spriteBatch, gameTime);
            //    DragNoteManager.Update(gameTime);
            //    perfectManager.Update(gameTime);
            //    goodManager.Update(gameTime);
            //}

            //if (gameState == GameStates.GameOver)
            //{

            //}
            

            //spriteBatch.End();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
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
                //가운데 빨간 사각형 주석하면 보이지않는다.
           //     spriteBatch.Draw(dot, removeAreaRec, Color.Red);
            }

            if (gameState == GameStates.SongMenu)
            {
                songMenu.Draw(spriteBatch);

            }
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

       
    }
}