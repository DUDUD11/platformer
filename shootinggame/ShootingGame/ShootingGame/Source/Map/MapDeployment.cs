using FlatPhysics;
using Microsoft.Xna.Framework;
using ShootingGame;

using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShootingGame
{
    public class MapDeployment
    {
        private Map map;
        private bool[,] chk;
        private Game1 game;
        private bool Map_Changed;
        private Vector2 Map_Sz;
       
      

        public MapDeployment()
        { 
            
        }




        public void init(Game1 game)
        {
            this.game = game;
        }

        public Vector2 currentMapSize()
        {
            if (Map_Changed)
            {
                Map_Sz = map.MapSize();
            }

            return Map_Sz;
        }

        public string NextMap(int dir)
        {
            if (dir < 0 || dir > 3) throw new Exception("Invalid NextMap setting");
            string _map = map.NextMap(dir);
         
            return _map;
        }


        public bool Load_Map(int stage, int substage)
        {
            string tmp = "Map" + stage + "-" + substage;

            map = Game1.save.LoadMapData(tmp);
            Map_Changed = true;

            chk = new bool[map.Map_height, map.Map_width];

            if (map == null) return false;

            Parsing_map();
            return true;
        }

        public bool Load_Map(string tmp)
        {
       
            map = Game1.save.LoadMapData(tmp);
            Map_Changed = true;

            chk = new bool[map.Map_height, map.Map_width];

            if (map == null) return false;

            Parsing_map();
            return true;
        }

        public void Parsing_map()
        {
            StaticTile_Width();
            StaticTile_Height();
            Trap_Gen();
            Hero_Gen();
            Env_Gen();
        }

        public void StaticTile_Width()
        {
            List<string> path_tmp = new List<string>();

            for (int i = 0; i < map.Map_height; i++)
            {
                for (int j = 0; j < map.Map_width -1 ; j++) // 끝은 처리하지않고 만약 1x1 타일이라면 height 타일로 처리합시다.
                {
                    if (!chk[i, j] && map.Deploy[i][j].Item1 == (int)D1.Tile)
                    {
                        path_tmp.Clear();
                        int len = 0;

                        for (int h = j; h < map.Map_width; h++)
                        {
                            //처리
                            if (!chk[i, h] && map.Deploy[i][h].Item1 == (int)D1.Tile)
                            {
                                len++;
                         
                                path_tmp.Add(Map.GetTexture(map.Deploy[i][h]));
                            }
                            else break;
 
                        }
                        if (len >= 2)
                        {

                            for (int t = j; t <j+len;t++)
                            {
                                chk[i, t] = true;
                            }

                            TileMap.Add_StaticTiles_Horizontal(game, new Vector2(j * TileMap.Tile_Size,i * TileMap.Tile_Size), len, path_tmp);
                        }
                    }
                
                
                }
            
            
            }

            //TileMap.Add_DynamicTile(new VanishingTile(game, new Vector2(700, 500), new Vector2(256, 128), 200));
            //TileMap.Add_DynamicTile(new FallingTile(game, "Tiles\\Tile_09", new Vector2(1100, 500), new Vector2(128, 128), true, false));
        }

        public void StaticTile_Height()
        {
            List<string> path_tmp = new List<string>();

            for (int i = 0; i < map.Map_height; i++)
            {
                for (int j = 0; j < map.Map_width; j++)
                {
                   

                    if (!chk[i, j] && map.Deploy[i][j].Item1 == (int)D1.Tile)
                    {
               

                        path_tmp.Clear();
                        int len = 0;

                        for (int h = i; h < map.Map_height; h++)
                        {
                      

                            //처리
                            if (!chk[h, j] && map.Deploy[h][j].Item1 == (int)D1.Tile)
                            {
                                len++;
                                chk[h, j] = true;
                                path_tmp.Add(Map.GetTexture(map.Deploy[h][j]));
                            }
                            else break;

                        }

                       

                        TileMap.Add_StaticTiles_Vertical(game, new Vector2(j * TileMap.Tile_Size, i * TileMap.Tile_Size), len, path_tmp);
                    }


                }




            }



        }

        public void Trap_Gen()
        {


            for (int i = 0; i < map.Map_height; i++)
            {
                for (int j = 0; j < map.Map_width; j++) // 끝은 처리하지않고 만약 1x1 타일이라면 height 타일로 처리합시다.
                {

                

                    if (!chk[i, j] && map.Deploy[i][j].Item1 == (int)D1.Trap)
                    {
                        chk[i, j] = true;
                        Vector2 init_pos = new Vector2(j * TileMap.Tile_Size + TileMap.Tile_Size / 2, i * TileMap.Tile_Size + TileMap.Tile_Size / 2);

                        switch ((int)map.Deploy[i][j].Item2)
                        {
                         
                            case 1:

                                // 현재 정사각형이기 때문에 타일생성시 회전을 처리하고 있지는 않다.

                                if (map.DeployInfo[i][j] != null)
                                {
                                    int jumpangle = JsonSerializer.Deserialize<int>((JsonElement)map.DeployInfo[i][j]);
                                    Trap jump = new JumpTrap(game, init_pos,jumpangle*MathHelper.PiOver2);
                                    game.AddSpriteWithBody(jump, jump.flatBody, FlatWorld.Wolrd_layer.Static_allias);
                                }
                                else
                                {
                                    Trap jump = new JumpTrap(game, init_pos);
                                    game.AddSpriteWithBody(jump, jump.flatBody, FlatWorld.Wolrd_layer.Static_allias);
                                }
                                break;
                            case 2:
                               
                                int[] MovingPlatformarr = JsonSerializer.Deserialize<int[]>((JsonElement)map.DeployInfo[i][j]);
                                Trap MovingPlatform = new MovingPlatForm(game, init_pos, init_pos + new Vector2(MovingPlatformarr[0]*TileMap.Tile_Size, MovingPlatformarr[1]*TileMap.Tile_Size), 1f);
                                game.AddSpriteWithBody(MovingPlatform, MovingPlatform.flatBody, FlatWorld.Wolrd_layer.Static_allias);
                                break;
                            case 3:
                          
                                int[] SawBladearr = JsonSerializer.Deserialize<int[]>((JsonElement)map.DeployInfo[i][j]);
                                Trap SawBlade1 = new SawBlade(game, init_pos, init_pos + new Vector2(SawBladearr[0]*TileMap.Tile_Size, SawBladearr[1]*TileMap.Tile_Size));
                                game.AddSpriteWithBody(SawBlade1, SawBlade1.flatBody, FlatWorld.Wolrd_layer.Static_allias);
                                break;
                            case 4:

                                

                                if (map.DeployInfo[i][j] != null)
                                {
                                    int Spineangle = JsonSerializer.Deserialize<int>((JsonElement)map.DeployInfo[i][j]);
                                    Console.WriteLine(Spineangle);


                                    Trap spine1 = new Spine(game, init_pos,Spineangle * MathHelper.PiOver2);
                                    game.AddSpriteWithBody(spine1, spine1.flatBody, FlatWorld.Wolrd_layer.Static_allias);

                                }
                                else
                                {
                                    Trap spine1 = new Spine(game, init_pos);
                                    game.AddSpriteWithBody(spine1, spine1.flatBody, FlatWorld.Wolrd_layer.Static_allias);
                                }
                                break;
                            case 5:
                                TileMap.Add_DynamicTile(new VanishingTile(game, init_pos, new Vector2(256, 128), 100));
                                break;
                            case 6:
                                int[] SpineMovingPlatformarr = JsonSerializer.Deserialize<int[]>((JsonElement)map.DeployInfo[i][j]);

                                SpineMovingPlatform SpineMovingPlatform = new SpineMovingPlatform(game, init_pos, init_pos + new Vector2(SpineMovingPlatformarr[0] * TileMap.Tile_Size, SpineMovingPlatformarr[1] * TileMap.Tile_Size), 1f, Convert.ToInt32(SpineMovingPlatformarr[2].ToString(),2));
                                game.AddSpriteWithBody(SpineMovingPlatform, SpineMovingPlatform.flatBody, FlatWorld.Wolrd_layer.Static_allias);
                                for (int h = 0; h < SpineMovingPlatform.SpineList.Count; h++)
                                {
                                    game.AddSpriteWithBody(SpineMovingPlatform.SpineList[h], SpineMovingPlatform.SpineList[h].flatBody, FlatWorld.Wolrd_layer.Static_allias);
                                }

                                break;

                            default:
                                throw new Exception("Error!");


                           

                        }
                           
      
                    }


                }


            }




        }

        public void Hero_Gen()
        { 
        
        }

        public void Env_Gen()
        {
            for (int i = 0; i < map.Map_height; i++)
            {
                for (int j = 0; j < map.Map_width; j++) // 끝은 처리하지않고 만약 1x1 타일이라면 height 타일로 처리합시다.
                {
                    if (!chk[i, j] && map.Deploy[i][j].Item1 == (int)D1.Env)
                    {
                        chk[i, j] = true;
                        Vector2 init_pos = new Vector2(j * TileMap.Tile_Size + TileMap.Tile_Size / 2, i * TileMap.Tile_Size + TileMap.Tile_Size / 2);

                        switch ((int)map.Deploy[i][j].Item2)
                        {

                            case 1:

                                    Star star = new Star(game, init_pos);                             
                              
                                    game.AddSpriteWithBody(star,star.flatBody,FlatWorld.Wolrd_layer.Static_allias);
                        
                                break;
                            case 2:

                                Diamond diamond = new Diamond(game, init_pos);

                                game.AddSpriteWithBody(diamond, diamond.flatBody, FlatWorld.Wolrd_layer.Static_allias);

                                break;



                            default:
                                throw new Exception("Error!");




                        }


                    }


                }


            }

        }



    }
}
