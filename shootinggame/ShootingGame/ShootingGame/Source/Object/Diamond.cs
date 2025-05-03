using Flat;
using Flat.Graphics;
using FlatPhysics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ShootingGame.Projectile2d;


namespace ShootingGame
{
    class Diamond : Animated2d
    {
     
        public static string Diamond_path = "Sprite\\Diamond";
        public static float Diamond_dims = TileMap.Tile_Size;
        public static int Diamond_totalframe = 7;
        public static int Diamond_millitimePerFrame = 100;

    
        public readonly float Diamond_respawntime = 2f;

        public FlatBody flatBody;

        private double Respawn_Timer = 0f;
        private bool active = true;


        public Diamond(Game1 game, Vector2 init_pos) : base(game, Diamond_path, init_pos, new Vector2(Diamond_dims, Diamond_dims), FlatWorld.Wolrd_layer.Static_allias, new Vector2(Diamond_totalframe, 1), 1, Diamond_totalframe, Diamond_millitimePerFrame, "Default")
        {
            InitFlatBody(init_pos, new Vector2(Diamond_dims, Diamond_dims));

        }

        private void InitFlatBody(Vector2 pos, Vector2 size)
        {
            if (!FlatBody.CreateCircleBody(size.X * FlatAABB.HitBoxSize,
            2f, true, 0.2f, out FlatBody DiamondBody, out string errorMessage))
            {
                throw new Exception(errorMessage);
            }

            DiamondBody.active = false;

            flatBody = DiamondBody;

            flatBody.MoveTo(pos.X, pos.Y);

        }

        public override void Update(Hero hero)
        {
            base.Update();

            if (!active)
            {
                if (Game1.WorldTimer.Elapsed.TotalSeconds > Respawn_Timer + Diamond_respawntime)
                {
                    active = true;
                }

                return;
            }

            float distance = FlatPhysics.FlatMath.Length(new FlatVector(hero.pos.X - this.pos.X, hero.pos.Y - this.pos.Y));

            if (distance < Diamond_dims)
            {
                Hero_Reach(hero);
               
            }
        }

        private void Hero_Reach(Hero hero)
        {
            active = false;
            hero.dash = 0;
            Respawn_Timer = Game1.WorldTimer.Elapsed.TotalSeconds;
        }

        public override void Draw(Sprites sprite, Vector2 o)
        {
            if (!active)
            {
                return;
            }

            base.Draw(sprite, o, flatBody.Angle);
        }


    }
}

