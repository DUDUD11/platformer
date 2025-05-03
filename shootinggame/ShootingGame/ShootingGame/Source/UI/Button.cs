using Flat;
using Flat.Graphics;
using Flat.Input;
using FlatPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;


namespace ShootingGame
{
    public class Button : UIEntity
    {
        public static string Button_path = "UI\\SimpleBtn";
        public Texture2D model;
        public Vector2 pos;
        public Vector2 dims;

        public bool isPressed, isHovered;
        public string textfont_path;
        public string button_text;
        public Color hoverColor;
        public SpriteFont textfont;

     
        public Action ButtonClicked;

        public Action<object> ButtonClickedObject;
        public readonly object info;

        public static Vector2 Default_Buttonsz = new Vector2(96, 32);
        public bool toggle;
        public Color ButtonTextColor { get; set; } = Color.Black;

        public Button(Game1 game, bool active, Vector2 pos, Vector2 dims, string textfont_path, string button_text, Action buttonClicked,bool toggle) : base(game, active)
        {
            this.active = active;
            this.model = game.Content.Load<Texture2D>(Button_path);
            this.pos = pos;
            this.dims = dims;
            this.textfont_path = textfont_path;
            this.textfont = game.Content.Load<SpriteFont>(textfont_path);
            this.button_text = button_text;
            ButtonClicked = buttonClicked;
            isPressed = false;
            isHovered = false;
            hoverColor =new Color(200,235,255);
            this.toggle = toggle;

        }

        public Button(Game1 game, string model, bool active, Vector2 pos, Vector2 dims, string textfont_path, string button_text, Action buttonClicked, bool toggle) : base(game, active)
        {
            this.active = active;
            this.model = game.Content.Load<Texture2D>(model);
            this.pos = pos;
            this.dims = dims;
            this.textfont_path = textfont_path;
            this.textfont = game.Content.Load<SpriteFont>(textfont_path);
            this.button_text = button_text;
            ButtonClicked = buttonClicked;
            isPressed = false;
            isHovered = false;
            hoverColor = new Color(200, 235, 255);
            this.toggle = toggle;

        }

        public Button(Game1 game, bool active, Vector2 pos, Vector2 dims, string textfont_path, string button_text, Action<object> ButtonClickedObject, bool toggle, object info = null) : base(game, active)
        {
            this.active = active;
            this.model = game.Content.Load<Texture2D>(Button_path);
            this.pos = pos;
            this.dims = dims;
            this.textfont_path = textfont_path;
            this.textfont = game.Content.Load<SpriteFont>(textfont_path);
            this.button_text = button_text;
            this.info = info;
            this.ButtonClickedObject = ButtonClickedObject;
            isPressed = false;
            isHovered = false;
            hoverColor = new Color(200, 235, 255);
            this.toggle = toggle;

        }

        public Button(Game1 game, string model , bool active, Vector2 pos, Vector2 dims, string textfont_path, string button_text, Action<object> ButtonClickedObject, bool toggle, object info = null) : base(game, active)
        {
            this.active = active;
            this.model = game.Content.Load<Texture2D>(model);
            this.pos = pos;
            this.dims = dims;
            this.textfont_path = textfont_path;
            this.textfont = game.Content.Load<SpriteFont>(textfont_path);
            this.button_text = button_text;
            this.info = info;
            this.ButtonClickedObject = ButtonClickedObject;
            isPressed = false;
            isHovered = false;
            hoverColor = new Color(200, 235, 255);
            this.toggle = toggle;

        }




        public override void ForceUpdate(Vector2 CursorPos)
        {
          
            if (!active) { return; }

            bool LeftClick = FlatMouse.Instance.IsLeftMouseButtonPressed();

            if (Hover(CursorPos))
            {
                isHovered = true;

                if (LeftClick)
                {
                    isHovered = false;
                    isPressed = true;
                }

                else if (FlatMouse.Instance.IsLeftMouseButtonReleased())
                {
                    RunBtnClick();

                }
            }

            else
            {
                isHovered = false;
            }

            if (!LeftClick && !FlatMouse.Instance.IsLeftMouseButtonDown())
            {
                isPressed = false;
            }
        }

        public void Reset()
        { 
            isPressed=false;
            isHovered=false;
        }

        public void RunBtnClick()
        {
            if (ButtonClickedObject != null)
            {
                if (info == null)
                {
                    throw new InvalidOperationException("shout not info null");
                }

                ButtonClickedObject.Invoke(info);

            }

            else if (ButtonClicked != null)
            {
                ButtonClicked();
            }

            Reset();

            if (toggle) {
                active = !active;
            }

        }

        public override void Draw(Sprites sprite)
        {
            if(!active) { return; }   

            Color tmpColor = Color.White;
            if (isPressed)
            {
                tmpColor = Color.Gray;
            }

            else if (isHovered)
            {
                tmpColor = hoverColor;
            }


            Game1.NoAntiAliasingShader(tmpColor);
            sprite.Draw(model, new Rectangle((int)(pos.X ), (int)(pos.Y), (int)dims.X, (int)dims.Y), tmpColor, 0f,
            new Vector2(model.Bounds.Width / 2, model.Bounds.Height / 2));

            if (button_text != null)
            {
                Vector2 ButtontextDims = textfont.MeasureString(button_text);
                sprite.DrawString(textfont, button_text, new Vector2(pos.X - ButtontextDims.X / 2, pos.Y - ButtontextDims.Y / 2), ButtonTextColor);
            }

        }

        public override bool Hover(Vector2 mousePosition)
        {
            if (!active) { return false; }

            FlatAABB button = new FlatAABB(pos.X - dims.X / 2, pos.Y - dims.Y / 2, pos.X + dims.X / 2, pos.Y + dims.Y / 2);
            FlatAABB mouse = new FlatAABB(mousePosition.X - 0.01f, mousePosition.Y - 0.01f, mousePosition.X + 0.01f, mousePosition.Y + 0.01f);

            if (Collisions.IntersectAABBs(button, mouse))
            {
                return true;
            }


            return false;
        }




    }
}



