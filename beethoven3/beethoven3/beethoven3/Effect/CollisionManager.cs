using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

using System.Diagnostics;
namespace beethoven3
{
    class CollisionManager
    {
        #region declarations

        private PerfectExplosionManager perfectManager;
        private GoodExplosionManager goodManager;
        private BadExplosionManager badManager;
        private ExplosionManager goldGetManager;

        //글씨나타내기
        private PerfectBannerManager perfectBannerManager;
        private GoodBannerManager goodBannerManager;
        private BadBannerManager badBannerManager;
        private MissBannerManager missBannerManager;

        //Combo 띄우기

        private ComboNumberManager comboNumberManager;


        private ScoreManager scoreManager;

        private MemberManager memberManager;

        private ItemManager itemManager;
        private float roundPoint = 15.0f;

        private Vector2 sizeScreen;
        private Vector2 perfectLocation;
        private Vector2 goodLocation;
        private Vector2 badLocation;
        private Vector2 missLocation;

        private Vector2 comboLocation;
       // private Vector2 goodComboLocation;
        private CharismaManager charismaManager;
        private DragNoteManager dragNoteManager;



        private float perfectBannerScale = 0.2f;
        private float goodBannerScale = 0.2f;
        private float badBannerScale = 0.2f;
        private float missBannerScale = 0.2f;


        private Vector2 perfectSize = new Vector2(1380.0f, 428.0f);
        private Vector2 goodSize = new Vector2(1020.0f, 368.0f);
        private Vector2 badSize = new Vector2(782.0f, 400.0f);
        private Vector2 missSize = new Vector2(975.0f, 412.0f);
       // private bool isEarlyOne = true;
        #endregion 

        #region constructor
        public CollisionManager(PerfectExplosionManager perfectManager, GoodExplosionManager goodManager, BadExplosionManager badManager, ExplosionManager goldGetManager,
            ScoreManager scoreManager, MemberManager memberManager, ItemManager itemManager,
            PerfectBannerManager perfectBannerManager, GoodBannerManager goodBannerManager, BadBannerManager badBannerManager, MissBannerManager missBannerManager, Vector2 sizeScreen,
            ComboNumberManager comboNumberManager, CharismaManager charismaManager, DragNoteManager dragNoteManager)
        {
            this.perfectManager = perfectManager;
            this.goodManager = goodManager;
            this.badManager = badManager;
            this.goldGetManager = goldGetManager;

            this.perfectBannerManager = perfectBannerManager;
            this.goodBannerManager = goodBannerManager;
            this.badBannerManager = badBannerManager;
            this.missBannerManager = missBannerManager;

            this.comboNumberManager = comboNumberManager;

            this.scoreManager = scoreManager;

            this.charismaManager = charismaManager;

            //화면사이즈
            this.sizeScreen = sizeScreen;
            this.memberManager = memberManager;
            this.itemManager = itemManager;

            this.dragNoteManager = dragNoteManager;

        
            this.perfectLocation = new Vector2(sizeScreen.X / 2 - (perfectSize.X * perfectBannerScale) / 2, sizeScreen.Y / 2 - (perfectSize.Y * perfectBannerScale) / 2);


            this.goodLocation = new Vector2(sizeScreen.X / 2 - (goodSize.X * goodBannerScale) / 2, sizeScreen.Y / 2 - (goodSize.Y * goodBannerScale) / 2);


            this.badLocation = new Vector2(sizeScreen.X / 2 - (badSize.X * badBannerScale) / 2, sizeScreen.Y / 2 - (badSize.Y * badBannerScale) / 2);


            this.missLocation = new Vector2(sizeScreen.X / 2 - (missSize.X * missBannerScale) / 2, sizeScreen.Y / 2 - (missSize.Y * missBannerScale) / 2);


            //this.goodLocation = new Vector2(sizeScreen.X / 2 - 170, sizeScreen.Y / 2 - 30);
            
          
            //this.badLocation = new Vector2(sizeScreen.X / 2 - 130, sizeScreen.Y / 2 - 30);


            //this.missLocation = new Vector2(sizeScreen.X / 2 - 100, sizeScreen.Y / 2 );

           
            
            this.comboLocation = new Vector2(sizeScreen.X / 2 , sizeScreen.Y / 2 + 40);
          //  this.goodComboLocation = new Vector2(sizeScreen.X / 2, sizeScreen.Y / 2 + 60);
        }
        #endregion

        #region method

