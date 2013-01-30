﻿
#define Kinect

//시작시 검사 
#define StartDetact

//삭제상자 보이기 
//#define Debug
//키보드모드일떄
//#define Keyboard

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

using System.Runtime.InteropServices;

using System.IO;
using System.Threading;
using System.Diagnostics;

#if Kinect
using Microsoft.Kinect;
using Coding4Fun.Kinect.Wpf;
using Microsoft.Samples.Kinect.SwipeGestureRecognizer;
using Microsoft.Speech.Recognition;
using Microsoft.Speech.AudioFormat;

#endif
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

        //여러장 저장안되고 한장만 저장되게 하는것. 현재는 여러장 저장
        private bool isScorePic = false;
        
        //사진 저장하는 큐
        private Queue playingPictures;

        private int playPicturesCount = 0;
        
        Texture2D[] showPictureTextures = new Texture2D[5];
        
        //Joint rightJoint;
        //Joint leftJoint;
        Texture2D backJesture;

        //노래의 위치를 알기위해서 현재 노래가 틀어지고 있는지 확인
        private bool isPlayingSong = false;
        private uint songLength;
        //temp

       
        private Texture2D loadingForAngle;
        //0 각도 재야함 // 1 각도 잴필요없음 //2 끝남 
        private int loadingTime = 1;

        private String resultMusic = System.Environment.CurrentDirectory + "\\result.mp3";
        private String title_Music = System.Environment.CurrentDirectory + "\\Title_Music.mp3";


        private Texture2D go_back;
        private Texture2D go_front;



        int _total_frames = 0;
        float _elapsed_time = 0.0f;
        public static int _fps = 0;

        //public static String mouseHoverSound = System.Environment.CurrentDirectory + "\\mouseHover.mp3";
        //public static String mouseClickSound = System.Environment.CurrentDirectory + "\\mouseOk.wav";
        //public static String wearSound = System.Environment.CurrentDirectory + "\\wear.mp3";
        //public static String sellSound = System.Environment.CurrentDirectory + "\\sell.mp3";
#if Debug
        public static Texture2D blackRect;
#endif
        public static bool finalClick;
        public static bool pastClick = false;
        //버튼 센터 

        public static Vector2 center = new Vector2(0, 0);

      
        //주변 버튼 여부
        public static bool nearButton = false;
 

        //사람 키에 따른 미세조정 파라미터
        public float userParam = .25f;

        //라이프이펙트 적용여부 
        double lifePlusEffect = 1;
        public static bool PicFlag = false;
#if Kinect
               //오른손 좌표

        public static Joint j1r;
        public static Joint j2r;


        //키넥트
        public KinectSensor nui = null;

        //화면에 띄우기
        Texture2D KinectVideoTexture;
        Rectangle VideoDisplayRectangle;



        public static Rectangle drawrec1;
        public static Rectangle drawrec2;
       

        
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

     // private String kinectMessage = "__UNKNOWN";
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
        public static bool isGesture = true;
        //머리찾기
        float fy;
      
        //키재기
        float fheadY;
        float fhipY;
        float fcenterZ;//사람의 거리
        double factheight;

        //폰트
        //SpriteFont messageFont;
        string message = "start";

        //손
        DepthImagePoint handPoint;
        short[] ImageBits;
        ColorImagePoint[] depthLocation;


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
       
        public static SpriteFont georgia;

        public enum GameStates { Loading, Menu, Playing, SongMenu, ShopDoor,
                          RightItemShop, LeftItemShop, EffectItemShop, NoteItemShop, BackgroundItemShop,
                          ResultManager, RecordBoard, ShowPictures, SettingBoard, TutorialScene };


        //게임 씬, 처음시작은 메뉴
        public static GameStates gameState = GameStates.Loading;


        private LoadingScene loadingScene;
 

        //타이틀 화면 
        private MenuScene menuScene;



        //상점 화면
        private ShopDoor shopDoor;
        
        //곡 선택 화면
        private SongMenu songMenu;

        //선택된 곡
        private int resultSongMenu;

        private TutorialScene tutorialScene;

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

        DragNoteManager dragNoteManager;

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
        //ResultNumberManager resultNumberManager;
        
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

        private BackJestureManager backJestureManager;

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
        public static MouseState mouseStateCurrent, mouseStatePrevious;

        //노래들이 있는 경로
        public static String songsDir = System.Environment.CurrentDirectory+"\\beethovenSong\\";
        
        /////Texture start

        //현재는 노트와 마커 텍스쳐 이펙트까지
        //NOTES, MARKERS TEXTERUE
        public static Texture2D spriteSheet;

        //드래그 노트 텍스쳐
        //DragNote Textere
        public static Texture2D heart;

        //UI 배경
        private Texture2D energyDarkBack;
        
        //UI 컨텐츠
        private Texture2D energy;

        private Texture2D processBar;
        private Texture2D processMark;


        //배경
        //whole background
        private Texture2D playBackgroud;
       
       //드래그 라인 모양//&&&
        public static Texture2D drawLineNote1;

     
        //perfect,good,bad,miss 판정 배너
        private Texture2D perfectBanner;

        private Texture2D goodBanner;

        private Texture2D badBanner;

        private Texture2D missBanner;


        //gold
        private Texture2D gold;

        private Texture2D getGold;


        public static Texture2D menuGold;
       

        public static Texture2D goldPlusEffect10;
        public static Texture2D goldPlusEffect15;

        public static Texture2D lifePlusEffect10;
        public static Texture2D lifePlusEffect15;
        //level

        public static Texture2D levelTexture;

        private Texture2D uiEnergyBackground;

        //오른손 텍스쳐들

        private Texture2D[] rightHandTextures;

        private Texture2D[] leftHandTextures;

        //사람 없음 나타내는 메시지 박스

        private Texture2D noPerson;


        public static Texture2D previousButton;
        public static Texture2D hoverPreviousButton;

          public static Texture2D nextButton;
        public static Texture2D hoverNextButton;

        
        

        /////Texture end 

        private CharismaManager charismaManager;


     //   private PhotoManager photoManager;
        //드래그 라인 안의 마커점

        public static Texture2D drawLineMarker;



        public static Texture2D one;
        public static Texture2D two;
        public static Texture2D three;

        public static Texture2D darkBackgroundImage;
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
   //     public static Texture2D idot;

       
        private bool isNoPerson;


        private double uiProcessTime;


        private double uiEndTime;
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
            this.IsFixedTimeStep = true;

#if Kinect
            
            //KINECT
            VideoDisplayRectangle = new Rectangle(0, 0, SCR_W, SCR_H);

            //drawrec1 = new Rectangle(0, 0, GraphicsDevice.Viewport.Width / 20, GraphicsDevice.Viewport.Height / 20);
            //drawrec2 = new Rectangle(0, 0, GraphicsDevice.Viewport.Width / 20, GraphicsDevice.Viewport.Height / 20);
            drawrec1 = new Rectangle(0, 0, 5, 5);
            drawrec2 = new Rectangle(0, 0, 5, 5);


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


#if Debug
        blackRect= Content.Load<Texture2D>(@"Textures\darkBackground");
