using Flat;
using Flat.Graphics;
using Flat.Input;
using FlatPhysics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using static FlatPhysics.FlatWorld;

namespace ShootingGame
{
    public class Hero : Animated2d
    {

        private FlatBody flatBody;
        private Trail trail;
        public Vector2 Revive_Pos;


        public static float trail_lifespan = 1f;
        public static float trail_gentime = 0.15f;
        public static float Speed = 500 * 75f;
        public static Vector2 Hero_Frames = new Vector2(11, 1);
        public static int Hero_animation_num = 7;
        public static float DoubleJumpIntime = 3f;
        public static float DashStartTime;
        public static float WallJumpCooldown =0.1f;
        public static float Hero_Throb_Time = 1.2f;
        public static float Hero_Dead_EffectTime = 0.25f;
        public static float WallJumpPortion = 0.8f;
        public static float Respawn_Time = 2f;
        public static int MovingPlatformWalkFactor = 30;

        public float health, maxHealth;
        public double JumpLastTime = float.MinValue;
        public double ActivatedWallJump = float.MinValue;
        public double WallJumpLastTime = float.MinValue;
        public List<Skill> skillList = new List<Skill>();

        private double throb_timer = 0f;
 
        private double Dead_timer = 0f;
    
        public int dash = 0;
        public bool Left = false;
        public bool Jumped = false;

        public Hero_Status status;
        public Hero_Status2 ani_status;

        public Vector2 delta_Velocity { get; set; } = Vector2.Zero;
        public bool Hero_Moved { get; set; } = false;



        public enum Hero_Status
        { 
            bottomed,
            wallContact,
            MovingPlatform,
            aerial,
           
        
        }

        public enum Hero_Status2
        { 
            idle,
            jump,
            isRun,
            throb,     
        }

     
       
        public FlatBody FlatBody
        { get { return flatBody; } }

        public void Set_RevivePos(Vector2 pos)
        { 
            this.Revive_Pos = pos;
        }

        public void SkillAdd(Skill skill)
        { 
            skillList.Add(skill);
        }

        public void ActivateSkill(Object obj)
        {
            if (obj is string obj_str)
            {
                for (int i = 0; i < skillList.Count; i++)
                {
                    if (obj_str.Equals(skillList[i].Name))
                    {
                        if (skillList[i].activate == true) return;
                        skillList[i].Activate(new Vector2(Game1.screen_width * 0.5f + Game1.mouseWorldPosition.X - Game1.offset.X, Game1.screen_height * 0.5f + Game1.mouseWorldPosition.Y - Game1.offset.Y));
                        return;
                    }

                }
            }
        }



        public bool ActivateSkill(string name, Vector2 Pos)
        {
            for (int i = 0; i < skillList.Count; i++)
            {
                if (skillList[i].Name.Equals(name))
                {
                    if (skillList[i].activate == true) return false;

                    skillList[i].Activate(Pos);
                    return true;
                }

            }
            return false;
        }


        public Hero(Game1 game, String path, Vector2 init_pos, Vector2 DIMS,FlatWorld.Wolrd_layer wolrd_Layer,int millisecondFrame, string name=null) : 
            base(game, path, init_pos, DIMS, wolrd_Layer,Hero_Frames,Hero_animation_num,
            (int)(Hero_Frames.X*Hero_Frames.Y),millisecondFrame, name??"Idle")
        {
            maxHealth = 5;
            health = maxHealth;
          //  Revive_Pos = init_pos;


            InitFlatBody(init_pos, DIMS);
            trail = new Trail(game, trail_lifespan, DIMS, trail_gentime);
        }

        //public Hero(Game1 game, String path, Vector2 init_pos, Vector2 DIMS, Vector2 velocity, Wolrd_layer wolrd_Layer) : base(game, path, init_pos, DIMS,velocity, wolrd_Layer, Hero_Frames)
        //{
        //    maxHealth = 5;
        //    health = maxHealth;

        //    InitFlatBody(init_pos, DIMS);
        //}

        private void InitFlatBody(Vector2 pos, Vector2 size)
        {
            if (!FlatBody.CreateBoxBody(size.X*FlatAABB.HitBoxSize,size.Y * FlatAABB.HitBoxSize,
            2f, false, 0.2f, out FlatBody HeroBody, out string errorMessage))
            {
                throw new Exception(errorMessage);
            }

            flatBody = HeroBody;

            flatBody.MoveTo(pos.X,pos.Y);

        }

