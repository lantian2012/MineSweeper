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
        public Game(int m_uXnum, int m_uYNum, int m_uMineNum)
        {
            m_pMines = new MINEWND[100][];

        }

        private void InitializeBoard()
        {
            

        }
        #endregion

    }
}
