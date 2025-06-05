using Flat.Graphics;
using FlatPhysics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using static FlatPhysics.FlatBody;

namespace ShootingGame
{
    public class SpineTile : Trap
    {


        public readonly static Vector2 SpineTile_Frames = new Vector2(1, 1);
        public readonly static string SpineTile_path = "Trap\\spinetile";
        public readonly static Vector2 SpineTile_Dims = TileMap.Tile_Dims;

        public static int Addition_variable = 3; //x, y, bit
        public Vector2[] rect_pos;
        public List<Spine> SpineList;
        public bool UpdateRequired = false;
        public Vector2 Size;

   
        private int spine_dir = 0;

        private float timer = 0f;
        private float activate_time = 2f;


        private int[,] active_square;


        //      1
        //
        //  2       8
        //
        //      4


        // init_pos는 그대로 계산을 해서 건네주고 Diagnoal_pos 에는 크기 위치만을 기입하자
        public SpineTile(Game1 game, Vector2 init_pos, Vector2 Diagnoal_pos, int spine_dir) :
            base(game, SpineTile.SpineTile_path, init_pos, SpineTile_Dims * Diagnoal_pos, 0f, FlatBody.ShapeType.Box, SpineTile.SpineTile_Frames, 0, false, null)
        {
            this.rect_pos = new Vector2[4];
            this.Size = Diagnoal_pos;
            this.spine_dir = spine_dir;

            rect_pos[0] = init_pos;
            rect_pos[2] = init_pos + Diagnoal_pos * SpineTile_Dims;


            rect_pos[1] = new Vector2(rect_pos[2].X, rect_pos[0].Y);
            rect_pos[3] = new Vector2(rect_pos[0].X, rect_pos[2].Y);

            Custom_FlatBody(rect_pos[2] / 2, SpineTile_Dims * Diagnoal_pos);

            SpineList = new List<Spine>();

            active_square = new int[(int)Diagnoal_pos.Y, (int)Diagnoal_pos.X];
         

        }


        private void Custom_FlatBody(Vector2 pos, Vector2 dims)
        {

            if (!FlatBody.CreateBoxBody(dims.X, dims.Y,
            1f, true, 0.5f, out FlatBody TrapBody, out string errorMessage))
            {
                throw new Exception(errorMessage);
            }
            flatBody = TrapBody;


            flatBody.MoveTo((rect_pos[0].X + rect_pos[2].X - TileMap.Tile_Size) / 2, (rect_pos[2].Y + rect_pos[0].Y - TileMap.Tile_Size) / 2);
            this.pos = new Vector2((rect_pos[0].X + rect_pos[2].X - TileMap.Tile_Size) / 2, (rect_pos[2].Y + rect_pos[0].Y - TileMap.Tile_Size) / 2);
        }


      
        public override void Interact(SpriteEntity spriteEntity)
        {
            if (spriteEntity is not Hero hero) return;

            //if (spriteEntity is Hero hero && hero.pos.Y >= (this.pos.Y + dims.Y / 2))

            int Y = (int)((hero.FlatBody.Position.Y - rect_pos[0].Y)/TileMap.Tile_Size);
            int X = (int)((hero.FlatBody.Position.X - rect_pos[0].X) / TileMap.Tile_Size);

            Y = (int)MathHelper.Clamp(Y, 0, Size.Y-1);
            X = (int)MathHelper.Clamp(X, 0, Size.X-1);

            if (Y == Size.Y - 1 && (hero.FlatBody.Position.X > (rect_pos[0].X - TileMap.Tile_Size / 2) && (hero.FlatBody.Position.X < (rect_pos[(int)Size.X - 1].X + TileMap.Tile_Size / 2))))
            {
                hero.bottomReach();
            }

            else if (hero.FlatBody.Position.Y - rect_pos[0].Y > - TileMap.Tile_Size / 2)
            {
                hero.WallReach();
            }


            active_square[Y, X]++;

    
            this.UpdateRequired = true;

            timer = (float)Game1.WorldTimer.Elapsed.TotalSeconds;

            Console.WriteLine(Y + " " + X + " ");

        }

        private Point Clamp_Need(Point tmp)
        { 
            int _Y = (int)MathHelper.Clamp(tmp.Y, 0, Size.Y - 1);
            int _X = (int)MathHelper.Clamp(tmp.X, 0, Size.X - 1);
            return new Point(_Y, _X);
        }


