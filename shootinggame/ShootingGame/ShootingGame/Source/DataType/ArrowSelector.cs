using Flat.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingGame
{
    public class ArrowSelector
    {
        private Game1 game;
        public int selected;
        public string title;
        public Vector2 pos, dims;
        public List<UIEntity> buttons = new List<UIEntity>();
        public List<FormOption> options = new List<FormOption>();
        public Action action;
        public CheckBox checkBox;
        public string input;
        public bool UpdateNextFrame=false;
        public UI_TYPE TYPE;
  

        public enum UI_TYPE
        { 
            Button,
            CheckBox,
            Input
        
        }

        public ArrowSelector(Game1 game, Vector2 pos, Vector2 dims, string title, UI_TYPE Ui_TYPE)
        { 
            this.title = title;
            this.pos = pos; 
            this.dims = dims;   
            this.game = game;
            this.TYPE = Ui_TYPE; 
           
            selected = 0;

            if (Ui_TYPE == UI_TYPE.Button)
            {
                buttons.Add(new Button(game, "UI\\ArrowLeft", true, new Vector2(pos.X - dims.X, pos.Y), dims, Text.default_font, null, ArrowLeftClick, false));
                buttons.Add(new Button(game, "UI\\ArrowRight", true, new Vector2(pos.X + dims.X, pos.Y), dims, Text.default_font, null, ArrowRightClick, false));
            }

            else 
            {
                throw new Exception("Type error");

               // buttons.Add(new CheckBox(game, true, new Vector2(pos.X - dims.X/2, pos.Y/2),dims,Text.default_font,null,))

            }

        }

        public ArrowSelector(Game1 game, Vector2 pos, Vector2 dims, string title, UI_TYPE Ui_TYPE,bool check)
        {
            this.title = title;
            this.pos = pos;
            this.dims = dims;
            this.game = game;
            this.TYPE = Ui_TYPE;
            selected = 0;



            if (Ui_TYPE == UI_TYPE.CheckBox)
            {
                checkBox = new CheckBox(game, true, new Vector2(pos.X - dims.X, pos.Y), dims, Text.default_font, null, ActionFunc, check);
                buttons.Add(checkBox);
            }

            else
            {
                throw new Exception("Type error");
            }

        }

        public ArrowSelector(Game1 game, Vector2 pos, Vector2 dims, string title, UI_TYPE Ui_TYPE, string str,Action action=null)
        {
            this.title = title;
            this.pos = pos;
            this.dims = dims;
            this.game = game;
            this.action = action;
            selected = 0;
            this.TYPE = Ui_TYPE;

            if (Ui_TYPE == UI_TYPE.Input)
            {
                checkBox = new CheckBox(game, true, new Vector2(pos.X - dims.X, pos.Y), dims, Text.default_font, str, ActionInput, false);
                buttons.Add(checkBox);
            }

            else
            {
                throw new Exception("Type error");
            }

        }

        public void SetInput(string str)
        { 
            this.input= str;
        }

        public void ActionInput()
        {
            if (action != null) action();
            checkBox.button_text = this.input;

            if (checkBox.isHovered)
            {
                UpdateNextFrame = true;
            }
            else UpdateNextFrame= false;

        }



        public void ActionFunc()
        {
            selected = 1 - selected;
          

            //if (selected == 0)
            //{
            //    checkBox.ChangeFlag(false);
            //}

            //else
            //{
            //    checkBox.ChangeFlag(true);
            //}

        }


        public void Update()
        {

            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].ForceUpdate(Game1.adjustmousePos);
            }
        }

        public void AddOption(FormOption option)
        { 
            options.Add(option);
        }

        public void Draw(Sprites sprites, Vector2 offset, SpriteFont font=null)
        {

            if (font == null)
            {
                font = game.Content.Load<SpriteFont>(Text.default_font);
            }

            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].Draw(sprites);
            }

            Game1.NoAntiAliasingShader(Color.White);
            if (options.Count > selected && selected >= 0)
            {
                Vector2 strDims = font.MeasureString(options[selected].Name);
                sprites.DrawString(font, options[selected].Name,
                     new Vector2((int)(pos.X - strDims.X/2),(int)( pos.Y - strDims.Y/2)), Color.Black);
                strDims = font.MeasureString(title+": ");
                sprites.DrawString(font, title, new Vector2((int)(pos.X - (dims.X*2 + strDims.X/2)), (int)(pos.Y - strDims.Y/2)), Color.Black);

            }
        }

        public void ArrowLeftClick()
        {
            
            if (--selected < 0)
            {
                selected = 0;
            }
        }

        public void ArrowSelectedCenter()
        {
            selected = options.Count / 2;
        }

        public void ArrowRightClick() 
        {
            if (++selected >= options.Count)
            {
                selected = options.Count - 1;
            }

        }    


    }
}
