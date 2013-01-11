using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
namespace beethoven3
{
    class ResultNumberManager
    {

        #region declarations

        private Texture2D zero;
        private Texture2D one;
        private Texture2D two;
        private Texture2D three;
        private Texture2D four;
        private Texture2D five;
        private Texture2D six;
        private Texture2D seven;
        private Texture2D eight;
        private Texture2D nine;
        private Texture2D texture;


        private Rectangle initialFrame = new Rectangle(0, 0, 126, 167);
        private float scale = 1.0f;
        private int frameCount = 1;

        private List<ResultNumber> resultNumbers = new List<ResultNumber>();


        #endregion


        #region constructor

        public void LoadContent(ContentManager cm)
        {
            //zero = cm.Load<Texture2D>(@"resultNumbers\zero");
            //one = cm.Load<Texture2D>(@"resultNumbers\one");
            //two = cm.Load<Texture2D>(@"resultNumbers\two");
            //three = cm.Load<Texture2D>(@"resultNumbers\three");
            //four = cm.Load<Texture2D>(@"resultNumbers\four");
            //five = cm.Load<Texture2D>(@"resultNumbers\five");
            //six = cm.Load<Texture2D>(@"resultNumbers\six");
            //seven = cm.Load<Texture2D>(@"resultNumbers\seven");
            //eight = cm.Load<Texture2D>(@"resultNumbers\eight");
            //nine = cm.Load<Texture2D>(@"resultNumbers\nine");

            zero = cm.Load<Texture2D>(@"resultNumbers\one");
            one = cm.Load<Texture2D>(@"resultNumbers\one");
            two = cm.Load<Texture2D>(@"resultNumbers\one");
            three = cm.Load<Texture2D>(@"resultNumbers\one");
            four = cm.Load<Texture2D>(@"resultNumbers\one");
            five = cm.Load<Texture2D>(@"resultNumbers\one");
            six = cm.Load<Texture2D>(@"resultNumbers\one");
            seven = cm.Load<Texture2D>(@"resultNumbers\one");
            eight = cm.Load<Texture2D>(@"resultNumbers\one");
            nine = cm.Load<Texture2D>(@"resultNumbers\one");

        }


        public void AddComboNumbers(Vector2 location, int num)
        {
            switch (num)
            {
                case 0:

                    texture = zero;

                    break;
                case 1:

                    texture = one;

                    break;
                case 2:

                    texture = two;

                    break;
                case 3:

                    texture = three;

                    break;
                case 4:

                    texture = four;

                    break;
                case 5:

                    texture = five;

                    break;
                case 6:

                    texture = six;

                    break;
                case 7:

                    texture = seven;

                    break;
                case 8:

                    texture = eight;

                    break;
                case 9:

                    texture = nine;

                    break;


            }

            ResultNumber thisComboNumbers = new ResultNumber(
                texture,
                location,
                initialFrame,
                frameCount,
                scale);
            resultNumbers.Add(thisComboNumbers);


        }
        public void deleteAllMarks()
        {

            for (int i = 0; i < resultNumbers.Count; i++)
            {
                resultNumbers.RemoveAt(i);
            }
        }



        #endregion

        #region properties
        public Rectangle InitialFrame
        {
            get { return initialFrame; }
            set { initialFrame = value; }
        }
        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public int FrameCount
        {
            get { return frameCount; }
            set { frameCount = value; }
        }

        #endregion

        #region method



        #endregion

        #region update and draw
        public void Update(GameTime gameTime)
        {

            int i;
            for (i = 0; i < resultNumbers.Count; i++)
            {
                resultNumbers[i].Update(gameTime);

            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            int i;
            for (i = 0; i < resultNumbers.Count; i++)
            {
                resultNumbers[i].Draw(spriteBatch);

            }

        }


        #endregion

    }

}
