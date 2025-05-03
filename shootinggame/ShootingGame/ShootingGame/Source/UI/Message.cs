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
    public class Message : UIEntity
    {
        public Vector2 pos, dims;
        public Color color;
        public TextZone textZone;
        public float LiveTime;
        public double MeasuredTime;
        private SpriteFont font;

        public Message(Game1 game, bool active, Vector2 pos, Vector2 dim
            , string msg, float livetime, Color color) : base(game, active)
        {
            this.pos = pos;
            this.dims = dim;
            this.LiveTime = livetime;
            this.color = color;

            this.font = game.Content.Load<SpriteFont>(Text.default_font);

            MeasuredTime = -1f;
            textZone = new TextZone(pos, msg, (int)(dims.X * 0.9f), 22, font, color);

        }

        public override void ForceUpdate(Vector2 mousePos)
        {
            double curTime = Game1.WorldTimer.Elapsed.TotalSeconds;
            if (MeasuredTime == -1f)
            {
                MeasuredTime = curTime;
            }


            if (MeasuredTime + LiveTime < curTime)
            {
                MeasuredTime = curTime;
                Destory = true;
                textZone.color = color * 0f;
            }

            else
            {
                textZone.color = color * (float)((MeasuredTime+LiveTime - curTime)/LiveTime);
            }


        }

        public override void Draw(Sprites sprite)
        {
            textZone.Draw(sprite);
        }


    }
}
