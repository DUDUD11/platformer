using Flat;
using Flat.Graphics;
using FlatPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ShootingGame.Source.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ShootingGame.Source.Buildings
{
    public class ArrowTower : Building
    {

        public static Vector2 ArrowTower_dims = new Vector2(80, 80);
        public float health;
        public float maxhealth;
        public static float ArrowTower_Period = 2f;
        public float ArrowTower_range = 1000f;
        private double timer;


        public ArrowTower(Game1 game, Vector2 init_pos, FlatWorld.Wolrd_layer world_layer, float maxhealth)
            : base(game, "2d\\ArrowTower", init_pos, ArrowTower_dims, world_layer)
        {
            this.maxhealth = maxhealth;
            health = maxhealth;
            timer = Game1.WorldTimer.Elapsed.TotalSeconds;
        }

        public void GetHit(float damage)
        {
            health -= damage;
            if (FlatUtil.IsNearlyEqual(health, 0f))
            {
                health = 0;
                Destroy = true;
            }
        }


        public override void Update(List<Mob> mob)
        {
            if (mob.Count <= 0) return;

            double curTime = Game1.WorldTimer.Elapsed.TotalSeconds;

            if (curTime - timer > ArrowTower_Period)
            {
                timer=curTime;

                FireEnemyAndShoot(mob);


            }


        }

        public void FireEnemyAndShoot(List<Mob> mob)
        {
            float dist = float.MaxValue;
            Mob saveMob = null;

            for (int i = 0; i < mob.Count; i++)
            {
                float tmp_dist = Flat.FlatMath.Distance(mob[i].pos, this.pos);

                if (tmp_dist < dist)
                {
                    dist = tmp_dist;
                    saveMob = mob[i];
                }

            }

            if (dist < ArrowTower_range && saveMob != null)
            {    
                Arrow arrow = new Arrow(game, pos, this,Vector2.Subtract(saveMob.pos, pos),FlatWorld.Wolrd_layer.Hero_allias);
                game.AddSpriteWithBody(arrow, arrow.FlatBody, FlatWorld.Wolrd_layer.Hero_allias);
            }
                    
        
        }

        public override bool GetFlatBody(out FlatBody flatBody)
        {
            flatBody = null;
            return false;
        }

        public override void Destroy_Sprite()
        {
            Destroy = true;
        }


        public override void Draw(Sprites sprite, Vector2 offset)
        {
            base.Draw(sprite, offset);
        }

        public override void Attacked(float damagae)
        {
            this.GetHit(damagae);
        }
    }
}
