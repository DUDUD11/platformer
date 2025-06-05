using Flat;
using Flat.Graphics;
using Flat.Input;
using FlatPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ShootingGame.Projectile2d;
using Color = Microsoft.Xna.Framework.Color;

namespace ShootingGame
{
    class CheckPoint : Animated2d
    {
        public static string CheckPoint_path = "Sprite\\CheckPoint";
        public static Vector2 CheckPoint_dims = TileMap.Tile_Dims * 5;
        public static int CheckPoint_totalframe = 7;
        public static int CheckPoint_millitimePerFrame = 100;
        private readonly float Interact_dims = 300f;

        private TextureUI XButton;



        public CheckPoint(Game1 game, Vector2 init_pos) : base(game, CheckPoint_path, init_pos, CheckPoint_dims, FlatWorld.Wolrd_layer.Static_allias, new Vector2(CheckPoint_totalframe, 1), 1, CheckPoint_totalframe, CheckPoint_millitimePerFrame, "Default")
        {
            XButton = new TextureUI(game,false,"UI\\Filled X",init_pos+new Vector2(-CheckPoint_dims.X/2,0f),TileMap.Tile_Dims*2,Color.White);

        }

        public override void Update(Hero hero)
        {
            base.Update();

        

            float distance = FlatPhysics.FlatMath.Length(new FlatVector(hero.pos.X - this.pos.X, hero.pos.Y - this.pos.Y));

            if (distance < Interact_dims && hero.status == Hero.Hero_Status.bottomed)
            {
                Hero_Reach(hero);

                if (FlatKeyboard.Instance.IsKeyClicked(Keys.X))
                {
                    game.Dialog_Add("Congratulation", "Animation\\Hero\\Jump");
                    game.Dialog_Add("You' ve Completed Chapter 1!", "Animation\\Hero\\Jump");
                    game.Dialog_Mode(true);
                }

            }

            else
            {
                XButton.active = false;
            }
        }

        private void Hero_Reach(Hero hero)
        {
            XButton.active = true;
        }

        public override void Draw(Sprites sprite, Vector2 o)
        {
            if (XButton.Active)
            {

                XButton.Draw(sprite,o);
            
            }
            Game1.AntiAliasingShader(model, dims, new Vector2(CheckPoint_totalframe,1),0);
            base.Draw(sprite, o, 0f);
        }






    }
}



