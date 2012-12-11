using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIMineComponent
{
    class MineDef
    {
        static int DEFAULT_LEVEL = 0;
        static int STATE_NORMAL = 0; //正常
        static int STATE_FLAG = 1;//标志为雷
        static int STATE_DICEY = 2;    //未知状态0
        static int STATE_BLAST = 3;    //爆炸状态
        static int STATE_ERROR = 4;   //错误状态
        static int STATE_MINE = 5;   //雷状态
        static int STATE_DICEY_DOWN = 6;  //未知状态1
        static int STATE_NUM8 = 7;  //周围有8雷
        static int STATE_NUM7 = 8; 
        static int STATE_NUM6 = 9; 
        static int STATE_NUM5 = 10;
        static int STATE_NUM4 = 11;
        static int STATE_NUM3 = 12;
        static int STATE_NUM2 = 13;
        static int STATE_NUM1 = 14;
        static int STATE_EMPTY = 15;  //无雷
        static int ATTRIB_EMPTY	= 0;
        static int ATTRIB_MINE = 1;
        static int GS_WAIT = 0;
        static int GS_RUN = 1;
        static int GS_DEAD = 2;
        static int GS_VICTORY = 3;
        static int DC_STRIKE_MINE = 0;
        static int DC_ERROR_MINE = 1; 
    }
}
