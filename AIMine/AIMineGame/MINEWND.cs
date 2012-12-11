using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIMineComponent
{
    public class MINEWND
    {
        public int uRow { get; set; }         //所在雷区二维数组的行
        public int uCol { get; set; }         //所在雷区二位数组的列
        public State uState { get; set; }       //当前状态
        public Attrib uAttrib { get; set; }      //方块属性
        public State uOldState { get; set; }    //历史状态
    }
}