#endif
            goldPlusEffect10 = Content.Load<Texture2D>(@"ui\goldPlus10");
            goldPlusEffect15 = Content.Load<Texture2D>(@"ui\goldPlus15");

            lifePlusEffect10 = Content.Load<Texture2D>(@"ui\lifePlus10");
            lifePlusEffect15 = Content.Load<Texture2D>(@"ui\lifePlus15");

            darkBackgroundImage = Content.Load<Texture2D>(@"Textures\darkBackground");
            go_back = Content.Load<Texture2D>(@"ui\go_back");
            go_front = Content.Load<Texture2D>(@"ui\go_front");

            //이거 없으면 STARTNOTE TEXTURE가 NULL이라 위치값을 잘못 줄때가 있다.
            spriteSheet = Content.Load<Texture2D>(@"ui\SpriteSheet8");
     
            backJesture = Content.Load<Texture2D>(@"game1\backJesture");
            backJestureManager = new BackJestureManager(backJesture, new Rectangle(0, 0, 204, 204), 10, new Vector2(800, 500));

       
           //타이틀화면
            menuScene = new MenuScene();
            menuScene.LoadContent(Content);

            uiEnergyBackground = Content.Load<Texture2D>(@"ui\uiEnergyBackground");
            energyDarkBack = Content.Load<Texture2D>(@"ui\energyDarkBack");

            processBar = Content.Load<Texture2D>(@"ui\processBar");
            processMark = Content.Load<Texture2D>(@"ui\processMark");

            //아이템관리
            //startnotemanager 생성보다 앞에 있어야 한다.
            itemManager = new ItemManager();
            itemManager.LoadContent(Content);
            itemManager.Init();

            loadingScene = new LoadingScene();
            //loadingScene.LoadContent(Content);


            //게임중 점수관리
            scoreManager = new ScoreManager();

            //상점 대문2
            shopDoor = new ShopDoor(scoreManager);
            shopDoor.LoadContent(Content);
            
            //점수 기록 (TO FILE)
            reportManager = new ReportManager(scoreManager);


            /////아이템 상점 -START
            rightItemShop = new RightItemShop(itemManager,  scoreManager, reportManager);
            rightItemShop.LoadContent(Content);

            leftItemShop = new LeftItemShop(itemManager, scoreManager, reportManager);
            leftItemShop.LoadContent(Content);

            effectItemShop = new EffectItemShop(itemManager, scoreManager, reportManager);
            effectItemShop.LoadContent(Content);
            
            noteItemShop = new NoteItemShop(itemManager, scoreManager, reportManager);
            noteItemShop.LoadContent(Content);

            backgroundItemShop = new BackgroundItemShop(itemManager, scoreManager, reportManager);
            backgroundItemShop.LoadContent(Content);
            /////아이템 상점 -START

            //연주자
            memberManager = new MemberManager(scoreManager);
            memberManager.LoadContent(Content);
            memberManager.init();

            /////텍스쳐 로드 -START
            //배경
            //playBackgroud1 = Content.Load<Texture2D>(@"background\ConcertHall_3");
            //playBackgroud2 = Content.Load<Texture2D>(@"background\crosswalk3");
            
            
            //노트,마커
           // spriteSheet = Content.Load<Texture2D>(@"Textures\SpriteSheet8");
            
            //드래그 노트
           // heart = Content.Load<Texture2D>(@"Textures\heart");
           
            //진행상황
            //uiBackground = Content.Load<Texture2D>(@"ui\background");
            energy = Content.Load<Texture2D>(@"ui\energy");

            //골드 업어
            getGold = Content.Load<Texture2D>(@"gold\getGold");
            //폰트
            georgia = Content.Load<SpriteFont>(@"Fonts\Georgia");

            
            //perfect,good,bad,miss 판정 배너 
            perfectBanner = Content.Load<Texture2D>(@"judgement\perfect");

            goodBanner = Content.Load<Texture2D>(@"judgement\good");

            badBanner = Content.Load<Texture2D>(@"judgement\bad");

            missBanner = Content.Load<Texture2D>(@"judgement\miss");


            noPerson = Content.Load<Texture2D>(@"game1\noperson");



            rightHandTextures = itemManager.GetRightHandTexture();
            leftHandTextures = itemManager.GetLeftHandTexture();
            
            
            //드래그 노트 셀떄

            one = Content.Load<Texture2D>(@"ui\one");
            two = Content.Load<Texture2D>(@"ui\two");
            three = Content.Load<Texture2D>(@"ui\three");


            previousButton = Content.Load<Texture2D>(@"game1\previousButton");
            hoverPreviousButton = Content.Load<Texture2D>(@"game1\hoverPrevious");
            

            nextButton = Content.Load<Texture2D>(@"game1\nextButton");
            hoverNextButton = Content.Load<Texture2D>(@"game1\hoverNextButton");

            loadingForAngle = Content.Load<Texture2D>(@"ui\LoadingKinect");
            //   charisma1 = Content.Load<Texture2D>(@"shopdoor\nogold");
            /////텍스쳐 로드 -END

         

            /////////////////드래그 라인 관련
            

            //드래그 라인
         //   drawLineNote1 = Content.Load<Texture2D>(@"DrawLine\drawLineNote1");

            //드래그 라인 마커점
         //   drawLineMarker = Content.Load<Texture2D>(@"DrawLine\drawLineMark");
            

            gold = Content.Load<Texture2D>(@"gold\gold");
            menuGold = Content.Load<Texture2D>(@"game1\menuGold");
           
            levelTexture = Content.Load<Texture2D>(@"game1\level");


            //로드하고, 기본적으로 넣어줌
            itemManager.LoadFileItem();

                   
            //현재 장착한 이펙트의 인덱스를 전체 베이스에 찾음
            int effectIndex = itemManager.getEffectIndex();

     
            //콤보 숫자
            comboNumberManager = new ComboNumberManager();
            comboNumberManager.LoadContent(Content);

            //***결과화면 숫자
            //resultNumberManager = new ResultNumberManager();
           // resultNumberManager.LoadContent(Content);

            tutorialScene = new TutorialScene();
            tutorialScene.Load(Content);
              

            //드래그 라인 렌더링
            dragLineRenderer = new LineRenderer();


            //드래그 라인 안의 마커점의 렌더링 
            dragLineMarkerRenderer = new LineRenderer();

            //드래그라인 
            curveManager = new CurveManager(dragLineRenderer, dragLineMarkerRenderer,dragNoteManager,itemManager);

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

            //카리스마 매니저
            charismaManager = new CharismaManager();
            charismaManager.LoadContent(Content);

          

            //드래그노트 초기화
            //이것은 노트 안에서 움직이는 마커점
            dragNoteManager = new DragNoteManager(
                 drawLineMarker,
                 itemManager.GetDragNoteBackground()[itemManager.getNoteIndex()],
                 new Rectangle(0, 0, 100, 100),
                 1,
                 15,
                 0,
                 missBannerManager,
                 scoreManager);



            //충돌관리 생성
            collisionManager = new CollisionManager(perfectManager, goodManager, badManager, goldGetManager, scoreManager, memberManager,/*effect크기*/itemManager,perfectBannerManager,goodBannerManager,badBannerManager,missBannerManager,new Vector2(this.Window.ClientBounds.Width,this.Window.ClientBounds.Height),comboNumberManager,charismaManager,dragNoteManager);
            
            //노트정보 관리 생성
            noteFileManager = new NoteFileManager();
            //드래그 라인 렌더링
            dragLineRenderer = new LineRenderer();

            dragLineMarkerRenderer = new LineRenderer();
            //드래그라인 
            curveManager = new CurveManager(dragLineRenderer, dragLineMarkerRenderer, dragNoteManager,itemManager);

          

        //    photoManager = new PhotoManager();
        //    photoManager.LoadContent(Content);


            //노트파일 읽기 관리 생성
            file = new File(startNoteManager, noteFileManager, collisionManager, scoreManager, itemManager, curveManager, guideLineManager, charismaManager);

            SoundFmod.initialize(file);
            //곡선택화면 곡 불러오는 폴더 
          
            //노래폴더 만들기 위해서 테스트
           // String songFile = System.Environment.CurrentDirectory+"\\beethovenSong\\song.txt";
            String dir = System.Environment.CurrentDirectory + "\\beethovenSong";

            DirectoryInfo di = new DirectoryInfo(dir);
            if (di.Exists == false)
            {
                di.Create();
            }
            
           
            //기록 만들기 위해서 테스트
          //  String songRecordFile = System.Environment.CurrentDirectory + "\\beethovenRecord\\song.txt";
            String songRecorddir = System.Environment.CurrentDirectory + "\\beethovenRecord";

            DirectoryInfo diRecord = new DirectoryInfo(songRecorddir);
            if (diRecord.Exists == false)
            {
                diRecord.Create();
            }
            


            
            //사진 폴더  위해서 테스트
            //String picFile = System.Environment.CurrentDirectory + "\\beethovenRecord\\userPicture\\song.txt";
            String picddir = System.Environment.CurrentDirectory + "\\beethovenRecord\\userPicture";
            String defaultPicture = System.Environment.CurrentDirectory + "\\No_Image.png";


            DirectoryInfo diPic = new DirectoryInfo(picddir);
            if (diPic.Exists == false)
            {
                diPic.Create();
                FileInfo fileInfo = new FileInfo(defaultPicture);
                fileInfo.CopyTo(System.Environment.CurrentDirectory + "\\beethovenRecord\\UserPicture\\No_Image.png");
            
            }
            
            
                      

            //곡을 불러오기
            file.FileLoading(dir, "*.mnf");
            
            ////드래그노트 초기화
            ////이것은 노트 안에서 움직이는 마커점
            //dragNoteManager = new DragNoteManager(
            //     drawLineMarker,
            //     new Rectangle(0, 0, 100, 100),
            //     1,
            //     15,
            //     0,
            //     missBannerManager,
            //     scoreManager);

            //골드 초기화
            //크기 0.3
            GoldManager.initialize(
                gold,
                new Rectangle(0, 0, 200, 200),
                1,
                70,
                0,
                0.3f);
            
            
            //***
           

            
            resultManager = new ResultManager(scoreManager);
            resultManager.LoadContent(Content);



            //점수기록판 화면
            recordBoard = new RecordBoard();
            recordBoard.LoadContent(Content);

#if Kinect
            settingBoard = new SettingBoard(this);
            settingBoard.LoadContent(Content);
#endif

            showPictureScene = new ShowPictureScene();
            showPictureScene.LoadContent(Content);

            

            songMenu = new SongMenu(noteFileManager, reportManager);
            songMenu.Load(Content, graphics.GraphicsDevice);




            //LOAD REPORT SCORE FILE
            //점수기록판을 로드해서 게임에 올린다. 

            reportManager.LoadReport();

            //골드를 로드해서 게임에 올린다. 
            reportManager.LoadGoldFromFile();

            currentSongName = "";
           
           // itemManager.LoadFileItem();
#if Kinect
          
        //    messageFont = Content.Load<SpriteFont>("MessageFont");
            setupKinect();

            /*제스쳐 인식인데 
             전체 구간에서 계속 실행 된다. 
             *이것이 다른것에 영향을 줄 수도 있다. 
             */
            activeRecognizer = CreateRecognizer();

            //!!! 페이스 인식, 디버깅 시에 잠시 

#if StartDetact
            settingBoard.LoadCheckFile();
            if(settingBoard.CheckFaceDetact)
            {
                //0 각도 재야함 // 1 각도 잴필요없음 //2 끝남 
                loadingTime = 0;

                ts4 = new ThreadStart(FaceDetect);
                th4 = new Thread(ts4);
                th4.Start();
            }

#endif


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

        #region 각도 재기 
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


            if (factheight >= 180)
            {
                userParam = 0.4f;
            }
            else if (factheight < 180 && factheight >= 170)
            {
                userParam = 0.35f;
            }
            else if (factheight < 170 && factheight >= 160)
            {
                userParam = 0.30f;
            }
            else if (factheight < 160 && factheight >= 150)
            {
                userParam = 0.25f;
            }
            else
            {
                userParam = 0.3f;
            }
            //0 각도 재야함 // 1 각도 잴필요없음 //2 끝남 
            loadingTime = 1;



        }