       /// <summary>
       /// 드래그노트 와 마우스 만. (이건 마크가 없다)
       /// </summary>
       /// <param name="mousePoint"></param>
        public void checkDragNote(Vector2 mousePoint)
        {
            //드래그 노트
            //Curve 클래스에 만든 dragnote를 검사해 간다.

            for (int i = 0; i < dragNoteManager.dragNotes.Count(); i++)
            {
                //DragNoteManager 의 텍스쳐 값.
                Sprite dragNote = dragNoteManager.dragNotes[i];


                int judgment = dragNote.JudgedNoteForDragNote(mousePoint);

               

                //퍼펙트
                if (judgment == 2)
                {
                    
                    badManager.AddExplosions(new Vector2(dragNote.Center.X - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Width / 2, dragNote.Center.Y - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Height / 2));
                    //퍼펙트 글자 띄우기
                    perfectBannerManager.AddBanners(perfectLocation, perfectBannerScale);


                    dragNoteManager.dragNotes.RemoveAt(i);

                    scoreManager.DragPerfect ++;

                    scoreManager.Combo += 2;


                    scoreManager.Gage += 0.5f;

                    AddComboNumber((int)scoreManager.Combo, 0);
                }
                //good
                else if (judgment == 1)
                {

                    badManager.AddExplosions(new Vector2(dragNote.Center.X - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Width / 2, dragNote.Center.Y - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Height / 2));

                    //굿 글자 띄우기
                    goodBannerManager.AddBanners(goodLocation,goodBannerScale);



                    dragNoteManager.dragNotes.RemoveAt(i);
                //    ScoreManager.otherScore += 1;
                    scoreManager.DragGood++;

                    scoreManager.Combo+=2;

                    scoreManager.Gage += 0.2f;

                    AddComboNumber((int)scoreManager.Combo, 1);
                }
                else
                {
                  
                }
            }
        }
        //타입 perfect = 0 , good = 1
        public void AddComboNumber(int number, int type , int duration = 30)
        {
            int num = number;
            int j;
            if (num > 10)
            {
                j = 2;
                j++;


            }
            int[] eachNumbers = new int[6];
            Vector2[] eachNumberLocations = new Vector2[6];

            //자릿수
            int length = 1;

            //몇 자리 인지 센다.
            //몫
            int share;
            share = num / 10;
            eachNumbers[length - 1] = num % 10;


            while (share > 0)
            {
                length++;
                eachNumbers[length - 1] = share % 10;
                share = share / 10;
                
                
            }


            int i;

            for (i = 0; i < length; i++)
            {
                if (length == 1)
                {
                    //끝자리 부터
                    comboNumberManager.AddComboNumbers(new Vector2(comboLocation.X - 55, comboLocation.Y), eachNumbers[i], type, duration);
                }
           
                if (length == 2)
                {

                     comboNumberManager.AddComboNumbers(new Vector2(comboLocation.X - 10 -(i * 80) , comboLocation.Y), eachNumbers[i], type, duration);
              

                }

                if (length == 3)
                {
                    comboNumberManager.AddComboNumbers(new Vector2(comboLocation.X - (i * 73) +20, comboLocation.Y), eachNumbers[i], type, duration);


                }

                if (length == 4)
                {
                    comboNumberManager.AddComboNumbers(new Vector2(comboLocation.X - (i * 73) +30, comboLocation.Y), eachNumbers[i], type, duration);


                }
            }

        }

        //private void DisappearAllMarks()
        //{


        //        perfectBannerManager.DisappearAllMarks();
        //        goodBannerManager.DisappearAllMarks();
        //        badBannerManager.DisappearAllMarks();
        //        missBannerManager.DisappearAllMarks();

        //}

        ////시간이지난 배너는 삭제
        //public void DeleteMarks()
        //{


        //    perfectBannerManager.deleteAllMarks();
        //    goodBannerManager.deleteAllMarks();
        //    badBannerManager.deleteAllMarks();
        //    missBannerManager.deleteAllMarks();

        //}


