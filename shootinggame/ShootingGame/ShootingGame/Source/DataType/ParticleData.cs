using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingGame
{
    public class ParticleData
    {
      

        public Texture2D Texture;
        public float lifespan;
        public Color colorstart;
        public Color colorEnd;
        public float opacityStart;
        public float opacityEnd;
        public float sizeStart;
        public float sizeEnd;
        public float speed;
        public float angle;
       

    

        // default value -> 0


        public ParticleData(Texture2D texture, float lifespan, Color colorstart, Color colorend, float opacitystart,float opacityend,
            float sizestart, float sizeend, float speed, float angle = 0f)
        {
            this.Texture = texture == null ? Game1.defaultParticle : null;
            this.lifespan = lifespan == 0 ? 2f : lifespan ;
            this.colorstart = colorstart; 
            this.colorEnd = colorend;
            this.opacityStart = Math.Min(1,opacitystart);
            this.opacityEnd = Math.Max(0,opacityend);
            this.sizeStart = sizestart;
            this.sizeEnd = sizeend;
            this.speed = speed;
            this.angle = angle;
        }

        public ParticleData(ParticleData particleData, float lifespan, float speed, float angle)
        {
            this.Texture = particleData.Texture;
            this.colorstart = particleData.colorstart;
            this.colorEnd = particleData.colorEnd;
            this.opacityStart = particleData.opacityStart;
            this.opacityEnd = particleData.opacityEnd;
            this.sizeStart = particleData.sizeStart;
            this.sizeEnd = particleData.sizeEnd;
            this.lifespan = lifespan;
            this.speed = speed;
            this.angle = angle;
        }

        public ParticleData(ParticleData particleData)
        {
            this.Texture = particleData.Texture;
            this.colorstart = particleData.colorstart;
            this.colorEnd = particleData.colorEnd;
            this.opacityStart = particleData.opacityStart;
            this.opacityEnd = particleData.opacityEnd;
            this.sizeStart = particleData.sizeStart;
            this.sizeEnd = particleData.sizeEnd;
        }

        public ParticleData(Color colorstart, Color colorend, float opacitystart, float opacityend,float sizestart, float sizeend)
        {
        
            this.colorstart = colorstart;
            this.colorEnd = colorend;
            this.opacityStart = Math.Min(1, opacitystart);
            this.opacityEnd = Math.Max(0, opacityend);
            this.sizeStart = sizestart;
            this.sizeEnd = sizeend;
            this.Texture = Game1.defaultParticle;
            this.lifespan = 2f;
            this.speed = 0f;
            this.angle = 0f;
        }




    }
}
