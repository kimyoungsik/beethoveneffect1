using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FMOD;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
//using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using System.Diagnostics;
namespace beethoven3
{
    static class SoundFmod
    {

        #region declarations

        /////FMOD 선언 - START
        public static FMOD.System sndSystem;
        public static FMOD.Channel sndChannel = new FMOD.Channel();
        public static FMOD.Sound sndSound = new FMOD.Sound();
        public static FMOD.RESULT resultFmod;
        // private FMOD.DSPConnection dspconnectiontemp = null;
        /////FMOD 선언 - END



        /////템포 관련 -START
        //템포 변경 여부 -TRUE면 템포 변경된 상태
        public static int isChangedTempo = 0;

        //변경된 템포
        public static double changedTempo = 0;

        //기존 변경되기전 템포
        public static float basicFrequency = 0;

        //템포 변하고 나서 변하는 시간
        public static double chagneLimitedTime = 0;

        //템포 변경되면 다른 뒤의 모든 노트들에게 영향이 가는 시간의 량, 더하거나 빼준다.
        public static double optionalTime = 0;

        //템포관련해서 한번만 실행 -- 임시로 넣은것
      //  public static bool oneTime = true;
        /////템포 관련 -END

        private static File file;
        #endregion

         #region initialization


        public static void initialize(File fileManager)
        {
            /* 음원을 로드시킬 때 createStream 과 createSound 두가지가 있는 것을 확인할 수 있는데
    createStream은 배경음악을, createSound는 효과음을 넣는것이 좋습니다.*/
            file = fileManager;

            //FMOD 세팅 -START
            resultFmod = FMOD.Factory.System_Create(ref sndSystem);
            sndSystem.init(1, FMOD.INITFLAGS.NORMAL, (IntPtr)null);
            //FMOD 세팅 -END
        }

        #endregion
 
        //템포변경
        public static void tempoChange(double changedT)
        {
            if (changedT == 1.1f)
            {
                isChangedTempo = 1;
            }
            else if (changedT == 1.2f)
            {
                isChangedTempo = 2;
            }
            else if (changedT == 1.3f)
            {
                isChangedTempo = 3;
            }
            else if (changedT == 0.9f)
            {
                isChangedTempo = -1;
            }
            else if (changedT == 0.8f)
            {
                isChangedTempo = -2;
            }
            else if (changedT == 0.7f)
            {
                isChangedTempo = -3;
            }
            else
            {

            }




            //바뀐 템포 => 나중에 file note들을 다시 재정비 할 때 필요
            changedTempo = changedT;

            float frequency = 0;

            //현재 템포 가져와소 float frequency에 넣기//resultFmod 는 성공여부만 나타남
            resultFmod = sndChannel.getFrequency(ref frequency);

            //템포설정
            sndChannel.setFrequency(frequency * (float)changedT);

            //템포를 다른 노트 모두에 적용
            file.ChangeArrayNoteTempo(changedT);

            //***다시확인
            //현재 설정된 두번째 가이드라인이 있으면 지움
            //GuideLineManager.DeleteAllSecondGuideLine();


        }

        //public static float GetTempo()
        //{

        //    float tempo = 0;
        //    sndChannel.getFrequency(ref tempo);
        //    return tempo;

        //}
        public static void PlaySound(String name)
        {
            //노래찾아서 재생하기    
            //*** 재생시간동안 로딩

            sndSystem.createSound(name, FMOD.MODE.HARDWARE, ref SoundFmod.sndSound);
            sndSystem.playSound(CHANNELINDEX.FREE, SoundFmod.sndSound, false, ref SoundFmod.sndChannel);

           // return 2;
        }
        public static void StopSound()
        {
            sndSystem.close();

            //다시 셋팅
            resultFmod = FMOD.Factory.System_Create(ref sndSystem);
            sndSystem.init(1, FMOD.INITFLAGS    .NORMAL, (IntPtr)null);
        }


        //템포 변경 전에 기본 템포를 저장해둠. 다시 롤백할 때 필요
        public static void SetBasicTempo()
        {
            //템포가 안변하면
            if (isChangedTempo == 0)
            {
                sndChannel.getFrequency(ref basicFrequency);
            }

           
        }

        //이전에 설정한 기본 템포로 돌아감 
        public static void ReturnBasicTempo()
        {
            if (basicFrequency != 0)
            {
                sndChannel.setFrequency(basicFrequency);

                //변하지 않음
                isChangedTempo = 0;
                changedTempo = 0;
            }
            //다시 원래 대로 돌아왔으면

            //이게 바뀐 템포인가에 대해서 바뀌지 않은 템포이다.
            isChangedTempo = 0;

            //현재 템포지정 
            changedTempo = 0;
        }


        public static float GetVolume()
        {
            float volume = 0;

            sndChannel.getVolume(ref volume);

            return volume;

        }


        public static void SetVolume(float volume)
        {
            if( volume <0)
            {

                volume = 0;
            }
            else if( volume > 1)
            {

                volume = 1;
            }
            sndChannel.setVolume(volume);
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
        public static void StartChangedTime(GameTime gameTime)
        {
            //템포가 바뀌는 중인가?
            //초기화안해도 되는건가? -> SetOptionalTime 여기에서 초기화 한다. 
            if (isChangedTempo != 0)
            {
                //처음시작 
                chagneLimitedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
                //Trace.WriteLine(chagneLimitedTime);
            }
        }

        
        //다시 원점으로 돌아갈 때 쓰임 

        //늘어나거나 줄어드는 양을 계산해주고 

        public static void SetOptionalTime()
        {
            ////임시로 넣은 것일 뿐
            //if (oneTime)
            //{
                //템포 다시 원상복귀
                //현재는 시작시간만, 지속시간(끝시간도 바꿀예정)

                file.ChangeArrayNoteTempoBack(changedTempo);

                double time = 0;

                //옵션 계산
                //chagneLimitedTime : 템포가 변하고 나서 얼마나 시간이 흘렀나 millisecond로 나타냄
                //옵션 : 
                time = ((chagneLimitedTime / 1000) - ((chagneLimitedTime / changedTempo) / 1000)) * -1;
                // time = 템포 변하고 지나간 일반적인 시간  -  템포가 변하고 지나간 변화된 시간. => 차이, 그 차이를 빼주면 된다.
                //빨라졌을 떄는 음수가 나오고 (전체적으로 떙기고), 느려졌을 때는 양수가 나온다. 
               
                //***이거 왜 축척?
               optionalTime = time;
              
                //각 노트 시작에 옵션을 더함
                //옵션이 지속시간(끝시간)에도 영향을 줘야 할듯. 
                file.OptionalArrayNote(optionalTime);



                //템포가 0.9배가 된상태에서 1초동안 지속이 된다면 모두 4-  4/4   3초씩 줄여야 한다ㅣ

                //oneTime = false;
                chagneLimitedTime = 0;


                //원래 템포로 돌아감
                ReturnBasicTempo();
            //}
        }

    }
}