        private void checkRightNoteToMarker(int number,Vector2 mousePoint)
        {
            //오른손 노트(움직이는것 반복)
            for (int x = 0; x < StartNoteManager.rightNoteManager.LittleNotes.Count;  x++)
            {
                //오른손 노트들 중 하나를 가져옴
                Sprite littleNote = StartNoteManager.rightNoteManager.LittleNotes[x];
               
                //이중에서 마커의 시작위치에서 출발한 노트만 가려낸다. 

                if (littleNote.StartNoteLoation == number)
                {
                    //0:bad 1:good 2:perfect

                    ///노트의 반지름으로 
                    //judgment = littleNote.JudgedNote(
                    //    mark.MarkSprite.Center,
                    //    mark.MarkSprite.CollisionRadius);

                    //마커의 반지름으로

                    //스파리이트 클래스에는 거리 판단하는것이 있음.
                    //이상한점. 0번 마커일지라도 모든 노트를 다 검사하고 있다. 
                    //0번 마커는 0번 스타토 노트에서 출발한 것만  검사하면 되지 않느가?
                    //0번 스타트 노트에서 출발한것은 알 수가 있는가?

                    //노트와 마커의 센터 위치 파악 하기 위한 것.
                 //   Trace.WriteLine("noteCenter:"+littleNote.Center+" markcndter:"+MarkManager.Marks[number].MarkSprite.Center);
                 //   Vector2 notecenter = littleNote.Center;
                 //   Vector2 markcenter = MarkManager.Marks[number].MarkSprite.Center;

                 
                    int judgment = MarkManager.Marks[number].MarkSprite.JudgedNote(
                        littleNote.Center,littleNote.CollisionRadius
                        );
                    //perfect
                 
                    if (judgment == 2)
                    {

                        int mouseJudgment = MarkManager.Marks[number].MarkSprite.JudgedNote(mousePoint);
                        if (mouseJudgment != 0)
                        {

                            perfectManager.AddExplosions(new Vector2(littleNote.Center.X - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Width / 2, littleNote.Center.Y - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Height / 2));
                            StartNoteManager.rightNoteManager.LittleNotes.RemoveAt(x);


                            //퍼펙트 글자 띄우기
                            //DisappearAllMarks();

                            perfectBannerManager.AddBanners(perfectLocation, perfectBannerScale);


                            scoreManager.OneHandPerfect++;

                            scoreManager.Combo ++;
                            scoreManager.Gage += 2;
                       
                            //콤보 글자 띄우기
                            //perfect라서 두번쨰 매개변수가 0
                            //good은 1
                            AddComboNumber((int)scoreManager.Combo, 0);

                            SoundFmod.slowBadCount = 0;
                            SoundFmod.fastBadCount = 0;

                            //템포 원상 복귀 //퍼펙트는 한번에 복귀 
                            if (SoundFmod.isChangedTempo != 0)
                            {
                              
                                    SoundFmod.SetOptionalTime();
                                    //움직이는 속도 정상
                                    memberManager.SetMembersFrameTime(0.1f);
                                    //멤버 스크로크 효과 없어지게함
                                    int i;
                                    for (i = 0; i < 6; i++)
                                    {
                                        memberManager.SetMemberState(i, 0);
                                    }
                                    SoundFmod.isChangedTempo = 0;
                              

                            }
                        }
                        
                        //가운데를 이미 지났으므로 느리게 맞은 노트가 될 수 있다.
                        littleNote.IsEarlyOne = false;
                      
                    }

                    //good
                    else if (judgment == 1)
                    {
                        int mouseJudgment = MarkManager.Marks[number].MarkSprite.JudgedNote(mousePoint);
                        if (mouseJudgment != 0)
                        {

                            goodManager.AddExplosions(new Vector2(littleNote.Center.X - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Width / 2, littleNote.Center.Y - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Height / 2));
                            //굿 글자 띄우기

                            //DisappearAllMarks();
                            goodBannerManager.AddBanners(goodLocation,goodBannerScale);

                            StartNoteManager.rightNoteManager.LittleNotes.RemoveAt(x);

                            scoreManager.OneHandGood++;

                            scoreManager.Combo++;
                            scoreManager.Gage += 1;

                            memberManager.SetMemberState(1, 1);




                            //콤보 글자 띄우기
                            //perfect라서 두번쨰 매개변수가 0
                            //good은 1
                            AddComboNumber((int)scoreManager.Combo, 1);


                            SoundFmod.slowBadCount = 0;
                            SoundFmod.fastBadCount = 0;

                            //템포 원상 복귀
                            if (SoundFmod.isChangedTempo != 0)
                            {
                                //1에서 0으로
                                if (SoundFmod.isChangedTempo == 1 || SoundFmod.isChangedTempo == -1)
                                {

                                    SoundFmod.SetOptionalTime();
                                    //움직이는 속도 정상
                                    memberManager.SetMembersFrameTime(0.1f);
                                    //멤버 스크로크 효과 없어지게함
                                    int i;
                                    for (i = 0; i < 6; i++)
                                    {
                                        memberManager.SetMemberState(i, 0);
                                    }
                                    SoundFmod.isChangedTempo = 0;
                                }

                                //2에서 1로
                                else if (SoundFmod.isChangedTempo == 2)
                                {

                                    int i;
                                    SoundFmod.SetOptionalTime();

                                    SoundFmod.tempoChange(1.1f);

                                    //스트로크

                                    for (i = 0; i < 6; i++)
                                    {

                                        memberManager.SetMemberState(i, 1);

                                    }
                                    //움직이는 속도 빨라짐
                                    memberManager.SetMembersFrameTime(0.07f);

                                    SoundFmod.isChangedTempo = 1;
                                }


                                //3에서 2로
                                else if (SoundFmod.isChangedTempo == 3)
                                {

                                    int i;
                                    SoundFmod.SetOptionalTime();

                                    SoundFmod.tempoChange(1.2f);

                                    //스트로크

                                    for (i = 0; i < 6; i++)
                                    {

                                        memberManager.SetMemberState(i, 2);

                                    }
                                    //움직이는 속도 빨라짐
                                    memberManager.SetMembersFrameTime(0.04f);

                                    SoundFmod.isChangedTempo = 2;
                                }



                                //-2에서 -1로
                                else if (SoundFmod.isChangedTempo == -2)
                                {

                                    int i;
                                    SoundFmod.SetOptionalTime();

                                    SoundFmod.tempoChange(0.9f);

                                    //스트로크

                                    for (i = 0; i < 6; i++)
                                    {

                                        memberManager.SetMemberState(i, 1);

                                    }
                                    //움직이는 속도 빨라짐
                                    memberManager.SetMembersFrameTime(0.13f);

                                    SoundFmod.isChangedTempo = -1;
                                }

                                             //-3에서 -2로
                                else if (SoundFmod.isChangedTempo == -3)
                                {

                                    int i;
                                    SoundFmod.SetOptionalTime();

                                    SoundFmod.tempoChange(0.8f);

                                    //스트로크

                                    for (i = 0; i < 6; i++)
                                    {

                                        memberManager.SetMemberState(i, 2);

                                    }
                                    //움직이는 속도 빨라짐
                                    memberManager.SetMembersFrameTime(0.16f);

                                    SoundFmod.isChangedTempo = -2;
                                }

                            }
                        }
                    }
                        //bad 
                    else if (judgment == -1)
                    {

                   

                       //빨리 맞은거 
                        if (littleNote.IsEarlyOne)
                        {
                            int mouseJudgment = MarkManager.Marks[number].MarkSprite.JudgedNote(mousePoint);
                            if (mouseJudgment != 0)
                            {

                                

                                if (scoreManager.Combo > scoreManager.Max)
                                {
                                    scoreManager.Max = scoreManager.Combo;
                                }
                                scoreManager.Combo = 0;



                                //효과와 내용
                                //이펙트 및 템포 빨라지기
                             //   badManager.AddExplosions(new Vector2(littleNote.Center.X - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Width / 2, littleNote.Center.Y - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Height / 2));

                                //배드 글자 띄우기
                                badBannerManager.AddBanners(badLocation,badBannerScale);


                                StartNoteManager.rightNoteManager.LittleNotes.RemoveAt(x);
                                int i;

                                //false

                                //느린상태 일 때
                                if (SoundFmod.isChangedTempo <= 0)
                                {
                                    SoundFmod.fastBadCount++;
                                    //빠른 배드는 초기화
                                    SoundFmod.slowBadCount = 0;

                                    scoreManager.Gage -= 5;

                                    if (SoundFmod.fastBadCount >= 2)
                                    {
                                        if (SoundFmod.isChangedTempo != 0)
                                        {
                                            SoundFmod.SetOptionalTime();

                                        }

                                        SoundFmod.tempoChange(1.1f);

                                        //스트로크

                                        for (i = 0; i < 6; i++)
                                        {

                                            memberManager.SetMemberState(i, 1);

                                        }
                                        //움직이는 속도 빨라짐
                                        memberManager.SetMembersFrameTime(0.07f);
                                        SoundFmod.fastBadCount = 0;
                                    }
                                 }
                                else if (SoundFmod.isChangedTempo == 1)
                                {
                                    SoundFmod.fastBadCount++;
                                    SoundFmod.slowBadCount = 0;
                                    scoreManager.Gage -= 15;


                                    if (SoundFmod.fastBadCount >= 2)
                                    {
                           
                                        SoundFmod.SetOptionalTime();


                                        SoundFmod.tempoChange(1.2f);
                                        //스트로크
                                        //memberManager.SetMemberState(4, 2);

                                        for (i = 0; i < 6; i++)
                                        {

                                            memberManager.SetMemberState(i, 2);

                                        }
                                        //움직이는 속도 빨라짐
                                        memberManager.SetMembersFrameTime(0.04f);
                                        SoundFmod.fastBadCount = 0;
                                    }

                                }
                                else if (SoundFmod.isChangedTempo == 2)
                                {
                                    SoundFmod.fastBadCount++;
                                    SoundFmod.slowBadCount = 0;
                                    scoreManager.Gage -= 30;

                                    if (SoundFmod.fastBadCount >= 2)
                                    {
                                 
                                        SoundFmod.SetOptionalTime();

                                        SoundFmod.tempoChange(1.3f);
                                        //스트로크
                                        //  memberManager.SetMemberState(4, 3);

                                        for (i = 0; i < 6; i++)
                                        {

                                            memberManager.SetMemberState(i, 3);

                                        }

                                        //움직이는 속도 빨라짐
                                        memberManager.SetMembersFrameTime(0.01f);
                                        SoundFmod.fastBadCount = 0;
                                    }
                                }
                            }
                        }
                        //느리게 맞은거
                        else
                        {
                            int mouseJudgment = MarkManager.Marks[number].MarkSprite.JudgedNote(mousePoint);
                            if (mouseJudgment != 0)
                            {

                                if (scoreManager.Combo > scoreManager.Max)
                                {
                                    scoreManager.Max = scoreManager.Combo;
                                }
                                scoreManager.Combo = 0;

                                //badManager.AddExplosions(new Vector2(littleNote.Center.X - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Width / 2, littleNote.Center.Y - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Height / 2));

                                //배드 글자 띄우기
                                badBannerManager.AddBanners(badLocation, badBannerScale);

                                StartNoteManager.rightNoteManager.LittleNotes.RemoveAt(x);
                                int i;

                                //효과와 내용
                                //이펙트 및 템포 느려지기
                                //빠른 상태일 때
                                if (SoundFmod.isChangedTempo >= 0)
                                {
                                     SoundFmod.fastBadCount = 0;
                                     SoundFmod.slowBadCount++;
                                     scoreManager.Gage -= 5;
                                    if (SoundFmod.slowBadCount >= 2)
                                    {
                                         //빨라졌다 갑자기 느려졌을 때
                                        if (SoundFmod.isChangedTempo != 0)
                                        {
                                            SoundFmod.SetOptionalTime();

                                        }
                                        //그 양만큼 템포 조절됨
                                        SoundFmod.tempoChange(0.9f);

                                        //멤버들 느려짐
                                        memberManager.SetMembersFrameTime(0.13f);

                                        //스트로크
                                        for (i = 0; i < 6; i++)
                                        {
                                            memberManager.SetMemberState(i, 1);
                                        }
                                        SoundFmod.slowBadCount = 0;
                                    }
                                }

                                else if (SoundFmod.isChangedTempo == -1)
                                {
                                    SoundFmod.fastBadCount = 0;
                                    SoundFmod.slowBadCount++;
                                    scoreManager.Gage -= 15;
                                    if (SoundFmod.slowBadCount >= 2)
                                    {
                                       
                                        SoundFmod.SetOptionalTime();

                                        //그 양만큼 템포 조절됨
                                        SoundFmod.tempoChange(0.8f);

                                        //멤버들 느려짐
                                        memberManager.SetMembersFrameTime(0.16f);

                                        //스트로크
                                        //memberManager.SetMemberState(4, 2);
                                        for (i = 0; i < 6; i++)
                                        {

                                            memberManager.SetMemberState(i, 2);

                                        }
                                        SoundFmod.slowBadCount = 0;
                                    }

                                }

                                else if (SoundFmod.isChangedTempo == -2)
                                {
                                    SoundFmod.fastBadCount = 0;
                                    SoundFmod.slowBadCount++;
                                    scoreManager.Gage -= 30;
                                    if (SoundFmod.slowBadCount >= 2)
                                    {
                                        
                                        SoundFmod.SetOptionalTime();

                                        //그 양만큼 템포 조절됨
                                        SoundFmod.tempoChange(0.7f);

                                        //멤버들 느려짐
                                        memberManager.SetMembersFrameTime(0.19f);

                                        //스트로크
                                        //memberManager.SetMemberState(4, 3);
                                        for (i = 0; i < 6; i++)
                                        {

                                            memberManager.SetMemberState(i, 3);

                                        }
                                        SoundFmod.slowBadCount = 0;
                                    }

                                }
                            }
                         


                        }
                    }
                    else
                    {

                    }
                }
            }
        }
       


