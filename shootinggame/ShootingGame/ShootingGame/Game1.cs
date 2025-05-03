using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Flat;
using FlatPhysics;
using Flat.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using Flat.Input;
using System.Reflection;
using System.IO;
using ShootingGame.Source.UI;
using ShootingGame.Source;
using ShootingGame.Source.Buildings;
using System.Runtime.CompilerServices;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using ShootingGame;




namespace ShootingGame
{
    public enum GamePlay_status
    { 
        Normal,
        GameOver,
        Dialog
    }


    public class Game1 : Game
    {
        //  public static System.Globalization.CultureInfo Culture = System.Globalization.CultureInfo.InvariantCulture; 

        public static Effect AntiAliasEffect;
        public static Effect defaulteffect;
        public static Effect ThrobEffect;
        public static Effect ShrinkCircleEffect;


      //  public static bool GameOverFlag = false;
        public static bool GamePauseFlag = false;
        public static bool GameMenuFlag = true;
        public static bool GameOptionFlag = false;
        public static bool GameStageFlag = false;
        public static bool GameMapEditorFlag = false;

        public static GraphicsDeviceManager graphics;     
        private Screen screen;
        private Sprites sprites;
        private Shapes shapes;
        private Camera camera;
        private FlatWorld world;
        private List<Layer> BGLayer;
        private List<Particle> ParticleList;
        private List<ParticleEmitter> particleEmitterList;
        private List<SpriteEntity> spriteList;
        private Dictionary<FlatBody, SpriteEntity> spriteMap;
        private List<SpriteEntity> spriteRemoveList;
        private List<UIEntity> UiRemoveList;
        private List<UIEntity> uIEntityList;
        private List<Tower> TowerList;
        private List<DynamicTile> DynamicTilesRemoveList;
        private Queue<Dialog> dialogQueue;
        private MapDeployment mapDeployment;
        private Texture2D cursor;
        private Hero hero;

        private static float left, right, bottom, top;

        public static Stopwatch WorldTimer = new Stopwatch();
        public static GameTime WorldGameTime;
        public static int screen_width;
        public static int screen_height;
        public static Vector2 offset;
        public static Button ResetButton;
        public static MainMenu mainMenu;
        public static OptionMenu optionMenu;
        public static StageMenu stageMenu;
        public static ExitMenu exitMenu;
        public static MapEditor mapEditor;
        public static Save save;

        public static Vector2 mouseWorldPosition;
        public static Vector2 adjustmousePos;
        public SkillBar skillBar;
        public HeroMenu HeroMenu;
        public static GamePlay_status gamePlay_Status = GamePlay_status.Normal;

        public static Texture2D defaultParticle;

 
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
          //  graphics.SynchronizeWithVerticalRetrace = true;
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
     
         //  filepath =  Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData); 
        }

        public static void AntiAliasingShader(Texture2D texture,Vector2 dims,int Mirror = 0)
        {
            AntiAliasEffect.Parameters["xSize"].SetValue((float)texture.Bounds.Width);
            AntiAliasEffect.Parameters["ySize"].SetValue((float)texture.Bounds.Height);
            AntiAliasEffect.Parameters["xDraw"].SetValue((float)(int)dims.X);
            AntiAliasEffect.Parameters["yDraw"].SetValue((float)(int)dims.Y);
            AntiAliasEffect.Parameters["filterColor"].SetValue(Color.White.ToVector4());
            AntiAliasEffect.Parameters["Mirror"].SetValue(Mirror);
            AntiAliasEffect.CurrentTechnique.Passes[0].Apply();
        }

        public static void AntiAliasingShader(Texture2D texture, Vector2 dims, Vector2 frameSize, int Mirror = 0)
        {
            AntiAliasEffect.Parameters["xSize"].SetValue(frameSize.X);
            AntiAliasEffect.Parameters["ySize"].SetValue(frameSize.Y);
            AntiAliasEffect.Parameters["xDraw"].SetValue((float)(int)dims.X);
            AntiAliasEffect.Parameters["yDraw"].SetValue((float)(int)dims.Y);
            AntiAliasEffect.Parameters["filterColor"].SetValue(Color.White.ToVector4());
            AntiAliasEffect.Parameters["Mirror"].SetValue(Mirror);
            AntiAliasEffect.CurrentTechnique.Passes[0].Apply();
        }

        public static void NoAntiAliasingShader(Color color)
        {
            defaulteffect.CurrentTechnique.Passes[0].Apply();
        }

        public static void ThrobShader(float sin, Color color, int Mirror = 0)
        {
            ThrobEffect.Parameters["SINLOC"].SetValue(sin);
            ThrobEffect.Parameters["filterColor"].SetValue(color.ToVector4());
            ThrobEffect.Parameters["Mirror"].SetValue(Mirror);
            ThrobEffect.CurrentTechnique.Passes[0].Apply();

        }

        public static void ShrinkCircleShader(float value, Color color,Vector2 Framesize, Vector2 origin)
        {

          

            ShrinkCircleEffect.Parameters["param1"].SetValue(1.0f-value);
            ShrinkCircleEffect.Parameters["filterColor"].SetValue(color.ToVector4());
            ShrinkCircleEffect.Parameters["framesize"].SetValue(Framesize);
            ShrinkCircleEffect.Parameters["origin"].SetValue(origin);

            ShrinkCircleEffect.CurrentTechnique.Passes[0].Apply();

        }



        //private void EnemySpawnPointAdd()
        //{

        //    //Vector2[] pos = { new Vector2(screen.Width / 2, screen.Height / 2), new Vector2(-screen.Width / 2, screen.Height / 2), new Vector2(0, screen_height / 2) 
        //    //, new Vector2(3 * screen.Width / 4, screen.Height / 2)
        //    //};

        //    //for (int i = 0; i < 1; i++)
        //    //{
        //    ////    SpawnPoint spawnPoint = new Portal(this, "2d\\Portal", pos[i], new Vector2(50, 50), FlatWorld.Wolrd_layer.Mob_allias, 5f, 3, SpawnPoint.SpawnType.Imp);
        //    ////    AddSpriteWithBody(spawnPoint, spawnPoint.flatBody, FlatWorld.Wolrd_layer.Mob_allias);
        //    //}

