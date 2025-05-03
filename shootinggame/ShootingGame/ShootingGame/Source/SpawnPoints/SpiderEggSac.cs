
using Flat;
using Flat.Graphics;
using FlatPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;


using static ShootingGame.SpawnPoint;

namespace ShootingGame
{
    public class SpiderEggSac : SpawnPoint
    {

        int MaxSpawn;
        int CurSpawn;


        public SpiderEggSac(Game1 game, string path, Vector2 init_pos, Vector2 dims, FlatWorld.Wolrd_layer wolrd_Layer, float spawnPeriod, float maxHealth, SpawnType spawnType, int MaxSpawn)
        : base(game, path, init_pos, dims, wolrd_Layer, spawnPeriod, maxHealth, spawnType)
        {
            this.MaxSpawn = MaxSpawn;
            CurSpawn = 0;

        }

        public override void SpawnMob()
        {
          
            if (CurSpawn < MaxSpawn)
            {
                Spiderling spiderling = new Spiderling(game, pos, FlatWorld.Wolrd_layer.Mob_allias, float.MaxValue);
                game.AddSpriteWithBody(spiderling, spiderling.FlatBody, FlatWorld.Wolrd_layer.Mob_allias);
            }

            CurSpawn++;

            if(CurSpawn>=MaxSpawn)
            { 
                Destroy = true;
            }
        }



    }
}
