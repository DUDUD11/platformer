using Flat.Graphics;
using Flat.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace ShootingGame
{
    public class TextInputBox : UIEntity
    {
        private Game1 game;
        private Texture2D model;
        private SpriteFont font;
        private string currentInput = ""; // 현재 입력된 문자열
        private string hint = "";
        
        public Vector2 pos,dims; // 입력 박스 위치와 크기
    
        private Microsoft.Xna.Framework.Color boxColor = Color.Gray; // 입력 박스 색상
        private Color textColor = Color.Black; // 텍스트 색상
        private bool isFocused = false; // 입력 박스가 포커스된 상태인지 확인
        private HashSet<Keys> keyDownState = new HashSet<Keys>();
        
      


        public TextInputBox(Game1 game,bool active, Vector2 pos,Vector2 dims, string currentInput = null) : base(game,active)
        {   
            this.game = game;

            font = game.Content.Load<SpriteFont>(Text.default_font);
            this.pos = pos;
            this.dims = dims;
            if (currentInput != null)
            {
                this.currentInput = currentInput;
            }

            this.model = game.Content.Load<Texture2D>("UI\\solid");

        }

        public TextInputBox(Game1 game, bool active, string hint, Vector2 pos, Vector2 dims, string currentInput = null) : base(game, active)
        {
            this.game = game;

            font = game.Content.Load<SpriteFont>(Text.default_font);
            this.pos = pos;
            this.dims = dims;
            if (currentInput != null)
            {
                this.currentInput = currentInput;
            }
            this.hint = hint;
            this.model = game.Content.Load<Texture2D>("UI\\solid");

        }

        public override void ForceUpdate(Vector2 mousePos)
        {
            MouseHover(mousePos);

            if (isFocused && FlatKeyboard.Instance.IsKeyAvailable)
            {

                    foreach (var key in FlatKeyboard.Instance.GetPressedKeys())
                    {

                        if(key == Keys.Back && currentInput.Length > 0)
                        {
                            currentInput = currentInput.Substring(0, currentInput.Length - 1);
                        }

                        if (keyDownState.Contains(key)) continue;

                    if (key >= Keys.A && key <= Keys.Z) // 알파벳 처리 (대소문자)
                    {
                        currentInput += key.ToString(); // 입력된 문자를 추가
                    }
                    else if (key >= Keys.D0 && key <= Keys.D9) // 숫자 키 처리
                    {
                        currentInput += (key - Keys.D0).ToString();
                    }
                    else if (key == Keys.Space) // 공백 처리
                    {
                        currentInput += " ";
                    }

                    else if (key == Keys.OemMinus)
                    {
                        currentInput += "-";
                    }

                        // 기타 특수문자 처리 (예: !, @, # 등)도 필요하면 추가 가능

                        keyDownState.Add(key);
                    }


                foreach (var key in keyDownState.ToArray())
                {
                    if (!FlatKeyboard.Instance.GetPressedKeys().Contains(key))
                    {
                        // 키가 떼어진 경우 상태를 초기화
                        keyDownState.Remove(key);
                    }
                }
            }


        }


        public override void Draw(Sprites sprite)
        {

            Game1.NoAntiAliasingShader(Color.White);

            sprite.Draw(model, new Rectangle((int)(pos.X), (int)(pos.Y), (int)dims.X, (int)dims.Y), boxColor, 0f,
               new Vector2(model.Bounds.Width / 2, model.Bounds.Height / 2));

            if (currentInput != null && !currentInput.Equals(""))
            {
                Vector2 TextInputDims = font.MeasureString(currentInput);
                sprite.DrawString(font, currentInput, new Vector2(pos.X - TextInputDims.X / 2, pos.Y - TextInputDims.Y / 2), Color.Black);

            }

            else if(hint != null && !hint.Equals(""))
            {
                Vector2 HintDims = font.MeasureString(hint);
                sprite.DrawString(font, hint, new Vector2(pos.X - HintDims.X / 2, pos.Y - HintDims.Y / 2), Color.Red);
            }
        
        }

        public void MouseHover(Vector2 mousePosition)
        {
            Rectangle rectangle = new Rectangle((int)(pos.X-dims.X/2), (int)(pos.Y-dims.Y/2), (int)dims.X, (int)dims.Y);
            // 마우스 클릭 위치가 입력 박스 내부라면 포커스를 얻음
            if (rectangle.Contains(mousePosition.ToPoint()))
            {
                isFocused = true;
            }
            else
            {
                isFocused = false;
                keyDownState.Clear();
            }
        }

        public string GetInput()
        {
            return currentInput;
        }

        public void SetInput(string str)
        {
            this.currentInput = str;
        }


    }
}