        //    //for (int i = 1; i < 2; i++)
        //    //{
        //    // //   SpawnPoint spawnPoint = new Portal(this, "2d\\Portal", pos[i], new Vector2(50, 50), FlatWorld.Wolrd_layer.Mob_allias, 5f, 3, SpawnPoint.SpawnType.Spider);
        //    ////    AddSpriteWithBody(spawnPoint, spawnPoint.flatBody, FlatWorld.Wolrd_layer.Mob_allias);
        //    //}

        //    //for (int i = 2; i < 3; i++)
        //    //{
        //    ////    SpawnPoint spawnPoint = new SpiderEggSac(this, "2d\\EggSac", pos[i], new Vector2(50, 50), FlatWorld.Wolrd_layer.Mob_allias, 5f, 3, SpawnPoint.SpawnType.Spiderling, 1);
        //    // //   AddSpriteWithBody(spawnPoint, spawnPoint.flatBody, FlatWorld.Wolrd_layer.Mob_allias);
        //    //}

        //    //for (int i = 3; i < 4; i++)
        //    //{
        //    //    SpawnPoint spawnPoint = new Portal(this, "2d\\Portal", pos[i], new Vector2(50, 50), FlatWorld.Wolrd_layer.Mob_allias, 5f, 3, SpawnPoint.SpawnType.AncientImp);
        //    //    AddSpriteWithBody(spawnPoint, spawnPoint.flatBody, FlatWorld.Wolrd_layer.Mob_allias);
        //    //}

        //}

        //private void CreateHeroTower()
        //{

        //    Vector2[] pos = { new Vector2(screen.Width / 2, 920), new Vector2(-screen.Width / 2, 120), new Vector2(0,120) };


        //    for (int i = 0; i < 1; i++)
        //    {
        //        Tower tower = new Tower(this,  pos[i], FlatWorld.Wolrd_layer.Hero_allias, 10);
        //        AddSpriteWithBody(tower, tower.flatBody, FlatWorld.Wolrd_layer.Hero_allias);
        //        TowerList.Add(tower);
        //    }


        //}

        protected override void Initialize()
        {
            #region static init

            TileMap.StaticTile_location = new string[TileMap.StaticTile_Num];
            for (int i = 1; i <= 9; i++)
            {
                TileMap.StaticTile_location[i - 1] = "Tiles\\Tile_0" + i;
            }

            for (int i = 10; i <= 125; i++)
            {
                if (i == 32 || i == 33 || i == 37 || i == 38 || i == 39 || i == 74 || i == 75 || i == 79
              || i == 80 || i == 81 || i == 98 || i == 112 || i == 116 || i == 117 || i == 121 || i == 122) continue;
                TileMap.StaticTile_location[i - 1] = "Tiles\\Tile_" + i;
            }

            #endregion

            //     Window.Position = new Point(10, 40);
            //int _aa, _bb;
            // FlatUtil.GetCurrentDisplaySize(graphics, out _aa, out _bb);
            //FlatUtil.SetRelativeBackBufferSize(graphics, 0.5f); 
            IsFixedTimeStep = true;

            FlatUtil.SetAbsoulteBackBufferSize(graphics, 1920, 1080);

            screen = new Screen(this, 1920, 1080);
            sprites = new Sprites(this);
            shapes = new Shapes(this);
            camera = new Camera(screen);
            camera.Zoom = 1;

            camera.GetExtents(out left, out right, out bottom, out top);
  
            screen_width = screen.Width;
            screen_height = screen.Height;

            KeyBindControl.keys.Add(new KeyBind("Left", (int)Keys.A));
            KeyBindControl.keys.Add(new KeyBind("Right", (int)Keys.D));
            KeyBindControl.keys.Add(new KeyBind("Top", (int)Keys.W));
            KeyBindControl.keys.Add(new KeyBind("Down", (int)Keys.S));


            this.world = new FlatWorld();
            
            WorldTimer.Start();

            ParticleList = new List<Particle>();
            particleEmitterList = new List<ParticleEmitter>();
            spriteList = new List<SpriteEntity>();
            spriteMap = new Dictionary<FlatBody, SpriteEntity>();
            spriteRemoveList = new List<SpriteEntity>();
            UiRemoveList = new List<UIEntity>();
            uIEntityList = new List<UIEntity>();
            TowerList = new List<Tower>();
            DynamicTilesRemoveList = new List<DynamicTile>();
            BGLayer = new List<Layer>();
            dialogQueue = new Queue<Dialog>();


            skillBar = new SkillBar(this, new Vector2(100, 50), 100, 5);
            HeroMenu = new HeroMenu(this, new Vector2(screen_width / 2, screen_height / 2), new Vector2(400, 600),false,hero);
            exitMenu = new ExitMenu(this, new Vector2(screen_width / 2, screen_height / 2), new Vector2(400, 600), false);

            offset = new Vector2(0f,0f);
            //   float padding = MathF.Abs(right - left) * 0.10f;



            mainMenu = new MainMenu(this, new Button(this,false,MainMenu.StartButton_Pos,Button.Default_Buttonsz, Text.default_font,"Start",StartButtonClicked, false),
               new Button(this, false, MainMenu.ExitButton_Pos, Button.Default_Buttonsz, Text.default_font, "Exit", ExitButtonClicked, false));
            optionMenu = new OptionMenu(this);
            stageMenu = new StageMenu(this,2);
            mapEditor = new MapEditor(this);
            save = new Save(this);


            //EnemySpawnPointAdd();
            //CreateHeroTower();

            mapDeployment = new MapDeployment();
            mapDeployment.init(this);

            defaultParticle = Content.Load<Texture2D>("Particle\\particle");


            ParticleData pD = new ParticleData(null, 0f, Color.Yellow, Color.Blue, 1f, 0f, 20, 10, 50, 0);
            //  ParticleList.Add(new Particle(adjustmousePos, new ParticleData(null,0f,Color.Yellow,Color.Blue,1f,0f,20,10,50,0)));

            //   ParticleEmitterData pED = new ParticleEmitterData(pD, 0f, 180f, 0.5f, 2.0f, 10f, 100f, 0.01f, 10);
            //     ParticleEmitter pE = new ParticleEmitter(this, adjustmousePos, pED,true);
            //    Add_ParticleEmitter(pE);

            ParticleEmitterData static_pED = new ParticleEmitterData(pD, 0f, MathHelper.Pi/4, 0.5f, 2.0f, 10f, 100f, 0.01f, 10);
            ParticleEmitter static_pE = new ParticleEmitter(this, new Vector2(500,500), static_pED, false);
       
            Add_ParticleEmitter(static_pE);




            base.Initialize();
        }

