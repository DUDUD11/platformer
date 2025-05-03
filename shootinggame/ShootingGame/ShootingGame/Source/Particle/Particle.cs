using Flat;
using Flat.Graphics;
using FlatPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace ShootingGame
{
    public class Particle
    {
      

        private readonly ParticleData data;
        public Vector2 pos;

        public float lifespanleft;
        public float lifespanAmount;
        public Color color;
        public float opacity;
        public bool finished;
        public float scale;
        public Vector2 origin;
        public Vector2 dir;

        public void init()
        {
            lifespanleft = data.lifespan;
            lifespanAmount = 1f;
            this.color = data.colorstart;
            this.opacity = data.opacityStart;

            finished = false;

            this.origin = new Vector2(data.Texture.Bounds.Width / 2, data.Texture.Bounds.Height / 2);
            dir = new Vector2(MathF.Sin(data.angle), MathF.Cos(data.angle));
        }

        public Particle(Vector2 pos, ParticleData data)
        {
            this.data = data;
            this.pos = pos;

            init();


        }
        public Particle(Vector2 pos, ParticleData data, float lifespan, float speed, float angle)
        {
            this.data = new ParticleData(data, lifespan, speed, angle);
            this.pos = pos;
            init();
        }


        public void Update()
        {
            lifespanleft -= FlatUtil.GetElapsedTimeInSeconds(Game1.WorldGameTime);
            if (lifespanleft < 0f)
            {
                finished = true;
                return;
            }


            lifespanAmount = MathHelper.Clamp(lifespanleft / data.lifespan, 0, 1);
            color = Color.Lerp(data.colorEnd, data.colorstart, lifespanAmount);
            opacity = MathHelper.Clamp(MathHelper.Lerp(data.opacityEnd, data.opacityStart, lifespanAmount),0,1);
            scale = MathHelper.Lerp(data.sizeEnd, data.sizeStart, lifespanAmount);
            pos += dir * data.speed * FlatUtil.GetElapsedTimeInSeconds(Game1.WorldGameTime);

     

        }

        public void Draw(Sprites sprite,Vector2 o)
        {


            Game1.NoAntiAliasingShader(Color.White);  
            sprite.Draw(data.Texture, new Rectangle((int)(pos.X+o.X ), (int)(pos.Y+o.Y ), (int)( scale), (int)( scale)), color * opacity,origin);


        }



    }
}
