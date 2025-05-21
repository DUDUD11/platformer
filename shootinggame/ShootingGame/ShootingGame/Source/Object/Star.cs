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
    class Star : Animated2d
    {
        private Message GetMsg;
        public static string star_path = "Sprite\\Star";
        public static float star_dims = TileMap.Tile_Size;
        public static int star_totalframe = 7;
        public static int star_millitimePerFrame = 100;

        public readonly Vector2 msg_sz = new Vector2(300,100);
        public readonly float msg_livetime = 2f;
        public readonly int star_point = 1000;
        public FlatBody flatBody;

        public Star(Game1 game, Vector2 init_pos) : base(game, star_path, init_pos, new Vector2(star_dims,star_dims), FlatWorld.Wolrd_layer.Static_allias, new Vector2(star_totalframe,1), 1, star_totalframe, star_millitimePerFrame,"Default")
        {
            InitFlatBody(init_pos, new Vector2(star_dims,star_dims));

        }



 
        private void InitFlatBody(Vector2 pos, Vector2 size)
        {
            if (!FlatBody.CreateCircleBody(size.X * FlatAABB.HitBoxSize,
            2f, true, 0.2f, out FlatBody StarBody, out string errorMessage))
            {
                throw new Exception(errorMessage);
            }

            StarBody.active = false;

            flatBody = StarBody;

            flatBody.MoveTo(pos.X, pos.Y);

        }


        public override void Update(Hero hero)
        {
            base.Update();

            float distance = FlatPhysics.FlatMath.Length(new FlatVector(hero.pos.X - this.pos.X,hero.pos.Y-this.pos.Y));

            if (distance < star_dims)
            {
                Hero_Reach();
                this.Destroy = true;
            }
        }

        private void Hero_Reach()
        {
            GetMsg = new Message(game, false, pos-Game1.offset, msg_sz, star_point.ToString(), msg_livetime, Color.OrangeRed);
            game.Add_UIEntityMessage(GetMsg);    
        }

        public override void Draw(Sprites sprite, Vector2 o)
        {
            base.Draw(sprite, o, flatBody.Angle);
        } 
      


    }
}
