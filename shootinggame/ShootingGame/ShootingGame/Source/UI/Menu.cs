using Flat.Graphics;
using FlatPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingGame
{
    public class Menu
    {
        public bool active;
        public Vector2 pos, dims, topLeft;
        public Animated2d bkg;
        public Button closeBtn;
        public static SpriteFont MenuFont;
        public static string MenuTexture = "UI\\SlotImage";
        public static string MenuBtnTexture = "UI\\XButton";
        public static Vector2 MenuExitBtnDims = new Vector2(50, 50);
        private Game1 game;


        public Menu(Game1 game, Vector2 pos, Vector2 dims,bool Active)
        {
            this.game = game;   
            this.pos = pos;
            this.dims = dims;

            bkg = new Animated2d(game, MenuTexture,pos,dims,FlatWorld.Wolrd_layer.None_Interact,new Vector2(1,1),1,1,1000,"default");
            closeBtn = new Button(game, MenuBtnTexture,true, new Vector2(pos.X+dims.X/2,dims.Y/2+pos.Y), MenuExitBtnDims,Text.default_font,null,CloseMenu,false);
            MenuFont = game.Content.Load<SpriteFont>(Text.default_font);
            this.active = Active;
        }


   

        public void CloseMenu()
        { 
            this.active = false;
        }

        public virtual void Update()
        {
            if (active)
            { 
                closeBtn.ForceUpdate(Game1.adjustmousePos);
            }
        }

        public virtual void Draw(Sprites sprites)
        {
            if (active)
            {
                bkg.Draw(sprites,Vector2.Zero,0);
                closeBtn.Draw(sprites);
            }
        }
        


    }
}