#endif
        #endregion
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

                //Smoothing = 0.7f,
                //Correction = 0.3f,
                //Prediction = 1.0f,
                //JitterRadius = 1.0f,
                //MaxDeviationRadius = 1.0f

                Smoothing = 0.05f,
                Correction = 0.5f,
                Prediction = 0.5f,
                JitterRadius = 0.8f,
                MaxDeviationRadius = 0.2f
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
                _dtw1 = new DtwGestureRecognizer(12, 0.6, 1.2, 2, 2);//정지포스쳐
                _dtw2 = new DtwGestureRecognizer(12, 1.2, 2, 2, 2);//일반포스쳐
                _dtw3 = new DtwGestureRecognizer(12, 1.2, 2, 2, 10);//제스쳐

                _video = new ArrayList();
                gestureType = 1;
                postureCount = 0;
                gestureFlag = true;
                string fileName = "0.txt";
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
                    //message = reinfo.Id;
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
          
            //choices.Add("next");
            //choices.Add("previous");
            //choices.Add("start");
            //choices.Add("tutorial");
            //choices.Add("setting");
            //choices.Add("shop");
            choices.Add("이전");
            choices.Add("시작");
            choices.Add("중지");
            choices.Add("다음");
            choices.Add("설정");
            choices.Add("상점");
            choices.Add("도움말");
            choices.Add("노트");
            choices.Add("효과");
            choices.Add("지휘봉");
            choices.Add("왼손");
            choices.Add("배경");
          
          

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
            if (e.Result.Confidence < 0.3) return;//신뢰도 0.5미만일땐 리턴
            if (skeleton != null)
            {

                if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
                    {
                        if (source.SoundSourceAngle < skeletonAngle + 10 && source.SoundSourceAngle > skeletonAngle - 10)
                        {
                            //message = e.Result.Text + " " + e.Result.Confidence.ToString();
                            switch (e.Result.Text)
                            {



                                //case "next":
                                case "다음":
                                case "담":
                                    if (gameState == GameStates.ResultManager)
                                    {

                                        gameState = GameStates.ShowPictures;
                                        nearButton = false;

                                        //현재 위치 말고, 기본은 0으로 해놓고


                                        Vector2 markerSize = MarkManager.GetMarkerSize();


                                        Vector2[] zeroIndexMarkers = MarkManager.GetPattern(0);
                                        //   removeAreaRec = MarkManager.GetRemoveArea(0);
                                        //두번째꺼 재실행시 이상한거 생기는것 방지
                                        startNoteManager.DeleateAllNote();
                                        //StartNoteManager.rightNoteManager.DeleteAllNote();
                                        //StartNoteManager.leftNoteManager.DeleteAllNote();
                                        //StartNoteManager.longNoteManager.DeleteAllNote();

                                        //@@@이걸 지워서 무제가 될수도 있다. 패턴이 바뀌었는데도 다음번에 안바뀌어서 이걸 넣음
                                        //startNoteManager = new StartNoteManager(
                                        //    spriteSheet,
                                        //    new Rectangle(0, 200, 52, 55),
                                        //    1);

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
                                        file.SetTime(TimeSpan.Zero);

                                        scoreManager.init();


                                        gameState = GameStates.ShowPictures;
                                    }else if (gameState == GameStates.ShowPictures)
                                    {
                                        gameState = GameStates.RecordBoard;
                                    }else if (gameState == GameStates.RecordBoard)
                                    {
                                        gameState = GameStates.SongMenu;
                                        //bool isPlay = false;
                                        //if (isPlay)
                                        //{
                                        //    SoundFmod.StopSound();
                                        //}

                                        //SoundFmod.PlaySound(title_Music);
                                        SongMenu.opening = true;
                                    }

                                    if (gameState == GameStates.SettingBoard)
                                    {
                                        gameState = GameStates.Menu;
                                      //  settingBoard.SaveCheckFile();
                                    }


                                    break;

                                //case "previous":
                                case "이전":

                                    if ( gameState == GameStates.TutorialScene|| gameState == GameStates.ShopDoor)
                                    {
                                        gameState = GameStates.Menu;
                                        
                                    }
                                    if (gameState == GameStates.Playing)
                                    {
                                        file.SetEndFile(true);
                                        resultManager.FailGame = true;
                                    }
                                    if (gameState == GameStates.SettingBoard)
                                    {
                                        gameState = GameStates.Menu;
                                        
                                    }

                                    if (gameState == GameStates.BackgroundItemShop || gameState == GameStates.EffectItemShop || gameState == GameStates.LeftItemShop || gameState == GameStates.RightItemShop || gameState == GameStates.NoteItemShop)
                                    {
                                        gameState = GameStates.ShopDoor;
                                    }

                                    if (gameState == GameStates.RecordBoard)
                                    {
                                        gameState = GameStates.ShowPictures;
                                    }

                                    if (gameState == GameStates.SongMenu)
                                    {
                                        gameState = GameStates.Menu;
                                        bool isPlay = false;
                                        SoundFmod.sndChannel.isPlaying(ref isPlay);
                                        if (isPlay)
                                        {
                                            SoundFmod.StopSound();
                                        }
                                        
                                        SoundFmod.PlaySound(title_Music);
                                    }
                                    break;


                                //case "start":
                                case "시작":
                                    if (gameState == GameStates.Menu)
                                    {
                                        gameState = GameStates.SongMenu;
                                    }else if (gameState == GameStates.SongMenu)
                                    {
                                        soundRecogStartIndex = songMenu.Scene_number;
                                    }


                                    break;


                                case "상점":
                                    
                                    if (gameState == GameStates.Menu)
                                    {
                                        gameState = GameStates.ShopDoor;
                                    }
                                    break;

                                case "설정":
                                case "설쩡":

                                    if (gameState == GameStates.Menu)
                                    {
                                        gameState = GameStates.SettingBoard;
                                    }
                                    break;

                                case "도움말":
                                case "돔말":
                                    if (gameState == GameStates.Menu)
                                    {
                                            gameState = GameStates.TutorialScene;
                                    }
                                    break;
                                case "노트":

                                    if (gameState == GameStates.ShopDoor)
                                    {
                                        gameState = GameStates.NoteItemShop;
                                    }
                                    break;
                                case "효과":
                                case "효꽈":
                                    if (gameState == GameStates.ShopDoor)
                                    {
                                        gameState = GameStates.EffectItemShop;
                                    }
                                    break;
                                case "지휘봉":

                                    if (gameState == GameStates.ShopDoor)
                                    {
                                        gameState = GameStates.RightItemShop;
                                    }
                                    break;
                                case "배경":

                                    if (gameState == GameStates.ShopDoor)
                                    {
                                        gameState = GameStates.BackgroundItemShop;
                                    }
                                    break;
                                case "왼손":

                                    if (gameState == GameStates.ShopDoor)
                                    {
                                        gameState = GameStates.LeftItemShop;
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
                PicFlag = false;
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


            //if (handPoint.X < tempWidth / 2)
            //{
            //    handPoint.X = 0;
            //    handPoint.Y = 0;
            //}
            //if (handPoint.X > ImageParam.Width - tempWidth / 2)
            //{
            //    handPoint.X = 0;
            //    handPoint.Y = 0;
            //}
            //if (handPoint.Y < tempHeight / 2)
            //{
            //    handPoint.X = 0;
            //    handPoint.Y = 0;
            //}
            //if (handPoint.Y > ImageParam.Height - tempHeight / 2)
            //{
            //    handPoint.X = 0;
            //    handPoint.Y = 0;
            //}
            //if (handPoint.X < 0)
            //{
            //    handPoint.X = 0;
            //}
            //if (handPoint.Y < 0)
            //{
            //    handPoint.Y = 0;
            //}


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
                finalClick = false;
                handList.Add(handPoint);
                clickList.Add(clickJudge);
                listCount++;
            }
            else
            {
                finalClick = true;
                handList.RemoveAt(0);
                clickList.RemoveAt(0);
                handList.Add(handPoint);
                clickList.Add(clickJudge);
            }

            //finalClick = true;
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

            ////클릭여부
            //if (finalClick == true)
            //{
            //    //message = "click";
            //}
            //else
            //{
            //    //message = "No click";
            //}

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
                    message = factheight.ToString();
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
                             //s.Joints[JointType.HandRight].TrackingState == JointTrackingState.Tracked &&
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
                             where s.TrackingState == SkeletonTrackingState.Tracked //&&
                             //s.Joints[JointType.HandRight].TrackingState == JointTrackingState.Tracked
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
                if (gameState == GameStates.SongMenu || gameState == GameStates.TutorialScene)
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
                //message = s.ToString();

                //////////////////////////////////////////
                if (!s.Contains("__UNKNOWN"))
                {
                    postureCount++;
                   // message = "stop yes";
                   // backJestureManager.ShowJestureMark = true;
                    if (postureCount > 2 && postureCount <= 7)
                    {
                        backJestureManager.ShowJestureMark = true;
                    }
                    if (postureCount > 7)
                    {
                        //여기에 정지했을 때 동작 넣기


                        if (gameState == GameStates.Playing)
                        {

                            file.SetEndFile(true);
                            resultManager.FailGame = true;
                        }
                        if (gameState == GameStates.TutorialScene || gameState == GameStates.ShopDoor )
                        {
                            gameState = GameStates.Menu;

                        }

                        if(gameState == GameStates.SettingBoard)
                        {
                            gameState = GameStates.Menu;
                          //  settingBoard.SaveCheckFile();
                        }

                        if (gameState == GameStates.BackgroundItemShop || gameState == GameStates.EffectItemShop || gameState == GameStates.LeftItemShop || gameState == GameStates.RightItemShop || gameState == GameStates.NoteItemShop)
                        {
                            gameState = GameStates.ShopDoor;
                        }

                        if (gameState == GameStates.RecordBoard)
                        {
                            gameState = GameStates.ShowPictures;
                        }

                        if (gameState == GameStates.SongMenu)
                        {
                            gameState = GameStates.Menu;
                            bool isPlay = false;
                            SoundFmod.sndChannel.isPlaying(ref isPlay);
                            if (isPlay)
                            {
                                SoundFmod.StopSound();
                            }
                        }

                        postureCount = 0;
                  

                        backJestureManager.ShowJestureMark = false;
                    }
                }

                if (s.Contains("__UNKNOWN"))
                {
                    postureCount = 0;
                 //   message = "stop no";
                    backJestureManager.ShowJestureMark = false;
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
                message = s.ToString();
                if (gestureFlag)
                {
                    if (!s.Contains("__UNKNOWN"))
                    {
                        postureCount++;
                        //message = "posture yes";
                        perfectBannerManager.AddBanners(collisionManager.perfectLocation,collisionManager.perfectBannerScale);
                       // scoreManager.Perfomance = scoreManager.Perfomance + 1;
                        scoreManager.PosturePerfect+=10;
                        scoreManager.Combo++;
                        scoreManager.ComboChanged = true;

                        collisionManager.AddComboNumber((int)scoreManager.Combo, 0);




                        scoreManager.Gage++;


                        //if (postureCount > 5)
                        //{
                            
                        //    gestureFlag = false;
                        //}
                    }

                    if (s.Contains("__UNKNOWN"))
                    {
                        //message = "posture no";
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
          
            if (_video.Count > MinimumFrames)
            {

                string s = _dtw3.Recognize(_video);
                double score = _dtw3.MinDist;//점수

                if (!s.Contains("__UNKNOWN"))
                {
                    _video = new ArrayList();
                }


                if (gestureFlag)
                {
                    if (!s.Contains("__UNKNOWN"))
                    {
                       scoreManager.Perfomance = scoreManager.Perfomance + 1;
                        if (score < 1.2 && score>=0.9)//굳
                        {
                           
                            goodBannerManager.AddBanners(collisionManager.goodLocation, collisionManager.goodBannerScale);
                            scoreManager.JestureGood ++ ;
                            scoreManager.Combo++;
                            scoreManager.ComboChanged = true;
                            collisionManager.AddComboNumber((int)scoreManager.Combo, 1);


                        }
                        else if (score < 0.9)//퍼펙트
                        {
                            perfectBannerManager.AddBanners(collisionManager.perfectLocation, collisionManager.perfectBannerScale);
                            scoreManager.JesturePerfect ++;
                            scoreManager.Combo++;
                            scoreManager.ComboChanged = true;
                            collisionManager.AddComboNumber((int)scoreManager.Combo, 0);


                        }

                        //message = "yes" + score.ToString();
                        gestureFlag = false;
                        postureCount++;
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

           // if (!isScorePic)
           // {
                //마지막 스코어 보드에 쓰이는 사진
                ScorePic = gesture;

                isScorePic = true;
                Stream str = System.IO.File.OpenWrite(dir);
                CapturePic.SaveAsJpeg(str, SCR_W, SCR_H);
                str.Dispose();
            //}

            playingPictures.Enqueue(CapturePic);

           
            th1.Abort();
        }

        private Recognizer CreateRecognizer()
        {      

            var recognizer = new Recognizer();

            
            recognizer.SwipeRightDetected += (s, e) =>
            {//오른손이 왼쪽으로 동작할때
                songMenu.isKinectRight = true;
                tutorialScene.isKinectRight = true;
               
            };


            recognizer.SwipeLeftDetected += (s, e) =>
            {//왼손이 오른쪽으로 동작할때

                songMenu.isKinectLeft = true;
                tutorialScene.isKinectLeft = true;
              
            };

            return recognizer;
        }


        //입력 타입
        //출력 센터

         public KinectSensor Nui
        {
            get { return nui; }
            set { nui = value; }

        }
    


#endif
        public float UserParam
        {
            get { return userParam; }
            set { userParam = value; }

        }

       
        public static void GetCenterOfButton(Rectangle rec)
        {

            center = new Vector2(rec.X + rec.Width / 2, rec.Y + rec.Height / 2);

        }

        //마우스 충돌 처리
        private void HandleMouseInput(MouseState mouseState)
        {
            Vector2 mouseCurrent = new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y);
            collisionManager.checkDragNote(new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y));

            collisionManager.CheckMouseCollisions(0, new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y));

            collisionManager.CheckMouseCollisions(1, new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y));

            collisionManager.CheckMouseCollisions(2, new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y));

            collisionManager.CheckMouseCollisions(3, new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y));

            collisionManager.CheckMouseCollisions(4, new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y));

            collisionManager.CheckMouseCollisions(5, new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y));
#if Keyboard
            collisionManager.checkMarkers2(0, new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y));
            collisionManager.checkMarkers2(1, new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y));
            collisionManager.checkMarkers2(2, new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y));
            collisionManager.checkMarkers2(3, new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y));
            collisionManager.checkMarkers2(4, new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y));
            collisionManager.checkMarkers2(5, new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y));
