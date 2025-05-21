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
    public class JumpTrap : Trap
    {
        public readonly static Vector2 JumpTrap_Frames = new Vector2(7, 1);
        public readonly static Vector2 JumpTrap_Dims = TileMap.Tile_Dims * 2;
        public readonly static FlatVector JumpTrap_Force = new FlatVector(0f,400f);
        public readonly static int JumpTrap_millitimeFrame = 100;
        public readonly static string JumpTrap_path = "Trap\\JumpTrap";

        public readonly static float JumpTrap_CoolTime = 0.5f;
        private float CoolTime = 0f;

        public JumpTrap(Game1 game, Vector2 init_pos) : base(game, JumpTrap_path, init_pos+TileMap.Tile_Dims*0.5f, JumpTrap_Dims, 0, ShapeType.Box, JumpTrap_Frames, JumpTrap_millitimeFrame,true,null)
        {

            this.angle = 0f;
        }

        public JumpTrap(Game1 game, Vector2 init_pos,float angle) : base(game, JumpTrap_path, init_pos + TileMap.Tile_Dims * 0.5f ,JumpTrap_Dims, 0, ShapeType.Box, JumpTrap_Frames, JumpTrap_millitimeFrame, true,null)
        {
            this.angle = angle;

        }
        public override void Interact(SpriteEntity spriteEntity)
        {

            if (Game1.WorldTimer.Elapsed.TotalSeconds < JumpTrap_CoolTime + CoolTime)
            {
                return;

            }

            CoolTime = (float)Game1.WorldTimer.Elapsed.TotalSeconds;


            if (spriteEntity is Hero hero)
            {
            

                FlatVector t = JumpTrap_Force + new FlatVector(0, MathF.Abs(hero.FlatBody.LinearVelocity.Y));


                hero.FlatBody.LinearVelocity += t;
            }

            else if (spriteEntity is Mob mob)
            {
                mob.FlatBody.AddForce(JumpTrap_Force);
            }
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Draw(Sprites sprite, Vector2 o)
        {
            base.Draw(sprite, o, angle);
        }



    }
}