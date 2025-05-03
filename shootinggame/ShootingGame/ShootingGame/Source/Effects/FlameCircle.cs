using Flat.Graphics;
using FlatPhysics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace ShootingGame
{
    public class FlameCircle : Effect2d
    {
        public static string FlameCircle_path = "Effects\\FireNova";
        public static Vector2 FlameCircle_Frame = new Vector2(1, 1);
        public static Vector2 FlameCircle_Dims = new Vector2(100, 100);

        public FlameCircle(Game1 game, FlatWorld.Wolrd_layer wolrd_Layer, float LiveTime)
        : base(game, FlameCircle_path, new Vector2(0,0), FlameCircle_Dims, wolrd_Layer, FlameCircle_Frame, LiveTime)
        {

        }

        public FlameCircle(Game1 game, Vector2 init_pos, FlatWorld.Wolrd_layer wolrd_Layer)
        : base(game, FlameCircle_path, init_pos, FlameCircle_Dims, wolrd_Layer, FlameCircle_Frame, float.MaxValue)
        {

        }

        public override void Update()
        {
            base.Update();
            
            angle += (float)(Flat.FlatMath.Pi*2.0f / 60.0f);
            
        }

        public override void Draw(Sprites sprite, Vector2 o)
        {
            Game1.AntiAliasingShader(model, dims, Animation_Set[currentAnimation].FrameSize);
            base.Draw(sprite, o, angle);
        }

    }
}
