using Flat;
using Flat.Graphics;
using FlatPhysics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FlatPhysics.FlatWorld;


namespace ShootingGame
{
    public class Effect2d : Animated2d
    {
        public float LiveTime;
        public double CurTime;
     


        public Effect2d(Game1 game, String path, Vector2 init_pos, Vector2 DIMS, FlatWorld.Wolrd_layer wolrd_Layer, Vector2 Frame, float LiveTime)
            : base(game, path, init_pos, DIMS, wolrd_Layer,Frame, 1, 1,100,"default")
        {
            this.LiveTime = LiveTime;
            this.CurTime = Game1.WorldTimer.Elapsed.TotalSeconds;
        }

        public Effect2d(Game1 game, String path, Vector2 init_pos, Vector2 DIMS, FlatWorld.Wolrd_layer wolrd_Layer, Vector2 Frame, float LiveTime,
                        int totalframe, int millitimePerFrame, string name = null)
    : base(game, path, init_pos, DIMS, wolrd_Layer, Frame,1, totalframe, millitimePerFrame, name)
        {
            this.LiveTime = LiveTime;
            this.CurTime = Game1.WorldTimer.Elapsed.TotalSeconds;
        }


        public override void Update()
        {
            base.Update();

            if (CurTime + (double)LiveTime < Game1.WorldTimer.Elapsed.TotalSeconds)
            {
                Destroy = true;
            }
        }

        public void CurTime_Reset()
        {
            CurTime = Game1.WorldTimer.Elapsed.TotalSeconds;
        }



    }

}