        private void checkLeftNoteToMarker(int number, Vector2 mousePoint)
        {
            int x;
            for (x = 0; x < StartNoteManager.leftNoteManager.LittleNotes.Count; x++)
            {
                Sprite littleNote = StartNoteManager.leftNoteManager.LittleNotes[x];
                if (littleNote.StartNoteLoation == number)
                {
                    //0:bad 1:good 2:perfect

                    ///노트의 반지름으로 
                    //judgment = littleNote.JudgedNote(
                    //    mark.MarkSprite.Center,
                    //    mark.MarkSprite.CollisionRadius);

                    //마커의 반지름으로
                    //int judgment = MarkManager.Marks[number].MarkSprite.JudgedNote(
                    //    littleNote.Center
                    //    );


                    //int judgment = MarkManager.Marks[number].MarkSprite.JudgedNote(
                    //   littleNote.Center, (littleNote.Texture.Width * littleNote.Scale) / 4
                    //   );
                    int judgment = MarkManager.Marks[number].MarkSprite.JudgedNote(
                       littleNote.Center, littleNote.CollisionRadius
                       );
                    //perfect
                    if (judgment == 2)
                    {
                        int mouseJudgment = MarkManager.Marks[number].MarkSprite.JudgedNote(mousePoint);
                        if (mouseJudgment != 0)
                        {

                            perfectManager.AddExplosions(new Vector2(littleNote.Center.X - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Width / 2, littleNote.Center.Y - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Height / 2));

                            // 글자 띄우기
                            perfectBannerManager.AddBanners(perfectLocation, perfectBannerScale);

                            StartNoteManager.leftNoteManager.LittleNotes.RemoveAt(x);

                            scoreManager.OneHandPerfect++;
                            scoreManager.Combo++;
                            scoreManager.Gage += 2;

                            AddComboNumber((int)scoreManager.Combo, 0);



                            SoundFmod.slowBadCount = 0;
                            SoundFmod.fastBadCount = 0;

                            //템포 원상 복귀 //퍼펙트는 한번에 복귀 
                            if (SoundFmod.isChangedTempo != 0)
                            {

                                SoundFmod.SetOptionalTime();
                                //움직이는 속도 정상
                                memberManager.SetMembersFrameTime(0.1f);
                                //멤버 스크로크 효과 없어지게함
                                int i;
                                for (i = 0; i < 6; i++)
                                {
                                    memberManager.SetMemberState(i, 0);
                                }
                                SoundFmod.isChangedTempo = 0;


                            }

                        }

                        //가운데를 이미 지났으므로 느리게 맞은 노트가 될 수 있다.
                        littleNote.IsEarlyOne = false;
                      
                    }

                    //good
                    else if (judgment == 1)
                    {
                        int mouseJudgment = MarkManager.Marks[number].MarkSprite.JudgedNote(mousePoint);
                        if (mouseJudgment != 0)
                        {
                            goodManager.AddExplosions(new Vector2(littleNote.Center.X - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Width / 2, littleNote.Center.Y - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Height / 2));

                            // 글자 띄우기
                            goodBannerManager.AddBanners(goodLocation,goodBannerScale);

                            StartNoteManager.leftNoteManager.LittleNotes.RemoveAt(x);

                            scoreManager.OneHandGood++;
                            scoreManager.Combo++;
                            scoreManager.Gage += 1;
                            SoundFmod.slowBadCount = 0;
                            SoundFmod.fastBadCount = 0;


                            AddComboNumber((int)scoreManager.Combo, 1);


                            //템포 원상 복귀
                            if (SoundFmod.isChangedTempo != 0)
                            {
                                //1에서 0으로
                                if (SoundFmod.isChangedTempo == 1 || SoundFmod.isChangedTempo == -1)
                                {

                                    SoundFmod.SetOptionalTime();
                                    //움직이는 속도 정상
                                    memberManager.SetMembersFrameTime(0.1f);
                                    //멤버 스크로크 효과 없어지게함
                                    int i;
                                    for (i = 0; i < 6; i++)
                                    {
                                        memberManager.SetMemberState(i, 0);
                                    }
                                    SoundFmod.isChangedTempo = 0;
                                }

                                //2에서 1로
                                else if (SoundFmod.isChangedTempo == 2)
                                {

                                    int i;
                                    SoundFmod.SetOptionalTime();

                                    SoundFmod.tempoChange(1.1f);

                                    //스트로크

                                    for (i = 0; i < 6; i++)
                                    {

                                        memberManager.SetMemberState(i, 1);

                                    }
                                    //움직이는 속도 빨라짐
                                    memberManager.SetMembersFrameTime(0.07f);

                                    SoundFmod.isChangedTempo = 1;
                                }


                                //3에서 2로
                                else if (SoundFmod.isChangedTempo == 3)
                                {

                                    int i;
                                    SoundFmod.SetOptionalTime();

                                    SoundFmod.tempoChange(1.2f);

                                    //스트로크

                                    for (i = 0; i < 6; i++)
                                    {

                                        memberManager.SetMemberState(i, 2);

                                    }
                                    //움직이는 속도 빨라짐
                                    memberManager.SetMembersFrameTime(0.04f);

                                    SoundFmod.isChangedTempo = 2;
                                }



                                //-2에서 -1로
                                else if (SoundFmod.isChangedTempo == -2)
                                {

                                    int i;
                                    SoundFmod.SetOptionalTime();

                                    SoundFmod.tempoChange(0.9f);

                                    //스트로크

                                    for (i = 0; i < 6; i++)
                                    {

                                        memberManager.SetMemberState(i, 1);

                                    }
                                    //움직이는 속도 빨라짐
                                    memberManager.SetMembersFrameTime(0.13f);

                                    SoundFmod.isChangedTempo = -1;
                                }

                                             //-3에서 -2로
                                else if (SoundFmod.isChangedTempo == -3)
                                {

                                    int i;
                                    SoundFmod.SetOptionalTime();

                                    SoundFmod.tempoChange(0.8f);

                                    //스트로크

                                    for (i = 0; i < 6; i++)
                                    {

                                        memberManager.SetMemberState(i, 2);

                                    }
                                    //움직이는 속도 빨라짐
                                    memberManager.SetMembersFrameTime(0.16f);

                                    SoundFmod.isChangedTempo = -2;
                                }

                            }

                        }
                    }

                                                //bad 
                    else if (judgment == -1)
                    {

                  
                       //빨리 맞은거 
                        if (littleNote.IsEarlyOne)
                        {

                            int mouseJudgment = MarkManager.Marks[number].MarkSprite.JudgedNote(mousePoint);
                            if (mouseJudgment != 0)
                            {

                                if (scoreManager.Combo > scoreManager.Max)
                                {
                                    scoreManager.Max = scoreManager.Combo;
                                }
                                scoreManager.Combo = 0;


                                
                                //효과와 내용
                                //이펙트 및 템포 빨라지기
                                //badManager.AddExplosions(new Vector2(littleNote.Center.X - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Width / 2, littleNote.Center.Y - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Height / 2));
                                //배드 글자 띄우기
                                badBannerManager.AddBanners(badLocation, badBannerScale);



                                StartNoteManager.leftNoteManager.LittleNotes.RemoveAt(x);
                                int i;
                                //false
                                //느린 상태 일 때나 0일때 
                                if (SoundFmod.isChangedTempo <= 0)
                                {
                                    SoundFmod.fastBadCount++;
                                    //빠른 배드는 초기화
                                    SoundFmod.slowBadCount = 0;

                                    scoreManager.Gage -= 5;

                                    if (SoundFmod.fastBadCount >= 2)
                                    {
                                        if (SoundFmod.isChangedTempo != 0)
                                        {
                                            SoundFmod.SetOptionalTime();

                                        }

                                        SoundFmod.tempoChange(1.1f);

                                        //스트로크

                                        for (i = 0; i < 6; i++)
                                        {

                                            memberManager.SetMemberState(i, 1);

                                        }
                                        //움직이는 속도 빨라짐
                                        memberManager.SetMembersFrameTime(0.07f);
                                        SoundFmod.fastBadCount = 0;
                                    }
                                }
                                else if (SoundFmod.isChangedTempo == 1)
                                {
                                   

                                    SoundFmod.fastBadCount++;
                                    SoundFmod.slowBadCount = 0;
                                    scoreManager.Gage -= 15;
                                    if (SoundFmod.fastBadCount >= 2)
                                    {
                                    

                                        SoundFmod.SetOptionalTime();


                                        SoundFmod.tempoChange(1.2f);
                                        //스트로크
                                        //memberManager.SetMemberState(4, 2);

                                        for (i = 0; i < 6; i++)
                                        {

                                            memberManager.SetMemberState(i, 2);

                                        }
                                        //움직이는 속도 빨라짐
                                        memberManager.SetMembersFrameTime(0.04f);
                                        SoundFmod.fastBadCount = 0;
                                    }

                                }
                                else if (SoundFmod.isChangedTempo == 2)
                                {

                                    SoundFmod.fastBadCount++;
                                    SoundFmod.slowBadCount = 0;
                                    scoreManager.Gage -= 30;
                                    if (SoundFmod.fastBadCount >= 2)
                                    {
                                        

                                        SoundFmod.SetOptionalTime();

                                        SoundFmod.tempoChange(1.3f);
                                        //스트로크
                                        //  memberManager.SetMemberState(4, 3);

                                        for (i = 0; i < 6; i++)
                                        {

                                            memberManager.SetMemberState(i, 3);

                                        }

                                        //움직이는 속도 빨라짐
                                        memberManager.SetMembersFrameTime(0.01f);
                                        SoundFmod.fastBadCount = 0;
                                    }
                                }
                            }
                        }
                        //느리게 맞은거
                        else
                        {
                            int mouseJudgment = MarkManager.Marks[number].MarkSprite.JudgedNote(mousePoint);
                            if (mouseJudgment != 0)
                            {

                                if (scoreManager.Combo > scoreManager.Max)
                                {
                                    scoreManager.Max = scoreManager.Combo;
                                }
                                scoreManager.Combo = 0;


                                //badManager.AddExplosions(new Vector2(littleNote.Center.X - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Width / 2, littleNote.Center.Y - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Height / 2));

                                //배드 글자 띄우기
                                badBannerManager.AddBanners(badLocation, badBannerScale);

                                StartNoteManager.leftNoteManager.LittleNotes.RemoveAt(x);
                                int i;

                                //효과와 내용
                                //이펙트 및 템포 느려지기
                                //빠른 상태일 때
                                if (SoundFmod.isChangedTempo >= 0)
                                {
                                    SoundFmod.fastBadCount = 0;
                                    SoundFmod.slowBadCount++;
                                 
                                    scoreManager.Gage -= 5;

                                    if (SoundFmod.slowBadCount >= 2)
                                    {
                                        
                                        //빨라졌다 갑자기 느려졌을 때
                                        if (SoundFmod.isChangedTempo != 0)
                                        {
                                            SoundFmod.SetOptionalTime();

                                        }
                                        //그 양만큼 템포 조절됨
                                        SoundFmod.tempoChange(0.9f);

                                        //멤버들 느려짐
                                        memberManager.SetMembersFrameTime(0.13f);

                                        //스트로크
                                        for (i = 0; i < 6; i++)
                                        {
                                            memberManager.SetMemberState(i, 1);
                                        }
                                        SoundFmod.slowBadCount = 0;
                                    }
                                }

                                else if (SoundFmod.isChangedTempo == -1)
                                {
                                    SoundFmod.fastBadCount = 0;
                                    SoundFmod.slowBadCount++;

                                    scoreManager.Gage -= 15;

                                    if (SoundFmod.slowBadCount >= 2)
                                    {
                                        //추가적으로
                                        scoreManager.Gage -= 10;

                                        SoundFmod.SetOptionalTime();

                                        //그 양만큼 템포 조절됨
                                        SoundFmod.tempoChange(0.8f);

                                        //멤버들 느려짐
                                        memberManager.SetMembersFrameTime(0.16f);

                                        //스트로크
                                        //memberManager.SetMemberState(4, 2);
                                        for (i = 0; i < 6; i++)
                                        {

                                            memberManager.SetMemberState(i, 2);

                                        }
                                        SoundFmod.slowBadCount = 0;
                                    }

                                }

                                else if (SoundFmod.isChangedTempo == -2)
                                {
                                    SoundFmod.fastBadCount = 0;
                                    SoundFmod.slowBadCount++;

                                    scoreManager.Gage -= 30;

                                    if (SoundFmod.slowBadCount >= 2)
                                    {
                                        SoundFmod.SetOptionalTime();

                                        //그 양만큼 템포 조절됨
                                        SoundFmod.tempoChange(0.7f);

                                        //멤버들 느려짐
                                        memberManager.SetMembersFrameTime(0.19f);

                                        //스트로크
                                        //memberManager.SetMemberState(4, 3);
                                        for (i = 0; i < 6; i++)
                                        {

                                            memberManager.SetMemberState(i, 3);

                                        }
                                        SoundFmod.slowBadCount = 0;
                                    }

                                }
                            }
                        }
                       
                        ////빨리 맞은거 
                        //if (littleNote.IsEarlyOne)
                        //{
                        //    int mouseJudgment = MarkManager.Marks[number].MarkSprite.JudgedNote(mousePoint);
                        //    if (mouseJudgment != 0)
                        //    {
                        //        //효과와 내용
                        //        //이펙트 및 템포 빨라지기

                        //    }
                        //}
                        ////느리게 맞은거
                        //else
                        //{
                        //    int mouseJudgment = MarkManager.Marks[number].MarkSprite.JudgedNote(mousePoint);
                        //    if (mouseJudgment != 0)
                        //    {

                        //        //효과와 내용
                        //        //이펙트 및 템포 느려지기

                        //    }



                        //}
                    }
                    else
                    {
                    }
                }
            
            }
        }

