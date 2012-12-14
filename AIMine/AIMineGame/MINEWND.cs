using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace AIMineComponent
{
  public  class MINEWND: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public int uRow { get; set; }         //所在雷区二维数组的行
        public int uCol { get; set; }     //所在雷区二位数组的列
        private Estimation _uEstimation;
        public Estimation uEstimation
        {
            get { return _uEstimation; }
            set
            {
                _uEstimation = value;
                NotifyPropertyChanged("uEstimation");
            }
        } 
        private State _uState;
      public State uState 
        {   get{return _uState;}
            set
            {
                _uState = value;
                NotifyPropertyChanged("uState");
            }
        }     
        //当前状态
        public Attrib uAttrib { get; set; }  //方块属性
        public State uOldState { get; set; }//历史状态
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

            }
        }
    }

    
}
