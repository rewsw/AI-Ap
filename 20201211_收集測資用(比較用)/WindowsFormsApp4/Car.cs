using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace WindowsFormsApp4
{
   public class Car
    {
       public int X = 0, Y = 0, r = 0, c = 0;
        public bool can_go = false;
        public List<Point> draw_sub_point = new List<Point>();
        public int index = 0;
        public Car(int X,int Y ,int r,int c,bool can_go, List<Point> draw_sub_point)
        {
            this.X = X;
            this.Y = Y;
            this.r = r;
            this.c = c;
            this.draw_sub_point = draw_sub_point;
            this.can_go = can_go;
            index = 0;
        }
    }
}