        public void checkLongNoteToMarker(int number, Vector2 mousePoint)
        {

            for (int x = 0; x < StartNoteManager.longNoteManager.LittleNotes.Count; x++)
            {
                Sprite littleNote = StartNoteManager.longNoteManager.LittleNotes[x];
                if (littleNote.StartNoteLoation == number)
                {
                    //0:bad 1:good 2:perfect
                    ///노트의 반지름으로 
                    //judgment = littleNote.JudgedNote(
                    //    mark.MarkSprite.Center,
                    //    mark.MarkSprite.CollisionRadius);
                    //마커의 반지름으로
                    int judgment = MarkManager.Marks[number].MarkSprite.JudgedNote(
                        littleNote.Center
                        );
                    //perfect
                    if (judgment == 2 || judgment == 1)
                    {
                        int mouseJudgment = MarkManager.Marks[number].MarkSprite.JudgedNote(mousePoint);
                        if (mouseJudgment != 0)
                        {
                     //       perfectManager.AddExplosions(new Vector2(littleNote.Center.X - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Width / 2, littleNote.Center.Y - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Height / 2));
                            // 글자 띄우기
                            //DisappearAllMarks();
                            perfectBannerManager.AddBanners(perfectLocation, perfectBannerScale);

                            
                            StartNoteManager.longNoteManager.LittleNotes.RemoveAt(x);
                            
                            //effect -> bad가 long노트의 이펙트.

                            badManager.AddExplosions(new Vector2(littleNote.Center.X - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Width / 2, littleNote.Center.Y - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Height / 2));

                            scoreManager.LongPerfect++;
                            scoreManager.Combo+=0.1f;

                            AddComboNumber((int)scoreManager.Combo, 0, 5);

                            scoreManager.Gage += 0.1f;

                            SoundFmod.slowBadCount = 0;
                            SoundFmod.fastBadCount = 0;

                            //템포 원상 복귀 //퍼펙트는 한번에 복귀 
                            if (SoundFmod.isChangedTempo != 0)
                            {

                                SoundFmod.SetOptionalTime();
                                //움직이는 속도 정상
                                memberManager.SetMembersFrameTime(0.1f);
                                //멤버 스크로크 효과 없어지게함
                                int i;
                                for (i = 0; i < 6; i++)
                                {
                                    memberManager.SetMemberState(i, 0);
                                }
                                SoundFmod.isChangedTempo = 0;


                            }
                        }
                    }
                    ////good
                    //else if (judgment == 1)
                    //{
                    //    int mouseJudgment = MarkManager.Marks[number].MarkSprite.JudgedNote(mousePoint);
                    //    if (mouseJudgment != 0)
                    //    {
                    //        //롱노트 효과를 바꾸던지 아니면 하나의 효과만 나오게 하던지
                    //  //      goodManager.AddExplosions(new Vector2(littleNote.Center.X - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Width / 2, littleNote.Center.Y - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Height / 2));

                    //        // 글자 띄우기
                    //        //missBannerManager.
                    //        //DisappearAllMarks();
                    //        perfectBannerManager.AddBanners(perfectLocation, perfectBannerScale);


                    //        badManager.AddExplosions(new Vector2(littleNote.Center.X - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Width / 2, littleNote.Center.Y - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Height / 2));
                

                    //        StartNoteManager.longNoteManager.LittleNotes.RemoveAt(x);


                    //        scoreManager.LongNoteScore = scoreManager.LongNoteScore + 1;
                    //        scoreManager.Combo = scoreManager.Combo + 1;
                    //        scoreManager.Gage += 1;

                    //        AddComboNumber(scoreManager.Combo, 1 ,5);

                    //        SoundFmod.slowBadCount = 0;
                    //        SoundFmod.fastBadCount = 0;

                    //        //템포 원상 복귀 //퍼펙트는 한번에 복귀 
                    //        if (SoundFmod.isChangedTempo != 0)
                    //        {

                    //            SoundFmod.SetOptionalTime();
                    //            //움직이는 속도 정상
                    //            memberManager.SetMembersFrameTime(0.1f);
                    //            //멤버 스크로크 효과 없어지게함
                    //            int i;
                    //            for (i = 0; i < 6; i++)
                    //            {
                    //                memberManager.SetMemberState(i, 0);
                    //            }
                    //            SoundFmod.isChangedTempo = 0;


                    //        }
                    //    }
                    //}
                    else
                    {

                    }
                }
            }
        }

