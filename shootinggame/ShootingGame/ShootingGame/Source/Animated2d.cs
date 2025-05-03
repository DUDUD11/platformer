using Flat;
using Flat.Graphics;
using FlatPhysics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Color = Microsoft.Xna.Framework.Color;
using FlatMath = Flat.FlatMath;
using Rectangle = Microsoft.Xna.Framework.Rectangle;



namespace ShootingGame
{

    public struct ani_set
    {
        public Vector2 Frames;
        public Vector2 FrameSize;
        public string path;

        public ani_set(Vector2 frame,Vector2 size, string path)
        { 
            this.Frames = frame;
            this.FrameSize = size;
            this.path = path;   
        }

    }

    public class Animated2d : SpriteEntity
    {
        public readonly int AnimationNum;
        public List<ani_set> Animation_Set;
        private List<FrameAnimation> FrameAnimationList;
        public bool AnimationFlag;
        public int currentAnimation;


        //public Animated2d(Game1 game, string path, Vector2 init_pos, Vector2 dims, FlatWorld.Wolrd_layer wolrd_Layer
        //    , Vector2 frames, int animationNum)
        //    : base(game, path, init_pos, dims, wolrd_Layer)
        //{
        //    this.AnimationNum = animationNum;
        //    this.Animation_Set = new List<ani_set>(animationNum);
        //    this.Animation_Set[0] = new ani_set(frames, new Vector2((int)(model.Bounds.Width / frames.X), (int)(model.Bounds.Height / frames.Y)), path);

        //    currentAnimation = 0;

        //    FrameAnimationList = new List<FrameAnimation>();
        //    AnimationFlag = true;
           
        //}

    //    public Animated2d(Game1 game, string path, Vector2 init_pos, Vector2 dims, FlatWorld.Wolrd_layer wolrd_Layer,
    //                        Vector2 frames, int curAnimation,int animationNum)
    //: base(game, path, init_pos, dims, wolrd_Layer)
    //    {
    //        this.frames = frames;
    //        FrameAnimationList = new List<FrameAnimation>();
    //        AnimationFlag = true;
    //        currentAnimation = curAnimation;
    //        FrameSize = new Vector2((int)(model.Bounds.Width / frames.X), (int)(model.Bounds.Height / frames.Y));
    //        AnimationNum = animationNum;
    //    }

        public Animated2d(Game1 game, string path, Vector2 init_pos, Vector2 dims,  FlatWorld.Wolrd_layer wolrd_Layer,
                            Vector2 frames, int animationNum, int totalframe, int millitimePerFrame, string name)
        : base(game, path, init_pos, dims, wolrd_Layer)
        {
            this.AnimationNum = animationNum;
            this.Animation_Set = new List<ani_set>(animationNum);
        
            FrameAnimationList = new List<FrameAnimation>(animationNum);
            AnimationFlag = true;
            currentAnimation = 0;

           
          
            AddAnimation(frames, path, totalframe, millitimePerFrame, name,currentAnimation);
            
        }

        // public Animated2d(Game1 game, string path, Vector2 init_pos, Vector2 dims, Vector2 velocity, int animationnum, FlatWorld.Wolrd_layer wolrd_Layer,
        //Vector2 frames)
        //: base(game, path, init_pos, dims, velocity, wolrd_Layer)
        // {
        //     this.frames = frames;
        //     this.AnimationNum=animationnum;
        //     FrameAnimationList = new List<FrameAnimation>();
        //     AnimationFlag = true;
        //     currentAnimation = 0;
        //     FrameSize = new Vector2((int)(model.Bounds.Width / frames.X), (int)(model.Bounds.Height / frames.Y));
        // }

        public void Set_repeat(int idx, bool flag)
        {
            FrameAnimationList[idx].repeat = flag;
        }

        public void AddAnimation(Vector2 frames, string path, int totalframes, int millitimePerFrame, string NAME, int idx)
        {
            // model bound와 height는 변하지 않는다고 가정할때
            if (idx == 0)
            {
                Animation_Set.Add(new ani_set(frames, new Vector2((int)(model.Bounds.Width / frames.X), (int)(model.Bounds.Height / frames.Y)), path));
            }

            else
            {
                Animation_Set.Add(new ani_set(frames, Animation_Set[0].FrameSize, path));
            }
            FrameAnimationList.Add(new FrameAnimation(FlatMath.VectorZero, Animation_Set[idx].Frames, (int)Animation_Set[idx].Frames.X, Vector2.Zero,totalframes, millitimePerFrame, NAME));
        }

        //public void AddAnimation(FrameAnimation frameAnimation)
        //{
        //    FrameAnimationList.Add(frameAnimation);
        //}

        //public void SetAnimationList(List<FrameAnimation> frameAnimations)
        //{
        //    this.FrameAnimationList = frameAnimations;
        //}

        public bool DeleteAnimation(string animationanme)
        {
            for (int i = 0; i < FrameAnimationList.Count; i++)
            {
                if (FrameAnimationList[i].name == animationanme)
                {
                    FrameAnimationList.RemoveAt(i);
                    Animation_Set.RemoveAt(i);  
                    return true;
                }
            }
            return false;
        }

        public void ChangeCurrentAnimation(int animationNum)
        {
            FrameAnimationList[currentAnimation].Reset();
            this.currentAnimation = animationNum;
            base.UpdateModel(Animation_Set[currentAnimation].path);
        }

        public void SetAnimationFlag(bool flag)
        {
            AnimationFlag = flag;
        }

        public Rectangle Src_Rectangle()
        {
            return FrameAnimationList[currentAnimation].Source_rectangle;
        }

        public Vector2 Get_Sheet()
        {
            return FrameAnimationList[currentAnimation].sheet;
        }

        public Vector2 Get_SheetFrame()
        {
            return FrameAnimationList[currentAnimation].sheetFrame;
        }


        public override void Update()
        {
            if (AnimationFlag && FrameAnimationList.Count > 0)
            {
                FrameAnimationList[currentAnimation].Update();
            }
        
        }

        public string GetCurrentAnimationModelPath()
        {
            return Animation_Set[currentAnimation].path;
        }


        public virtual int GetAnimationFromName(string ANIMATIONNAME)
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

        public override void Draw(Sprites sprite, Vector2 o, float Angle)
        {

         //   Game1.AntiAliasingShader(model, dims, Animation_Set[currentAnimation].FrameSize);

            if (AnimationFlag && FrameAnimationList.Count !=0 && FrameAnimationList[currentAnimation].Frames > 0)
            {
                FrameAnimationList[currentAnimation].Draw(sprite, Animation_Set[currentAnimation].FrameSize, model, new Rectangle((int)(pos.X + o.X), (int)(pos.Y + o.Y), (int)dims.X, (int)dims.Y), Angle, Color.White);
            }
            else
            {
        
                Rectangle Source_rectangle = new Rectangle(0, 0, (int)Animation_Set[currentAnimation].FrameSize.X, (int)Animation_Set[currentAnimation].FrameSize.Y);
                 sprite.Draw(model, new Rectangle((int)(pos.X + o.X), (int)(pos.Y + o.Y), (int)dims.X, (int)dims.Y), Source_rectangle, Color.White, Angle,
                     new Vector2((int)(model.Bounds.Width/(2* Animation_Set[currentAnimation].Frames.X)), (int)(model.Bounds.Height/(2* Animation_Set[currentAnimation].Frames.Y))));
                
            }
        }

    }
}
