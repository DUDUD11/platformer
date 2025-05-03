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
    public class TargetingCircle : Effect2d
    {
        public static string TargetingCircle_path = "Effects\\TargetCircle";
        public static Vector2 TargetingCircle_Frame = new Vector2(1, 1);

        public TargetingCircle(Game1 game, Vector2 DIMS, FlatWorld.Wolrd_layer wolrd_Layer, float LiveTime)  
        : base(game, TargetingCircle_path,new Vector2(0,0),DIMS, wolrd_Layer, TargetingCircle_Frame, LiveTime)
        {
     
        }


        public TargetingCircle(Game1 game, Vector2 DIMS, FlatWorld.Wolrd_layer wolrd_Layer)
        : base(game, TargetingCircle_path, new Vector2(0,0), DIMS, wolrd_Layer, TargetingCircle_Frame, float.MaxValue)
        {

        }

        public override void Update()
        {
            base.Update();

       //     angle += (float)(Flat.FlatMath.Pi * 2.0f / 60.0f);

        }

        public override void Draw(Sprites sprite, Vector2 o)
        {
            Game1.AntiAliasingShader(model, dims, Animation_Set[currentAnimation].FrameSize);
            base.Draw(sprite, o, angle);
        }

    }
}