        protected override void LoadContent()
        {
            #region Hero
            hero = new Hero(this, "Animation\\Hero\\Idle", new Vector2(200, 300), new Vector2(50, 50),FlatWorld.Wolrd_layer.Hero_allias,100);
            hero.AddAnimation(new Vector2(1, 1), "Animation\\Hero\\Jump", 1, 100, "Jump", 1);
            hero.AddAnimation(new Vector2(6, 1), "Animation\\Hero\\Double_Jump", 6, 100, "Double_Jump", 2);
            hero.AddAnimation(new Vector2(1, 1), "Animation\\Hero\\Fall", 1, 100, "Fall", 3);
            hero.AddAnimation(new Vector2(7, 1), "Animation\\Hero\\Hit",7, 200, "Hit", 4);
            hero.AddAnimation(new Vector2(12, 1), "Animation\\Hero\\Run", 12, 100, "Run", 5);
            hero.AddAnimation(new Vector2(5, 1), "Animation\\Hero\\Wall_Jump", 5, 100, "Wall_Jump", 6);


            //  hero.AddAnimation(new Vector2(0, 0), 11, 100, "Movement");
            //    hero.AddAnimation(new Vector2(0,0), )

            hero.SkillAdd(new Flamewave(this,new TargetingCircle(this,FlameCircle.FlameCircle_Dims, hero.layertype),
                new FlameCircle(this,hero.layertype,2f),"HeroFlame"));

         
            hero.SkillAdd(new Blink(this, new BlinkEffect(this, hero.layertype, 1f,250), "Blink"));
            //  hero.SetAnimationFlag(false);
            
            skillBar.Set_Skill(0, "UI\\Icons\\FireIcon", hero, true, false, "HeroFlame");
            skillBar.Set_Skill(1, "UI\\Icons\\Blink", hero, true, false, "Blink");

            AddSpriteWithBody(hero, hero.FlatBody,FlatWorld.Wolrd_layer.Hero_allias);
            #endregion Hero

            cursor = Content.Load<Texture2D>("2d\\CursorArrow");
            defaulteffect = Content.Load<Effect>("Shader\\base");
            AntiAliasEffect = Content.Load<Effect>("Shader\\antialiasing");
            ThrobEffect = Content.Load<Effect>("Shader\\Throb");
            ShrinkCircleEffect = Content.Load<Effect>("Shader\\shrinkCircle");

            uIEntityList.Add(new Text(this, true, "Fonts\\Arial16",hero));
            uIEntityList.Add(new PlayerHealthBar(this, true, new Vector2(hero.health*50,20), 0, Color.White));


            //ResetButton = new Button(this, false, new Vector2(screen_width / 2 - 30 , screen_height / 2 - 30), Button.Default_Buttonsz, "Fonts\\Arial16", "Reset", RetryButtonClicked, true);

            //uIEntityList.Add(ResetButton);
            //uIEntityList.Add(new Message(this, true, new Vector2(300, 300), new Vector2(100, 100), "Level 1", 2f, Color.Black));

         //   dialogQueue.Enqueue(new Dialog(this, true, new Point((int)(screen_width / 6), screen_height / 9), new Vector2(screen_width / 2, screen_height / 5), "Use WASD or the arrow keys to move"));

            for (int i = 0; i < Layer.BG_layer_Num; i++)
            {
                float ran = RandomHelper.RandomSingle();

                BGLayer.Add(new Layer(this,"Layer\\Layer_"+i,ran,i));
            }
            

            // TextInputBox textInputBox = new TextInputBox(this, true, new Vector2(500, 500), new Vector2(100, 50));
            // uIEntityList.Add(textInputBox);




            //#region TileMap

            ////TileMap.Add_StaticTiles_Horizontal_randomly(this, new Vector2(TileMap.Tile_Size * 2 + TileMap.Tile_Size, TileMap.Tile_Size * 2), 10);
            ////TileMap.Add_StaticTiles_Horizontal_randomly(this, new Vector2(TileMap.Tile_Size * 13, TileMap.Tile_Size * 2), 10);
            ////TileMap.Add_StaticTiles_Vertical_randomly(this, new Vector2(TileMap.Tile_Size * 2, TileMap.Tile_Size * 2), 10);

            ////TileMap.Add_DynamicTile(new VanishingTile(this,new Vector2(700,500),new Vector2(256,128),200));
            ////TileMap.Add_DynamicTile(new FallingTile(this, "Tiles\\Tile_09",new Vector2(1100, 500), new Vector2(128, 128), true, false));

            //#endregion

            //#region Trap
            ////Trap spine1 = new Spine(this, new Vector2(500, 300));
            ////AddSpriteWithBody(spine1, spine1.flatBody, FlatWorld.Wolrd_layer.Static_allias);

            ////Trap jump1 = new JumpTrap(this, new Vector2(700, 350));
            ////AddSpriteWithBody(jump1, jump1.flatBody, FlatWorld.Wolrd_layer.Static_allias);

            ////Trap SawBlade1 = new SawBlade(this, new Vector2(900, 350), new Vector2(900, 550));
            ////AddSpriteWithBody(SawBlade1, SawBlade1.flatBody, FlatWorld.Wolrd_layer.Static_allias);

            ////Trap MovingPlatform = new MovingPlatForm(this, new Vector2(800,500), new Vector2(1000,600), 1f);
            ////AddSpriteWithBody(MovingPlatform, MovingPlatform.flatBody, FlatWorld.Wolrd_layer.Static_allias);

            //#endregion Trap

            mapDeployment.Load_Map(1, 1);

            SoundControl.BkgMusicChange(this,"ImGood", "Audio\\ImGood", 0.05f);
            SoundControl.SoundAdd(new SoundItem(this, "FireSound", "Audio\\Projectile\\FireSound", 1f));


        }

