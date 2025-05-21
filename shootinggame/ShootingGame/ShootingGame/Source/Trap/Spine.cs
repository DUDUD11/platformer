using Flat;
using Flat.Graphics;
using FlatPhysics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FlatPhysics.FlatBody;

namespace ShootingGame
{
    public class Spine : Trap
    {
        public readonly static Vector2 spine_Frames = new Vector2(1, 1);
        public readonly static Vector2 spine_Dims = new Vector2(TileMap.Tile_Dims.X, TileMap.Tile_Dims.Y);
        public readonly static int spine_damage = 100;
        public readonly static string spine_path = "Trap\\static_spine";

        public Spine(Game1 game, Vector2 init_pos) : base(game, spine_path, init_pos, spine_Dims, 0,ShapeType.Box,spine_Frames,-1,false,null)
        {
            this.angle = 0f;
            base.InitFlatBody(new Vector2(init_pos.X, (int)(init_pos.Y - 3* dims.Y / 8)), new Vector2(spine_Dims.X,spine_Dims.Y/2));

        }

        public Spine(Game1 game, Vector2 init_pos,float angle) : base(game, spine_path, init_pos, spine_Dims, 0, ShapeType.Box, spine_Frames, -1,false, null)
        {
            this.angle = angle;

            if (FlatUtil.IsNearlyEqual(angle, MathHelper.PiOver2))
            {
                Vector2 offset = new Vector2(0, -3 * dims.Y / 8); // init_pos 기준의 상대 위치
                Vector2 rotatedOffset = new Vector2(-offset.Y, offset.X); // 90도 회전
                Vector2 result = init_pos + rotatedOffset;
                base.InitFlatBody(result, new Vector2(spine_Dims.Y / 2, spine_Dims.X));
            }


            else if (FlatUtil.IsNearlyEqual(angle, MathHelper.Pi))
            {
                Vector2 offset = new Vector2(0, -3 * dims.Y / 8); // init_pos 기준의 상대 위치
                Vector2 rotatedOffset = new Vector2(offset.X,-offset.Y); // 90도 회전
                Vector2 result = init_pos + rotatedOffset;
                base.InitFlatBody(result, new Vector2(spine_Dims.X, spine_Dims.Y / 2));


            }

            else if (FlatUtil.IsNearlyEqual(angle, 3 * MathHelper.PiOver2))
            {
                Vector2 offset = new Vector2(0, -3 * dims.Y / 8); // init_pos 기준의 상대 위치
                Vector2 rotatedOffset = new Vector2(offset.Y, -offset.X); // 90도 회전
                Vector2 result = init_pos + rotatedOffset;
                base.InitFlatBody(result, new Vector2(spine_Dims.Y / 2, spine_Dims.X));


            }


            else
            {
                base.InitFlatBody(new Vector2(init_pos.X, (int)(init_pos.Y - 3 * dims.Y / 8)), new Vector2(spine_Dims.X, spine_Dims.Y / 2));
            }
        }

        public void Move(FlatVector val)
        {
            this.flatBody.Move(val);
            this.pos += FlatVector.ToVector2(val);
        
        
        }


        public override void Interact(SpriteEntity spriteEntity)
        {
            // 나중에는 brdige처럼 각도도 추가

            if (spriteEntity is Hero hero)
            {
                hero.Get_Hit(spine_damage);
            }

            else if (spriteEntity is Mob mob)
            {
                mob.Destroy_Sprite();
            }
        }


        public override void Update()
        {
            base.Update();
        }

        public override void Draw(Sprites sprite, Vector2 o)
        {
 
            base.Draw(sprite, o,angle);
        }




    }
}
