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
using System.Diagnostics;
using FMOD;


namespace beethoven3
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region 선언
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //기본 글꼴
        //basic font
        private SpriteFont pericles36Font;


        enum GameStates { Menu, Playing, SongMenu, ShopDoor,
                          RightItemShop, LeftItemShop, EffectItemShop, NoteItemShop, BackgroundItemShop,
                          ResultManager, RecordBoard };

        //게임 씬, 처음시작은 메뉴
        private GameStates gameState = GameStates.Menu;

        //타이틀 화면 
        private MenuScene menuScene;

        //상점 화면
        private ShopDoor shopDoor;
        
        //곡 선택 화면
        private SongMenu songMenu;

        //선택된 곡
        private int resultSongMenu;

        //마지막 순위 리스트 보여주는 화면 
        private RecordBoard recordBoard;



        //노트 생성 부분 관리
        private StartNoteManager startNoteManager;
        
        //충돌 관리
        private CollisionManager collisionManager;
        
        //악보파일 관리(불러오기 등)
        private File file;

        /////이펙트 관리 - start
        private ExplosionManager perfectManager;
        private ExplosionManager goodManager;
        private ExplosionManager badManager;
        private ExplosionManager goldGetManager;
        /////이펙트 관리 - end
        
       
        //아이템 관리
        private ItemManager itemManager;

        /////아이템 상점 - start
        private RightItemShop rightItemShop;
        private LeftItemShop leftItemShop;
        private EffectItemShop effectItemShop;
        private NoteItemShop noteItemShop;
        private BackgroundItemShop backgroundItemShop;
        /////아이템 상점 - end


        //곡 중간에 점수,골드 관리
        private ScoreManager scoreManager;


        //악기연주자 관리
        private MemberManager memberManager;


        //곡 끝나고 점수, 골드 관리
        private ResultManager resultManager;

        //곡당 높은 점수 기록 관리
        private ReportManager reportManager;

        //각 노트에대한 정보를 관리, 곡선택이나 결과화면에 나타내는 노트 정보를 가지고 있음
        private NoteFileManager noteFileManager;

        //현재 연주되는 노래 제목
        private String currentSongName;
        
        //화면 크기
        const int SCR_W = 1024;
        const int SCR_H = 768;

        //점수위치
        Vector2 scorePosition = new Vector2(705, 5);

        /////FMOD 선언 - START
        private FMOD.System sndSystem;
        private FMOD.Channel sndChannel = new FMOD.Channel();
        private FMOD.Sound sndSound = new FMOD.Sound();
        private FMOD.RESULT resultFmod;
        // private FMOD.DSPConnection dspconnectiontemp = null;
        /////FMOD 선언 - END


        /////템포 관련 -START
        //템포 변경 여부 -TRUE면 템포 변경된 상태
        private bool isChangedTempo= false;
        
        //변경된 템포
        private double changedTempo = 0;
        
        //기존 변경되기전 템포
        private float basicFrequency = 0;

        //템포 변하고 나서 변하는 시간
        private double chagneLimitedTime = 0;

        //템포 변경되면 다른 뒤의 모든 노트들에게 영향이 가는 시간의 량, 더하거나 빼준다.
        private double optionalTime = 0;

        //템포관련해서 한번만 실행 -- 임시로 넣은것
        private bool oneTime = true;
        /////템포 관련 -END

      
        //마우스 상태 
        MouseState mouseStateCurrent, mouseStatePrevious;

        //노래들이 있는 경로
        String songsDir = "c:\\beethoven\\";
        
        /////Texture start

        //현재는 노트와 마커 텍스쳐 이펙트까지
        //NOTES, MARKERS TEXTERUE
        public static Texture2D spriteSheet;

        //드래그 노트 텍스쳐
        //DragNote Textere
        public static Texture2D heart;

        //UI 배경
        private Texture2D uiBackground;

        //UI 컨텐츠
        private Texture2D uiHeart;

        //배경
        //whole background
        private Texture2D playBackgroud1;
        private Texture2D playBackgroud2;

        ////Explosion
        //private Texture2D windExplosion;
        //private Texture2D heartExplosion;
        //private Texture2D needleExplosion;
        //private Texture2D starExplosion1;
        //private Texture2D starExplosion2;
        //private Texture2D leafExplosion;

        //놓친 노트가 없어지는 곳
        //the place where miss note disapper
        //마커 패턴에 따라 달라져야 함. 
        private Rectangle removeAreaRec = new Rectangle(0, 0, 0, 0);

        /////Texture end 


        /////키넥트 관련 선언 - START
        //for kinect
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
        string message = "";


        Texture2D KinectVideoTexture;
        Rectangle VideoDisplayRectangle;
        Texture2D idot1;
        Texture2D idot2;

        Rectangle drawrec1;
        Rectangle drawrec2;
        Item selectedItem;
