using Flat;
using Flat.Graphics;
using FlatPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static FlatPhysics.FlatWorld;


namespace ShootingGame.Source
{
    public class Building : SpriteEntity
    {

        public FlatBody flatBody;
        public FlatBody FlatBody
        { get { return flatBody; } }


        public Building(Game1 game, string path, Vector2 init_pos, Vector2 dims, FlatWorld.Wolrd_layer world_layer) :base( game,path,init_pos,dims,world_layer)
        {
            InitFlatBody(pos,dims); 
        }

        public void InitFlatBody(Vector2 pos, Vector2 size)
        {
            if (!FlatBody.CreateBoxBody(size.X * FlatAABB.HitBoxSize, size.Y * FlatAABB.HitBoxSize,
            1f, true, 0.5f, out FlatBody BuildingBody, out string errorMessage))
            {
                throw new Exception(errorMessage);
            }

            flatBody = BuildingBody;

            flatBody.MoveTo(pos.X, pos.Y);

        }

        public virtual void Attacked(float damagae)
        { 
        
        }


        public override void Update()
        {

        }

        public override void Update(List<Mob> mob)
        {

        }

        public override bool GetFlatBody(out FlatBody flatBody)
        {
            flatBody = this.flatBody;
            return true;
        }

        public override void Destroy_Sprite()
        {
            Destroy = true;
        }


        public override void Draw(Sprites sprite, Vector2 o)
        {


            Game1.AntiAliasingShader(model, dims);

            sprite.Draw(model, new Rectangle((int)(pos.X + o.X), (int)(pos.Y + o.Y), (int)dims.X, (int)dims.Y), Color.White, flatBody.Angle,
               new Vector2(model.Bounds.Width / 2, model.Bounds.Height / 2));

        }
    }
}
