using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlatPhysics;
using Flat;

namespace ShootingGame
{
    public class SoundItem
    {
        public float volume;
        public string name;
        public SoundEffect sound;
        public SoundEffectInstance instance;
        private Game1 game;

        public SoundItem (Game1 game,string Name, string soundpath, float volume)
        { 
            this.game = game;
            name = Name;
            this.volume = FlatUtil.Clamp(volume,0f,1f);
            sound = game.Content.Load<SoundEffect>(soundpath);
            CreateInstance();
        
        }

        public SoundItem(SoundItem soundItem)
        {
            name = soundItem.name;
            this.volume = FlatUtil.Clamp(soundItem.volume, 0f, 1f);
            sound = soundItem.sound;
            CreateInstance();
        }

        public void CreateInstance()
        {
            instance = sound.CreateInstance();
        }

    }
}
