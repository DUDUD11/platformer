using Flat;
using Flat.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ShootingGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingGame
{
    public class OptionMenu
    {
       // public Button StartButton;
        public Button ExitButton;
    //    public static Vector2 StartButton_Pos = new Vector2(300, 300);
        public static Vector2 ExitButton_Pos = new Vector2(300, 250);
        protected Game1 game;
        public Texture2D model;
        private SpriteFont spriteFont;
        public static Vector2 option_pos = new Vector2(Game1.screen_width/2, Game1.screen_height*3/4);
        public static Vector2 option_substracter = new Vector2(0, 100f);
        public List<ArrowSelector> arrowSelector = new List<ArrowSelector>();
        
        public OptionMenu(Game1 game)
        {
            //    this.StartButton = startbutton;
            this.ExitButton = new Button(game, false, ExitButton_Pos, Button.Default_Buttonsz, Text.default_font, "Back", ExitClick, false);
            this.game = game;
            model = game.Content.Load<Texture2D>("UI\\solid");
            spriteFont = game.Content.Load<SpriteFont>(Text.default_font);
            

          //  StartButton.active = true;
            ExitButton.active = true;
          
            arrowSelector.Add(new ArrowSelector(game, option_pos, new Vector2(128,32), "Full Screen",ArrowSelector.UI_TYPE.CheckBox,false));
            arrowSelector[0].AddOption(new FormOption("NO", 0));
            arrowSelector[0].AddOption(new FormOption("YES", 1));


            arrowSelector.Add(new ArrowSelector(game, option_pos-option_substracter, new Vector2(128, 32), "Music Volume", ArrowSelector.UI_TYPE.Button));
            for (int i = 0; i <= 20; i++)
            {
                arrowSelector[1].AddOption(new FormOption(""+i, i));
            }

            arrowSelector[1].ArrowSelectedCenter();

            arrowSelector.Add(new ArrowSelector(game, option_pos-(2*option_substracter), new Vector2(128, 32), "Sound Volume", ArrowSelector.UI_TYPE.Button));
            for (int i = 0; i <= 20; i++)
            {
                arrowSelector[2].AddOption(new FormOption("" + i, i));
            }
            arrowSelector[2].ArrowSelectedCenter();

        }

    

        public void ExitClick()
        { 
            Game1.GameOptionFlag = false;
            Game1.GameMenuFlag = true;

            obj_tofloat obj = new obj_tofloat(1f*arrowSelector[1].selected / arrowSelector[1].options.Count, 1f*arrowSelector[2].selected/ arrowSelector[2].options.Count);
            SoundControl.OptionMenuSoundVolume(obj);

            if (arrowSelector[0].selected == 0)
            {
                Game1.graphics.IsFullScreen = false;
            }

            else
            {
                Game1.graphics.IsFullScreen = true;
            }

            Game1.graphics.ApplyChanges();

        }

        public void Update(Vector2 CursorPos)
        {
     //       StartButton.ForceUpdate(CursorPos);
            ExitButton.ForceUpdate(CursorPos);

            for (int i = 0; i < arrowSelector.Count; i++)
            {
                arrowSelector[i].Update();
            }
          

        }

        public void Draw(Sprites sprite,Vector2 o)
        {
            Game1.NoAntiAliasingShader(Color.White);
            Vector2 strDims = spriteFont.MeasureString("Option");
            sprite.Draw(model, new Rectangle(0, 0, Game1.screen_width, Game1.screen_height), Color.White);
            sprite.DrawString(spriteFont, "Option", new Vector2((int)(option_pos.X-strDims.X/2), (int)(option_pos.Y-strDims.Y/2+100f)),Color.Blue);
       //     StartButton.Draw(sprite);
            ExitButton.Draw(sprite);

            for (int i = 0; i < arrowSelector.Count; i++)
            {
                arrowSelector[i].Draw(sprite, o,spriteFont);
            }

        }




    }
}