        public void Generate_ParticleEmitter(PE_Set val, Vector2 pos)
        {

            switch (val)
            {
                case PE_Set.Pouring_Circle:
                    ParticleEmitterData ped = new ParticleEmitterData(
                    new ParticleData(Color.Green, Color.Yellow, 1, 0, 8, 32),
                    0,
                    MathHelper.Pi,
                    2f,
                    2f,
                    50f,
                    50f,
                    0.2f,
                    150
                    );

                    ParticleEmitter Release_Circle = new ParticleEmitter(this, pos, ped, false,Hero.Respawn_Time);
                    this.Add_ParticleEmitter(Release_Circle);
                    break;
            }




        }


        public void Add_UIEntityMessage(Message message)
        {
            
            uIEntityList.Add(message);
        }

        public void Add_Particle(Particle particle)
        {
            ParticleList.Add(particle);
        }

        public void Add_ParticleEmitter(ParticleEmitter particleEmitter)
        {
            particleEmitterList.Add(particleEmitter);
        }


        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        public static bool GameOver()
        {
         

            return gamePlay_Status == GamePlay_status.GameOver;
        }

        private void CheckGameOver(out GamePlay_status prevflag)
        {
            prevflag = gamePlay_Status;


            if (hero.Destroy)
            {
                gamePlay_Status = GamePlay_status.GameOver;
                return;
            }

            if (TowerList.Count == 0) return;

            for (int i = 0; i<TowerList.Count; i++)
            {
                if (!TowerList[i].Destroy)
                {
                    return;
                }
            }

     
            return;

        }

        public void StartButtonClicked()
        {
            GameMenuFlag = false;
            GameStageFlag = true;
        }

        public void ExitButtonClicked()
        {
            Exit();
        }

        public void Dialog_Close()
        {
            dialogQueue.Dequeue();
            if (dialogQueue.Count == 0)
            {
                gamePlay_Status = GamePlay_status.Normal;
            }
        }

        //public void RetryButtonClicked()
        //{
        //    if (hero.Destroy)
        //    {
        //        hero.Revive(new Vector2(300, 300));
        //    }

        //    for (int i = 0; i < TowerList.Count; i++)
        //    {
        //        Tower tower1 = TowerList[i];

        //        if (!tower1.Destroy) continue;

        //        if (tower1 is Tower tower)
        //        {
        //            TowerList[i] = new Tower(this, tower.pos, tower.layertype, tower.maxhealth);
        //            AddSpriteWithBody(TowerList[i], TowerList[i].flatBody, tower.layertype);
        //        }


        //    }

        //    GameOverFlag = false;

