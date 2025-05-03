using Flat.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingGame
{
    public class SkillBar
    {
        public float Spacer;
        public Vector2 FirstPos;
        public int NumSlots;
        private Game1 game;
        public SkillButtonSlot[] slots;
        public SkillBar(Game1 game, Vector2 firstPos, float spacer, int numslots) 
        {
            Spacer = spacer; 
            FirstPos = firstPos;
            NumSlots = numslots;
            this.game = game;

            slots = new SkillButtonSlot[NumSlots];
            for (int i = 0; i < NumSlots; i++)
            {
                slots[i] = new SkillButtonSlot(game, FirstPos);
            }
        }

        public void Update(Vector2 cursorPos)
        {
            for (int i = 0; i < NumSlots; i++)
            {
                slots[i].Update(cursorPos);
            }
        }

        public void Draw(Sprites sprites)
        {
            for (int i = 0; i < NumSlots; i++)
            {
                slots[i].Draw(sprites, new Vector2((Spacer + SkillButtonSlot.skill_slotdims.X) * i, 0));
            }
        }

        public void Set_Skill(int idx, string model, Hero hero, bool active, bool toggle, object info)
        {
            if (idx >= NumSlots)
            {
                throw new Exception("Set skill idx exceed NumSlots");
            }

            slots[idx].skillButton= new SkillButton(game,model,hero,active, new Vector2(FirstPos.X + ((Spacer + SkillButtonSlot.skill_slotdims.X) * idx), FirstPos.Y), toggle, null,info);
         
        }


    }
}
