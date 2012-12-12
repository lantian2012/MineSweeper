using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIMineComponent
{
    public class Game
    {
        #region Properties of Game Class
        
        public int	m_uXNum { get; set; }				// X方向小方块个数
        public int m_uYNum { get; set; }				// Y方向小方块个数
        public int m_uMineNum { get; set; }				// 总的雷个数
        public int m_nLeaveNum { get; set; }			// 剩余的雷个数
        public int m_uGameState { get; set; }			// 游戏状态
        public int m_uNewState { get; set; }			// 当前选中的小方块的状态
        public MINEWND[][] m_pMines { get; set; }		// 表示雷区内的所有小方块的二维数组
        public MINEWND m_pNewMine { get; set; }				// 当前选中的小方块
        public MINEWND m_pOldMine { get; set; }				// 上次选中的小方块
        
        #endregion

        #region Initialize and Setup
        public Game(int x, int y, int n)
        {
            
            m_uXNum = x;
            m_uYNum = y;
            m_uMineNum = n;
            m_pMines = new MINEWND[m_uXNum][];
            this.InitializeBoard();
        }

        private void InitializeBoard()
        {
            foreach (var row in Enumerable.Range(0, m_uYNum))
            {
                m_pMines[row] = new MINEWND[m_uXNum];
            }

            foreach (var row in Enumerable.Range(0, m_uXNum))
            {
                foreach (var column in Enumerable.Range(0, m_uYNum))
                {
                    m_pMines[row][column] = new MINEWND();
                    if (row == 0) m_pMines[row][column].uState = State.Num4;
                    if (row == 1) m_pMines[row][column].uState = State.Num1;
                    if (row == 2) m_pMines[row][column].uState = State.Num2;
                    if (row == 3) m_pMines[row][column].uState = State.Num3;
                }
            }
        }
        #endregion


    }
}
