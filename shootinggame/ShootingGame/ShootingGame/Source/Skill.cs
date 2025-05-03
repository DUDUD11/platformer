using Flat;
using Flat.Graphics;
using Flat.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace ShootingGame
{
    public class Skill
    {
        public Game1 game;
        public bool activate = false;
        public bool released = false;
        public string Name;


        public Effect2d TargetingEffect;
        public Effect2d ReleaseEffect;

        public Skill(Game1 game, Effect2d target, Effect2d Release, string skill_name)
        {
            this.game = game;
            this.TargetingEffect = target;
            this.ReleaseEffect = Release;
            this.Name = skill_name;
        }

        public Skill(Game1 game, Effect2d Release, string skill_name)
        {
            this.game = game;
            this.ReleaseEffect = Release;
            this.Name = skill_name;
        }

        public Skill(Game1 game, bool active, Effect2d target, Effect2d Release, string skill_name)
        {
            TargetingEffect = target;
            ReleaseEffect = Release;
            this.game = game;
            this.activate = active;
            this.Name = skill_name;
        }

        public void Init()
        {

            activate = true;
            
            if (TargetingEffect != null)
            {
                TargetingEffect.CurTime_Reset();
                TargetingEffect.Destroy = false;
            }
            
            ReleaseEffect.Destroy = false;

            //ReleaseEffect.CurTime_Reset();
            //if (TargetingEffect != null)
            //{
            //    game.AddSpriteList(new TargetingCircle(game,AdjustPos,TargetingEffect.dims,TargetingEffect.layertype));
            //}

        }

        public void Finish()
        { 
            activate= false;
            released= false;
        }

        public bool Holding_Target()
        {
            return TargetingEffect != null && !TargetingEffect.Destroy;
        }


        public virtual void Update(Vector2 o)
        {
            throw new NotImplementedException("override skill update");

            //if (active && !done)
            //{
            //    Targeting(o);
            //}
        }

        public virtual void Targeting(Sprites sprite,Vector2 o)
        {
            throw new NotImplementedException("override skill Targeting");

            //if (FlatMouse.Instance.IsLeftMouseButtonReleased())
            //{
            //    Reset();
            //}
        }

        public virtual void Targeting(Vector2 o, List<Mob> mob,Hero hero )
        {
            throw new NotImplementedException("override skill Targeting");

            //if (FlatMouse.Instance.IsLeftMouseButtonReleased())
            //{
            //    Reset();
            //}
        }
   

        public virtual void Activate(Vector2 AdjustPos)
        {
            throw new NotImplementedException("override skill Activate");
        }

        public virtual void Activate()
        {
            throw new NotImplementedException("override skill Activate");
        }

        public virtual void Draw(Sprites sprite, Vector2 o)
        { 
        
        }

    }
}
