using Flat.Graphics;
using Flat.Input;
using FlatPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ShootingGame
{

    // 데이터 추가시 수정해야할 목록
    // Map.GetTexture
    // Map.enum 값
    // Map editor 초기화 데이터에 작업
    // Update 조건문 확인 MAIN문
    // infookbutton
    // trap_gen 등의 gen
    // rightbutton

    public class MapEditor
    {

        private enum buttons
        {
            DeleteButton,
            SaveButton,
            LoadButton,
            ExitButton,
            RevButton,
            MapSizeChangeButton,
          //  RbCancelButton,
          //  RbOkButton,
            LeftButton,
            RightButton,
            UpButton,
            DownButton,
        };

        private string[] buttons_str = { 
            "Clean",
            "Save",
            "Load",
            "Back",
            "Rev",
            "MpCg",
           // "Cancel",
         //   "OK",
            "Left",
            "Right",
            "Up",
            "Down",
 
        };

        public Action[] buttons_action;
        public Button[] Buttons;

        public TextInputBox MapNameInput;
        public TextInputBox MapWidthSizeInput;
        public TextInputBox MapHeightSizeInput;

        private TextInputBox[] RbInput;
        private const int RbInput_Cnt = 10;

        public Button RbOkButton;
        public Button RbCancleButton;
        public Button MapSizeChangeButton;

        private Vector2 Button_Dims = new Vector2(30, 30);
        private Vector2 CurPos = new Vector2(50, 50);

        public List<Button> StaticTilesMapEditor = new List<Button>();
        public List<Button> TrapMapEditor = new List<Button>();
        public List<Button> EnvMapEditor = new List<Button>();


        private Map CurrentMap;
        private Tuple<int,int> CurSelected = Tuple.Create(0, 0);
        private Vector2 CurDeploy = new Vector2(-1,0);
       
        private Save save;

        public static bool Reverse = false;
        protected Game1 game;
        public Texture2D model;

        private bool RbClicked = false;
        private Vector2 offset = Vector2.Zero;

        public MapEditor(Game1 game)
        {
            this.game = game;
            CurrentMap = new Map(Tuple.Create(0,0),Tuple.Create(0,0));
            save = new Save(game);
            RbInput = new TextInputBox[RbInput_Cnt];
            Buttons = new Button[Enum.GetValues(typeof(buttons)).Length];
            buttons_action = new Action[] { DeleteButtonClicked,SaveButtonClicked,LoadButtonClicked,BackButtonClicked,RevButtonClicked,MapSizeChangeButtonClicked};
          

            for (int i = 0; i < buttons_action.Length; i++)
            {
                Buttons[i] = new Button(game, true, U(Button_Dims, 2), new Vector2(Button_Dims.X * 2, Button_Dims.Y), Text.default_font, buttons_str[i], buttons_action[i], false);
            }

            for (int i = buttons_action.Length; i < buttons_action.Length + 4; i++)
            {
                Buttons[i] = new Button(game, true, U(Button_Dims, 2), new Vector2(Button_Dims.X * 2, Button_Dims.Y), Text.default_font, buttons_str[i], ScreenMoveButton, false, i - buttons_action.Length);
            }


            MapNameInput = new TextInputBox(game, true, U(Button_Dims,3)-new Vector2(25,0), new Vector2(Button_Dims.X * 3, Button_Dims.Y));
            MapWidthSizeInput = new TextInputBox(game, true, U(Button_Dims, 3) - new Vector2(25, 0), new Vector2(Button_Dims.X * 3, Button_Dims.Y), Map.width_pixel.ToString());
            MapHeightSizeInput = new TextInputBox(game, true, U(Button_Dims, 3) - new Vector2(25, 0), new Vector2(Button_Dims.X * 3, Button_Dims.Y), Map.height_pixel.ToString());



            this.game = game;
            model = game.Content.Load<Texture2D>("UI\\solid");

            for (int i = 1; i<=TileMap.StaticTile_Num; i++)
            {
                if (i == 32 || i == 33 || i == 37 || i == 38 || i == 39 || i == 74 || i == 75 || i == 79 
                    || i == 80 || i == 81 ||i==98 || i==112 || i== 116 || i==117 || i==121 || i==122) continue;
                StaticTilesMapEditor.Add(new Button(game, TileMap.StaticTile_location[i-1], true, U(Button_Dims), Button_Dims, Text.default_font, null, StaticTileButtonClicked, false,i-1));
            }

            for (int i = 0; i < Enum.GetValues(typeof(D2_Trap)).Length; i++)
            {
                D2_Trap trap = (D2_Trap)i;
                if (trap == D2_Trap.SpineMovingPlatform) trap = D2_Trap.MovingPlatform;

                string trapName = Enum.GetName(typeof(D2_Trap), trap);

                TrapMapEditor.Add(new Button(game,"Trap\\"+trapName,true,U(Button_Dims,4),new Vector2(Button_Dims.X*4,Button_Dims.Y),Text.default_font,null,TrapButtonClicked,false,i));

            }

           
            {

                EnvMapEditor.Add(new Button(game, Star.star_path, true, U(Button_Dims, 4), new Vector2(Button_Dims.X * 4, Button_Dims.Y), Text.default_font, null, EnvButtonClicked, false, 1));
                EnvMapEditor.Add(new Button(game, Diamond.Diamond_path, true, U(Button_Dims, 4), new Vector2(Button_Dims.X * 4, Button_Dims.Y), Text.default_font, null, EnvButtonClicked, false, 2));


            }

        }

        private Vector2 U(Vector2 Dims)
        { 
            Vector2 tmp = new Vector2(CurPos.X + Dims.X,CurPos.Y);

            if (tmp.X >= Game1.screen_width)
            {
                tmp = new Vector2(100,CurPos.Y+ Button_Dims.Y);
            }

            return CurPos = tmp;    
        }

        private Vector2 U(Vector2 Dims, int size)
        {
            Vector2 tmp = new Vector2(CurPos.X + Dims.X * size, CurPos.Y);

            if (tmp.X >= Game1.screen_width)
            {
                tmp = new Vector2(100, CurPos.Y + Button_Dims.Y);
            }

            return CurPos = tmp;
        }

        private Vector2 RUI(bool upper,bool right, int idx)
        {
            Vector2 pos = new Vector2((CurDeploy.X-offset.X) * TileMap.Tile_Size,(CurDeploy.Y-offset.Y) * TileMap.Tile_Size);

            if (upper) pos += new Vector2(0, 100);
            else pos -= new Vector2(0, 100);
            if (right) pos += new Vector2(100, 0);
            else pos -= new Vector2(100, 0);

            if (idx != 0)
            {
                pos += new Vector2(Button_Dims.X * 3 * idx, 0);
            }
            return pos;
        }



        public void StaticTileButtonClicked(Object obj)
        {
            if (obj is int x)
            {
                CurSelected = Tuple.Create((int)ShootingGame.D1.Tile, x);
               
            }
        }

        public void TrapButtonClicked(Object obj)
        {
            if (obj is int x)
            {
                CurSelected = Tuple.Create((int)ShootingGame.D1.Trap, x);
               
            }
        }

        public void EnvButtonClicked(Object obj)
        {
            if (obj is int x)
            {
                CurSelected = Tuple.Create((int)ShootingGame.D1.Env, x);

            }
        }

        public void BackButtonClicked()
        {
            Game1.GameMenuFlag = true;
            Game1.GameMapEditorFlag = false;
        }

        public void DeleteButtonClicked()
        {
            CurrentMap = new Map(Tuple.Create(0,0), Tuple.Create(0, 0));
        }

        public void LoadButtonClicked()
        {
            if (MapNameInput.GetInput() == null || MapNameInput.GetInput().Equals(""))
            {
                CurrentMap = save.LoadMapData("Test");
            }

            else
            {
                CurrentMap = save.LoadMapData(MapNameInput.GetInput());
            }

            if (CurrentMap == null)
            {
                throw new Exception("file not exist!");
            }

            MapWidthSizeInput.SetInput(CurrentMap.Map_width.ToString());
            MapHeightSizeInput.SetInput(CurrentMap.Map_height.ToString());

        }

        public void MapSizeChangeButtonClicked()
        {
            CurrentMap.Change_MapSize(int.Parse(MapWidthSizeInput.GetInput()), int.Parse(MapHeightSizeInput.GetInput()));
        }

        public void SaveButtonClicked()
        {
  
            if (MapNameInput.GetInput() == null || MapNameInput.GetInput().Equals(""))
            {
                save.SaveMapData(CurrentMap, "Test");
                return;
            }

            save.SaveMapData(CurrentMap, MapNameInput.GetInput());
        }

        public void RightButtonClicked()
        {
            if (CurSelected != null && CurSelected.Item1 == (int)ShootingGame.D1.Trap && CurDeploy.X != -1)
            {

                RbClicked = true;

                switch ((int)CurSelected.Item2)
                {

                    case 1:
                        OneIntValRightButton(1);

                        break;

                    case 2:

                        MovingPlatformRightButton();



                        break;
                    case 3:

                        SawBladRightButton();


                        break;

                    case 4:
                        OneIntValRightButton(4);
                        break;

                    case 6:
                        SpineMovingPlatformRightButton();
                        break;

                    case 7:
                        FallingTileRightButton();
                        break;

                    default:
                        RbClicked = false;
                        break;

                }

            }

            else if (CurSelected != null && CurSelected.Item1 == (int)ShootingGame.D1.Tile && CurDeploy.X != -1)
            {
                CurrentMap.Map_Update((int)CurDeploy.X, (int)CurDeploy.Y, new Tuple<int, int>(0,0));
                CurDeploy = new Vector2(-1, 0);

            }
        }

  


        public void Info_Okbutton(Object type)
        {
            D2_Trap trap = (D2_Trap)type;
            int val = (int)trap;

      
                switch (val)
                {
                    case 1:
                        CurrentMap.DeployInfo[(int)CurDeploy.Y][(int)CurDeploy.X] = int.Parse(RbInput[0].GetInput());
                        break;
                    case 2:
                        CurrentMap.DeployInfo[(int)CurDeploy.Y][(int)CurDeploy.X] = new int[] { int.Parse(RbInput[0].GetInput()), int.Parse(RbInput[1].GetInput()) };
                        break;
                    case 3:
                        CurrentMap.DeployInfo[(int)CurDeploy.Y][(int)CurDeploy.X] = new int[] { int.Parse(RbInput[0].GetInput()), int.Parse(RbInput[1].GetInput()) };
                        break;
                    case 4:
                        CurrentMap.DeployInfo[(int)CurDeploy.Y][(int)CurDeploy.X] = int.Parse(RbInput[0].GetInput());
                        break;
                    case 6:
                        CurrentMap.DeployInfo[(int)CurDeploy.Y][(int)CurDeploy.X] = new int[] { int.Parse(RbInput[0].GetInput()), int.Parse(RbInput[1].GetInput()), int.Parse(RbInput[2].GetInput()) };
                        break;
                case 7:
                    CurrentMap.DeployInfo[(int)CurDeploy.Y][(int)CurDeploy.X] = new int[] { int.Parse(RbInput[0].GetInput()), int.Parse(RbInput[1].GetInput()), int.Parse(RbInput[2].GetInput()) };
                    break;

                default:
                        break;

                }

            
       
            for (int i = 0; i < RbInput_Cnt; i++)
            {
                RbInput[i] = null;
            }

            this.RbOkButton.active = false;
            this.RbCancleButton.active = false;
            this.RbClicked = false;
        }

        public void Info_CancelButton()
        {
            for (int i = 0; i < RbInput_Cnt; i++)
            {
                RbInput[i] = null;
            }

            this.RbOkButton.active = false;
            this.RbCancleButton.active = false;
            this.RbClicked = false;

        }

        private void RightButton_Pos(out bool upper, out bool right)
        {
            if (CurDeploy.X- offset.X>= Game1.screen_width / (TileMap.Tile_Size * 2)) right = false;
            else right = true;
            if (CurDeploy.Y-offset.Y >= Game1.screen_height / (TileMap.Tile_Size * 2)) upper = false;
            else upper = true;
        }


        private void OneIntValRightButton(int trap_idx)
        {
           
            string[] hint_str = { "Val" };

            

            RightButton_Pos(out bool upper, out bool right);

      
            RbInput[0] = new TextInputBox(game, true, hint_str[0], RUI(upper, right, 0) - new Vector2(25, 0), new Vector2(Button_Dims.X * 3 , Button_Dims.Y));
            

            this.RbOkButton = new Button(game, true, RUI(upper, right, 1) - new Vector2(25, 0), new Vector2(Button_Dims.X, Button_Dims.Y), Text.default_font, "OK", Info_Okbutton, false, trap_idx);
            this.RbCancleButton = new Button(game, true, RUI(upper, right, 2) - new Vector2(25, 0), new Vector2(Button_Dims.X , Button_Dims.Y), Text.default_font, "Cancel", Info_CancelButton, false);


        }

        private void MovingPlatformRightButton()
        {
            int i = 0;
            string[] hint_str = { "XPOS", "YPOS" };

          

            RightButton_Pos(out bool upper, out bool right);


            for (i = 0; i < MovingPlatForm.Addition_variable; i++)
            {
                RbInput[i] = new TextInputBox(game, true, hint_str[i], RUI(upper, right, i) - new Vector2(25, 0), new Vector2(Button_Dims.X * 3, Button_Dims.Y));
            }

            this.RbOkButton = new Button(game, true, RUI(upper, right, i) - new Vector2(25, 0), new Vector2(Button_Dims.X * 3, Button_Dims.Y), Text.default_font, "OK", Info_Okbutton, false, ShootingGame.D2_Trap.MovingPlatform);
            this.RbCancleButton = new Button(game, true, RUI(upper, right, i + 1) - new Vector2(25, 0), new Vector2(Button_Dims.X * 3, Button_Dims.Y), Text.default_font, "Cancel", Info_CancelButton, false);

        }

        private void SpineMovingPlatformRightButton()
        {
            int i = 0;
            string[] hint_str = { "XPOS", "YPOS", "SpineBit" };


            RightButton_Pos(out bool upper, out bool right);


            for (i = 0; i < SpineMovingPlatform.Addition_variable; i++)
            {
                RbInput[i] = new TextInputBox(game, true, hint_str[i], RUI(upper, right, i) - new Vector2(25, 0), new Vector2(Button_Dims.X * 3, Button_Dims.Y));
            }

            this.RbOkButton = new Button(game, true, RUI(upper, right, i) - new Vector2(25, 0), new Vector2(Button_Dims.X * 3, Button_Dims.Y), Text.default_font, "OK", Info_Okbutton, false, ShootingGame.D2_Trap.fallingTile);
            this.RbCancleButton = new Button(game, true, RUI(upper, right, i + 1) - new Vector2(25, 0), new Vector2(Button_Dims.X * 3, Button_Dims.Y), Text.default_font, "Cancel", Info_CancelButton, false);

        }

        private void FallingTileRightButton()
        {
            int i = 0;
            string[] hint_str = { "XDims", "YDims", "SpineBit" };


            RightButton_Pos(out bool upper, out bool right);


            for (i = 0; i < FallingTile.Addition_variable; i++)
            {
                RbInput[i] = new TextInputBox(game, true, hint_str[i], RUI(upper, right, i) - new Vector2(25, 0), new Vector2(Button_Dims.X * 3, Button_Dims.Y));
            }

            this.RbOkButton = new Button(game, true, RUI(upper, right, i) - new Vector2(25, 0), new Vector2(Button_Dims.X * 3, Button_Dims.Y), Text.default_font, "OK", Info_Okbutton, false, ShootingGame.D2_Trap.fallingTile);
            this.RbCancleButton = new Button(game, true, RUI(upper, right, i + 1) - new Vector2(25, 0), new Vector2(Button_Dims.X * 3, Button_Dims.Y), Text.default_font, "Cancel", Info_CancelButton, false);

        }


        private void SawBladRightButton()
        {
            int i = 0;
            string[] hint_str = { "XPOS", "YPOS" };

            RightButton_Pos(out bool upper, out bool right);


            for (i = 0; i < SawBlade.Addition_variable; i++)
            {
                RbInput[i] = new TextInputBox(game, true, hint_str[i],RUI(upper, right, i) - new Vector2(25, 0), new Vector2(Button_Dims.X * 3, Button_Dims.Y));
            }

            this.RbOkButton = new Button(game, true, RUI(upper, right, i) - new Vector2(25, 0), new Vector2(Button_Dims.X * 3, Button_Dims.Y), Text.default_font, "OK", Info_Okbutton, false, ShootingGame.D2_Trap.SawBlade);
            this.RbCancleButton = new Button(game, true, RUI(upper, right, i + 1) - new Vector2(25, 0), new Vector2(Button_Dims.X * 3, Button_Dims.Y), Text.default_font, "Cancel", Info_CancelButton, false);

        }

        public void RevButtonClicked()
        {
            Reverse = !Reverse;

            {
                for (int i = 0; i < Enum.GetValues(typeof(buttons)).Length; i++)
                {
                    Buttons[i].pos.Y = Game1.screen_height - Buttons[i].pos.Y;
                }

                MapNameInput.pos.Y = Game1.screen_height - MapNameInput.pos.Y;
                MapWidthSizeInput.pos.Y = Game1.screen_height - MapWidthSizeInput.pos.Y;
                MapHeightSizeInput.pos.Y = Game1.screen_height - MapHeightSizeInput.pos.Y;

                for (int i = 0; i < StaticTilesMapEditor.Count; i++)
                {
                    StaticTilesMapEditor[i].pos.Y = Game1.screen_height - StaticTilesMapEditor[i].pos.Y;
                }
                for (int i = 0; i < Enum.GetValues(typeof(D2_Trap)).Length; i++)
                {
                    TrapMapEditor[i].pos.Y = Game1.screen_height - TrapMapEditor[i].pos.Y;
                }

                for (int i = 0; i < Enum.GetValues(typeof(D2_Env)).Length; i++)
                {
                    EnvMapEditor[i].pos.Y = Game1.screen_height - EnvMapEditor[i].pos.Y;
                }


            }

        }

        public void ScreenMoveButton(Object _val)
        {
            int val = (int)_val;

            if (val == 0) // left
            {
                offset.X = Math.Max(offset.X-Map.width_pixel,0);        
            }

            if (val == 1) //right
            {
                offset.X = Math.Min(CurrentMap.Map_width-Map.width_pixel, Map.width_pixel + offset.X);
            }

            if (val == 2) // UP
            {
                offset.Y = Math.Min(offset.Y + Map.height_pixel, CurrentMap.Map_height - Map.height_pixel);
            }

            if (val == 3) // Down
            {
                offset.Y = Math.Max(offset.Y - Map.height_pixel,0);
            }



        }

     

        public void Update(Vector2 CursorPos)
        {
            for (int i = 0; i < Enum.GetValues(typeof(buttons)).Length; i++)
            {
                Buttons[i].ForceUpdate(CursorPos);
            }

            MapNameInput.ForceUpdate(CursorPos);
            MapWidthSizeInput.ForceUpdate(CursorPos);
            MapHeightSizeInput.ForceUpdate(CursorPos);

            if (RbClicked)
            {
                    RbOkButton.ForceUpdate(CursorPos);
                    RbCancleButton.ForceUpdate(CursorPos);

                    for (int i = 0; i < RbInput_Cnt; i++)
                    {
                        if (RbInput[i] == null) break;
                        RbInput[i].ForceUpdate(CursorPos);
                    }
            }

            if (FlatMouse.Instance.IsRightMouseButtonPressed() && !RbClicked)
            {
                RightButtonClicked();
            }


            for (int i = 0; i < StaticTilesMapEditor.Count; i++)
            {
                StaticTilesMapEditor[i].ForceUpdate(CursorPos);
            }

            for (int i = 0; i < Enum.GetValues(typeof(D2_Trap)).Length; i++)
            {
                TrapMapEditor[i].ForceUpdate(CursorPos);
            }

            for (int i = 0; i < EnvMapEditor.Count; i++)
            {
                EnvMapEditor[i].ForceUpdate(CursorPos);
            
            }

            bool LeftClick = FlatMouse.Instance.IsLeftMouseButtonPressed();

           

            if (LeftClick && CurSelected!=Tuple.Create(0, 0) && !RbClicked)
            {
                Rectangle rect;

                if (!Reverse)
                {
                    rect = new Rectangle(0, 0, Game1.screen_width, (int)(CurPos.Y + Button_Dims.Y));
                }

                else
                {
                    rect = new Rectangle(0, Game1.screen_height-((int)(CurPos.Y + Button_Dims.Y)), Game1.screen_width, Game1.screen_height);
                }

                if (!rect.Contains(CursorPos))
                {
                    int x = (int)(CursorPos.X / TileMap.Tile_Size) + (int)offset.X;
                    int y = (int)(CursorPos.Y / TileMap.Tile_Size) + (int)offset.Y;
                    CurrentMap.Map_Update(x,y, CurSelected);
                    CurDeploy = new Vector2(x, y);

                }

            }

        }

        public void Draw(Sprites sprite)
        {
            Game1.NoAntiAliasingShader(Color.White);
            sprite.Draw(model, new Rectangle(0, 0, Game1.screen_width, Game1.screen_height), Color.White);
            if (!Reverse)
            {
                sprite.Draw(model, new Rectangle(0, 0, Game1.screen_width, (int)(CurPos.Y + Button_Dims.Y)), Color.Aqua);
            }

            else
            {
                sprite.Draw(model, new Rectangle(0, Game1.screen_height - ((int)(CurPos.Y + Button_Dims.Y)), Game1.screen_width, Game1.screen_height), Color.Aqua);
            }

            for (int i = 0; i < CurrentMap.Map_height; i++)
            {
                for (int j = 0; j < CurrentMap.Map_width; j++)
                {
                    if (CurrentMap.Deploy[i][j].Item1 != 0)
                    {
                        string t = Map.GetTexture(CurrentMap.Deploy[i][j]);
                        Texture2D texture2D = game.Content.Load<Texture2D>(t);

                        sprite.Draw(texture2D, new Rectangle((int)((j-offset.X) * TileMap.Tile_Size), (int)((i-offset.Y) * TileMap.Tile_Size), (int)TileMap.Tile_Size, (int)TileMap.Tile_Size), Color.White, 0f,
                        new Vector2(model.Bounds.Width / 2, model.Bounds.Height / 2));
                    }
                }
            }


            Game1.NoAntiAliasingShader(Color.White);

            for (int i = 0; i < Enum.GetValues(typeof(buttons)).Length; i++)
            {
                Buttons[i].Draw(sprite);
            }

            MapNameInput.Draw(sprite);
            MapWidthSizeInput.Draw(sprite);
            MapHeightSizeInput.Draw(sprite);

            if (RbClicked)
            {
                 RbOkButton.Draw(sprite);
                 RbCancleButton.Draw(sprite);

                for (int i = 0; i < RbInput_Cnt; i++)
                {
                    if (RbInput[i] == null) break;
                    RbInput[i].Draw(sprite);
                }

            }


            for (int i = 0; i < StaticTilesMapEditor.Count; i++)
            {
                StaticTilesMapEditor[i].Draw(sprite);
            }
            for (int i = 0; i < Enum.GetValues(typeof(D2_Trap)).Length; i++)
            {
                TrapMapEditor[i].Draw(sprite);
            }

            for (int i = 0; i< EnvMapEditor.Count; i++)
            {
                EnvMapEditor[i].Draw(sprite);
            }

        }


    }
}