        //}

  
        protected override void Update(GameTime gameTime)
        {
            FlatKeyboard keyboard = FlatKeyboard.Instance;
            FlatMouse mouse = FlatMouse.Instance;
            WorldGameTime = gameTime;

            keyboard.Update();
            mouse.Update();
            mouseWorldPosition = mouse.GetMouseWorldPosition(this, this.screen, this.camera);
            adjustmousePos = new Vector2(screen_width * 0.5f + mouseWorldPosition.X - offset.X, screen_height * 0.5f + mouseWorldPosition.Y - offset.Y);

            if (GameMenuFlag)
            {
                mainMenu.Update(adjustmousePos);
            }

            else if (GameOptionFlag)
            {
                optionMenu.Update(adjustmousePos);
            }

            else if (GameStageFlag)
            {
                stageMenu.Update(adjustmousePos);
            }

            else if (GameMapEditorFlag)
            {
                mapEditor.Update(adjustmousePos);
            }

            else
            {

                CheckGameOver(out GamePlay_status prev);
                
                #region UpdateUI

                for (int i = 0; i < uIEntityList.Count; i++)
                {
                    uIEntityList[i].ForceUpdate(adjustmousePos);
                }


                if (HeroMenu.active)
                {
                    HeroMenu.Update();
                    base.Update(gameTime);
                    return;
                }

                else if (exitMenu.active)
                {
                    exitMenu.Update();
                    base.Update(gameTime);
                    return;

                }
                
                if (keyboard.IsKeyClicked(Keys.L))
                {
                    Game1.GamePauseFlag = !Game1.GamePauseFlag;
                }

                if (Game1.GamePauseFlag)
                {
                    base.Update(gameTime);
                    return;

                }

                #endregion

                #region Particle Update

                for (int i = 0; i < ParticleList.Count; i++)
                {
                    ParticleList[i].Update();
                }

                for (int i = 0; i < particleEmitterList.Count; i++)
                {

                    particleEmitterList[i].Update(adjustmousePos);
                }

                ParticleList.RemoveAll(p => p.finished);
                particleEmitterList.RemoveAll(pE => pE.Destroy);

                #endregion


                if (keyboard.IsKeyClicked(Keys.O) && !exitMenu.active)
                {
                    HeroMenu.active = true;

                }

                if (keyboard.IsKeyClicked(Keys.Escape) && !HeroMenu.active)
                {
                    exitMenu.active = true;
                }

                if (gamePlay_Status == GamePlay_status.Dialog)
                {
                    if (dialogQueue.Count == 0)
                    {
                        throw new Exception("Dialog Empty!");
                    }

                    dialogQueue.Peek().Update(adjustmousePos);



                    if (keyboard.IsKeyClicked(Keys.Enter))
                    {
                        Dialog_Close();
                    }
                    base.Update(gameTime);
                    return;

                }

                if (GameOver())
                {
                 



                    //if (ResetButton.active == false)
                    //{
                    //    ResetButton.Activate(true);
                    //}

                    //if (keyboard.IsKeyClicked(Keys.Enter))
                    //{

                    //    if (hero.Destroy)
                    //    {
                    //        hero.Revive(new Vector2(300, 300));
                    //    }

                    //    for (int i = 0; i < TowerList.Count; i++)
                    //    {
                    //        Tower tower1 = TowerList[i];

                    //        if (!tower1.Destroy) continue;

                    //        if (tower1 is Tower tower)
                    //        {
                    //            TowerList[i] = new Tower(this, tower.pos, tower.layertype, tower.maxhealth);
                    //            AddSpriteWithBody(TowerList[i], TowerList[i].flatBody, tower.layertype);
                    //        }


                    //    }

                    //    GameOverFlag = false;
                    //    ResetButton.Activate(false);
                    //}
                    if (prev != GamePlay_status.GameOver)
                    {
                        Generate_ParticleEmitter(PE_Set.Pouring_Circle, hero.pos);
                    }

                    if (hero.Try_Revive())
                    {
                        // dialog로 인해서버그가 생길수있는가?
                        // normal로 돌아가도 좋은가

                        gamePlay_Status = GamePlay_status.Normal;
                    
                    }
                    base.Update(gameTime);
                    return;

                }

                #region mouse_input

                if (mouse.IsLeftMouseButtonPressed() && !hero.skillList[0].activate)
                {

                    //Vector2 Ejection_dir = CursorDirectionVector2(mouseWorldPosition);

                    //Fireball fireball = new Fireball(this, hero.pos, hero, Ejection_dir, FlatWorld.Wolrd_layer.Hero_allias);
                    //AddSpriteWithBody(fireball, fireball.FlatBody, FlatWorld.Wolrd_layer.Hero_allias);
                    //fireball.FlatBody.RotateTo(MathF.Atan2(Ejection_dir.Y, Ejection_dir.X) - MathHelper.Pi / 2);
                    //SoundControl.SoundChange("FireSound");

              
                }


                if (mouse.IsRightMouseButtonPressed())
                {
                    camera.Shake(0.5f, new Vector2(5, 5), 0.05f);
                }

                //      FlatVector cursorDir = CursorDirection(mouseWorldPosition);
                //     hero.FlatBody.RotateTo(MathF.Atan2(cursorDir.Y, cursorDir.X) - MathHelper.Pi / 2);

                #endregion

                #region keyboard_input

                if (keyboard.IsKeyAvailable)
                {
          
                    if (!hero.Destroy)
                    {

                        //if (keyboard.IsKeyClicked(Keys.T))
                        //{
                        //    ArrowTower arrowTower = new ArrowTower(this, hero.pos, FlatWorld.Wolrd_layer.Hero_allias, 5f);
                        //    AddSpriteWithBody(arrowTower, arrowTower.flatBody, arrowTower.layertype);


                        //}
                     
                        if (keyboard.IsKeyClicked(Keys.D1))
                        {
                            hero.ActivateSkill("HeroFlame", new Vector2(screen_width * 0.5f + mouseWorldPosition.X - offset.X, screen_height * 0.5f + mouseWorldPosition.Y - offset.Y));

                        }

                        if (keyboard.IsKeyClicked(Keys.D2))
                        {
                            hero.ActivateSkill("Blink", new Vector2(screen_width * 0.5f + mouseWorldPosition.X - offset.X, screen_height * 0.5f + mouseWorldPosition.Y - offset.Y));

                        }

                        float dx = 0f;
                        float dy = 0f;
                        float forceMagnitude = Hero.Speed * 75f;

                        if (keyboard.IsKeyDown(Keys.Left) || keyboard.IsKeyDown(KeyBindControl.getKeyByName("Left")))
                        {
                            dx--;
                            hero.Left = true;
                        }

                        if (keyboard.IsKeyDown(Keys.Right) || keyboard.IsKeyDown(KeyBindControl.getKeyByName("Right")))
                        {
                            dx++;
                            hero.Left = false;
                        }

                        if (keyboard.IsKeyDown(Keys.Up) || keyboard.IsKeyDown(KeyBindControl.getKeyByName("Top")))
                        {
                            dy++;
                        }

                        if (keyboard.IsKeyDown(Keys.Down) || keyboard.IsKeyDown(KeyBindControl.getKeyByName("Down")))
                        {
                            dy--;
                        }

                        if (keyboard.IsKeyClicked(Keys.Space))
                        {
                            if (hero.status == Hero.Hero_Status.bottomed || hero.status == Hero.Hero_Status.MovingPlatform)
                            {
                                hero.Jump();
                            }

                            else if (hero.status==Hero.Hero_Status.wallContact && (hero.WallJumpLastTime + (double)Hero.WallJumpCooldown) < Game1.WorldTimer.Elapsed.TotalSeconds)
                            {
                                if (dx != 0 || dy != 0)
                                {
                                    FlatVector forecdirection = FlatPhysics.FlatMath.Normalize(new FlatVector(dx, dy));

                                    hero.WallJump(forecdirection);

                                }
                            }

                            else if ((hero.JumpLastTime + (double)Hero.DoubleJumpIntime) > Game1.WorldTimer.Elapsed.TotalSeconds)
                            {
                                hero.DoubleJump();
                            }
                        }


                        if (hero != null)
                        {
                            if (dx == 0)
                            {
                                hero.ani_status = Hero.Hero_Status2.idle;
                            }
                            if (dx != 0 || dy != 0)
                            {
                                FlatVector forecdirection = FlatPhysics.FlatMath.Normalize(new FlatVector(dx, dy));
                                FlatVector force = new FlatVector(forecdirection.X * forceMagnitude, 0f);
                                hero.FlatBody.AddForce(force * FlatUtil.GetElapsedTimeInSeconds(gameTime));

                                if (keyboard.IsKeyClicked(Keys.R))
                                {
                                    if (hero.status != Hero.Hero_Status.bottomed && hero.dash == 0)
                                    {
                                        hero.Dash(forecdirection);
                                    }
                                }

                            }




                        }
                    }
                }


                {


                    if (hero != null && Math.Abs(hero.FlatBody.LinearVelocity.X) < 2f)
                    {
                        hero.ani_status = Hero.Hero_Status2.idle;
                    }
                }

                #endregion

                this.world.Step(FlatUtil.GetElapsedTimeInSeconds(gameTime), 20);

                List<Mob> mobs = new List<Mob>();

                for (int i = 0; i < spriteList.Count; i++)
                {
                    if (spriteList[i] is Mob)
                    {
                        mobs.Add((Mob)spriteList[i]);
                    }
                }


                for (int i = 0; i < spriteList.Count; i++)
                {
                    SpriteEntity spriteEntity = spriteList[i];

                    if (spriteEntity is Hero)
                    {
                        Vector2 tmp_mousepos = new Vector2(screen_width * 0.5f + mouseWorldPosition.X - offset.X, screen_height * 0.5f + mouseWorldPosition.Y - offset.Y);
                        spriteEntity.Update(mobs, tmp_mousepos);
                    }

                    else if (spriteEntity is Mob || spriteEntity is Star || spriteEntity is Diamond)
                    {
                        spriteEntity.Update(hero);
                    }

                    else if (spriteEntity is ArrowTower)
                    {
                        spriteEntity.Update(mobs);
                    }

                    else
                    {
                        spriteEntity.Update();
                    }

                }

                TileMap.Update(hero);

                foreach (var tuple in world.CollideTile)
                {
                    // 타일과 trap등의 처리도 필요함

                    if (spriteMap.TryGetValue(tuple.Item1, out SpriteEntity a) && a is Hero _ah)
                    {

                        if (TileMap.FlatBodyIsBottom(tuple.Item2, _ah))
                        {
                            _ah.bottomReach();
                        }

                        else if (TileMap.FlatBodyIsSideWall(tuple.Item2, _ah))
                        {
                            _ah.WallReach();
                        }


                    }

                    else if (spriteMap.TryGetValue(tuple.Item2, out SpriteEntity b) && b is Hero _bh)
                    {

                        if (TileMap.FlatBodyIsBottom(tuple.Item1, _bh))
                        {
                            _bh.bottomReach();
                        }

                        else if (TileMap.FlatBodyIsSideWall(tuple.Item1, _bh))
                        {
                            _bh.WallReach();
                        }
                    }

                }


                foreach (var tuple in world.CollideTuple)
                {

                    SpriteEntity a;
                    SpriteEntity b;
                    if (!spriteMap.TryGetValue(tuple.Item1, out a))
                    {
                        throw new Exception("spriteMap not found body");
                    }

                    if (!spriteMap.TryGetValue(tuple.Item2, out b))
                    {
                        throw new Exception("spriteMap not found body");
                    }

                    if (a is Projectile2d || b is Projectile2d)
                    {

                        if (a is Projectile2d projectile_A)
                        {
                            projectile_A.HitSomething(b);
                        }

                        if (b is Projectile2d projectile_B)
                        {
                            projectile_B.HitSomething(a);
                        }
                    }

                    else if (a is Mob || b is Mob)
                    {
                        if (a is Mob MobA)
                        {
                            if (b is Hero)
                            {
                                MobA.AttackPlayer(MobA.Atkdamage, hero);
                                MobA.Destroy_Sprite();
                            }

                            else if (b is Building buildingb)
                            {
                                buildingb.Attacked(MobA.Atkdamage);
                            }


                        }

                        else
                        {
                            Mob MobB = b as Mob;

                            if (a is Hero)
                            {
                                MobB.AttackPlayer(MobB.Atkdamage, hero);
                                MobB.Destroy_Sprite();
                            }

                            else if (a is Building buildinga)
                            {
                                buildinga.Attacked(MobB.Atkdamage);
                            }

                        }


                    }

                    else if (a is Trap || b is Trap)
                    {


                        if (a is Trap trapA)
                        {
                            trapA.Interact(b);
                        }

                        else if (b is Trap trapB)
                        {
                            trapB.Interact(a);
                        }


                    }


                }


                UiRemoveList.Clear();


                for (int i = 0; i < uIEntityList.Count; i++)
                {
                    if (uIEntityList[i].Destory)
                    {
                        UiRemoveList.Add(uIEntityList[i]);
                    }


                    if (uIEntityList[i] is PlayerHealthBar)
                    {
                        uIEntityList[i].Update(hero.health, hero.maxHealth);
                    }

                    else
                    {
                        uIEntityList[i].Update();
                    }
                }

                skillBar.Update(adjustmousePos);


                this.spriteRemoveList.Clear();

                for (int i = 0; i < spriteList.Count; i++)
                {
                    //FlatEntity entity = entityList[i];
                    //FlatBody body = entity.Body;

                    //if (body.isStatic) { continue; }

                    //FlatAABB box = body.GetAABB();

                    //if (box.Max.Y < viewBottom)
                    //{
                    //    entityRemovalList.Add(entity);

                    //}
                    if (spriteList[i].Destroy && spriteList[i] is not Hero)
                    {
                        spriteRemoveList.Add(spriteList[i]);
                    }
                }

                for (int i = 0; i < spriteRemoveList.Count; i++)
                {
                    SpriteEntity spriteEntity = spriteRemoveList[i];
                    if (spriteEntity.GetFlatBody(out FlatBody flatBody))
                    {
                        RemoveSpriteWithBody(spriteEntity, flatBody);
                    }
                    else
                    {
                        spriteList.Remove(spriteEntity);
                    }
                }

                for (int i = 0; i < UiRemoveList.Count; i++)
                {
                    UIEntity uiEntity = UiRemoveList[i];

                    uIEntityList.Remove(uiEntity);

                }

                DynamicTilesRemoveList.Clear();

                for (int i = 0; i < TileMap.DynamicTiles.Count; i++)
                {
                    if (TileMap.DynamicTiles[i].Destroy) DynamicTilesRemoveList.Add((TileMap.DynamicTiles[i]));

                }

                for (int i = 0; i < DynamicTilesRemoveList.Count; i++)
                {
                    RemoveBody(DynamicTilesRemoveList[i].flatBody, FlatWorld.Wolrd_layer.Static_allias);
                    TileMap.DynamicTiles.Remove(DynamicTilesRemoveList[i]);

                }

           

            }

            base.Update(gameTime);
        }


