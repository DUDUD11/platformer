using Flat;
using Flat.Graphics;
using FlatPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

using Color = Microsoft.Xna.Framework.Color;
using FlatMath = Flat.FlatMath;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
namespace ShootingGame
{
    public class UIAnimation : UIEntity
    {
        public Vector2 frames;
        public readonly Vector2 FrameSize;
        private List<FrameAnimation> FrameAnimationList = new List<FrameAnimation>();
        public bool AnimationFlag;
        public int currentAnimation;

        public Vector2 pos, dims;
        public Texture2D model;


        public UIAnimation(Game1 game, bool active, string path, Vector2 pos,Vector2 dims, Vector2 frames) : base (game,active) 
        
        {
            this.model = game.Content.Load<Texture2D>(path);
            this.frames = frames;
            this.pos = pos;
            this.dims = dims;

            FrameSize = new Vector2((int)(model.Bounds.Width / frames.X), (int)(model.Bounds.Height / frames.Y));
            currentAnimation = 0;
            AnimationFlag = true;
            
        }

        public UIAnimation(Game1 game, bool active, string path, Vector2 pos, Vector2 dims, Vector2 frames, int curAnimation) : base(game, active)

        {
            this.model = game.Content.Load<Texture2D>(path);
            this.frames = frames;
            this.pos = pos;
            this.dims = dims;
            this.currentAnimation = curAnimation;   

            FrameSize = new Vector2((int)(model.Bounds.Width / frames.X), (int)(model.Bounds.Height / frames.Y));
            AnimationFlag = true;

        }

        public UIAnimation(Game1 game, bool active, string path, Vector2 pos, Vector2 dims, Vector2 frames, int totalframe, int millitimePerFrame, string name = null)
        : base(game, active)
        {
            this.model = game.Content.Load<Texture2D>(path);
            this.frames = frames;
            this.pos = pos;
            this.dims = dims;

            FrameSize = new Vector2((int)(model.Bounds.Width / frames.X), (int)(model.Bounds.Height / frames.Y));
            currentAnimation = 0;
            AnimationFlag = true;

            AddAnimation(Vector2.Zero, totalframe, millitimePerFrame, name);

        }

        public void AddAnimation(Vector2 start, int totalframes, int millitimePerFrame, string NAME = "Default")
        {

            FrameAnimation frameAnimation = new FrameAnimation(FlatMath.VectorZero, frames, (int)frames.X, start, totalframes, millitimePerFrame, NAME);
            FrameAnimationList.Add(frameAnimation);
        }

        public void AddAnimation(FrameAnimation frameAnimation)
        {
            FrameAnimationList.Add(frameAnimation);
        }

        public bool DeleteAnimation(string animationanme)
        {
            for (int i = 0; i < FrameAnimationList.Count; i++)
            {
                if (FrameAnimationList[i].name == animationanme)
                {
                    FrameAnimationList.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        public void ChangeCurrentAnimation(int frame)
        {
            if (frame >= frames.X * frames.Y)
            {
                throw new ArgumentException("Exceed frame Size");
            }

            this.currentAnimation = frame;

        }

        public void SetAnimationFlag(bool flag)
        {
            AnimationFlag = flag;
        }

        public override void ForceUpdate(Vector2 CursorPos)
        {
            if (AnimationFlag && FrameAnimationList.Count > 0)
            {
                FrameAnimationList[currentAnimation].Update();
            }

        }

        public int GetAnimationFromName(string ANIMATIONNAME)
        {
            for (int i = 0; i < FrameAnimationList.Count; i++)
            {
                if (FrameAnimationList[i].name.Equals(ANIMATIONNAME))
                {
                    return i;
                }
            }

            return -1;
        }

        public bool SetAnimationFrameDefault(bool flag)
        {
            if (FrameAnimationList.Count > 0 && FrameAnimationList[0] != null)
            {
          
                FrameAnimationList[0].MoveForceNextFrame();
          

                return true;
            }
            return false;
        }
        public void SetAnimationByName(string NAME)
        {
            int tempAnimation = GetAnimationFromName(NAME);

            if (tempAnimation == -1)
            {
                throw new Exception("NO Animation Found");
            }

            if (tempAnimation != currentAnimation)
            {
                FrameAnimationList[tempAnimation].Reset();
            }

            currentAnimation = tempAnimation;
        }

        public override void Draw(Sprites sprite,Color color)
        {

            if (AnimationFlag && FrameAnimationList.Count != 0 && FrameAnimationList[currentAnimation].Frames > 0)
            {
                FrameAnimationList[currentAnimation].Draw(sprite, FrameSize, model, new Rectangle((int)(pos.X), (int)(pos.Y), (int)dims.X, (int)dims.Y),0f, color);
            }
            else
            {
                Rectangle Source_rectangle = new Rectangle(0, 0, (int)FrameSize.X, (int)FrameSize.Y);
                sprite.Draw(model, new Rectangle((int)(pos.X), (int)(pos.Y), (int)dims.X, (int)dims.Y), Source_rectangle, color, 0f,
                    new Vector2((int)(model.Bounds.Width / (2 * frames.X)), (int)(model.Bounds.Height / (2 * frames.Y))));

            }
        }


    }
}