#endif

        }

#if Kinect
        //키넥트 충돌 처리
        private void HandleInput()
        
            
        {
            //오른손 /
            Vector2 drawrecR = new Vector2(drawrec1.X, drawrec1.Y);
            Vector2 drawrecL = new Vector2(drawrec2.X, drawrec2.Y);
            collisionManager.checkDragNote(drawrecR);

            collisionManager.CheckRightHandCollisions(0, new Vector2(drawrec1.X, drawrec1.Y));

            collisionManager.CheckRightHandCollisions(1, new Vector2(drawrec1.X, drawrec1.Y));

            collisionManager.CheckRightHandCollisions(2, new Vector2(drawrec1.X, drawrec1.Y));

            collisionManager.CheckRightHandCollisions(3, new Vector2(drawrec1.X, drawrec1.Y));

            collisionManager.CheckRightHandCollisions(4, new Vector2(drawrec1.X, drawrec1.Y));

            collisionManager.CheckRightHandCollisions(5, new Vector2(drawrec1.X, drawrec1.Y));
            collisionManager.checkMarkers(0, new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y), new Vector2(drawrec1.X, drawrec1.Y), new Vector2(drawrec2.X, drawrec2.Y));
            collisionManager.checkMarkers(1, new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y), new Vector2(drawrec1.X, drawrec1.Y), new Vector2(drawrec2.X, drawrec2.Y));
            collisionManager.checkMarkers(2, new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y), new Vector2(drawrec1.X, drawrec1.Y), new Vector2(drawrec2.X, drawrec2.Y));
            collisionManager.checkMarkers(3, new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y), new Vector2(drawrec1.X, drawrec1.Y), new Vector2(drawrec2.X, drawrec2.Y));
            collisionManager.checkMarkers(4, new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y), new Vector2(drawrec1.X, drawrec1.Y), new Vector2(drawrec2.X, drawrec2.Y));
            collisionManager.checkMarkers(5, new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y), new Vector2(drawrec1.X, drawrec1.Y), new Vector2(drawrec2.X, drawrec2.Y));

            //왼손
            //  collisionManager.checkDragNote(new Vector2(drawrec2.X, drawrec2.Y));

            collisionManager.CheckLeftHandCollisions(0, new Vector2(drawrec2.X, drawrec2.Y));

            collisionManager.CheckLeftHandCollisions(1, new Vector2(drawrec2.X, drawrec2.Y));

            collisionManager.CheckLeftHandCollisions(2, new Vector2(drawrec2.X, drawrec2.Y));

            collisionManager.CheckLeftHandCollisions(3, new Vector2(drawrec2.X, drawrec2.Y));

            collisionManager.CheckLeftHandCollisions(4, new Vector2(drawrec2.X, drawrec2.Y));

            collisionManager.CheckLeftHandCollisions(5, new Vector2(drawrec2.X, drawrec2.Y));
       
        }
#endif


   
      

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
            //if (keyState.IsKeyDown(Keys.B))
            //{

            //    gameState = GameStates.Menu;

            //}

        }

        //상점 대문에서 타이틀 화면으로 가는 키보드 처리
        private void HandleKeyboardInputinItemShop(KeyboardState keyState)
        {
            //if (keyState.IsKeyDown(Keys.B))
            //{
            //    itemManager.SaveFileItem();
            //    gameState = GameStates.ShopDoor;

            //}

        }


        private void HandleKeyboardInput(KeyboardState keyState)
        {
          
            if (keyState.IsKeyDown(Keys.Escape))
            {

                file.SetEndFile(true);
                resultManager.FailGame = true;
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
             // Update
            _elapsed_time += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
 
            // 1 Second has passed
            if (_elapsed_time >= 1000.0f)
            {
                _fps = _total_frames;
                _total_frames = 0;
                _elapsed_time = 0;
            }
            //Trace.WriteLine(_fps);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
      
            mouseStateCurrent = Mouse.GetState();

#if Kinect
            Rectangle rightHandPosition = new Rectangle((int)j1r.Position.X, (int)j1r.Position.Y, 5, 5);

#else
            Rectangle rightHandPosition = new Rectangle(0, 0, 1, 1);

#endif


            
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
                    //HandleKeyboardInputGoToMenu(Keyboard.GetState());
                    shopDoor.Update(gameTime, rightHandPosition);
                   // pastClick = finalClick;

                break;

                #endregion

               #region 아이템상점들


               #region 오른쪽상점
               case GameStates.RightItemShop:
                   //상점대문으로 돌아가는 키보드처리

                 
                   //HandleKeyboardInputinItemShop(Keyboard.GetState());
                   rightItemShop.Update(gameTime, rightHandPosition);
                   
             
                   break;

               #endregion

               #region 왼쪽상점

               case GameStates.LeftItemShop:
                   //상점대문으로 돌아가는 키보드처리
                    //HandleKeyboardInputinItemShop(Keyboard.GetState());
                    leftItemShop.Update(gameTime, rightHandPosition);


                   break;

               #endregion 

               #region 노트상점
               case GameStates.NoteItemShop:
                    //HandleKeyboardInputinItemShop(Keyboard.GetState());
                    noteItemShop.Update(gameTime, rightHandPosition);
             
                   break;
               #endregion
               #region 이펙트상점


               case GameStates.EffectItemShop:


                   //HandleKeyboardInputinItemShop(Keyboard.GetState());
                   effectItemShop.Update(gameTime, rightHandPosition);
                  
                
                   break;
               #endregion



               #region 배경상점
               case GameStates.BackgroundItemShop:
                   //상점대문으로 돌아가는 키보드처리
                   //HandleKeyboardInputinItemShop(Keyboard.GetState());
                   backgroundItemShop.Update(gameTime, rightHandPosition);

                break;

               #endregion
               #endregion

               #region 플레이화면
               case GameStates.Playing:
                   //if (scoreManager.Gage < 1)
                   //{//$$$게이지
                   //    file.SetEndFile(true);
                   //    resultManager.FailGame = true;
                   //}

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
                           ScorePic = "No_Image.png";
                       }


                       reportManager.AddSongInfoManager(scoreManager.SongName, scoreManager.TotalScore, ScorePic);


                       isScorePic = false;
                       //현재 노래 제목
                       currentSongName = scoreManager.SongName;

                       if (scoreManager.Combo > scoreManager.Max)
                       {
                           scoreManager.Max = scoreManager.Combo;
                       }
                       //scoreManager.Combo = 0;

                       
                       //점수 기록 파일로 저장
                       //save recored scores in the file
                       reportManager.SaveReport();

                       //photoManager.PhotoFrams.Clear();
                       //gold 파일에  저장
                       int indexGoldPlusItem = 3;

                       double goldEffect = 1;

                     //  int goldPlus = 0;
                       if (itemManager.getRightHandIndex() == indexGoldPlusItem)
                       {
                           goldEffect *= 1.1;
                       }
                       if(itemManager.getLeftHandIndex() == indexGoldPlusItem)
                       {
                        
                             goldEffect *= 1.1;
                       }

                       if(   itemManager.getNoteIndex() == indexGoldPlusItem)
                       {
                           goldEffect *= 1.1;
                          
                       }

                       if (itemManager.getBackgroundIndex() == indexGoldPlusItem)
                       {
                             goldEffect *= 1.15;
                       }
                       if (itemManager.getEffectIndex() == indexGoldPlusItem)
                       {
                            goldEffect *= 1.15;
                       }

                         
                       // goldPlus = 1;
                       scoreManager.Gold = (int)(scoreManager.Gold * goldEffect); 
                           
                       ////골드추가 아이템을 장착 했으면 추가 점수 
                       //if (goldPlus == 1)
                       //{
                           

                       //}
                       //else if (goldPlus == 2)
                       //{

                         

                       //}


                



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

                       //
                       resultManager.FailGame = false;

                       bool isPlay = false;
                       SoundFmod.sndChannel.isPlaying(ref isPlay);
                       if (isPlay)
                       {
                           SoundFmod.StopSound();
                       }
                      
                       SoundFmod.PlaySound(resultMusic);
                      // resultNumberManager.AddResultNumbers(new Vector2(200, 300), scoreManager.Perfect);
               
                   }



                   double playTime = 0;

                   if (isPlayingSong)
                   {

                       
                       SoundFmod.sndChannel.getPosition(ref songLength, TIMEUNIT.MS);
          //             Trace.WriteLine("Time" + (songLength / 1000 / 60) + ":" + (songLength / 1000 % 60) + ":" + (songLength / 10 % 100));
                       playTime = (songLength / 1000 / 60) * 60 + (songLength / 1000 % 60) + ((songLength / 10 % 100) * 0.01 );
                       //Trace.WriteLine(playTime);
                   }

                   





                   //마크 업데이트
                    MarkManager.Update(gameTime);
                    startNoteManager.Update(gameTime);
                    HandleKeyboardInput(Keyboard.GetState());
                    HandleMouseInput(Mouse.GetState());
                    file.Update(spriteBatch, playTime, SoundFmod.changedTempo, SoundFmod.optionalTime);
                    //*** 어떻게 돼는건지 모르겠음 
                    dragNoteManager.Update(gameTime);
                   ////
                    GoldManager.Update(gameTime);
                    perfectManager.Update(gameTime);
                    goodManager.Update(gameTime);
                    badManager.Update(gameTime);
                    goldGetManager.Update(gameTime);
                    scoreManager.Update(gameTime);
                    memberManager.Update(gameTime);
                    
                    

                    SoundFmod.StartChangedTime(gameTime);
                    //photoManager.Update(gameTime);


                    perfectBannerManager.Update(gameTime);
                    goodBannerManager.Update(gameTime);
                    badBannerManager.Update(gameTime);
                    missBannerManager.Update(gameTime);

                    comboNumberManager.Update(gameTime);
                 //   collisionManager.DeleteMarks();