        private void MoveScreen()
        {
            Vector2 Map_Size = mapDeployment.currentMapSize();
            bool update = false;

            if (hero.pos.X < screen_width * 0.75f + left && left > -screen_width * 0.5f)
            {
                float remain = hero.pos.X - (screen_width * 0.75f + left);
                camera.Move(new Vector2(remain, 0));
                offset.X += remain;
                update = true;
            }

            if (hero.pos.X > screen_width * 0.25f + right && screen_width * 0.5f + right < Map_Size.X)
            {
                float remain = hero.pos.X - (screen_width * 0.25f + right);
                camera.Move(new Vector2(remain, 0));
                offset.X += remain;
                update = true;
            }

            if (hero.pos.Y< screen_height* 0.75f + bottom && bottom > -screen_height*0.5f)
            {
                float remain = hero.pos.Y - (screen_height * 0.75f + bottom);
                camera.Move(new Vector2(0, remain));
                offset.Y += remain;
                update = true;
            }

            if (hero.pos.Y > screen_height * 0.25f + top && screen_height*0.5f + top < Map_Size.Y)
            {

                float remain = hero.pos.Y - (screen_height * 0.25f + top );
                camera.Move(new Vector2(0, remain));
                offset.Y += remain;
                update = true;
            }

            if (update)
            {
                camera.GetExtents(out left, out right, out bottom, out top);
            }

            if (// out of position)
            {
                //맵을 바꾸든가 죽게 처리
                {
                    string next_map = mapDeployment.NextMap(0);

                    if (next_map != null)
                    {
                        mapDeployment.Load_Map(next_map);
                    }
                    else
                    {
                        // 죽을필요가 있을까?
                    }
                }
            }

        }


        public void AddSpriteWithBody(SpriteEntity spriteEntity, FlatBody flatBody,FlatWorld.Wolrd_layer wolrd_Type) 
        { 
            spriteList.Add(spriteEntity);
            world.AddBody(flatBody, wolrd_Type);
            spriteMap.Add(flatBody,spriteEntity);
        }

        public void AddBody(FlatBody flatBody, FlatWorld.Wolrd_layer wolrd_Type)
        {
            world.AddBody(flatBody, wolrd_Type);
        }


        //public void AddSpriteList(SpriteEntity spriteEntity)
        //{
        //    spriteList.Add(spriteEntity);
        //}


        public void RemoveSpriteWithBody(SpriteEntity spriteEntity, FlatBody flatBody)
        {
            spriteMap.Remove(flatBody);
            world.RemoveBody(flatBody,spriteEntity.layertype);
            spriteList.Remove(spriteEntity);
        }

        public void RemoveBody(FlatBody flatBody, FlatWorld.Wolrd_layer wolrd_Type)
        {
            world.RemoveBody(flatBody, wolrd_Type);
        }

       


        
        protected override void Draw(GameTime gameTime)
        {
    
            screen.Set();
            GraphicsDevice.Clear(new Color(118, 147, 179));
            shapes.RemoveAll();

            // 배경하자
            // begin end 해서 그림자만 따로처리하죠



            //for (int i = 0; i < world.CollideTuple.Count; i++)
            //{
            //    FlatBody spriteEntity = world.CollideTuple[i].Item1;
            //    FlatBody spriteEntity2 = world.CollideTuple[i].Item2;

            //    Vector2 adjustPos = new Vector2(spriteEntity.Position.X - screen.Width / 2, spriteEntity.Position.Y - screen.Height / 2);
            //    Vector2 adjustPos2 = new Vector2(spriteEntity2.Position.X - screen.Width / 2, spriteEntity2.Position.Y - screen.Height / 2);
            //    shapes.DrawBox(adjustPos, spriteEntity.width, spriteEntity.height, Color.White);
            //    shapes.DrawBox(adjustPos, spriteEntity2.width, spriteEntity2.height, Color.White);
            //}

            if (GameMenuFlag)
            {
                sprites.Begin();

                mainMenu.Draw(sprites);

                Vector2 mouseWorldPosition = FlatMouse.Instance.GetMouseWorldPosition(this, this.screen, this.camera);
                AntiAliasingShader(cursor, new Vector2(28, 28));
                sprites.Draw(cursor, new Rectangle((int)(screen_width * 0.5f + mouseWorldPosition.X - offset.X), (int)(screen_height * 0.5f + mouseWorldPosition.Y - offset.Y), 28, 28), Color.White, new Vector2(0, cursor.Bounds.Height));
                sprites.End();
            }

            else if (GameOptionFlag)
            {
                sprites.Begin();
                optionMenu.Draw(sprites, -offset);
                Vector2 mouseWorldPosition = FlatMouse.Instance.GetMouseWorldPosition(this, this.screen, this.camera);
                AntiAliasingShader(cursor, new Vector2(28, 28));
                sprites.Draw(cursor, new Rectangle((int)(screen_width * 0.5f + mouseWorldPosition.X - offset.X), (int)(screen_height * 0.5f + mouseWorldPosition.Y - offset.Y), 28, 28), Color.White, new Vector2(0, cursor.Bounds.Height));
                sprites.End();

            }

            else if (GameStageFlag)
            {
                sprites.Begin();
                stageMenu.Draw(sprites);
                Vector2 mouseWorldPosition = FlatMouse.Instance.GetMouseWorldPosition(this, this.screen, this.camera);
                AntiAliasingShader(cursor, new Vector2(28, 28));
                sprites.Draw(cursor, new Rectangle((int)(screen_width * 0.5f + mouseWorldPosition.X - offset.X), (int)(screen_height * 0.5f + mouseWorldPosition.Y - offset.Y), 28, 28), Color.White, new Vector2(0, cursor.Bounds.Height));
                sprites.End();
            }

            else if (GameMapEditorFlag)
            {
                sprites.Begin();
                mapEditor.Draw(sprites);
                Vector2 mouseWorldPosition = FlatMouse.Instance.GetMouseWorldPosition(this, this.screen, this.camera);
                AntiAliasingShader(cursor, new Vector2(28, 28));
                sprites.Draw(cursor, new Rectangle((int)(screen_width * 0.5f + mouseWorldPosition.X - offset.X), (int)(screen_height * 0.5f + mouseWorldPosition.Y - offset.Y), 28, 28), Color.White, new Vector2(0, cursor.Bounds.Height));
                sprites.End();


                shapes.Begin(camera);

                //안보이는건 안보이게끔만들수잇게 수정필요

         

                for (int i =(int)( -screen.Height / 2 + TileMap.Tile_Dims.Y/2); i < screen_height/2; i += (int)TileMap.Tile_Dims.Y)
                {
                    for (int j =(int)( -screen.Width / 2 + TileMap.Tile_Dims.X/2); j < screen_width/2; j += (int)TileMap.Tile_Dims.X)
                    {
                        shapes.DrawBox(new Vector2(j, i), TileMap.Tile_Dims.X, TileMap.Tile_Dims.Y, Color.Black);

                    }

                }
                shapes.End();

            }

            else
            {
                MoveScreen();

                sprites.Begin(spriteSort: SpriteSortMode.BackToFront);

                foreach (Layer layer in BGLayer)
                {
                    layer.Draw(sprites);
                }

                sprites.End();



                sprites.Begin();

                TileMap.Draw(sprites, -offset);

                Vector2 mouseWorldPosition = FlatMouse.Instance.GetMouseWorldPosition(this, this.screen, this.camera);

                foreach (SpriteEntity spriteEntity in spriteList)
                {
                    spriteEntity.Draw(sprites, -offset);

                }

                for (int i = 0; i < uIEntityList.Count; i++)
                {
                    if (uIEntityList[i] is PlayerHealthBar)
                    {
                        uIEntityList[i].Draw(sprites);
                    }

                    else
                    {
                        uIEntityList[i].Draw(sprites);
                    }
                }

                foreach (Particle particle in ParticleList)
                {
                    particle.Draw(sprites, -offset);
                }

                skillBar.Draw(sprites);

                if (HeroMenu.active)
                {
                    HeroMenu.Draw(sprites);
                }

                else if (exitMenu.active)
                {
                    exitMenu.Draw(sprites);
                }

                if (dialogQueue.Count != 0)
                {
                    dialogQueue.Peek().Draw(sprites);
                }


                AntiAliasingShader(cursor, new Vector2(28, 28));
                sprites.Draw(cursor, new Rectangle((int)(screen_width * 0.5f + mouseWorldPosition.X - offset.X), (int)(screen_height * 0.5f + mouseWorldPosition.Y - offset.Y), 28, 28), Color.White, new Vector2(0, cursor.Bounds.Height));
                sprites.End();


                shapes.Begin(camera);

        

                foreach (SpriteEntity spriteEntity in spriteList)
                {

                    Vector2 adjustPos = new Vector2(spriteEntity.pos.X - screen.Width / 2,  - spriteEntity.pos.Y + screen_height /2 + 2*offset.Y  );

                   

                    if (spriteEntity is not Fireball)
                    {
                        
                        shapes.DrawBox(adjustPos, spriteEntity.dims.X, spriteEntity.dims.Y, Color.White);
                    }
                    else
                    {
                        shapes.DrawCircle(new FlatCircle(adjustPos, spriteEntity.dims.X), 24, Color.White);
                    }

                }

                //foreach (DynamicTile dynamicTile in TileMap.DynamicTiles)
                //{
                //    Vector2 adjustPos = new Vector2(dynamicTile.pos.X - screen.Width / 2, dynamicTile.pos.Y - screen.Height / 2);
                //    shapes.DrawBox(adjustPos, dynamicTile.dims.X, dynamicTile.dims.Y, Color.White);
                //}

                //foreach (StaticTile staticTile in TileMap.StaticTiles)
                //{
                //    Vector2 adjustPos = new Vector2(staticTile.pos.X - screen.Width / 2, staticTile.pos.Y - screen.Height / 2);
                //    shapes.DrawBox(adjustPos, staticTile.dims.X, staticTile.dims.Y, Color.White);

                //}

                foreach (FlatBody flatBody in world.bodyList[2])
                {
                    Vector2 adjustPos = new Vector2(flatBody.Position.X - screen.Width / 2, -flatBody.Position.Y + screen_height/2 + 2 * offset.Y);


                    shapes.DrawBox(adjustPos,flatBody.width, flatBody.height, Color.White);
                }
  
                shapes.End();
            }

   

            screen.Unset();
            screen.Present(sprites);
            base.Draw(gameTime);
        }
        
    }
}

