using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public struct FloatRange
{
    public float min;
    public float max;

    public float Delta { get { return max - min; } }

    public void ClampIncreasing()
    {
        if (max < min)
            max = min;
    }
}
