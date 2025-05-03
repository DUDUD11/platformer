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
    public class Flamewave : Skill
    {
        public Vector2 Released_pos;

        public Flamewave(Game1 game, TargetingCircle target,FlameCircle effect, string skill_name) :
            base(game, target,effect,skill_name)
        {
      
        }

        public override void Activate(Vector2 AdjustPos)
        {
            Released_pos = Vector2.Zero;
            Init();
        }


        public override void Targeting(Vector2 mousePos, List<Mob> mobs, Hero h)
        {
            if (!activate) { return; }

            if (released)
            {

                for (int i = 0; i < mobs.Count; i++)
                {
                    Vector2 real_pos = Vector2.Add(Released_pos, Game1.offset);

                    if (Flat.FlatMath.Distance(mobs[i].pos, real_pos) < ReleaseEffect.dims.X / 2)
                    {
                        mobs[i].Destroy_Sprite();
                    }
                }

                ReleaseEffect.Update();



                if (ReleaseEffect.Destroy)
                {
                    Finish();
                }
            }
            // Targeting 쿨타임이 있다면
            //else if (TargetingEffect != null)
            //{
            //    TargetingEffect.Update();
            //}

            else if (FlatMouse.Instance.IsLeftMouseButtonReleased())
            {
              
                released = true;
                ReleaseEffect.CurTime_Reset();
                Released_pos = mousePos;

                if (TargetingEffect != null)
                {
                    TargetingEffect.Destroy = true;
                }

                
            }

            else
            {
                TargetingEffect.pos = mousePos;
                TargetingEffect.CurTime_Reset(); // 사실 targeting은 시간 무한대라 필요없음
            }
        }

        public override void Draw(Sprites sprite, Vector2 o)
        {
            if (Holding_Target())
            {
                TargetingEffect.Draw(sprite, Vector2.Zero);
            }

            else if (released)
            {   
                ReleaseEffect.Draw(sprite, Released_pos);
            }

        }

    }
}
