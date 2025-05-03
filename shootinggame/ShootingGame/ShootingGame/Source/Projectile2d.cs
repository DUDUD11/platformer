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
    public class Projectile2d : SpriteEntity
    {
        public struct st_LiveTime
        {
            public double init_time;
            public double live_time;

            public st_LiveTime(double init_time, double live_time)
            {
                this.init_time = init_time;
                this.live_time = live_time;
            }

            public st_LiveTime()
            {
                init_time = 0;
                live_time = Double.MaxValue;
            }
        }

        public float projectile_speed = 500f;
        protected FlatBody flatBody;
        public SpriteEntity owner;
        public st_LiveTime st_timer;
      

        public FlatBody FlatBody
        { get { return flatBody; } }


        public Projectile2d(Game1 game, String path, Vector2 init_pos, Vector2 DIMS, SpriteEntity owner,Vector2 proectile_dir, Wolrd_layer wolrd_Layer) : base(game, path, init_pos, DIMS, wolrd_Layer)
        {
            this.owner = owner;
            st_timer = new st_LiveTime();
         //   InitFlatBody(init_pos, DIMS, owner.velocity.Length(), proectile_dir);
        }

        public Projectile2d(Game1 game, String path, Vector2 init_pos, Vector2 DIMS, SpriteEntity owner, Vector2 proectile_dir,st_LiveTime livetime, Wolrd_layer wolrd_Layer) : base(game, path, init_pos, DIMS, wolrd_Layer)
        {
            this.owner = owner;
            st_timer = livetime;
           // InitFlatBody(init_pos, DIMS, owner.velocity.Length(), proectile_dir);

        }




        public override void Update()
        {
            base.Update();
            pos.X = FlatBody.Position.X;
            pos.Y = FlatBody.Position.Y;

            

            if (Game1.WorldTimer.Elapsed.TotalSeconds - st_timer.init_time > st_timer.live_time)
            {
                TimeExpired();
            }
        }

        public override void Draw(Sprites sprite, Vector2 o)
        {

            Game1.AntiAliasingShader(model, dims);

            sprite.Draw(model, new Rectangle((int)(pos.X + o.X), (int)(pos.Y + o.Y), (int)dims.X, (int)dims.Y), Color.White, flatBody.Angle,
                 new Vector2(model.Bounds.Width / 2, model.Bounds.Height / 2));

        }

        public override bool GetFlatBody(out FlatBody flatBody)
        {
            flatBody = this.FlatBody;
           return true;
        }

        public void TimeExpired()
        {
            Destroy = true;
        }

        public virtual bool HitSomething(SpriteEntity spriteEntity)
        {
            return false;
        }

    }
}


 




        
        