#if Kinect
                    HandleInput();
#endif
                    collisionManager.CheckRightNoteInCenterArea();
                    collisionManager.CheckLeftNoteInCenterArea();

                  //JudgeCharisma();


                    //3초만에 원상복귀
                    //       AutoRetrunChangeTempo(gameTime);

                break;

               #endregion

               #region Setting
               case GameStates.SettingBoard:

                //HandleKeyboardInputGoToMenu(Keyboard.GetState());
#if Kinect
                settingBoard.Update(gameTime, rightHandPosition);
#endif


                pastClick = finalClick;

                break;

               
               
               #endregion

               #region 결과 결산
               //결과 창
                case GameStates.ResultManager:

                   //resultNumberManager.Update(gameTime);

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
                            
                            //@@@이걸 지워서 무제가 될수도 있다. 패턴이 바뀌었는데도 다음번에 안바뀌어서 이걸 넣음
                            //startNoteManager = new StartNoteManager(
                            //    spriteSheet,
                            //    new Rectangle(0, 200, 52, 55),
                            //    1);

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
                            file.SetTime(TimeSpan.Zero);

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
                //Rectangle rectMouseShowPictures = new Rectangle(mouseStateCurrent.X, mouseStateCurrent.Y, 5, 5);

                showPictureScene.Update(gameTime, rightHandPosition);

                break;
                #endregion 
                #region 순위판
                case GameStates.RecordBoard:

                    recordBoard.Update(gameTime, rightHandPosition);

                break;

                
                #endregion
               
               #region 곡선택메뉴
                case GameStates.SongMenu:


                resultSongMenu = songMenu.Update(rightHandPosition);


                
                   
#if Kinect

                //제스쳐//좌우 만 있음
             //   activeRecognizer = CreateRecognizer();
#endif


                //뒤로가기
                if (resultSongMenu == -1)
                    {
                        gameState = GameStates.Menu;
                        bool isPlay = false;
                        SoundFmod.sndChannel.isPlaying(ref isPlay);
                        if (isPlay)
                        {
                            SoundFmod.StopSound();
                        }
                       
                        SoundFmod.PlaySound(title_Music);

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


                    //프로세스 타임 초기화
                        uiProcessTime = 0;

                        lifePlusEffect = 1;

                        //노트 맞는 scale 
                        float[] rightNoteScale = itemManager.GetRightNoteScale();
                        float[] leftNoteScale = itemManager.GetRightNoteScale();
                        float[] longNoteScale = itemManager.GetRightNoteScale();
                       
                      

                      Texture2D[] backgroudTextures  =itemManager.GetBackgroundTexture();

                      ScorePic = null;
                       
                         //배경 바꾸기
                        playBackgroud = backgroudTextures[itemManager.getBackgroundIndex()];
                        
                        ////롱노트 //*** 임시로 오른손노트랑 똑같은걸로 해놓음
                        startNoteManager.changeLongNoteImage(longNoteTextures[itemManager.getNoteIndex()], new Rectangle(0, 0, longNoteTextures[itemManager.getNoteIndex()].Width, longNoteTextures[itemManager.getNoteIndex()].Height), longNoteScale[0]);

                        ////왼손노트

                        //고스톱이 아니면
                        if (itemManager.getNoteIndex() != 3)
                        {
                            startNoteManager.changeLeftNoteImage(leftNoteTextures[itemManager.getNoteIndex()], new Rectangle(0, 0, leftNoteTextures[itemManager.getNoteIndex()].Width, leftNoteTextures[itemManager.getNoteIndex()].Height), leftNoteScale[0]);
                        }
                    // 고스톱이면 스프라이트 적용
                        else if (itemManager.getNoteIndex() == 3)
                        {
                            startNoteManager.changeLeftNoteImage(leftNoteTextures[itemManager.getNoteIndex()], new Rectangle(0, 0, 100, leftNoteTextures[itemManager.getNoteIndex()].Height), leftNoteScale[0],15, 10);

                        }





                        //오른손 노트 이미지 바꾸기

                        //고스톱이 아니면
                        if (itemManager.getNoteIndex() != 3)
                        {
                            startNoteManager.changeRightNoteImage(rightNoteTextures[itemManager.getNoteIndex()], new Rectangle(0, 0, rightNoteTextures[itemManager.getNoteIndex()].Width, rightNoteTextures[itemManager.getNoteIndex()].Height), rightNoteScale[0]);
                        }


                        // 고스톱이면 스프라이트 적용
                        //
                        else if (itemManager.getNoteIndex() == 3)
                        {
                            startNoteManager.changeRightNoteImage(rightNoteTextures[itemManager.getNoteIndex()], new Rectangle(0, 0, 100, rightNoteTextures[itemManager.getNoteIndex()].Height), rightNoteScale[0],15 ,10);
                 
                        }

                        //curveManager.DeleteAllCurve();

                        Texture2D[] dragTextures = itemManager.GetDragNoteTexture();
                        Texture2D[] dragTextureBackgrounds = itemManager.GetDragNoteBackground();
                        
                        int noteIndex = itemManager.getNoteIndex();

                        //드래그노트 이미지 바꾸기
                        dragNoteManager.Texture = dragTextures[noteIndex];

                        //드래그노트 가 무엇인지 인덱스
                        dragNoteManager.Index = noteIndex;

                        //드래그노트 크기 설정
                     //   dragNoteManager.InitialFrame = itemManager.GetDragNoteInitFrame()[noteIndex];

                        //드래그노트 백그라운드 이마지 바꾸기
                        dragNoteManager.Background = dragTextureBackgrounds[noteIndex];

                        

                        //마커리스트 텍스쳐 가져오기
                        Texture2D[] markersTextures = itemManager.GetMarkerTexture();
                        
                        //*** 마커리스트에 맞는 rect, 바로width와 height 를 가져와서 넣기 때문에 필요 없을 수 도 있다.
                        //Rectangle[] markersRectangle = new Rectangle[5];
                        //markersRectangle[0] = new Rectangle(0,0,265,240);

                        //***마커리스트에 맞는 =>>itemManager로 옮김
                        float[] markersScale = itemManager.GetMarkersScale();

                    //배너문구 초기화
                        perfectBannerManager.Clear();
                        goodBannerManager.Clear();
                        badBannerManager.Clear();
                        missBannerManager.Clear();
                    //콤보숫자 초기화
                        comboNumberManager.Clear();


                    //템포 초기화 
                        SoundFmod.isChangedTempo = 0;
                        SoundFmod.changedTempo = 0;
                        for (int q = 0; q < 6; q++)
                        {
                            memberManager.SetMemberState(q, 0);
                        }
                    //속도초기화 
                        memberManager.SetMembersFrameTime(0.1f);
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

                        collisionManager = new CollisionManager(perfectManager, goodManager, badManager, goldGetManager, scoreManager, memberManager, itemManager, perfectBannerManager, goodBannerManager, badBannerManager, missBannerManager, new Vector2(this.Window.ClientBounds.Width, this.Window.ClientBounds.Height), comboNumberManager, charismaManager,dragNoteManager);
            

                        /////이펙트 생성 -END

                        //lifeplus 아이템 장착
                        int indexLifePlusItem = 2;
                        //라이프아이템의 인덱스

                        if (itemManager.getRightHandIndex() == indexLifePlusItem)
                        {
                            lifePlusEffect*=1.1f;
                        }
                        if(itemManager.getLeftHandIndex() == indexLifePlusItem)
                        {
                            lifePlusEffect*=1.1f;
                        }
                        if (itemManager.getNoteIndex() == indexLifePlusItem)
                        {
                            lifePlusEffect*=1.1f;
                        }
                        if(itemManager.getBackgroundIndex() == indexLifePlusItem)
                        {
                            lifePlusEffect*=1.15f;
                        }
                        if(itemManager.getEffectIndex() == indexLifePlusItem)
                        {
                            lifePlusEffect*=1.15f;
                        }
                         

                        gameState = GameStates.Playing;
                        
                        file.Loading(resultSongMenu);

                        uiEndTime = file.EndTime;

                        bool isPlay = false;
                        SoundFmod.sndChannel.isPlaying(ref isPlay);
                        if (isPlay)
                        {
                            SoundFmod.StopSound();
                        }
                      
                        SoundFmod.PlaySound(songsDir + noteFileManager.noteFiles[resultSongMenu].Mp3);
                        isPlayingSong = true;
                    }

                    break;
               #endregion

               #region 튜토리얼씬

                case GameStates.TutorialScene:

                    int resultTutorialScene = tutorialScene.Update();


                    
                    if (resultTutorialScene == -1)
                    {
                        gameState = GameStates.Menu;
                       

                    }




                    break;

               #endregion


           }
           mouseStatePrevious = mouseStateCurrent;
           pastClick = finalClick;

           backJestureManager.Update(gameTime);
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
                    dbVal = dbVal * 1.3;
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

            _total_frames++;
