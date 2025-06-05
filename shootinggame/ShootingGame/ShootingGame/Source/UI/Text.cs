using Flat.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;


namespace ShootingGame { 
    public  class Text : UIEntity
    {
        public SpriteFont Font;
        private Hero hero;
        public static string default_font = "Fonts\\Arial16";
        public static int MobKilled = 0;

        public Text(Game1 game,  bool active, string spriteFont,Hero hero) : base(game,active)
        {
            Font = game.Content.Load<SpriteFont>(spriteFont);
            this.hero = hero;
        }

        public override void Update()
        {

        }


        public override void Draw(Sprites sprite)
        {
            //Color mobkilled_color = Color.Black;
            
            //Game1.NoAntiAliasingShader(mobkilled_color);


            //string mobkilled = "Num killed = " + MobKilled;
            //Vector2 strDims = Font.MeasureString(mobkilled);
            //sprite.DrawString(Font, mobkilled, new Vector2(Game1.screen_width / 2 - strDims.X / 2, Game1.screen_height / 8), mobkilled_color);

            //if (Game1.GameOver())
            //{
            //    Color restartColor = Color.Red;

            //    Game1.NoAntiAliasingShader(restartColor);

            //    string Gameoverstr = "Press Enter to Restart or Click Button";
            //    Vector2 GameoverDims = Font.MeasureString(Gameoverstr);
            //    sprite.DrawString(Font, Gameoverstr, new Vector2(Game1.screen_width / 2 - GameoverDims.X / 2, Game1.screen_height / 2 - GameoverDims.Y / 2), restartColor);

            //}

            if (Game1.GamePauseFlag)
            {
                Color restartColor = Color.Green;

                Game1.NoAntiAliasingShader(restartColor);

                string Gamepausestr = "Press SpaceBar to resume ";
                Vector2 GamepauseDims = Font.MeasureString(Gamepausestr);
                sprite.DrawString(Font, Gamepausestr, new Vector2(Game1.screen_width / 2 - GamepauseDims.X / 2, Game1.screen_height / 2 - GamepauseDims.Y / 2), restartColor);

            }

        }

    }
}

