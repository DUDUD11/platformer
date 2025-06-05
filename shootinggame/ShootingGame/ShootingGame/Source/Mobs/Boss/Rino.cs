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
    class Rino : Mob
    {
        public static float defaultAngle = MathHelper.PiOver2;
        public static string Rino_path = "Animation\\Rino\\Idle";
        public static Vector2 Rino_Idel_Frame = new Vector2(11, 1);

        public static string Rino_Runpath = "Animation\\Rino\\Run";
        public static string Rino_Hitpath = "Animation\\Rino\\Hit";

        public static Vector2 Rino_dims = new Vector2(50, 50);
        public static Vector2 Rino_dims2 = Rino_dims * 7;
        

        public static float Rino_speed = 1000f;
        public static int Rino_AnimationNum = 3;
        public static int Rino_MilliFrameRate = 100;


        private float RespawnCoolTime = 2f;
        private float RespawnTimer = 0f;

        private float AttackCoolTime = 2f;
        private float AttackTimer = 0f;

        private bool Need_Respawn = false;
        private bool slowEffect = false;
        private bool big = false;
        private bool Chasing = false;
        private bool Once = false;

       
        public Rino(Game1 game, Vector2 init_pos, FlatWorld.Wolrd_layer wolrd_Layer, float aggrodist) :
         base(game, Rino_path, init_pos, Rino_dims, Wolrd_layer.Mob_allias, Rino_speed, 0f, 0f, -1, Rino_Idel_Frame, Rino_MilliFrameRate, Rino_AnimationNum, circleBody: false)
        {
            AddAnimation(new Vector2(6, 1), "Animation\\Rino\\Run", 6, Rino_MilliFrameRate, "Run", 1);
            AddAnimation(new Vector2(5, 1), "Animation\\Rino\\Hit", 5, Rino_MilliFrameRate, "Hit", 2);
        }

        public override void Update(Hero hero)
        {
            base.Update(hero);
            if (!big)
            {

                if (MathF.Abs(hero.FlatBody.LinearVelocity.X) < 0.1f && !Once)
                {

                    game.Dialog_Add("This feels like a trap... there's no way it's this easy.", "Animation\\Rino\\One");
                    Once = true;
                }

                Game1.gamePlay_Status = GamePlay_status.Cinematic_Boss;

                return;
            }

            if (Need_Respawn)
            {
                if (RespawnTimer + RespawnCoolTime < Game1.WorldTimer.Elapsed.TotalSeconds)
                {
                    Need_Respawn = false;
                    FlatVector pos = new FlatVector(Game1.offset.X + TileMap.Tile_Dims.X * 2, hero.FlatBody.Position.Y);
                    flatBody.LinearVelocity = FlatVector.Zero;
                    this.flatBody.MoveTo(pos);
                    ChangeCurrentAnimation(0);

                    AttackTimer = (float)Game1.WorldTimer.Elapsed.TotalSeconds;
                }
                return;
            }

            if (Chasing)
            {
                if (this.flatBody.Position.X > Game1.offset.X + Game1.screen_width)
                {
                    Need_Respawn = true;
                    Chasing = false;
                    FlatUtil.ResetGameSpeed();
                    slowEffect = false;
                }

                else
                {
                    AI(hero);
                    
                }
                return;
            }

            if (AttackTimer + AttackCoolTime < Game1.WorldTimer.Elapsed.TotalSeconds)
            {
                Chasing = true;
                RespawnTimer = (float)Game1.WorldTimer.Elapsed.TotalSeconds;
                this.flatBody.LinearVelocity = new FlatVector(Rino_speed, 0);
                ChangeCurrentAnimation(1);

            }

            else
            {
                this.flatBody.MoveTo(new FlatVector(Game1.offset.X, hero.FlatBody.Position.Y));
            }
        }

        public void Phase2()
        {
            ChangeCurrentAnimation(0);
            Need_Respawn = true;
            RespawnTimer = (float)Game1.WorldTimer.Elapsed.TotalSeconds;
            this.big = true;
            this.dims = Rino_dims2;
  
           
            game.DeleteFlatBody(this, this.flatBody, Wolrd_layer.Mob_allias);
            InitFlatBody(pos, dims, false);
            flatBody.MoveTo(new FlatVector(-1 * dims.X / 4, TileMap.Tile_Dims.Y * 10));
            game.AddFlatBody(this, this.flatBody, Wolrd_layer.Mob_allias);
            flatBody.OnlyHero = true;
        }

       
        public override void AI(Hero hero)
        {
    

            if (hero.FlatBody.Position.X - this.flatBody.Position.X < Rino_dims2.X && !slowEffect)
            {

                slowEffect = true;
                FlatUtil.ChangeGameSpeed(0.5f);
            }

            else if (slowEffect && this.flatBody.Position.X > hero.FlatBody.Position.X)
            {
                FlatUtil.ResetGameSpeed();
                slowEffect = false;
            }
        }

        public override void Interact(Hero hero)
        {
     
       

            if (hero.FlatBody.Position.Y > this.flatBody.Position.Y + Rino_dims2.Y * 0.2f)
            {
                this.flatBody.LinearVelocity = new FlatVector(0f, -1000f);
                ChangeCurrentAnimation(2);
                RespawnTimer = (float)Game1.WorldTimer.Elapsed.TotalSeconds;
             
            }

            else
            {
                
                hero.Get_Hit(-1);
            }

            Need_Respawn = true;
            FlatUtil.ResetGameSpeed();
            slowEffect = false;
            Chasing = false;

        }

        public override void AttackPlayer(Hero hero)
        {
            base.AttackPlayer(-1, hero);
        }

        public override void Draw(Sprites sprite, Vector2 o)
        {

            if (!big)
            {
                Game1.AntiAliasingShader(model, Rino_dims,Rino_Idel_Frame,0);
                base.Draw(sprite,o,0f);
               
            }

            else
            {
                Game1.AntiAliasingShader(model, dims, Animation_Set[currentAnimation].Frames,1);
                base.Draw(sprite, o, 0f);
            }
        }
    }
}