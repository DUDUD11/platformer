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
using System.Text;
using System.Threading.Tasks;



namespace ShootingGame
{
    public class SpriteEntity
    {
        protected Game1 game;
        public readonly FlatWorld.Wolrd_layer layertype;
        public Vector2 pos, dims;
        public Vector2 velocity;
        public float angle;
        public Texture2D model;
        public bool Destroy = false;
       

        public SpriteEntity(Game1 game, string path, Vector2 init_pos, Vector2 dims, Vector2 velocity, FlatWorld.Wolrd_layer wolrd_Layer)
        {
            this.game = game;
            model = game.Content.Load<Texture2D>(path);
            this.pos = init_pos;
            this.dims = dims;
            this.velocity = velocity;
            this.layertype = wolrd_Layer;
            this.angle = 0f;
        }

        public SpriteEntity(Game1 game, string path, Vector2 init_pos, Vector2 dims, FlatWorld.Wolrd_layer wolrd_Layer)
        {
            this.game = game;
            model = game.Content.Load<Texture2D>(path);
            this.pos = init_pos;
            this.dims = dims;
            this.velocity = new Vector2(0,0);
            this.layertype = wolrd_Layer;
            this.angle = 0f;
        }


        public virtual void Update()
        { 
            

        }

        public virtual void Update(List<Mob> mob)
        {


        }
        public virtual void Update(List<Mob> mob,Vector2 AdjustPos)
        {


        }

        public void UpdateModel(string path)
        {
            model = game.Content.Load<Texture2D>(path);
        }

        public Texture2D GetModel()
        {
            return model;
        }



        public virtual bool GetFlatBody(out FlatBody flatBody)
        {
            flatBody = null;
            return false;
        }

        public virtual void Destroy_Sprite()
        {
            Destroy = true;


        }



        public virtual void Update(Hero hero)
        {
            throw new ArgumentNullException("draw method must override ");
        }

        public virtual void Draw(Sprites sprite, Vector2 offset)
        {
            throw new ArgumentNullException("draw method must override "); 
        }

        public virtual void Draw(Sprites sprite, Vector2 offset, float Angle)
        {
            throw new ArgumentNullException("draw method must override ");
        }



    }
}
