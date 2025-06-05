using Flat;
using Flat.Graphics;
using FlatPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ShootingGame;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

//여유되면 자동 text넘김도 구현

namespace ShootingGame
{
    class Dialog : UIEntity
    {
        public static string default_Dialog = "UI\\Icons\\Dialog";

        public readonly Texture2D texture;
        public readonly TextureUI picture;
        public readonly SpriteFont font;

        private readonly Game1 game;
        private Rectangle[] _sourceRectangles;
        private Rectangle[] _destinationRectangles;
        private Vector2 _position;

        public int Width { get; private set; }
        public int Height { get; private set; }

        public Color TextColor { get; set; } = Color.White;

        private Vector2 _textPosition;
     
        private string _text;
        private  Point _cornerSize;


        private Button EnterButton;
        private string EnterButton_url = "UI\\Icons\\EnterButton";
        private string _EnterText = "Enter";
        private Vector2 _EnterTextPosition;
        private Vector2 EnterButtonDims;

 


        public bool Destroy;

        /*
         678
         345
         012
         */

        public void CalculateDestinationRectangles()
        {
            Vector2 textSize = font.MeasureString(_text);
            Width = (int)textSize.X + 2 * _cornerSize.X;
            Height = (int)textSize.Y + 2 * _cornerSize.Y;

            if (Width < Game1.screen_width) Width = (int)(Game1.screen_width/1.5f);


            int x = (int)_position.X-Width/2;
            int y = (int)_position.Y-Height/2;
            int w = Width - 2 * _cornerSize.X;
            int h = Height - 2 * _cornerSize.Y;

            _textPosition = new(x + _cornerSize.X, y + _cornerSize.Y);


            _EnterTextPosition = new(x + Width - _cornerSize.X, (2*y + _cornerSize.Y)/2);
            EnterButton.pos = _EnterTextPosition;

         

            _destinationRectangles[0] = new(x, y, _cornerSize.X, _cornerSize.Y);
            _destinationRectangles[1] = new(x + _cornerSize.X, y, w, _cornerSize.Y);
            _destinationRectangles[2] = new(x + Width - _cornerSize.X, y, _cornerSize.X, _cornerSize.Y);
            _destinationRectangles[3] = new(x, y + _cornerSize.Y, _cornerSize.X, h);
            _destinationRectangles[4] = new(x + _cornerSize.X, y + _cornerSize.Y, w, h);
            _destinationRectangles[5] = new(x + Width - _cornerSize.X, y + _cornerSize.Y, _cornerSize.X, h);
            _destinationRectangles[6] = new(x, y + Height - _cornerSize.Y, _cornerSize.X, _cornerSize.Y);
            _destinationRectangles[7] = new(x + _cornerSize.X, y + Height - _cornerSize.Y, w, _cornerSize.Y);
            _destinationRectangles[8] = new(x + Width - _cornerSize.X, y + Height - _cornerSize.Y, _cornerSize.X, _cornerSize.Y);

          
        }
        //from x
        //conrnersize,Width - 2 * _cornerSize,_cornersize

        public void Init_SourceRectangle()
        {

            int corner = (_cornerSize.X + _cornerSize.Y) / 6;
            int centerW = texture.Width - corner * 2;
            int centerH = texture.Height - corner * 2;

            _sourceRectangles[0] = new(0, texture.Height - corner, corner, corner); // Bottom-left
            _sourceRectangles[1] = new(corner, texture.Height - corner, centerW, corner); // Bottom
            _sourceRectangles[2] = new(texture.Width - corner, texture.Height - corner, corner, corner); // Bottom-right

            _sourceRectangles[3] = new(0, corner, corner, centerH); // Left
            _sourceRectangles[4] = new(corner, corner, centerW, centerH); // Center
            _sourceRectangles[5] = new(texture.Width - corner, corner, corner, centerH); // Right

            _sourceRectangles[6] = new(0, 0, corner, corner); // Top-left
            _sourceRectangles[7] = new(corner, 0, centerW, corner); // Top
            _sourceRectangles[8] = new(texture.Width - corner, 0, corner, corner); // Top-right

        }
        public Dialog(Game1 game, bool active, Point cornerSize, Vector2 pos, string text,string picture)
            : base(game, active)
        {
            this.game = game;
            texture = game.Content.Load<Texture2D>(default_Dialog);
          
            font = game.Content.Load<SpriteFont>(Text.default_font);

            this._text = text;
            this._position = pos;
       
            this._cornerSize = cornerSize;
            _sourceRectangles = new Rectangle[9];
            _destinationRectangles = new Rectangle[9];

            EnterButtonDims = new Vector2(_cornerSize.X / 3, _cornerSize.Y / 3);
           
            EnterButton = new Button(game, EnterButton_url, true, new(0, 0), EnterButtonDims, Text.default_font, _EnterText, EnterButtonClicked,false);
            EnterButton.ButtonTextColor = Color.Green;
      
            Init_SourceRectangle();
            CalculateDestinationRectangles();


            int start_x = (_destinationRectangles[0].X + _destinationRectangles[1].X )/2;
            int end_x = (_destinationRectangles[1].X + _destinationRectangles[2].X )/2;
            int start_y = _destinationRectangles[3].Y;
            int end_y = (_destinationRectangles[0].Height * 2);



            this.picture = new TextureUI(game, true,picture,new Vector2(start_x,start_y),new Vector2(end_x-start_x,end_y-start_y),Color.White);

        }

        public void EnterButtonClicked()
        {
            game.Dialog_Close();
        }

        public void SetText(string text)
        {
            _text = text;
            CalculateDestinationRectangles();
        }

        public void SetPosition(Vector2 position)
        {
            _position = position;
            CalculateDestinationRectangles();
        }



        public void Update(Vector2 cursorPos)
        {
            EnterButton.ForceUpdate(cursorPos);
        }


        public override void Draw(Sprites sprite)
        {

          
            for (int i = 0; i < _sourceRectangles.Length; i++)
            {
                sprite.Draw(texture, _destinationRectangles[i],_sourceRectangles[i], Color.White);
            }




            picture.Draw(sprite,Vector2.Zero);


            sprite.DrawString(font, _text, _textPosition, TextColor);


            EnterButton.Draw(sprite);


        }

   


    }
}
