using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace WindowsFormsApp4
{
    public class Sensor_data
    {
        public enum draw_rectangle :int{
            None,inside,outside,Hand,non_Hand
        }
        public enum AI_class{
            Hand,Water
        }
        public int area_label = 0; //if bigger than  threshold : 1 
        public int area_size = 0;// area size 
        public short value = 0;
        static int R = 35, C = 46;
        public int position_r, position_c;
        public bool is_peak = false;
        public Color draw_color;
        public AI_class Class;
        public Sensor_data(short value,bool is_peak,int position_r,int position_c)
        {
            this.value = value;
            this.is_peak = is_peak;
            this.position_c = position_c;
            this.position_r = position_r;
            this.draw_color = Node.GrayTonew_color_V(value+127);
        }
        public Sensor_data(short value, bool is_peak, int position_r, int position_c, int area_label, int area_size) : this(value, is_peak, position_r, position_c)
        {
            this.area_label = area_label;
            this.area_size = area_size;
        }
        public static bool BoundaryCheck(int r, int c)
        {
            return r >= 0 && c >= 0 && r < R && c < C;
        }
    }

    public class Node
    {
        public int X, Y, r, c;
        public Node()
        {

        }
        public Node(int X,int Y,int r,int c)
        {
            this.X = X;
            this.Y = Y;
            this.r = r;
            this.c = c;
        }
        public static void save_arround(short[] input,Form1.Save_data s )
        {
            Bitmap bitmap = new Bitmap(350, 350);
            Graphics e = Graphics.FromImage(bitmap);
            if (input.Length == 25)
            {
                for (int i = 0; i < 6; i++)
                {
                    e.DrawLine(new Pen(Color.Black), i * 70, 0, i * 70, 349);
                    e.DrawLine(new Pen(Color.Black), 0, i * 70, 349, i * 70);
                }
                e.DrawLine(new Pen(Color.Black), 349, 0, 349, 350);
                e.DrawLine(new Pen(Color.Black), 0, 349, 350, 349);
                Font drawFont = new Font("Arial", 20, FontStyle.Regular);
                int num = 0;
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        e.FillRectangle(new SolidBrush(GrayTonew_color_V(normailze(input[num]))), new Rectangle(70 * j, 70 * i, 70, 70));
                        e.DrawString(input[num].ToString(), drawFont, new SolidBrush(Color.Black), new PointF(70 * j + 20, 70 * i + 20));
                        num++;
                    }
                }
                bitmap.Save(string.Format("{0}{1}_{2}_{3}_{4}_({5},{6}).png", s.path,s.NN_output,s.area,s.lenght.ToString("F2"),s.second_area,s.x,s.y));
            }
            else
            {
                for (int i = 0; i < 8; i++)
                {
                    e.DrawLine(new Pen(Color.Black), i * 50, 0, i * 50, 349);
                    e.DrawLine(new Pen(Color.Black), 0, i * 50, 349, i * 50);
                }
                e.DrawLine(new Pen(Color.Black), 349, 0, 349, 350);
                e.DrawLine(new Pen(Color.Black), 0, 349, 350, 349);
                Font drawFont = new Font("Arial", 15, FontStyle.Regular);
                int num = 0;
                for (int i = 0; i < 7; i++)
                {
                    for (int j = 0; j < 7; j++)
                    {
                        e.FillRectangle(new SolidBrush(GrayTonew_color_V(normailze(input[num]))), new Rectangle(50 * j, 50 * i, 50, 50));
                        e.DrawString(input[num].ToString(), drawFont, new SolidBrush(Color.Black), new PointF(50 * j + 10, 50 * i + 10));
                        num++;
                    }
                }
                bitmap.Save(string.Format("{0}{1}_{2}_{3}_{4}_({5},{6}).png", s.path, s.NN_output, s.area, s.lenght.ToString("F2"), s.second_area, s.x, s.y));
            }
        }
        private static int normailze(int v)
        {
            return v + 127;
        }
        public static Color GrayTonew_color_V(int val)
        {
            if (val == 0)
            {
                return Color.FromArgb(255, 0, 0, 0);
            }
            else if (val <= 42 && val>0)
            {
                return Color.FromArgb(255, 127+val * 3, 0, val*3);
            }
            else if (val <= 84 && val > 42)
            {
                val -= 42;
                return Color.FromArgb(255, 255, val * 3, 127);
            }
            else if (val <= 126 && val > 84)
            {
                val -= 84;
                return Color.FromArgb(255, 255, 127 + val * 3, 127 + val * 3);
            }
            else if (val <= 168 && val > 126)
            {
                val -= 126;
                return Color.FromArgb(255, 255 - val * 3, 255, 255-val*3);
            }
            else if (val <= 210 && val > 168)
            {
                val -= 168;
                return Color.FromArgb(255,127-val*3, 255 , 127);
            }
            else
            {
                val -= 210;
                return Color.FromArgb(255, 0, 255, 127 - (int)(val * 2.7));
            }

        }
    }
    public class Touch_point:Node
    {
       public double NN_output;
       public int sucessful = 0, interval_time = 0,disapper_time = 0,all_time = 0;
       public bool can_out = false, have_child = false;
        public List<Point> draw_point = new List<Point>();
        public Color color;
       public Touch_point()
       {

       }
       public Touch_point(int X,int Y,int r ,int c,double NN_output)
        {
            this.X = X;
            this.Y = Y;
            this.r = r;
            this.c = c;
            this.NN_output = NN_output;
            this.color = get_init_color();
            

        }
        public void update(int X,int Y,int r,int c, double NN_output)
        {
            sucessful++;//成功次數+1
            this.X = X;
            this.Y = Y;
            this.r = r;
            this.c = c;
            this.NN_output = NN_output;
            interval_time = 0;
        }
        private Color get_init_color()
        {
            Random rand = new Random();
            
            return Color.FromArgb(rand.Next(50,255), rand.Next(0, 255), rand.Next(80, 255));
        }
        public void add_point_to_list(Point p)
        {
            draw_point.Insert(0, p);
          
        }
    }
    
    
}
