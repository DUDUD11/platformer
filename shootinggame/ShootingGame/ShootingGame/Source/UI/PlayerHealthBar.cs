using Flat.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ShootingGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingGame.Source.UI
{
    public class PlayerHealthBar : UIEntity
    {
        public int boarder;
        public TextureUI bar, barBKG;
        public Color color;

      
        public PlayerHealthBar(Game1 game, bool active, Vector2 DIMS, int Boarder, Color color) : base(game,active)
        {
            boarder = Boarder;
            this.color = color;

            bar = new TextureUI(game, true,"UI\\solid",new Vector2(0,0),new Vector2(DIMS.X-boarder*2,DIMS.Y-boarder*2),Color.Red);
            barBKG = new TextureUI(game, true, "UI\\shade", new Vector2(0, 0), new Vector2(DIMS.X, DIMS.Y),Color.White);
        }

        public override void Update(float Current, float Max)
        {
            Current = Math.Max(0f, Current);

            bar.dims = new Vector2(Current/Max*(barBKG.dims.X-boarder*2),bar.dims.Y);

        }


        public override void Draw(Sprites sprite, Vector2 o)
        {
            barBKG.Draw(sprite, o);
            bar.Draw(sprite,new Vector2(o.X+boarder,o.Y+boarder));
        }



    }
}








