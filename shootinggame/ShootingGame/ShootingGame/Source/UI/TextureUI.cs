using Flat.Graphics;
using FlatPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ShootingGame
{
    public class TextureUI : UIEntity
    {

        private Texture2D texture;
        public Vector2 position;
        public Vector2 dims;
        public Color color;
        

        public TextureUI(Game1 game, bool active, string path, Vector2 pos,Vector2 Dims,Color color) : base(game, active)
        {
     
            texture = game.Content.Load<Texture2D>(path);
            position = pos;
            this.dims = Dims;
            this.color = color;

        }

        public override void Update()
        {

        }


        public override void Draw(Sprites sprite, Vector2 o)
        {
            Game1.NoAntiAliasingShader(color);
           
            sprite.Draw(this.texture, new Rectangle((int)(position.X + o.X), (int)(position.Y + o.Y), (int)dims.X, (int)dims.Y), color, new Vector2(texture.Bounds.Width / 2, texture.Bounds.Height / 2));
        }

    }
}
