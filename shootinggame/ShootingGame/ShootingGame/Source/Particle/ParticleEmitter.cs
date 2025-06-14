﻿using Flat;
using Flat.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace ShootingGame
{

    public enum PE_Set
    { 
        Pouring_Circle,
        SnowFlake
    }


    public class ParticleEmitter
    {
        public readonly ParticleEmitterData data;
        public bool Destroy = false;
        private float Left; 
        private float intervalLeft;
        public Vector2 Pos;
        private readonly Vector2 PosVar;
        private bool moveable;
      
        private Game1 game;

        public ParticleEmitter(Game1 game, Vector2 pos, Vector2 posVar,ParticleEmitterData data, bool moveable, float left = float.MaxValue)
        {
            this.game = game;
            this.Pos = pos;
            this.PosVar = posVar;
            this.data = data;
            this.intervalLeft = data.interval;
            this.moveable = moveable;
            this.Left = left;
        }

        private void Emit(Vector2 pos)
        {
        

            ParticleData d = data.particleData;

            float lifespan = RandomHelper.RandomSingle(data.lifespanMin, data.lifespanMax);
            float speed = RandomHelper.RandomSingle(data.speedMin, data.speedMax);
            float r = RandomHelper.RandomSingle() * 2 - 1;
            float posVar_X = (RandomHelper.RandomSingle() * 2 - 1) * PosVar.X;
            float posVar_Y = (RandomHelper.RandomSingle() * 2 - 1) * PosVar.Y;

            Vector2 _pos = pos + new Vector2(posVar_X + posVar_Y);

            float angle = d.angle + data.angleVar * r;

            Particle p = new Particle(_pos, d,lifespan,speed, angle);
            game.Add_Particle(p);
        }


        //pos자체를 변경하지는 않을거임
        public void Update(Vector2 pos)
        {
            float val = FlatUtil.GetElapsedTimeInSeconds(Game1.WorldGameTime);



            intervalLeft -= val;
            Left -= val;

   


            if (Left <= 0f)
            {
             

                this.Destroy = true;
                return;
            }

            while (intervalLeft <= 0f)
            {
                intervalLeft += data.interval;
                for (int i = 0; i < data.emitCount; i++)
                {
                    if (moveable)
                    {
                        Emit(pos);
                    }
                    else
                    {
                        Emit(this.Pos);
                    }
                }
            
            }


        }

     
 



    }
}
