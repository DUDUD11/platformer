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
    public class SkillButtonSlot
    {
        public TextureUI slotIcon;
        public SkillButton skillButton;
        public static string skill_slotimage = "UI\\SlotImage";
        public static Vector2 skill_slotdims = new Vector2(44, 44);
        private Game1 game;

        public SkillButtonSlot(Game1 game, Vector2 Pos)
        { 
            this.game = game;
            slotIcon = new TextureUI(game,true,skill_slotimage,Pos,skill_slotdims,Color.White);
            skillButton = null;
        }

        public void Update(Vector2 o)
        {
            //slotIcon.update(offset)
            if (skillButton != null)
            {
                skillButton.ForceUpdate(o);
            }
        }

        public void Draw(Sprites sprites, Vector2 o)
        {
            Game1.NoAntiAliasingShader(Color.White);
            slotIcon.Draw(sprites, o);
            if (skillButton != null)
            {
                skillButton.Draw(sprites);
            }
        }

    }
}