        private void checkGold(Vector2 mousePoint)
        {
            int i;
            for (i = 0; i < GoldManager.Golds.Count; i++)
            { 
                Sprite gold = GoldManager.Golds[i];

                int judgment = gold.JudgedNote(mousePoint);

                if (judgment != 0)
                {
                    goldGetManager.AddExplosions(new Vector2(gold.Center.X - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Width / 2, gold.Center.Y - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Height / 2));
                    GoldManager.Golds.RemoveAt(i);
                  
                    scoreManager.Gold = scoreManager.Gold + 1;
                }
            }
        }
        public void CheckMouseCollisions(int number,Vector2 mousePoint)
        {
            checkRightNoteToMarker(number, mousePoint);
            checkLeftNoteToMarker(number, mousePoint);
            checkLongNoteToMarker(number, mousePoint);
            checkGold(mousePoint);
        }
        public void CheckRightHandCollisions(int number, Vector2 mousePoint)
        {
            checkRightNoteToMarker(number, mousePoint);
            checkLongNoteToMarker(number, mousePoint);
            checkGold(mousePoint);
        }
        public void CheckLeftHandCollisions(int number, Vector2 mousePoint)
        {
            checkLeftNoteToMarker(number, mousePoint);
            checkLongNoteToMarker(number, mousePoint);
          //  checkGold(mousePoint);
        }

