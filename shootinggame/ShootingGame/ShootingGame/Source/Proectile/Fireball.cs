using Flat;
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
    public class Fireball : Projectile2d
    {

        public static Vector2 Fireball_dims = new Vector2(50, 50);
        public static float Fireball_damage = 1f;
        public static string Fireball_path = "Projectile\\Fireball";
       
        public Fireball(Game1 game, Vector2 init_pos, SpriteEntity owner, Vector2 projectile_dir,FlatWorld.Wolrd_layer wolrd_Layer) : base(game, Fireball_path, init_pos, Fireball_dims , owner,projectile_dir,wolrd_Layer)
        {
            this.owner = owner;
            InitFlatBody(init_pos, Fireball_dims, owner.velocity.Length(), projectile_dir);
        }

        public Fireball(Game1 game, String path, Vector2 init_pos, SpriteEntity owner, Vector2 projectile_dir, st_LiveTime livetime, FlatWorld.Wolrd_layer wolrd_Layer) : base(game, Fireball_path, init_pos, Fireball_dims, owner, projectile_dir,livetime,wolrd_Layer)
        {
            this.owner = owner;
            InitFlatBody(init_pos, Fireball_dims, owner.velocity.Length(), projectile_dir);
        
        }

        protected void InitFlatBody(Vector2 pos, Vector2 size, float owner_speed, Vector2 dir)
        {
            if (!FlatBody.CreateCircleBody((size.X+size.Y)*FlatAABB.HitBoxSize / 2 ,
            1f, false, 0.5f, out FlatBody ProjectileBody, out string errorMessage))
            {
                throw new Exception(errorMessage);
            }

            flatBody = ProjectileBody;
            flatBody.MoveTo(pos.X, pos.Y);

            Vector2 normal_dir = Flat.FlatMath.Normalize(dir);
            Vector2 Projectile2d_Velocity = (owner_speed + projectile_speed) * normal_dir;

            flatBody.LinearVelocity = new FlatVector(Projectile2d_Velocity.X, Projectile2d_Velocity.Y);
            this.velocity = Projectile2d_Velocity;


        }

        public override void Update()
        {
            base.Update();
        }

        public override void Draw(Sprites sprite , Vector2 o)
        {
            base.Draw(sprite,o);
        }

        public override bool HitSomething(SpriteEntity spriteEntity)
        {
            Destroy = true;

            if (spriteEntity is Mob mob)
            {
                mob.Destroy_Sprite();

                return true;
            }

            else if (spriteEntity is SpawnPoint spawnPoint) {
                spawnPoint.GetHit(Fireball_damage);
                return true;
            }


            return false;
        }

    }
}

