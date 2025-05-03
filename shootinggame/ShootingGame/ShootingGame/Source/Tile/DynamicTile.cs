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
using System.Runtime.CompilerServices;
namespace ShootingGame
{
    public class DynamicTile : Animated2d
    {
        public FlatBody flatBody;
        public bool wallJump=false;
        public bool wallBottom=false;
        public bool active = false;  


        public DynamicTile(Game1 game, string path, Vector2 init_pos, Vector2 dims, bool wallJump, bool wallBottom, Vector2 frames, int animationNum, int totalframe, int millitimePerFrame, bool horizontal, bool vertical, string name=null)
            : base(game, path, init_pos, dims, FlatWorld.Wolrd_layer.Static_allias,frames, animationNum, totalframe, millitimePerFrame, name??"Default")
        {
            this.game = game;
            model = game.Content.Load<Texture2D>(path);
            this.pos = init_pos;
            this.dims = dims;
            this.wallJump = wallJump;
            this.wallBottom = wallBottom;
            InitFlatBody(game,pos, dims,horizontal,vertical);
        }

        //public virtual void Interact()
        //{
            
        //}

        public virtual void Interact(SpriteEntity spriteEntity)
        {

        }


        private void InitFlatBody(Game1 game, Vector2 pos, Vector2 dims, bool horizontal, bool vertical)
        {
            if (!FlatBody.CreateBoxBody(dims.X, dims.Y,
            1f, true, 0.5f, out FlatBody TileBody, out string errorMessage))
            {
                throw new Exception(errorMessage);
            }
            flatBody = TileBody;
            TileBody.MoveTo(pos.X, pos.Y);
            TileBody.isTile = true;
            TileBody.isHorizontalTile = horizontal;
            TileBody.isVerticalTile = vertical;
            game.AddBody(TileBody, FlatWorld.Wolrd_layer.Static_allias);
          

        }


        //public override void Update()
        //{
        //    base.Update();
        //}

        public override void Draw(Sprites sprite, Vector2 o, float angle)
        {
            Game1.AntiAliasingShader(model, dims, Animation_Set[currentAnimation].Frames);
            base.Draw(sprite, o, angle);

        }

    }
}
