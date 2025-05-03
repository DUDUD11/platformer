using Flat.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingGame
{
    public class Trail
    {
   
        private Game1 game;
        public List<TrailPart> _trail;
        public float LIFESPAN;
        public float TrailGenPeriod;
        public double TrailGenTime;
        public Vector2 dims;

        public Trail(Game1 game, float lifespan,Vector2 dims, float TrailGenPeriod)
        { 
            this.game = game;   
            this.LIFESPAN = lifespan;
            this.dims = dims;
            this.TrailGenPeriod = TrailGenPeriod;
            this.TrailGenTime = Game1.WorldTimer.Elapsed.TotalSeconds;
            this._trail = new List<TrailPart>();
        }


        private void AddTrail(string model, Vector2 position, float rotation)
        {
            _trail.Add(new TrailPart(game,model,position,dims,rotation,LIFESPAN));
        }

        private void AddAniTrail(string model, Vector2 position, float rotation,Rectangle src_rect, Vector2 sheet)
        {
            _trail.Add(new TrailPart(game, model, position, dims, rotation, LIFESPAN,src_rect,sheet));
        }


        public void Update(string model ,Vector2 position, float rotation)
        {
            UpdateTrail();
            if (Game1.WorldTimer.Elapsed.TotalSeconds > (TrailGenTime+TrailGenPeriod))
            {
                TrailGenTime = Game1.WorldTimer.Elapsed.TotalSeconds;
                AddTrail(model, position, rotation);
            }
        }

        public void Update(string model, Vector2 position, float rotation,Rectangle src_rect,Vector2 sheet)
        {
            UpdateTrail();
            if (Game1.WorldTimer.Elapsed.TotalSeconds > (TrailGenTime + TrailGenPeriod))
            {
                TrailGenTime = Game1.WorldTimer.Elapsed.TotalSeconds;
                AddAniTrail(model, position, rotation,src_rect,sheet);
            }
        }

        private void UpdateTrail()
        {
            for (int i = 0; i < _trail.Count; i++)
            {
                _trail[i].Update();
            }

            _trail.RemoveAll(p => p.Destroy);
        }

        public void Draw(Sprites sprite, Vector2 o)
        {
            for (int i = 0; i < _trail.Count; i++)
            {
                _trail[i].Draw(sprite, o);
            }
        }

        public void AnimationDraw(Sprites sprite, Vector2 o)
        {
            for (int i = 0; i < _trail.Count; i++)
            {
                _trail[i].AnimationDraw(sprite, o);
            }
        }

    }
}
