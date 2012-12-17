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

       public bool testingBool = false;
        public SimpleList KB;
        private SimpleList Requested;
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

        #region usefulFunctions for Deduction

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
       /// 将周围标记为雷
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

       /// <summary>
       /// 判断在（x,y）处添加的假设状态是否合理
       /// </summary>
       private bool isLegitimateWithAssumption(int x, int y)
       {
           bool IsLegitimate = true;//假设满足，一旦有不满足的，返回，return false
           int minRow = (x == 0) ? 0 : x - 1;
           int maxRow = x + 2;
           int minCol = (y == 0) ? 0 : y - 1;
           int maxCol = y + 2;
           for (var i = minRow; i < maxRow; i++)
           {
               for (var j = minCol; j < maxCol; j++)
               {
                   if (!IsInMineArea(i, j)) continue;
                   if ((int)m_pMines[i][j].uState >= 0 && (int)m_pMines[i][j].uState <= 8)
                   {
                       int a=getAroundNumOfEstimateMinesAndTempMines(i, j);
                       int b= getAroundNumOfNormalNotMineNotEmpty(i,j);
                       int c =  (int)m_pMines[i][j].uState;
                       if   ((a <= (int)m_pMines[i][j].uState)==false  ||  ((a + b) >= (int)m_pMines[i][j].uState) == false )
                           return IsLegitimate = false;
                   }
                       
               }
           }
           return IsLegitimate;

       }

       /// <summary>
       /// //既没有被判断为Mine，也没有被判断为Empty。
       /// 既没有被假设为Mine，也没有被假设为Empty。
       /// </summary>
       private int getAroundNumOfNormalNotMineNotEmpty(int x, int y)
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
                   if (m_pMines[i][j].uState == State.Normal && m_pMines[i][j].uEstimation != Estimation.TempMine
                       && m_pMines[i][j].uEstimation != Estimation.Mine && m_pMines[i][j].uEstimation != Estimation.TempEmpty
                       && m_pMines[i][j].uEstimation != Estimation.Empty) Num++;
               }
           }
           return Num;
       }
       /// <summary>
       /// 进行深度优先搜索时，对每个Num周围的雷数进行计算（包括TempMine以及EstimationMine）
       /// </summary>
       private int getAroundNumOfEstimateMinesAndTempMines(int x, int y)
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
                   if (m_pMines[i][j].uEstimation == Estimation.Mine || m_pMines[i][j].uEstimation == Estimation.TempMine) Num++;
               }
           }
           return Num;
       }

       /// <summary>
       /// 开始深度优先搜索时先清空所有的Temp
       /// </summary>
       private void clearAllTemp()
       {
           foreach (var i in Enumerable.Range(0, m_uXNum))
           {
               foreach (var j in Enumerable.Range(0, m_uYNum))
               {
                   if (m_pMines[i][j].uEstimation == Estimation.TempEmpty | m_pMines[i][j].uEstimation == Estimation.TempMine | m_pMines[i][j].uEstimation == Estimation.HighestEmpty)
                       m_pMines[i][j].uEstimation = Estimation.None;
               }
           }

       }

       private void AddToTemporaryMines(List<SimpleList> TemporaryMines)
       {
           SimpleList TemporaryMine = new SimpleList(Requested.Count,3);
           foreach (var i in Enumerable.Range(0, Requested.Count))
           {
               int[] b = (int[])Requested[i];
               int x = b[0]; int  y = b[1];
               if (m_pMines[x][y].uEstimation == Estimation.TempEmpty)
               {
                   int[] c = { x, y, 0 };
                   TemporaryMine.Add(c);
               }
               if (m_pMines[x][y].uEstimation == Estimation.TempMine)
               {
                   int[] c = { x, y, 1 };
                   TemporaryMine.Add(c);
               }               
           }
           TemporaryMines.Add(TemporaryMine);

       }

       private void InitializeCountingMines(SimpleList CountingMines)
       {
           foreach (var i in Enumerable.Range(0, Requested.Count))
           {
               int[] b = (int[])Requested[i];
               int x = b[0]; int y = b[1];
               int[] c = {x,y,0,0}; 
               CountingMines.Add(c);
           }
       }

       private void CountingEveryRequired(List<SimpleList> TemporaryMines, SimpleList CountingMines)
       {
           foreach (var i in Enumerable.Range(0, TemporaryMines.Count)) //对每种可能的结果进行遍历，假设他们可能性相等
           {
               SimpleList TemporaryMine = TemporaryMines[i];
               foreach (var j in Enumerable.Range(0, TemporaryMine.Count)) 
                   //首先，TemporaryMine.Count = Requested.Count； 其次，CountingMines中的顺序与下表对应应该和TemporaryMines中完全一致
               {
                   int[] a = (int[])TemporaryMine[j]; //得到了一个三个数的数组
                   int[] b = (int[])CountingMines[j];
                   if (a[2] == 0) b[2]++;
                   if (a[2] == 1) b[3]++;
               }
           }
       }

       private int FindHighestAppearance(SimpleList CountingMines)
       {
            //首先遍历，找到最大的值。分母的基数即为TemporaryMines.Count因此技术即可
           int HighestApp = 0;
           foreach (var i in Enumerable.Range(0, CountingMines.Count))
           {
               int a =((int[])CountingMines[i])[2]; //该位置出现Empty的次数
                HighestApp = ( a > HighestApp) ? a : HighestApp;
           }
           return HighestApp;


       }

       private void FindAllHighestEmpty(int HighestApp, SimpleList CountingMines, bool isSure)
       {
           foreach (var i in Enumerable.Range(0, CountingMines.Count))
           {
               int[]  a = (int[])CountingMines[i]; 
               int b = a[2];//该位置出现Empty的次数
               if (HighestApp == b)
               {
                   int x = a[0]; int y = a[1];
                   if (isSure)
                       m_pMines[x][y].uEstimation = Estimation.Empty;
                   else
                       m_pMines[x][y].uEstimation = Estimation.HighestEmpty;
               }
           }
       }

        #endregion

        #region Deduction

       /// <summary>
       /// The Most Common and obvious way of deducting, as most people do
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
            //初始时需要清空所有m_pMines的Temp状态
            clearAllTemp();
            int Depth = Requested.Count;//实际数组坐标到达Depth-1
            if (Depth == 0) return;

            List<SimpleList> TemporaryMines = new List<SimpleList>(); //用于存储各种可能的结果，其实不用，只要最后判断整体结果合格时，将Requested中的所有点的当前值放入即可
            SimpleList CountingMines = new SimpleList(Requested.Count, 4);//用于存放此归结数组
            InitializeCountingMines(CountingMines);
            
            //每加一个雷，只会对周围的Num产生影响，因此只需判断此个函数
            DFSMine(0,Depth, TemporaryMines);
            ///在此之后，无非以下情况：
            ///（1） 没有Solution，则智能体随机点击
            /// （2） 有Solution，智能体搜索是否有存在于所有Solution中的结果，若存在，则可以判断。
            /// 当然，均可以归结为对每一个需要推断的地方的出现Mine以及非Empty的和作归结。利用CountingMines
            /// 经过无数次Debug，总算可以确定，TemporaryMines中确实有所需要的所有正确的解。完全而且不遗漏。
            /// 例如：当棋盘上只有1时，显示的解为8；只有2时，为28；当由SimpleDeduction有定解时，解为0。
            /// 
            //下面开始，对TemporaryMines中的每个值进行累加。
            //找到具有最大可能性为空的，并且若该可能性为1，则标志出来为Estimation.Empty。
            CountingEveryRequired(TemporaryMines, CountingMines);
            int HighestApp = FindHighestAppearance(CountingMines);
            //新建一个SimpleList，其中存放所有具有最高可能性Empty的格子的坐标
            FindAllHighestEmpty(HighestApp,CountingMines,(HighestApp==TemporaryMines.Count));
            

        }

        private void DFSMine( int CurrentDepth, int MaxDepth, List<SimpleList> TemporaryMines)
        {
            bool hasSolution = false; //假设找不到任何解，若找到，赋值为true
            int[] a = (int[]) Requested[CurrentDepth];
            int cd = CurrentDepth + 1;
            int x = a[0];  int y = a[1];
            if (MaxDepth != 0)
            {
                if (CurrentDepth < MaxDepth - 1)
                {
                    //假设这一个节点为空TempEmpty
                    m_pMines[x][y].uEstimation = Estimation.TempEmpty;
                    //若这个假设满足，则继续深入，否则返回
                    if (isLegitimateWithAssumption(x, y)) DFSMine(cd, MaxDepth, TemporaryMines);
                    //假设次节点为雷，TempMine
                    m_pMines[x][y].uEstimation = Estimation.TempMine;
                    if (isLegitimateWithAssumption(x, y)) DFSMine(cd, MaxDepth, TemporaryMines);
                }
                else if (CurrentDepth == MaxDepth - 1)//达到最后一层递归，注意：此处，在上一层的时候数组坐标为MaxDepth - 2，本层为最后一层。本层currentDepth为MaxDepth - 1
                //本层只判断
                {
                    m_pMines[x][y].uEstimation = Estimation.TempEmpty;
                    if (isLegitimateWithAssumption(x, y))
                    {
                        hasSolution = true;
                        AddToTemporaryMines(TemporaryMines);
                    }
                    m_pMines[x][y].uEstimation = Estimation.TempMine;
                    if (isLegitimateWithAssumption(x, y))
                    {
                        hasSolution = true;
                        AddToTemporaryMines(TemporaryMines);
                    }
                }
                //每次不管是成功还是失败退出函数，均要还原。
                //但是如果只有一次就出来，即MaxDepth=0的状况，有可能会过执行。将之前的结果Estimation.Empty清空成Estimation.None
                m_pMines[x][y].uEstimation = Estimation.None;
            }
            
        }


       

        #endregion


    }
       

    
}
