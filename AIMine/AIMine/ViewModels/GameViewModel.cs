using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIMineComponent;

namespace AIMine.ViewModels
{
    public class GameViewModel
    {
        public GameViewModel(SettingsViewModel settings)
        {
            m_uSpendTime = 0;
            m_uTimer = 0;
            m_uPrimary = 0;
            m_uSecond = 0;
            m_uAdvance = 0;
            game = new Game(settings.m_uXnum, settings.m_uYnum, settings.m_uMineNum);
        }
        public int m_uSpendTime { get; set; }			// 游戏开始击到目前所花费的时间
        public int m_uTimer { get; set; }				// 定时器标识
        public int m_uLevel { get; set; }				// 当前游戏等级
        public int m_uPrimary { get; set; }				// 初级记录
        public int m_uSecond { get; set; }				// 中级记录
        public int m_uAdvance { get; set; }				// 高级记录
        public Game game { get; set; }
    }
}
