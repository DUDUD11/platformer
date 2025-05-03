
using System;
using System.Runtime;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using ShootingGame;
using System.Xml.Linq;
using Flat.Graphics;
using FlatPhysics;
using System.Reflection;
using System.Reflection.Metadata;
using System.Threading;



namespace ShootingGame
{
    public class FrameAnimation
    {
        public string name;
        public bool hasFired; // 루프당 한번만 실행
        public int totalframes;
        public int currentFrame;
        public Vector2 sheet;
        public Vector2 startFrame;
        public Vector2 sheetFrame;
        public Vector2 spriteDims;
        public int sheetXsize;
        public float frameTimer;
        public double curTime;
        public int ActionFrame;
        public Action ActionFunc;
        public Rectangle Source_rectangle;
        public bool repeat = true;


        public FrameAnimation(Vector2 SpriteDims, Vector2 sheetDims, int sheetXsize, Vector2 start, int totalframes, int timePerFrame, string NAME)
        {
            spriteDims = SpriteDims;
            sheet = sheetDims;
            startFrame = start;
            sheetFrame = new Vector2(start.X, start.Y);
            this.totalframes = totalframes;
            currentFrame = 0;
            frameTimer = timePerFrame / 1000f;
            curTime = Game1.WorldTimer.Elapsed.TotalSeconds;
 

            name = NAME;
            ActionFunc = null;
            hasFired = false;
            ActionFrame = 0;
            this.sheetXsize = sheetXsize;
        }

        public FrameAnimation(Vector2 SpriteDims, Vector2 sheetDims, int sheetXsize, Vector2 start, int totalframes, int timePerFrame, int FIREFRAME, Action FIREACTION, string NAME)
        {
            spriteDims = SpriteDims;
            sheet = sheetDims;
            startFrame = start;
            sheetFrame = new Vector2(start.X, start.Y);
            this.totalframes = totalframes;
            currentFrame = 0;
            frameTimer = timePerFrame / 1000f;
            curTime = Game1.WorldTimer.Elapsed.TotalSeconds;
 
            name = NAME;
            ActionFunc = FIREACTION;
            hasFired = false;
            ActionFrame = FIREFRAME;
            this.sheetXsize = sheetXsize;
        }

        #region Properties
        public int Frames
        {
            get { return totalframes; }
        }
        public int CurrentFrame
        {
            get { return currentFrame; }
        }

        #endregion


        public void MoveForceNextFrame()
        {

           

            currentFrame = (CurrentFrame + 1) % totalframes;

            if (currentFrame == 0)
            {
                sheetFrame.X = startFrame.X;
                sheetFrame.Y = startFrame.Y;
                hasFired = false;
            }

            else
            {
                if ((int)sheetFrame.X + 1 >= sheetXsize)
                {
                    sheetFrame.X = 0;
                    sheetFrame.Y = sheetFrame.Y + 1;
                }

                else
                {
                    sheetFrame.X = sheetFrame.X + 1;
                }
            }


            if (ActionFunc != null && ActionFrame == currentFrame && !hasFired)
            {
                ActionFunc();
                hasFired = true;
            }

        }



        public void Update()
        {

            if (totalframes > 1)
            {
                double tmptime = Game1.WorldTimer.Elapsed.TotalSeconds;
                bool framestep = false;
                if (tmptime - curTime > frameTimer)
                {
                    curTime = tmptime;
                    framestep = true;
                }

                if (framestep)
                {
                    currentFrame = (CurrentFrame + 1) % totalframes;

                    if (currentFrame == 0)
                    {
                        if (repeat)
                        {

                            sheetFrame.X = startFrame.X;
                            sheetFrame.Y = startFrame.Y;
                            hasFired = false;
                        }

                        else
                        {
                            currentFrame = totalframes - 1;
                        }
                    }

                    else
                    {
                        if ((int)sheetFrame.X + 1 >= sheetXsize)
                        {
                            sheetFrame.X = 0;
                            sheetFrame.Y = sheetFrame.Y + 1;
                        }

                        else
                        {
                            sheetFrame.X = sheetFrame.X + 1;
                        }


                    }
                }
            }
      
            if(ActionFunc != null && ActionFrame == currentFrame && !hasFired)
            {
                ActionFunc();
                hasFired = true;
            }
        }

        public void Reset()
        {
            currentFrame = 0;
            sheetFrame.X = startFrame.X;
            sheetFrame.Y = startFrame.Y;
            hasFired = false;
        }

        public bool IsAtEnd()
        {
            if(currentFrame+1 >= totalframes)
            {
                return true;
            }
            return false;
        }

        public void Draw(Sprites sprite, Vector2 FrameSize, Texture2D model, Rectangle rectangle, float Angle, Color color)
        {
        
           // Game1.AntiAliasingShader(model,new Vector2(rectangle.Width,rectangle.Height),FrameSize);
            Source_rectangle = new Rectangle((int)(FrameSize.X*sheetFrame.X), (int)(FrameSize.Y * sheetFrame.Y), (int)FrameSize.X, (int)FrameSize.Y);
          
            int origin_x = (int)(model.Bounds.Width / (2 * sheet.X));
            int origin_y = (int)(model.Bounds.Height  / (2 * sheet.Y));

        

            sprite.Draw(model, rectangle, Source_rectangle, color, Angle,new Vector2(origin_x,origin_y));
      
        }

    }
}
