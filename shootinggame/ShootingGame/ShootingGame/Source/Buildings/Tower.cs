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
    public class Tower : Building
    {

        public static Vector2 Tower_dims = new Vector2(100, 100);
        public float health;
        public float maxhealth;



        public Tower(Game1 game, Vector2 init_pos, FlatWorld.Wolrd_layer world_layer, float maxhealth) 
            : base(game, "2d\\Tower", init_pos, Tower_dims, world_layer)
        {
            this.maxhealth = maxhealth;
            health = maxhealth;

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


        public override void Update()
        {

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