//#if Kinect
//            getDepthFrame();
//#endif

            //타이틀화면
            #region 로딩 화면
            if (gameState == GameStates.Loading)
            {

              //  loadingScene.Draw(spriteBatch);
                if (loadingTime == 0) //로딩해야한다.
                {

                    spriteBatch.Draw(loadingForAngle, new Rectangle(0, 0, 1024, 769), Color.White);

                }
                else if (loadingTime == 1) //각도 이미 설정
                {

                    bool isPlay = false;
                    SoundFmod.sndChannel.isPlaying(ref isPlay);
                    if (isPlay)
                    {
                        SoundFmod.StopSound();
                    }

                    SoundFmod.PlaySound(title_Music);
                    loadingTime = 2;//게임시작
                    gameState = GameStates.Menu;
                }

                if (isNoPerson)
                {
                    spriteBatch.Draw(noPerson, new Rectangle(0, 0, 1024, 769), Color.White);

                }
            }



            #endregion



            #region 타이틀화면




            if (gameState == GameStates.Menu)
            {      
                menuScene.Draw(spriteBatch);
                if (loadingTime == 0) //로딩해야한다.
                {

                    spriteBatch.Draw(loadingForAngle, new Rectangle(0, 0, 1024, 769), Color.White);

                }
                else if (loadingTime == 1) //각도 이미 설정
                {

                    bool isPlay = false;
                    SoundFmod.sndChannel.isPlaying(ref isPlay);
                    if (isPlay)
                    {
                        SoundFmod.StopSound();
                    }

                    SoundFmod.PlaySound(title_Music);
                    loadingTime = 2;//게임시작
                }
             


             
#if Kinect
                if (KinectVideoTexture != null)
                {
                    spriteBatch.Draw(KinectVideoTexture, VideoDisplayRectangle, Color.White);
                    //setupKinect.draw();

                }

#endif



            }

            #endregion

            #region 상점 대문
            //상전대문
            if (gameState == GameStates.ShopDoor)
            {


                shopDoor.Draw(spriteBatch, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height);

#if Kinect
                if (KinectVideoTexture != null)
                {
                    spriteBatch.Draw(KinectVideoTexture, VideoDisplayRectangle, Color.White);
                    //setupKinect.draw();

                }
    
#endif

            }

            #endregion
            #region 아이템샵들
            if (gameState == GameStates.RightItemShop)
            {
                rightItemShop.Draw(spriteBatch, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height);
#if Kinect
                if (KinectVideoTexture != null)
                {
                    spriteBatch.Draw(KinectVideoTexture, VideoDisplayRectangle, Color.White);
                    //setupKinect.draw();

                }
          
#endif
            }

            if (gameState == GameStates.LeftItemShop)
            {
                leftItemShop.Draw(spriteBatch, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height);
#if Kinect
                if (KinectVideoTexture != null)
                {
                    spriteBatch.Draw(KinectVideoTexture, VideoDisplayRectangle, Color.White);
                    //setupKinect.draw();

                }
          
#endif
            }


            if (gameState == GameStates.EffectItemShop)
            {
                effectItemShop.Draw(spriteBatch, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height);
#if Kinect
                if (KinectVideoTexture != null)
                {
                    spriteBatch.Draw(KinectVideoTexture, VideoDisplayRectangle, Color.White);
                    //setupKinect.draw();

                }
       
#endif
            }

            if (gameState == GameStates.NoteItemShop)
            {
                noteItemShop.Draw(spriteBatch, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height);
#if Kinect
                if (KinectVideoTexture != null)
                {
                    spriteBatch.Draw(KinectVideoTexture, VideoDisplayRectangle, Color.White);
                    //setupKinect.draw();

                }
           
#endif
            }


            if (gameState == GameStates.BackgroundItemShop)
            {
                backgroundItemShop.Draw(spriteBatch, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height);
#if Kinect
                if (KinectVideoTexture != null)
                {
                    spriteBatch.Draw(KinectVideoTexture, VideoDisplayRectangle, Color.White);
                    //setupKinect.draw();

                }
    
#endif
            }


            #endregion


            #region 곡 선택
            if (gameState == GameStates.SongMenu)
            {
                songMenu.Draw(spriteBatch);
               

#if Kinect
                //if (message.Length > 0)
                //{
                //    spriteBatch.DrawString(messageFont, message, Vector2.Zero, Color.Red);
                //}

                //

                if (KinectVideoTexture != null)
                {
                    spriteBatch.Draw(KinectVideoTexture, VideoDisplayRectangle, Color.White);
                    //setupKinect.draw();

                }
              


#endif
            }

            #endregion 

            #region 플레이화면
            if ((gameState == GameStates.Playing))
            {
       

                //배경
                spriteBatch.Draw(playBackgroud,
                new Rectangle(0, 0, this.Window.ClientBounds.Width,
                    this.Window.ClientBounds.Height),
                    Color.White);

             

                double tempo =SoundFmod.changedTempo;
             

                uiProcessTime = (file.ProcessTime).TotalSeconds;
    
                uiProcessTime = (uiProcessTime / uiEndTime * 800);


                //uiProcessTime = (int) (uiProcessTime / 100.0 * 800);
                spriteBatch.Draw(processBar, new Vector2(0, 730), new Rectangle(0, 0, 1024, 38), Color.White);
                spriteBatch.Draw(processMark, new Vector2((float)uiProcessTime+120, 704), new Rectangle(0, 0, 49, 59), Color.White);
                

                Texture2D metoroTex = GetMetroTexture(tempo);
               
                
                //메트로늄
                spriteBatch.Draw(
                  metoroTex,
                    //위치: Center-> location 으로 바꿈 (마커와 노트 매칭 떄문에 )
                  new Vector2(-5, 600),

                  new Rectangle(0, 0, 150, 169),
                  Color.White,
                  0f,
                    //origin ->  new Vector2(frameWidth / 2, frameHeight / 2) ->  new Vector2(0,0) 으로 바꿈 (마커와 노트 매칭 떄문에 )
                  new Vector2(0, 0),
                    //오른쪽 마크 크기 
                  1f,
                  SpriteEffects.None,
                  0.0f);


                memberManager.Draw(spriteBatch);
               
                MarkManager.Draw(spriteBatch);
                
                //startnoteclass에 가야 보이고 안보이게 할 수 있음
                startNoteManager.Draw(spriteBatch);

                

                

                
                //이걸 주석하면 드래그노트 체크하는거 안보임 하지만 체크는 됨
                //DragNoteManager.Draw(spriteBatch);
                
                

                TimeSpan processTime = file.Draw(spriteBatch, gameTime);


                //처음에 아이템 설명

                if (processTime < TimeSpan.FromSeconds(5))
                {
                    //오른손
                    Color color = Color.White;
                    color.A = 80;

                    int index = itemManager.getNoteIndex();

                    int x = 210;
                    int y = 350;

                    spriteBatch.Draw(darkBackgroundImage, new Rectangle(0, 0, 1024, 769), color);
                    Texture2D rightNote;
                    if (index != 3)
                    {
                        rightNote = itemManager.GetRightNoteTexture()[index];
                    }
                    else
                    {
                        rightNote = itemManager.GetGo_stop_One()[0];
                    }

                    spriteBatch.Draw(rightNote, new Rectangle(x, y, 100, 100), Color.White);
                    spriteBatch.DrawString(georgia, "Right", new Vector2(x-30, y + 100), Color.White);


                    //왼손
                    Texture2D leftNote;
                    if (index != 3)
                    {
                        leftNote = itemManager.GetLeftNoteTexture()[index];
                    }
                    else
                    {
                        leftNote = itemManager.GetGo_stop_One()[1];
                    }

                    spriteBatch.Draw(leftNote, new Rectangle(x+150, y, 100, 100), Color.White);
                    spriteBatch.DrawString(georgia, "Left", new Vector2(x + 150, y + 100), Color.White);


                    //롱노트
                    
                    Texture2D longNote = itemManager.GetLongNoteTexture()[index];
                    
                    spriteBatch.Draw(longNote, new Rectangle(x + 300, y, 100, 100), Color.White);
                    spriteBatch.DrawString(georgia, "Long", new Vector2(x + 290, y + 100), Color.White);



                    //드래그노트 
                    Texture2D dragNote = itemManager.GetDragNoteTexture()[index];
                    spriteBatch.Draw(dragNote, new Rectangle(x + 450, y, 100, 100), Color.White);

                    spriteBatch.DrawString(georgia, "Drag", new Vector2(x + 440, y + 100), Color.White);


                }


                curveManager.Draw(gameTime, spriteBatch, processTime);
                guideLineManager.Draw(processTime, spriteBatch);

                //기본 템포 설정( 템포가 바뀐상태이면 안변함)
                SoundFmod.SetBasicTempo();

                //하트. gage양 만큼 하트가 나타남.

                //330은 현재 최대 width, 이건 그림이 바뀌면 바뀜
                //100은 gage의 최대값. 

                GoldManager.Draw(spriteBatch);


               // int gageMax = 330;
                //게이지에 이펙트
                //이펙트 없으면 1 
                double gageRate = 3.3 * lifePlusEffect;
                

                int gageWidth = (int)(gageRate * scoreManager.Gage);

                if (gageWidth > 330)
                {
                    gageWidth = 330;
                }
              

                spriteBatch.Draw(energyDarkBack, new Vector2(0, 6), new Rectangle(0, 0, 335, 35), Color.White);


                spriteBatch.Draw(energy, new Vector2(0, 6), new Rectangle(0, 0, gageWidth, 35), Color.White);
                spriteBatch.Draw(uiEnergyBackground, new Vector2(0, 0), Color.White);

                spriteBatch.DrawString(georgia, scoreManager.TotalScore.ToString(), new Vector2(900, 2), Color.LightGray, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0f);


#if Kinect
              
              


                if (charismaManager.charismaFrames.Count > 0)
                {
                    string fileName;
                  //  charismaManager.currentTime += gameTime.ElapsedGameTime.TotalSeconds;
                    CharisimaFrame charismaFrame = (CharisimaFrame)charismaManager.charismaFrames.Peek();


                    if (processTime >= TimeSpan.FromSeconds(charismaFrame.StartTime))
                    {

                        spriteBatch.Draw(charismaFrame.Texture, charismaManager.picLocation, Color.White);
                        spriteBatch.Draw(charismaFrame.Message, charismaManager.picLocation, Color.White);
                        //Trace.WriteLine(charismaManager.IsCharismaTime);
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
                                fileName = "1.txt";
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
                                fileName = "2.txt";
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
                                fileName = "3.txt";
                                LoadGesturesFromFile(fileName);
                                Skeleton2DDataExtract.Skeleton2DdataCoordReady += NuiSkeleton2DdataCoordReadyPosture;

                            }
                            else if (charismaManager.Type == 4)
                            {
                                ////카리스마타임 제스쳐 시작부분
                                Skeleton2DDataExtract.Skeleton2DdataCoordReady -= NuiSkeleton2DdataCoordReadyStop;
                                gestureType = 3;
                                postureCount = 0;
                                gestureFlag = true;
                                fileName = "4.txt";
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
                                fileName = "6.txt";
                                LoadGesturesFromFile(fileName);
                                Skeleton2DDataExtract.Skeleton2DdataCoordReady += NuiSkeleton2DdataCoordReadyPosture;

                            }
                            charismaManager.IsCharismaTime = false;
                        }
                    }
                 //   Trace.WriteLine(isGesture);

                    if (processTime >= TimeSpan.FromSeconds(charismaFrame.EndTime))
                    {

                        isGesture = false;
                        charismaManager.PlayCharisma = false;
                        charismaManager.charismaFrames.Dequeue();
                    }

                    if (!isGesture)
                    {
                        //포스쳐 
                        if (gestureType == 2)
                        {
                            Skeleton2DDataExtract.Skeleton2DdataCoordReady -= NuiSkeleton2DdataCoordReadyPosture;

                            if (postureCount == 0)
                            {
                                missBannerManager.AddBanners(collisionManager.missLocation ,collisionManager.missBannerScale);
                                scoreManager.Gage -= 20;


                                if (scoreManager.Combo > scoreManager.Max)
                                {
                                    scoreManager.Max = scoreManager.Combo;
                                }
                                scoreManager.Combo = 0;
                                scoreManager.ComboChanged = true;
                            }


                        }

                        //제스쳐
                        if (gestureType == 3)
                        {
                            Skeleton2DDataExtract.Skeleton2DdataCoordReady -= NuiSkeleton2DdataCoordReadyGesture;
                            if (postureCount == 0)
                            {
                                missBannerManager.AddBanners(collisionManager.missLocation, collisionManager.missBannerScale);

                                scoreManager.Gage -= 20;


                                if (scoreManager.Combo > scoreManager.Max)
                                {
                                    scoreManager.Max = scoreManager.Combo;
                                }
                                scoreManager.Combo = 0;
                                scoreManager.ComboChanged = true;
                            }


                        }
                        gestureType = 1;
                        postureCount = 0;
                        gestureFlag = true;
                        fileName = "0.txt";
                        LoadGesturesFromFile(fileName);
                        Skeleton2DDataExtract.Skeleton2DdataCoordReady += NuiSkeleton2DdataCoordReadyStop;
                        isGesture = true;
                      

                    }


                 



                }

             //   photoManager.Draw(gameTime, spriteBatch);




                if (KinectVideoTexture != null)
                {
                    spriteBatch.Draw(KinectVideoTexture, VideoDisplayRectangle, Color.White);
                    //setupKinect.draw();

                }
                //if (Skeletons != null)
                //{
                //    foreach (Skeleton s in Skeletons)
                //    {
                //        if (s.TrackingState == SkeletonTrackingState.Tracked)
                //        {
                //            drawpoint(s.Joints[JointType.HandRight], s.Joints[JointType.HandLeft]);

                //        }
                //    }

                //}

                if (skeleton != null)
                {

                    if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
                    {
                       
                            drawpoint(skeleton.Joints[JointType.HandRight], skeleton.Joints[JointType.HandLeft]);
                       
                    }

                }

                


