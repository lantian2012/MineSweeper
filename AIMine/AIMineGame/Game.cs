using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.ComponentModel;
/*
 * 
     foreach (var i in Enumerable.Range(0, m_uXNum))
            {
                foreach (var j in Enumerable.Range(0, m_uYNum))
                {
 * */
namespace AIMineComponent
{
    public sealed class Game: INotifyPropertyChanged
    {
        #region Properties of Game Class
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

            }
        }
        private double _output;
        public double output
        {
            get { return _output; }
            set
            {
                _output = value;
                NotifyPropertyChanged("output");
            }
        }
        private int _m_nLeaveNum;
        public int	m_uXNum { get; set; }				// X方向小方块个数
        public int m_uYNum { get; set; }				// Y方向小方块个数
        public int m_uMineNum { get; set; }				// 总的雷个数
        public int m_nLeaveNum
        {
            get
            {
                return _m_nLeaveNum;
            }
            set
            {
                _m_nLeaveNum = value;
                NotifyPropertyChanged("m_nLeaveNum");
            }
        }
        public GameState m_uGameState { get; set; }			// 游戏状态
        public int m_uNewState { get; set; }			// 当前选中的小方块的状态
        public IList<IList<MINEWND>>  m_pMines { get; set; }		// 表示雷区内的所有小方块的二维数组
        public MINEWND m_pNewMine { get; set; }				// 当前选中的小方块
        public MINEWND m_pOldMine { get; set; }				// 上次选中的小方块
        public SweeperAgent sweeperAgent; //扫雷智能体
        
        #endregion

        #region Initialize and Setup

        /// <summary>
        /// initialize with default settings: medium level, 16 x 16. 40 mines
        /// </summary>
        public Game() : this(16, 16, 40) { }

       /// <summary>
       /// Initializes a new instance of the Game class using the specified board size
       /// </summary>
       public Game(int XNum, int YNum, int MineNum)
        {
            m_uXNum = XNum;
            m_uYNum = YNum;
            m_pMines = new MINEWND[m_uXNum][];
            m_uGameState = GameState.Normal;
            m_uMineNum = MineNum;
            m_nLeaveNum = m_uMineNum;
            InitializeBoard();

        }

        /// <summary>
        /// Populates the m_pMines array and set up the inintial board state, set mines
        /// </summary>
        private void InitializeBoard()
        {
            
            foreach  (var i in Enumerable.Range(0,m_uXNum) )
                m_pMines[i] = new MINEWND[m_uYNum];
            foreach (var i in Enumerable.Range(0, m_uXNum))
            {
                foreach (var j in Enumerable.Range(0, m_uYNum))
                {
                    m_pMines[i][j] = new MINEWND();
                    m_pMines[i][j].uRow = i;
                    m_pMines[i][j].uCol = j;
                    m_pMines[i][j].uState = State.Normal;
                    m_pMines[i][j].uAttrib = Attrib.Empty;
                    m_pMines[i][j].uOldState = State.Normal;
                    m_pMines[i][j].uEstimation = Estimation.None;
                }
            }
            //随机布置地雷
            
            Random rnd = new Random();

            int k;
            for (k=0; k<m_uMineNum; k++)
            {
                int i = (int)Math.Floor(m_uXNum * rnd.NextDouble());
                int j = (int)Math.Floor(m_uYNum * rnd.NextDouble());
                if (m_pMines[i][j].uAttrib == Attrib.Mine) k--;
                else      m_pMines[i][j].uAttrib = Attrib.Mine;              
            }
            //Agent
            sweeperAgent =new  SweeperAgent(m_pMines, m_uXNum,m_uYNum);





        }

        #endregion

        #region GameLogic , win and lose

        /// <summary>
        /// If all of the Mines are labeled RIGHT and others open with number
        /// </summary>
        public bool Victory()
        {
            foreach (var i in Enumerable.Range(0, m_uXNum))
            {
                foreach (var j in Enumerable.Range(0, m_uYNum))
                {
                    if (m_pMines[i][j].uState == State.Normal) return false;
                    if (m_pMines[i][j].uState == State.Dicey) return false;
                }
            }
            m_nLeaveNum = 0;
            m_uGameState = GameState.Victory;
            return true;
        }

        /// <summary>
        /// Since the data is bound to the UI, and the GameState has already changed, no further 
        /// actions is required to do in this function
        /// </summary>
        public void Lose(int x, int y)
        {
            if (m_pMines[x][y].uAttrib == Attrib.Mine)
            { //失败——点中雷                
                // 点中的雷进行爆炸
                m_pMines[x][y].uState = State.Blast;
                m_pMines[x][y].uOldState = State.Blast;
                foreach (var i in Enumerable.Range(0, m_uXNum))
                {
                    foreach (var j in Enumerable.Range(0, m_uYNum))
                    {
                        
                        if (m_pMines[i][j].uAttrib == Attrib.Mine
                            && m_pMines[i][j].uState != State.Flag)
                        { //将未标出的雷进行显示
                            m_pMines[i][j].uState = State.Mine;
                            m_pMines[i][j].uOldState = State.Mine;
                        }
                        if (m_pMines[i][j].uAttrib != Attrib.Mine
                            && m_pMines[i][j].uState == State.Flag)
                        { //将未标出的雷进行显示
                            m_pMines[i][j].uState = State.Error;
                            m_pMines[i][j].uOldState = State.Error;
                        }
                    }
                }
            } //if
            else  //失败——错误雷
            {
                m_pMines[x][y].uState = State.Error;
				m_pMines[x][y].uOldState = State.Error;
                foreach (var i in Enumerable.Range(0, m_uXNum))
                {   
                    foreach (var j in Enumerable.Range(0, m_uYNum))
                    {                        
                        if (m_pMines[i][j].uAttrib == Attrib.Mine     && m_pMines[i][j].uState != State.Flag)
				        {
					        m_pMines[i][j].uState = State.Mine;
					        m_pMines[i][j].uOldState = State.Mine;
                        }
                    }
                }
            } //else
            m_uGameState = GameState.Lose;
        }

        /// <summary>
        /// 判断是否在合法的雷区域中。Not Necessary
        /// </summary>
        private bool IsInMineArea(int x, int y)
        {
            return (x >= 0 && x < m_uXNum && y >= 0 && y < m_uYNum);
        }

        /// <summary>
        ///   获取某个小方块区域相邻8个区域的已标志状态数，flags
        /// </summary>
        private int GetAroundFlags(int x ,int y )
        {
            int flags = 0;
            int minRow = (x == 0) ? 0 : x - 1;
            int maxRow = x + 2;
            int minCol = (y == 0) ? 0 : y - 1;
            int maxCol = y + 2;
            for (var i = minRow; i < maxRow; i++)
            {
                for (var  j = minCol; j < maxCol; j++)
                {
                    if (!IsInMineArea(i, j)) continue;
                    if (m_pMines[i][j].uState == State.Mine) flags++;
                }
            }
            return flags;
        }

        /// <summary>
        /// 获取某个小方块相邻8个区域中雷的个数，Attribute
        /// </summary>
        private int GetAroundNum(int x, int y)
        {
            int around = 0;
            int minRow = (x == 0) ? 0 : x - 1;
            int maxRow = x + 2;
            int minCol = (y == 0) ? 0 : y - 1;
            int maxCol = y + 2;
            for (var i = minRow; i < maxRow; i++)
            {
                for (var j = minCol; j < maxCol; j++)
                {
                    if (!IsInMineArea(i, j)) continue;
                    if (m_pMines[i][j].uAttrib == Attrib.Mine) around++;
                }
            }
            return around;
        }

        /// <summary>
        /// 雷方块拓展(对于周围无雷的空白区域)
        /// </summary>
        private void ExpandMines(int x, int y)
        {   
	        int i, j;
	        int minRow = (x == 0) ? 0 : x - 1;
	        int maxRow = x + 2;
	        int minCol = (y == 0) ? 0 : y - 1;
	        int maxCol = y + 2;
	        int around = GetAroundNum(x, y);
	        m_pMines[x][y].uState =  (State)  around;
	        m_pMines[x][y].uOldState = (State) around;
            // 对周围一个雷都没有的空白区域
            if (around == 0)
            {
                for (i = minRow; i < maxRow; i++)
                {
                    for (j = minCol; j < maxCol; j++)
                    {//对于周围可以拓展的区域进行的规拓展			
                        if (!IsInMineArea(i, j)) continue; //先进行合法判断
                        if (!(i == x && j == y) &&
                            m_pMines[i][j].uState == State.Normal
                            && m_pMines[i][j].uAttrib != Attrib.Mine)
                        {                            
                            ExpandMines(i, j); //iteration, good
                        }
                    }
                }
            }

        }

        /// <summary>
        /// 展开周围8个方向，for LRButtonUp
        /// </summary>
        private void OpenAround(int x, int y)
        {
            int i, j;
            int around = 0;
            if (GetAroundFlags(x, y) != GetAroundNum(x, y)) return;
            int minRow = (x == 0) ? 0 : x - 1;
            int maxRow = x + 2;
            int minCol = (y == 0) ? 0 : y - 1;
            int maxCol = y + 2;
            for (i = minRow; i < maxRow; i++)
            {
                for (j = minCol; j < maxCol; j++)
                {
                    if (!IsInMineArea(i, j)) continue;
                    if (m_pMines[i][j].uState == State.Normal)
                    {//如果该区域为正常区域

                        //拓展该雷区
                        ExpandMines(i, j);
                        around = GetAroundNum(i, j);
                        m_pMines[i][j].uState = (State) around;
                        m_pMines[i][j].uOldState = (State) around;
                    }
                }
            }

            // 判断是否胜利，是则将地图中所有雷标识出来
            if (Victory())
            {
                for (i = 0; i < m_uXNum; i++)
                {
                    for (j = 0; j < m_uYNum; j++)
                    {
                        if (m_pMines[i][j].uAttrib == Attrib.Mine)
                        {
                            m_pMines[i][j].uState = State.Flag;
                            m_pMines[i][j].uOldState = State.Flag;
                        }
                    }
                }
                m_nLeaveNum = 0;
            }
        }

        /// <summary>
        /// 当周围的flag数未达到实际雷数时，不报错（返回false）
        /// 当周围的flag数达到实际雷数，而且有标错的情况时，报错（返回true）
        /// </summary>
        private bool ErrorAroundFlag(int x, int y)
        {
            //如果周围相邻的标志雷数 != 周围相邻的雷数 则返回
	        if (GetAroundFlags(x, y) != GetAroundNum(x, y)) return false;
	        int i, j;
        	int minRow = (x == 0) ? 0 : x - 1;
        	int maxRow = x + 2;
        	int minCol = (y == 0) ? 0 : y - 1;
        	int maxCol = y + 2;

            for (i = minRow; i < maxRow; i++)
            {
                for (j = minCol; j < maxCol; j++)
                {
                    if (!IsInMineArea(i, j)) continue;
                    if (m_pMines[i][j].uState == State.Flag)
                    {
                        if (m_pMines[i][j].uAttrib != Attrib.Mine)
                        {
                            Lose(i, j);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        #endregion

        #region ClickEvents
        /// <summary>
        /// Do Nothing But change the state to pressed
        /// but oldState remains unchanged for recovery
        /// </summary>
        public void OnLButtonDown(int x, int y)
        {
            switch (m_pMines[x][y].uState)
            {
                case State.Normal:
                    m_pMines[x][y].uState = State.Pressed;
                    break;
                case State.Dicey:
                    m_pMines[x][y].uState = State.Dicey_down;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 可以考虑根据不同的游戏状态进行处理，暂时不考虑
        /// </summary> 
        public void OnLButtonUp(int x, int y)
        {
            int around;
            if (m_pMines[x][y].uOldState == State.Normal)
            {   //当该雷区域为正常未作标记才打开
                // first judge if the special MINEWND is a mine
                //如果该区域为雷，则死亡
                if (m_pMines[x][y].uAttrib == Attrib.Mine) 
                {
                    Lose(x, y);
                    return; 
                }
                //不是雷的时候，获取其周围的雷数目
                around = GetAroundNum(x, y);
                // 如果为空白区域，拓展，否则打开该区域（显示周围有多少雷数）
                if (around == 0)    ExpandMines(x, y);
                else            m_pMines[x][y].uState = (State) around;
                //若胜利，返回
                if (Victory()) return;                
            }
            else if (m_pMines[x][y].uOldState == State.Dicey)
            {//标志为“？”问号的时候
                m_pMines[x][y].uOldState = State.Dicey;
            }
        }

        /// <summary>
        ///  改变旗帜，未知以及正常三种状态
        /// </summary>
        public void OnRButtonDown(int x, int y)
        {
            switch (m_pMines[x][y].uState)
            {
                case State.Normal:
                    m_pMines[x][y].uState = State.Flag;
                    m_pMines[x][y].uOldState = State.Flag;
                    m_nLeaveNum--;
                    break;
                case State.Flag:
                    m_pMines[x][y].uState = State.Dicey;
                    m_pMines[x][y].uOldState = State.Dicey;
                    m_nLeaveNum++;
                    break;
                case State.Dicey:
                    m_pMines[x][y].uState = State.Normal;
                    m_pMines[x][y].uOldState = State.Normal;
                    break;
                default: break;            
            }
        }

        /// <summary>
        /// 暂时先只进行是否胜利判断
        /// </summary>
        public void OnRButtonUp(int x, int y)
        {
            Victory();
        }

        /// <summary>
        /// 首先令Normal的Space变成Pressed，暂时先令Flag和Dicey不变
        /// 对于Num8~Empty的，进行扩展操作
        /// </summary>
        public void OnLRButtonDown(int x, int y)
        {
            int i, j;
            if (GetAroundFlags(x, y) != GetAroundNum(x, y)) return;
            int minRow = (x == 0) ? 0 : x - 1;
            int maxRow = x + 2;
            int minCol = (y == 0) ? 0 : y - 1;
            int maxCol = y + 2;
            for (i = minRow; i < maxRow; i++)
            {
                for (j = minCol; j < maxCol; j++)
                {
                    if (!IsInMineArea(i, j)) continue;
                    //			if (i == row && j == col) continue;
                    if (m_pMines[i][j].uState == State.Normal)
                    {
                        m_pMines[i][j].uState = State.Empty;
                    }
                    else if (m_pMines[i][j].uState == State.Dicey)
                    {
                        m_pMines[i][j].uState = State.Dicey_down;
                    }
                }
            }
        }

        public void OnLRButtonUp(int x, int y)
        {
            int i, j;
            if (GetAroundFlags(x, y) != GetAroundNum(x, y)) return;
            int minRow = (x == 0) ? 0 : x - 1;
            int maxRow = x + 2;
            int minCol = (y == 0) ? 0 : y - 1;
            int maxCol = y + 2;
            for (i = minRow; i < maxRow; i++)
            {
                for (j = minCol; j < maxCol; j++)
                {
                    if (!IsInMineArea(i, j)) continue;
                    //			if (i == row && j == col) continue;
                    if (m_pMines[i][j].uOldState == State.Normal)
                    {
                        m_pMines[i][j].uState = State.Normal;
                    }
                    else if (m_pMines[i][j].uState == State.Dicey)
                    {
                        m_pMines[i][j].uState = State.Dicey;
                    }
                }
            }
            // check whether the MINEWND around the special MINEWND is a mine, if it is then dead.
            //此判错方式对Normal按钮同样有效
            if ( ErrorAroundFlag(x , y) ) // 在ErrorAroundFlag中也有引用Lose
            {
                Lose( x, y);
                return;
            }
 
        }

        public void OnMouseMove(int x, int y)
        {
 
        }

        #endregion

        #region AI

        #endregion 

        #region move validation
        #endregion

    }
}
