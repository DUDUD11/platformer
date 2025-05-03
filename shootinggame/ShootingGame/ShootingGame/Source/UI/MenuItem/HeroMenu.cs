using Flat.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingGame
{
    public class HeroMenu : Menu
    {
        public Hero hero;
        public TextZone textZone;
        public static Vector2 TextZoneStart;

        public HeroMenu(Game1 game, Vector2 pos, Vector2 dims, bool Active, Hero hero)
            : base(game, pos, dims, Active)
        { 
            this.hero = hero;
            TextZoneStart = new Vector2(pos.X-dims.X/2+10,pos.Y+dims.Y/2-100);
            textZone = new TextZone(Vector2.Zero, "AAAAAAAAAAAAAA BBBBBBBBB CCCC DDDDDDD EFEFEFEF GGGG", (int)(dims.X * .9f), 22,MenuFont, Color.Gray);
        }


        public override void Draw(Sprites sprites)
        {
            base.Draw(sprites);

            Game1.NoAntiAliasingShader(Color.Black);
            string hero_name = "Hello World";
            Vector2 herodims = MenuFont.MeasureString(hero_name);
            sprites.DrawString(MenuFont, hero_name, new Vector2((int)(pos.X- herodims.X/2),  (int)(pos.Y+dims.Y/2-herodims.Y/2-50f)), Color.Black);

            textZone.Draw(sprites);


        }


    }
}