#endif


                //dragNoteManager.Draw(spriteBatch);
                missBannerManager.Draw(spriteBatch);
                badBannerManager.Draw(spriteBatch);
                goodBannerManager.Draw(spriteBatch);
                perfectBannerManager.Draw(spriteBatch);
                
                comboNumberManager.Draw(spriteBatch);


                perfectManager.Draw(spriteBatch);
                goodManager.Draw(spriteBatch);
                badManager.Draw(spriteBatch);
                goldGetManager.Draw(spriteBatch);
       
                //if (message.Length > 0)
                //{
                //    spriteBatch.DrawString(messageFont, message, Vector2.Zero, Color.Red);
                //}

      //          spriteBatch.Draw(sit1,
      //new Rectangle(0, 0, this.Window.ClientBounds.Width,
      //    this.Window.ClientBounds.Height),
      //    Color.White);


            }
            #endregion
            #region 결과 화면
            if (gameState == GameStates.ResultManager)
            {
                //class resultManager 에 draw실행
                resultManager.Draw(spriteBatch,reportManager.IsHighScore(currentSongName,scoreManager.TotalScore));

                //노래제목
                String songFile = scoreManager.SongName;
                //제목으로 노트파일에서 찾기
                NoteFile noteFile = noteFileManager.FindNoteFile(songFile);
                
                //노트파일로 사진 가져오기
                spriteBatch.Draw(songMenu.FindPicture(noteFile), new Rectangle(120, 200, 160, 160), Color.White);

                String name = noteFile.Name;
                if (name.Length > 15)
                {

                    name = name.Remove(15);
                    name = name + "...";
                }


                //노트파일로 노래 제목가져오기
                spriteBatch.DrawString(georgia, name, new Vector2(320, 200), Color.Gray);

                String artist = noteFile.Artist;
                if (artist.Length > 15)
                {

                    artist = artist.Remove(15);
                    artist = artist + "...";
                }
                
                
                //노트파일로 가수 가져오기

                spriteBatch.DrawString(georgia, artist, new Vector2(320, 260), Color.Gray);
                //***//난이도//spriteBatch.DrawString(pericles36Font, , new Vector2(200,80), Color.White);



                Rectangle rec = new Rectangle(0, 0, noteFile.Level * 26/*하나의 그림의 width*/, 22);
                //하트. gage양 만큼 하트가 나타남.
                spriteBatch.Draw(Game1.levelTexture, new Vector2(320, 320), rec, Color.White);


               // int indexGoldPlusItem = 3;
               // int indexLifePlusItem = 2;

               //// int goldPlus = 0;
               // if (itemManager.getRightHandIndex() == indexGoldPlusItem ||
               //    itemManager.getLeftHandIndex() == indexGoldPlusItem ||
               //    itemManager.getNoteIndex() == indexGoldPlusItem)
               // {

               //  //   goldPlus = 1;
               //     spriteBatch.Draw(goldPlusEffect10, new Vector2(750, 400), Color.White);
               //    // spriteBatch.Draw(lifePlusEffect15, thirdItemLocation, Color.White);
               // }

               // if (itemManager.getBackgroundIndex() == indexGoldPlusItem ||
               //    itemManager.getEffectIndex() == indexGoldPlusItem)
               // {
               //     spriteBatch.Draw(goldPlusEffect15, new Vector2(750, 400), Color.White);
               //    // goldPlus = 2;
               // }



               // if (itemManager.getRightHandIndex() == indexLifePlusItem ||
               // itemManager.getLeftHandIndex() == indexLifePlusItem ||
               // itemManager.getNoteIndex() == indexLifePlusItem)
               // {

               //     //   goldPlus = 1;
               //     spriteBatch.Draw(lifePlusEffect10, new Vector2(750, 300), Color.White);
               //     // spriteBatch.Draw(lifePlusEffect15, thirdItemLocation, Color.White);
               // }

               // if (itemManager.getBackgroundIndex() == indexLifePlusItem ||
               //    itemManager.getEffectIndex() == indexLifePlusItem)
               // {
               //     spriteBatch.Draw(lifePlusEffect15, new Vector2(750, 300), Color.White);
               //     // goldPlus = 2;
               // }
                


                #if Kinect
                if (KinectVideoTexture != null)
                {
                    spriteBatch.Draw(KinectVideoTexture, VideoDisplayRectangle, Color.White);
                    //setupKinect.draw();

                }
            
                #endif
         
            }

            #endregion

            #region 사진 보여주기 


            if (gameState == GameStates.ShowPictures)
            {
                showPictureScene.Draw(spriteBatch);
              
                if(playingPictures.Count > 0 )
                {
                    //3개보다 많으면 3으로 
                    playPicturesCount = (playingPictures.Count > 3 ? 3 : playingPictures.Count);
                    int i;

                    for (i = 0; i < playPicturesCount; i++)
                    {
                                          
                        showPictureTextures[i] = (Texture2D)playingPictures.Dequeue();


                    }

                }
                for (int i = 0; i < playPicturesCount; i++)
                    {
                        if (i == 0)
                        {
                         //   spriteBatch.Draw(showPictureTextures[i], new Rectangle(200, (i + 1) * 100, 100, 100), Color.White);
                            spriteBatch.Draw(showPictureTextures[i], new Vector2(42f, 358f), new Rectangle(showPictureTextures[i].Width / 2 - 126, showPictureTextures[i].Height/2 - 126, 252, 252), Color.White, -0.26f, Vector2.Zero, 1f, SpriteEffects.None, 1.0f);
                        }
                        else if (i == 1)
                        {
                            spriteBatch.Draw(showPictureTextures[i], new Vector2(396f, 261f), new Rectangle(showPictureTextures[i].Width / 2 - 126, showPictureTextures[i].Height / 2 - 126, 252, 252), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1.0f);
                        }
                        else if (i == 2)
                        {
                            spriteBatch.Draw(showPictureTextures[i], new Vector2(739f, 359f), new Rectangle(showPictureTextures[i].Width / 2 - 126, showPictureTextures[i].Height / 2 - 126, 252, 252), Color.White, 0.26f, Vector2.Zero, 1f, SpriteEffects.None, 1.0f);

                        }
                    }

#if Kinect
                if (KinectVideoTexture != null)
                {
                    spriteBatch.Draw(KinectVideoTexture, VideoDisplayRectangle, Color.White);
                    //setupKinect.draw();

                }
           
#endif
            }

            #endregion

            #region Setting
            if (gameState == GameStates.SettingBoard)
            {

#if Kinect
                settingBoard.Draw(spriteBatch);
#endif
#if Kinect
                if (KinectVideoTexture != null)
                {
                    spriteBatch.Draw(KinectVideoTexture, VideoDisplayRectangle, Color.White);
                    //setupKinect.draw();

                }
           
#endif
            }

            #endregion


            #region 스코어 보드

            if (gameState == GameStates.RecordBoard)
            {
                recordBoard.Draw(spriteBatch);

                //현재 노래 제목으로 5개 높은 노래 가져오기
                List<ScoreInfo> highScores = reportManager.GetHighScore(currentSongName);

                //노래제목
             //   String songFile = scoreManager.SongName;
                //제목으로 노트파일에서 찾기
                NoteFile noteFile = noteFileManager.FindNoteFile(currentSongName);

                //노트파일로 사진 가져오기
                spriteBatch.Draw(songMenu.FindPicture(noteFile), new Rectangle(98, 182, 121, 123), Color.White);



                String name = noteFile.Name;
                if (name.Length > 15)
                {

                    name = name.Remove(15);
                    name = name + "...";
                }


                String artist = noteFile.Artist;
                if (artist.Length > 15)
                {

                    artist = artist.Remove(15);
                    artist = artist + "...";
                }



                //노트파일로 노래 제목가져오기
                spriteBatch.DrawString(georgia, name, new Vector2(270, 162), Color.Gray);
                //노트파일로 가수 가져오기
                spriteBatch.DrawString(georgia, artist, new Vector2(270, 222), Color.Gray);
                //***//난이도//spriteBatch.DrawString(pericles36Font, , new Vector2(200,80), Color.White);


                Rectangle rec = new Rectangle(0, 0, noteFile.Level * 26/*하나의 그림의 width*/, 22);
                //하트. gage양 만큼 하트가 나타남.
                spriteBatch.Draw(Game1.levelTexture, new Vector2(270, 282), rec, Color.White);


                int i;
                for (i = 0; i < highScores.Count; i++)
                {
                  //  Texture2D picture = reportManager.FindPicture(highScores[i].UserPicture);
                    
                    //노래 사진
                    if (i == 0)
                    {
                        spriteBatch.Draw(reportManager.FindPicture(highScores[i].UserPicture), new Rectangle(163, 347, 113, 113), Color.White);
                     //   spriteBatch.Draw(picture, new Vector2(163, 347), new Rectangle(picture.Width / 2 - 56, picture.Height / 2 - 56, 113, 113), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1.0f);
              
                        spriteBatch.DrawString(georgia, highScores[i].Score.ToString(), new Vector2(283, 347), Color.DarkRed);
                    }
                    else if (i == 1)
                    {
                        spriteBatch.Draw(reportManager.FindPicture(highScores[i].UserPicture), new Rectangle(163, 469, 113, 113), Color.White);
                        spriteBatch.DrawString(georgia, highScores[i].Score.ToString(), new Vector2(283, 469), Color.DarkRed);
                    }
                    else if (i == 2)
                    {
                        spriteBatch.Draw(reportManager.FindPicture(highScores[i].UserPicture), new Rectangle(163, 590, 113, 113), Color.White);
                        spriteBatch.DrawString(georgia, highScores[i].Score.ToString(), new Vector2(283, 590), Color.DarkRed);
                    }
                    else if (i == 3)
                    {
                        spriteBatch.Draw(reportManager.FindPicture(highScores[i].UserPicture), new Rectangle(585, 347, 80, 80), Color.White);
                        spriteBatch.DrawString(georgia, highScores[i].Score.ToString(), new Vector2(685, 347), Color.Gray);
                    }
                    else if (i == 4)
                    {
                        spriteBatch.Draw(reportManager.FindPicture(highScores[i].UserPicture), new Rectangle(585, 439, 80, 80), Color.White);
                        spriteBatch.DrawString(georgia, highScores[i].Score.ToString(), new Vector2(685, 439), Color.Gray);
                    }
                    else if (i == 5)
                    {
                        spriteBatch.Draw(reportManager.FindPicture(highScores[i].UserPicture), new Rectangle(585, 530, 80, 80), Color.White);
                        spriteBatch.DrawString(georgia, highScores[i].Score.ToString(), new Vector2(685, 530), Color.Gray);
                    }
                    else if (i == 6)
                    {
                        spriteBatch.Draw(reportManager.FindPicture(highScores[i].UserPicture), new Rectangle(585, 622, 80, 80), Color.White);
                        spriteBatch.DrawString(georgia, highScores[i].Score.ToString(), new Vector2(685, 622), Color.Gray);
                    }
                

                }
#if Kinect
                if (KinectVideoTexture != null)
                {
                    spriteBatch.Draw(KinectVideoTexture, VideoDisplayRectangle, Color.White);
                    //setupKinect.draw();

                }
            
#endif
            }


            #endregion

            #region 튜토리얼씬

            if (gameState == GameStates.TutorialScene)
            {
                tutorialScene.Draw(spriteBatch);
             
                #if Kinect
                if (KinectVideoTexture != null)
                {
                    spriteBatch.Draw(KinectVideoTexture, VideoDisplayRectangle, Color.White);
                    //setupKinect.draw();

                }

                #endif
            }

            #endregion

            if (gameState != GameStates.Playing)
            {
                #if Kinect
                if (skeleton != null)
                {

                    if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
                    {
                        drawpoint(skeleton.Joints[JointType.HandRight], skeleton.Joints[JointType.HandLeft]);

                    }

                }
                    #endif
            }
            //Trace.WriteLine(finalClick);
