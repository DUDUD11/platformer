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
    public class AcidSplash : Projectile2d
    {
        public static Vector2 AcidSplash_dims = new Vector2(50, 50);
        public static float AcidSplash_damage = 1f;
        public static string AcidSplash_path = "Projectile\\Acidball";
        public AcidSplash(Game1 game, Vector2 init_pos, SpriteEntity owner, Vector2 projectile_dir, FlatWorld.Wolrd_layer wolrd_Layer) : base(game, AcidSplash_path, init_pos, AcidSplash_dims, owner, projectile_dir, wolrd_Layer)
        {
            this.owner = owner;
            InitFlatBody(init_pos, AcidSplash_dims, owner.velocity.Length(), projectile_dir);
        }

        public AcidSplash(Game1 game, String path, Vector2 init_pos, SpriteEntity owner, Vector2 projectile_dir, st_LiveTime livetime, FlatWorld.Wolrd_layer wolrd_Layer) : base(game, AcidSplash_path, init_pos, AcidSplash_dims, owner, projectile_dir, livetime, wolrd_Layer)
        {
            this.owner = owner;
            InitFlatBody(init_pos, AcidSplash_dims, owner.velocity.Length(), projectile_dir);

        }

        protected void InitFlatBody(Vector2 pos, Vector2 size, float owner_speed, Vector2 dir)
        {
            if (!FlatBody.CreateCircleBody((size.X + size.Y) * FlatAABB.HitBoxSize / 2,
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

        public override void Draw(Sprites sprite, Vector2 o)
        {
            base.Draw(sprite, o);
        }

        public override bool HitSomething(SpriteEntity spriteEntity)
        {
            Destroy = true;

            if (spriteEntity is Mob mob)
            {
                mob.Destroy_Sprite();

                return true;
            }

            else if (spriteEntity is SpawnPoint spawnPoint)
            {
                spawnPoint.GetHit(AcidSplash_damage);
                return true;
            }

            else if (spriteEntity is Hero hero)
            {
                hero.Get_Hit(AcidSplash_damage);
                return true;
            }

            return false;
        }


    }
}



