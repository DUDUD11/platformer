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
    public class SpawnPoint : SpriteEntity
    {
        public readonly float spawnPeriod;
        public double spawnTimer;
        public float health;
        public float maxHealth;
        public FlatBody flatBody;
        private SpawnType spawnType;

        public enum SpawnType
        { 
            Imp,
            AncientImp,
            Spider,
            Spiderling,
          
        }


        public SpawnPoint(Game1 game, string path, Vector2 init_pos, Vector2 dims, FlatWorld.Wolrd_layer wolrd_Layer, float spawnPeriod, float maxHealth, SpawnType spawnType) : base(game, path, init_pos, dims, wolrd_Layer)
        {
            this.spawnPeriod = spawnPeriod;
            spawnTimer = Game1.WorldTimer.Elapsed.TotalSeconds;
            this.maxHealth = maxHealth;
            health = maxHealth;
            InitFlatBody(init_pos, dims);
            this.spawnType = spawnType;
        }

        public override bool GetFlatBody(out FlatBody flatBody)
        {

            flatBody = this.flatBody;
            return true;
        }

        private void InitFlatBody(Vector2 pos, Vector2 size)
        {
            if (!FlatBody.CreateBoxBody(size.X * FlatAABB.HitBoxSize, size.Y * FlatAABB.HitBoxSize,
            1f, true, 0.5f, out FlatBody SpawnBody, out string errorMessage))
            {
                throw new Exception(errorMessage);
            }

            flatBody = SpawnBody;

            flatBody.MoveTo(pos.X, pos.Y);

        }

        public void GetHit(float damage)
        {
            health -= damage;
            if (FlatUtil.IsNearlyEqual(health,0f))
            {
                health = 0;
                Destroy = true;
            }
        }

        public override void Update()
        {
            base.Update();

            double curtime = Game1.WorldTimer.Elapsed.TotalSeconds;

            if (curtime - spawnTimer > spawnPeriod)
            { 
                spawnTimer = curtime;
                SpawnMob();

            }

        }

        public virtual void SpawnMob()
        {
            if (spawnType == SpawnType.Imp)
            {
                Imp imp = new Imp(game, pos, FlatWorld.Wolrd_layer.Mob_allias, float.MaxValue);
                game.AddSpriteWithBody(imp, imp.FlatBody, FlatWorld.Wolrd_layer.Mob_allias);
            }

            else if (spawnType == SpawnType.Spider)
            {
                Spider spider = new Spider(game, pos, FlatWorld.Wolrd_layer.Mob_allias, float.MaxValue);
                game.AddSpriteWithBody(spider, spider.FlatBody, FlatWorld.Wolrd_layer.Mob_allias);

            }

            else if (spawnType == SpawnType.AncientImp)
            {
                AncientImp ancientImp = new AncientImp(game, pos, FlatWorld.Wolrd_layer.Mob_allias, float.MaxValue);
                game.AddSpriteWithBody(ancientImp, ancientImp.FlatBody, FlatWorld.Wolrd_layer.Mob_allias);

            }

        }

        public override void Draw(Sprites sprite, Vector2 o)
        {

            Game1.AntiAliasingShader(model, dims);

            sprite.Draw(model, new Rectangle((int)(pos.X + o.X), (int)(pos.Y + o.Y), (int)dims.X, (int)dims.Y), Color.White,
               new Vector2(model.Bounds.Width / 2, model.Bounds.Height / 2));
        }

    }
}
