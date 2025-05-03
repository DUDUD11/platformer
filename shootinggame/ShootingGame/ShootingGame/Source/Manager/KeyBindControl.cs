using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ShootingGame
{
    public struct KeyBind
    {
     
        public string name;
        public int key;

        public KeyBind(string name, int key)
        { 
            this.name = name;
            this.key = key; 
        }

        public void setkey(int key) { this.key = key; }

    }

    public enum keys_order
    { 
        left = 0,
        right = 1,
        top = 2,
        down = 3,

    
    }



    public static class KeyBindControl
    {
        public static List<KeyBind> keys = new List<KeyBind>();
        public static Keys[] keysToCheck = new Keys[] { Keys.Q, Keys.W, Keys.E, Keys.R, Keys.A, Keys.S, Keys.D, Keys.F, Keys.G, Keys.H, Keys.Z, Keys.X, Keys.C, Keys.V, Keys.B};
        public static bool ChangeKey(string name, int key)
        {
           
            int idx = getKeyBindName(name);
            
            for (int i = 0; i < keys.Count; i++)
            {
                KeyBind keyBind = keys[i];

                if (keyBind.key == key)
                {
                    if (idx==-1)
                    {
                        throw new Exception("logical error");
                    }

                    return false;

                    //int tmp = kb.key;
                    //kb.key = keyBind.key;
                    //keyBind.key = tmp;

                    //return;
                }
            
            }

            KeyBind tmp = keys[idx];
            tmp.setkey(key);
            keys[idx]= tmp;

            return true;
        }

        public static Keys Keyval(int x)
        {
            return (Keys)Enum.ToObject(typeof(Keys), x);
        }

        public static Keys getKeyByName(string name)
        {
            for (int i = 0; i < keys.Count; i++)
            {
                if (keys[i].name.Equals(name))
                {
                    return (Keys)Enum.ToObject(typeof(Keys), keys[i].key);
                }
            }

            return Keys.None; // Keys.None을 반환하여 기본값을 제공
        }


        public static int getKeyBindName(string name)
        {
          

            for (int i = 0; i < keys.Count; i++)
            {
                if (keys[i].name.Equals(name))
                {
                    return i;
                }
            }

            return -1;
      
        }


    }
}
