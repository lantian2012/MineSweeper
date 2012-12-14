using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AIMineComponent
{
   public  class SweeperAgent
    {
        #region Properties

        public SimpleList KB;
        private SimpleList Requested;
        private int MaxDepth = 100; //表示最多寻找的解的个数
       //每次进行Deduction的时候，Estimation新建并且刷新为零。
        private SimpleList Output;
        public IList<IList<MINEWND>> m_pMines { get; set; }
        private int m_uXNum { get; set; }				// X方向小方块个数
        private int m_uYNum { get; set; }				// Y方向小方块个数

        #endregion

        #region Initialize, binding m_PMines
        public SweeperAgent(  IList<IList<MINEWND>> Mines, int XNum, int YNum) 
        {
            KB = new SimpleList(XNum * YNum,3);
            Requested = new SimpleList(XNum * YNum,2);
            Output = new SimpleList(XNum * YNum,2);
            m_uXNum = XNum;
            m_uYNum = YNum;
            m_pMines = new MINEWND[m_uXNum][];
            foreach (var i in Enumerable.Range(0, m_uXNum))
                m_pMines[i] = new MINEWND[m_uYNum];
            
            foreach (var i in Enumerable.Range(0, m_uXNum))
            {
                foreach (var j in Enumerable.Range(0, m_uYNum))
                {
                    m_pMines[i][j] = Mines[i][j];
                }
            }
            //Now the m_PMines are bounded with the one in Game.cs!!!
            /*
            foreach (var i in Enumerable.Range(0, m_uXNum))
            {
                foreach (var j in Enumerable.Range(0, m_uYNum))
                {
                    m_pMines[i][j].uAttrib = Attrib.Mine;
                }
            }
             * */
        }
        #endregion

        #region usefulFunctions

       /// <summary>
       /// 得到有数字的格子的信息，先清空
       /// </summary>
        private void getKB()
        {
            KB.Clear();
            foreach (var i in Enumerable.Range(0, m_uXNum))
            {
                foreach (var j in Enumerable.Range(0, m_uYNum))
                {
                    if (((int)m_pMines[i][j].uState) >= 1 && ((int)m_pMines[i][j].uState) <= 8)
                    {
                        int[] b = { i, j, (int)m_pMines[i][j].uState };
                        KB.Add(b);
                    }
                }
            }
        }

       /// <summary>
       /// 得到待确定的Normal格子的信息，先清空
       /// </summary>
        private void getRequested() 
        {
            Requested.Clear();
            foreach (var i in Enumerable.Range(0, m_uXNum))
            {
                foreach (var j in Enumerable.Range(0, m_uYNum))
                {
                    if (isNumAround(i,j) && m_pMines[i][j].uState == State.Normal)
                    {
                        int[] b = { i, j };
                        Requested.Add(b);
                    }
                }
            }
        }


       /// <summary>
       /// 判断周围是否有有数字的Space
       /// </summary>
        private bool isNumAround(int x, int y)
        {
            int minRow = (x == 0) ? 0 : x - 1;
            int maxRow = x + 2;
            int minCol = (y == 0) ? 0 : y - 1;
            int maxCol = y + 2;
            for (var i = minRow; i < maxRow; i++)
            {
                for (var j = minCol; j < maxCol; j++)
                {
                    if (!IsInMineArea(i, j)) continue;
                    if (((int)m_pMines[i][j].uState) >= 1 && ((int)m_pMines[i][j].uState) <= 8) return true;
                }
            }
            return false;
        }

       /// <summary>
       /// 判断是否在地图内的合法点上，防止数组越界
       /// </summary>
        private bool IsInMineArea(int x, int y)
        {
            return (x >= 0 && x < m_uXNum && y >= 0 && y < m_uYNum);
        }
       
       private int getNumOfNormalAndOthers(int x, int y)
       {
           int Num=0;
           int minRow = (x == 0) ? 0 : x - 1;
           int maxRow = x + 2;
           int minCol = (y == 0) ? 0 : y - 1;
           int maxCol = y + 2;
           for (var i = minRow; i < maxRow; i++)
           {
               for (var j = minCol; j < maxCol; j++)
               {
                   if (!IsInMineArea(i, j)) continue;
                   if (((int)m_pMines[i][j].uState) >= 8 && ((int)m_pMines[i][j].uState) <= 16) Num++;
               }
           }
           return Num;
       }

       /// <summary>
       /// j将周围标记为雷
       /// </summary>
       private void estimateAroundAsMines(int x, int y)
       {
           int minRow = (x == 0) ? 0 : x - 1;
           int maxRow = x + 2;
           int minCol = (y == 0) ? 0 : y - 1;
           int maxCol = y + 2;
           for (var i = minRow; i < maxRow; i++)
           {
               for (var j = minCol; j < maxCol; j++)
               {
                   if (!IsInMineArea(i, j)) continue;
                   if (m_pMines[i][j].uState == State.Normal)
                   {
                       int[] b = { i, j };
                       bool c = Requested.Contains(b);
                       Requested.Remove(b);
                       Output.Add(b);
                       m_pMines[i][j].uEstimation = Estimation.Mine;
                   }
               }
           }
       }

       private void estimateAroundAsEmpty(int x, int y)
       {
           int minRow = (x == 0) ? 0 : x - 1;
           int maxRow = x + 2;
           int minCol = (y == 0) ? 0 : y - 1;
           int maxCol = y + 2;
           for (var i = minRow; i < maxRow; i++)
           {
               for (var j = minCol; j < maxCol; j++)
               {
                   if (!IsInMineArea(i, j)) continue;
                   if (m_pMines[i][j].uState == State.Normal && m_pMines[i][j].uEstimation != Estimation.Mine)
                   {
                       int[] b = { i, j };
                       Requested.Remove(b);
                       Output.Add(b);
                       m_pMines[i][j].uEstimation = Estimation.Empty;
                   }
               }
           }
       }

       private bool isSatisfied(int x, int y) 
       {
           int Num = 0;
           int minRow = (x == 0) ? 0 : x - 1;
           int maxRow = x + 2;
           int minCol = (y == 0) ? 0 : y - 1;
           int maxCol = y + 2;
           for (var i = minRow; i < maxRow; i++)
           {
               for (var j = minCol; j < maxCol; j++)
               {
                   if (!IsInMineArea(i, j)) continue;
                   if (m_pMines[i][j].uEstimation == Estimation.Mine)
                   {
                       Num++;
                   }
               }
           }
           if (Num == (int)m_pMines[x][y].uState) return true;
           return false;
       }
        #endregion

        #region Deduction

       /// <summary>
       /// The Most Common and obvious way of deducting
       /// </summary>
        public void SimpleDeduction()
        {
            getKB();
            SimpleList known = KB; 
            getRequested();
            int c = Requested.Count;
            Output.Clear();
            int x,y,k;
            //找雷
            foreach (var i in Enumerable.Range(0,KB.Count))
            {
                int[] b = (int[])KB[i];
                x = b[0]; y = b[1]; k = b[2];
                if (getNumOfNormalAndOthers(x,y) == k) 
                {
                    estimateAroundAsMines(x, y);
                }
            }
            //找空
            foreach (var i in Enumerable.Range(0, KB.Count))
            {
                int[] b = (int[])KB[i];
                x = b[0]; y = b[1]; k = b[2];
                if (isSatisfied(x,y))
                {
                    estimateAroundAsEmpty(x, y);
                }
            }
        }


       /// <summary>
       /// 利用深度优先，寻找所有SimpleDeduction未能发现的雷区。
       /// </summary>
        public void DepthFirstDeductoin()
        {
            SimpleDeduction();
            int Depth = Requested.Count;
            SimpleList TemporaryMines = new SimpleList(); //用于存储各种可能的结果
            //每加一个雷，只会对周围的Num产生影响，因此只需判断此个函数
            DFSMine(TemporaryMines,0,Depth);

        }

        private bool DFSMine(SimpleList TemporaryMines, int CurrentDepth, int MaxDepth)
        {
            int[] a = (int[]) Requested[CurrentDepth];
            int x = a[0];  int y = a[1];
            m_pMines[x][y].uEstimation = Estimation.TempEmpty;


            return true;
        }

        #endregion


    }
       

    
}
