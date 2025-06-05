////using Flat.Graphics;
////using FlatPhysics;
////using Microsoft.Xna.Framework;
////using System;
////using System.Collections.Generic;
////using System.Linq;
////using System.Text;
////using System.Threading.Tasks;
////using static FlatPhysics.FlatWorld;


////namespace ShootingGame
////{
////    public class Spider : Mob
////    {
////        public static float defaultAngle = MathHelper.PiOver2;
////        public static string Spider_path = "Sprite\\Spider";
////        public static Vector2 Spider_dims = new Vector2(50, 50);
////        public static float Spider_speed = 300f;
////        public static float Spider_atkrange = 100f;
////        public static float Spider_atkdamage = 1f;

////        public Spider(Game1 game, Vector2 init_pos, FlatWorld.Wolrd_layer wolrd_Layer, float aggrodist) : base(game, Spider_path, init_pos, Spider_dims, wolrd_Layer, Spider_speed, aggrodist, Spider_atkrange, Spider_atkdamage)
////        {

////        }


////        public override void Update(Hero hero)
////        {
////            base.Update(hero);


////        }

////        public override void AI(Hero hero)
////        {
////            if (ShorterthanChaseDistance(hero.pos))
////            {
////                FlatVector tmp = new FlatVector(hero.pos.X, hero.pos.Y);


////                FlatVector dir = FlatMath.Normalize(tmp - FlatBody.Position);
////                dir *= mob_Speed;

////                if (FlatMath.Length(FlatBody.LinearVelocity) < Spider_speed || FlatMath.Dot(FlatBody.LinearVelocity, dir) < 0)
////                {
////                    FlatBody.AddForce(dir);
////                }

////                FlatBody.RotateTo(FlatMath.ATAN(dir) + defaultAngle);
////            }
////            /*
////             * attack을 시도하는식으로 가자
////             * 
////            if (ShorterthanAtkDistance(hero.pos))
////            {
////                AttackPlayer(Imp_atkdamage, hero);
////            }


////            */




////        }

////        public override void AttackPlayer(float damage, Hero hero)
////        {
////            base.AttackPlayer(damage, hero);
////        }


////        public override void Draw(Sprites sprite, Vector2 o)
////        {

////            Game1.AntiAliasingShader(model, dims);

////            sprite.Draw(model, new Rectangle((int)(pos.X + o.X), (int)(pos.Y + o.Y), (int)dims.X, (int)dims.Y), Color.White, flatBody.Angle,
////            new Vector2(model.Bounds.Width / 2, model.Bounds.Height / 2));

////        }
////    }
////}


