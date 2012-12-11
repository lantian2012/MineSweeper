using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIMineComponent
{
    public enum State
    {
        Normal,
        Flag,
        Dicey,
        Blast,
        Error,
        Mine,
        Dicey_down,
        Num8,
        Num7,
        Num6,
        Num5,
        Num4,
        Num3,
        Num2,
        Num1,
        Empty
    }
    public enum Attrib
    {
        Empty,
        Mine
    }
}
