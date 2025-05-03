using Flat.Graphics;
using Flat.Input;
using FlatPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace ShootingGame
{
    public class Blink : Skill
    {
        public Blink(Game1 game,  BlinkEffect effect, string skill_name) :
          base(game, effect, skill_name)
        {

        }

        public override void Activate(Vector2 AdjustPos)
        {
            Init();
        }

        public override void Targeting(Vector2 mousePos, List<Mob> mobs, Hero h)
        {
            if (!activate) { return; }

            if (!released)
            {
                released = true;
                ReleaseEffect.CurTime_Reset();
                
                FlatVector tmp_dir = FlatPhysics.FlatMath.Normalize(h.FlatBody.LinearVelocity);
                tmp_dir *= 200;

                h.FlatBody.Move(tmp_dir);

 
            }

            ReleaseEffect.Update();

          
            // blink의 경우 위치를 업데이트하지 않고 바로 draw한다.
            //  Released_pos = mousePos;

            if (ReleaseEffect.Destroy)
            {
               Finish();
            }

        }

        public override void Draw(Sprites sprite, Vector2 o)
        {
            if (released)
            {
                ReleaseEffect.Draw(sprite, o);
            }

        }

    }
}




