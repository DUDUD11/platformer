using Flat.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace ShootingGame
{
    public class UIEntity
    {
    
        public bool active;
        public bool Destory = false;

        public bool Active
            { get { return active; } }
        

        public UIEntity(Game1 game, bool active)
        { 
            this.active = active;
        }

        public void Activate(bool flag)
        {
            this.active = flag;
        }

        public virtual void Update()
        { 
        
        }

      

        public virtual void ForceUpdate(Vector2 CursorPos)
        {

        }

        public virtual void Update(float a,float b)
        {

        }


        public virtual void Draw(Sprites sprite, Vector2 o)
        {            
        }

        public virtual void Draw(Vector2 o)
        {
        }


        public virtual void Draw(Sprites sprite)
        {
        }

        public virtual void Draw(Sprites sprite, Color color)
        {
        }

        public virtual bool Hover(Vector2 mousePosition)
        {
            // offset까지 이미 적용된 mouseposition 입니다

            return false;
        }


    }
}
