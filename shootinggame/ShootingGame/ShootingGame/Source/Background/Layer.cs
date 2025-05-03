using Flat;
using Flat.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingGame
{
    class Layer
    {
        public static int BG_layer_Num = 12;

        Texture2D Texture;
        Vector2 Pos;
        Vector2 SecondPos;
        float MoveScale;
        float depth;

        public Layer(Game1 game,string Layer, float moveScale, int _depth)
        {
            Texture = game.Content.Load<Texture2D>(Layer);
            this.MoveScale = moveScale;
            this.depth =  ((float)_depth / BG_layer_Num);
        }

        public void Draw(Sprites sprite)
        {
            Pos.X = (Game1.offset.X * MoveScale) % Game1.screen_width;
            Pos.Y = -Game1.offset.Y;

            SecondPos.X = Pos.X - Game1.screen_width;
            SecondPos.Y = Pos.Y;

            sprite.Draw(Texture,new Rectangle(Pos.ToPoint(),new Point(Game1.screen_width, Game1.screen_height)),Color.White,depth);
            sprite.Draw(Texture, new Rectangle(SecondPos.ToPoint(), new Point(Game1.screen_width, Game1.screen_height)), Color.White,depth);

        }



    }
}
