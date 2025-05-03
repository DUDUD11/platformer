using Flat.Graphics;
using FlatPhysics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FlatPhysics.FlatWorld;

namespace ShootingGame
{
    public class AncientImp : Mob
    {
        public static float defaultAngle = MathHelper.PiOver2;
        public static string AncientImp_path = "Sprite\\ImpBlue";
        public static Vector2 Imp_dims = new Vector2(100, 100);
        public static float Imp_speed = 1000f;
        public static float Imp_atkrange = 1000f;
        public static float Imp_atkdamage = 1f;
        public static float LongRange_atkdamage = 1f;
        public static float LongRange_atkCooldown = 2f;
        public static double LongRange_atktimer;


        public AncientImp(Game1 game, Vector2 init_pos, FlatWorld.Wolrd_layer wolrd_Layer, float aggrodist) : base(game, AncientImp_path, init_pos, Imp_dims, wolrd_Layer, Imp_speed, aggrodist, Imp_atkrange, Imp_atkdamage)
        {
            LongRange_atktimer = Game1.WorldTimer.Elapsed.TotalSeconds;
        }

        public override void Update(Hero hero)
        {
            base.Update(hero);
        }

        public override void AI(Hero hero)
        {
            if (ShorterthanChaseDistance(hero.pos))
            {
                FlatVector tmp = new FlatVector(hero.pos.X, hero.pos.Y);


                FlatVector dir = FlatMath.Normalize(tmp - FlatBody.Position);
                dir *= mob_Speed;

                if (FlatMath.Length(FlatBody.LinearVelocity) < Imp_speed || FlatMath.Dot(FlatBody.LinearVelocity, dir) < 0)
                {
                    FlatBody.AddForce(dir);
                }

                FlatBody.RotateTo(FlatMath.ATAN(dir) + defaultAngle);
            }
         
            if (ShorterthanAtkDistance(hero.pos) && (LongRange_atktimer+LongRange_atkCooldown ) < Game1.WorldTimer.Elapsed.TotalSeconds)
            {
                LongRange_atktimer = Game1.WorldTimer.Elapsed.TotalSeconds;

                AcidSplash acidSplash = new AcidSplash(game, this.pos, this, Vector2.Subtract(hero.pos,this.pos), FlatWorld.Wolrd_layer.Mob_allias);
                game.AddSpriteWithBody(acidSplash, acidSplash.FlatBody, FlatWorld.Wolrd_layer.Mob_allias);
                //fireball.FlatBody.RotateTo(MathF.Atan2(Ejection_dir.Y, Ejection_dir.X) - MathHelper.Pi / 2);

                // AttackPlayer(Imp_atkdamage, hero);
            }
          

        }

        public override void AttackPlayer(float damage, Hero hero)
        {
            base.AttackPlayer(damage, hero);
        }


        public override void Draw(Sprites sprite, Vector2 o)
        {
            Game1.AntiAliasingShader(model, dims);

            sprite.Draw(model, new Rectangle((int)(pos.X + o.X), (int)(pos.Y + o.Y), (int)dims.X, (int)dims.Y), Color.White, flatBody.Angle,
            new Vector2(model.Bounds.Width / 2, model.Bounds.Height / 2));

        }


    }
}


