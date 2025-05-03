using Flat.Graphics;
using Flat;
using FlatPhysics;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ShootingGame
{
    public class StaticTile : SpriteEntity
    {
        public FlatBody flatBody;
        public bool wallJump;
        public bool wallBottom;

        public static string[] bridge_tile_path = new string[] {"Tiles\\Tile_95", "Tiles\\Tile_96", "Tiles\\Tile_97" };

        public StaticTile(Game1 game, string path, Vector2 init_pos, bool wallJump, bool wallBottom)
            :base (game,path,init_pos, TileMap.Tile_Dims, FlatWorld.Wolrd_layer.Static_allias)
        {
            this.game = game;
            model = game.Content.Load<Texture2D>(path);
            this.pos = init_pos;
            this.dims = TileMap.Tile_Dims;
            this.wallJump = wallJump;
            this.wallBottom = wallBottom;
        
        }

        public static bool isBridgeTile(string val)
        {
            for (int i = 0; i < StaticTile.bridge_tile_path.Length; i++)
            {
                if (StaticTile.bridge_tile_path[i].Equals(val))
                {
                    return true;
                 
                }
            }
            return false;
        }
  

        public override void Draw(Sprites sprite, Vector2 o)
        {
            Game1.AntiAliasingShader(model, dims);
            sprite.Draw(model, new Rectangle((int)(pos.X + o.X), (int)(pos.Y + o.Y), (int)dims.X, (int)dims.Y), Color.White, 
             new Vector2(model.Bounds.Width / 2, model.Bounds.Height / 2));
        }
   


    }
}