        private void dash_Update()
        {
            if (dash == 1)
            {
                if (trail_lifespan + DashStartTime > Game1.WorldTimer.Elapsed.TotalSeconds)
                {
                    trail.Update(GetCurrentAnimationModelPath(), this.pos, flatBody.Angle, Src_Rectangle(), Get_Sheet());
                }

                else
                {
                    dash = 2;
                }
            }

        }

 

        public void Revive()
        {

            Destroy = false;
            dash = 0;

            status = Hero_Status.aerial;
            ani_status = Hero_Status2.idle;

            health = maxHealth;
            flatBody.Reset(Revive_Pos.X,Revive_Pos.Y);
            this.pos = Revive_Pos;

            FlatUtil.ResetGameSpeed();


            game.Camera_Reset();

        }

        public void Dash(FlatVector vec)
        {
            dash = 1;
            status = Hero_Status.aerial;

            DashStartTime = (float)Game1.WorldTimer.Elapsed.TotalSeconds;
            FlatBody.LinearVelocity += vec * 200;

        }

        public void Jump()
        {
            status = Hero_Status.aerial;
            JumpLastTime = Game1.WorldTimer.Elapsed.TotalSeconds;
            FlatVector forecdirection = new FlatVector(0, 1);
          //  FlatVector force = forecdirection * forceMagnitude / 800;
            FlatBody.LinearVelocity += new FlatVector(0, 200);
            if (FlatBody.LinearVelocity.Y < 200)
            {
                FlatBody.LinearVelocity = new FlatVector(FlatBody.LinearVelocity.X, 200f);
            }

            Jumped = true;

            ChangeCurrentAnimation(1);
        }
        public void DoubleJump()
        {
            status = Hero_Status.aerial;
            JumpLastTime = -1;

            FlatVector forecdirection = new FlatVector(0, 1);
            //  FlatVector force = forecdirection * forceMagnitude / 800;
            FlatBody.LinearVelocity += new FlatVector(0, 200);
            ChangeCurrentAnimation(2);
        }


        public void WallJump(FlatVector Normaldir)
        {
            status = Hero_Status.aerial;
            WallJumpLastTime = Game1.WorldTimer.Elapsed.TotalSeconds;

            //  FlatVector force = forecdirection * forceMagnitude / 800;
            FlatBody.LinearVelocity += Normaldir * 200;

            if (FlatBody.LinearVelocity.Y < Normaldir.Y * 220)
            {
                FlatBody.LinearVelocity = new FlatVector(FlatBody.LinearVelocity.X, Normaldir.Y * 220);
            }




            ChangeCurrentAnimation(6);
        }

        public void bottomReach()
        {
            status = Hero_Status.bottomed;
            dash = 0;
            Jumped = false;
        }

        public void WallReach()
        {
            status = Hero_Status.wallContact;
            ActivatedWallJump = Game1.WorldTimer.Elapsed.TotalSeconds;
        }

        public bool Can_WallJump()
        {
            return ActivatedWallJump + 0.2f > Game1.WorldTimer.Elapsed.TotalSeconds;
        }



        public void Get_Hit(float damage)
        {
     
            if (ani_status== Hero_Status2.throb || Destroy) return;

            health-=damage;

            if (damage <0 || health < 0f || FlatUtil.IsNearlyEqual(health, 0f))
            {

                health = 0;
                Destroy = true;
                ani_status = Hero_Status2.idle;

                Dead_timer = Game1.WorldTimer.Elapsed.TotalSeconds;

            }

            else
            {
                ani_status = Hero_Status2.throb;
                throb_timer = Game1.WorldTimer.Elapsed.TotalSeconds;
                ChangeCurrentAnimation(4);
            }
        }

        private bool falling(float a, float b)
        {
            if (status == Hero_Status.bottomed) return false;

            if (this.flatBody.LinearVelocity.Y < -2f && (a - b > 1f))
            {
                return true;
            }
            return false;
        }


        public override bool GetFlatBody(out FlatBody flatBody)
        {
            flatBody = this.FlatBody;
            return true;
        }

