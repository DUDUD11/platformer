using Flat.Graphics;
using FlatPhysics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FlatPhysics.FlatWorld;


namespace ShootingGame
{
    public class Mob : Animated2d
    {
        public float mob_Speed;
        public FlatBody flatBody;

        public FlatBody FlatBody
        { get { return flatBody; } }

        public bool ChasePlayer = false;
        public float AggroDistance=float.MaxValue;
        public float Atkrange = 0f;
        public float Atkdamage = 0f;

        public Mob(Game1 game, String path, Vector2 init_pos, Vector2 DIMS, FlatWorld.Wolrd_layer wolrd_Layer,
            float mobspeed, float aggrodist, float long_atkrange, float atkdamage, Vector2 Frame, int millisecondFrame,
            int animation_num, string name = null, bool circleBody = false) 
            : base(game, path, init_pos, DIMS, wolrd_Layer,Frame,animation_num,(int)(Frame.X*Frame.Y),100, name ?? "Idle")
        {
            InitFlatBody(init_pos, DIMS,circleBody);
            mob_Speed = mobspeed;
            AggroDistance = aggrodist;
            Atkrange = long_atkrange;
            Atkdamage = atkdamage;
        }

        protected FlatBody InitFlatBody(Vector2 pos, Vector2 size,bool circle=false)
        {
            FlatBody mobBody;
            
            if (!circle)
            {
                if (!FlatBody.CreateBoxBody(size.X * FlatAABB.HitBoxSize, size.Y * FlatAABB.HitBoxSize,
                1f, false, 0.5f, out  mobBody, out string errorMessage))
                {
                    throw new Exception(errorMessage);
                }

               
            }

            else
            {
                if (!FlatBody.CreateCircleBody((size.X + size.Y)/2f * FlatAABB.HitBoxSize,
                   1f, false, 0.5f, out mobBody, out string errorMessage))
                {
                    throw new Exception(errorMessage);
                }

                
            }
            flatBody = mobBody;
            flatBody.MoveTo(pos.X, pos.Y);

            return mobBody;
        }
        public override void Update (Hero hero) 
        {
     
            pos.X = FlatBody.Position.X;
            pos.Y = FlatBody.Position.Y;

            velocity = FlatVector.ToVector2(flatBody.LinearVelocity);

            
        }

        public override bool GetFlatBody(out FlatBody flatBody)
        {
            flatBody = this.FlatBody;
            return true;
        }

        public override void Destroy_Sprite()
        {
            Destroy = true;
      //      Text.MobKilled = Text.MobKilled + 1;

        }

        public virtual void AI(Hero hero)
        { 

        }

        public virtual void AttackPlayer(float damage, Hero hero)
        {
            hero.Get_Hit(damage);
        }

        public virtual void AttackPlayer(Hero hero)
        {
            hero.Get_Hit(Atkdamage);
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

        public virtual void Interact(Hero hero)
        { 
            
        }


        public override void Draw(Sprites sprite, Vector2 o,float angle)
        {

            base.Draw(sprite, o,angle);
    
        }


    }
}

    
