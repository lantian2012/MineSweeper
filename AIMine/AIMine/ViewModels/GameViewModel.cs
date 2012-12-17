using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIMineComponent;
using AIMine.Common;
using Windows.UI.Popups;

namespace AIMine.ViewModels
{
    public class GameViewModel: BindableBase
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
        public State teststate;
        
        public void leftDown(int row, int column)
        {
            game.OnLButtonDown(row, column);
            //game.OnLButtonUp(row, column);
        }
        public void leftUp(int row, int column)
        {
            //game.OnLButtonDown(row, column);
            game.OnLButtonUp(row, column);
        }
        public void rightDown(int row, int column)
        {
            game.OnRButtonDown(row, column);
            //game.OnLRButtonDown(row, column);
        }
        public void rightUp(int row, int column)
        {
            game.OnRButtonUp(row, column);
            //game.OnLButtonUp(row, column);
        }
        public void twoDown(int row, int column)
        {
            game.OnLRButtonDown(row, column);
            //game.OnLButtonUp(row, column);
        }
        public void towUp(int row, int column)
        {
            game.OnLRButtonUp(row, column);
        }


        

        public void AIsearch()
        {
            game.sweeperAgent.SimpleDeduction();
        }
        public async void winDisplay()
        {
            var messageDialog = new MessageDialog("You Win!", "Congratulations!");
           
            messageDialog.Commands.Add(new UICommand("OK"));
            await messageDialog.ShowAsync();
        }
        public async void loseDisplay()
        {
            var messageDialog = new MessageDialog("You Lose!", "Just Have Another Try");

            messageDialog.Commands.Add(new UICommand("OK"));
            await messageDialog.ShowAsync();
        }
        public void button1()
        {
            game.sweeperAgent.DepthFirstDeductoin();
        }
        public void button2()
        {
        }
        public void button3()
        {
        }
        public void button4()
        {
            game.output++;
        }
    }
}
