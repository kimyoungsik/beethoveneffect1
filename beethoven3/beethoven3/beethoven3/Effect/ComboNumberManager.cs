using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
namespace beethoven3
{
    class ComboNumberManager
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
      //  private int duration = 40;
        private List<Number> ComboNumbers = new List<Number>();


        #endregion


        #region constructor

        public void LoadContent(ContentManager cm)
        {
            //zero = cm.Load<Texture2D>(@"comboNumbers\zero");
            //one = cm.Load<Texture2D>(@"comboNumbers\one");
            //two = cm.Load<Texture2D>(@"comboNumbers\two");
            //three = cm.Load<Texture2D>(@"comboNumbers\three");
            //four = cm.Load<Texture2D>(@"comboNumbers\four");
            //five = cm.Load<Texture2D>(@"comboNumbers\five");
            //six = cm.Load<Texture2D>(@"comboNumbers\six");
            //seven = cm.Load<Texture2D>(@"comboNumbers\seven");
            //eight = cm.Load<Texture2D>(@"comboNumbers\eight");
            //nine = cm.Load<Texture2D>(@"comboNumbers\nine");

            zero = cm.Load<Texture2D>(@"comboNumbers\one");
            one = cm.Load<Texture2D>(@"comboNumbers\one");
            two = cm.Load<Texture2D>(@"comboNumbers\one");
            three = cm.Load<Texture2D>(@"comboNumbers\one");
            four = cm.Load<Texture2D>(@"comboNumbers\one");
            five = cm.Load<Texture2D>(@"comboNumbers\one");
            six = cm.Load<Texture2D>(@"comboNumbers\one");
            seven = cm.Load<Texture2D>(@"comboNumbers\one");
            eight = cm.Load<Texture2D>(@"comboNumbers\one");
            nine = cm.Load<Texture2D>(@"comboNumbers\one");



        }

              
        public void AddComboNumbers(Vector2 location, int num, int duration = 30)
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

            Number thisComboNumbers = new Number(
                texture,
                location,
                initialFrame,
                duration,
                frameCount,
                scale);
            ComboNumbers.Add(thisComboNumbers);


        }
        public void deleteAllMarks()
        {

            for (int i = 0; i < ComboNumbers.Count; i++)
            {
                ComboNumbers.RemoveAt(i);
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
            for (i = 0; i < ComboNumbers.Count; i++)
            {
                ComboNumbers[i].Update(gameTime);

            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            int i;
            for (i = 0; i < ComboNumbers.Count; i++)
            {
                ComboNumbers[i].Draw(spriteBatch);

            }

        }


        #endregion

    }

}
