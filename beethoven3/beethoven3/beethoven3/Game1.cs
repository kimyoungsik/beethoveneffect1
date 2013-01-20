﻿#define Kinect

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using FMOD;
using Microsoft.Kinect;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Coding4Fun.Kinect.Wpf;
using System.IO;
using System.Threading;
using Microsoft.Samples.Kinect.SwipeGestureRecognizer;
using Microsoft.Speech.Recognition;
using Microsoft.Speech.AudioFormat;

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


     

        //스코어 보드에 쓰이는 사진
        private String ScorePic;
        private bool isScorePic = false;


        private Queue playingPictures;
        private int playPicturesCount = 0;
        Texture2D[] showPictureTextures = new Texture2D[5];


#if Kinect
        //화면에 띄우기
        Texture2D KinectVideoTexture;
        Rectangle VideoDisplayRectangle;


        //오른손 좌표
        
        public static Joint j1r;
        public static Joint j2r;


        public static Rectangle drawrec1;
        public static Rectangle drawrec2;
        public static bool finalClick;
        public static bool pastClick = false;


        //주변 버튼 여부
        public static bool nearButton = false;
        
        //버튼 센터 

        public static Vector2 center = new Vector2(0, 0);
        
        //키넥트
        KinectSensor nui = null;
        
        Skeleton[] Skeletons = null;
        
        Skeleton skeleton = null;
        //스켈레톤 한명만
        int CurrentTrackingId = 0;
 
        //음성인식
        SpeechRecognitionEngine sre;
        RecognizerInfo ri;
        KinectAudioSource source;
        Stream audioStream;
        double skeletonAngle;

        private String kinectMessage = "__UNKNOWN";
        //쓰레드
        ThreadStart ts;
        Thread th;

        ThreadStart ts1;
        Thread th1;

        ThreadStart ts2;
        Thread th2;

        ThreadStart ts3;
        Thread th3;

        ThreadStart ts4;
        Thread th4;

        //제스쳐
        private Recognizer activeRecognizer;

        //사진찍기
        Texture2D CapturePic;
        public static bool PicFlag = false;

        //일반 제스쳐
        private const int MinimumFrames = 6;
        private const int BufferSize = 32;
        private int _flipFlop;
        private ArrayList _video;
        
        

        private DtwGestureRecognizer _dtw1;
        private DtwGestureRecognizer _dtw2;
        private DtwGestureRecognizer _dtw3;

        int postureCount = 0;
        bool gestureFlag = true;//제스쳐 동작이 수행됐는지 안됐는지
        int gestureType = 1;//정지 포스쳐인지 일반 포스쳐인지 제스쳐인지 (1 : 정지 포스쳐, 2 : 일반 포스쳐, 3 : 제스쳐)
        bool isGesture = true;
        //머리찾기
        float fy;
        double bestFy = 1000;
        int bestAngle = 0;
        

        //키재기
        float fheadY;
        float fhipY;
        float fcenterZ;
        double factheight;

        //폰트
        SpriteFont messageFont;
        string message = "start";

        //손
        DepthImagePoint handPoint;
        short[] ImageBits;
        ColorImagePoint[] depthLocation;

        //사람 키에 따른 미세조정 파라미터
        float userParam = .3f;

        //손가락 클릭
        private void skip() { }
        public delegate void afterReady();
        private afterReady afterColorReady;
        private afterReady afterDepthReady;
        public KinectSettings settings { get; set; }
        public List<Hand> hands { get; set; }
        int listCount = 0;
        bool clickJudge = false;
        List<DepthImagePoint> handList = new List<DepthImagePoint>();
        List<bool> clickList = new List<bool>();
        
#endif
        //기본 글꼴
        //basic font
        public static SpriteFont pericles36Font;


        public enum GameStates { Menu, Playing, SongMenu, ShopDoor,
                          RightItemShop, LeftItemShop, EffectItemShop, NoteItemShop, BackgroundItemShop,
                          ResultManager, RecordBoard, ShowPictures, SettingBoard };


        //게임 씬, 처음시작은 메뉴
        public static GameStates gameState = GameStates.Menu;

        //타이틀 화면 
        private MenuScene menuScene;

        //상점 화면
        private ShopDoor shopDoor;
        
        //곡 선택 화면
        private SongMenu songMenu;

        //선택된 곡
        private int resultSongMenu;

        //곡을 음성인식으로 선택했을때 리턴받는 값을 저장하는 변수
        public static int soundRecogStartIndex = -1;
        
        //마지막 순위 리스트 보여주는 화면 
        private RecordBoard recordBoard;


        private ShowPictureScene showPictureScene;

        //노트 생성 부분 관리
        private StartNoteManager startNoteManager;
        
        //충돌 관리
        private CollisionManager collisionManager;

        //세팅
        private SettingBoard settingBoard;

        
        //악보파일 관리(불러오기 등)
        private File file;

        /////이펙트 관리 - start
        private PerfectExplosionManager perfectManager;
        private GoodExplosionManager goodManager;
        private BadExplosionManager badManager;
        private ExplosionManager goldGetManager;
        /////이펙트 관리 - end
        
       //판정 배너 띄우기 

        private PerfectBannerManager perfectBannerManager;
        private GoodBannerManager goodBannerManager;
        private BadBannerManager badBannerManager;
        private MissBannerManager missBannerManager;


        //콤보 숫자
        ComboNumberManager comboNumberManager;

        //결과화면 숫자
        ResultNumberManager resultNumberManager;
        
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
        
        //마우스 상태 
        MouseState mouseStateCurrent, mouseStatePrevious;

        //노래들이 있는 경로
        String songsDir = System.Environment.CurrentDirectory+"\\beethovenSong\\";
        
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

       //드래그 라인 모양
        public static Texture2D drawLineNote1;

     
        //perfect,good,bad,miss 판정 배너
        private Texture2D perfectBanner;

        private Texture2D goodBanner;

        private Texture2D badBanner;

        private Texture2D missBanner;


        //gold
        private Texture2D gold;

        private Texture2D getGold;

        //오른손 텍스쳐들

        private Texture2D[] rightHandTextures;

        private Texture2D[] leftHandTextures;

        //사람 없음 나타내는 메시지 박스

        private Texture2D noPerson;


        public static Texture2D previousButton;
        public static Texture2D hoverPreviousButton;
        //float volume = 0;

        /////Texture end 

        private CharismaManager charismaManager;


        private PhotoManager photoManager;
        //드래그 라인 안의 마커점

        public static Texture2D drawLineMarker;



        public static Texture2D one;
        public static Texture2D two;
        public static Texture2D three;


        //드래그 라인의 렌더링
        LineRenderer dragLineRenderer;
 
        //드래그 라인안의 마커점 렌더링

        LineRenderer dragLineMarkerRenderer;
        
        //드래그라인 파일에서 뽑아낼떄 CurveManager만듬
        CurveManager curveManager;

        //가이드 라인의 렌더링
        LineRenderer guideLineRenderer;

        GuideLineManager guideLineManager;


    //    StaticSprite comboNumber;

        /////키넥트 관련 선언 - START
        //for kinect
        public static Texture2D idot;

        private Item selectedItem;


        private bool isNoPerson;


      //  private bool isCharisma1;

        private Texture2D[] metronomes;
#if Kinect
       


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

//#if Kinect
            
            //KINECT
            VideoDisplayRectangle = new Rectangle(0, 0, SCR_W, SCR_H);

            //drawrec1 = new Rectangle(0, 0, GraphicsDevice.Viewport.Width / 20, GraphicsDevice.Viewport.Height / 20);
            //drawrec2 = new Rectangle(0, 0, GraphicsDevice.Viewport.Width / 20, GraphicsDevice.Viewport.Height / 20);
            drawrec1 = new Rectangle(0, 0, 5, 5);
            drawrec2 = new Rectangle(0, 0, 5, 5);


//#endif
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
            ///***
            ////FMOD 세팅 -START
            //resultFmod = FMOD.Factory.System_Create(ref sndSystem);
            //sndSystem.init(1, FMOD.INITFLAG.NORMAL, (IntPtr)null);
            ////FMOD 세팅 -END
           
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

            /////텍스쳐 로드 -START
            //배경
            playBackgroud1 = Content.Load<Texture2D>(@"background\park");
            playBackgroud2 = Content.Load<Texture2D>(@"background\crosswalk3");
            
            //노트,마커
            spriteSheet = Content.Load<Texture2D>(@"Textures\SpriteSheet8");
            
            //드래그 노트
           // heart = Content.Load<Texture2D>(@"Textures\heart");
           
            //진행상황
            uiBackground = Content.Load<Texture2D>(@"ui\background");
            uiHeart = Content.Load<Texture2D>(@"ui\heart");

            //골드 업어
            getGold = Content.Load<Texture2D>(@"gold\getGold");
            //폰트
            pericles36Font = Content.Load<SpriteFont>(@"Fonts\Pericles36");

            
            //perfect,good,bad,miss 판정 배너 
            perfectBanner = Content.Load<Texture2D>(@"judgement\perfect");

            goodBanner = Content.Load<Texture2D>(@"judgement\good");

            badBanner = Content.Load<Texture2D>(@"judgement\bad");

            missBanner = Content.Load<Texture2D>(@"judgement\miss");


            noPerson = Content.Load<Texture2D>(@"shopdoor\nogold");



            rightHandTextures = itemManager.GetRightHandTexture();
            leftHandTextures = itemManager.GetLeftHandTexture();
            
            //드래그 노트 셀떄

            one = Content.Load<Texture2D>(@"comboNumbers\good1");
            two = Content.Load<Texture2D>(@"comboNumbers\good2");
            three = Content.Load<Texture2D>(@"comboNumbers\good3");


            previousButton = Content.Load<Texture2D>(@"game1\previousButton");
            hoverPreviousButton = Content.Load<Texture2D>(@"game1\previousButton");
            
            //   charisma1 = Content.Load<Texture2D>(@"shopdoor\nogold");
            /////텍스쳐 로드 -END

         
            /////////////////드래그 라인 관련

           
            

            //드래그 라인
            drawLineNote1 = Content.Load<Texture2D>(@"DrawLine\drawLineNote1");

            //드래그 라인 마커점
            drawLineMarker = Content.Load<Texture2D>(@"DrawLine\drawLineMark");



            gold = Content.Load<Texture2D>(@"gold\gold");


            //로드하고, 기본적으로 넣어줌
            itemManager.LoadFileItem();


            //현재 장착한 이펙트의 인덱스를 전체 베이스에 찾음
            int effectIndex = itemManager.getEffectIndex();

            //***임시로 - 아래에 바로 넣었음
            //텍스쳐 가져오기
            //Texture2D[] effectTextures = itemManager.GetEffectTexture();
            //Texture2D[] goodEffectTextures = itemManager.GetGoodEffectTexture();
            //Texture2D[] badEffectTextures = itemManager.GetBadEffectTexture();
            //Texture2D[] missEffectTextures = itemManager.GetMissEffectTexture();

            //콤보 숫자
            comboNumberManager = new ComboNumberManager();
            comboNumberManager.LoadContent(Content);

            //결과화면 숫자
            resultNumberManager = new ResultNumberManager();
            resultNumberManager.LoadContent(Content);




          //  comboNumber = new StaticSprite(new Vector2(0, 0), comboNumberTexture, new Rectangle(0, 0, 154, 200), Vector2.Zero, 1f);

            //드래그 라인 렌더링
            dragLineRenderer = new LineRenderer();


            //드래그 라인 안의 마커점의 렌더링 
            dragLineMarkerRenderer = new LineRenderer();

            //드래그라인 
            curveManager = new CurveManager(dragLineRenderer, dragLineMarkerRenderer);

            //가이드 라인 렌더링

            guideLineRenderer = new LineRenderer();

            //가이드 라인 
            guideLineManager = new GuideLineManager(guideLineRenderer);

            //텍스쳐 크기
            ///////이펙트 생성 -START
            

            //폭발 효과
            perfectManager = new PerfectExplosionManager();
            perfectManager.ExplosionInit(itemManager.GetEffectTexture()[effectIndex], itemManager.GetEffectInitFrame()[effectIndex], itemManager.GetEffectFrameCount()[effectIndex], itemManager.GetEffectScale()[effectIndex], itemManager.GetEffectDulation()[effectIndex]);

            goodManager = new GoodExplosionManager();
            goodManager.ExplosionInit(itemManager.GetGoodEffectTexture()[effectIndex], itemManager.GetEffectInitFrame()[effectIndex], itemManager.GetEffectFrameCount()[effectIndex], itemManager.GetEffectScale()[effectIndex], itemManager.GetEffectDulation()[effectIndex]);

            badManager = new BadExplosionManager();
            badManager.ExplosionInit(itemManager.GetBadEffectTexture()[effectIndex], itemManager.GetEffectInitFrame()[effectIndex], itemManager.GetEffectFrameCount()[effectIndex], itemManager.GetEffectScale()[effectIndex], itemManager.GetEffectDulation()[effectIndex]);


            //판정 글씨 나타내기 
            perfectBannerManager = new PerfectBannerManager();
            perfectBannerManager.BannerInit(perfectBanner,new Rectangle(0,0,1380,428),/*sprite로 바꾸면 frameCount바꾸기*/1,0.5f,30);


            goodBannerManager = new GoodBannerManager();
            goodBannerManager.BannerInit(goodBanner, new Rectangle(0, 0, 1020, 368),/*sprite로 바꾸면 frameCount바꾸기*/1, 0.5f, 30);


            badBannerManager = new BadBannerManager();
            badBannerManager.BannerInit(badBanner, new Rectangle(0, 0, 782, 400),/*sprite로 바꾸면 frameCount바꾸기*/1, 0.7f, 30);

            missBannerManager = new MissBannerManager();
            missBannerManager.BannerInit(missBanner, new Rectangle(0, 0, 975, 412),/*sprite로 바꾸면 frameCount바꾸기*/1, 0.5f,30);


            //미스도 투입되면
            //missManager = new ExplosionManager();
            //missManager.ExplosionInit(missEffectTextures[effectIndex], new Rectangle(0, 0, 166, 162), 9, 1f, 45);


            //일단은 miss effect로
            goldGetManager = new ExplosionManager();
            goldGetManager.ExplosionInit(getGold,new Rectangle(0,0,200,200), 5, 1.0f, 40);

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

        //    removeAreaRec = MarkManager.GetRemoveArea(0);

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
                startNoteManager
                
                );

           

            //충돌관리 생성
            collisionManager = new CollisionManager(perfectManager, goodManager, badManager, goldGetManager, scoreManager, memberManager,/*effect크기*/itemManager,perfectBannerManager,goodBannerManager,badBannerManager,missBannerManager,new Vector2(this.Window.ClientBounds.Width,this.Window.ClientBounds.Height),comboNumberManager,charismaManager);
            
            //노트정보 관리 생성
            noteFileManager = new NoteFileManager();
            //드래그 라인 렌더링
            dragLineRenderer = new LineRenderer();

            dragLineMarkerRenderer = new LineRenderer();
            //드래그라인 
            curveManager = new CurveManager(dragLineRenderer, dragLineMarkerRenderer);

            //카리스마 매니저
            charismaManager = new CharismaManager();
            charismaManager.LoadContent(Content);

            photoManager = new PhotoManager();
            photoManager.LoadContent(Content);


            //노트파일 읽기 관리 생성
            file = new File(startNoteManager, noteFileManager, collisionManager, scoreManager, itemManager, curveManager, guideLineManager, charismaManager, photoManager);

            SoundFmod.initialize(file);
            //곡선택화면 곡 불러오는 폴더 
          
            //노래폴더 만들기 위해서 테스트
            String songFile = System.Environment.CurrentDirectory+"\\beethovenSong\\song.txt";
            String dir = System.Environment.CurrentDirectory + "\\beethovenSong";
            
            //노래폴더가 있느냐?
            if (!System.IO.File.Exists(songFile))
            {
                //노래 폴더 만들기
                System.IO.Directory.CreateDirectory(dir);
                var myFile = System.IO.File.Create(songFile);
                myFile.Close();
                
            }

            //기록 만들기 위해서 테스트
            String songRecordFile = System.Environment.CurrentDirectory + "\\beethovenRecord\\song.txt";
            String songRecorddir = System.Environment.CurrentDirectory + "\\beethovenRecord";

            //기록폴더가 있느냐?
            if (!System.IO.File.Exists(songRecordFile))
            {
                //기록 폴더 만들기
                System.IO.Directory.CreateDirectory(songRecorddir);
                var myFile = System.IO.File.Create(songRecordFile);
                myFile.Close();
            }


            
            //사진 폴더  위해서 테스트
            String picFile = System.Environment.CurrentDirectory + "\\beethovenRecord\\userPicture\\song.txt";
            String picddir = System.Environment.CurrentDirectory + "\\beethovenRecord\\userPicture";
            String defaultPicture = System.Environment.CurrentDirectory + "\\myPicture.jpg";
            //기록폴더가 있느냐?
            if (!System.IO.File.Exists(picFile))
            {
                //기록 폴더 만들기
                System.IO.Directory.CreateDirectory(picddir);
                var myFile = System.IO.File.Create(picFile);
                myFile.Close();

                FileInfo fileInfo = new FileInfo(defaultPicture);
                fileInfo.MoveTo(System.Environment.CurrentDirectory + "\\beethovenRecord\\UserPicture\\myPicture.jpg");
              

            }
            

            //곡을 불러오기
            file.FileLoading(dir, "*.mnf");
            
            //드래그노트 초기화
            //이것은 노트 안에서 움직이는 마커점
            DragNoteManager.initialize(
                 drawLineMarker,
                 new Rectangle(0, 0, 100, 100),
                 1,
                 15,
                 0,
                 missBannerManager,
                 scoreManager);

            //골드 초기화
            //크기 0.3
            GoldManager.initialize(
                gold,
                new Rectangle(0, 0, 200, 200),
                1,
                50,
                0,
                0.3f);
            
            
            //***
            songMenu = new SongMenu(noteFileManager);
            songMenu.Load(Content,graphics.GraphicsDevice);

            
            resultManager = new ResultManager();
            resultManager.LoadContent(Content);



            //점수기록판 화면
            recordBoard = new RecordBoard();
            recordBoard.LoadContent(Content);

            settingBoard = new SettingBoard(this);
            settingBoard.LoadContent(Content);


            showPictureScene = new ShowPictureScene();
            showPictureScene.LoadContent(Content);

            //점수 기록 (TO FILE)
            reportManager = new ReportManager(scoreManager);
            
           

            //LOAD REPORT SCORE FILE
            //점수기록판을 로드해서 게임에 올린다. 

            reportManager.LoadReport();

            //골드를 로드해서 게임에 올린다. 
            reportManager.LoadGoldFromFile();

            currentSongName = "";
           
           // itemManager.LoadFileItem();
#if Kinect
          
            messageFont = Content.Load<SpriteFont>("MessageFont");
            setupKinect();

            /*제스쳐 인식인데 
             전체 구간에서 계속 실행 된다. 
             *이것이 다른것에 영향을 줄 수도 있다. 
             */
            activeRecognizer = CreateRecognizer();

            //!!! 페이스 인식, 디버깅 시에 잠시 
            //ts4 = new ThreadStart(FaceDetect);
            //th4 = new Thread(ts4);
            //th4.Start();
           

         
#endif
            playingPictures = new Queue();
            showPictureTextures = new Texture2D[5];

            metronomes = new Texture2D[8];
            metronomes[0] = Content.Load<Texture2D>(@"metronome\metronome0");
            metronomes[1] = Content.Load<Texture2D>(@"metronome\metronome1");
            metronomes[2] = Content.Load<Texture2D>(@"metronome\metronome2");
            metronomes[3] = Content.Load<Texture2D>(@"metronome\metronome3");
            metronomes[4] = Content.Load<Texture2D>(@"metronome\metronome4");
            metronomes[5] = Content.Load<Texture2D>(@"metronome\metronome5");
            metronomes[6] = Content.Load<Texture2D>(@"metronome\metronome6");
            metronomes[7] = Content.Load<Texture2D>(@"metronome\metronome7");
        }


