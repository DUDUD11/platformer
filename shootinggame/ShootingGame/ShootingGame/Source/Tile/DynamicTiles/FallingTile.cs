using Flat.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ShootingGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlatPhysics;

namespace ShootingGame
{
    public class FallingTile : DynamicTile
    {


        public FallingTile(Game1 game, string path, Vector2 init_pos, Vector2 dims, bool WallJump, bool WallBottom)
            : base(game, path, init_pos, dims, WallJump, WallBottom, new Vector2(1, 1), 1, 1, 100,true,false)
        { 
            
    
        }



        public override void Interact(SpriteEntity spriteEntity)
        {
            if (active)
            {
                if (spriteEntity is Hero hero)
                {
                    if (hero.status == Hero.Hero_Status.bottomed)
                    {
                        hero.Destroy = true;

                    }
                
                }

            }

            else
            {
                this.active = true;
            }
        }

        public override void Update()
        {
            if (!active) return;

            if (flatBody.isStatic)
            {
                flatBody.isStatic = false;
            }
            this.pos = FlatVector.ToVector2(flatBody.Position);



            base.Update();
      
        }

        public override void Draw(Sprites sprite, Vector2 o, float angle)
        {
            base.Draw(sprite, o, angle);

        }



    }


}