#endif
        /////키넥트 관련 선언 - END

        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //화면 크기 고정
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
            //마우스 보이기
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
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            /* 음원을 로드시킬 때 createStream 과 createSound 두가지가 있는 것을 확인할 수 있는데
 createStream은 배경음악을, createSound는 효과음을 넣는것이 좋습니다.*/

            //FMOD 세팅 -START
            resultFmod = FMOD.Factory.System_Create(ref sndSystem);
            sndSystem.init(1, FMOD.INITFLAG.NORMAL, (IntPtr)null);
            //FMOD 세팅 -END
           
           //타이틀화면
            menuScene = new MenuScene();
            menuScene.LoadContent(Content);

            //상점 대문
            shopDoor = new ShopDoor();
            shopDoor.LoadContent(Content);

            //아이템관리
            //startnotemanager 생성보다 앞에 있어야 한다.
            itemManager = new ItemManager();
            itemManager.LoadContent(Content);
            itemManager.Init();

            //게임중 점수관리
            scoreManager = new ScoreManager(); 

            /////아이템 상점 -START
            rightItemShop = new RightItemShop(itemManager,  scoreManager);
            rightItemShop.LoadContent(Content);

            leftItemShop = new LeftItemShop(itemManager, scoreManager);
            leftItemShop.LoadContent(Content);

            effectItemShop = new EffectItemShop(itemManager, scoreManager);
            effectItemShop.LoadContent(Content);

            noteItemShop = new NoteItemShop(itemManager, scoreManager);
            noteItemShop.LoadContent(Content);

            backgroundItemShop = new BackgroundItemShop(itemManager, scoreManager);
            backgroundItemShop.LoadContent(Content);
            /////아이템 상점 -START

            //연주자
            memberManager = new MemberManager();
            memberManager.LoadContent(Content);
            memberManager.init();

            /////기존 XNA MP3플레이 - 삭제 가능성 있음 //***
            SoundManager.Init();
            LoadSound();
            /////
            

            /////텍스쳐 로드 -START
            //배경
            playBackgroud1 = Content.Load<Texture2D>(@"background\tutorialbear");
            playBackgroud2 = Content.Load<Texture2D>(@"background\crosswalk3");
            
            //노트,마커
            spriteSheet = Content.Load<Texture2D>(@"Textures\SpriteSheet8");
            
            //드래그 노트
            heart = Content.Load<Texture2D>(@"Textures\heart");
           
            //진행상황
            uiBackground = Content.Load<Texture2D>(@"ui\background");
            uiHeart = Content.Load<Texture2D>(@"ui\heart");
            
            //폰트
            pericles36Font = Content.Load<SpriteFont>(@"Fonts\Pericles36");
            /////텍스쳐 로드 -END

            //explosion
            //windExplosion = Content.Load<Texture2D>(@"Explosion\windExplosion2");
            //heartExplosion = Content.Load<Texture2D>(@"Explosion\smallheart2");
            //needleExplosion = Content.Load<Texture2D>(@"Explosion\needleExplosion2");

            //starExplosion1 = Content.Load<Texture2D>(@"Explosion\starExplosion");
            //starExplosion2 = Content.Load<Texture2D>(@"Explosion\starExplosion2");
            //leafExplosion = Content.Load<Texture2D>(@"Explosion\leafExplosion");
            ////wind용

            //현재 장착한 이펙트의 인덱스를 전체 베이스에 찾음
            int effectIndex = itemManager.getEffectIndex();

            //***임시로 - 아래에 바로 넣었음
            //텍스쳐 가져오기
            Texture2D[] effectTextures = itemManager.GetEffectTexture();
            Texture2D[] goodEffectTextures = itemManager.GetGoodEffectTexture();
            Texture2D[] badEffectTextures = itemManager.GetBadEffectTexture();
            Texture2D[] missEffectTextures = itemManager.GetMissEffectTexture();

            //텍스쳐 크기


            ///////이펙트 생성 -START

         

            perfectManager = new ExplosionManager();
            perfectManager.ExplosionInit(itemManager.GetEffectTexture()[effectIndex], itemManager.GetEffectInitFrame()[effectIndex], itemManager.GetEffectFrameCount()[effectIndex], itemManager.GetEffectScale()[effectIndex], itemManager.GetEffectDulation()[effectIndex]);

            goodManager = new ExplosionManager();
            goodManager.ExplosionInit(itemManager.GetGoodEffectTexture()[effectIndex], itemManager.GetEffectInitFrame()[effectIndex], itemManager.GetEffectFrameCount()[effectIndex], itemManager.GetEffectScale()[effectIndex], itemManager.GetEffectDulation()[effectIndex]);

            badManager = new ExplosionManager();
            badManager.ExplosionInit(itemManager.GetBadEffectTexture()[effectIndex], itemManager.GetEffectInitFrame()[effectIndex], itemManager.GetEffectFrameCount()[effectIndex], itemManager.GetEffectScale()[effectIndex], itemManager.GetEffectDulation()[effectIndex]);

            //미스도 투입되면
            //missManager = new ExplosionManager();
            //missManager.ExplosionInit(missEffectTextures[effectIndex], new Rectangle(0, 0, 166, 162), 9, 1f, 45);


            //일단은 miss effect로
            goldGetManager = new ExplosionManager();
            goldGetManager.ExplosionInit(itemManager.GetMissEffectTexture()[effectIndex], itemManager.GetEffectInitFrame()[effectIndex], itemManager.GetEffectFrameCount()[effectIndex], itemManager.GetEffectScale()[effectIndex], itemManager.GetEffectDulation()[effectIndex]);

            ///////이펙트 생성 -END
       
            //시작 노트 관리 생성    
            //여기에 들어가는 텍스쳐가 노트의 텍스쳐가 된다.
            //상점의 텍스쳐가 들어가야 한다. 


            startNoteManager = new StartNoteManager(
                spriteSheet,
                new Rectangle(0, 200, 52, 55),
                1);


            //첫 마커 위치   
            //마커 위치 처음 시작은 일단 마커매니저의 제일 0번째 것으로 시작
            Vector2[] initMarkLocation = MarkManager.GetPattern(0);


            //마크 관리 초기화 (STATIC)
            //***시작시 마커 위치에 관한 사항 결정 못함.  
            MarkManager.initialize(
                spriteSheet,
                new Rectangle(0, 200, 50, 55),
                1,
                initMarkLocation[0],
                initMarkLocation[1],
                initMarkLocation[2],
                initMarkLocation[3],
                initMarkLocation[4],
                initMarkLocation[5],
                startNoteManager,
                removeAreaRec
                );

           

            //충돌관리 생성
            collisionManager = new CollisionManager(perfectManager, goodManager, badManager, goldGetManager, scoreManager, memberManager,/*effect크기*/itemManager);
            
            //노트정보 관리 생성
            noteFileManager = new NoteFileManager();
            
            //노트파일 읽기 관리 생성
            file = new File(startNoteManager, noteFileManager, badManager, scoreManager);
            
            //곡선택화면 곡 불러오는 폴더 
            String dir = "C:\\beethoven";
           
            //폴더가 없으면 새로 만들기 
            if (!System.IO.File.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir);
            }

            //곡을 불러오기
            file.FileLoading(dir, "*.mnf");
            
            //드래그노트 초기화
            DragNoteManager.initialize(
                 spriteSheet,
                 new Rectangle(0, 100, 50, 50),
                 6,
                 15,
                 0,
                 badManager,
                 scoreManager);

            //골드 초기화
            GoldManager.initialize(
                spriteSheet,
                new Rectangle(0, 100, 20, 20),
                1,
                15,
                0);

            //***
            songMenu = new SongMenu(noteFileManager);
            songMenu.Load(Content,graphics.GraphicsDevice);

            
            resultManager = new ResultManager();
            resultManager.LoadContent(Content);

            //점수기록판 화면
            recordBoard = new RecordBoard();
            recordBoard.LoadContent(Content);

            //점수 기록 (TO FILE)
            reportManager = new ReportManager(scoreManager);
            

            //LOAD REPORT SCORE FILE
            //점수기록판을 로드해서 게임에 올린다. 

            reportManager.LoadReport();

            //골드를 로드해서 게임에 올린다. 
            reportManager.LoadGoldFromFile();

            currentSongName = "";
#if Kinect
            idot1 = Content.Load<Texture2D>("Bitmap1");
            idot2 = Content.Load<Texture2D>("Bitmap2");
            messageFont = Content.Load<SpriteFont>("MessageFont");

            ////키넥트 셋업
            setupKinect();
