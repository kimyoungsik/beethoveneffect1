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
        private Texture2D texture;


        private Texture2D pzero;
        private Texture2D pone;
        private Texture2D ptwo;
        private Texture2D pthree;
        private Texture2D pfour;
        private Texture2D pfive;
        private Texture2D psix;
        private Texture2D pseven;
        private Texture2D peight;
        private Texture2D pnine;

        private Texture2D gzero;
        private Texture2D gone;
        private Texture2D gtwo;
        private Texture2D gthree;
        private Texture2D gfour;
        private Texture2D gfive;
        private Texture2D gsix;
        private Texture2D gseven;
        private Texture2D geight;
        private Texture2D gnine;
        
        private Rectangle initialFrame = new Rectangle(0, 0, 275, 376);
        private float scale = 0.4f;
        private int frameCount = 1;
      //  private int duration = 40;
        private List<Number> ComboNumbers = new List<Number>();


        #endregion


        #region constructor

        public void LoadContent(ContentManager cm)
        {
            pzero = cm.Load<Texture2D>(@"comboNumbers\perfect0");
            pone = cm.Load<Texture2D>(@"comboNumbers\perfect1");
            ptwo = cm.Load<Texture2D>(@"comboNumbers\perfect2");
            pthree = cm.Load<Texture2D>(@"comboNumbers\perfect3");
            pfour = cm.Load<Texture2D>(@"comboNumbers\perfect4");
            pfive = cm.Load<Texture2D>(@"comboNumbers\perfect5");
            psix = cm.Load<Texture2D>(@"comboNumbers\perfect6");
            pseven = cm.Load<Texture2D>(@"comboNumbers\perfect7");
            peight = cm.Load<Texture2D>(@"comboNumbers\perfect8");
            pnine = cm.Load<Texture2D>(@"comboNumbers\perfect9");

            gzero = cm.Load<Texture2D>(@"comboNumbers\good0");
            gone = cm.Load<Texture2D>(@"comboNumbers\good1");
            gtwo = cm.Load<Texture2D>(@"comboNumbers\good2");
            gthree = cm.Load<Texture2D>(@"comboNumbers\good3");
            gfour = cm.Load<Texture2D>(@"comboNumbers\good4");
            gfive = cm.Load<Texture2D>(@"comboNumbers\good5");
            gsix = cm.Load<Texture2D>(@"comboNumbers\good6");
            gseven = cm.Load<Texture2D>(@"comboNumbers\good7");
            geight = cm.Load<Texture2D>(@"comboNumbers\good8");
            gnine = cm.Load<Texture2D>(@"comboNumbers\good9");
       

        }

        //타입에 따라,퍼펙트 0 ,good 1   
        public void AddComboNumbers(Vector2 location, int num, int type, int duration = 30)
        {
            if (type == 0)
            {
                switch (num)
                {
                    case 0:

                        texture = pzero;

                        break;
                    case 1:

                        texture = pone;

                        break;
                    case 2:

                        texture = ptwo;

                        break;
                    case 3:

                        texture = pthree;

                        break;
                    case 4:

                        texture = pfour;

                        break;
                    case 5:

                        texture = pfive;

                        break;
                    case 6:

                        texture = psix;

                        break;
                    case 7:

                        texture = pseven;

                        break;
                    case 8:

                        texture = peight;

                        break;
                    case 9:

                        texture = pnine;

                        break;


                }
            }
            else if (type == 1)
            {
                switch (num)
                {
                    case 0:

                        texture = gzero;

                        break;
                    case 1:

                        texture = gone;

                        break;
                    case 2:

                        texture = gtwo;

                        break;
                    case 3:

                        texture = gthree;

                        break;
                    case 4:

                        texture = gfour;

                        break;
                    case 5:

                        texture = gfive;

                        break;
                    case 6:

                        texture = gsix;

                        break;
                    case 7:

                        texture = gseven;

                        break;
                    case 8:

                        texture = geight;

                        break;
                    case 9:

                        texture = gnine;

                        break;


                }

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

        public void Clear()
        {
            ComboNumbers.Clear();
            
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
