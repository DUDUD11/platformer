using Flat;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingGame
{
    public static class SoundControl
    {
        public static SoundItem BkgSound;
        public static SoundItem EffectSound;
        public static float EffectSound_Volume = 0.5f;
        public static List<SoundItem> Sounds =new List<SoundItem>();
     
        private static void PlayBackgroundMusic()
        {
            if (BkgSound != null)
            {
                BkgSound.instance.Volume = BkgSound.volume;
                BkgSound.instance.IsLooped = true;
                BkgSound.instance.Play();
            }
            else
            {
                throw new Exception("bkgmusic not set");
            }
        }

        public static void OptionMenuSoundVolume(Object param)
        {
            if (param is obj_tofloat obj)
            {
                ChagneBackgroundVolume(obj.first);
                ChangeEffectSoundVolume(obj.second);
            }
        
        }

        public static void ChagneBackgroundVolume(float val)
        {
            val = FlatUtil.Clamp(val, 0f, 1f);
            if (BkgSound != null) 
            {
                BkgSound.volume = val;
                BkgSound.instance.Volume = val;
            }
        }

        public static void ChangeEffectSoundVolume(float val)
        { 
            val = FlatUtil.Clamp(val, 0f, 1f);
            EffectSound_Volume = val;
            if (EffectSound != null)
            {
          //      EffectSound.volume = val ;
                EffectSound.instance.Volume = val * EffectSound.volume;
            }

        }


        private static void PlayEffectMusic()
        {
            if (EffectSound != null)
            {
                EffectSound.instance.Volume = EffectSound_Volume;
                EffectSound.instance.Play();
            }
            else
            {
                throw new Exception("EffectSound not set");
            }
        }

        public static void BkgMusicChange(Game1 game, string name, string path,float volume)
        {
            BkgSound = new SoundItem(game, name, path, volume);
            PlayBackgroundMusic();
        }

        private static void SoundChange(SoundItem item)
        {
            EffectSound = new SoundItem(item);
            PlayEffectMusic();
        }

        public static void SoundAdd(SoundItem item)
        {
            Sounds.Add(item);
        }

        public static bool SoundChange(string name)
        {
            for (int i = 0; i < Sounds.Count; i++)
            {
                if (Sounds[i].name.Equals(name))
                { 
                    SoundChange(Sounds[i]);
                    return true;
                }
            }
            return false;
        }

    }
}