        public bool Try_Revive()
        {
            if (!Destroy) return false;

            if (Game1.WorldTimer.Elapsed.TotalSeconds - Dead_timer > Respawn_Time)
            {
              
                Revive();
                return true;
            }

            return false;

        }

        public override void Update(List<Mob> mobs, Vector2 AdjustPos)
        {

            if (Destroy)
            {
                return;
            }

            base.Update();

            float tmpY = pos.Y;

            pos.X = FlatBody.Position.X;
            pos.Y = FlatBody.Position.Y;
            velocity = FlatVector.ToVector2(flatBody.LinearVelocity);

            dash_Update();


            if (ani_status == Hero_Status2.throb)
            {
                if (Hero_Throb_Time + throb_timer < Game1.WorldTimer.Elapsed.TotalSeconds)
                {
                    throb_timer = Game1.WorldTimer.Elapsed.TotalSeconds;
                    ani_status = Hero_Status2.idle;
                    ChangeCurrentAnimation(0);
                }
            }

            else if (status == Hero_Status.MovingPlatform)
            {
                ChangeCurrentAnimation(0);
            }

            else
            {
                if (falling(tmpY,pos.Y) )
                {
                 //   status = Hero_Status.aerial;
                    ChangeCurrentAnimation(3);
                }

                else if (status == Hero_Status.bottomed)
                {
                    if (Math.Abs(this.flatBody.LinearVelocity.X) > 2f && ani_status != Hero_Status2.isRun)
                    {
                        ani_status = Hero_Status2.isRun;
                        ChangeCurrentAnimation(5);
                    }

                    else if (currentAnimation != 0 && Math.Abs(this.flatBody.LinearVelocity.X) < 2f)
                    {
                        ChangeCurrentAnimation(0);
                    }
                }

            }
            //ani가 없음
            //else if(wallContact)
            //{ 
                  
            //}
            
            

            for (int i = 0; i < skillList.Count; i++)
            {
                if (skillList[i].activate)
                {
                    skillList[i].Targeting(AdjustPos,mobs,this);
                }
            }
     
        }



        public override void Draw(Sprites sprite, Vector2 o)
        {

            if (Destroy)
            {
                float portion = (float)(Game1.WorldTimer.Elapsed.TotalSeconds - Dead_timer);

                if (Game1.WorldTimer.Elapsed.TotalSeconds - Dead_timer > Hero_Dead_EffectTime)
                {
                    return;
                }

                Game1.ShrinkCircleShader(portion / Hero_Dead_EffectTime, Color.White, Animation_Set[currentAnimation].Frames, Get_SheetFrame());

       
                base.Draw(sprite, o, flatBody.Angle);


                return;
            }



            if (ani_status == Hero_Status2.throb)
            {
                float phase = (float)(Game1.WorldTimer.Elapsed.TotalSeconds - throb_timer);

                Game1.ThrobShader((float)(Math.Sin(phase + MathF.PI / 2) * (MathF.PI * 3)), Color.Red,Left==false?0:1);

            }
            else
            {
                Game1.AntiAliasingShader(model, dims, Hero.Hero_Frames, Left == false ? 0 : 1);

            }
        //    Game1.HorizontalMirrorShader(this.GetModel(), this.dims, Hero.Hero_Frames, Left==false?0:1);

            base.Draw(sprite, o, flatBody.Angle);

            // trail.Draw(sprite,o);
            // aniatmo이 항상 동작하단다고 가정

            if (dash==1)
            {
                trail.AnimationDraw(sprite, o);
            }

            for (int i = 0; i < skillList.Count; i++)
            {
                if (skillList[i].activate)
                {
                    if (skillList[i] is Blink)
                    {
                        skillList[i].Draw(sprite,Vector2.Add(pos,-Game1.offset));
                    }
                    else
                    {
                        skillList[i].Draw(sprite, o);
                    }
                }
            }

            //      Game1.AntiAliasingShader(model, dims);

            //    sprite.Draw(model, new Rectangle((int)(pos.X+o.X), (int)(pos.Y+o.Y), (int)dims.X, (int)dims.Y), Color.White,flatBody.Angle,
            //       new Vector2(model.Bounds.Width / 2, model.Bounds.Height / 2));

        }

    }
}
