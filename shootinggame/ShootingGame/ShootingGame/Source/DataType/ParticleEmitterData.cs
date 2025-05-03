using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingGame
{
    public class ParticleEmitterData
    {
        public ParticleData particleData;
        public float angle;
        public float angleVar;
        public float lifespanMin;
        public float lifespanMax;
        public float speedMin;
        public float speedMax;
        public float interval;
        public int emitCount;

        public ParticleEmitterData(ParticleData particleData, float angle, float angleVar, float lifespanMin,
            float lifespanMax, float speedMin, float speedMax, float interval, int emitCount)
        {

            this.particleData = particleData;
            this.angle = angle;
            this.angleVar = angleVar;
            this.lifespanMin = lifespanMin;
            this.lifespanMax = lifespanMax;
            this.speedMin = speedMin;
            this.speedMax = speedMax;
            this.interval = interval;
            this.emitCount = emitCount;
        }


    }
}
