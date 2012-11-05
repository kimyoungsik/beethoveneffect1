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

namespace beethoven3
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        enum GameStates { TitleScreen, Playing, PlayerDead, GameOver };
        GameStates gameState = GameStates.Playing;

        Texture2D spriteSheet;
        Texture2D titleScreen;


        StartNoteManager startNoteManager;
        CollisionManager collisionManager;
        File file;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
   
        
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

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            spriteSheet = Content.Load<Texture2D>(@"Textures\SpriteSheet");
            titleScreen = Content.Load<Texture2D>(@"Textures\TitleScreen");
            // TODO: use this.Content to load your game content here
             
            Vector2 mark1Location = new Vector2(400, 70);
            Vector2 mark2Location = new Vector2(500, 170);
            Vector2 mark3Location = new Vector2(500, 270);
            Vector2 mark4Location = new Vector2(400, 370);
            Vector2 mark5Location = new Vector2(300, 270);
            Vector2 mark6Location = new Vector2(300, 170);


            startNoteManager = new StartNoteManager(
                spriteSheet,
                new Rectangle(0, 200, 50, 50),
                1);

            MarkManager.initialize(
           // markManager = new MarkManager(
                spriteSheet,
                new Rectangle(0, 200, 50, 50),
                6,
                mark1Location,
                mark2Location,
                mark3Location,
                mark4Location,
                mark5Location,
                mark6Location,
                startNoteManager
                
                );
            collisionManager = new CollisionManager();

            file = new File(startNoteManager);
            //곡선택화면에서
            file.Loading("a.txt");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        private void HandleKeyboardInput(KeyboardState keyState)
        {
            if (keyState.IsKeyDown(Keys.NumPad8))
            {
                collisionManager.CheckCollisions(0);
            }
            if (keyState.IsKeyDown(Keys.NumPad9))
            {
                collisionManager.CheckCollisions(1);
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

            // TODO: Add your update logic here
            spriteBatch.Begin();

            if (gameState == GameStates.TitleScreen)
            {

            }
            if ((gameState == GameStates.Playing) ||
                (gameState == GameStates.PlayerDead) ||
                (gameState == GameStates.GameOver))
            {
                MarkManager.Update(gameTime);
                startNoteManager.Update(gameTime);
                HandleKeyboardInput(Keyboard.GetState());
               // collisionManager.CheckCollisions();
          
                //file.Update(gameTime);
                file.Update(gameTime);


            }

            if (gameState == GameStates.GameOver)
            {

            }

            spriteBatch.End();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            MarkManager.Draw(spriteBatch);
            startNoteManager.Draw(spriteBatch);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
