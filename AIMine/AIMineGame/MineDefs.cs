using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIMineComponent
{
    public enum State
    {
        /// <summary>
        /// Empty 至 Num8均为按下去之后的状态
        /// 右击事件转换顺序为 Normal -> Flag -> Dicey
        /// </summary>
        Normal = 16, //正常，即初始什么都不知道的状态
        Pressed = 15, // 被按下
        Flag = 14,  //标志为0
        Dicey = 13, //unknown status 0，带问号的状态
        Blast = 12, //爆炸状态
        Error = 11, // 错误状态
        Mine = 10, //雷状态，被标志出来为雷
        Dicey_down = 9, // unknown status 1, 即一个不知道状态的格子被按下去之后的状态。。。。
      
        Num8 = 8, //周围有8雷 
        Num7 = 7,
        Num6 = 6,
        Num5 = 5,
        Num4 = 4,
        Num3 = 3,
        Num2 = 2,
        Num1 = 1,
        Empty = 0, //周围无雷
    }
    public enum Attrib
    {
        Empty,
        Mine
    }
    public enum GameState
    {
        Normal,
        Victory,
        Lose
    }
}
