//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace beethoven3
//{
//    class Particle : Sprite
//    {

//        //가속(속도에 반영)
//        private Vector2 acceleration;
//        //속도 값 범위 정함
//        private float maxSpeed;
//        private int initialDuration;

//        //남은시점
//        private int remainingDuration;

//        //색의 변화 
//        private Color initialColor;
//        private Color finalColor;

//        //차이, 입자가 얼마나 많은 프레임동안 존재하는지
//        public int ElapsedDuration
//        {
//            get
//            {
//                return initialDuration - remainingDuration;
//            }
//        }

//        //어떤 시점에 놓여있는지 퍼센트
//        public float DurationProgress
//        {
//            get
//            {
//                return (float)ElapsedDuration /
//                    (float)initialDuration;
//            }
//        }

//        //remainDuration이 0이되면 false를 리턴 
//        public bool IsActive
//        {
//            get
//            {
//                return (remainingDuration > 0);
//            }
//        }

//        public Particle(
//            Vector2 location,
//            Texture2D texture,
//            Rectangle initialFrame,
//            Vector2 velocity,
//            Vector2 acceleration,
//            float maxSpeed,
//            int duration,
//            Color initialColor,
//            Color finalColor,
//            float scale)
//            : base(location, texture, initialFrame, velocity,scale)
//        {
//            initialDuration = duration;
//            remainingDuration = duration;
//            this.acceleration = acceleration;
//            this.initialColor = initialColor;
//            this.maxSpeed = maxSpeed;
//            this.finalColor = finalColor;
//        }


//        public override void Update(GameTime gameTime)
//        {
//            if (IsActive)
//            {

//                velocity += acceleration;

//                //입자 최대 속도를 초과하는지 체크 
                
//                if (velocity.Length() > maxSpeed)
//                {
//                //보정
//                    velocity.Normalize();
//                    velocity *= maxSpeed;
//                }
//                //Lerp, 0이면 첫번째 파라미터 리턴 1이면 두번쨰파라미터,, 그 사이값
//                TintColor = Color.Lerp(
//                    initialColor,
//                    finalColor,
//                    DurationProgress);
//                remainingDuration--;
//                base.Update(gameTime);
//            }
//        }

//        public override void Draw(SpriteBatch spriteBatch)
//        {
//            if (IsActive)
//            {
//                base.Draw(spriteBatch);
        
//            }
//        }

//    }
//}
