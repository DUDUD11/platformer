using Flat.Graphics;
using FlatPhysics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
namespace ShootingGame
{
    public class BlinkEffect : Effect2d
    {
        public static string BlinkEffect_path = "Effects\\Rez";
        public static Vector2 BlinkEffect_Frame = new Vector2(4, 1);
        public static Vector2 BlinkEffect_Dims = new Vector2(100, 100);

        public BlinkEffect(Game1 game,FlatWorld.Wolrd_layer wolrd_Layer, float LiveTime, int millitimePerFrame, string name = null)
        : base(game, BlinkEffect_path, new Vector2(0, 0), BlinkEffect_Dims, wolrd_Layer, BlinkEffect_Frame, LiveTime, (int)(BlinkEffect_Frame.X* BlinkEffect_Frame.Y), millitimePerFrame, name)
        {

        }

        public override void Update()
        {
            base.Update();

         

        }

        public override void Draw(Sprites sprite, Vector2 o)
        {
            Game1.AntiAliasingShader(model, dims, Animation_Set[0].FrameSize);
            base.Draw(sprite, o, angle);
        }
    }
}

