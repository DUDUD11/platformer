
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


namespace ShootingGame
{
    public class Portal : SpawnPoint
    {
        public Portal(Game1 game, string path, Vector2 init_pos, Vector2 dims, FlatWorld.Wolrd_layer wolrd_Layer, float spawnPeriod, float maxHealth, SpawnType spawnType) 
        : base(game, path, init_pos, dims,  wolrd_Layer,spawnPeriod,maxHealth, spawnType)
        {
            

        }



    }
}


