using Flat;
using Flat.Graphics;
using Flat.Input;
using FlatPhysics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FlatPhysics.FlatBody;
using static FlatPhysics.FlatWorld;


namespace ShootingGame
{
    public class Trap : Animated2d
    {
        // 플레이어와 충돌유무가 중요하다.
        // 조건을 만족했을때 플레이어와 충돌 or 어느타일에 도달했으면 플레이어는 죽어야한다.
        // flatbody 에 attack? 같은게 켜져있을때 (감전같은경우) 플레이어가 충돌했으면 죽이는걸로 하자

        public FlatBody flatBody;
        private ShapeType shapeType;

        public static int Addition_variable = 0;
        public float trap_period;
        public readonly static int Trap_Animation_num = 1;

        public Trap(Game1 game, string path, Vector2 init_pos, Vector2 DIMS, float trap_period, ShapeType shape, Vector2 frames, int millisecondFrame, bool flatbody, string name = null) :
            base(game, path, init_pos, DIMS, FlatWorld.Wolrd_layer.Static_allias, frames, Trap_Animation_num, (int)(frames.X * frames.Y), millisecondFrame, name ?? "Idle")
        {
            this.trap_period = trap_period;
            this.shapeType = shape;
            if (flatbody)
            {
                InitFlatBody(init_pos, DIMS);
            }
         
        }

        public override bool GetFlatBody(out FlatBody _flatBody)
        {
            _flatBody = null;

            if (this.flatBody != null)
            {
                _flatBody = flatBody;
                return true;
            }
            return false;
        }
        protected void InitFlatBody(Vector2 pos, Vector2 size)
        {

            if (shapeType == ShapeType.Box)
            {
                if (!FlatBody.CreateBoxBody(size.X, size.Y,
                1f, true, 0.5f, out FlatBody TrapBody, out string errorMessage))
                {
                    throw new Exception(errorMessage);
                }
                flatBody = TrapBody;
            }

            else if (shapeType == ShapeType.Circle)//circle
            {
                if (!FlatBody.CreateCircleBody((size.X + size.Y) / 2 * FlatAABB.HitBoxSize,
                1f, true, 0.5f, out FlatBody TrapBody, out string errorMessage))
                {
                    throw new Exception(errorMessage);
                }

                flatBody = TrapBody;
            }


            flatBody.MoveTo(pos.X, pos.Y);

        }

        public virtual void Interact(SpriteEntity spriteEntity)
        {

        }

        public override void Update()
        {
            base.Update();
        }

        public override void Draw(Sprites sprite, Vector2 o, float angle)
        {
            Game1.AntiAliasingShader(model, this.dims, this.Animation_Set[currentAnimation].FrameSize);
            base.Draw(sprite, o, angle);
        }

        

    }
}

/*

namespace ShootingGame
{
    public class Hero : Animated2d
    {
        public override void Update(List<Mob> mobs, Vector2 AdjustPos)
        { 
            base.Update();

            trail.Update(GetCurrentAnimationModelPath(),this.pos,flatBody.Angle,Src_Rectangle(),Get_Sheet());

            pos.X = FlatBody.Position.X;
            pos.Y = FlatBody.Position.Y;

            velocity = FlatVector.ToVector2(flatBody.LinearVelocity);

            if (throb)
            {
                if (Hero_Throb_Time + throb_timer < Game1.WorldTimer.Elapsed.TotalSeconds)
                {
                    throb_timer = Game1.WorldTimer.Elapsed.TotalSeconds;
                    throb = false;
                    ChangeCurrentAnimation(0);
                }
            }

            else
            {
                if (this.flatBody.LinearVelocity.Y < -2f)
                {
                    bottomed = false;
                    ChangeCurrentAnimation(3);
                }

                else if (bottomed)
                {
                    if (Math.Abs(this.flatBody.LinearVelocity.X) > 2f && !IsRun)
                    {
                        IsRun = true;
                        ChangeCurrentAnimation(5);
                    }


                    else if (currentAnimation != 0 && Math.Abs(this.flatBody.LinearVelocity.X)<2f)
                    {
                        ChangeCurrentAnimation(0);
                    }
                }

            }

   
        }


 */