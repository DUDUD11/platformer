using Flat;
using Flat.Graphics;
using FlatPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FlatPhysics.FlatWorld;

namespace ShootingGame
{
    public class MainMenu
    {
        public Button StartButton;
        public Button ExitButton;
        public Button OptionButton;
        public Button MapEditorButton;
        public static Vector2 StartButton_Pos = new Vector2(300, 300);
        public static Vector2 OptionButton_Pos = new Vector2(300, 250);
        public static Vector2 MapEditorButton_Pos = new Vector2(300, 150);
        public static Vector2 ExitButton_Pos = new Vector2(300, 200);

        protected Game1 game;
    
        public Texture2D model;

        public MainMenu(Game1 game,Button startbutton, Button exitbutton) 
        {
            this.StartButton = startbutton;
            this.ExitButton = exitbutton;
            this.OptionButton = new Button(game,true, OptionButton_Pos,StartButton.dims,Text.default_font,"Option",OptionMenuClicked,false);
            this.MapEditorButton = new Button(game, true, MapEditorButton_Pos, startbutton.dims, Text.default_font, "Editor", MapEditorBtnClicked, false);
            this.game = game;
            model = game.Content.Load<Texture2D>("UI\\MainMenuBkg");

            StartButton.active = true;
            ExitButton.active = true;
 
        }

        public void OptionMenuClicked()
        {
            Game1.GameMenuFlag = false;
            Game1.GameOptionFlag = true;
        }

        public void MapEditorBtnClicked()
        {
            Game1.GameMapEditorFlag = true;
            Game1.GameMenuFlag = false;
        }


        public void Update(Vector2 CursorPos)
        { 
            StartButton.ForceUpdate(CursorPos);
            OptionButton.ForceUpdate(CursorPos);
            ExitButton.ForceUpdate(CursorPos);
            MapEditorButton.ForceUpdate(CursorPos);
        }

        public void Draw(Sprites sprite)
        {
            Game1.NoAntiAliasingShader(Color.White);
            sprite.Draw(model, new Rectangle(0, 0, Game1.screen_width, Game1.screen_height), Color.White);

            StartButton.Draw(sprite);
            OptionButton.Draw(sprite);
            ExitButton.Draw(sprite);
            MapEditorButton.Draw(sprite);
            

        }


    }
}