        public override void Update()
        {
            base.Update();


            if (!this.UpdateRequired) return;

            if (timer + activate_time < Game1.WorldTimer.Elapsed.TotalSeconds)
            {
         
                if ((spine_dir & 1) == 1)
                {
                    for (int i = (int)rect_pos[0].X; i < (int)rect_pos[2].X; i += (int)Spine.spine_Dims.X)
                    {
                        Point tmp = new Point((int)(i - rect_pos[0].X) / TileMap.Tile_Size, (int)(rect_pos[2].Y - rect_pos[0].Y) / TileMap.Tile_Size);
                        tmp=Clamp_Need(tmp);

                        //헷갈리게 만들었네
                        if (active_square[tmp.X, tmp.Y] >0)
                        {
                      
                            SpineList.Add(new Spine(game, new Vector2(i, (int)rect_pos[2].Y)));
                        }
                    }
                }

                if ((spine_dir & 2) == 2)
                {
                    for (int i = (int)rect_pos[0].Y; i < (int)rect_pos[2].Y; i += (int)Spine.spine_Dims.Y)
                    {
                        Point tmp = new Point(0, (int)(i - rect_pos[0].Y) / TileMap.Tile_Size);
                        tmp = Clamp_Need(tmp);
                        if (active_square[tmp.X, tmp.Y] > 0)
                        {
                           
                            SpineList.Add(new Spine(game, new Vector2(rect_pos[0].X - (int)Spine.spine_Dims.X, i), MathHelper.PiOver2));
                        }
                    }
                }

                if ((spine_dir & 4) == 4)
                {
                    for (int i = (int)rect_pos[0].X; i < (int)rect_pos[2].X; i += (int)Spine.spine_Dims.X)
                    {
                        Point tmp = new Point((int)(i - rect_pos[0].X) / TileMap.Tile_Size, 0);
                        tmp = Clamp_Need(tmp);
                        if (active_square[tmp.X, tmp.Y] > 0)
                        {
                 
                            SpineList.Add(new Spine(game, new Vector2(i, rect_pos[0].Y - (int)Spine.spine_Dims.Y), MathHelper.PiOver2 * 2));
                        }
                    }
                }

                if ((spine_dir & 8) == 8)
                {
                    for (int i = (int)rect_pos[0].Y; i < (int)rect_pos[2].Y; i += (int)Spine.spine_Dims.Y)
                    {
                        Point tmp = new Point((int)(rect_pos[2].X - rect_pos[0].X) / TileMap.Tile_Size, (int)(i - rect_pos[0].Y) / TileMap.Tile_Size);
                        tmp = Clamp_Need(tmp);
                        if (active_square[tmp.X, tmp.Y] > 0)
                        {
                      
                            SpineList.Add(new Spine(game, new Vector2(rect_pos[2].X, i), MathHelper.PiOver2 * 3));
                        }
                    }
                }

                for (int i = 0; i < Size.Y; i++)
                {
                    for (int j = 0; j < Size.X; j++)
                    {
                        

                        if (active_square[i, j] > 0)
                        {
                            
                            active_square[i, j] = int.MinValue;
                        }
                    }
                }


                for (int h = 0; h < SpineList.Count; h++)
                {
                    game.AddSpriteWithBody(SpineList[h], SpineList[h].flatBody, FlatWorld.Wolrd_layer.Static_allias);
                }

                SpineList.Clear();
                UpdateRequired = false;

            }
        }

        public override void Draw(Sprites sprite, Vector2 o)
        {
            for (int i = 0; i < Size.X; i++)
            {
                for (int j = 0; j < Size.Y; j++)
                {

                    if (UpdateRequired && (timer + activate_time > Game1.WorldTimer.Elapsed.TotalSeconds))
                    {
                        Game1.ThrobShader((float)(Math.Sin((Game1.WorldTimer.Elapsed.TotalSeconds - timer) * MathF.PI / 2)), Color.Red);
                    }
                    //dims 는 다르기때문에 현재껄 사용
                    else
                    {
                        Game1.AntiAliasingShader(model, SpineTile_Dims);
                    }
                    sprite.Draw(model, new Rectangle((int)(rect_pos[0].X+ o.X + i * SpineTile_Dims.X), (int)(rect_pos[0].Y + o.Y + j * SpineTile_Dims.Y), (int)SpineTile_Dims.X, (int)SpineTile_Dims.Y), Color.White,
                    new Vector2(model.Bounds.Width / 2, model.Bounds.Height / 2));
                }

            }
        }

    }
}