#if Kinect
        void FaceDetect()
        {
            bool maxFlg = false;
            bool minFlg = false;
            isNoPerson = false;
            if (nui.ElevationAngle >= 0)
            {
                nui.ElevationAngle = nui.MinElevationAngle;
                nui.ElevationAngle = nui.MaxElevationAngle;


                while (true)
                {
                    if (nui.ElevationAngle < nui.MinElevationAngle + 3)
                    {//각도가 다내려갈 때까지 아무것도 없으면
                        isNoPerson = true;
                        break;
                    }

                    if (fy > 0.2f && fy < 0.3f)
                    {
                        break;
                    }

                    try
                    {
                        nui.ElevationAngle -= 3;
                    }
                    catch (Exception ex)
                    {

                    }
                }
                if (isNoPerson)
                {
                    maxFlg = false;
                    minFlg = false;
                    while (true)
                    {
                        if (maxFlg == true)
                            break;
                        try
                        {
                            nui.ElevationAngle = nui.MaxElevationAngle;
                            maxFlg = true;
                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    while (true)
                    {
                        if (minFlg == true)
                            break;
                        try
                        {
                            nui.ElevationAngle = nui.MinElevationAngle;
                            minFlg = true;
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    
                    
                    isNoPerson = false;
                    while (true)
                    {
                        if (nui.ElevationAngle > nui.MaxElevationAngle - 3)
                        {//각도가 다내려갈 때까지 아무것도 없으면
                            isNoPerson = true;
                            break;
                        }

                        if (fy > 0.2f && fy < 0.3f)
                        {
                            break;
                        }

                        try
                        {
                            nui.ElevationAngle += 3;
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
                if (isNoPerson)
                {
                    maxFlg = false;
                    minFlg = false;
                    while (true)
                    {
                        if (minFlg == true)
                            break;
                        try
                        {
                            nui.ElevationAngle = nui.MinElevationAngle;
                            minFlg = true;
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    while (true)
                    {
                        if (maxFlg == true)
                            break;
                        try
                        {
                            nui.ElevationAngle = nui.MaxElevationAngle;
                            maxFlg = true;
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    
                    
                    isNoPerson = false;
                    while (true)
                    {
                        if (nui.ElevationAngle < nui.MinElevationAngle + 3)
                        {//각도가 다내려갈 때까지 아무것도 없으면
                            //프로그램 종료
                            isNoPerson = true;
                            break;
                        }

                        if (fy > 0.2f && fy < 0.3f)
                        {
                            break;
                        }

                        try
                        {
                            nui.ElevationAngle -= 3;
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }
            else
            {
                nui.ElevationAngle = nui.MaxElevationAngle;
                nui.ElevationAngle = nui.MinElevationAngle;

                while (true)
                {
                    if (nui.ElevationAngle > nui.MaxElevationAngle - 3)
                    {//각도가 다내려갈 때까지 아무것도 없으면
                        isNoPerson = true;
                        break;
                    }

                    if (fy > 0.3f && fy < 0.4f)
                    {
                        break;
                    }
                    try
                    {
                        nui.ElevationAngle += 3;
                    }
                    catch (Exception ex)
                    {

                    }
                }
                if (isNoPerson)
                {
                    maxFlg = false;
                    minFlg = false;
                    
                    while (true)
                    {
                        if (minFlg == true)
                            break;
                        try
                        {
                            nui.ElevationAngle = nui.MinElevationAngle;
                            minFlg = true;
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    while (true)
                    {
                        if (maxFlg == true)
                            break;
                        try
                        {
                            nui.ElevationAngle = nui.MaxElevationAngle;
                            maxFlg = true;
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    
                    isNoPerson = false;
                    while (true)
                    {
                        if (nui.ElevationAngle < nui.MinElevationAngle + 3)
                        {//각도가 다내려갈 때까지 아무것도 없으면
                            isNoPerson = true;
                            break;
                        }

                        if (fy > 0.3f && fy < 0.4f)
                        {
                            break;
                        }
                        try
                        {
                            nui.ElevationAngle -= 3;
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
                if (isNoPerson)
                {
                    maxFlg = false;
                    minFlg = false;
                    
                    while (true)
                    {
                        if (maxFlg == true)
                            break;
                        try
                        {
                            nui.ElevationAngle = nui.MaxElevationAngle;
                            maxFlg = true;
                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    while (true)
                    {
                        if (minFlg == true)
                            break;
                        try
                        {
                            nui.ElevationAngle = nui.MinElevationAngle;
                            minFlg = true;
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    
                    
                    isNoPerson = false;
                    while (true)
                    {
                        if (nui.ElevationAngle > nui.MaxElevationAngle - 3)
                        {//각도가 다내려갈 때까지 아무것도 없으면
                            //각도가 다내려갈 때까지 아무것도 없으면
                            isNoPerson = true;
                            break;
                        }

                        if (fy > 0.3f && fy < 0.4f)
                        {
                            break;
                        }
                        try
                        {
                            nui.ElevationAngle += 3;
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }

            }


            if (fy >= 180)
            {
                userParam = 0.4f;
            }
            else if (fy < 180 && fy >= 170)
            {
                userParam = 0.35f;
            }
            else if (fy < 170 && fy >= 160)
            {
                userParam = 0.30f;
            }
            else if (fy < 160 && fy >= 150)
            {
                userParam = 0.25f;
            }
            else
            {
                userParam = 0.3f;
            }


        }
#endif

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
#if Kinect
        protected bool setupKinect()
        {
            if (KinectSensor.KinectSensors.Count == 0)
            {
                //메시지 박스
                //종료

                return false;
            }

            //파라미터
            var parameters = new TransformSmoothParameters
            {
                //Smoothing = 0.3f,
                //Correction = 0.0f,
                //Prediction = 0.0f,
                //JitterRadius = 1.0f,
                //MaxDeviationRadius = 0.5f


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

                Smoothing = 0.7f,
                Correction = 0.3f,
                Prediction = 1.0f,
                JitterRadius = 1.0f,
                MaxDeviationRadius = 1.0f

                //Smoothing = 0.05f,
                //Correction = 0.5f,
                //Prediction = 0.5f,
                //JitterRadius = 0.8f,
                //MaxDeviationRadius = 0.2f
            };

            //키넥트 센서
            nui = KinectSensor.KinectSensors[0];

            try
            {
                nui.Start();
            }
            catch
            {
                nui = null;
                return false;
               //Exit();
            }

            //if (nui == null)
            //{
            //    //메시지 박스 
            //    //종료
                
            //}

            if (nui != null)
            {
                
                //스켈레톤 스트림
                nui.SkeletonStream.Enable(parameters);
                //nui.SkeletonStream.Enable();
                nui.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(nui_SkeletonFrameReady);

                //뎁스스트림
                nui.DepthStream.Enable();
                //nui.DepthFrameReady += new EventHandler<DepthImageFrameReadyEventArgs>(nui_DepthFrameReady);

                //컬러스트림 

                nui.ColorStream.Enable();
                nui.ColorFrameReady += new EventHandler<ColorImageFrameReadyEventArgs>(nui_ColorFrameReady);

                //클릭
                afterColorReady = skip;
                afterDepthReady = skip;
                settings = new KinectSettings();
                hands = new List<Hand>();

                //제스쳐2
                _dtw1 = new DtwGestureRecognizer(12, 0.6, 2, 2, 2);//정지포스쳐
                _dtw2 = new DtwGestureRecognizer(12, 1.0, 2, 2, 2);//일반포스쳐
                _dtw3 = new DtwGestureRecognizer(12, 1.2, 2, 2, 10);//제스쳐

                _video = new ArrayList();
                gestureType = 1;
                postureCount = 0;
                gestureFlag = true;
                string fileName = "111.txt";
                LoadGesturesFromFile(fileName);
                Skeleton2DDataExtract.Skeleton2DdataCoordReady += NuiSkeleton2DdataCoordReadyStop;
                setupAudio();
            }
            
            return true;
        }

        private void setupAudio()
        {

            foreach (RecognizerInfo reinfo in SpeechRecognitionEngine.InstalledRecognizers())//인스톨된 모든 스피치 엔진 불러온다.
            {
                //if (reinfo.Id == "SR_MS_en-US_Kinect_11.0")
                if (reinfo.Id.Contains("KR"))
                {
                    message = reinfo.Id;
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
          
            choices.Add("next");
            choices.Add("previous");
            choices.Add("start");
            choices.Add("tutorial");
            choices.Add("setting");
            choices.Add("shop");
            choices.Add("이전");
            choices.Add("시작");
            choices.Add("중지");
            
            choices.Add("다음");
          
          
          

            var gb = new GrammarBuilder { Culture = ri.Culture };
            gb.Append(choices);
            var g = new Grammar(gb);

            sre.LoadGrammar(g);
            //sre.SpeechHypothesized += new EventHandler<SpeechHypothesizedEventArgs>(sre_SpeechHypothesized);
            sre.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(sre_SpeechRecognized);

            //쓰레드 시작
            ts = new ThreadStart(UserFunc);
            th = new Thread(ts);
            th.Start();

        }

        //void sre_SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        //{
        //    if (e.Result.Confidence < 0.5) return;//신뢰도 0.5미만일땐 리턴
        //    if (Skeletons != null)
        //    {
        //        foreach (Skeleton s in Skeletons)
        //            if (s.TrackingState == SkeletonTrackingState.Tracked)
        //            {
        //                if (source.SoundSourceAngle < skeletonAngle + 10 && source.SoundSourceAngle > skeletonAngle - 10)
        //                {
        //                    message = e.Result.Text + " " + e.Result.Confidence.ToString();
        //                    switch (e.Result.Text)
        //                    {



        //                        case "next":

        //                            if (gameState == GameStates.ResultManager)
        //                            {
        //                                gameState = GameStates.ShowPictures;
        //                            }
        //                            if (gameState == GameStates.ShowPictures)
        //                            {
        //                                gameState = GameStates.RecordBoard;
        //                            }
        //                            if (gameState == GameStates.ShowPictures)
        //                            {
        //                                gameState = GameStates.SongMenu;
        //                            }

        //                            break;

        //                        case "previous":

        //                            if (gameState == GameStates.SongMenu || gameState == GameStates.ShopDoor || gameState == GameStates.SettingBoard)
        //                            {
        //                                gameState = GameStates.Menu;
        //                            }

        //                            if (gameState == GameStates.BackgroundItemShop || gameState == GameStates.EffectItemShop || gameState == GameStates.LeftItemShop || gameState == GameStates.RightItemShop || gameState == GameStates.NoteItemShop)
        //                            {
        //                                gameState = GameStates.ShopDoor;
        //                            }
        //                            break;


        //                        case "start":

        //                            if (gameState == GameStates.Menu)
        //                            {
        //                                gameState = GameStates.SongMenu;
        //                            }
        //                            if (gameState == GameStates.SongMenu)
        //                            {
        //                                resultSongMenu = songMenu.Scene_number;
        //                            }


        //                            break;


        //                        case "shop":

        //                            if (gameState == GameStates.Menu)
        //                            {
        //                                gameState = GameStates.ShopDoor;
        //                            }
        //                            break;

        //                        case "setting":

        //                            if (gameState == GameStates.Menu)
        //                            {
        //                                gameState = GameStates.SettingBoard;
        //                            }
        //                            break;

        //                        case "tutorial":

        //                            if (gameState == GameStates.Menu)
        //                            {
        //                                //    gameState = GameStates;
        //                            }
        //                            break;
        //                    }
        //                }
        //            }


        //    }
        //}

    



        private void UserFunc()
        {

            //오디오 스트림
            try
            {
                source = nui.AudioSource;
                source.BeamAngleMode = BeamAngleMode.Adaptive;
                source.EchoCancellationMode = EchoCancellationMode.CancellationAndSuppression;
                source.NoiseSuppression = true;
                source.AutomaticGainControlEnabled = false;
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
            if (skeleton != null)
            {

                if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
                    {
                        if (source.SoundSourceAngle < skeletonAngle + 10 && source.SoundSourceAngle > skeletonAngle - 10)
                        {
                            //message = e.Result.Text + " " + e.Result.Confidence.ToString();
                            switch (e.Result.Text)
                            {



                                case "next":
                                case "다음":
                                    if (gameState == GameStates.ResultManager)
                                    {
                                        gameState = GameStates.ShowPictures;
                                    }else if (gameState == GameStates.ShowPictures)
                                    {
                                        gameState = GameStates.RecordBoard;
                                    }else if (gameState == GameStates.RecordBoard)
                                    {
                                        gameState = GameStates.SongMenu;
                                    }

                                    break;

                                case "previous":
                                case "이전":

                                    if (gameState == GameStates.SongMenu || gameState == GameStates.ShopDoor || gameState == GameStates.SettingBoard)
                                    {
                                        gameState = GameStates.Menu;
                                    }

                                    if (gameState == GameStates.BackgroundItemShop || gameState == GameStates.EffectItemShop || gameState == GameStates.LeftItemShop || gameState == GameStates.RightItemShop || gameState == GameStates.NoteItemShop)
                                    {
                                        gameState = GameStates.ShopDoor;
                                    }
                                    break;


                                case "start":
                                case "시작":
                                    if (gameState == GameStates.Menu)
                                    {
                                        gameState = GameStates.SongMenu;
                                    }else if (gameState == GameStates.SongMenu)
                                    {
                                        soundRecogStartIndex = songMenu.Scene_number;
                                    }


                                    break;


                                case "shop":

                                    if (gameState == GameStates.Menu)
                                    {
                                        gameState = GameStates.ShopDoor;
                                    }
                                    break;

                                case "setting":

                                    if (gameState == GameStates.Menu)
                                    {
                                        gameState = GameStates.SettingBoard;
                                    }
                                    break;

                                case "tutorial":

                                    if (gameState == GameStates.Menu)
                                    {
                                        //    gameState = GameStates;
                                    }
                                    break;
                                

                            }
                        }
                    }


            }
        }

        //설정에 넣어야 할 부분
        //앵글 업(쓰레드용)
        void AngleUp()
        {
            try
            {
                if (nui.ElevationAngle <= nui.MaxElevationAngle - 3)
                    nui.ElevationAngle += 3;
            }
            catch (Exception ex)
            {
            }

            //th2.Abort();
        }

        //앵글 다운(쓰레드용)
        void AngleDown()
        {
            try
            {

                if (nui.ElevationAngle >= nui.MinElevationAngle + 3)
                    nui.ElevationAngle -= 3;
            }
            catch (Exception ex)
            {
            }
            //th3.Abort();
        }

        void nui_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            if (PicFlag)
            {
                byte[] ColorData = null;

                using (ColorImageFrame ImageParam = e.OpenColorImageFrame())
                {
                    if (ImageParam == null) return;
                    if (ColorData == null)
                        ColorData = new byte[ImageParam.Width * ImageParam.Height * 4];
                    ImageParam.CopyPixelDataTo(ColorData);
                    CapturePic = new Texture2D(GraphicsDevice, ImageParam.Width, ImageParam.Height);
                    Color[] bitmap = new Color[ImageParam.Width * ImageParam.Height];
                    bitmap[0] = new Color(ColorData[2], ColorData[1], ColorData[0], 255);

                    int sourceOffset = 0;
                    for (int i = 0; i < bitmap.Length; i++)
                    {
                        bitmap[i] = new Color(ColorData[sourceOffset + 2], ColorData[sourceOffset + 1], ColorData[sourceOffset], 255);
                        sourceOffset += 4;
                    }
                    CapturePic.SetData(bitmap);
                    //coding4fung tool kit에 비트맵 세이브가 있으니 그걸로 해볼것.
                    ts1 = new ThreadStart(SavePic);
                    th1 = new Thread(ts1);
                    th1.Start();



                }
                PicFlag = false;
            }
#if Kinect
            getDepthFrame();
#endif
        }

        void nui_DepthFrameReady(object sender, DepthImageFrameReadyEventArgs e)
        {
            //에러
            DepthImageFrame ImageParam = null;
            try
            {
                ImageParam = e.OpenDepthImageFrame();
            }
            catch(Exception ex)
            {
               
            }
               if (ImageParam == null)
                return;

            ImageBits = new short[ImageParam.PixelDataLength]; 
            ImageParam.CopyPixelDataTo(ImageBits);

            /////////////////////////////// 클릭이 필요할때///////////
            depthLocation = new ColorImagePoint[ImageParam.PixelDataLength];

            nui.MapDepthFrameToColorFrame(DepthImageFormat.Resolution640x480Fps30, ImageBits, ColorImageFormat.RgbResolution640x480Fps30, depthLocation);

            int handDepth = handPoint.Depth;
            int tempWidth = 110;
            int tempHeight = 110;


            KinectVideoTexture = new Texture2D(GraphicsDevice, tempWidth, tempHeight);
            Color[] bitmap = new Color[tempWidth * tempHeight];
            bitmap[0] = new Color(255, 255, 255, 255);

            if (handPoint.X < tempWidth / 2)
            {
                handPoint.X = tempWidth / 2;
            }
            if (handPoint.X > ImageParam.Width - tempWidth / 2)
            {
                handPoint.X = ImageParam.Width - tempWidth / 2;
            }
            if (handPoint.Y < tempHeight / 2)
            {
                handPoint.Y = tempHeight / 2;
            }
            if (handPoint.Y > ImageParam.Height - tempHeight / 2)
            {
                handPoint.Y = ImageParam.Height - tempHeight / 2;
            }
            if (handPoint.X < 0)
            {
                handPoint.X = 0;
            }
            if (handPoint.Y < 0)
            {
                handPoint.Y = 0;
            }



            int indexCount = 0;
            int[] depth = new int[tempHeight * tempWidth];

            if (skeleton != null)
            {

                if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
                    {
                        for (int j = handPoint.Y - tempHeight / 2; j < handPoint.Y + tempHeight / 2; j++)
                        {
                            for (int i = handPoint.X - tempWidth / 2; i < handPoint.X + tempWidth / 2; i++)
                            {
                                //손만
                                ColorImagePoint point = depthLocation[ImageParam.Width * j + i];
                                depth[indexCount] = ImageBits[ImageParam.Width * j + i] >> DepthImageFrame.PlayerIndexBitmaskWidth;//인덱스 오류
                                //Trace.WriteLine(indexCount);
                                indexCount++;
                            }
                        }
                    }


            }
            if (skeleton != null)
            {

                if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
                    {
                        bool[][] near = generateValidMatrix(tempWidth, tempHeight, depth);//에러


                        //화면에 띄우기
                        //indexCount = 0;
                        //for (int i = 0; i < tempWidth; i++)
                        //{
                        //    for (int j = 0; j < tempHeight; j++)
                        //    {
                        //        if (near[j][i] == true)
                        //        {
                        //            bitmap[indexCount] = new Color(255, 255, 255, 255);
                        //        }
                        //        else
                        //        {
                        //            bitmap[indexCount] = new Color(0, 0, 0, 255);
                        //        }
                        //        indexCount++;
                        //    }
                        //}
                        //KinectVideoTexture.SetData(bitmap);


                        hands = localizeHands(near);
                        afterDepthReady();
                    }
            }


            if (skeleton != null)
            {

                if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
                    {
                        if (hands.Count > 0)
                        {
                            if (hands[0].fingertips.Count > 0)
                            {
                                clickJudge = false;
                            }
                            else
                            {
                                clickJudge = true;
                            }
                        }
                    }
            }

            //10개의 프레임에 대한 정보를 리스트에 저장
            int maxCount = 10;
            if (listCount < maxCount)
            {
                handList.Add(handPoint);
                clickList.Add(clickJudge);
                listCount++;
            }
            else
            {
                handList.RemoveAt(0);
                clickList.RemoveAt(0);
                handList.Add(handPoint);
                clickList.Add(clickJudge);
            }

            finalClick = true;
            if (listCount >= maxCount)
            {

                double handDistance = Math.Sqrt((handList[0].X - handList[9].X) * (handList[0].X - handList[9].X) + (handList[0].Y - handList[9].Y) * (handList[0].Y - handList[9].Y));
                //message = handDistance.ToString();
                if (clickList[0] == true)
                {
                    if (handDistance > 10)
                    {
                        finalClick = false;
                    }
                    for (int i = 0; i < maxCount - 1; i++)
                    {
                        for (int j = i + 1; j < maxCount; j++)
                        {
                            if (clickList[i] != clickList[j])
                            {
                                finalClick = false;
                            }
                        }
                    }
                }
                else
                {
                    finalClick = false;
                }

            }

            //클릭여부
            if (finalClick == true)
            {
                //message = "click";
            }
            else
            {
                //message = "No click";
            }

            /////////////////////////////////

            //키재기
            if (skeleton != null)
            {


                if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
                {
                    Joint headJoint = skeleton.Joints[JointType.Head];
                    DepthImagePoint depthPoint = ImageParam.MapFromSkeletonPoint(headJoint.Position);
                    fy = (float)depthPoint.Y / (float)ImageParam.Height;



                    foreach (Joint joint in skeleton.Joints)
                    {
                        DepthImagePoint depthP;
                        depthP = ImageParam.MapFromSkeletonPoint(joint.Position);
                        switch (joint.JointType)
                        {
                            case JointType.Head:
                                fheadY = (float)depthP.Y / ImageParam.Height;
                                break;
                            case JointType.ShoulderCenter:
                                fcenterZ = (float)joint.Position.Z;
                                break;
                            case JointType.HipCenter:
                                fhipY = (float)depthP.Y / ImageParam.Height;
                                break;

                        }
                    }
                    double dbVal = (fhipY - fheadY) * 2;
                    dbVal = dbVal * 1.25;
                    factheight = (dbVal * (fcenterZ * 100) - fcenterZ * 2);
                    //message = factheight.ToString();
                }


                    
                
            }
        }

        private List<Hand> localizeHands(bool[][] valid)
        {
            int i, j, k;

            List<Hand> hands = new List<Hand>();

            List<PointFT> insidePoints = new List<PointFT>();
            List<PointFT> contourPoints = new List<PointFT>();


            bool[][] contour = new bool[valid.Length][];
            for (i = 0; i < valid.Length; ++i)
            {
                contour[i] = new bool[valid[0].Length];
            }

            int count = 0;
            for (i = 1; i < valid.Length - 1; ++i)
            {
                for (j = 1; j < valid[i].Length - 1; ++j)
                {

                    if (valid[i][j])
                    {
                        count = this.numValidPixelAdjacent(ref i, ref j, ref valid);

                        if (count == 4)
                        {
                            insidePoints.Add(new PointFT(i, j));
                        }
                        else
                        {
                            contour[i][j] = true;
                            contourPoints.Add(new PointFT(i, j));
                        }

                    }
                }
            }
            for (i = 0; i < contourPoints.Count; ++i)
            {
                Hand hand = new Hand();
                if (contour[contourPoints[i].X][contourPoints[i].Y])
                {
                    hand.contour = CalculateFrontier(ref valid, contourPoints[i], ref contour);

                    if (hand.contour.Count / (contourPoints.Count * 1.0f) > 0.20f
                        && hand.contour.Count > settings.k)
                    {
                        hand.calculateContainerBox(settings.containerBoxReduction);
                        hands.Add(hand);
                    }
                    if (hands.Count >= settings.maxTrackedHands)
                    {
                        break;
                    }
                }

            }

            for (i = 0; i < insidePoints.Count; ++i)
            {
                for (j = 0; j < hands.Count; ++j)
                {
                    if (hands[j].isPointInsideContainerBox(insidePoints[i]))
                    {
                        hands[j].inside.Add(insidePoints[i]);
                    }
                }
            }

            float min, max, distance = 0;

            for (i = 0; i < hands.Count; ++i)
            {
                max = float.MinValue;
                for (j = 0; j < hands[i].inside.Count; j += settings.findCenterInsideJump)
                {
                    min = float.MaxValue;
                    for (k = 0; k < hands[i].contour.Count; k += settings.findCenterInsideJump)
                    {
                        distance = PointFT.distanceEuclidean(hands[i].inside[j], hands[i].contour[k]);
                        if (!hands[i].isCircleInsideContainerBox(hands[i].inside[j], distance)) continue;
                        if (distance < min) min = distance;
                        if (min < max) break;
                    }

                    if (max < min && min != float.MaxValue)
                    {
                        max = min;
                        hands[i].palm = hands[i].inside[j];
                    }
                }
            }

            PointFT p1, p2, p3, pAux, r1, r2;
            int size;
            double angle;
            int jump;

            for (i = 0; i < hands.Count; ++i)
            {
                max = hands[i].contour.Count;
                size = hands[i].contour.Count;
                jump = (int)(size * settings.fingertipFindJumpPerc);
                for (j = 0; j < settings.k; j += 1)
                {
                    p1 = hands[i].contour[(j - settings.k + size) % size];
                    p2 = hands[i].contour[j];
                    p3 = hands[i].contour[(j + settings.k) % size];
                    r1 = p1 - p2;
                    r2 = p3 - p2;

                    angle = PointFT.angle(r1, r2);

                    if (angle > 0 && angle < settings.theta)
                    {
                        pAux = p3 + ((p1 - p3) / 2);
                        if (PointFT.distanceEuclideanSquared(pAux, hands[i].palm) >
                            PointFT.distanceEuclideanSquared(hands[i].contour[j], hands[i].palm))
                            continue;

                        hands[i].fingertips.Add(hands[i].contour[j]);
                        max = hands[i].contour.Count + j - jump;
                        max = Math.Min(max, hands[i].contour.Count);
                        j += jump;
                        break;
                    }
                }
                for (; j < max; j += settings.findFingertipsJump)
                {
                    p1 = hands[i].contour[(j - settings.k + size) % size];
                    p2 = hands[i].contour[j];
                    p3 = hands[i].contour[(j + settings.k) % size];
                    r1 = p1 - p2;
                    r2 = p3 - p2;

                    angle = PointFT.angle(r1, r2);

                    if (angle > 0 && angle < settings.theta)
                    {
                        pAux = p3 + ((p1 - p3) / 2);
                        if (PointFT.distanceEuclideanSquared(pAux, hands[i].palm) >
                            PointFT.distanceEuclideanSquared(hands[i].contour[j], hands[i].palm))
                            continue;

                        hands[i].fingertips.Add(hands[i].contour[j]);
                        j += jump;
                    }
                }
            }

            return hands;
        }
        private List<PointFT> CalculateFrontier(ref bool[][] valid, PointFT start, ref bool[][] contour)
        {
            List<PointFT> list = new List<PointFT>();
            PointFT last = new PointFT(-1, -1);
            PointFT current = new PointFT(start);
            int dir = 0;

            do
            {
                if (valid[current.X][current.Y])
                {
                    dir = (dir + 1) % 4;
                    if (current != last)
                    {
                        list.Add(new PointFT(current.X, current.Y));
                        last = new PointFT(current);
                        contour[current.X][current.Y] = false;
                    }
                }
                else
                {
                    dir = (dir + 4 - 1) % 4;
                }

                switch (dir)
                {
                    case 0: current.X += 1; break;
                    case 1: current.Y += 1; break;
                    case 2: current.X -= 1; break;
                    case 3: current.Y -= 1; break;
                }
            } while (current != start);

            return list;
        }

        private int numValidPixelAdjacent(ref int i, ref int j, ref bool[][] valid)
        {
            int count = 0;

            if (valid[i + 1][j]) ++count;
            if (valid[i - 1][j]) ++count;
            if (valid[i][j + 1]) ++count;
            if (valid[i][j - 1]) ++count;
            return count;
        }



        private bool[][] generateValidMatrix(int tempWidth, int tempHeight, int[] distance)
        {
            int x1 = 0;
            int x2 = tempWidth;
            int y1 = 0;
            int y2 = tempHeight;
            bool[][] near = new bool[y2 - y1][];
            for (int i = 0; i < near.Length; ++i)
            {
                near[i] = new bool[x2 - x1];
            }
            int max = int.MinValue, min = int.MaxValue;

            for (int k = 0; k < distance.Length; ++k)
            {
                if (distance[k] > max) max = distance[k];
                if (distance[k] < min && distance[k] != -1) min = distance[k];
            }

            int handDepth = handPoint.Depth;//에러
            ////

            int margin = (int)(min + settings.nearSpacePerc * (max - min));
            int index = 0;
            if (settings.absoluteSpace != -1) margin = min + settings.absoluteSpace;
            for (int i = 0; i < near.Length; ++i)
            {
                for (int j = 0; j < near[i].Length; ++j)
                {
                    index = tempWidth * (i + y1) + (j + x1);
                    if (distance[index] < handDepth + 120 && distance[index] > handDepth - 120)
                    {
                        near[i][j] = true;
                    }
                    else
                    {
                        near[i][j] = false;
                    }
                }
            }

            if (settings.smoothingIterations > 0)
            {
                near = dilate(near, settings.smoothingIterations);
                near = erode(near, settings.smoothingIterations);
            }


            int m;
            for (int j = 0; j < near[0].Length; ++j)
                near[0][j] = false;
            m = near.Length - 1;
            for (int j = 0; j < near[0].Length; ++j)
                near[m][j] = false;

            for (int i = 0; i < near.Length; ++i)
                near[i][0] = false;

            m = near[0].Length - 1;
            for (int i = 0; i < near.Length; ++i)
                near[i][m] = false;

            return near;
        }

        private bool[][] dilate(bool[][] image, int it)
        {
            bool[][] dilateImage = new bool[image.Length][];
            for (int i = 0; i < image.Length; ++i)
            {
                dilateImage[i] = new bool[image[i].Length];
            }

            int[][] distance = manhattanDistanceMatrix(image, true);

            for (int i = 0; i < image.Length; i++)
            {
                for (int j = 0; j < image[i].Length; j++)
                {
                    dilateImage[i][j] = ((distance[i][j] <= it) ? true : false);
                }
            }

            return dilateImage;
        }

        private bool[][] erode(bool[][] image, int it)
        {
            bool[][] erodeImage = new bool[image.Length][];
            for (int i = 0; i < image.Length; ++i)
            {
                erodeImage[i] = new bool[image[i].Length];
            }

            int[][] distance = manhattanDistanceMatrix(image, false);

            for (int i = 0; i < image.Length; i++)
            {
                for (int j = 0; j < image[i].Length; j++)
                {
                    erodeImage[i][j] = ((distance[i][j] > it) ? true : false);
                }
            }

            return erodeImage;
        }


        private int[][] manhattanDistanceMatrix(bool[][] image, bool zeroDistanceValue)
        {
            int[][] distanceMatrix = new int[image.Length][];
            for (int i = 0; i < distanceMatrix.Length; ++i)
            {
                distanceMatrix[i] = new int[image[i].Length];
            }

            for (int i = 0; i < distanceMatrix.Length; i++)
            {
                for (int j = 0; j < distanceMatrix[i].Length; j++)
                {
                    if ((image[i][j] && zeroDistanceValue) || (!image[i][j] && !zeroDistanceValue))
                    {
                        distanceMatrix[i][j] = 0;
                    }
                    else
                    {

                        distanceMatrix[i][j] = image.Length + image[i].Length;
                        if (i > 0) distanceMatrix[i][j] = Math.Min(distanceMatrix[i][j], distanceMatrix[i - 1][j] + 1);
                        if (j > 0) distanceMatrix[i][j] = Math.Min(distanceMatrix[i][j], distanceMatrix[i][j - 1] + 1);
                    }
                }
            }
            for (int i = distanceMatrix.Length - 1; i >= 0; i--)
            {
                for (int j = distanceMatrix[i].Length - 1; j >= 0; j--)
                {

                    if (i + 1 < distanceMatrix.Length)
                        distanceMatrix[i][j] = Math.Min(distanceMatrix[i][j], distanceMatrix[i + 1][j] + 1);
                    if (j + 1 < distanceMatrix[i].Length)
                        distanceMatrix[i][j] = Math.Min(distanceMatrix[i][j], distanceMatrix[i][j + 1] + 1);
                }
            }

            return distanceMatrix;
        }


        void nui_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (SkeletonFrame frame = e.OpenSkeletonFrame())
            {
                if (frame == null) return;
                if (frame != null)
                {

                    Skeletons = new Skeleton[frame.SkeletonArrayLength];

                    frame.CopySkeletonDataTo(Skeletons);

                    
                    if (CurrentTrackingId != 0)
                    {
                        skeleton =
                            (from s in Skeletons
                             where s.TrackingState == SkeletonTrackingState.Tracked &&
                                   s.Joints[JointType.Head].TrackingState == JointTrackingState.Tracked &&
                                   s.TrackingId == CurrentTrackingId
                             select s).FirstOrDefault();

                        if (skeleton == null)
                        {
                            CurrentTrackingId = 0;
                            nui.SkeletonStream.AppChoosesSkeletons = false;
                        }
                    }
                    else
                    {
                        skeleton =
                            (from s in Skeletons
                             where s.TrackingState == SkeletonTrackingState.Tracked &&
                                   s.Joints[JointType.Head].TrackingState == JointTrackingState.Tracked
                             select s).FirstOrDefault();

                        if (skeleton != null)
                        {
                            CurrentTrackingId = skeleton.TrackingId;
                            nui.SkeletonStream.AppChoosesSkeletons = true;
                            nui.SkeletonStream.ChooseSkeletons(CurrentTrackingId);
                        }
                    }

                }
                if (skeleton != null)
                {

                    if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
                        {
                            skeletonAngle = (Math.Atan2(skeleton.Joints[JointType.ShoulderCenter].Position.X, skeleton.Joints[JointType.ShoulderCenter].Position.Z) * 180.00) / Math.PI + 10;//음성인식을 위한 스켈레톤 각도
                            handPoint = nui.MapSkeletonPointToDepth(skeleton.Joints[JointType.HandRight].Position, DepthImageFormat.Resolution640x480Fps30);
                        }
                }


                //제스쳐
                if (gameState == GameStates.SongMenu)
                {
                    //activeRecognizer = CreateRecognizer();
                    this.activeRecognizer.Recognize(sender, frame, Skeletons);
                }
                //제스쳐2
                if (skeleton != null)
                {
                    if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
                    {
                        Skeleton2DDataExtract.ProcessData(skeleton);
                    }
                }
            }
        }

        //정지 포스쳐
        private void NuiSkeleton2DdataCoordReadyStop(object sender, Skeleton2DdataCoordEventArgs a)
        {
            if (_video.Count > MinimumFrames)
            {
                string s = _dtw1.Recognize(_video);

                if (!s.Contains("__UNKNOWN"))
                {
                    _video = new ArrayList();
                }

                //////////////////////////////////////////
                if (!s.Contains("__UNKNOWN"))
                {
                    postureCount++;
                    //message = "yes";
                    if (postureCount > 9)
                    {
                        //여기에 정지했을 때 동작 넣기
                        file.SetEndFile(true);
                        postureCount = 0;
                        //message = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";

                    }
                }

                if (s.Contains("__UNKNOWN"))
                {
                    postureCount = 0;
                    //message = "no";
                }


            }

            //////////////////////////////////////////



            if (_video.Count > BufferSize)
            {
                _video.RemoveAt(0);
            }

            if (!double.IsNaN(a.GetPoint(0).X))
            {

                _flipFlop = (_flipFlop + 1) % 2;
                if (_flipFlop == 0)
                {
                    _video.Add(a.GetCoords());
                }
            }


        }

        //일반 포스쳐
        private void NuiSkeleton2DdataCoordReadyPosture(object sender, Skeleton2DdataCoordEventArgs a)
        {


            if (_video.Count > MinimumFrames)
            {

                string s = _dtw2.Recognize(_video);
                if (!s.Contains("__UNKNOWN"))
                {
                    _video = new ArrayList();
                }

                if (!gestureFlag)
                {
                    //Skeleton2DDataExtract.Skeleton2DdataCoordReady -= NuiSkeleton2DdataCoordReadyPosture;
                    //gestureType = 1;
                    //postureCount = 0;
                    //gestureFlag = true;
                    //string fileName = "111.txt";
                    //LoadGesturesFromFile(fileName);
                    //Skeleton2DDataExtract.Skeleton2DdataCoordReady += NuiSkeleton2DdataCoordReadyStop;
                }
                if (gestureFlag)
                {
                    if (!s.Contains("__UNKNOWN"))
                    {
                        postureCount++;
                        //message = "yes";
                        perfectBannerManager.AddBanners(new Vector2(this.Window.ClientBounds.Width / 2 - 1380 / 4, this.Window.ClientBounds.Height / 2 - 428 / 4));
                        scoreManager.Perfomance = scoreManager.Perfomance + 1;
                            
                        if (postureCount > 100)
                        {
                            
                            gestureFlag = false;
                        }
                    }

                    if (s.Contains("__UNKNOWN"))
                    {
                        //message = "no";
                    }
                }
            }

            if (_video.Count > BufferSize)
            {
                _video.RemoveAt(0);
            }

            if (!double.IsNaN(a.GetPoint(0).X))
            {

                _flipFlop = (_flipFlop + 1) % 2;
                if (_flipFlop == 0)
                {
                    _video.Add(a.GetCoords());
                }
            }
        }

        //제스쳐
        private void NuiSkeleton2DdataCoordReadyGesture(object sender, Skeleton2DdataCoordEventArgs a)
        {
            Trace.WriteLine(gestureFlag);
            if (_video.Count > MinimumFrames)
            {

                string s = _dtw3.Recognize(_video);
                double score = _dtw3.MinDist;//점수

                if (!s.Contains("__UNKNOWN"))
                {
                    _video = new ArrayList();
                }

                if (!gestureFlag)
                {
                    //Skeleton2DDataExtract.Skeleton2DdataCoordReady -= NuiSkeleton2DdataCoordReadyGesture;
                    //gestureType = 1;
                    //postureCount = 0;
                    //gestureFlag = true;
                    //string fileName = "111.txt";
                    //LoadGesturesFromFile(fileName);
                    //Skeleton2DDataExtract.Skeleton2DdataCoordReady += NuiSkeleton2DdataCoordReadyStop;
                }
                if (gestureFlag)
                {
                    if (!s.Contains("__UNKNOWN"))
                    {
                        perfectBannerManager.AddBanners(new Vector2(this.Window.ClientBounds.Width / 2 - 1380 / 4, this.Window.ClientBounds.Height / 2 - 428 / 4));
                        scoreManager.Perfomance = scoreManager.Perfomance + 1;


                        //message = "yes" + score.ToString();
                        gestureFlag = false;
                    }

                    if (s.Contains("__UNKNOWN"))
                    {
                        //message = "no";
                    }
                }
            }

            if (_video.Count > BufferSize)
            {
                _video.RemoveAt(0);
            }

            if (!double.IsNaN(a.GetPoint(0).X))
            {

                _flipFlop = (_flipFlop + 1) % 2;
                if (_flipFlop == 0)
                {
                    _video.Add(a.GetCoords());
                }
            }
        }

        private void LoadGesturesFromFile(string fileName)
        {
            int itemCount = 0;
            string line;
            string gestureName = String.Empty;


            ArrayList frames = new ArrayList();
            double[] items = new double[12];

            System.IO.StreamReader file = new System.IO.StreamReader(fileName);
            while ((line = file.ReadLine()) != null)
            {
                if (line.StartsWith("@"))
                {
                    gestureName = line;
                    continue;
                }

                if (line.StartsWith("~"))
                {
                    frames.Add(items);
                    itemCount = 0;
                    items = new double[12];
                    continue;
                }

                if (!line.StartsWith("----"))
                {
                    items[itemCount] = Double.Parse(line);
                }

                itemCount++;

                if (line.StartsWith("----"))
                {
                    if (gestureType == 1)
                    {
                        _dtw1.AddOrUpdate(frames, gestureName);
                    }
                    if (gestureType == 2)
                    {
                        _dtw2.AddOrUpdate(frames, gestureName);
                    }
                    if (gestureType == 3)
                    {
                        _dtw3.AddOrUpdate(frames, gestureName);
                    }
                    frames = new ArrayList();
                    gestureName = String.Empty;
                    itemCount = 0;
                }
            }

            file.Close();
        }

        void SavePic()
        {
            DateTime dateTime =DateTime.Now;
   
            String gesture = "gesture_" + dateTime.Day.ToString() + "_" + dateTime.Hour.ToString() + "_" + dateTime.Minute.ToString() + "_" + dateTime.Second.ToString()+".jpg";
            String dir = System.Environment.CurrentDirectory +  "\\beethovenRecord\\userPicture\\" + gesture;

            if (!isScorePic)
            {
                //마지막 스코어 보드에 쓰이는 사진
                ScorePic = gesture;

                isScorePic = true;
                Stream str = System.IO.File.OpenWrite(dir);
                CapturePic.SaveAsJpeg(str, SCR_W, SCR_H);
                str.Dispose();
            }

            playingPictures.Enqueue(CapturePic);

            
           

            
           
            th1.Abort();
        }

        private Recognizer CreateRecognizer()
        {      

            var recognizer = new Recognizer();

            
            recognizer.SwipeRightDetected += (s, e) =>
            {//오른손이 왼쪽으로 동작할때
                songMenu.isKinectRight = true;
                //사진
                //foreach (Skeleton sd in Skeletons)
                //{

                //    if (sd.TrackingState == SkeletonTrackingState.Tracked)
                //    {
                //        PicFlag = true;


                //    }
                //}
                //message = "right";
            };


            recognizer.SwipeLeftDetected += (s, e) =>
            {//왼손이 오른쪽으로 동작할때

                songMenu.isKinectLeft = true;
                //ts4 = new ThreadStart(FaceDetect);
                //th4 = new Thread(ts4);
                //th4.Start();
                //message = "left";
            };

            return recognizer;
        }

#endif
        //입력 타입
        //출력 센터


        public static void GetCenterOfButton(Rectangle rec)
        {
            
            center = new Vector2(rec.X + rec.Width / 2, rec.Y + rec.Height / 2);
            
        }

        public float UserParam
        {
            get { return userParam; }
            set { userParam = value; }

        }



        public KinectSensor Nui
        {
            get { return nui; }
            set { nui = value; }

        }


        //마우스 충돌 처리
        private void HandleMouseInput(MouseState mouseState)
        {

            collisionManager.checkDragNote(new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y));

            collisionManager.CheckMouseCollisions(0, new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y));

            collisionManager.CheckMouseCollisions(1, new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y));

            collisionManager.CheckMouseCollisions(2, new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y));

            collisionManager.CheckMouseCollisions(3, new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y));

            collisionManager.CheckMouseCollisions(4, new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y));

            collisionManager.CheckMouseCollisions(5, new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y));

        }

#if Kinect
        //키넥트 충돌 처리
        private void HandleInput()
        {
            //오른손 /
           collisionManager.checkDragNote(new Vector2(drawrec1.X, drawrec1.Y));

            collisionManager.CheckRightHandCollisions(0, new Vector2(drawrec1.X, drawrec1.Y));

            collisionManager.CheckRightHandCollisions(1, new Vector2(drawrec1.X, drawrec1.Y));

            collisionManager.CheckRightHandCollisions(2, new Vector2(drawrec1.X, drawrec1.Y));

            collisionManager.CheckRightHandCollisions(3, new Vector2(drawrec1.X, drawrec1.Y));

            collisionManager.CheckRightHandCollisions(4, new Vector2(drawrec1.X, drawrec1.Y));

            collisionManager.CheckRightHandCollisions(5, new Vector2(drawrec1.X, drawrec1.Y));


            //왼손
            collisionManager.checkDragNote(new Vector2(drawrec2.X, drawrec2.Y));

            collisionManager.CheckLeftHandCollisions(0, new Vector2(drawrec2.X, drawrec2.Y));

            collisionManager.CheckLeftHandCollisions(1, new Vector2(drawrec2.X, drawrec2.Y));

            collisionManager.CheckLeftHandCollisions(2, new Vector2(drawrec2.X, drawrec2.Y));

            collisionManager.CheckLeftHandCollisions(3, new Vector2(drawrec2.X, drawrec2.Y));

            collisionManager.CheckLeftHandCollisions(4, new Vector2(drawrec2.X, drawrec2.Y));

            collisionManager.CheckLeftHandCollisions(5, new Vector2(drawrec2.X, drawrec2.Y));

        }
#endif


   
        ///***
        ////템포변경
        //private void tempoChange(double changedT)
        //{
        //    isChangedTempo = true;

            
        //    this.changedTempo = changedT;

        //    float frequency = 0;
            
        //    //현재 템포 가져와소 float frequency에 넣기//resultFmod 는 성공여부만 나타남
        //    resultFmod = sndChannel.getFrequency(ref frequency);
         
        //    //템포설정
        //    sndChannel.setFrequency(frequency * (float)changedT);
            
        //    //템포를 다른 노트 모두에 적용
        //    file.ChangeArrayNoteTempo(changedT);

        //    //현재 설정된 두번째 가이드라인이 있으면 지움
        //    GuideLineManager.DeleteAllSecondGuideLine();
        

        //}

        /////***
        ////템포 변경 전에 기본 템포를 저장해둠. 다시 롤백할 때 필요
        //public void SetBasicTempo()
        //{
        //    if (!isChangedTempo)
        //    {
        //       sndChannel.getFrequency(ref basicFrequency);
        //    }
        //}

        ////이전에 설정한 기본 템포로 돌아감 
        //public void ReturnBasicTempo()
        //{
        //    if (basicFrequency != 0)
        //    {
        //        sndChannel.setFrequency(basicFrequency);
        //        isChangedTempo = false;
        //        changedTempo = 0;
        //    }
        //}

        ////일단 안쓰임
        ////일정 시간이 지나면 다시 원래 템포로 돌아옴
        ////private void AutoRetrunChangeTempo(GameTime gameTime)
        ////{
        ////    if (isChangedTempo)
        ////    {              
        ////        //처음시작 
        ////        chagneLimitedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
        ////        //Trace.WriteLine(chagneLimitedTime.ToString());
                
                
        ////        if (chagneLimitedTime >= 3000 && oneTime)
        ////        {
        ////            optionalTime =( 3 - (3 / this.changedTempo) ) *-1;
        ////                //템포가 4배가 된상태에서 1초동안 지속이 된다면 모두 1-  1/4   0.75초씩 줄여야 한다ㅣ
        ////            oneTime = false;
        ////            ReturnBasicTempo();
        ////        }   
        ////    }
        ////}

        /////***
        //////템포가 변하고나서 얼마나 변했는지 시간을 재는데 사용
        //private void StartChangedTime(GameTime gameTime)
        //{
            
        //    if (isChangedTempo)
        //    {
        //        //처음시작 
        //        chagneLimitedTime += gameTime.ElapsedGameTime.TotalMilliseconds;  
        //    }
        //}


        ////다시 원점으로 돌아갈 때 쓰임 
        
        ////늘어나거나 줄어드는 양을 계산해주고 
        ///// <summary>
        ///// ***
        ///// </summary>
        //private void SetOptionalTime()
        //{
        //    //임시로 넣은 것일 뿐
        //    if (oneTime)
        //    {
        //        //템포 다시 원상복귀
        //        file.ChangeArrayNoteTempoBack(this.changedTempo);
           
        //        double time = 0;

        //        //옵션 계산
        //        time = ((this.chagneLimitedTime / 1000) - ((this.chagneLimitedTime / 1000) / this.changedTempo)) * -1;
                                
        //        optionalTime += time;

        //        //각 노트 시작에 옵션을 더함
        //        file.OptionalArrayNote(optionalTime);
                
        //        //템포가 0.9배가 된상태에서 1초동안 지속이 된다면 모두 4-  4/4   3초씩 줄여야 한다ㅣ
                
        //        oneTime = false;
        //        chagneLimitedTime = 0;
                

        //        //원래 템포로 돌아감
        //        ReturnBasicTempo();
        //    }   
        //}


        //메트로놈
        private Texture2D GetMetroTexture(double tempo)
        {
            Texture2D metoroTexture = metronomes[4];
            if (tempo == 1.0f)
            {
                metoroTexture = metronomes[4];
            }
            else if (tempo == 1.1f)
            {
                metoroTexture = metronomes[5];
            }
            else if (tempo == 1.2f)
            {
                metoroTexture = metronomes[6];
            }
            else if (tempo == 1.3f)
            {
                metoroTexture = metronomes[7];
            }

            else if (tempo == 0.9f)
            {
                metoroTexture = metronomes[3];
            }
            else if (tempo == 0.8f)
            {
                metoroTexture = metronomes[2];
            }
            else if (tempo == 0.7f)
            {
                metoroTexture = metronomes[1];
            }

            return metoroTexture;
        }
         



        //상점안에서 상점 대문으로 가는 키보드 처리
        private void HandleKeyboardInputGoToMenu(KeyboardState keyState)
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
                itemManager.SaveFileItem();
                gameState = GameStates.ShopDoor;

            }

        }


        private void HandleKeyboardInput(KeyboardState keyState)
        {
            if (keyState.IsKeyDown(Keys.B))
            {
                SoundFmod.sndSystem.createSound("C:\\beethoven\\" + noteFileManager, FMOD.MODE.HARDWARE, ref SoundFmod.sndSound);
                SoundFmod.sndSystem.playSound(CHANNELINDEX.FREE, SoundFmod.sndSound, false, ref SoundFmod.sndChannel);

              
            }
            if (keyState.IsKeyDown(Keys.P))
            {
             //   file.IsStop = true;


            }

            if (keyState.IsKeyDown(Keys.O))
            {
               // file.IsStop = false;


            }


            if (keyState.IsKeyDown(Keys.Escape))
            {

                file.SetEndFile(true);
            }

            if (keyState.IsKeyDown(Keys.L))
            {
               // sndChannel.setFrequency(44100.0f);
               // ReturnBasicTempo();

                SoundFmod.SetOptionalTime();
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

                float fCurrentVolume = SoundFmod.GetVolume();
                //Trace.WriteLine(fCurrentVolume);
            }

            //스트로크 3
            if (keyState.IsKeyDown(Keys.U))
            {
             //   //0부터 1까지

             //   RESULT a = SoundFmod.sndChannel.setVolume(0.5f);
             ////   SoundFmod.sndChannel.setPaused(true);
             //   Trace.WriteLine(volume);
            }
           
        }
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 


        //카리스마 타임이맞는지 확인한다.
//        public void JudgeCharisma()
//        {

//#if Kinect
//            if(charismaManager.IsCharismaTime == 1 && !charismaManager.IsJudgeCheck) 
//            {
//                if(!kinectMessage.Contains("__UNKNOWN"))
//                {

//                }
//                //나중에는 타입마다 이렇게 설정
//                //else if(charismaManager.Type == 0 && kinectMessage.Contains("@xxxxxx");
//                else
//                {
//                    //perfectBannerManager.AddBanners(new Vector2(this.Window.ClientBounds.Width / 2 - 1380 / 4, this.Window.ClientBounds.Height / 2 - 428 / 4));
//                    //scoreManager.Perfomance = scoreManager.Perfomance + 1;
//                    charismaManager.IsJudgeCheck = true;
//                }
            
//            }
//#endif
//        }
            
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
      
            mouseStateCurrent = Mouse.GetState();
            Rectangle rightHandPosition = new Rectangle((int)j1r.Position.X, (int)j1r.Position.Y, 5, 5);

           switch (gameState)
           {
               #region 타이틀
               //타이틀 화면
                case GameStates.Menu:

                    menuScene.Update(gameTime, rightHandPosition);

                break;
               #endregion

               #region 상점대문
                //상점대문
               case GameStates.ShopDoor:

                    //타이틀화면으로 돌아가는 키보드처리
                    HandleKeyboardInputGoToMenu(Keyboard.GetState());
                    shopDoor.Update(gameTime, rightHandPosition);
                   // pastClick = finalClick;

                break;

                #endregion

               #region 아이템상점들


               #region 오른쪽상점
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
                   if (!rightItemShop.getSellOrWearOne() && !rightItemShop.getBuyOne() && !rightItemShop.getNoGold() && !rightItemShop.getHandInItem())
                   {
                       int count = 0;
                       for (i = 0; i < rectRightItems.Count; i++)
                       {
                           //Trace.WriteLine(finalClick + " - " + pastClick);
                           //아이템을 선택 했을때
                           if (mouseRect.Intersects(rectRightItems[i]) || rightHandPosition.Intersects(rectRightItems[i]))
                           {

                               nearButton = true;
                               GetCenterOfButton(rectRightItems[i]);
                               //if ((mouseRect.Intersects(rectRightItems[i]) && mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) ||
                               //    (drawrec1.Intersects(rectRightItems[i]) && finalClick && !pastClick))

                               if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
                               {

                                   //nearButton = false;
                                   //어두어짐
                                   rightItemShop.setDarkBackground(true);

                                   selectedItem = shopRightItems[i];

                                   //이미 산거이면 true
                                   if (rightItemShop.haveOne(shopRightItems[i]))
                                   {
                                       //장착 메시지 or 팔기 메시지 박스 띄우기 
                                       rightItemShop.setSellOrWearOne(true);
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

                                   //이게 있어야 중복해서 안된다.
                                   //중복 구입 막음

                                   mouseStatePrevious = mouseStateCurrent;
                                   pastClick = finalClick;
                               }
                           }
                           else
                           {
                               count++;
                           }
                           
                           
                          
                       }

                       if (count == rectRightItems.Count)
                       {
                           nearButton = false;
                       }
                       
                   }


                   //돈 부족 메시지 띄우기
                   if (rightItemShop.getNoGold())
                   {
                       //버튼 Hover
                       if (mouseRect.Intersects(rightItemShop.getRectNoGoldButton()) || rightHandPosition.Intersects(rightItemShop.getRectNoGoldButton()))
                       {
                           //눌린모양
                           rightItemShop.setHoverNoGoldButton(true);
                           nearButton = true;
                           GetCenterOfButton(rightItemShop.getRectNoGoldButton());
                           //버튼 누르면
                           if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
                           {
                               nearButton = false;
                                //다시 밝게
                               rightItemShop.setDarkBackground(false);
                               //메시지 없애기
                               rightItemShop.setNoGold(false);
                              
                           }
                      
                       }
                       //버튼 not hover
                       else
                       {
                           nearButton = false;
                           rightItemShop.setHoverNoGoldButton(false);
                       }
                   }


                   

                    //장착한 아이템 팔 수없게 
                    if (rightItemShop.getHandInItem())
                    {
                        //버튼 Hover
                        if (mouseRect.Intersects(rightItemShop.getRectHandInItemButton()) || rightHandPosition.Intersects(rightItemShop.getRectHandInItemButton()))
                        {
                            //눌린모양
                            nearButton = true;
                            GetCenterOfButton(rightItemShop.getRectHandInItemButton());

                            rightItemShop.setHoverHandInItemButton(true);
                            //버튼 누르면
                            if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
                            {
                                nearButton = false;
                                //다시 밝게
                                rightItemShop.setDarkBackground(false);
                                //메시지 없애기
                                rightItemShop.setHandInItem(false);


                            }
                        }
                        //버튼 not hover
                        else
                        {
                            nearButton = false;
                            rightItemShop.setHoverHandInItemButton(false);
                        }
                        //mouseStatePrevious = mouseStateCurrent;
                        //pastClick = finalClick;
                    }

                   //장착할것인지 팔것인지 묻는 메시지

                if (rightItemShop.getSellOrWearOne())
                {
                    //마우스가 장착 버튼에 올려졌을 때
                    if (mouseRect.Intersects(rightItemShop.getRectWearButton()) || rightHandPosition.Intersects(rightItemShop.getRectWearButton()))
                    {
                        rightItemShop.setHoverWearButton(true);
                        nearButton = true;
                        GetCenterOfButton(rightItemShop.getRectWearButton());

                        if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
                        {
                            //장착 메시지 박스 띄우기 
                            // rightItemShop.setWearOne(true);
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
                                   // rightItemShop.setWearOne(false);
                                    //밝게 하기
                                    rightItemShop.setDarkBackground(false);
                                    rightItemShop.setSellOrWearOne(false);
                                    nearButton = false;

                                    //파일로저장
                                    itemManager.SaveFileItem();
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

                        
                        
                        rightItemShop.setHoverWearButton(false);
                        
                    }


                    //마우스가 팔기 버튼에 눌러졌을 때
                    if (mouseRect.Intersects(rightItemShop.getRectSellButton()) || rightHandPosition.Intersects(rightItemShop.getRectSellButton()))
                    {

                        rightItemShop.setHoverSellButton(true);
                        nearButton = true;
                        GetCenterOfButton(rightItemShop.getRectSellButton());

                        if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
                        {
                            //팔기 메시지 박스 띄우기 
                            // rightItemShop.setSellOne(true);
                            //선택된 아이템이 있으면
                            if (selectedItem != null)
                            {
                                //find the index of item in myrightitem list
                                //아이템 찾기
                                int index = itemManager.getIndexOfAllRightItem(selectedItem);

                                //찾은 아이템이 장착하고 있는 아이템이면 팔 수 없다.
                                if (index != itemManager.getRightHandIndex())
                                {
                                    //아이템을 찾았으면
                                    if (index != -1)
                                    {
                                        nearButton = true;
                                        //전체 돈에서 판 비용 더해줌
                                        scoreManager.TotalGold += selectedItem.GetCost() / 2;

                                        //자신의 인벤토리에서 빼기
                                        rightItemShop.removeItemtoMyItem(selectedItem);


                                        rightItemShop.setDarkBackground(false);

                                        //돈을 파일에 저장
                                        reportManager.SaveGoldToFile();

                                        //추가적으로 sellorwear 버튼도 false로 해야 한다.
                                        rightItemShop.setSellOrWearOne(false);
                                        
                                        //파일로저장
                                        itemManager.SaveFileItem();


                                    }
                                    //아이템을 찾지 못했으면
                                    else
                                    {
                                        Trace.WriteLine("get wrong index( no item in list)");
                                    }
                                }
                                else
                                {
                                    rightItemShop.setHandInItem(true);

                                    rightItemShop.setSellOrWearOne(false);
                                    rightItemShop.setSellOne(false);


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
                        //nearButton = true;
                        rightItemShop.setHoverSellButton(false);
                    }


                    //취소버튼 눌렀을 때
                    if (mouseRect.Intersects(rightItemShop.getRectCancelButton()) || rightHandPosition.Intersects(rightItemShop.getRectCancelButton()))
                    {
                        rightItemShop.setHoverCancelButton(true);
                        nearButton = true;
                        GetCenterOfButton(rightItemShop.getRectCancelButton());

                        if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
                        {
                            //return to normal , remove message box
                            rightItemShop.setSellOrWearOne(false);
                            rightItemShop.setDarkBackground(false);
                            nearButton = false;
                            //mouseStatePrevious = mouseStateCurrent;
                            //pastClick = finalClick;
                        }
                    }
                    else
                    {
                       // nearButton = false;
                        rightItemShop.setHoverCancelButton(false);
                    }
                   
                    if(
                     !(mouseRect.Intersects(rightItemShop.getRectWearButton()) || rightHandPosition.Intersects(rightItemShop.getRectWearButton()))
                     &&!(mouseRect.Intersects(rightItemShop.getRectCancelButton()) || rightHandPosition.Intersects(rightItemShop.getRectCancelButton()))
                     &&!(mouseRect.Intersects(rightItemShop.getRectSellButton()) || rightHandPosition.Intersects(rightItemShop.getRectSellButton()))
                    )
                    {
                        nearButton = false;
                    }

                }

                  
                  
                   //구입 메시지
                   //message box about buying item
                   if (rightItemShop.getBuyOne())
                   {
                       //mouse cursor on right button
                       if (mouseRect.Intersects(rightItemShop.getRectYesButton()) || rightHandPosition.Intersects(rightItemShop.getRectYesButton()))
                       {
                           nearButton = true;
                           GetCenterOfButton(rightItemShop.getRectYesButton());
                           rightItemShop.setHoverYesButton(true);
                           if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
                           {
                               //add item to my item
                               if (selectedItem != null)
                               {
                                   //돈으로 물건사기 
                                   //buy item with money

                                   nearButton = false;
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

                                       //파일로저장
                                       itemManager.SaveFileItem();
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

                       if (mouseRect.Intersects(rightItemShop.getRectNoButton()) || rightHandPosition.Intersects(rightItemShop.getRectNoButton()))
                       {
                           nearButton = true;
                           GetCenterOfButton(rightItemShop.getRectNoButton());
                           rightItemShop.setHoverNoButton(true);
                           if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
                           {
                               //return to normal , remove message box
                               rightItemShop.setBuyOne(false);
                               rightItemShop.setDarkBackground(false);
                               nearButton = false;
                           }
                       }
                       else
                       {
                           rightItemShop.setHoverNoButton(false);
                       }

                       if (
                       !(mouseRect.Intersects(rightItemShop.getRectYesButton()) || rightHandPosition.Intersects(rightItemShop.getRectYesButton()))
                       && !(mouseRect.Intersects(rightItemShop.getRectNoButton()) || rightHandPosition.Intersects(rightItemShop.getRectNoButton()))
                       )
                       {

                           nearButton = false;
                       }
                       

                   }




                   //다른메시지 창이 안떳을 때

                   if (!(rightItemShop.getBuyOne()) && !(rightItemShop.getSellOrWearOne()) && !rightItemShop.getHandInItem() && !rightItemShop.getNoGold() )

                   {
                       if (mouseRect.Intersects(rightItemShop.getRectPreviousButton()) || rightHandPosition.Intersects(rightItemShop.getRectPreviousButton()))
                       {
                           nearButton = true;
                           GetCenterOfButton(rightItemShop.getRectPreviousButton());
                           rightItemShop.setClickPreviousButton(true);
                           //click the right hand item section
                           if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
                           {
                               nearButton = false;
                               gameState = GameStates.ShopDoor;
                           }
                       }
                       else
                       {
                           //nearButton = false;
                           rightItemShop.setClickPreviousButton(false);
                       }
                   }
                   break;

               #endregion

               #region 왼쪽상점

               case GameStates.LeftItemShop:
                   //상점대문으로 돌아가는 키보드처리
                   HandleKeyboardInputinItemShop(Keyboard.GetState());

                   Rectangle mouseRectinleft = new Rectangle(mouseStateCurrent.X, mouseStateCurrent.Y, 5, 5);
                   int j;
                   //클릭했을때 어두워지는것.
                   List<Rectangle> rectLeftItems = leftItemShop.getRectLeftItem();
                   List<Item> shopLeftItems = leftItemShop.getShopLeftItem();
                   
               
               
                    //다이얼로그를 띄웠을 때 이것이 중복실행되지 않도록 

                


                       //돈 부족 메시지 
                    if (leftItemShop.getNoGold())
                    {
                        if (mouseRectinleft.Intersects(leftItemShop.getRectNoGoldButton()) || drawrec1.Intersects(leftItemShop.getRectNoGoldButton()))
                        {

                            leftItemShop.setHoverNoGoldButton(true);
                            if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
                            {

                                leftItemShop.setDarkBackground(false);

                                leftItemShop.setNoGold(false);
                            }

                        }
                        else
                        {
                            leftItemShop.setHoverNoGoldButton(false);
                        }

                        //mouseStatePrevious = mouseStateCurrent;
                        //pastClick = finalClick;
                    }


                    //장착한 아이템 팔 수없게 
                    if (leftItemShop.getHandInItem())
                    {
                        //버튼 Hover
                        if (mouseRectinleft.Intersects(leftItemShop.getRectHandInItemButton()) || drawrec1.Intersects(leftItemShop.getRectHandInItemButton()))
                        {
                            //눌린모양
                            leftItemShop.setHoverHandInItemButton(true);
                            //버튼 누르면
                            if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
                            {
                                //다시 밝게
                                leftItemShop.setDarkBackground(false);
                                //메시지 없애기
                                leftItemShop.setHandInItem(false);
                            }
                        }
                        //버튼 not hover
                        else
                        {
                            leftItemShop.setHoverHandInItemButton(false);
                        }
                        //mouseStatePrevious = mouseStateCurrent;
                        //pastClick = finalClick;
                    }




                    //장착할것인지 팔것인지 묻는 메시지

                    if (leftItemShop.getSellOrWearOne())
                    {
                        //마우스가 장착 버튼에 올려졌을 때
                        if (mouseRectinleft.Intersects(leftItemShop.getRectWearButton()) || drawrec1.Intersects(leftItemShop.getRectWearButton()))
                        {
                            leftItemShop.setHoverWearButton(true);

                            if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
                            {
                                //장착 메시지 박스 띄우기 
                                leftItemShop.setWearOne(true);
                            }
                        }
                        else
                        {
                            leftItemShop.setHoverWearButton(false);
                        }


                        //마우스가 팔기 버튼에 눌러졌을 때
                        if (mouseRectinleft.Intersects(leftItemShop.getRectSellButton()) || drawrec1.Intersects(leftItemShop.getRectSellButton()))
                        {

                            leftItemShop.setHoverSellButton(true);

                            if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
                            {
                                //장착 메시지 박스 띄우기 
                                leftItemShop.setSellOne(true);
                            }
                        }
                        else
                        {
                            leftItemShop.setHoverSellButton(false);
                        }


                        //취소버튼 눌렀을 때
                        if (mouseRectinleft.Intersects(leftItemShop.getRectCancelButton()) || drawrec1.Intersects(leftItemShop.getRectCancelButton()))
                        {
                            leftItemShop.setHoverCancelButton(true);
                            if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
                            {
                                //return to normal , remove message box
                                leftItemShop.setSellOrWearOne(false);
                                leftItemShop.setDarkBackground(false);
                            }
                        }
                        else
                        {
                            leftItemShop.setHoverCancelButton(false);
                        }
                        //mouseStatePrevious = mouseStateCurrent;
                        //pastClick = finalClick;
                    }

                    //message box about wearing item 
                    if (leftItemShop.getWearOne())
                    {
                        //mouse cursor on yes button
                        if (mouseRectinleft.Intersects(leftItemShop.getRectYesButton()) || drawrec1.Intersects(leftItemShop.getRectYesButton()))
                        {
                            leftItemShop.setHoverYesButton(true);
                            if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
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
                        if (mouseRectinleft.Intersects(leftItemShop.getRectNoButton()) || drawrec1.Intersects(leftItemShop.getRectNoButton()))
                        {
                            leftItemShop.setHoverNoButton(true);
                            if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
                            {
                                //return to normal , remove message box
                                leftItemShop.setWearOne(false);
                                leftItemShop.setDarkBackground(false);

                                //추가적으로 sellorwear 버튼도 false로 해야 한다.
                                leftItemShop.setSellOrWearOne(false);
                            }
                        }
                        else
                        {
                            leftItemShop.setHoverNoButton(false);
                        }
                        //mouseStatePrevious = mouseStateCurrent;
                        //pastClick = finalClick;
                    }



                    //팔기 메시지 띄우기
                    //message box about wearing item 
                    if (leftItemShop.getSellOne())
                    {
                        //mouse cursor on yes button
                        if (mouseRectinleft.Intersects(leftItemShop.getRectYesButton()) || drawrec1.Intersects(leftItemShop.getRectYesButton()))
                        {
                            leftItemShop.setHoverSellYesButton(true);

                            if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
                            {
                                //선택된 아이템이 있으면
                                if (selectedItem != null)
                                {
                                    //find the index of item in myrightitem list
                                    //아이템 찾기
                                    int index = itemManager.getIndexOfAllLeftItem(selectedItem);

                                    //찾은 아이템이 장착하고 있는 아이템이면 팔 수 없다.
                                    if (index != itemManager.getLeftHandIndex())
                                    {
                                        //아이템을 찾았으면
                                        if (index != -1)
                                        {

                                            //전체 돈에서 판 비용 더해줌
                                            scoreManager.TotalGold += selectedItem.GetCost() / 2;

                                            //자신의 인벤토리에서 빼기
                                            leftItemShop.removeItemtoMyItem(selectedItem);


                                            leftItemShop.setSellOne(false);

                                            leftItemShop.setDarkBackground(false);

                                            //돈을 파일에 저장
                                            reportManager.SaveGoldToFile();

                                            //추가적으로 sellorwear 버튼도 false로 해야 한다.
                                            leftItemShop.setSellOrWearOne(false);


                                        }
                                        //아이템을 찾지 못했으면
                                        else
                                        {
                                            Trace.WriteLine("get wrong index( no item in list)");
                                        }
                                    }
                                    else
                                    {
                                        leftItemShop.setSellOrWearOne(false);
                                        leftItemShop.setSellOne(false);


                                        leftItemShop.setHandInItem(true);

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
                            leftItemShop.setHoverSellYesButton(false);
                        }
                        //mouse cursor on no button
                        //노버튼 눌렀을 때
                        if (mouseRectinleft.Intersects(leftItemShop.getRectNoButton()) || drawrec1.Intersects(leftItemShop.getRectNoButton()))
                        {
                            leftItemShop.setHoverSellNoButton(true);
                            if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
                            {
                                //return to normal , remove message box
                                leftItemShop.setSellOne(false);
                                leftItemShop.setDarkBackground(false);
                            }
                        }
                        else
                        {
                            leftItemShop.setHoverSellNoButton(false);
                        }
                        //mouseStatePrevious = mouseStateCurrent;
                        //pastClick = finalClick;

                    }

                    //message box about buying item
                    if (leftItemShop.getBuyOne())
                    {
                        //mouse cursor on right button

                        if (mouseRectinleft.Intersects(leftItemShop.getRectYesButton()) || drawrec1.Intersects(leftItemShop.getRectYesButton()))
                        {
                            leftItemShop.setHoverYesButton(true);
                            if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
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
                        if (mouseRectinleft.Intersects(leftItemShop.getRectNoButton()) || drawrec1.Intersects(leftItemShop.getRectNoButton()))
                        {
                            leftItemShop.setHoverNoButton(true);
                            if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
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
                        //mouseStatePrevious = mouseStateCurrent;
                        //pastClick = finalClick;
                    }

                    if (!leftItemShop.getWearOne() && !leftItemShop.getBuyOne() && !leftItemShop.getNoGold() && !leftItemShop.getHandInItem())
                    {
                        for (j = 0; j < rectLeftItems.Count; j++)
                        {
                            if ((mouseRectinleft.Intersects(rectLeftItems[j]) && mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released)
                                || (drawrec1.Intersects(rectLeftItems[j]) && finalClick && !pastClick))
                            {
                                leftItemShop.setDarkBackground(true);
                                //메시지 박스 띄우기  
                                selectedItem = shopLeftItems[j];
                                //이미 산거이면 true
                                if (leftItemShop.haveOne(shopLeftItems[j]))
                                {
                                    //장착 메시지 or 팔기 메시지 박스 띄우기 
                                    leftItemShop.setSellOrWearOne(true);
                                    i = rectLeftItems.Count;
                                }
                                else
                                {
                                    leftItemShop.setBuyOne(true);
                                    i = rectLeftItems.Count;
                                }
                                mouseStatePrevious = mouseStateCurrent;
                                pastClick = finalClick;
                            }

                        }
                    }


                    if (mouseRectinleft.Intersects(leftItemShop.getRectPreviousButton()) || drawrec1.Intersects(leftItemShop.getRectPreviousButton()))
                    {
                        leftItemShop.setClickPreviousButton(true);
                        //click the right hand item section
                        if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
                        {
                            gameState = GameStates.ShopDoor;
                        }
                    }
                    else
                    {
                        leftItemShop.setClickPreviousButton(false);
                    }


                   break;

               #endregion 

               #region 노트상점

               case GameStates.NoteItemShop:
                   //상점대문으로 돌아가는 키보드처리
                   HandleKeyboardInputinItemShop(Keyboard.GetState());

                   Rectangle mouseRectinNote = new Rectangle(mouseStateCurrent.X, mouseStateCurrent.Y, 5, 5);
                   int s;
                   //클릭했을때 어두워지는것.
                   List<Rectangle> rectNoteItems = noteItemShop.getRectNoteItem();
                   List<Item> shopNoteItems = noteItemShop.getShopNoteItem();


                   //돈 부족 메시지 
                   if (noteItemShop.getNoGold())
                   {
                       if (mouseRectinNote.Intersects(noteItemShop.getRectNoGoldButton()) || drawrec1.Intersects(noteItemShop.getRectNoGoldButton()))
                       {

                           noteItemShop.setHoverNoGoldButton(true);
                           if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
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

                   //장착한 아이템 팔 수없게 
                   if (noteItemShop.getHandInItem())
                   {
                       //버튼 Hover
                       if (mouseRectinNote.Intersects(noteItemShop.getRectHandInItemButton()) || drawrec1.Intersects(noteItemShop.getRectHandInItemButton()))
                       {
                           //눌린모양
                           noteItemShop.setHoverHandInItemButton(true);
                           //버튼 누르면
                           if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
                           {
                               //다시 밝게
                               noteItemShop.setDarkBackground(false);
                               //메시지 없애기
                               noteItemShop.setHandInItem(false);
                           }
                       }
                       //버튼 not hover
                       else
                       {
                           noteItemShop.setHoverHandInItemButton(false);
                       }
                       
                   }




                   //장착할것인지 팔것인지 묻는 메시지

                   if (noteItemShop.getSellOrWearOne())
                   {
                       //마우스가 장착 버튼에 올려졌을 때
                       if (mouseRectinNote.Intersects(noteItemShop.getRectWearButton()) || drawrec1.Intersects(noteItemShop.getRectWearButton()))
                       {
                           noteItemShop.setHoverWearButton(true);

                           if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
                           {
                               //장착 메시지 박스 띄우기 
                               noteItemShop.setWearOne(true);
                           }
                       }
                       else
                       {
                           noteItemShop.setHoverWearButton(false);
                       }


                       //마우스가 팔기 버튼에 눌러졌을 때
                       if (mouseRectinNote.Intersects(noteItemShop.getRectSellButton()) || drawrec1.Intersects(noteItemShop.getRectSellButton()))
                       {

                           noteItemShop.setHoverSellButton(true);

                           if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
                           {
                               //장착 메시지 박스 띄우기 
                               noteItemShop.setSellOne(true);
                           }
                       }
                       else
                       {
                           noteItemShop.setHoverSellButton(false);
                       }


                       //취소버튼 눌렀을 때
                       if (mouseRectinNote.Intersects(noteItemShop.getRectCancelButton()) || drawrec1.Intersects(noteItemShop.getRectCancelButton()))
                       {
                           noteItemShop.setHoverCancelButton(true);
                           if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
                           {
                               //return to normal , remove message box
                               noteItemShop.setSellOrWearOne(false);
                               noteItemShop.setDarkBackground(false);
                           }
                       }
                       else
                       {
                           noteItemShop.setHoverCancelButton(false);
                       }
                      

                   }

                   //message box about wearing item 
                   if (noteItemShop.getWearOne())
                   {
                       //mouse cursor on yes button
                       if (mouseRectinNote.Intersects(noteItemShop.getRectYesButton()) || drawrec1.Intersects(noteItemShop.getRectYesButton()))
                       {
                           noteItemShop.setHoverYesButton(true);
                           if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
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
                       if (mouseRectinNote.Intersects(noteItemShop.getRectNoButton()) || drawrec1.Intersects(noteItemShop.getRectNoButton()))
                       {
                           noteItemShop.setHoverNoButton(true);
                           if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
                           {
                               //return to normal , remove message box
                               noteItemShop.setWearOne(false);
                               noteItemShop.setDarkBackground(false);

                               //추가적으로 sellorwear 버튼도 false로 해야 한다.
                               noteItemShop.setSellOrWearOne(false);
                           }
                       }
                       else
                       {
                           noteItemShop.setHoverNoButton(false);
                       }
                       //mouseStatePrevious = mouseStateCurrent;
                       //pastClick = finalClick;
                   }


                   //팔기 메시지 띄우기
                   //message box about wearing item 
                   if (noteItemShop.getSellOne())
                   {
                       //mouse cursor on yes button
                       if (mouseRectinNote.Intersects(noteItemShop.getRectYesButton()) || drawrec1.Intersects(noteItemShop.getRectYesButton()))
                       {
                           noteItemShop.setHoverSellYesButton(true);

                           if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
                           {
                               //선택된 아이템이 있으면
                               if (selectedItem != null)
                               {
                                   //find the index of item in myrightitem list
                                   //아이템 찾기
                                   int index = itemManager.getIndexOfAllNoteItem(selectedItem);

                                   //찾은 아이템이 장착하고 있는 아이템이면 팔 수 없다.
                                   if (index != itemManager.getNoteIndex())
                                   {
                                       //아이템을 찾았으면
                                       if (index != -1)
                                       {

                                           //전체 돈에서 판 비용 더해줌
                                           scoreManager.TotalGold += selectedItem.GetCost() / 2;

                                           //자신의 인벤토리에서 빼기
                                           noteItemShop.removeItemtoMyItem(selectedItem);


                                           noteItemShop.setSellOne(false);

                                           noteItemShop.setDarkBackground(false);

                                           //돈을 파일에 저장
                                           reportManager.SaveGoldToFile();

                                           //추가적으로 sellorwear 버튼도 false로 해야 한다.
                                           noteItemShop.setSellOrWearOne(false);


                                       }
                                       //아이템을 찾지 못했으면
                                       else
                                       {
                                           Trace.WriteLine("get wrong index( no item in list)");
                                       }
                                   }
                                   else
                                   {
                                       noteItemShop.setSellOrWearOne(false);
                                       noteItemShop.setSellOne(false);


                                       noteItemShop.setHandInItem(true);

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
                           noteItemShop.setHoverSellYesButton(false);
                       }
                       //mouse cursor on no button
                       //노버튼 눌렀을 때
                       if (mouseRectinNote.Intersects(noteItemShop.getRectNoButton()) || drawrec1.Intersects(noteItemShop.getRectNoButton()))
                       {
                           noteItemShop.setHoverSellNoButton(true);
                           if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
                           {
                               //return to normal , remove message box
                               noteItemShop.setSellOne(false);
                               noteItemShop.setDarkBackground(false);
                           }
                       }
                       else
                       {
                           noteItemShop.setHoverSellNoButton(false);
                       }
                      

                   }


                   //message box about buying item
                   if (noteItemShop.getBuyOne())
                   {
                       //mouse cursor on right button

                       if (mouseRectinNote.Intersects(noteItemShop.getRectYesButton()) || drawrec1.Intersects(noteItemShop.getRectYesButton()))
                       {
                           noteItemShop.setHoverYesButton(true);
                           if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
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
                       if (mouseRectinNote.Intersects(noteItemShop.getRectNoButton()) || drawrec1.Intersects(noteItemShop.getRectNoButton()))
                       {
                           noteItemShop.setHoverNoButton(true);
                           if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
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


                   if (!noteItemShop.getWearOne() && !noteItemShop.getBuyOne() && !noteItemShop.getNoGold() && !noteItemShop.getHandInItem())
                   {
                       for (s = 0; s < rectNoteItems.Count; s++)
                       {

                           if ((mouseRectinNote.Intersects(rectNoteItems[s]) && mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released)
                          || (drawrec1.Intersects(rectNoteItems[s]) && finalClick && !pastClick))
                           {
                               noteItemShop.setDarkBackground(true);
                               //메시지 박스 띄우기  
                               selectedItem = shopNoteItems[s];
                               //이미 산거이면 true
                               if (noteItemShop.haveOne(shopNoteItems[s]))
                               {
                                   //장착 메시지 or 팔기 메시지 박스 띄우기 
                                   noteItemShop.setSellOrWearOne(true);
                                   s = rectNoteItems.Count();
                               }
                               else
                               {
                                   noteItemShop.setBuyOne(true);
                                   s = rectNoteItems.Count();
                               }
                               mouseStatePrevious = mouseStateCurrent;
                               pastClick = finalClick;
                           }

                       }
                   }


                   if (mouseRectinNote.Intersects(noteItemShop.getRectPreviousButton()) || drawrec1.Intersects(noteItemShop.getRectPreviousButton()))
                   {
                       noteItemShop.setClickPreviousButton(true);
                       //click the right hand item section
                       if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
                       {
                           gameState = GameStates.ShopDoor;
                       }
                   }
                   else
                   {
                       noteItemShop.setClickPreviousButton(false);
                   }
                   break;
               #endregion
               #region 이펙트상점


               case GameStates.EffectItemShop:
                   //상점대문으로 돌아가는 키보드처리
                   HandleKeyboardInputinItemShop(Keyboard.GetState());

                   Rectangle mouseRectinEffect = new Rectangle(mouseStateCurrent.X, mouseStateCurrent.Y, 5, 5);
                   int k;
                   //클릭했을때 어두워지는것.
                   List<Rectangle> rectEffectItems = effectItemShop.getRectEffectItem();
                   List<Item> shopEffectItems = effectItemShop.getShopEffectItem();

                 
                   //돈 부족 메시지 
                   if (effectItemShop.getNoGold())
                   {
                       if (mouseRectinEffect.Intersects(effectItemShop.getRectNoGoldButton()) || drawrec1.Intersects(effectItemShop.getRectNoGoldButton()))
                       {

                           effectItemShop.setHoverNoGoldButton(true);
                           if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
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



                   //장착한 아이템 팔 수없게 
                   if (effectItemShop.getHandInItem())
                   {
                       //버튼 Hover
                       if (mouseRectinEffect.Intersects(effectItemShop.getRectHandInItemButton()) || drawrec1.Intersects(effectItemShop.getRectHandInItemButton()))
                       {
                           //눌린모양
                           effectItemShop.setHoverHandInItemButton(true);
                           //버튼 누르면
                           if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
                           {
                               //다시 밝게
                               effectItemShop.setDarkBackground(false);
                               //메시지 없애기
                               effectItemShop.setHandInItem(false);
                           }
                       }
                       //버튼 not hover
                       else
                       {
                           effectItemShop.setHoverHandInItemButton(false);
                       }
                       
                   }




                   //장착할것인지 팔것인지 묻는 메시지

                   if (effectItemShop.getSellOrWearOne())
                   {
                       //마우스가 장착 버튼에 올려졌을 때
                       if (mouseRectinEffect.Intersects(effectItemShop.getRectWearButton()) || drawrec1.Intersects(effectItemShop.getRectWearButton()))
                       {
                           effectItemShop.setHoverWearButton(true);

                           if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
                           {
                               //장착 메시지 박스 띄우기 
                               effectItemShop.setWearOne(true);
                           }
                       }
                       else
                       {
                           effectItemShop.setHoverWearButton(false);
                       }


                       //마우스가 팔기 버튼에 눌러졌을 때
                       if (mouseRectinEffect.Intersects(effectItemShop.getRectSellButton()) || drawrec1.Intersects(effectItemShop.getRectSellButton()))
                       {

                           effectItemShop.setHoverSellButton(true);

                           if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
                           {
                               //장착 메시지 박스 띄우기 
                               effectItemShop.setSellOne(true);
                           }
                       }
                       else
                       {
                           effectItemShop.setHoverSellButton(false);
                       }


                       //취소버튼 눌렀을 때
                       if (mouseRectinEffect.Intersects(effectItemShop.getRectCancelButton()) || drawrec1.Intersects(effectItemShop.getRectCancelButton()))
                       {
                           effectItemShop.setHoverCancelButton(true);
                           if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
                           {
                               //return to normal , remove message box
                               effectItemShop.setSellOrWearOne(false);
                               effectItemShop.setDarkBackground(false);
                           }
                       }
                       else
                       {
                           effectItemShop.setHoverCancelButton(false);
                       }
                       
                   }

                   //message box about wearing item 
                   if (effectItemShop.getWearOne())
                   {
                       //mouse cursor on yes button
                       if (mouseRectinEffect.Intersects(effectItemShop.getRectYesButton()) || drawrec1.Intersects(effectItemShop.getRectYesButton()))
                       {
                           effectItemShop.setHoverYesButton(true);
                           if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
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
                       if (mouseRectinEffect.Intersects(effectItemShop.getRectNoButton()) || drawrec1.Intersects(effectItemShop.getRectNoButton()))
                       {
                           effectItemShop.setHoverNoButton(true);
                           if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
                           {
                               //return to normal , remove message box
                               effectItemShop.setWearOne(false);
                               effectItemShop.setDarkBackground(false);

                               //추가적으로 sellorwear 버튼도 false로 해야 한다.
                               effectItemShop.setSellOrWearOne(false);
                           }
                       }
                       else
                       {
                           effectItemShop.setHoverNoButton(false);
                       }
                     
                   }


                   //팔기 메시지 띄우기
                   //message box about wearing item 
                   if (effectItemShop.getSellOne())
                   {
                       //mouse cursor on yes button
                       if (mouseRectinEffect.Intersects(effectItemShop.getRectYesButton()) || drawrec1.Intersects(effectItemShop.getRectYesButton()))
                       {
                           effectItemShop.setHoverSellYesButton(true);

                           if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
                           {
                               //선택된 아이템이 있으면
                               if (selectedItem != null)
                               {
                                   //find the index of item in myrightitem list
                                   //아이템 찾기
                                   int index = itemManager.getIndexOfAllEffectItem(selectedItem);

                                   //찾은 아이템이 장착하고 있는 아이템이면 팔 수 없다.
                                   if (index != itemManager.getEffectIndex())
                                   {
                                       //아이템을 찾았으면
                                       if (index != -1)
                                       {

                                           //전체 돈에서 판 비용 더해줌
                                           scoreManager.TotalGold += selectedItem.GetCost() / 2;

                                           //자신의 인벤토리에서 빼기
                                           effectItemShop.removeItemtoMyItem(selectedItem);


                                           effectItemShop.setSellOne(false);

                                           effectItemShop.setDarkBackground(false);

                                           //돈을 파일에 저장
                                           reportManager.SaveGoldToFile();

                                           //추가적으로 sellorwear 버튼도 false로 해야 한다.
                                           effectItemShop.setSellOrWearOne(false);


                                       }
                                       //아이템을 찾지 못했으면
                                       else
                                       {
                                           Trace.WriteLine("get wrong index( no item in list)");
                                       }
                                   }
                                   else
                                   {
                                       effectItemShop.setSellOrWearOne(false);
                                       effectItemShop.setSellOne(false);


                                       effectItemShop.setHandInItem(true);

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
                           effectItemShop.setHoverSellYesButton(false);
                       }
                       //mouse cursor on no button
                       //노버튼 눌렀을 때
                       if (mouseRectinEffect.Intersects(effectItemShop.getRectNoButton()) || drawrec1.Intersects(effectItemShop.getRectNoButton()))
                       {
                           effectItemShop.setHoverSellNoButton(true);
                           if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
                           {
                               //return to normal , remove message box
                               effectItemShop.setSellOne(false);
                               effectItemShop.setDarkBackground(false);
                           }
                       }
                       else
                       {
                           effectItemShop.setHoverSellNoButton(false);
                       }
                       //mouseStatePrevious = mouseStateCurrent;
                       //pastClick = finalClick;
                   }

                   //message box about buying item
                   if (effectItemShop.getBuyOne())
                   {
                       //mouse cursor on right button

                       if (mouseRectinEffect.Intersects(effectItemShop.getRectYesButton()) || drawrec1.Intersects(effectItemShop.getRectYesButton()))
                       {
                           effectItemShop.setHoverYesButton(true);
                           if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
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
                       if (mouseRectinEffect.Intersects(effectItemShop.getRectNoButton()) || drawrec1.Intersects(effectItemShop.getRectNoButton()))
                       {
                           effectItemShop.setHoverNoButton(true);
                           if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
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
                       //mouseStatePrevious = mouseStateCurrent;
                       //pastClick = finalClick;
                   }

                   if (!effectItemShop.getWearOne() && !effectItemShop.getBuyOne() && !effectItemShop.getNoGold() && !effectItemShop.getHandInItem())
                   {
                       for (k = 0; k < rectEffectItems.Count; k++)
                       {
                           if ((mouseRectinEffect.Intersects(rectEffectItems[k]) && mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released)
                             || (drawrec1.Intersects(rectEffectItems[k]) && finalClick && !pastClick))
                           {
                               effectItemShop.setDarkBackground(true);
                               //메시지 박스 띄우기  
                               selectedItem = shopEffectItems[k];
                               //이미 산거이면 true
                               if (effectItemShop.haveOne(shopEffectItems[k]))
                               {
                                   //장착 메시지 or 팔기 메시지 박스 띄우기 
                                   effectItemShop.setSellOrWearOne(true);
                                   k = rectEffectItems.Count();
                               }
                               else
                               {
                                   effectItemShop.setBuyOne(true);
                                   k = rectEffectItems.Count();
                               }
                               mouseStatePrevious = mouseStateCurrent;
                               pastClick = finalClick;
                           }

                       }
                   }
                   if (mouseRectinEffect.Intersects(effectItemShop.getRectPreviousButton()) || drawrec1.Intersects(effectItemShop.getRectPreviousButton()))
                   {
                       effectItemShop.setClickPreviousButton(true);
                       //click the right hand item section
                       if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
                       {
                           gameState = GameStates.ShopDoor;
                       }
                   }
                   else
                   {
                       effectItemShop.setClickPreviousButton(false);
                   }
                   break;
               #endregion



               #region 배경상점
               case GameStates.BackgroundItemShop:
                   //상점대문으로 돌아가는 키보드처리
                   HandleKeyboardInputinItemShop(Keyboard.GetState());

                   Rectangle mouseRectinBackground = new Rectangle(mouseStateCurrent.X, mouseStateCurrent.Y, 5, 5);
                   int r;
                   //클릭했을때 어두워지는것.

                   List<Rectangle> rectBackgroundItems = backgroundItemShop.getRectBackgroundItem();
                   List<Item> shopBackgroundItems = backgroundItemShop.getShopBackgroundItem();

                  

                   //돈 부족 메시지 
                   if (backgroundItemShop.getNoGold())
                   {
                       if (mouseRectinBackground.Intersects(backgroundItemShop.getRectNoGoldButton()) || drawrec1.Intersects(backgroundItemShop.getRectNoGoldButton()))
                       {

                           backgroundItemShop.setHoverNoGoldButton(true);
                           if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
                           {

                               backgroundItemShop.setDarkBackground(false);

                               backgroundItemShop.setNoGold(false);
                           }

                       }
                       else
                       {
                           backgroundItemShop.setHoverNoGoldButton(false);
                       }
                       //mouseStatePrevious = mouseStateCurrent;
                       //pastClick = finalClick;

                   }

                   //if (!backgroundItemShop.getHandInItem())
                   //{

                       //장착한 아이템 팔 수없게 
                       if (backgroundItemShop.getHandInItem())
                       {
                           //버튼 Hover
                           if (mouseRectinBackground.Intersects(backgroundItemShop.getRectHandInItemButton()) || drawrec1.Intersects(backgroundItemShop.getRectHandInItemButton()))
                           {
                               //눌린모양
                               backgroundItemShop.setHoverHandInItemButton(true);
                               //버튼 누르면
                               if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
                               {
                                   //다시 밝게
                                   backgroundItemShop.setDarkBackground(false);
                                   //메시지 없애기
                                   backgroundItemShop.setHandInItem(false);
                               }
                           }
                           //버튼 not hover
                           else
                           {
                               backgroundItemShop.setHoverHandInItemButton(false);
                           }
                           //mouseStatePrevious = mouseStateCurrent;
                           //pastClick = finalClick;
                       //}


                   }

                   //장착할것인지 팔것인지 묻는 메시지

                   if (backgroundItemShop.getSellOrWearOne())
                   {
                       //마우스가 장착 버튼에 올려졌을 때
                       if (mouseRectinBackground.Intersects(backgroundItemShop.getRectWearButton()) || drawrec1.Intersects(backgroundItemShop.getRectWearButton()))
                       {
                           backgroundItemShop.setHoverWearButton(true);

                           if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
                           {
                               //장착 메시지 박스 띄우기 
                               backgroundItemShop.setWearOne(true);
                           }
                       }
                       else
                       {
                           backgroundItemShop.setHoverWearButton(false);
                       }


                       //마우스가 팔기 버튼에 눌러졌을 때
                       if (mouseRectinBackground.Intersects(backgroundItemShop.getRectSellButton()) || drawrec1.Intersects(backgroundItemShop.getRectSellButton()))
                       {

                           backgroundItemShop.setHoverSellButton(true);

                           if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
                           {
                               //장착 메시지 박스 띄우기 
                               backgroundItemShop.setSellOne(true);
                           }
                       }
                       else
                       {
                           backgroundItemShop.setHoverSellButton(false);
                       }


                       //취소버튼 눌렀을 때
                       if (mouseRectinBackground.Intersects(backgroundItemShop.getRectCancelButton()) || drawrec1.Intersects(backgroundItemShop.getRectCancelButton()))
                       {
                           backgroundItemShop.setHoverCancelButton(true);
                           if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
                           {
                               //return to normal , remove message box
                               backgroundItemShop.setSellOrWearOne(false);
                               backgroundItemShop.setDarkBackground(false);
                           }
                       }
                       else
                       {
                           backgroundItemShop.setHoverCancelButton(false);
                       }
                       //mouseStatePrevious = mouseStateCurrent;
                       //pastClick = finalClick;

                   }

                   





                   //message box about wearing item 
                   if (backgroundItemShop.getWearOne())
                   {
                       //mouse cursor on yes button
                       if (mouseRectinBackground.Intersects(backgroundItemShop.getRectYesButton()) || drawrec1.Intersects(backgroundItemShop.getRectYesButton()))
                       {
                           backgroundItemShop.setHoverYesButton(true);
                           if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
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
                       if (mouseRectinBackground.Intersects(backgroundItemShop.getRectNoButton()) || drawrec1.Intersects(backgroundItemShop.getRectNoButton()))
                       {
                           backgroundItemShop.setHoverNoButton(true);
                           if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
                           {
                               //return to normal , remove message box
                               backgroundItemShop.setWearOne(false);
                               backgroundItemShop.setDarkBackground(false);

                               //추가적으로 sellorwear 버튼도 false로 해야 한다.
                               backgroundItemShop.setSellOrWearOne(false);
                           }
                       }
                       else
                       {
                           backgroundItemShop.setHoverNoButton(false);
                       }

                       //mouseStatePrevious = mouseStateCurrent;
                       //pastClick = finalClick;
                   }



                   //팔기 메시지 띄우기
                   //message box about wearing item 
                   if (backgroundItemShop.getSellOne())
                   {
                       //mouse cursor on yes button
                       if (mouseRectinBackground.Intersects(backgroundItemShop.getRectYesButton()) || drawrec1.Intersects(backgroundItemShop.getRectYesButton()))
                       {
                           backgroundItemShop.setHoverSellYesButton(true);

                           if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
                           {
                               //선택된 아이템이 있으면
                               if (selectedItem != null)
                               {
                                   //find the index of item in myrightitem list
                                   //아이템 찾기
                                   int index = itemManager.getIndexOfAllBackgroundItem(selectedItem);

                                   //찾은 아이템이 장착하고 있는 아이템이면 팔 수 없다.
                                   if (index != itemManager.getBackgroundIndex())
                                   {
                                       //아이템을 찾았으면
                                       if (index != -1)
                                       {

                                           //전체 돈에서 판 비용 더해줌
                                           scoreManager.TotalGold += selectedItem.GetCost() / 2;

                                           //자신의 인벤토리에서 빼기
                                           backgroundItemShop.removeItemtoMyItem(selectedItem);


                                           backgroundItemShop.setSellOne(false);

                                           backgroundItemShop.setDarkBackground(false);

                                           //돈을 파일에 저장
                                           reportManager.SaveGoldToFile();

                                           //추가적으로 sellorwear 버튼도 false로 해야 한다.
                                           backgroundItemShop.setSellOrWearOne(false);


                                       }
                                       //아이템을 찾지 못했으면
                                       else
                                       {
                                           Trace.WriteLine("get wrong index( no item in list)");
                                       }
                                   }
                                   else
                                   {
                                       backgroundItemShop.setSellOrWearOne(false);
                                       backgroundItemShop.setSellOne(false);


                                       backgroundItemShop.setHandInItem(true);

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
                           backgroundItemShop.setHoverSellYesButton(false);
                       }
                       //mouse cursor on no button
                       //노버튼 눌렀을 때
                       if (mouseRectinBackground.Intersects(backgroundItemShop.getRectNoButton()) || drawrec1.Intersects(backgroundItemShop.getRectNoButton()))
                       {
                           backgroundItemShop.setHoverSellNoButton(true);
                           if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
                           {
                               //return to normal , remove message box
                               backgroundItemShop.setSellOne(false);
                               backgroundItemShop.setDarkBackground(false);
                           }
                       }
                       else
                       {
                           backgroundItemShop.setHoverSellNoButton(false);
                       }

                       //mouseStatePrevious = mouseStateCurrent;
                       //pastClick = finalClick;

                   }


                   //message box about buying item
                   if (backgroundItemShop.getBuyOne())
                   {
                       //mouse cursor on right button

                       if (mouseRectinBackground.Intersects(backgroundItemShop.getRectYesButton()) || drawrec1.Intersects(backgroundItemShop.getRectYesButton()))
                       {
                           backgroundItemShop.setHoverYesButton(true);
                           if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
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
                       if (mouseRectinBackground.Intersects(backgroundItemShop.getRectNoButton()) || drawrec1.Intersects(backgroundItemShop.getRectNoButton()))
                       {
                           backgroundItemShop.setHoverNoButton(true);
                           if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
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
                       //mouseStatePrevious = mouseStateCurrent;
                       //pastClick = finalClick;
                   }

                   if (!backgroundItemShop.getWearOne() && !backgroundItemShop.getBuyOne() && !backgroundItemShop.getNoGold() && !backgroundItemShop.getHandInItem())
                   {

                       for (r = 0; r < rectBackgroundItems.Count; r++)
                       {

                           if ((mouseRectinBackground.Intersects(rectBackgroundItems[r]) && mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released)
                             || (drawrec1.Intersects(rectBackgroundItems[r]) && finalClick && !pastClick))
                           {
                               backgroundItemShop.setDarkBackground(true);
                               //메시지 박스 띄우기  
                               selectedItem = shopBackgroundItems[r];
                               //이미 산거이면 true
                               if (backgroundItemShop.haveOne(shopBackgroundItems[r]))
                               {
                                   //장착 메시지 or 팔기 메시지 박스 띄우기 
                                   backgroundItemShop.setSellOrWearOne(true);
                               }
                               else
                               {
                                   backgroundItemShop.setBuyOne(true);
                               }
                               mouseStatePrevious = mouseStateCurrent;
                               pastClick = finalClick;
                           }

                       }
                   }

                   if (mouseRectinBackground.Intersects(backgroundItemShop.getRectPreviousButton()) || drawrec1.Intersects(backgroundItemShop.getRectPreviousButton()))
                   {
                       backgroundItemShop.setClickPreviousButton(true);
                       //click the right hand item section
                       if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
                       {
                           gameState = GameStates.ShopDoor;
                       }
                   }
                   else
                   {
                       backgroundItemShop.setClickPreviousButton(false);
                   }
                   break;

               #endregion
               #endregion

               #region 플레이화면
               case GameStates.Playing:
                   if (scoreManager.Gage < 1)
                   {
                       file.SetEndFile(true);
                   }

                   //곡이 끝내게 되면 결과 화면으로
                   //go to result scene right after finishing a piece
                   if (file.GetEndFile())
                   {
                      

                       //노래 총 시간으로 끝을 바꾸자
                       gameState = GameStates.ResultManager;
                       //점수기록판에 기재
                      
                       //reportManager의 scoreInfoManager에 곡명과 자기사진 추가
                       //여기에 현재 자신의 사진 이름이 들어가야 함.(날짜시간 포함해서 독립적으로)
                       if(ScorePic == null)
                       {
                           ScorePic = "myPicture.jpg";
                       }


                       reportManager.AddSongInfoManager(scoreManager.SongName, scoreManager.TotalScore, ScorePic);


                       isScorePic = false;
                       //현재 노래 제목
                       currentSongName = scoreManager.SongName;
                       
                       //점수 기록 파일로 저장
                       //save recored scores in the file
                       reportManager.SaveReport();

                       //gold 파일에  저장
                       scoreManager.TotalGold += scoreManager.Gold;
                       reportManager.SaveGoldToFile();

                       //도중에 꺼져도 모든 드래그모양 지우기 

                       curveManager.DeleteAllCurve();

                       //도중에 꺼져도 두번째 가이드라인 지우기 
                       
                       guideLineManager.DeleteAllSecondGuideLine();

                       //기록판에 보여줄 유저 사진 찾기
                       //Fine user pictures which will be seen in score board
                       reportManager.MakePictures(currentSongName, GraphicsDevice);

                       playPicturesCount = 0;
                       showPictureTextures = new Texture2D[5];
                       //Texture2D texture = null; 
                       //Stream str = System.IO.File.OpenWrite("gesture.jpg");
                       //texture.SaveAsJpeg(str, 1200, 900);


                       SoundFmod.StopSound();

                       resultNumberManager.AddResultNumbers(new Vector2(200, 300), scoreManager.Perfect);
               
                   }

                   //마크 업데이트
                    MarkManager.Update(gameTime);
                    startNoteManager.Update(gameTime);
                    HandleKeyboardInput(Keyboard.GetState());
                    HandleMouseInput(Mouse.GetState());
                    file.Update(spriteBatch, gameTime, SoundFmod.changedTempo, SoundFmod.optionalTime);
                    //*** 어떻게 돼는건지 모르겠음 
                    DragNoteManager.Update(gameTime);
                   ////
                    GoldManager.Update(gameTime);
                    perfectManager.Update(gameTime);
                    goodManager.Update(gameTime);
                    badManager.Update(gameTime);
                    goldGetManager.Update(gameTime);
                    scoreManager.Update(gameTime);
                    memberManager.Update(gameTime);            
                    SoundFmod.StartChangedTime(gameTime);

                    perfectBannerManager.Update(gameTime);
                    goodBannerManager.Update(gameTime);
                    badBannerManager.Update(gameTime);
                    missBannerManager.Update(gameTime);

                    comboNumberManager.Update(gameTime);
                 //   collisionManager.DeleteMarks();
#if Kinect
                    HandleInput();

                    collisionManager.CheckRightNoteInCenterArea();
                    collisionManager.CheckLeftNoteInCenterArea();

                  //JudgeCharisma();

#endif
                    //3초만에 원상복귀
                    //       AutoRetrunChangeTempo(gameTime);

                break;

               #endregion

               #region Setting
               case GameStates.SettingBoard:

                HandleKeyboardInputGoToMenu(Keyboard.GetState());

                Rectangle rectMouseSettingBoard = new Rectangle(mouseStateCurrent.X, mouseStateCurrent.Y, 5, 5);
                   //nextButton 위에 마우스를 올려놨을 때
                    //mousecursor on nextButton item section
                   
                   //뒤로 버튼
                if (rectMouseSettingBoard.Intersects(settingBoard.RectNextButton) || rightHandPosition.Intersects(settingBoard.RectNextButton))
                {

                    nearButton = true;
                    GetCenterOfButton(settingBoard.RectNextButton);

                    settingBoard.ClickNextButton = true;
                    //click the right hand item section
                    if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
                    {
                        nearButton = false;
                        gameState = GameStates.Menu;
                    }
                }
                else
                {
                    settingBoard.ClickNextButton = false;
                }


                //스케일 증가 
                if (rectMouseSettingBoard.Intersects(settingBoard.RecScaleUpButton) || rightHandPosition.Intersects(settingBoard.RecScaleUpButton))
                {
                    nearButton = true;
                    GetCenterOfButton(settingBoard.RecScaleUpButton);

                    settingBoard.ClickScaleUpButton = true;
                    //click the right hand item section
                    if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
                    {

                        nearButton = false;
                        if (this.userParam < 1)
                        {
                            this.userParam += 0.05f;
                        }
                        else
                        {
                            this.userParam = 1;
                        }


                    }
                }
                else
                {
                    settingBoard.ClickScaleUpButton = false;
                }



                //스케일 감소
                if (rectMouseSettingBoard.Intersects(settingBoard.RecScaleDownButton) || rightHandPosition.Intersects(settingBoard.RecScaleDownButton))
                {
                    nearButton = true;
                    GetCenterOfButton(settingBoard.RecScaleDownButton);

                    settingBoard.ClickScaleDownButton = true;
                    //click the right hand item section
                    if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
                    {
                        nearButton = false;
                        if (this.userParam > 0.05)
                        {
                            this.userParam -= 0.05f;
                        }
                        else
                        {
                            this.userParam = 0.05f;
                        }

                    }
                }
                else
                {
                    settingBoard.ClickScaleDownButton = false;
                }




            //각도 증가 
                if (rectMouseSettingBoard.Intersects(settingBoard.RecAngleUpButton) || rightHandPosition.Intersects(settingBoard.RecAngleUpButton))
                {
                    nearButton = true;
                    GetCenterOfButton(settingBoard.RecAngleUpButton);

                    settingBoard.ClickAngleUpButton = true;
                    //click the right hand item section
                    if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
                    {

                        nearButton = false;
                        if (nui.ElevationAngle < nui.MaxElevationAngle - 3)
                        {
                            while (true)
                            {
                                try
                                {
                                    nui.ElevationAngle += 3;
                                    break;
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                        }
                       
                    }
                }
                else
                {
                    settingBoard.ClickAngleUpButton = false;
                }



                //각도 감소
                if (rectMouseSettingBoard.Intersects(settingBoard.RecAngleDownButton) || rightHandPosition.Intersects(settingBoard.RecAngleDownButton))
                {
                    nearButton = true;
                    GetCenterOfButton(settingBoard.RecAngleDownButton);

                    settingBoard.ClickAngleDownButton = true;
                    //click the right hand item section
                    if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
                    {

                        nearButton = false;
                        if (nui.ElevationAngle > nui.MinElevationAngle + 3)
                        {
                            while (true)
                            {
                                try
                                {
                                    nui.ElevationAngle -= 3;
                                    break;
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                        }
                    }
                }
                else
                {
                    settingBoard.ClickAngleDownButton = false;
                }


                if(
                !(rectMouseSettingBoard.Intersects(settingBoard.RectNextButton) || rightHandPosition.Intersects(settingBoard.RectNextButton))
                &&!(rectMouseSettingBoard.Intersects(settingBoard.RecScaleUpButton) || rightHandPosition.Intersects(settingBoard.RecScaleUpButton))
                &&!(rectMouseSettingBoard.Intersects(settingBoard.RecScaleDownButton) || rightHandPosition.Intersects(settingBoard.RecScaleDownButton))
                &&!(rectMouseSettingBoard.Intersects(settingBoard.RecAngleUpButton) || rightHandPosition.Intersects(settingBoard.RecAngleUpButton))
                &&!(rectMouseSettingBoard.Intersects(settingBoard.RecAngleDownButton) || rightHandPosition.Intersects(settingBoard.RecAngleDownButton))
                )
                   {

                       nearButton = false;
                   }


                pastClick = finalClick;

                break;

               
               
               #endregion

               #region 결과 결산
               //결과 창
                case GameStates.ResultManager:

                   resultNumberManager.Update(gameTime);

                    Rectangle rectMouse = new Rectangle(mouseStateCurrent.X, mouseStateCurrent.Y, 5, 5);
                 
                    
                    //nextButton 위에 마우스를 올려놨을 때
                    //mousecursor on nextButton item section
                    if (rectMouse.Intersects(resultManager.getRectNextButton()) || rightHandPosition.Intersects(resultManager.getRectNextButton()))
                    {
                        nearButton = true;
                        GetCenterOfButton(resultManager.getRectNextButton());



                        resultManager.setClickNextButton(true);
                        //click the right hand item section
                        if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released )|| (finalClick &&!pastClick))
                        {
                            gameState = GameStates.ShowPictures;
                            nearButton = false;
                           
                            //현재 마커 위치 저장
                            //Vector2 mark1Location = MarkManager.Marks[0].MarkSprite.Location;
                            //Vector2 mark2Location = MarkManager.Marks[1].MarkSprite.Location;
                            //Vector2 mark3Location = MarkManager.Marks[2].MarkSprite.Location;
                            //Vector2 mark4Location = MarkManager.Marks[3].MarkSprite.Location;
                            //Vector2 mark5Location = MarkManager.Marks[4].MarkSprite.Location;
                            //Vector2 mark6Location = MarkManager.Marks[5].MarkSprite.Location;

                            //현재 위치 말고, 기본은 0으로 해놓고


                            Vector2 markerSize = MarkManager.GetMarkerSize();


                            Vector2[] zeroIndexMarkers = MarkManager.GetPattern(0);
                         //   removeAreaRec = MarkManager.GetRemoveArea(0);
                            //두번째꺼 재실행시 이상한거 생기는것 방지
                            startNoteManager.DeleateAllNote();
                            //StartNoteManager.rightNoteManager.DeleteAllNote();
                            //StartNoteManager.leftNoteManager.DeleteAllNote();
                            //StartNoteManager.longNoteManager.DeleteAllNote();


                            startNoteManager = new StartNoteManager(
                                spriteSheet,
                                new Rectangle(0, 200, 52, 55),
                                1);

                            //드로우 라인
                            file.SetDrawLine(false);
                            
                            //골드 초기화 
                            GoldManager.DeleteAll();
                            //카리스마
                          //  charismaManager.IsCharismaTime = 0;

                            //froze 방지
                            MarkManager.initialize(
                                // markManager = new MarkManager(
                                spriteSheet,
                                new Rectangle(0, 200, 50, 55),
                                1,
                                zeroIndexMarkers[0],
                                zeroIndexMarkers[1],
                                zeroIndexMarkers[2],
                                zeroIndexMarkers[3],
                                zeroIndexMarkers[4],
                                zeroIndexMarkers[5],
                                startNoteManager
                               

                                );


                            //파일을 다시 만들 필요는 없고 초기화만 시켜주면 된다.
                            //여러개 파일 다시 생성되서

                            //파일 저장
                            //file = new File(startNoteManager, noteFileManager, collisionManager, scoreManager, itemManager, curveManager, guideLineManager);

                            file.SetEndFile(false);
                            file.SetTime(0);

                            //if (!System.IO.File.Exists(songsDir))
                            //{
                            //    System.IO.Directory.CreateDirectory(songsDir);
                            //}

                            //file.FileLoading(songsDir, "*.mnf");
                           
                            scoreManager.init();

                          }
                    }
                    else
                    {
                        nearButton = false;
                        resultManager.setClickNextButton(false);
                    }

                    pastClick = finalClick;
                break;
                #endregion
                   
                #region 사진들
                case GameStates.ShowPictures:
                Rectangle rectMouseShowPictures = new Rectangle(mouseStateCurrent.X, mouseStateCurrent.Y, 5, 5);

                if (rectMouseShowPictures.Intersects(showPictureScene.getRectNextButton()) || rightHandPosition.Intersects(showPictureScene.getRectNextButton()))
                    {

                        nearButton = true;
                        GetCenterOfButton(showPictureScene.getRectNextButton());

                        showPictureScene.setClickNextButton(true);
                        //click the right hand item section
                        if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
                        {
                            nearButton = false;
                            gameState = GameStates.RecordBoard;
                        }
                    }
                    else
                    {
                        nearButton = false;

                        showPictureScene.setClickNextButton(false);
                    }
                    pastClick = finalClick;

                break;
                #endregion 
                #region 순위판
                case GameStates.RecordBoard:

                    Rectangle rectMouseRecordBoard = new Rectangle(mouseStateCurrent.X, mouseStateCurrent.Y, 5, 5);
                   //nextButton 위에 마우스를 올려놨을 때
                    //mousecursor on nextButton item section


                    if (rectMouseRecordBoard.Intersects(recordBoard.getRectNextButton()) || rightHandPosition.Intersects(recordBoard.getRectNextButton()))
                    {
                        nearButton = true;
                        GetCenterOfButton(recordBoard.getRectNextButton());

                        recordBoard.setClickNextButton(true);
                        //click the right hand item section
                        if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (finalClick && !pastClick))
                        {
                            nearButton = false;

                            gameState = GameStates.SongMenu;
                        }
                    }
                    else
                    {
                        nearButton = false;

                        recordBoard.setClickNextButton(false);
                    }
                    pastClick = finalClick;
                break;

                
                #endregion
               
               #region 곡선택메뉴
                case GameStates.SongMenu:





                resultSongMenu = songMenu.Update();
                   
#if Kinect

                //제스쳐//좌우 만 있음
             //   activeRecognizer = CreateRecognizer();
#endif


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

                        Texture2D[] rightNoteTextures = itemManager.GetRightNoteTexture();

                        ////왼손 노트 // 일단은 오른손노트랑 같이 함.
                        Texture2D[] leftNoteTextures = itemManager.GetLeftNoteTexture();

                        ////롱노트 // 일단은 오른손노트랑 같이 함.
                        Texture2D[] longNoteTextures = itemManager.GetLongNoteTexture();

                        ////노트 - 달라야 될때도 있어서 나누어 놨다.
                        ////오른손노트

                        //노트 맞는 scale 
                        float[] rightNoteScale = itemManager.GetRightNoteScale();
                        float[] leftNoteScale = itemManager.GetRightNoteScale();
                        float[] longNoteScale = itemManager.GetRightNoteScale();
                       
                       
                        
                        ////롱노트 //*** 임시로 오른손노트랑 똑같은걸로 해놓음
                        startNoteManager.changeLongNoteImage(longNoteTextures[itemManager.getNoteIndex()], new Rectangle(0, 0, longNoteTextures[itemManager.getNoteIndex()].Width, longNoteTextures[itemManager.getNoteIndex()].Height), longNoteScale[0]);

                        ////왼손노트
                        startNoteManager.changeLeftNoteImage(leftNoteTextures[itemManager.getNoteIndex()], new Rectangle(0, 0, leftNoteTextures[itemManager.getNoteIndex()].Width, leftNoteTextures[itemManager.getNoteIndex()].Height), leftNoteScale[0]);

                        //오른손 노트 이미지 바꾸기
                        startNoteManager.changeRightNoteImage(rightNoteTextures[itemManager.getNoteIndex()], new Rectangle(0, 0, rightNoteTextures[itemManager.getNoteIndex()].Width, rightNoteTextures[itemManager.getNoteIndex()].Height), rightNoteScale[0]);


                        //마커리스트 텍스쳐 가져오기
                        Texture2D[] markersTextures = itemManager.GetMarkerTexture();
                        
                        //*** 마커리스트에 맞는 rect, 바로width와 height 를 가져와서 넣기 때문에 필요 없을 수 도 있다.
                        //Rectangle[] markersRectangle = new Rectangle[5];
                        //markersRectangle[0] = new Rectangle(0,0,265,240);

                        //***마커리스트에 맞는 =>>itemManager로 옮김
                        float[] markersScale = itemManager.GetMarkersScale();


                        
                        for (int q = 0; q < 6; q++)
                        {
                            memberManager.SetMemberState(q, 0);
                        }
                        
                        //현재 장착한 마커로 설정//(마커,마커의 rect크기. scale)
                        //개별 마커및 전체 마커에도 설정함
                        MarkManager.chageMarksImages(markersTextures[itemManager.getNoteIndex()], new Rectangle(0,0,markersTextures[itemManager.getNoteIndex()].Width,markersTextures[itemManager.getNoteIndex()].Height), markersScale[0]);
                        
                        //노트 사라지는 영역 지정을 위해 사용
                        Vector2 markerSize = MarkManager.GetMarkerSize();

                        //일단 처음은 항상 0 부터 시작
                      //  MarkManager.GetPattern(0, (int)markerSize.X, (int)markerSize.Y);
                        MarkManager.SetRemoveArea(0, (int)markerSize.X, (int)markerSize.Y);
                     

                      //  Vector2[] zeroIndexMarkers = MarkManager.GetPattern(0, (int)markerSize.X, (int)markerSize.Y);
                        //   removeAreaRec = MarkManager.GetRemoveArea(0);
                  

 
                        //MarkManager.initialize(
                        //    // markManager = new MarkManager(
                        //    markersTextures[itemManager.getNoteIndex()],
                        //   new Rectangle(0, 0, markersTextures[itemManager.getNoteIndex()].Width, markersTextures[itemManager.getNoteIndex()].Height),
                        //    1,
                        //    zeroIndexMarkers[0],
                        //    zeroIndexMarkers[1],
                        //    zeroIndexMarkers[2],
                        //    zeroIndexMarkers[3],
                        //    zeroIndexMarkers[4],
                        //    zeroIndexMarkers[5],
                        //    startNoteManager,
                        //    markersScale[0]


                        //    );


                        /////이펙트 생성 -START
                        Texture2D[] explosionTexture = itemManager.GetEffectTexture();
                        
                        //특성 로드
                        Rectangle[] effectInitFrame = itemManager.GetEffectInitFrame();
                        int[] effectFramCount = itemManager.GetEffectFrameCount();
                        float[] effecScale = itemManager.GetEffectScale();

        
                        //이펙트 
                        int effectIndex = itemManager.getEffectIndex();

                        perfectManager = new PerfectExplosionManager();
                        perfectManager.ExplosionInit(itemManager.GetEffectTexture()[effectIndex], itemManager.GetEffectInitFrame()[effectIndex], itemManager.GetEffectFrameCount()[effectIndex], itemManager.GetEffectScale()[effectIndex], itemManager.GetEffectDulation()[effectIndex]);

                        goodManager = new GoodExplosionManager();
                        goodManager.ExplosionInit(itemManager.GetGoodEffectTexture()[effectIndex], itemManager.GetEffectInitFrame()[effectIndex], itemManager.GetEffectFrameCount()[effectIndex], itemManager.GetEffectScale()[effectIndex], itemManager.GetEffectDulation()[effectIndex]);

                        badManager = new BadExplosionManager();
                        badManager.ExplosionInit(itemManager.GetBadEffectTexture()[effectIndex], itemManager.GetEffectInitFrame()[effectIndex], itemManager.GetEffectFrameCount()[effectIndex], itemManager.GetEffectScale()[effectIndex], itemManager.GetEffectDulation()[effectIndex]);

                        //미스도 투입되면
                        //missManager = new ExplosionManager();
                        //missManager.ExplosionInit(missEffectTextures[effectIndex], new Rectangle(0, 0, 166, 162), 9, 1f, 45);

                        goldGetManager = new ExplosionManager();
                        goldGetManager.ExplosionInit(getGold, new Rectangle(0, 0, 200, 200), 5, 0.7f, 30);

                        ////일단은 miss effect로
                        //goldGetManager = new ExplosionManager();
                        //goldGetManager.ExplosionInit(itemManager.GetMissEffectTexture()[effectIndex], itemManager.GetEffectInitFrame()[effectIndex], itemManager.GetEffectFrameCount()[effectIndex], itemManager.GetEffectScale()[effectIndex], itemManager.GetEffectDulation()[effectIndex]);

                        collisionManager = new CollisionManager(perfectManager, goodManager, badManager, goldGetManager, scoreManager, memberManager, itemManager, perfectBannerManager, goodBannerManager, badBannerManager, missBannerManager, new Vector2(this.Window.ClientBounds.Width, this.Window.ClientBounds.Height), comboNumberManager, charismaManager);
            
                        /////이펙트 생성 -END
                        
                        gameState = GameStates.Playing;
                        
                        file.Loading(resultSongMenu);
                        
                        //일반 0
                        //로딩중 1
                        //준비완료 2
                        //isReady = 1;

                        //노래 재생

                       // isReady = SoundFmod.PlaySound(songsDir + noteFileManager.noteFiles[resultSongMenu].Mp3);

                        SoundFmod.PlaySound(songsDir + noteFileManager.noteFiles[resultSongMenu].Mp3);
                       
                    }

                    break;
               #endregion
           }
           mouseStatePrevious = mouseStateCurrent;
           pastClick = finalClick;
           base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        
#if Kinect
        void getDepthFrame()
        {
            //DepthImageFrame ImageParam = null;
            //try
            //{
            //    ImageParam = e.OpenDepthImageFrame();
            //}
            //catch (Exception ex)
            //{

            //}
            DepthImageFrame ImageParam;
            try
            {
                 ImageParam= nui.DepthStream.OpenNextFrame(0);
            }
            catch(IndexOutOfRangeException e )
            {
                ImageParam = null;
            }
                if (ImageParam == null)
                return;

            ImageBits = new short[ImageParam.PixelDataLength];
            ImageParam.CopyPixelDataTo(ImageBits);

            /////////////////////////////// 클릭이 필요할때///////////
            depthLocation = new ColorImagePoint[ImageParam.PixelDataLength];

            nui.MapDepthFrameToColorFrame(DepthImageFormat.Resolution640x480Fps30, ImageBits, ColorImageFormat.RgbResolution640x480Fps30, depthLocation);

            int handDepth = handPoint.Depth;
            int tempWidth = 110;
            int tempHeight = 110;


            KinectVideoTexture = new Texture2D(GraphicsDevice, tempWidth, tempHeight);
            Color[] bitmap = new Color[tempWidth * tempHeight];
            bitmap[0] = new Color(255, 255, 255, 255);

            if (handPoint.X < tempWidth / 2)
            {
                handPoint.X = tempWidth / 2;
            }
            if (handPoint.X > ImageParam.Width - tempWidth / 2)
            {
                handPoint.X = ImageParam.Width - tempWidth / 2;
            }
            if (handPoint.Y < tempHeight / 2)
            {
                handPoint.Y = tempHeight / 2;
            }
            if (handPoint.Y > ImageParam.Height - tempHeight / 2)
            {
                handPoint.Y = ImageParam.Height - tempHeight / 2;
            }



            int indexCount = 0;
            int[] depth = new int[tempHeight * tempWidth];

            if (skeleton != null)
            {

                if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
                    {
                        for (int j = handPoint.Y - tempHeight / 2; j < handPoint.Y + tempHeight / 2; j++)
                        {
                            for (int i = handPoint.X - tempWidth / 2; i < handPoint.X + tempWidth / 2; i++)
                            {
                                //손만
                                ColorImagePoint point = depthLocation[ImageParam.Width * j + i];
                                depth[indexCount] = ImageBits[ImageParam.Width * j + i] >> DepthImageFrame.PlayerIndexBitmaskWidth;//인덱스 오류

                                indexCount++;
                            }
                        }
                    }


            }
            if (skeleton != null)
            {

                if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
                    {
                        bool[][] near = generateValidMatrix(tempWidth, tempHeight, depth);//에러


                        //화면에 띄우기
                        //indexCount = 0;
                        //for (int i = 0; i < tempWidth; i++)
                        //{
                        //    for (int j = 0; j < tempHeight; j++)
                        //    {
                        //        if (near[j][i] == true)
                        //        {
                        //            bitmap[indexCount] = new Color(255, 255, 255, 255);
                        //        }
                        //        else
                        //        {
                        //            bitmap[indexCount] = new Color(0, 0, 0, 255);
                        //        }
                        //        indexCount++;
                        //    }
                        //}
                        //KinectVideoTexture.SetData(bitmap);


                        hands = localizeHands(near);
                        afterDepthReady();
                    }
            }


            if (skeleton != null)
            {

                if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
                    {
                        if (hands.Count > 0)
                        {
                            if (hands[0].fingertips.Count > 0)
                            {
                                clickJudge = false;
                            }
                            else
                            {
                                clickJudge = true;
                            }
                        }
                    }
            }

            //10개의 프레임에 대한 정보를 리스트에 저장
            int maxCount = 10;
            if (listCount < maxCount)
            {
                handList.Add(handPoint);
                clickList.Add(clickJudge);
                listCount++;
            }
            else
            {
                handList.RemoveAt(0);
                clickList.RemoveAt(0);
                handList.Add(handPoint);
                clickList.Add(clickJudge);
            }

            finalClick = true;
            if (listCount >= maxCount)
            {

                double handDistance = Math.Sqrt((handList[0].X - handList[9].X) * (handList[0].X - handList[9].X) + (handList[0].Y - handList[9].Y) * (handList[0].Y - handList[9].Y));
                //message = handDistance.ToString();
                if (clickList[0] == true)
                {
                    if (handDistance > 10)
                    {
                        finalClick = false;
                    }
                    for (int i = 0; i < maxCount - 1; i++)
                    {
                        for (int j = i + 1; j < maxCount; j++)
                        {
                            if (clickList[i] != clickList[j])
                            {
                                finalClick = false;
                            }
                        }
                    }
                }
                else
                {
                    finalClick = false;
                }

            }

            //클릭여부
            if (finalClick == true)
            {
                //message = "click";
            }
            else
            {
                //message = "No click";
            }

            /////////////////////////////////

            //키재기
            if (skeleton != null)
            {

                if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
                {
                    Joint headJoint = skeleton.Joints[JointType.Head];
                    DepthImagePoint depthPoint = ImageParam.MapFromSkeletonPoint(headJoint.Position);
                    fy = (float)depthPoint.Y / (float)ImageParam.Height;



                    foreach (Joint joint in skeleton.Joints)
                    {
                        DepthImagePoint depthP;
                        depthP = ImageParam.MapFromSkeletonPoint(joint.Position);
                        switch (joint.JointType)
                        {
                            case JointType.Head:
                                fheadY = (float)depthP.Y / ImageParam.Height;
                                break;
                            case JointType.ShoulderCenter:
                                fcenterZ = (float)joint.Position.Z;
                                break;
                            case JointType.HipCenter:
                                fhipY = (float)depthP.Y / ImageParam.Height;
                                break;

                        }
                    }
                    double dbVal = (fhipY - fheadY) * 2;
                    dbVal = dbVal * 1.25;
                    factheight = (dbVal * (fcenterZ * 100) - fcenterZ * 2);
                    //message = factheight.ToString();


                }    
                
            }
        }
#endif
    
        protected override void Draw(GameTime gameTime)
        {

            
            GraphicsDevice.Clear(Color.White);
           // spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
            spriteBatch.Begin();


//#if Kinect
//            getDepthFrame();
//#endif

            //타이틀화면
            #region 타이틀화면
            if (gameState == GameStates.Menu)
            {      
                menuScene.Draw(spriteBatch);
                if (isNoPerson)
                {
                    spriteBatch.Draw(noPerson, new Rectangle(200, 200, 727, 278), Color.White);


                }
            }

            #endregion

            #region 상점 대문
            //상전대문
            if (gameState == GameStates.ShopDoor)
            {


                shopDoor.Draw(spriteBatch, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height);


            }

            #endregion
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


            #region 곡 선택
            if (gameState == GameStates.SongMenu)
            {
                songMenu.Draw(spriteBatch);


                spriteBatch.Draw(rightHandTextures[itemManager.getRightHandIndex()], new Vector2(600, 40), null, Color.White, 0f,new Vector2(0, 0), 1f,   SpriteEffects.None, 0.0f);
                spriteBatch.Draw(leftHandTextures[itemManager.getLeftHandIndex()], new Vector2(700, 40), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0.0f);
               // spriteBatch.Draw(rightHandTextures[itemManager.getRightHandIndex()], new Vector2(600, 40), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0.0f);
               // spriteBatch.Draw(rightHandTextures[itemManager.getRightHandIndex()], new Vector2(600, 40), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0.0f);
               // spriteBatch.Draw(rightHandTextures[itemManager.getRightHandIndex()], new Vector2(600, 40), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0.0f);
              



#if Kinect
                if (message.Length > 0)
                {
                    spriteBatch.DrawString(messageFont, message, Vector2.Zero, Color.Red);
                }


#endif
                //if (isReady  == 1)
                //{

                //    spriteBatch.Draw(perfectBanner, new Rectangle(70, 70, 100, 100), Color.White);
             
                //}
                //String name = noteFileManager.noteFiles[songMenu.Scene_number].Name;


            }

            #endregion 

            #region 플레이화면
            if ((gameState == GameStates.Playing))
            {
                //배경
                spriteBatch.Draw(playBackgroud1,
                new Rectangle(0, 0, this.Window.ClientBounds.Width,
                    this.Window.ClientBounds.Height),
                    Color.White);


                double tempo =SoundFmod.changedTempo;
              //  Trace.write(tempo);
                Texture2D metoroTex = GetMetroTexture(tempo);
               
                
                //메트로늄
                spriteBatch.Draw(
                  metoroTex,
                    //위치: Center-> location 으로 바꿈 (마커와 노트 매칭 떄문에 )
                  new Vector2(10, 580),

                  new Rectangle(0, 0, 150, 169),
                  Color.White,
                  0f,
                    //origin ->  new Vector2(frameWidth / 2, frameHeight / 2) ->  new Vector2(0,0) 으로 바꿈 (마커와 노트 매칭 떄문에 )
                  new Vector2(0, 0),
                    //오른쪽 마크 크기 
                  1f,
                  SpriteEffects.None,
                  0.0f);   



                MarkManager.Draw(spriteBatch);
                
                //startnoteclass에 가야 보이고 안보이게 할 수 있음
                startNoteManager.Draw(spriteBatch);
                curveManager.Draw(gameTime, spriteBatch);
                guideLineManager.Draw(gameTime, spriteBatch);
                
                //이걸 주석하면 드래그노트 체크하는거 안보임 하지만 체크는 됨
                //DragNoteManager.Draw(spriteBatch);
                
                memberManager.Draw(spriteBatch);
                GoldManager.Draw(spriteBatch);

                file.Draw(spriteBatch, gameTime);
                perfectManager.Draw(spriteBatch);
                goodManager.Draw(spriteBatch);
                badManager.Draw(spriteBatch);
                goldGetManager.Draw(spriteBatch);

                missBannerManager.Draw(spriteBatch);
                badBannerManager.Draw(spriteBatch);
                goodBannerManager.Draw(spriteBatch);
                perfectBannerManager.Draw(spriteBatch);
                
                comboNumberManager.Draw(spriteBatch);

                
                //가운데 빨간 사각형 주석하면 보이지않는다.
            //    spriteBatch.Draw(idot, removeAreaRec, Color.Red);
                
                //콤보 글씨
                spriteBatch.DrawString(pericles36Font, scoreManager.Perfomance.ToString(), new Vector2(512, 420), Color.Black);
                //골드 글씨
                spriteBatch.DrawString(pericles36Font, scoreManager.Gold.ToString(), new Vector2(scorePosition.X + 120, scorePosition.Y), Color.Black);
                //최대 max
                spriteBatch.DrawString(pericles36Font, scoreManager.Max.ToString(), new Vector2(scorePosition.X + 240, scorePosition.Y), Color.Black);

                //기본 템포 설정( 템포가 바뀐상태이면 안변함)
                SoundFmod.SetBasicTempo();
                //512, 454 중심
                ////test

                spriteBatch.Draw(uiBackground, new Vector2(0, 0), Color.White);
                

                
                //하트. gage양 만큼 하트가 나타남.

                //300은 현재 최대 width, 이건 그림이 바뀌면 바뀜
                //100은 gage의 최대값. 

                int gageWidth = 300 / 100 *  scoreManager.Gage;

                spriteBatch.Draw(uiHeart, new Vector2(0, 6), new Rectangle(0, 0, gageWidth, 50), Color.White);
                //Trace.WriteLine(scoreManager.Gage);




                if (message.Length > 0)
                {
                    spriteBatch.DrawString(messageFont, message, Vector2.Zero, Color.Red);
                }
               


#if Kinect
                charismaManager.Draw(gameTime, spriteBatch);
               
                if (charismaManager.charismaFrames.Count > 0)
                {
                    string fileName;
                    charismaManager.currentTime += gameTime.ElapsedGameTime.TotalSeconds;
                    CharisimaFrame charismaFrame = (CharisimaFrame)charismaManager.charismaFrames.Peek();
                    

                    if (charismaManager.currentTime > charismaFrame.StartTime)
                    {

                        spriteBatch.Draw(charismaFrame.Texture, charismaManager.picLocation, Color.White);
                        if (charismaManager.IsCharismaTime)
                        {
                            charismaManager.PlayCharisma = true;

                            if (charismaManager.Type == 1)
                            {
                                ////카리스마타임 제스쳐 시작부분
                                Skeleton2DDataExtract.Skeleton2DdataCoordReady -= NuiSkeleton2DdataCoordReadyStop;
                                gestureType = 2;
                                postureCount = 0;
                                gestureFlag = true;
                                fileName = "22.txt";
                                LoadGesturesFromFile(fileName);
                                Skeleton2DDataExtract.Skeleton2DdataCoordReady += NuiSkeleton2DdataCoordReadyPosture;
                                


                            }
                            else if (charismaManager.Type == 2)
                            {
                                ////카리스마타임 제스쳐 시작부분
                                Skeleton2DDataExtract.Skeleton2DdataCoordReady -= NuiSkeleton2DdataCoordReadyStop;
                                gestureType = 2;
                                postureCount = 0;
                                gestureFlag = true;
                                fileName = "22.txt";
                                LoadGesturesFromFile(fileName);
                                Skeleton2DDataExtract.Skeleton2DdataCoordReady += NuiSkeleton2DdataCoordReadyPosture;

                            }
                            else if (charismaManager.Type == 3)
                            {
                                ////카리스마타임 제스쳐 시작부분
                                Skeleton2DDataExtract.Skeleton2DdataCoordReady -= NuiSkeleton2DdataCoordReadyStop;
                                gestureType = 2;
                                postureCount = 0;
                                gestureFlag = true;
                                fileName = "22.txt";
                                LoadGesturesFromFile(fileName);
                                Skeleton2DDataExtract.Skeleton2DdataCoordReady += NuiSkeleton2DdataCoordReadyPosture;

                            }
                            else if (charismaManager.Type == 4)
                            {
                                ////카리스마타임 제스쳐 시작부분
                                Skeleton2DDataExtract.Skeleton2DdataCoordReady -= NuiSkeleton2DdataCoordReadyStop;
                                gestureType = 3;
                                gestureFlag = true;
                                fileName = "66.txt";
                                LoadGesturesFromFile(fileName);
                                Skeleton2DDataExtract.Skeleton2DdataCoordReady += NuiSkeleton2DdataCoordReadyGesture;

                            }

                                //중립
                            else if (charismaManager.Type == 6)
                            {
                                ////카리스마타임 제스쳐 시작부분
                                Skeleton2DDataExtract.Skeleton2DdataCoordReady -= NuiSkeleton2DdataCoordReadyStop;
                                gestureType = 2;
                                postureCount = 0;
                                gestureFlag = true;
                                fileName = "33.txt";
                                LoadGesturesFromFile(fileName);
                                Skeleton2DDataExtract.Skeleton2DdataCoordReady += NuiSkeleton2DdataCoordReadyPosture;

                            }
                            charismaManager.IsCharismaTime = false;
                        }
                    }


                    if (!isGesture)
                    {

                        if (gestureType == 2)
                        {
                            Skeleton2DDataExtract.Skeleton2DdataCoordReady -= NuiSkeleton2DdataCoordReadyPosture;
                        }
                        if (gestureType == 3)
                        {
                            Skeleton2DDataExtract.Skeleton2DdataCoordReady -= NuiSkeleton2DdataCoordReadyGesture;
                        }
                        gestureType = 1;
                        postureCount = 0;
                        gestureFlag = true;
                        fileName = "111.txt";
                        LoadGesturesFromFile(fileName);
                        Skeleton2DDataExtract.Skeleton2DdataCoordReady += NuiSkeleton2DdataCoordReadyStop;
                        isGesture = true;
                        charismaManager.charismaFrames.Dequeue();
                    }


                    if (charismaManager.currentTime > charismaFrame.EndTime)
                    {
                       
                        isGesture = false;
                        charismaManager.PlayCharisma = false;
                            
                    }



                }

                photoManager.Draw(gameTime, spriteBatch);



                
#endif

            }
            #endregion
            #region 결과 화면
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


                resultNumberManager.Draw(spriteBatch);

                //진짜 화면에 나타나는것은 update,플레이에 있음.
                //이것은 지울 예정임/
                spriteBatch.DrawString(pericles36Font, scoreManager.Perfect.ToString(), new Vector2(300, 300), Color.White);
                spriteBatch.DrawString(pericles36Font, scoreManager.Good.ToString(), new Vector2(300, 350), Color.White);
                spriteBatch.DrawString(pericles36Font, scoreManager.Bad.ToString(), new Vector2(300, 400), Color.White);
                spriteBatch.DrawString(pericles36Font, scoreManager.Perfomance.ToString(), new Vector2(700, 300), Color.White);
                spriteBatch.DrawString(pericles36Font, scoreManager.Max.ToString(), new Vector2(700, 350), Color.White);
                spriteBatch.DrawString(pericles36Font, scoreManager.Combo.ToString(), new Vector2(700, 400), Color.White);
                spriteBatch.DrawString(pericles36Font, scoreManager.Gold.ToString(), new Vector2(700, 450), Color.White);
                spriteBatch.DrawString(pericles36Font, scoreManager.TotalScore.ToString(), new Vector2(700, 500), Color.White);
            }

            #endregion

            #region 사진 보여주기 


            if (gameState == GameStates.ShowPictures)
            {
                showPictureScene.Draw(spriteBatch, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height);
                
                

                if(playingPictures.Count > 0 )
                {
                    //3개보다 많으면 3으로 
                    playPicturesCount = (playingPictures.Count > 3 ? 3 : playingPictures.Count);
                    int i;

                    for (i = 0; i < playPicturesCount; i++)
                    {

                     //   FileStream fileStream = new FileStream(@"C:\\beethovenRecord\\userPicture\\" + playingPictures.Dequeue(), FileMode.Open);

                        showPictureTextures[i] = (Texture2D)playingPictures.Dequeue();


                    }

                }
                for (int i = 0; i < playPicturesCount; i++)
                    {

                        spriteBatch.Draw(showPictureTextures[i], new Rectangle(200, (i + 1) * 100, 100, 100), Color.White);
                  

                    }

            }

            #endregion

            #region Setting
            if (gameState == GameStates.SettingBoard)
            {
                settingBoard.Draw(spriteBatch, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height);

           

            }
            #endregion


            #region 스코어 보드

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


            #endregion
#if Kinect
            if (KinectVideoTexture != null)
            {
                spriteBatch.Draw(KinectVideoTexture, VideoDisplayRectangle, Color.White);
                //setupKinect.draw();

            }
            if (skeleton != null)
            {

                if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
                    {
                        drawpoint(skeleton.Joints[JointType.HandRight], skeleton.Joints[JointType.HandLeft]);

                    }
            }
#endif
            spriteBatch.End();
            
            base.Draw(gameTime);
        }


        
#if Kinect
        void drawpoint(Joint j1, Joint j2)
        {
       
            
            ////실질적인 스케일 변환
            j1r = j1.ScaleTo(SCR_W, SCR_H, userParam, userParam);
         //   Vector2 rec = new Vector2(0,100,100);
            //그리기

            if (!nearButton)
            {
                drawrec1.X = (int)j1r.Position.X;
                drawrec1.Y = (int)j1r.Position.Y;
            }
            else
            {
                  drawrec1.X = (int)center.X;
                  drawrec1.Y = (int)center.Y;

            }
               
         //   spriteBatch.Draw(rightHandTextures[itemManager.getRightHandIndex()], drawrec1, new Rectangle(0,0,100,100), Color.White,0f,Vector2.Zero, 1.0f , SpriteEffects.None , 1.0f);
            spriteBatch.Draw(
              rightHandTextures[itemManager.getRightHandIndex()],
                //위치: Center-> location 으로 바꿈 (마커와 노트 매칭 떄문에 )
              new Vector2(drawrec1.X,drawrec1.Y),

              null,
              Color.White,
              0f,
                //origin ->  new Vector2(frameWidth / 2, frameHeight / 2) ->  new Vector2(0,0) 으로 바꿈 (마커와 노트 매칭 떄문에 )
              new Vector2(0, 0),
              //오른쪽 마크 크기 
              1.5f,
              SpriteEffects.None,
              0.0f);   

            j2r = j2.ScaleTo(SCR_W, SCR_H, userParam, userParam);

            //drawrec2.X = (int)j2r.Position.X - drawrec2.Width / 2;
            //drawrec2.Y = (int)j2r.Position.Y - drawrec2.Height / 2;
            drawrec2.X = (int)j2r.Position.X ;
            drawrec2.Y = (int)j2r.Position.Y ;

            spriteBatch.Draw(leftHandTextures[itemManager.getLeftHandIndex()], drawrec2, Color.White);
        }
#endif


       
    }




}