#if false
 #region groundbody

            if (!FlatBody.CreateBoxBody(right - left - padding * 2, 3f,
                1f, true, 0.5f, out FlatBody groundBody, out string errorMessage))
            { 
                throw new Exception(errorMessage);  
            }

            groundBody.MoveTo(new FlatVector(0, -10));

            this.world.AddBody(groundBody);
            this.entityList.Add(new FlatEntity(groundBody));


            #endregion
            #region ledgeBody

            if (!FlatBody.CreateBoxBody(20f, 2f,
                1f, true, 0.5f, out FlatBody ledgeBody1, out errorMessage))
            {
                throw new Exception(errorMessage);
            }
            ledgeBody1.MoveTo(new FlatVector(-10, 3));
            ledgeBody1.Rotate(-MathHelper.TwoPi / 20f);

            this.world.AddBody(ledgeBody1);
            this.entityList.Add(new FlatEntity(ledgeBody1));


            if (!FlatBody.CreateBoxBody(15f, 2f, 
                1f, true, 0.5f, out FlatBody ledgeBody2, out errorMessage))
            {
                throw new Exception(errorMessage);
            }
            ledgeBody2.MoveTo(new FlatVector(10, 10));
            ledgeBody2.Rotate(MathHelper.TwoPi / 20f);

            this.world.AddBody(ledgeBody2);
            this.entityList.Add(new FlatEntity(ledgeBody2));
           


            #endregion
#endif


