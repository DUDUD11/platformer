using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlatPhysics
{
    public readonly struct FlatAABB
    {
        public readonly FlatVector Min;
        public readonly FlatVector Max;
        public readonly static float HitBoxSize = 0.6f;

        public FlatAABB(FlatVector min, FlatVector max)
        { 
            Min = min; Max = max;
        }

        public FlatAABB(float minX, float minY, float maxX, float maxY)
        {
            Min = new FlatVector(minX, minY); 
            Max = new FlatVector(maxX, maxY);

        }

    }
}
