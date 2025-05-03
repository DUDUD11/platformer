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
    public class CheckBox : UIAnimation
    {
        public static string Checkbox_path = "UI\\Animation\\CheckBox";
        public static Vector2 Default_Checkboxsz = new Vector2(96, 32);
        public static Vector2 CheckBox_Frames = new Vector2(1,2);

        public bool isPressed, isHovered;
        public string textfont_path;
        public string button_text;
        public Color hoverColor;
        public SpriteFont textfont;
        public Action CheckBoxClicked;
        private bool Checked;
        private bool InputBox;

        public CheckBox(Game1 game, bool active, Vector2 pos, Vector2 dims, string textfont_path, string button_text, Action buttonClicked,bool Checked) :
                base(game, active, Checkbox_path, pos, dims, CheckBox_Frames, 2, int.MaxValue, null)
        {


            this.textfont_path = textfont_path;
            this.textfont = game.Content.Load<SpriteFont>(textfont_path);
            this.button_text = button_text;
            CheckBoxClicked = buttonClicked;
            isPressed = false;
            isHovered = false;
            hoverColor = new Color(200, 235, 255);
            this.Checked = Checked;
            if(Checked)
            AnimationChange(Checked);

            if (button_text != null)
            { 
                InputBox=true;
            }


        }



        public override void ForceUpdate(Vector2 CursorPos)
        {

            if (!active) { return; }
            

            base.ForceUpdate(CursorPos);

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
            isPressed = false;
        //    isHovered = false;
        }

        public void ChangeFlag(bool flag)
        {
            Checked = flag;
            AnimationChange(flag);
        }


        public void AnimationChange(bool flag)
        {
            SetAnimationFrameDefault(flag);
        }


        public void RunBtnClick()
        {

            if (InputBox)
            {
                CheckBoxClicked();
            }

            else if (CheckBoxClicked != null)
            {
                Checked = !Checked;
                AnimationChange(Checked);
                CheckBoxClicked();
            }

            Reset();

        }

        public override void Draw(Sprites sprite)
        {
            if (!active) { return; }

     

            Color tmpColor = Color.White;
            if (isPressed)
            {
                tmpColor = Color.Gray;
            }

            else if (isHovered)
            {
                tmpColor = hoverColor;
            }

            base.Draw(sprite,tmpColor);


            if (button_text != null)
            {
                Vector2 ButtontextDims = textfont.MeasureString(button_text);
                sprite.DrawString(textfont, button_text, new Vector2(pos.X - ButtontextDims.X / 2, pos.Y - ButtontextDims.Y / 2), Color.Black);
            }

        }

        public override bool Hover(Vector2 mousePosition)
        {
            if (!active) { return false; }

            FlatAABB button = new FlatAABB(pos.X - dims.X / 2, pos.Y - dims.Y, pos.X + dims.X / 2, pos.Y + dims.Y / 2);
            FlatAABB mouse = new FlatAABB(mousePosition.X - 0.01f, mousePosition.Y - 0.01f, mousePosition.X + 0.01f, mousePosition.Y + 0.01f);

            if (Collisions.IntersectAABBs(button, mouse))
            {
                return true;
            }


            return false;
        }
    }
}



