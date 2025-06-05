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
    // 이게 스테이지에 있다면 Door가 반드시 있다는것
    class BluePoint : Animated2d
    {
    
        public static string BluePoint_path = "Sprite\\BluePoint";
        public static float BluePoint_dims = TileMap.Tile_Size;
        public static int BluePoint_totalframe = 7;
        public static int BluePoint_millitimePerFrame = 100;


        public readonly float msg_livetime = 2f;
        public readonly int BluePoint_point = 1000;
        public FlatBody flatBody;
        private Door door;


        public BluePoint(Game1 game, Vector2 init_pos) : base(game, BluePoint_path, init_pos, new Vector2(BluePoint_dims, BluePoint_dims), FlatWorld.Wolrd_layer.Static_allias, new Vector2(BluePoint_totalframe, 1), 1, BluePoint_totalframe, BluePoint_millitimePerFrame, "Default")
        {
            InitFlatBody(init_pos, new Vector2(BluePoint_dims, BluePoint_dims));

        }


        public void SetDoor(Door door)
        {
            this.door = door;
        }


        private void InitFlatBody(Vector2 pos, Vector2 size)
        {
            if (!FlatBody.CreateCircleBody(size.X * FlatAABB.HitBoxSize,
            2f, true, 0.2f, out FlatBody BluePointBody, out string errorMessage))
            {
                throw new Exception(errorMessage);
            }

            BluePointBody.active = false;
            flatBody = BluePointBody;
            flatBody.MoveTo(pos.X, pos.Y);

        }


        public override void Update(Hero hero)
        {
            base.Update();

            float distance = FlatPhysics.FlatMath.Length(new FlatVector(hero.pos.X - this.pos.X, hero.pos.Y - this.pos.Y));

            if (distance < BluePoint_dims)
            {
                Hero_Reach();
                this.Destroy = true;
            }
        }

        private void Hero_Reach()
        {
            door.Earn_BluePoint();
            this.Destroy = true;
        }

        public override void Draw(Sprites sprite, Vector2 o)
        {
            base.Draw(sprite, o, flatBody.Angle);
        }



    }
}
