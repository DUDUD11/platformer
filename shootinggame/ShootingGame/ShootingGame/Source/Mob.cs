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
    public class Mob : SpriteEntity
    {
        public float mob_Speed;
        protected FlatBody flatBody;

        public FlatBody FlatBody
        { get { return flatBody; } }

        public bool ChasePlayer = false;
        public float AggroDistance=float.MaxValue;
        public float Atkrange = 0f;
        public float Atkdamage = 0f;

        public Mob(Game1 game, String path, Vector2 init_pos, Vector2 DIMS, FlatWorld.Wolrd_layer wolrd_Layer, float mobspeed, float aggrodist, float long_atkrange, float long_atkdamage) : base(game, path, init_pos, DIMS, wolrd_Layer)
        {
            InitFlatBody(init_pos, DIMS);
            mob_Speed = mobspeed;
            AggroDistance = aggrodist;
            Atkrange = long_atkrange;
            Atkdamage = long_atkdamage;
        }

        private void InitFlatBody(Vector2 pos, Vector2 size)
        {
            if (!FlatBody.CreateBoxBody(size.X * FlatAABB.HitBoxSize, size.Y * FlatAABB.HitBoxSize, 
            1f, false, 0.5f, out FlatBody mobBody, out string errorMessage))
            {
                throw new Exception(errorMessage);
            }

            flatBody = mobBody;

            flatBody.MoveTo(pos.X, pos.Y);

        }
        public override void Update (Hero hero) 
        {
     
            pos.X = FlatBody.Position.X;
            pos.Y = FlatBody.Position.Y;

            velocity = FlatVector.ToVector2(flatBody.LinearVelocity);

            AI(hero);
        }

        public override bool GetFlatBody(out FlatBody flatBody)
        {
            flatBody = this.FlatBody;
            return true;
        }

        public override void Destroy_Sprite()
        {
            Destroy = true;
            Text.MobKilled = Text.MobKilled + 1;

        }

        public virtual void AI(Hero hero)
        { 
        
        
        }

        public virtual void AttackPlayer(float damage, Hero hero)
        {
            hero.Get_Hit(damage);
        }

        public bool ShorterthanChaseDistance(Vector2 Pos)
        {
            float length = FlatMath.Distance(FlatBody.Position, new FlatVector(Pos.X,Pos.Y));
            return AggroDistance > length;
        }


        public bool ShorterthanAtkDistance(Vector2 Pos)
        {
            float length = FlatMath.Distance(FlatBody.Position, new FlatVector(Pos.X, Pos.Y));
            return Atkrange > length;
        }


        public override void Draw(Sprites sprite, Vector2 o)
        {
            Game1.AntiAliasingShader(model, dims);
            sprite.Draw(model, new Rectangle((int)(pos.X + o.X), (int)(pos.Y + o.Y), (int)dims.X, (int)dims.Y), Color.White, flatBody.Angle,
             new Vector2(model.Bounds.Width / 2, model.Bounds.Height / 2));
        }




    }
}

    