#endif


        }

     
        //*** xna지원 mp3재생
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


        //마우스 충돌 처리
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
        //키넥트 충돌 처리
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


        //템포변경
        private void tempoChange(double changedT)
        {
            isChangedTempo = true;

            this.changedTempo = changedT;

            float frequency = 0;
            
            //현재 템포 가져와소 float frequency에 넣기//resultFmod 는 성공여부만 나타남
            resultFmod = sndChannel.getFrequency(ref frequency);
         
            //템포설정
            sndChannel.setFrequency(frequency * (float)changedT);
            
            //템포를 다른 노트 모두에 적용
            file.ChangeArrayNoteTempo(changedT);

            //현재 설정된 두번째 가이드라인이 있으면 지움
            GuideLineManager.DeleteAllSecondGuideLine();
        

        }


        //템포 변경 전에 기본 템포를 저장해둠. 다시 롤백할 때 필요
        public void SetBasicTempo()
        {
            if (!isChangedTempo)
            {
               sndChannel.getFrequency(ref basicFrequency);
            }
        }

        //이전에 설정한 기본 템포로 돌아감 
        public void ReturnBasicTempo()
        {
            if (basicFrequency != 0)
            {
                sndChannel.setFrequency(basicFrequency);
                isChangedTempo = false;
                changedTempo = 0;
            }
        }

        //일단 안쓰임
        //일정 시간이 지나면 다시 원래 템포로 돌아옴
        //private void AutoRetrunChangeTempo(GameTime gameTime)
        //{
        //    if (isChangedTempo)
        //    {              
        //        //처음시작 
        //        chagneLimitedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
        //        //Trace.WriteLine(chagneLimitedTime.ToString());
                
                
        //        if (chagneLimitedTime >= 3000 && oneTime)
        //        {
        //            optionalTime =( 3 - (3 / this.changedTempo) ) *-1;
        //                //템포가 4배가 된상태에서 1초동안 지속이 된다면 모두 1-  1/4   0.75초씩 줄여야 한다ㅣ
        //            oneTime = false;
        //            ReturnBasicTempo();
        //        }   
        //    }
        //}


        ////템포가 변하고나서 얼마나 변했는지 시간을 재는데 사용
        private void StartChangedTime(GameTime gameTime)
        {
            
            if (isChangedTempo)
            {
                //처음시작 
                chagneLimitedTime += gameTime.ElapsedGameTime.TotalMilliseconds;  
            }
        }


        //다시 원점으로 돌아갈 때 쓰임 
        
        //늘어나거나 줄어드는 양을 계산해주고 
        
        private void SetOptionalTime()
        {
            //임시로 넣은 것일 뿐
            if (oneTime)
            {
                //템포 다시 원상복귀
                file.ChangeArrayNoteTempoBack(this.changedTempo);
           
                double time = 0;

                //옵션 계산
                time = ((this.chagneLimitedTime / 1000) - ((this.chagneLimitedTime / 1000) / this.changedTempo)) * -1;
                                
                optionalTime += time;

                //각 노트 시작에 옵션을 더함
                file.OptionalArrayNote(optionalTime);
                
                //템포가 0.9배가 된상태에서 1초동안 지속이 된다면 모두 4-  4/4   3초씩 줄여야 한다ㅣ
                
                oneTime = false;
                chagneLimitedTime = 0;
                

                //원래 템포로 돌아감
                ReturnBasicTempo();
            }   
        }

        //상점안에서 상점 대문으로 가는 키보드 처리
        private void HandleKeyboardInputinShopDoor(KeyboardState keyState)
        {
            if (keyState.IsKeyDown(Keys.B))
            {
                gameState = GameStates.Menu;

            }

        }

        //상점 대문에서 타이틀 화면으로 가는 키보드 처리
        private void HandleKeyboardInputinItemShop(KeyboardState keyState)
        {
            if (keyState.IsKeyDown(Keys.B))
            {
                gameState = GameStates.ShopDoor;

            }

        }


        private void HandleKeyboardInput(KeyboardState keyState)
        {
            if (keyState.IsKeyDown(Keys.B))
            {
                sndSystem.createSound("C:\\beethoven\\"+noteFileManager, FMOD.MODE.HARDWARE, ref sndSound);
                sndSystem.playSound(CHANNELINDEX.FREE, sndSound, false, ref sndChannel);
            }
            if (keyState.IsKeyDown(Keys.P))
            {
               // resultFmod = sndChannel.getFrequency(ref basicFrequency);
               // sndChannel.setFrequency(60000.0f);
                //임시
                //BOOL 은 일단 임시로 
                oneTime = true;
                if (!isChangedTempo)
                {
                    tempoChange(1.2f);

                    //2의 템포가 2초동안 빨라지는 ㅔ
                }
            }

            if (keyState.IsKeyDown(Keys.O))
            {
                // resultFmod = sndChannel.getFrequency(ref basicFrequency);
                // sndChannel.setFrequency(60000.0f);
                //임시
                //BOOL 은 일단 임시로 
                oneTime = true;
                if (!isChangedTempo)
                {

                    //그 양만큼 템포 조절됨
                    tempoChange(0.8f);

                    //2의 템포가 2초동안 빨라지는 ㅔ
                }
            }

            if (keyState.IsKeyDown(Keys.L))
            {
               // sndChannel.setFrequency(44100.0f);
               // ReturnBasicTempo();

                SetOptionalTime();
            }

             if (keyState.IsKeyDown(Keys.F))
            {
               // sndChannel.setFrequency(44100.0f);
               // ReturnBasicTempo();
                 memberManager.SetMembersFrameTime(0.02f);
                //memberSetMembersFrameTime
            }

            //스트로크 1
            if (keyState.IsKeyDown(Keys.T))
            {
                memberManager.SetMemberState(4, 1);
            }

            //스트로크 2
            if (keyState.IsKeyDown(Keys.Y))
            {
                memberManager.SetMemberState(4, 2);
            }

            //스트로크 3
            if (keyState.IsKeyDown(Keys.U))
            {
                memberManager.SetMemberState(4, 3);
            }
           
        }
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
      
            mouseStateCurrent = Mouse.GetState();

           switch (gameState)
           {
               #region 타이틀
               //타이틀 화면
                case GameStates.Menu:
                    //곡선택화면으로
                    if ((Keyboard.GetState().IsKeyDown(Keys.Space))                    )
                    {
                        gameState = GameStates.SongMenu;
                    }
                    
                    //상점 대문으로
                    if ((Keyboard.GetState().IsKeyDown(Keys.S)))
                    {
                        gameState = GameStates.ShopDoor;
                    }
                    break;
               #endregion

               #region 상점대문
                //상점대문
               case GameStates.ShopDoor:
                    //타이틀화면으로 돌아가는 키보드처리
                    HandleKeyboardInputinShopDoor(Keyboard.GetState());

                   Rectangle rect = new Rectangle(mouseStateCurrent.X, mouseStateCurrent.Y, 5, 5);

                   //mousecursor on right hand item section
                   //오른쪽지휘봉 아이템
                   if (rect.Intersects(shopDoor.getRectRightHand()))
                   {
                       //hover on
                       shopDoor.setClickRightHand(true);
                       //click the right hand item section
                       if (mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released )
                       {
                           gameState = GameStates.RightItemShop;
                       }
                   }
                   else
                   {
                       //hover off
                       shopDoor.setClickRightHand(false);
                   }


                   //mouse cursor on left hadn item section
                   if (rect.Intersects(shopDoor.getRectLeftHand()))
                   {
                       shopDoor.setClickLeftHand(true);

                       if (mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released)
                       {
                           gameState = GameStates.LeftItemShop;
                       }
                   }
                   else
                   {
                       shopDoor.setClickLeftHand(false);
                   }


                   //note
                   if (rect.Intersects(shopDoor.getRectNote()))
                   {

                       shopDoor.setClickNote(true);
                       if (mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released)
                       {
                           gameState = GameStates.NoteItemShop;
                       }

                   }
                   else
                   {
                       shopDoor.setClickNote(false);
                   }


                   if (rect.Intersects(shopDoor.getRectEffect()))
                   {

                       shopDoor.setClickEffect(true);
                       if (mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released)
                       {
                           gameState = GameStates.EffectItemShop;
                       }

                   }
                   else
                   {
                       shopDoor.setClickEffect(false);
                   }


                   if (rect.Intersects(shopDoor.getRectBackground()))
                   {

                       shopDoor.setClickBackground(true);
                       if (mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released)
                       {
                           gameState = GameStates.BackgroundItemShop;
                       }

                   }
                   else
                   {
                       shopDoor.setClickBackground(false);
                   }
                    break;

                #endregion

               #region 아이템상점들
               case GameStates.RightItemShop:
                   //상점대문으로 돌아가는 키보드처리
                   HandleKeyboardInputinItemShop(Keyboard.GetState());

                   Rectangle mouseRect = new Rectangle(mouseStateCurrent.X, mouseStateCurrent.Y, 5, 5);
                   int i;

                   //아이템 rect
                   List<Rectangle> rectRightItems = rightItemShop.getRectRightItem();
                   
                   //아이템/
                   List<Item> shopRightItems = rightItemShop.getShopRightItem();

                   //다이얼로그를 띄웠을 때 이것이 중복실행되지 않도록 
                   if (!rightItemShop.getWearOne() && !rightItemShop.getBuyOne())
                   {
                       for (i = 0; i < rectRightItems.Count; i++)
                       {
                           //아이템을 선택 했을때
                           if (mouseRect.Intersects(rectRightItems[i]) && mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released)
                           {
                               //어두어짐
                               rightItemShop.setDarkBackground(true);
                                
                               selectedItem = shopRightItems[i];
                               
                               //이미 산거이면 true
                               if (rightItemShop.haveOne(shopRightItems[i]))
                               {
                                   //장착 메시지 박스 띄우기 
                                   rightItemShop.setWearOne(true);
                                   //반복 없애기
                                   i = rectRightItems.Count;                                 
                               }
                               else
                               {
                                   //구입 메시지 박스 띄우기 
                                   rightItemShop.setBuyOne(true);
                                   //반복 없애기
                                   i = rectRightItems.Count;
                               }
                           }
                       }
                   }

                   //돈 부족 메시지 띄우기
                   if (rightItemShop.getNoGold())
                   {
                       //버튼 Hover
                       if (mouseRect.Intersects(rightItemShop.getRectNoGoldButton()))
                       {
                           //눌린모양
                           rightItemShop.setHoverNoGoldButton(true);
                           //버튼 누르면
                           if (mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released)
                           {
                                //다시 밝게
                               rightItemShop.setDarkBackground(false);
                               //메시지 없애기
                               rightItemShop.setNoGold(false);
                           }
                       }
                       //버튼 not hover
                       else
                       {
                           rightItemShop.setHoverNoGoldButton(false);
                       }
                   }

                    //장착 메시지 띄우기
                   //message box about wearing item 
                   if (rightItemShop.getWearOne())
                   {
                       //mouse cursor on yes button
                       if (mouseRect.Intersects(rightItemShop.getRectYesButton()))
                       {
                           rightItemShop.setHoverYesButton(true);

                           if (mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released)
                           {
                               //선택된 아이템이 있으면
                               if (selectedItem != null)
                               {
                                   //find the index of item in myrightitem list
                                   //아이템 찾기
                                   int index = itemManager.getIndexOfAllRightItem(selectedItem);

                                   //아이템을 찾았으면
                                   if (index != -1)
                                   {
                                       //change index
                                       //장착
                                       itemManager.setRightHandIndex(index);

                                       //return to normal , remove message box
                                       //메시지 박스 지우기
                                       rightItemShop.setWearOne(false);
                                       //밝게 하기
                                       rightItemShop.setDarkBackground(false);
                                   }
                                   //아이템을 찾지 못했으면
                                   else
                                   {
                                       Trace.WriteLine("get wrong index( no item in list)");
                                   }
                               }
                               //선택된 아이템이 없으면
                               else
                               {
                                   Trace.WriteLine("nothing is selected");
                               }
                           }
                       }
                       else
                       {
                           rightItemShop.setHoverYesButton(false);
                       }
                       //mouse cursor on no button
                       //노버튼 눌렀을 때
                       if (mouseRect.Intersects(rightItemShop.getRectNoButton()))
                       {
                           rightItemShop.setHoverNoButton(true);
                           if (mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released)
                           {
                               //return to normal , remove message box
                               rightItemShop.setWearOne(false);
                               rightItemShop.setDarkBackground(false);
                           }
                       }
                       else
                       {
                           rightItemShop.setHoverNoButton(false);
                       }

                   }
                  
                   //구입 메시지
                   //message box about buying item
                   if (rightItemShop.getBuyOne())
                   {
                       //mouse cursor on right button
                       if (mouseRect.Intersects(rightItemShop.getRectYesButton()) )
                       {
                           rightItemShop.setHoverYesButton(true);
                           if (mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released)
                           {
                               //add item to my item
                               if (selectedItem != null)
                               {
                                   //돈으로 물건사기 
                                   //buy item with money


                                   //돈이 충분히 있다.
                                   if (scoreManager.TotalGold >= selectedItem.GetCost())
                                   {

                                       //전체 돈에서 구입비용 차감
                                       scoreManager.TotalGold -= selectedItem.GetCost();
                                       rightItemShop.addItemtoMyItem(selectedItem);
                                       rightItemShop.setBuyOne(false);
                                       rightItemShop.setDarkBackground(false);
                                       //돈을 파일에 저장
                                       reportManager.SaveGoldToFile();
                                   }
                                   //돈이 없다.
                                   else
                                   {

                                       rightItemShop.setBuyOne(false);
                                       //"돈 부족" 메시지 띄운다.
                                       rightItemShop.setNoGold(true);

                                       
                                   }

                                 
                                  
                               }
                               else
                               {
                                   Trace.WriteLine("nothing is selected");
                                 
                               }
                           }
                           
                       }
                       else
                       {
                           rightItemShop.setHoverYesButton(false);
                       }

                       if (mouseRect.Intersects(rightItemShop.getRectNoButton()))
                       {
                           rightItemShop.setHoverNoButton(true);
                           if (mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released)
                           {
                               //return to normal , remove message box
                               rightItemShop.setBuyOne(false);
                               rightItemShop.setDarkBackground(false);
                           }
                       }
                       else
                       {
                           rightItemShop.setHoverNoButton(false);
                       }

                   }

                   break;


               case GameStates.LeftItemShop:
                   //상점대문으로 돌아가는 키보드처리
                   HandleKeyboardInputinItemShop(Keyboard.GetState());

                   Rectangle mouseRectinleft = new Rectangle(mouseStateCurrent.X, mouseStateCurrent.Y, 5, 5);
                   int j;
                   //클릭했을때 어두워지는것.
                   List<Rectangle> rectLeftItems = leftItemShop.getRectLeftItem();
                   List<Item> shopLeftItems = leftItemShop.getShopLeftItem();
                   for (j = 0; j < rectLeftItems.Count; j++)
                   {
                       if (mouseRectinleft.Intersects(rectLeftItems[j]) && mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released)
                       {
                           leftItemShop.setDarkBackground(true);
                           //메시지 박스 띄우기  
                           selectedItem = shopLeftItems[j];
                           //이미 산거이면 true
                           if (leftItemShop.haveOne(shopLeftItems[j]))
                           {
                               leftItemShop.setWearOne(true);
                           }
                           else
                           {
                               leftItemShop.setBuyOne(true);
                           }
                       }
                   }



                   //돈 부족 메시지 
                   if (leftItemShop.getNoGold())
                   {
                       if (mouseRectinleft.Intersects(leftItemShop.getRectNoGoldButton()))
                       {

                           leftItemShop.setHoverNoGoldButton(true);
                           if (mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released)
                           {

                               leftItemShop.setDarkBackground(false);

                               leftItemShop.setNoGold(false);
                           }

                       }
                       else
                       {
                           leftItemShop.setHoverNoGoldButton(false);
                       }


                   }


                   //message box about wearing item 
                   if (leftItemShop.getWearOne())
                   {
                       //mouse cursor on yes button
                       if (mouseRectinleft.Intersects(leftItemShop.getRectYesButton()))
                       {
                           leftItemShop.setHoverYesButton(true);
                           if (mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released)
                           {
                               if (selectedItem != null)
                               {
                                   //find the index of item in myleftitem list
                                   int index = itemManager.getIndexOfAllLeftItem(selectedItem);
                                   if (index != -1)
                                   {
                                       //change index
                                       itemManager.setLeftHandIndex(index);

                                       //return to normal , remove message box
                                       leftItemShop.setWearOne(false);
                                       leftItemShop.setDarkBackground(false);
                                   }
                                   else
                                   {

                                       //get wrong index( no item in list)
                                   }
                               }
                               else
                               {
                                   //nothing is selected
                               }
                           }
                       }
                       else
                       {
                           leftItemShop.setHoverYesButton(false);
                       }
                       //mouse cursor on no button
                       if (mouseRectinleft.Intersects(leftItemShop.getRectNoButton()))
                       {
                           leftItemShop.setHoverNoButton(true);
                           if (mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released)
                           {
                               //return to normal , remove message box
                               leftItemShop.setWearOne(false);
                               leftItemShop.setDarkBackground(false);
                           }
                       }
                       else
                       {
                           leftItemShop.setHoverNoButton(false);
                       }
                   }




                   //message box about buying item
                   if (leftItemShop.getBuyOne())
                   {
                       //mouse cursor on right button

                       if (mouseRectinleft.Intersects(leftItemShop.getRectYesButton()))
                       {
                           leftItemShop.setHoverYesButton(true);
                           if (mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released)
                           {
                               //add item to my item
                               if (selectedItem != null)
                               {
                                   //돈으로 물건사기 
                                   //buy item with money

                                   //돈이 충분히 있다.
                                   if (scoreManager.TotalGold >= selectedItem.GetCost())
                                   {

                                       //전체 돈에서 구입비용 차감
                                       scoreManager.TotalGold -= selectedItem.GetCost();
                                       leftItemShop.addItemtoMyItem(selectedItem);
                                       //return to normal , remove message box
                                       leftItemShop.setBuyOne(false);
                                       leftItemShop.setDarkBackground(false);
                                       //돈을 파일에 저장
                                       reportManager.SaveGoldToFile();
                                   }
                                   //돈이 없다.
                                   else
                                   {

                                       leftItemShop.setBuyOne(false);
                                       //"돈 부족" 메시지 띄운다.
                                       leftItemShop.setNoGold(true);


                                   }

                               }
                               else
                               {

                                   //nothing is selected
                               }
                           }
                       }
                       else
                       {
                           leftItemShop.setHoverYesButton(false);
                       }
                       if (mouseRectinleft.Intersects(leftItemShop.getRectNoButton()))
                       {
                           leftItemShop.setHoverNoButton(true);
                           if (mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released)
                           {
                               //return to normal , remove message box
                               leftItemShop.setBuyOne(false);
                               leftItemShop.setDarkBackground(false);
                           }
                       }
                       else
                       {
                           leftItemShop.setHoverNoButton(false);
                       }
                   }
                   break;

               case GameStates.NoteItemShop:
                   //상점대문으로 돌아가는 키보드처리
                   HandleKeyboardInputinItemShop(Keyboard.GetState());

                   Rectangle mouseRectinNote = new Rectangle(mouseStateCurrent.X, mouseStateCurrent.Y, 5, 5);
                   int s;
                   //클릭했을때 어두워지는것.
                   List<Rectangle> rectNoteItems = noteItemShop.getRectNoteItem();
                   List<Item> shopNoteItems = noteItemShop.getShopNoteItem();
                   for (s = 0; s < rectNoteItems.Count; s++)
                   {
                       if (mouseRectinNote.Intersects(rectNoteItems[s]) && mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released)
                       {
                           noteItemShop.setDarkBackground(true);
                           //메시지 박스 띄우기  
                           selectedItem = shopNoteItems[s];
                           //이미 산거이면 true
                           if (noteItemShop.haveOne(shopNoteItems[s]))
                           {
                               noteItemShop.setWearOne(true);
                           }
                           else
                           {
                               noteItemShop.setBuyOne(true);
                           }
                       }
                   }

                   //돈 부족 메시지 
                   if (noteItemShop.getNoGold())
                   {
                       if (mouseRectinNote.Intersects(noteItemShop.getRectNoGoldButton()))
                       {

                           noteItemShop.setHoverNoGoldButton(true);
                           if (mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released)
                           {

                               noteItemShop.setDarkBackground(false);

                               noteItemShop.setNoGold(false);
                           }

                       }
                       else
                       {
                           noteItemShop.setHoverNoGoldButton(false);
                       }


                   }

                   //message box about wearing item 
                   if (noteItemShop.getWearOne())
                   {
                       //mouse cursor on yes button
                       if (mouseRectinNote.Intersects(noteItemShop.getRectYesButton()))
                       {
                           noteItemShop.setHoverYesButton(true);
                           if (mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released)
                           {
                               if (selectedItem != null)
                               {
                                   //find the index of item in myleftitem list
                                   int index = itemManager.getIndexOfAllNoteItem(selectedItem);
                                   if (index != -1)
                                   {
                                       //change index
                                       itemManager.setNoteIndex(index);

                                       //return to normal , remove message box
                                       noteItemShop.setWearOne(false);
                                       noteItemShop.setDarkBackground(false);
                                   }
                                   else
                                   {
                                       //get wrong index( no item in list)
                                   }
                               }
                               else
                               {
                                   //nothing is selected
                               }
                           }
                       }
                       else
                       {
                           noteItemShop.setHoverYesButton(false);
                       }
                       //mouse cursor on no button
                       if (mouseRectinNote.Intersects(noteItemShop.getRectNoButton()))
                       {
                           noteItemShop.setHoverNoButton(true);
                           if (mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released)
                           {
                               //return to normal , remove message box
                               noteItemShop.setWearOne(false);
                               noteItemShop.setDarkBackground(false);
                           }
                       }
                       else
                       {
                           noteItemShop.setHoverNoButton(false);
                       }
                   }
                   //message box about buying item
                   if (noteItemShop.getBuyOne())
                   {
                       //mouse cursor on right button

                       if (mouseRectinNote.Intersects(noteItemShop.getRectYesButton()))
                       {
                           noteItemShop.setHoverYesButton(true);
                           if (mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released)
                           {
                               //add item to my item
                               if (selectedItem != null)
                               {

                                   //돈으로 물건사기 
                                   //buy item with money

                                   //돈이 충분히 있다.
                                   if (scoreManager.TotalGold >= selectedItem.GetCost())
                                   {

                                       //전체 돈에서 구입비용 차감
                                       scoreManager.TotalGold -= selectedItem.GetCost();
                                       noteItemShop.addItemtoMyItem(selectedItem);
                                       //return to normal , remove message box
                                       noteItemShop.setBuyOne(false);
                                       noteItemShop.setDarkBackground(false);
                                       //돈을 파일에 저장
                                       reportManager.SaveGoldToFile();
                                   }
                                   //돈이 없다.
                                   else
                                   {

                                       noteItemShop.setBuyOne(false);
                                       //"돈 부족" 메시지 띄운다.
                                       noteItemShop.setNoGold(true);


                                   }

                                 
                               }
                               else
                               {
                                   //nothing is selected
                               }
                           }
                       }
                       else
                       {
                           noteItemShop.setHoverYesButton(false);
                       }
                       if (mouseRectinNote.Intersects(noteItemShop.getRectNoButton()))
                       {
                           noteItemShop.setHoverNoButton(true);
                           if (mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released)
                           {
                               //return to normal , remove message box
                               noteItemShop.setBuyOne(false);
                               noteItemShop.setDarkBackground(false);
                           }
                       }
                       else
                       {
                           noteItemShop.setHoverNoButton(false);
                       }
                   }
                   break;


               case GameStates.EffectItemShop:
                   //상점대문으로 돌아가는 키보드처리
                   HandleKeyboardInputinItemShop(Keyboard.GetState());

                   Rectangle mouseRectinEffect = new Rectangle(mouseStateCurrent.X, mouseStateCurrent.Y, 5, 5);
                   int k;
                   //클릭했을때 어두워지는것.
                   List<Rectangle> rectEffectItems = effectItemShop.getRectEffectItem();
                   List<Item> shopEffectItems = effectItemShop.getShopEffectItem();
                   for (k = 0; k < rectEffectItems.Count; k++)
                   {
                       if (mouseRectinEffect.Intersects(rectEffectItems[k]) && mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released)
                       {
                           effectItemShop.setDarkBackground(true);
                           //메시지 박스 띄우기  
                           selectedItem = shopEffectItems[k];
                           //이미 산거이면 true
                           if (effectItemShop.haveOne(shopEffectItems[k]))
                           {
                               effectItemShop.setWearOne(true);
                           }
                           else
                           {
                               effectItemShop.setBuyOne(true);
                           }
                       }
                   }


                   //돈 부족 메시지 
                   if (effectItemShop.getNoGold())
                   {
                       if (mouseRectinEffect.Intersects(effectItemShop.getRectNoGoldButton()))
                       {

                           effectItemShop.setHoverNoGoldButton(true);
                           if (mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released)
                           {

                               effectItemShop.setDarkBackground(false);

                               effectItemShop.setNoGold(false);
                           }

                       }
                       else
                       {
                           effectItemShop.setHoverNoGoldButton(false);
                       }


                   }



                   //message box about wearing item 
                   if (effectItemShop.getWearOne())
                   {
                       //mouse cursor on yes button
                       if (mouseRectinEffect.Intersects(effectItemShop.getRectYesButton()))
                       {
                           effectItemShop.setHoverYesButton(true);
                           if (mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released)
                           {
                               if (selectedItem != null)
                               {
                                   //find the index of item in myleftitem list
                                   //전체 아이템에서 내가 산 아이템의 위치를 찾는다.
                                   int index = itemManager.getIndexOfAllEffectItem(selectedItem);
                                   if (index != -1)
                                   {
                                       //change index
                                       itemManager.setEffectIndex(index);

                                       //return to normal , remove message box
                                       effectItemShop.setWearOne(false);
                                       effectItemShop.setDarkBackground(false);
                                   }
                                   else
                                   {
                                       //get wrong index( no item in list)
                                   }
                               }
                               else
                               {
                                   //nothing is selected
                               }
                           }
                       }
                       else
                       {
                           effectItemShop.setHoverYesButton(false);
                       }
                       //mouse cursor on no button
                       if (mouseRectinEffect.Intersects(effectItemShop.getRectNoButton()))
                       {
                           effectItemShop.setHoverNoButton(true);
                           if (mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released)
                           {
                               //return to normal , remove message box
                               effectItemShop.setWearOne(false);
                               effectItemShop.setDarkBackground(false);
                           }
                       }
                       else
                       {
                           effectItemShop.setHoverNoButton(false);
                       }
                   }
                   //message box about buying item
                   if (effectItemShop.getBuyOne())
                   {
                       //mouse cursor on right button

                       if (mouseRectinEffect.Intersects(effectItemShop.getRectYesButton()))
                       {
                           effectItemShop.setHoverYesButton(true);
                           if (mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released)
                           {
                               //add item to my item
                               if (selectedItem != null)
                               {

                                   //돈으로 물건사기 
                                   //buy item with money


                                   //돈이 충분히 있다.
                                   if (scoreManager.TotalGold >= selectedItem.GetCost())
                                   {

                                       //전체 돈에서 구입비용 차감
                                       scoreManager.TotalGold -= selectedItem.GetCost();
                                       effectItemShop.addItemtoMyItem(selectedItem);
                                       //return to normal , remove message box
                                       effectItemShop.setBuyOne(false);
                                       effectItemShop.setDarkBackground(false);

                                       //돈을 파일에 저장
                                       reportManager.SaveGoldToFile();
                                   }
                                   //돈이 없다.
                                   else
                                   {

                                       effectItemShop.setBuyOne(false);
                                       //"돈 부족" 메시지 띄운다.
                                       effectItemShop.setNoGold(true);


                                   }



                                  
                               }
                               else
                               {
                                   //nothing is selected
                               }
                           }
                       }
                       else
                       {
                           effectItemShop.setHoverYesButton(false);
                       }
                       if (mouseRectinEffect.Intersects(effectItemShop.getRectNoButton()))
                       {
                           effectItemShop.setHoverNoButton(true);
                           if (mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released)
                           {
                               //return to normal , remove message box
                               effectItemShop.setBuyOne(false);
                               effectItemShop.setDarkBackground(false);
                           }
                       }
                       else
                       {
                           effectItemShop.setHoverNoButton(false);
                       }
                   }
                   break;


               case GameStates.BackgroundItemShop:
                   //상점대문으로 돌아가는 키보드처리
                   HandleKeyboardInputinItemShop(Keyboard.GetState());

                   Rectangle mouseRectinBackground = new Rectangle(mouseStateCurrent.X, mouseStateCurrent.Y, 5, 5);
                   int r;
                   //클릭했을때 어두워지는것.
                   List<Rectangle> rectBackgroundItems = backgroundItemShop.getRectBackgroundItem();
                   List<Item> shopBackgroundItems = backgroundItemShop.getShopBackgroundItem();
                   for (r = 0; r < rectBackgroundItems.Count; r++)
                   {
                       if (mouseRectinBackground.Intersects(rectBackgroundItems[r]) && mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released)
                       {
                           backgroundItemShop.setDarkBackground(true);
                           //메시지 박스 띄우기  
                           selectedItem = shopBackgroundItems[r];
                           //이미 산거이면 true
                           if (backgroundItemShop.haveOne(shopBackgroundItems[r]))
                           {
                               backgroundItemShop.setWearOne(true);
                           }
                           else
                           {
                               backgroundItemShop.setBuyOne(true);
                           }
                       }
                   }


                   //돈 부족 메시지 
                   if (backgroundItemShop.getNoGold())
                   {
                       if (mouseRectinBackground.Intersects(backgroundItemShop.getRectNoGoldButton()))
                       {

                           backgroundItemShop.setHoverNoGoldButton(true);
                           if (mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released)
                           {

                               backgroundItemShop.setDarkBackground(false);

                               backgroundItemShop.setNoGold(false);
                           }

                       }
                       else
                       {
                           backgroundItemShop.setHoverNoGoldButton(false);
                       }


                   }



                   //message box about wearing item 
                   if (backgroundItemShop.getWearOne())
                   {
                       //mouse cursor on yes button
                       if (mouseRectinBackground.Intersects(backgroundItemShop.getRectYesButton()))
                       {
                           backgroundItemShop.setHoverYesButton(true);
                           if (mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released)
                           {
                               if (selectedItem != null)
                               {
                                   //find the index of item in myleftitem list
                                   int index = itemManager.getIndexOfAllBackgroundItem(selectedItem);
                                   if (index != -1)
                                   {
                                       //change index
                                       itemManager.setBackgroundIndex(index);

                                       //return to normal , remove message box
                                       backgroundItemShop.setWearOne(false);
                                       backgroundItemShop.setDarkBackground(false);
                                   }
                                   else
                                   {
                                       //get wrong index( no item in list)
                                   }
                               }
                               else
                               {
                                   //nothing is selected
                               }
                           }
                       }
                       else
                       {
                           backgroundItemShop.setHoverYesButton(false);
                       }
                       //mouse cursor on no button
                       if (mouseRectinBackground.Intersects(backgroundItemShop.getRectNoButton()))
                       {
                           backgroundItemShop.setHoverNoButton(true);
                           if (mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released)
                           {
                               //return to normal , remove message box
                               backgroundItemShop.setWearOne(false);
                               backgroundItemShop.setDarkBackground(false);
                           }
                       }
                       else
                       {
                           backgroundItemShop.setHoverNoButton(false);
                       }
                   }
                   //message box about buying item
                   if (backgroundItemShop.getBuyOne())
                   {
                       //mouse cursor on right button

                       if (mouseRectinBackground.Intersects(backgroundItemShop.getRectYesButton()))
                       {
                           backgroundItemShop.setHoverYesButton(true);
                           if (mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released)
                           {
                               //add item to my item
                               if (selectedItem != null)
                               {

                                   //돈으로 물건사기 
                                   //buy item with money


                                   //돈이 충분히 있다.
                                   if (scoreManager.TotalGold >= selectedItem.GetCost())
                                   {

                                       //전체 돈에서 구입비용 차감
                                       scoreManager.TotalGold -= selectedItem.GetCost();

                                       backgroundItemShop.addItemtoMyItem(selectedItem);
                                       //return to normal , remove message box
                                       backgroundItemShop.setBuyOne(false);
                                       backgroundItemShop.setDarkBackground(false);
                                       //돈을 파일에 저장
                                       reportManager.SaveGoldToFile();
                                   }
                                   //돈이 없다.
                                   else
                                   {

                                       backgroundItemShop.setBuyOne(false);
                                       //"돈 부족" 메시지 띄운다.
                                       backgroundItemShop.setNoGold(true);


                                   }

                               }
                               else
                               {
                                   //nothing is selected
                               }
                           }
                       }
                       else
                       {
                           backgroundItemShop.setHoverYesButton(false);
                       }
                       if (mouseRectinBackground.Intersects(backgroundItemShop.getRectNoButton()))
                       {
                           backgroundItemShop.setHoverNoButton(true);
                           if (mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released)
                           {
                               //return to normal , remove message box
                               backgroundItemShop.setBuyOne(false);
                               backgroundItemShop.setDarkBackground(false);
                           }
                       }
                       else
                       {
                           backgroundItemShop.setHoverNoButton(false);
                       }
                   }
                   break;

               #endregion

               #region 플레이화면
               case GameStates.Playing:

                   //곡이 끝내게 되면 결과 화면으로
                   //go to result scene right after finishing a piece
                   if (file.GetEndFile())
                   {
                       //노래 총 시간으로 끝을 바꾸자
                       gameState = GameStates.ResultManager;
                       //점수기록판에 기재

                       //reportManager의 scoreInfoManager에 곡명과 자기사진 추가
                       //여기에 현재 자신의 사진 이름이 들어가야 함.(날짜시간 포함해서 독립적으로)
                       reportManager.AddSongInfoManager(scoreManager.SongName, scoreManager.TotalScore, "myPicture.jpg");
                       
                       //현재 노래 제목
                       currentSongName = scoreManager.SongName;
                       
                       //점수 기록 파일로 저장
                       //save recored scores in the file
                       reportManager.SaveReport();

                       //gold 파일에  저장
                       scoreManager.TotalGold += scoreManager.Gold;
                       reportManager.SaveGoldToFile();
                       
                       //기록판에 보여줄 유저 사진 찾기
                       //Fine user pictures which will be seen in score board
                       reportManager.MakePictures(currentSongName, GraphicsDevice);
                       
                       //Texture2D texture = null; 
                       //Stream str = System.IO.File.OpenWrite("gesture.jpg");
                       //texture.SaveAsJpeg(str, 1200, 900);

                   }

                   //마크 업데이트
                    MarkManager.Update(gameTime);
                    startNoteManager.Update(gameTime);
                    HandleKeyboardInput(Keyboard.GetState());
                    HandleMouseInput(Mouse.GetState());
                    file.Update(spriteBatch, gameTime, this.changedTempo, this.optionalTime);
                    DragNoteManager.Update(gameTime);
                    GoldManager.Update(gameTime);
                    perfectManager.Update(gameTime);
                    goodManager.Update(gameTime);
                    badManager.Update(gameTime);
                    goldGetManager.Update(gameTime);
                    scoreManager.Update(gameTime);
                    memberManager.Update(gameTime);            
                    StartChangedTime(gameTime);
#if Kinect
                    HandleInput();
#endif
                    //3초만에 원상복귀
                    //       AutoRetrunChangeTempo(gameTime);

                break;

               #endregion

               #region 결과 결산
               //결과 창
                case GameStates.ResultManager:

                    Rectangle rectMouse = new Rectangle(mouseStateCurrent.X, mouseStateCurrent.Y, 5, 5);

                    //nextButton 위에 마우스를 올려놨을 때
                    //mousecursor on nextButton item section
                    if (rectMouse.Intersects(resultManager.getRectNextButton()))
                    {
                        resultManager.setClickNextButton(true);
                        //click the right hand item section
                        if (mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released)
                        {
                            gameState = GameStates.RecordBoard;

                           
                            //현재 마커 위치 저장
                            Vector2 mark1Location = MarkManager.Marks[0].MarkSprite.Location;
                            Vector2 mark2Location = MarkManager.Marks[1].MarkSprite.Location;
                            Vector2 mark3Location = MarkManager.Marks[2].MarkSprite.Location;
                            Vector2 mark4Location = MarkManager.Marks[3].MarkSprite.Location;
                            Vector2 mark5Location = MarkManager.Marks[4].MarkSprite.Location;
                            Vector2 mark6Location = MarkManager.Marks[5].MarkSprite.Location;
                            
                            
                            //두번째꺼 재실행시 이상한거 생기는것 방지
                            startNoteManager = new StartNoteManager(
                                spriteSheet,
                                new Rectangle(0, 200, 52, 55),
                                1);


                            //froze 방지
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
                            //파일 저장
                            file = new File(startNoteManager, noteFileManager, badManager, scoreManager);

                            

                            if (!System.IO.File.Exists(songsDir))
                            {
                                System.IO.Directory.CreateDirectory(songsDir);
                            }

                            file.FileLoading(songsDir, "*.mnf");
                           
                            scoreManager.init();

                          }
                    }
                    else
                    {
                        resultManager.setClickNextButton(false);
                    }
                break;
                #endregion

               #region 순위판 
                case GameStates.RecordBoard:

                    Rectangle rectMouseRecordBoard = new Rectangle(mouseStateCurrent.X, mouseStateCurrent.Y, 5, 5);
                   //nextButton 위에 마우스를 올려놨을 때
                    //mousecursor on nextButton item section
                    

                    if (rectMouseRecordBoard.Intersects(recordBoard.getRectNextButton()))
                    {
                        recordBoard.setClickNextButton(true);
                        //click the right hand item section
                        if (mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released)
                        {
                            gameState = GameStates.SongMenu;
                        }
                    }
                    else
                    {
                        recordBoard.setClickNextButton(false);
                    }
                break;
                #endregion
               
               #region 곡선택메뉴
                case GameStates.SongMenu:
                resultSongMenu = songMenu.Update();

                //뒤로가기
                if (resultSongMenu == -1)
                    {
                        gameState = GameStates.Menu;

                    }
                // 선택 되었음
                else if (resultSongMenu != -2)
                    {

                        //*** 롱노트와 왼손노트에도 다른 텍스쳐를 입혀야 함.
                        ////각 아이템에 따른 텍스쳐 변경

                        Texture2D[] rightNoteTextures = itemManager.GetNoteTexture();

                        ////왼손 노트 // 일단은 오른손노트랑 같이 함.
                        Texture2D[] leftNoteTextures = itemManager.GetNoteTexture();
                        ////롱노트 // 일단은 오른손노트랑 같이 함.
                        Texture2D[] longNoteTextures = itemManager.GetNoteTexture();

                        ////노트 - 달라야 될때도 있어서 나누어 놨다.
                        ////오른손노트

                        //노트 맞는 scale 
                        float[] rightNoteScale = itemManager.GetRightNoteScale();
                        float[] leftNoteScale = itemManager.GetRightNoteScale();
                        float[] longNoteScale = itemManager.GetRightNoteScale();
                       
                        //오른손 노트 이미지 바꾸기
                        startNoteManager.changeRightNoteImage(rightNoteTextures[itemManager.getNoteIndex()], new Rectangle(0, 0, rightNoteTextures[itemManager.getNoteIndex()].Width, rightNoteTextures[itemManager.getNoteIndex()].Height), rightNoteScale[0]);

                        
                        ////롱노트 //*** 임시로 오른손노트랑 똑같은걸로 해놓음
                        startNoteManager.changeLongNoteImage(longNoteTextures[itemManager.getNoteIndex()], new Rectangle(0, 0, longNoteTextures[itemManager.getNoteIndex()].Width, longNoteTextures[itemManager.getNoteIndex()].Height), longNoteScale[0]);

                        ////왼손노트
                        startNoteManager.changeLeftNoteImage(leftNoteTextures[itemManager.getNoteIndex()], new Rectangle(0, 0, leftNoteTextures[itemManager.getNoteIndex()].Width, leftNoteTextures[itemManager.getNoteIndex()].Height), leftNoteScale[0]);

                        //마커리스트 텍스쳐 가져오기
                        Texture2D[] markersTextures = itemManager.GetMarkerTexture();
                        
                        //*** 마커리스트에 맞는 rect, 바로width와 height 를 가져와서 넣기 때문에 필요 없을 수 도 있다.
                        //Rectangle[] markersRectangle = new Rectangle[5];
                        //markersRectangle[0] = new Rectangle(0,0,265,240);

                        //***마커리스트에 맞는 =>>itemManager로 옮김
                        float[] markersScale = itemManager.GetMarkersScale();
                        

                        //현재 장착한 마커로 설정//(마커,마커의 rect크기. scale)
                        MarkManager.chageMarksImages(markersTextures[itemManager.getNoteIndex()], new Rectangle(0,0,markersTextures[itemManager.getNoteIndex()].Width,markersTextures[itemManager.getNoteIndex()].Height), markersScale[0]);

                        /////이펙트 생성 -START
                        Texture2D[] explosionTexture = itemManager.GetEffectTexture();
                        
                        //특성 로드
                        Rectangle[] effectInitFrame = itemManager.GetEffectInitFrame();
                        int[] effectFramCount = itemManager.GetEffectFrameCount();
                        float[] effecScale = itemManager.GetEffectScale();

        
                        //이펙트 
                        int effectIndex = itemManager.getEffectIndex();

                        perfectManager = new ExplosionManager();
                        perfectManager.ExplosionInit(itemManager.GetEffectTexture()[effectIndex], itemManager.GetEffectInitFrame()[effectIndex], itemManager.GetEffectFrameCount()[effectIndex], itemManager.GetEffectScale()[effectIndex], itemManager.GetEffectDulation()[effectIndex]);

                        goodManager = new ExplosionManager();
                        goodManager.ExplosionInit(itemManager.GetGoodEffectTexture()[effectIndex], itemManager.GetEffectInitFrame()[effectIndex], itemManager.GetEffectFrameCount()[effectIndex], itemManager.GetEffectScale()[effectIndex], itemManager.GetEffectDulation()[effectIndex]);

                        badManager = new ExplosionManager();
                        badManager.ExplosionInit(itemManager.GetBadEffectTexture()[effectIndex], itemManager.GetEffectInitFrame()[effectIndex], itemManager.GetEffectFrameCount()[effectIndex], itemManager.GetEffectScale()[effectIndex], itemManager.GetEffectDulation()[effectIndex]);

                        //미스도 투입되면
                        //missManager = new ExplosionManager();
                        //missManager.ExplosionInit(missEffectTextures[effectIndex], new Rectangle(0, 0, 166, 162), 9, 1f, 45);


                        //일단은 miss effect로
                        goldGetManager = new ExplosionManager();
                        goldGetManager.ExplosionInit(itemManager.GetMissEffectTexture()[effectIndex], itemManager.GetEffectInitFrame()[effectIndex], itemManager.GetEffectFrameCount()[effectIndex], itemManager.GetEffectScale()[effectIndex], itemManager.GetEffectDulation()[effectIndex]);
  
                        collisionManager = new CollisionManager(perfectManager, goodManager, badManager, goldGetManager, scoreManager, memberManager,itemManager);
            
                        /////이펙트 생성 -END
                        
                        gameState = GameStates.Playing;
                        file.Loading(resultSongMenu);
                    
                    
                    
                    
                    //노래찾아서 재생하기    
                    //*** 재생시간동안 로딩
                   
                        sndSystem.createSound(songsDir + noteFileManager.noteFiles[resultSongMenu].Mp3, FMOD.MODE.HARDWARE, ref sndSound);
                        sndSystem.playSound(CHANNELINDEX.FREE, sndSound, false, ref sndChannel);
                    }

                    break;
               #endregion
           }
           mouseStatePrevious = mouseStateCurrent;
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
            
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();
            //타이틀화면
            if (gameState == GameStates.Menu)
            {
                menuScene.Draw(spriteBatch, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height);

            }

            //상전대문
            if (gameState == GameStates.ShopDoor)
            {
                shopDoor.Draw(spriteBatch, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height);
            }

            #region 아이템샵들
            if (gameState == GameStates.RightItemShop)
            {
                rightItemShop.Draw(spriteBatch, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height);
            }

            if (gameState == GameStates.LeftItemShop)
            {
                leftItemShop.Draw(spriteBatch, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height);
            }


            if (gameState == GameStates.EffectItemShop)
            {
                effectItemShop.Draw(spriteBatch, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height);
            }

            if (gameState == GameStates.NoteItemShop)
            {
                noteItemShop.Draw(spriteBatch, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height);
            }


            if (gameState == GameStates.BackgroundItemShop)
            {
                backgroundItemShop.Draw(spriteBatch, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height);
            }
            #endregion

            if (gameState == GameStates.SongMenu)
            {
                songMenu.Draw(spriteBatch);

            }

            #region 플레이화면
            if ((gameState == GameStates.Playing))
            {
                //배경
                spriteBatch.Draw(playBackgroud1,
                new Rectangle(0, 0, this.Window.ClientBounds.Width,
                    this.Window.ClientBounds.Height),
                    Color.White);

                MarkManager.Draw(spriteBatch);
                
                //startnoteclass에 가야 보이고 안보이게 할 수 있음
                startNoteManager.Draw(spriteBatch);
                CurveManager.Draw(gameTime, spriteBatch);
                GuideLineManager.Draw(gameTime, spriteBatch);
                
                //이걸 주석하면 드래그노트 체크하는거 안보임 하지만 체크는 됨
                //DragNoteManager.Draw(spriteBatch);
                
                memberManager.Draw(spriteBatch);
                GoldManager.Draw(spriteBatch);

                file.Draw(spriteBatch, gameTime);
                perfectManager.Draw(spriteBatch);
                goodManager.Draw(spriteBatch);
                badManager.Draw(spriteBatch);
                goldGetManager.Draw(spriteBatch);
                
                //가운데 빨간 사각형 주석하면 보이지않는다.
                //     spriteBatch.Draw(dot, removeAreaRec, Color.Red);
                
                //콤보 글씨
                spriteBatch.DrawString(pericles36Font, scoreManager.Combo.ToString(), new Vector2(512, 420), Color.Black);
                //골드 글씨
                spriteBatch.DrawString(pericles36Font, scoreManager.Gold.ToString(), new Vector2(scorePosition.X + 120, scorePosition.Y), Color.Black);
                //최대 max
                spriteBatch.DrawString(pericles36Font, scoreManager.Max.ToString(), new Vector2(scorePosition.X + 240, scorePosition.Y), Color.Black);

                //기본 템포 설정( 템포가 바뀐상태이면 안변함)
                SetBasicTempo();

                //512, 454 중심
                ////test

                spriteBatch.Draw(uiBackground, new Vector2(0, 0), Color.White);
                
                int gage = scoreManager.Gage;
                //0이하이거나 넘어가지 않게 

                //하트. gage양 만큼 하트가 나타남.
                spriteBatch.Draw(uiHeart, new Vector2(0, 6), new Rectangle(0, 0, gage, 50), Color.White);
              
  
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
            #endregion

            if (gameState == GameStates.ResultManager)
            {
                //class resultManager 에 draw실행
                resultManager.Draw(spriteBatch, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height,reportManager.IsHighScore(currentSongName,scoreManager.TotalScore));

                //노래제목
                String songFile = scoreManager.SongName;
                //제목으로 노트파일에서 찾기
                NoteFile noteFile = noteFileManager.FindNoteFile(songFile);
                
                //노트파일로 사진 가져오기
                spriteBatch.Draw(songMenu.FindPicture(noteFile), new Rectangle(70, 70, 100, 100), Color.White);
                
                //노트파일로 노래 제목가져오기
                spriteBatch.DrawString(pericles36Font, noteFile.Name, new Vector2(200,80), Color.White);
                //노트파일로 가수 가져오기
                spriteBatch.DrawString(pericles36Font, noteFile.Artist, new Vector2(200,130), Color.White);
                //***//난이도//spriteBatch.DrawString(pericles36Font, , new Vector2(200,80), Color.White);

                spriteBatch.DrawString(pericles36Font, scoreManager.Perfect.ToString(), new Vector2(300, 300), Color.White);
                spriteBatch.DrawString(pericles36Font, scoreManager.Good.ToString(), new Vector2(300, 350), Color.White);
                spriteBatch.DrawString(pericles36Font, scoreManager.Bad.ToString(), new Vector2(300, 400), Color.White);
                spriteBatch.DrawString(pericles36Font, scoreManager.Perfomance.ToString(), new Vector2(700, 300), Color.White);
                spriteBatch.DrawString(pericles36Font, scoreManager.Max.ToString(), new Vector2(700, 350), Color.White);
                spriteBatch.DrawString(pericles36Font, scoreManager.Combo.ToString(), new Vector2(700, 400), Color.White);
                spriteBatch.DrawString(pericles36Font, scoreManager.Gold.ToString(), new Vector2(700, 450), Color.White);
                spriteBatch.DrawString(pericles36Font, scoreManager.TotalScore.ToString(), new Vector2(700, 500), Color.White);
            }


            if (gameState == GameStates.RecordBoard)
            {
                recordBoard.Draw(spriteBatch, this. Window.ClientBounds.Width, this.Window.ClientBounds.Height);

                //현재 노래 제목으로 5개 높은 노래 가져오기
                List<ScoreInfo> highScores = reportManager.GetHighScore(currentSongName);

                int i;
                for (i = 0; i < highScores.Count; i++)
                {
                    //노래 사진
                    spriteBatch.Draw(reportManager.FindPicture(highScores[i].UserPicture), new Rectangle(200, (i + 1) * 100, 100, 100), Color.White);
                    
                    spriteBatch.DrawString(pericles36Font, highScores[i].Score.ToString(), new Vector2(300, (i+1)*100), Color.Black);
                }

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

            //이게 조금 느리다면 static으로 해서 개선 
            //임시로 상점에 있는거 썼다.

            List<Item> myRightHand = itemManager.getShopRightHandItem();
            spriteBatch.Draw(myRightHand[itemManager.getRightHandIndex()].ItemSprite.Texture, drawrec1, Color.Red);

            Joint j2r = j2.ScaleTo(1024, 768, .3f, .3f);

            drawrec2.X = (int)j2r.Position.X - drawrec2.Width / 2;
            drawrec2.Y = (int)j2r.Position.Y - drawrec2.Height / 2;

            //ColorImagePoint jp2 = nui.MapSkeletonPointToColor(j2.Position, ColorImageFormat.RgbResolution640x480Fps30);
            //drawrec2.X = jp2.X - drawrec2.Width / 2;
            //drawrec2.Y = jp2.Y - drawrec2.Height / 2;

            List<Item> myLeftHand = itemManager.getShopLeftHandItem();
            spriteBatch.Draw(myLeftHand[itemManager.getLeftHandIndex()].ItemSprite.Texture, drawrec2, Color.Blue);
        }
        #endregion
#endif
       
    }
}