        /// <summary>
        /// 오른 손 노트 사각형 범위 들어가면 삭제 , 반복문 돌 필요가 없는지 다시 검토
        /// </summary>
        public void CheckRightNoteInCenterArea()
        {
            int i;
            for (i = 0; i < StartNoteManager.rightNoteManager.LittleNotes.Count; i++)
            {
                Sprite littleNote = StartNoteManager.rightNoteManager.LittleNotes[i];


                if (littleNote.IsBoxColliding(MarkManager.centerArea))
                {


//                    badManager.AddExplosions(new Vector2(littleNote.Center.X - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Width / 2, littleNote.Center.Y - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Height / 2));
                    
                    //글자 띄우기
                    missBannerManager.AddBanners(missLocation,missBannerScale);


                    StartNoteManager.rightNoteManager.LittleNotes.RemoveAt(i);
                    scoreManager.OneHandMiss++;
                    if (scoreManager.Combo > scoreManager.Max)
                    {
                        scoreManager.Max = scoreManager.Combo;
                    }

                        
                    scoreManager.Combo = 0;

                    if (!charismaManager.PlayCharisma)
                    {
                        scoreManager.Gage -= 10;
                        if (SoundFmod.isChangedTempo == -1)
                        {
                            scoreManager.Gage -= 10;
                        }
                        else if (SoundFmod.isChangedTempo == -2)
                        {
                            scoreManager.Gage -= 20;
                        }
                        else if (SoundFmod.isChangedTempo == 1)
                        {
                            scoreManager.Gage -= 10;
                        }
                        else if (SoundFmod.isChangedTempo == 2)
                        {
                            scoreManager.Gage -= 20;
                        }
                    }
                }

            }


        }

