using Flat.Graphics;
using Flat;
using FlatPhysics;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ShootingGame
{
    public class VanishingTile : DynamicTile
    {
        public float VanishingTime;
        public static string Vanishing_path = "Trap\\distile";
        public static Vector2 Vanishing_frames = new Vector2(7,1);
        private double timer;

        public VanishingTile(Game1 game,  Vector2 init_pos, Vector2 dims, int millitimePerFrame)
            : base(game, Vanishing_path, init_pos, new Vector2(dims.X/2,dims.Y/4),false, true, Vanishing_frames, 1, (int)(Vanishing_frames.X* Vanishing_frames.Y),  millitimePerFrame,true,false)
        {
            this.game = game;
            model = game.Content.Load<Texture2D>(Vanishing_path);
            pos = init_pos;
            this.dims = dims;
            VanishingTime = Vanishing_frames.X * millitimePerFrame / 1000;
            Set_repeat(0,false);
           

        }




        public override void Interact(SpriteEntity spriteEntity)
        {
            if (active) return;
            active = true;
            timer = Game1.WorldTimer.Elapsed.TotalSeconds;
        }

        public override void Update()
        {
            if (!active) return;
  
            base.Update();

            if (timer + VanishingTime <= Game1.WorldTimer.Elapsed.TotalSeconds)
            {
                Console.WriteLine(Game1.WorldTimer.Elapsed.TotalSeconds-(timer + VanishingTime));

                Destroy = true;
            }
         
        }

        public override void Draw(Sprites sprite, Vector2 o, float angle)
        {
            base.Draw(sprite, o, angle);

        }


    }
}
