using Flat;
using Flat.Graphics;
using FlatPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace ShootingGame
{
    public class TrailPart : SpriteEntity
    {

        public float Lifespan;
        public float rotation;
        private double genTime;
        private Color color = Color.White;

        public Rectangle src_rect;
        public Vector2 sheet;


        public TrailPart(Game1 game, string path, Vector2 init_pos, Vector2 dims, float rot, float lifespan) :
              base(game,path,init_pos,dims,FlatWorld.Wolrd_layer.None_Interact)
          

        {
            genTime = Game1.WorldTimer.Elapsed.TotalSeconds;
            Lifespan = lifespan;
            this.rotation = rot;
        }

        public TrailPart(Game1 game, string path, Vector2 init_pos, Vector2 dims, float rot, float lifespan,Rectangle srcrect,Vector2 sheet) :
      base(game, path, init_pos, dims, FlatWorld.Wolrd_layer.None_Interact)


        {
            genTime = Game1.WorldTimer.Elapsed.TotalSeconds;
            Lifespan = lifespan;
            this.rotation = rot;
            this.src_rect = srcrect;
            this.sheet = sheet;
        }




        public override void Update()
        {

            double lifespan = ((Lifespan+ genTime - Game1.WorldTimer.Elapsed.TotalSeconds) / Lifespan);
            if (lifespan < 0)
            {
                lifespan = 0;
                Destroy = true;
            }

            color = Color.White * (float)lifespan;
        }

        public override void Draw(Sprites sprite, Vector2 o)
        {
            if (Destroy) return;

            Game1.AntiAliasingShader(model, dims);
            sprite.Draw(model, new Rectangle((int)(pos.X + o.X), (int)(pos.Y + o.Y), (int)dims.X, (int)dims.Y), color, rotation,
             new Vector2(model.Bounds.Width / 2, model.Bounds.Height / 2));
        }

        public void AnimationDraw(Sprites sprite, Vector2 o)
        {
           // Game1.NoAntiAliasingShader(Color.Black);

            if(Destroy) return;

            int origin_x = (int)(model.Bounds.Width / (2 * sheet.X));
            int origin_y = (int)(model.Bounds.Height / (2 * sheet.Y));
            sprite.Draw(model, new Rectangle((int)(pos.X + o.X), (int)(pos.Y + o.Y), (int)dims.X, (int)dims.Y), src_rect,color, rotation, new Vector2(origin_x, origin_y));

            /*
                       
            Source_rectangle = new Rectangle((int)(FrameSize.X*sheetFrame.X), (int)(FrameSize.Y * sheetFrame.Y), (int)FrameSize.X, (int)FrameSize.Y);
          
            int origin_x = (int)(model.Bounds.Width / (2 * sheet.X));
            int origin_y = (int)(model.Bounds.Height  / (2 * sheet.Y));
            sprite.Draw(model, rectangle, Source_rectangle, color, Angle,new Vector2(origin_x,origin_y)); 
             */
        }

    }
}
