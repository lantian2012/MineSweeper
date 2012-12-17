using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using AIMineComponent;

namespace AIMineUnitTestLZeng
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestInitialize()
        {
            Game game = new Game();
            int k = 0;

            //check if the number of the mines are equal
            foreach (var i in Enumerable.Range(0, game.m_uXNum))
            {
                foreach (var j in Enumerable.Range(0, game.m_uYNum))
                {
                    if (game.m_pMines[i][j].uAttrib == Attrib.Mine) k++;
                }
            }
            Assert.AreEqual(k, 40); 
            
            
        }
        [TestMethod]
        public void TestLButton()
        {
            Game game = new Game(16, 16, 0);
            int k = 0;
            //Test Click Events
            game.OnLButtonDown(2, 2);
            game.OnLButtonUp(2, 2);
            Assert.AreEqual(game.m_pMines[2][2].uState, State.Empty);
        }

        [TestMethod]
        public void TestLButtonLose()
        {
            Game game = new Game(4, 4, 16);
            int k = 0;
            //Test Click Events
            game.OnLButtonDown(2, 2);
            game.OnLButtonUp(2, 2);
            Assert.AreEqual(game.m_uGameState, GameState.Lose);
        }

        [TestMethod]
        public void TestRButtonWin()
        {
            Game game = new Game(4, 4, 16);
            int k = 0;
            //Test Click Events
            foreach (var i in Enumerable.Range(0, game.m_uXNum))
            {
                foreach (var j in Enumerable.Range(0, game.m_uYNum))
                {
                    game.OnRButtonDown(i, j);
                    game.OnRButtonUp(i, j);
                }
            }
            
            Assert.AreEqual(game.m_uGameState, GameState.Victory);
        }

        [TestMethod]
        public void TestKB()
        {
            Game game = new Game();
            //Test Click Events
            //需要显式转换
            int[] b= {1,2,3};
            int[] a = (int[])game.sweeperAgent.KB[0];
            Assert.AreEqual(a[0], 1);
            Assert.AreEqual(a[1], 2);
            Assert.AreEqual(a[2], 3);
            
        }
        [TestMethod]
        public void TestDepthDeduction()
        {
            Game game = new Game(16,16,30);
            int x,y;
            x = 0; y = 0;
            bool found = false;
            foreach (var i in Enumerable.Range(0, game.m_uXNum))
            {
                foreach (var j in Enumerable.Range(0, game.m_uYNum))
                {
                    if (game.m_pMines[i][j].uAttrib == Attrib.Empty) { x = i; y = j; break; }
                }
                if (found) break;
            }
            game.OnLButtonDown(x, y);
            game.OnLButtonUp(x, y);
            game.sweeperAgent.DepthFirstDeductoin();
            Assert.AreEqual(game.sweeperAgent.testingBool, true);

        }
    }
}
