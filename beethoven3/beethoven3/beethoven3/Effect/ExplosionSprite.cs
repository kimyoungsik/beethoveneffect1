using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
namespace beethoven3
{
    class ExplosionSprite : Sprite

    {

        private int initialDuration;

        //남은시점
        private int remainingDuration;



        //차이, 입자가 얼마나 많은 프레임동안 존재하는지
        public int ElapsedDuration
        {
            get
            {
                return initialDuration - remainingDuration;
            }
        }

        //어떤 시점에 놓여있는지 퍼센트
        public float DurationProgress
        {
            get
            {
                return (float)ElapsedDuration /
                    (float)initialDuration;
            }
        }

        //remainDuration이 0이되면 false를 리턴 
        public bool IsActive
        {
            get
            {
                return (remainingDuration > 0);
            }
        }

        public ExplosionSprite(
            Vector2 location,
            Texture2D texture,
            Rectangle initialFrame,
            Vector2 velocity,
            int duration,
            float scale)
            : base(location, texture, initialFrame, velocity,scale)
        {
            initialDuration = duration;
            remainingDuration = duration;
          
        }


        public override void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                remainingDuration--;
                Trace.WriteLine(remainingDuration);
                base.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive)
            {
                base.Draw(spriteBatch);

            }
        }

    }
}