        public void CheckLeftNoteInCenterArea()
        {
            int i;
            for (i = 0; i < StartNoteManager.leftNoteManager.LittleNotes.Count; i++)
            {
                Sprite littleNote = StartNoteManager.leftNoteManager.LittleNotes[i];


                if (littleNote.IsBoxColliding(MarkManager.centerArea))
                {
                  //  badManager.AddExplosions(new Vector2(littleNote.Center.X - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Width / 2, littleNote.Center.Y - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Height / 2));
                    
                    //글자 띄우기
                    missBannerManager.AddBanners(missLocation, missBannerScale);

                    StartNoteManager.leftNoteManager.LittleNotes.RemoveAt(i);
                    scoreManager.OneHandMiss++;
                    if (scoreManager.Combo > scoreManager.Max)
                    {
                        scoreManager.Max = scoreManager.Combo;
                    }
                    scoreManager.Combo = 0;
                    if (!charismaManager.PlayCharisma)
                    {
                        scoreManager.Gage -= 10;
                        if (SoundFmod.isChangedTempo == -1)
                        {
                            scoreManager.Gage -= 10;
                        }
                        else if (SoundFmod.isChangedTempo == -2)
                        {
                            scoreManager.Gage -= 20;
                        }
                        else if (SoundFmod.isChangedTempo == 1)
                        {
                            scoreManager.Gage -= 10;
                        }
                        else if (SoundFmod.isChangedTempo == 2)
                        {
                            scoreManager.Gage -= 20;
                        }



                    }

                }
            }
        }

         public void CheckLongNoteInCenterArea()
        {
          //롱노트 사각형을 만나면 사라지게끔
                  
              int i;


              for (i = 0; i < StartNoteManager.longNoteManager.LittleNotes.Count; i++)
              {
                  Sprite littleNote = StartNoteManager.longNoteManager.LittleNotes[i];

                  if (littleNote.IsBoxColliding(MarkManager.centerArea))
                  {
                      
                    //  badManager.AddExplosions(new Vector2(littleNote.Center.X - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Width / 2, littleNote.Center.Y - itemManager.GetEffectInitFrame()[itemManager.getEffectIndex()].Height / 2));
                      //글자 띄우기
                      //DisappearAllMarks();
                      missBannerManager.AddBanners(missLocation, missBannerScale);
                      
                      StartNoteManager.longNoteManager.LittleNotes.RemoveAt(i);
                      scoreManager.LongMiss++;
                      if (scoreManager.Combo > scoreManager.Max)
                      {
                          scoreManager.Max = scoreManager.Combo;
                      }

                      scoreManager.Combo = 0;

                      //에러
                      if (!charismaManager.PlayCharisma)
                      {
                          scoreManager.Gage -= 0.3;
                          //if (SoundFmod.isChangedTempo == -1)
                          //{
                          //    scoreManager.Gage -= 10;
                          //}
                          //else if (SoundFmod.isChangedTempo == -2)
                          //{
                          //    scoreManager.Gage -= 20;
                          //}
                          //else if (SoundFmod.isChangedTempo == 1)
                          //{
                          //    scoreManager.Gage -= 10;
                          //}
                          //else if (SoundFmod.isChangedTempo == 2)
                          //{
                          //    scoreManager.Gage -= 20;
                          //}


                      }
                  }
              }
        }





        #endregion

    }
}
