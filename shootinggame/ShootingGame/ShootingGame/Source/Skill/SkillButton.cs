using Flat;
using Flat.Graphics;
using Flat.Input;
using FlatPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
namespace ShootingGame
{
    public class SkillButton : Button
    {
        public static Vector2 skillButton_dims = new Vector2(40, 40);

        public SkillButton(Game1 game, string model, Hero hero, bool active, Vector2 pos, bool toggle, Action<object> ButtonClickedObject = null, object info = null) 
            : base(game, model, active, pos, skillButton_dims, Text.default_font, null, ButtonClickedObject??hero.ActivateSkill, false, info)
        {
          
        }

        public override void ForceUpdate(Vector2 CursorPos)
        {
            base.ForceUpdate(CursorPos);
        }

        public override void Update()
        {
            return;
        }

        public override void Draw(Sprites sprite)
        {
            base.Draw(sprite);
            
        }

    }
}
