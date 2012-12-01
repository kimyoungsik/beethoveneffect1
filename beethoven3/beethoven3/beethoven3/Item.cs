using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
namespace beethoven3
{
    static class Item
    {

        public static Texture2D[] rightHand = new Texture2D[5];



        public static void LoadContent(ContentManager cm)
        {

            rightHand[0] = cm.Load<Texture2D>(@"Textures\red");

        }


    }
}