#if Kinect


//
//            if (KinectVideoTexture != null)
//            {
//                spriteBatch.Draw(KinectVideoTexture, VideoDisplayRectangle, Color.White);
//                //setupKinect.draw();

//            }
//            if (skeleton != null)
//            {

//                if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
//                    {
//                        drawpoint(skeleton.Joints[JointType.HandRight], skeleton.Joints[JointType.HandLeft]);

//                    }
//            }

            if (fcenterZ < 1.8)
            {

                //앞뒤로 가시오
                spriteBatch.Draw(go_back, new Rectangle(0, 0, 1024, 768), Color.White);



            }

            if (fcenterZ > 2.8)
            {

                //앞뒤로 가시오
                spriteBatch.Draw(go_front, new Rectangle(0, 0, 1024, 768), Color.White);



            }
            backJestureManager.Draw(spriteBatch);
            
#endif
            spriteBatch.End();
            
            base.Draw(gameTime);
        }


        
#if Kinect
        void drawpoint(Joint j1, Joint j2)
        {


            ////실질적인 스케일 변환
            j1r = j1.ScaleTo(SCR_W, SCR_H, userParam, userParam);
            
     
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


            //스케일 없을떄는.,
            //spriteBatch.Draw(rightHandTextures[itemManager.getRightHandIndex()],new Vector2(drawrec1.X, drawrec1.Y),Color.White);

            Vector2 pointLocation = new Vector2(drawrec1.X, drawrec1.Y);
            if (itemManager.getRightHandIndex() == 5 || itemManager.getRightHandIndex() == 3)
            {
                pointLocation = new Vector2(drawrec1.X-40,drawrec1.Y-40);
            }



            spriteBatch.Draw(
             rightHandTextures[itemManager.getRightHandIndex()],
                    //위치: Center-> location 으로 바꿈 (마커와 노트 매칭 떄문에 )
            pointLocation,

             null,
             Color.White,
             0f,
                    //origin ->  new Vector2(frameWidth / 2, frameHeight / 2) ->  new Vector2(0,0) 으로 바꿈 (마커와 노트 매칭 떄문에 )
             new Vector2(0, 0),
                    //오른쪽 마크 크기 
             0.8f,
             SpriteEffects.None,
             0.0f);   

            j2r = j2.ScaleTo(SCR_W, SCR_H, userParam, userParam);

            //drawrec2.X = (int)j2r.Position.X - drawrec2.Width / 2;
            //drawrec2.Y = (int)j2r.Position.Y - drawrec2.Height / 2;
            drawrec2.X = (int)j2r.Position.X ;
            drawrec2.Y = (int)j2r.Position.Y ;

            Texture2D leftHandTexture = leftHandTextures[itemManager.getLeftHandIndex()];
            
            //스케이 ㄹ없을 때는
             //spriteBatch.Draw(leftHandTexture, new Vector2((float)drawrec2.X - (float)(leftHandTexture.Width * 0.5), (float)drawrec2.Y - (float)(leftHandTexture.Height * 0.5)), Color.White);
            //Trace.WriteLine(drawrec2);

            spriteBatch.Draw(
             leftHandTexture,
                //위치: Center-> location 으로 바꿈 (마커와 노트 매칭 떄문에 )
             new Vector2((float)drawrec2.X - (float)(leftHandTexture.Width * 0.25f), (float)drawrec2.Y - (float)(leftHandTexture.Height * 0.25f)),
                    //  new Vector2((float)drawrec2.X , (float)drawrec2.Y ),
             null,
             Color.White,
             0f,
                //origin ->  new Vector2(frameWidth / 2, frameHeight / 2) ->  new Vector2(0,0) 으로 바꿈 (마커와 노트 매칭 떄문에 )
              new Vector2(0, 0),
                //오른쪽 마크 크기 
             0.5f,
             SpriteEffects.None,
             0.0f);   

        }
#endif
       
    }

}