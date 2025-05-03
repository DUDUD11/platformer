using Flat.Graphics;
using Flat.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ShootingGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ShootingGame
{
    public class ExitMenu : Menu
    {
        public List<Button> buttons = new List<Button>();
        public List<ArrowSelector> arrowSelector = new List<ArrowSelector>();


        public ExitMenu(Game1 game, Vector2 pos, Vector2 dims, bool Active)
        : base(game, pos, dims, Active)
        {
            Vector2 init_button_pos = new Vector2(pos.X,pos.Y+dims.Y/2-100f);

            buttons.Add(new Button(game, true, init_button_pos, new Vector2(150, 40), Text.default_font, "Return", Return,false));
            buttons.Add(new Button(game, true, new Vector2(init_button_pos.X,init_button_pos.Y-50f), new Vector2(150, 40), Text.default_font, "Exit Level", ExitLevel, false));

            Vector2 option_pos = new Vector2(init_button_pos.X + 150f, init_button_pos.Y - 100f);
            Vector2 subset = new Vector2(0, 50f);

            arrowSelector.Add(new ArrowSelector(game, option_pos, new Vector2(128, 32), "Left", ArrowSelector.UI_TYPE.Input, KeyBindControl.Keyval(KeyBindControl.keys[0].key).ToString(),FirstKeyFunc));
            arrowSelector[0].AddOption(new FormOption("", 0));

            arrowSelector.Add(new ArrowSelector(game, option_pos -= subset, new Vector2(128, 32), "Right", ArrowSelector.UI_TYPE.Input, KeyBindControl.Keyval(KeyBindControl.keys[1].key).ToString(), SecondKeyFunc));
            arrowSelector[1].AddOption(new FormOption("", 0));

            arrowSelector.Add(new ArrowSelector(game, option_pos -= subset, new Vector2(128, 32), "Top", ArrowSelector.UI_TYPE.Input, KeyBindControl.Keyval(KeyBindControl.keys[2].key).ToString(), ThirdKeyFunc));
            arrowSelector[2].AddOption(new FormOption("", 0));

            arrowSelector.Add(new ArrowSelector(game, option_pos -= subset, new Vector2(128, 32), "Down", ArrowSelector.UI_TYPE.Input, KeyBindControl.Keyval(KeyBindControl.keys[3].key).ToString(), ForthKeyFunc));
            arrowSelector[3].AddOption(new FormOption("", 0));

        }

        #region func

        public void FirstKeyFunc()
        {
            if (!arrowSelector[0].checkBox.isHovered) return;
            foreach (var key in KeyBindControl.keysToCheck)
            {
                if (FlatKeyboard.Instance.IsKeyDown(key))
                {
                    if (KeyBindControl.ChangeKey("Left", (int)key))
                    {
                        arrowSelector[0].SetInput(key.ToString());
                    }
                    break;

                }
            }
        }

        public void SecondKeyFunc()
        {
            if (!arrowSelector[1].checkBox.isHovered) return;
            foreach (var key in KeyBindControl.keysToCheck)
            {
                if (FlatKeyboard.Instance.IsKeyDown(key))
                {
                    if (KeyBindControl.ChangeKey("Right", (int)key))
                    {
                        arrowSelector[1].SetInput(key.ToString());
                    }
                    break;

                }
            }
        }

        public void ThirdKeyFunc()
        {
            if (!arrowSelector[2].checkBox.isHovered) return;
            foreach (var key in KeyBindControl.keysToCheck)
            {
                if (FlatKeyboard.Instance.IsKeyDown(key))
                {
                    if (KeyBindControl.ChangeKey("Top", (int)key))
                    {
                        arrowSelector[2].SetInput(key.ToString());
                    }
                    break;

                }
            }
        }

        public void ForthKeyFunc()
        {
            if (!arrowSelector[3].checkBox.isHovered) return;
            foreach (var key in KeyBindControl.keysToCheck)
            {
                if (FlatKeyboard.Instance.IsKeyDown(key))
                {
                    if (KeyBindControl.ChangeKey("Down", (int)key))
                    {
                        arrowSelector[3].SetInput(key.ToString());
                    }
                    break;

                }
            }
        }


        #endregion



        public void Return()
        {
            this.active = false;    
        }

        public void ExitLevel()
        {
            this.active = false;
            Game1.GameStageFlag = true;
        }
        public override void Update()
        {
            if (active)
            {
                base.Update();
                for (int i = 0; i < buttons.Count; i++)
                {
                    buttons[i].ForceUpdate(Game1.adjustmousePos);
                }

                for (int i = 0; i < arrowSelector.Count; i++)
                {
                    arrowSelector[i].Update();
                    if (arrowSelector[i].TYPE == ArrowSelector.UI_TYPE.Input)
                    {
                        if (arrowSelector[i].UpdateNextFrame)
                        {
                            arrowSelector[i].ActionInput();
                        }
                    }
                }


            }
        }


        public override void Draw(Sprites sprites)
        {
            if (active)
            {
                base.Draw(sprites);

                Game1.NoAntiAliasingShader(Color.Black);
                for (int i = 0; i < buttons.Count; i++)
                {
                    buttons[i].Draw(sprites);
                }

                for (int i = 0; i < arrowSelector.Count; i++)
                {
                    arrowSelector[i].Draw(sprites, Vector2.Zero );
                }
            }

         

         //   Game1.NoAntiAliasingShader(Color.Black);
         //   string hero_name = "Hello World";
         //   Vector2 herodims = MenuFont.MeasureString(hero_name);
           // sprites.DrawString(MenuFont, hero_name, new Vector2((int)(pos.X - herodims.X / 2), (int)(pos.Y + dims.Y / 2 - herodims.Y / 2 - 50f)), Color.Black);

        }




    }
}
/*
public class HeroMenu : Menu
{
    public Hero hero;

    public HeroMenu(Game1 game, Vector2 pos, Vector2 dims, bool Active, Hero hero)
        : base(game, pos, dims, Active)
    {
        this.hero = hero;
        TextZoneStart = new Vector2(pos.X - dims.X / 2 + 10, pos.Y + dims.Y / 2 - 100);
        textZone = new TextZone(Vector2.Zero, "AAAAAAAAAAAAAA BBBBBBBBB CCCC DDDDDDD EFEFEFEF GGGG", (int)(dims.X * .9f), 22, MenuFont, Color.Gray);
    }


    public override void Draw(Sprites sprites)
    {
        base.Draw(sprites);

        Game1.NoAntiAliasingShader(Color.Black);
        string hero_name = "Hello World";
        Vector2 herodims = MenuFont.MeasureString(hero_name);
        sprites.DrawString(MenuFont, hero_name, new Vector2((int)(pos.X - herodims.X / 2), (int)(pos.Y + dims.Y / 2 - herodims.Y / 2 - 50f)), Color.Black);

        textZone.Draw(sprites, TextZoneStart);


    }


}
*/