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
    public class StageMenu
    {
        public List<Button> StageButtons;
        public static Vector2 StartButton_Pos = new Vector2(100, 100);
        public static Vector2 StageButton_Dim = new Vector2(50, 50);
        protected Game1 game;
        public Texture2D model;

        public static void StageButtonClick(Object obj)
        {
            if (obj is int stage)
            {
                //OnstageChange(stage);

                Game1.GameStageFlag = false;
            }
            else
            {
                throw new Exception("Object Type for stageButton Wrong");
            }
        }

        public void Set_StageButton(int x)
        {



            for (int i = 0; i < x; i++)
            {
                StageButtons.Add( new Button(game, true, StartButton_Pos * (i+3), StageButton_Dim, Text.default_font, i + 1 + "", StageButtonClick, false, (int)(i + 1)));
            }
        
        }


        public StageMenu(Game1 game, int num, string path=null)
        { 
            this.game = game;
            model = game.Content.Load<Texture2D>("UI\\WorldMap");
            this.StageButtons = new List<Button>();
            Set_StageButton(num);
        }

    

        public void Update(Vector2 CursorPos)
        {
            for (int i = 0; i < StageButtons.Count; i++)
            {
                StageButtons[i].ForceUpdate(CursorPos);
            }

        }

        public void Draw(Sprites sprite)
        {
            Game1.NoAntiAliasingShader(Color.White);
            sprite.Draw(model, new Rectangle(0, 0, Game1.screen_width, Game1.screen_height), Color.White);

            for (int i = 0; i < StageButtons.Count; i++)
            {
                StageButtons[i].Draw(sprite);
            }
        }
    }
}

