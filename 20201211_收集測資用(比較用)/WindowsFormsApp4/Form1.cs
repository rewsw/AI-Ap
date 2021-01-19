#define compare
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.IO;

namespace WindowsFormsApp4
{
    public partial class Form1 : Form
    {
        public enum state : int
        {
            Normal, draw,Base
        }

        public struct save_frame
        {
            public Save_data[] Area;
            public save_frame(Save_data[] Area)
            {
                this.Area = Area;
            }
        }
        public struct Vector3
        {
            public double x;
            public double y;
            public double z;
            public Vector3(double x, double y, double z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }
        }
        Color[] Rand_color = new Color[100];
        public struct Save_data
        {
            public short[] data_7x7;
            public int frame_id;
            public string path;
            public int area;
            public double lenght;
            public int x, y;
            public int second_area;
            public Save_data(short[] data_7x7, int frame_id, string path, int area, double lenght, int x, int y, int second_area)
            {
                this.frame_id = frame_id;
                this.path = path;
                this.data_7x7 = data_7x7;
                this.area = area;
                this.lenght = lenght;
                this.x = x;
                this.y = y;
                this.second_area = second_area;
            }
        }
        public struct area
        {
            public int size;
            public int min_x, min_y, max_x, max_y;
            public area(int size)
            {
                min_x = 100;
                min_y = 100;
                max_x = 0;
                max_y = 0;
                this.size = size;
            }
        }
        bool record = false;
        public const int r = 43, c = 75;
        public static int peak = 20;
        List<Node> big_node = new List<Node>();
        //List<Sensor_data> data = new List<Sensor_data>();
        Sensor_data[,] data = new Sensor_data[r, c];
        int p_w;
        int p_h;
        Car car = new Car(0, 0, 0, 0, false, new List<Point>());
        state Now_state = state.Normal;
        List<Point> Draw_point = new List<Point>();
        int[,] class_array = new int[,] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
        int[,] score_range = new int[,] { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } };
        int[,] BASE = new int[r,c];
        StreamWriter csv_swriter;
        StreamWriter csv_all_swriter;
        StreamWriter csv_bitmap_swriter;
        int Max_Diff = 500, Min_Diff = -500;
        int Max_ADiff = 2000, Min_ADiff = 1000;
        int alpha = 200;
        int AVG_times = 10;
        string data_path = System.Windows.Forms.Application.StartupPath + @"\\" + "2020_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "\\";
        int Base_time = 0;
        int frame_id = 0;
        DataTable dt;
        string H_save_path = System.Windows.Forms.Application.StartupPath + @"\\" + "2020_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "\\Picture";
        string nH_save_path = System.Windows.Forms.Application.StartupPath + @"\\" + "2020_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "\\nH";
        public Form1()
        {
            #region color initialize
            Random rand = new Random();
            for (int i = 0; i < 100; i++)
            {

                Rand_color[i] = Color.FromArgb(rand.Next(0, 125), rand.Next(0, 254), rand.Next(0, 100));
            }
            Console.WriteLine(Rand_color[0].R + " " + Rand_color[0].G + " " + Rand_color[0].B);

            Console.WriteLine(Rand_color[1].R + " " + Rand_color[1].G + " " + Rand_color[1].B);
            #endregion
            InitializeComponent();
            //this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            //  this.TopMost = true;
            // supervised = new AI(@"\\10.1.2.88\jack2\David\AI\NN\20201207 Level1 Binary Cross entropy\2020_12_10_資料還沒旋轉的(目前主要測試用)\month_12_day_8_time_11_27\ckpt\weight.csv");
            // supervised = new AI(@"\\10.1.2.88\jack2\David\AI\NN\20201207 Level1 Binary Cross entropy\2020_12_14_拿掉Palm做拇指按壓測試\month_12_day_11_time_17_26\ckpt\weight.csv");
            //supervised = new AI(@"\\10.1.2.88\jack2\David\AI\NN\20201207 Level1 Binary Cross entropy\month_12_day_15_time_20_48\ckpt\weight.csv");
            // supervised_ori = new AI(@"\\10.1.2.88\jack2\David\AI\NN\20201207 Level1 Binary Cross entropy\month_12_day_23_time_11_29\ckpt\weight.csv");
            //supervised_Hao_one = new AI(@"\\10.1.2.88\jack2\David\AI\NN\20201215 Level1 Binary Cross Entropy Hao Net\month_12_day_16_time_15_59\ckpt\weight.csv");
            // supervised_Hao_two = new AI(@"\\10.1.2.88\jack2\David\AI\NN\20201215 Level1 Binary Cross Entropy Hao Net(2)\month_12_day_21_time_15_34\ckpt\weight.csv");
          //  OpenCSV(ref BASE,@"\\10.1.2.88\jack2\David\Base\21.5_90k_Base.csv");

        }
        Thread t;

        private void Form1_Load(object sender, EventArgs e)
        {
            EndPoint IP;
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint server_ad = new IPEndPoint(IPAddress.Any, 5098);
            socket.Bind(server_ad);
            t = new Thread(new ThreadStart(delegate { callback_method(socket); }));
            t.Start();
            MAX_Diff_tb.Text = Max_Diff.ToString();
            MIN_Diff_tb.Text = Min_Diff.ToString();
            MAX_ADiff_tb.Text = Max_ADiff.ToString();
            MIN_ADiff_tb.Text = Min_ADiff.ToString();
            pictureBox1.Location = new Point(5, 5);
            pictureBox1.Size = new Size(this.Size.Width - 300, this.Height - 100);
            pictureBox2.Location = new Point(0, 0);
            pictureBox2.Size = pictureBox1.Size;
            pictureBox2.Parent = pictureBox1;
            pictureBox2.BackColor = Color.Transparent;

            BASE_bar.Location = new Point((int)(this.Size.Width*0.87), (int)(this.Size.Height*0.1));
            BASE_bar.Size = new Size((int)(this.Size.Width * 0.10), (int)(this.Size.Height * 0.02));
            BASE_bar.Maximum = AVG_times;
            data_lbox.Location = new Point((int)(this.Size.Width * 0.87), (int)(this.Size.Height * 0.15));
            data_lbox.Size = new Size((int)(this.Size.Width * 0.10), (int)(this.Size.Height * 0.5));

            label1.Location = new Point((int)(this.Size.Width * 0.87), (int)(this.Size.Height * 0.7));
            label2.Location = new Point((int)(this.Size.Width * 0.9), (int)(this.Size.Height * 0.7));
            label3.Location = new Point((int)(this.Size.Width * 0.93), (int)(this.Size.Height * 0.7));
            label4.Location = new Point((int)(this.Size.Width * 0.96), (int)(this.Size.Height * 0.7));

            MAX_Diff_tb.Location = new Point((int)(this.Size.Width * 0.87), (int)(this.Size.Height * 0.72));
            MIN_Diff_tb.Location = new Point((int)(this.Size.Width * 0.9), (int)(this.Size.Height * 0.72));
            MAX_ADiff_tb.Location = new Point((int)(this.Size.Width * 0.93), (int)(this.Size.Height * 0.72));
            MIN_ADiff_tb.Location = new Point((int)(this.Size.Width * 0.96), (int)(this.Size.Height * 0.72));

            Result_lb.Location = new Point((int)(this.Size.Width * 0.87), (int)(this.Size.Height * 0.8));
            this.KeyPreview = true;
            pic1_size = new Size(pictureBox1.Width, pictureBox1.Height);
            pic2_size = new Size(pictureBox2.Width, pictureBox2.Height);

            Directory.CreateDirectory(nH_save_path);
            Directory.CreateDirectory(H_save_path);
            #region 新創csv
            FileStream fs = new FileStream(data_path + "data.csv", FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.Flush();
            sw.Close();
            fs.Close();
            fs = new FileStream(data_path + "all.csv", FileMode.Create, FileAccess.Write);
            sw = new StreamWriter(fs);
            sw.Flush();
            sw.Close();
            fs.Close();
            fs = new FileStream(data_path + "bitmap.csv", FileMode.Create, FileAccess.Write);
            sw = new StreamWriter(fs);
            sw.Flush();
            sw.Close();
            fs.Close();
            #endregion
            csv_swriter = new StreamWriter(data_path + "data.csv", true);
            csv_all_swriter = new StreamWriter(data_path + "all.csv", true);
            csv_bitmap_swriter = new StreamWriter(data_path + "bitmap.csv", true);
            save_t = new Thread(new ThreadStart(save_picture));
            save_t.Start();
        }
        bool BASE_OK = false;
        bool have_peak;
        int supervised_num = 0;
        Thread save_t;
        List<Touch_point> now_can_out_Point = new List<Touch_point>();
        List<Touch_point> Alternative = new List<Touch_point>();
        List<save_frame> save_frames = new List<save_frame>();
        List<area> area_size_set = new List<area>();
        
        bool Save_all = false;
        void callback_method(Socket sc)
        {
            int frame_number = 0;
            int record_number = 0;
            Stopwatch sw = new Stopwatch();
            while (true)
            {
                sw.Reset();
                sw.Start();
                int dataLength;
                byte[] myBufferBytes = new byte[100000];
                dataLength = sc.Receive(myBufferBytes);
                frame_number++;
                int now_r = 0, now_c = 0;
                if (Base_time < AVG_times)
                {
                    for (int i = 0; i < dataLength; i = i + 2)
                    {
                        Int16 r_data = (Int16)((byte)myBufferBytes[i] << 8 | (byte)myBufferBytes[i + 1]);
                        BASE[now_r, now_c] += r_data;
                        now_r = (now_c == c - 1) ? now_r + 1 : now_r;
                        now_c = (now_c == c - 1) ? 0 : now_c + 1;
                        
                    }
                    Base_time++;
                    BeginInvoke(new MethodInvoker(() =>
                    {
                        BASE_bar.Value++;
                    }));
                    continue;
                }
                now_r = 0;
                now_c = 0;
                if (Base_time == AVG_times)
                {
          
                    
                    for (int i = 0; i < r; ++i)
                    {
                        for (int j = 0; j < c; ++j)
                        {
                            BASE[i, j] /= AVG_times;
                        }
                    }

                    BASE_OK = true;
                    Base_time++;
                }
                if (BASE_OK)
                {
                    int diff_sum = 0;
                    int diff_abs_sum = 0;
                    for (int i = 0; i < dataLength; i = i + 2)
                    {
                        Int16 ans = (Int16)((byte)myBufferBytes[i] << 8 | (byte)myBufferBytes[i + 1]);
                        ans = (Int16)(alpha - ans * (alpha / (double)BASE[now_r, now_c]));
                        diff_sum += ans;
                        diff_abs_sum += Math.Abs(ans);
                        data[now_r, now_c] = new Sensor_data(ans, false, now_r, now_c, (ans > 37) ? 1 : 0, 0); //add label and area size initliaize
                        now_r = (now_c == c - 1) ? now_r + 1 : now_r;
                        now_c = (now_c == c - 1) ? 0 : now_c + 1;
                    }
                    bool have_peak = false;
                    for(int i = 0; i < r; i++)
                    {
                        for(int j = 0; j < c; j++)
                        {
                            if(PatternMatch(ref data, i, j))
                            {
                                data[i, j].is_peak = true;
                                have_peak = true;
                            }
                        }
                    }
                    if (!have_peak)
                    {
                        if (diff_sum > Max_Diff || diff_sum < Min_Diff || diff_abs_sum > Max_ADiff || diff_abs_sum < Min_ADiff)
                        {
                            Result_lb.BeginInvoke(new MethodInvoker(() =>
                            {
                                Result_lb.Text = " Dectect Noise";
                                Result_lb.ForeColor = Color.Red;
                            }));
                        }
                        else//no noise
                        {
                            Result_lb.BeginInvoke(new MethodInvoker(() =>
                            {
                                Result_lb.Text = "Normal";
                                Result_lb.ForeColor = Color.Green;
                            }));
                        }
                    }
                    else
                    {
                        Result_lb.BeginInvoke(new MethodInvoker(() =>
                        {
                            Result_lb.Text = "Peak";
                            Result_lb.ForeColor = Color.Blue;
                        }));
                    }
                    if (Save_all)
                    {
                        csv_class.write_all_csv(ref csv_all_swriter, ref data, r, c);
                        csv_class.write_bitmap_csv(ref csv_bitmap_swriter, ref data, r, c);
                    }
                    frame_id++;
                    try
                    {
                        data_lbox.BeginInvoke(new MethodInvoker(() =>
                        {
                            data_lbox.Items.Add(string.Format("Diff Sum {0}  Diff abs Sum {1}", diff_sum, diff_abs_sum));
                            this.data_lbox.TopIndex =
                                            this.data_lbox.Items.Count - (int)(this.data_lbox.Height / this.data_lbox.ItemHeight);
                        }));
                    }
                    catch (InvalidOperationException ex)
                    {

                    }
                    pictureBox1.Invalidate();
                }
                sw.Stop();
                double fps = sw.ElapsedMilliseconds;
                try
                {
                    BeginInvoke(new MethodInvoker(() =>
                    {
                        this.Text = "Noise Detect Fps : " + (1 / fps * 1000.0).ToString("F2");
                    }));
                }
                catch (InvalidOperationException ex)
                {

                }
                
            }
        }
        double get_dis(Touch_point a, Point p)
        {
            return Math.Sqrt(Math.Pow(a.X - p.X, 2) + Math.Pow(a.Y - p.Y, 2));
        }
        static int[] dirX = new int[] { -1, 0, 1, -1, 1, -1, 0, 1 };
        static int[] dirY = new int[] { -1, -1, -1, 0, 0, 1, 1, 1 };
        int get_Negative_value(ref List<short> ans, ref int second_area)
        {
            int sum = 0;
            for (int i = 0; i < ans.Count; i++)
            {
                second_area += (ans[i] > 10) ? 1 : 0;
                sum += (ans[i] < 0) ? ans[i] : 0;
            }
            return sum;
        }
        static void seed_filling(ref Sensor_data[,] pixel, int x, int y, int label, ref area area_size)
        {
            pixel[y, x].area_label = label;
            area_size.size++;
            if (area_size.max_x < x)
                area_size.max_x = x;

            if (area_size.max_y < y)
                area_size.max_y = y;

            if (area_size.min_x > x)
                area_size.min_x = x;

            if (area_size.min_y > y)
                area_size.min_y = y;

            for (int i = 0; i < 8; i++)
            {
                if (dirY[i] + y >= 0 && dirY[i] + y < Form1.r && dirX[i] + x >= 0 && dirX[i] + x < Form1.c && pixel[dirY[i] + y, dirX[i] + x].area_label == 1)
                {
                    pixel[dirY[i] + y, dirX[i] + x].area_label = label;
                    seed_filling(ref pixel, dirX[i] + x, dirY[i] + y, label, ref area_size);
                }
            }
        }

        void save_picture()
        {
            while (true)
            {
                if (save_frames.Count != 0)
                {
                    try
                    {
                        for (int i = 0; i < save_frames[0].Area.Length; i++)
                        {
                            csv_class.WriteCVS(ref csv_swriter, save_frames[0].Area[i]);
                        }
                        for (int i = 0; i < save_frames[0].Area.Length; i++)
                        {
                            Node.save_arround(save_frames[0].Area[i].data_7x7, save_frames[0].Area[i]);
                        }

                        save_frames.RemoveAt(0);
                    }
                    catch (NullReferenceException ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
        }

        Sensor_data get_date(ref Sensor_data[,] s_data, int row, int col)
        {
            return s_data[row, col];
        }
        List<short> get_arround(ref Sensor_data[,] s_data, int row, int col)
        {
            List<short> ans = new List<short>();
            for (int y = -2; y <= +2; ++y)
            {
                for (int x = -2; x <= +2; ++x)
                {
                    int tr = row + y;
                    int tc = col + x;
                    if (!Sensor_data.BoundaryCheck(tr, tc))
                    {
                        ans.Add(0);
                    }
                    else
                    {
                        short sig = get_date(ref s_data, tr, tc).value;
                        //*bp = (sig <= 0)? 0: sig;
                        ans.Add(sig);
                    }

                }
            }
            return ans;
        }
        List<short> get_arround7x7(ref Sensor_data[,] s_data, int row, int col)
        {
            List<short> ans = new List<short>();
            for (int y = -3; y <= +3; ++y)
            {
                for (int x = -3; x <= +3; ++x)
                {
                    int tr = row + y;
                    int tc = col + x;
                    if (!Sensor_data.BoundaryCheck(tr, tc))
                    {
                        ans.Add(0);
                    }
                    else
                    {
                        short sig = get_date(ref s_data, tr, tc).value;
                        ans.Add(sig);
                    }
                }
            }
            return ans;
        }
        bool check_in_car(int row, int col)
        {
            int car_row = car.draw_sub_point[car.index].Y / p_h;
            int car_col = car.draw_sub_point[car.index].X / p_w;
            bool inside = Math.Sqrt(Math.Pow(row - car_row, 2) + Math.Pow(col - car_col, 2)) < 1.6 ? true : false;
            return inside;
        }
        public enum compare
        {
            more, more_equal
        }
        bool PatternMatch(ref Sensor_data[,] s_data, int row, int col)
        {
            int[] dirX = new int[] { -1, 0, 1, -1, 1, -1, 0, 1 };
            int[] dirY = new int[] { -1, -1, -1, 0, 0, 1, 1, 1 };
            compare[] com = new compare[] { compare.more_equal, compare.more_equal, compare.more_equal, compare.more, compare.more_equal, compare.more, compare.more, compare.more };

            short mid = get_date(ref s_data, row, col).value;
            if (mid < peak) return false;

            for (int d = 0; d < dirX.Length; ++d)
            {
                int tr = row + dirY[d];
                int tc = col + dirX[d];
                if (!Sensor_data.BoundaryCheck(tr, tc))
                    continue;
                switch (com[d])
                {
                    case compare.more:
                        if (get_date(ref s_data, tr, tc).value > mid || get_date(ref s_data, tr, tc).is_peak)
                            return false;
                        break;
                    case compare.more_equal:
                        if (get_date(ref s_data, tr, tc).value >= mid || get_date(ref s_data, tr, tc).is_peak)
                            return false;
                        break;
                    default:
                        break;
                }

            }
            get_date(ref s_data, row, col).is_peak = true;
            return true;
        }

        private List<Point> get_sub_point(List<Node> node_list)
        {
            List<Point> sub_path = new List<Point>();
            for (int i = 0; i < node_list.Count - 1; i++)
            {
                Point start = new Point(node_list[i].X * p_w + p_w / 2, node_list[i].Y * p_h + p_h / 2);
                Point end = new Point(node_list[i + 1].X * p_w + p_w / 2, node_list[i + 1].Y * p_h + p_h / 2);
                Vector3 line = cross(new Vector3(start.X, start.Y, 1), new Vector3(end.X, end.Y, 1));
                if (Math.Abs(start.X - end.X) > Math.Abs(start.Y - end.Y))
                {
                    if (start.X < end.X)
                    {
                        for (int j = start.X; j < end.X; j++)
                        {
                            int x = j;
                            int y = (int)((-1 * line.z + (-1 * line.x * j)) / line.y);

                            sub_path.Add(new System.Drawing.Point(x, y));
                        }
                    }
                    else if (start.X == end.X)
                    {
                        //int St = 0, Se = 0;
                        if (start.Y > end.Y)
                        {
                            for (int s = start.Y; s > end.Y; s--)
                            {
                                sub_path.Add(new Point(start.X, s));
                            }
                        }
                        else
                        {
                            for (int s = start.Y; s < end.Y; s++)
                            {
                                sub_path.Add(new Point(start.X, s));
                            }
                        }
                    }
                    else
                    {
                        for (int j = start.X; j > end.X; j--)
                        {
                            int x = j;
                            int y = (int)((-1 * line.z + (-1 * line.x * j)) / line.y);
                            //_e.DrawArc(pen, new Rectangle(new Point(x - 2, y - 2), new Size(4, 4)), 0, 360);
                            sub_path.Add(new System.Drawing.Point(x, y));
                            // Console.WriteLine(x + " " + y);
                        }
                    }
                }
                else
                {
                    if (start.Y < end.Y)
                    {
                        for (int j = start.Y; j < end.Y; j++)
                        {
                            int y = j;
                            int x = (int)((-1 * line.z + (-1 * line.y * j)) / line.x);

                            sub_path.Add(new System.Drawing.Point(x, y));
                        }
                    }
                    else if (start.Y == end.Y)
                    {
                        //int St = 0, Se = 0;
                        if (start.Y > end.Y)
                        {
                            for (int s = start.Y; s > end.Y; s--)
                            {
                                sub_path.Add(new Point(start.X, s));
                            }
                        }
                        else
                        {
                            for (int s = start.Y; s < end.Y; s++)
                            {
                                sub_path.Add(new Point(start.X, s));
                            }
                        }
                    }
                    else
                    {
                        for (int j = start.Y; j > end.Y; j--)
                        {
                            int y = j;
                            int x = (int)((-1 * line.z + (-1 * line.y * j)) / line.x);
                            //_e.DrawArc(pen, new Rectangle(new Point(x - 2, y - 2), new Size(4, 4)), 0, 360);
                            sub_path.Add(new System.Drawing.Point(x, y));
                            // Console.WriteLine(x + " " + y);
                        }
                    }
                }
            }
            return sub_path;
        }
        Size pic1_size, pic2_size;
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

            switch (e.KeyData)
            {
                case Keys.A:
                    MessageBox.Show(this.Size.Width + " " + this.Size.Height);
                    break;

                case Keys.NumPad4:
                    pic1_size = new Size(pictureBox1.Size.Width - 1, pictureBox1.Size.Height);
                    //update_size = true;
                    break;
                case Keys.NumPad6:
                    pic1_size = new Size(pictureBox1.Size.Width + 1, pictureBox1.Size.Height);
                    //  update_size = true;
                    break;
                case Keys.NumPad2:
                    pic1_size = new Size(pictureBox1.Size.Width, pictureBox1.Size.Height + 1);
                    // update_size = true;
                    break;
                case Keys.NumPad8:
                    pic1_size = new Size(pictureBox1.Size.Width, pictureBox1.Size.Height - 1);
                    //  update_size = true;
                    break;
                case Keys.S:
                    Save_all = true;
                    break;
                case Keys.C:

                    break;
                case Keys.H:
                    record = !record;

                    //   record_lbox.Items.Clear();
                    DateTime saveNow = DateTime.Now;

                    break;
                case Keys.E:
                    Draw_point.Clear();
                    //ans_lb.Text = "ans : " + string.Format("{.2f}", (record_lbox.Items.Count / (double)(record_lbox.Items.Count + no_record_lbox.Items.Count)) * 100.0);
                    //  no_record_lbox.Items.Clear();
                    //record_lbox.Items.Clear();
                    break;
                case Keys.D:
                    Now_state = state.draw;
                    Draw_point.Clear();
                    break;
                case Keys.B:
                    Now_state = state.Base;
                    break;
                case Keys.N:
                    Now_state = state.Normal;
                    break;
                default:
                    break;
            }
            pic2_size = new Size(pic1_size.Width, pic1_size.Height);
            //  pictureBox1.Invalidate();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            csv_swriter.Flush();
            csv_swriter.Close();
            System.Environment.Exit(System.Environment.ExitCode);
        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                Node node = new Node(e.X / p_w, e.Y / p_h, e.Y / p_h, e.X / p_w);
                big_node.Add(node);
                var st = "r: {0} c: {1}";
                st = String.Format(st, node.r, node.c);
                //   node_lbox.Items.Add(st);
            }
            //  pictureBox2.Invalidate();
        }

        private void MIN_Diff_tb_TextChanged(object sender, EventArgs e)
        {
            try {
                Min_Diff = int.Parse(MIN_Diff_tb.Text);
            } catch (FormatException ex) {
                Min_Diff = 0;
            }
        }

        private void MAX_Diff_tb_TextChanged(object sender, EventArgs e)
        {
            try {
                Max_Diff = int.Parse(MAX_Diff_tb.Text);
            } catch (FormatException ex) {
                Max_Diff = 0;
            }
            
        }

        private void MAX_ADiff_tb_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Max_ADiff = int.Parse(MAX_ADiff_tb.Text);
            }catch(FormatException ex) {
                Max_ADiff = 0; 
            }
        }

        private void MIN_ADiff_tb_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Min_ADiff = int.Parse(MIN_ADiff_tb.Text);
            }
            catch (FormatException ex) {
                Min_ADiff = 0;
            }
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            switch (Now_state)
            {
                case state.Normal:
                    if (big_node.Count != 0)
                    {
                        for (int i = 0; i < big_node.Count; i++)
                        {
                            g.DrawEllipse(new Pen(Color.Blue, 2), new Rectangle(big_node[i].X * p_w, big_node[i].Y * p_h, p_w, p_h));
                            if (i < big_node.Count - 1)
                            {
                                Point start = new Point(big_node[i].X * p_w + p_w / 2, big_node[i].Y * p_h + p_h / 2);
                                Point end = new Point(big_node[i + 1].X * p_w + p_w / 2, big_node[i + 1].Y * p_h + p_h / 2);
                                g.DrawLine(new Pen(Color.Yellow), start, end);
                            }
                        }
                    }
                    if (car.can_go)
                    {
                        g.DrawArc(new Pen(Color.Red), new Rectangle((int)(car.draw_sub_point[car.index].X - 1.5 * p_w),
                            (int)(car.draw_sub_point[car.index].Y - 1.5 * p_h), p_w * 3, p_h * 3), 0, 360);
                    }
                    break;
                case state.draw:
                    int count = Alternative.Count;
                    if (count > 0)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            if (Alternative[i].draw_point.Count != 0)
                            {
                                for (int j = 0; j < Alternative[i].draw_point.Count; j++)
                                {
                                    g.DrawArc(new Pen(Alternative[i].color, 5), new Rectangle((int)(Alternative[i].draw_point[j].X - p_w / 3),
                                        (int)(Alternative[i].draw_point[j].Y - p_h / 3), p_w / 3 * 2, p_h / 3 * 2), 0, 360);
                                }
                                if (Alternative[i].draw_point.Count > 3)
                                {
                                    g.DrawCurve(new Pen(Alternative[i].color, 5), Alternative[i].draw_point.ToArray());
                                }
                            }

                        }

                    }
                    for (int i = 0; i < 256; i++)
                    {
                        g.DrawRectangle(new Pen(Node.GrayTonew_color_V(i)), new Rectangle(10, 30 + i * 2, 20, 2));

                    }

                    break;
                default:
                    break;
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {

            Graphics g = e.Graphics;
            p_w = pictureBox1.Width / c;
            p_h = pictureBox1.Height / r;
            switch (Now_state)
            {
                case state.Normal:
                    int row = 0, col = 0;
                        for (int i = 0; i < data.Length; i++)
                        {
                            if (data[row, col] != null)
                            {
                                String drawString = data[row, col].value.ToString();

                                Font drawFont = new Font("Arial", 8);
                                SolidBrush drawBrush = new SolidBrush(Color.Black);
                                if (data[row, col].area_label > 1)
                                {
                                    g.FillRectangle(new SolidBrush(Rand_color[data[row, col].area_label]), new Rectangle(data[row, col].position_c * p_w, data[row, col].position_r * p_h, p_w, p_h));
                                }
                                else
                                    g.FillRectangle(new SolidBrush(data[row, col].draw_color), new Rectangle(data[row, col].position_c * p_w, data[row, col].position_r * p_h, p_w, p_h));
                                if (data[row, col].is_peak)
                                    g.DrawEllipse(new Pen((data[row, col].Class == Sensor_data.AI_class.Water) ? Color.Blue : Color.Red, 2), new Rectangle(data[row, col].position_c * p_w, data[row, col].position_r * p_h, p_w, p_h));
                                //if (i == 0)
                                //{
                                //    g.DrawString(frame_id.ToString(), drawFont, drawBrush, new PointF(0 * p_w + (p_w / 3), 0 * p_h + (p_h / 3)));
                                //}else
                                g.DrawString(drawString, drawFont, drawBrush, new PointF(col * p_w + (p_w / 3)-5, row * p_h + (p_h / 3)));

                                row = (col == c - 1) ? row + 1 : row;
                                col = (col == c - 1) ? 0 : col + 1;
                            }
                            else
                            {
                                break;
                            }
                        }
                    
                    for (int i = 0; i < area_size_set.Count; i++)
                    {
                        g.DrawLine(new Pen(Color.Red, 2), area_size_set[i].min_x * p_w + (p_w / 3), area_size_set[i].min_y * p_h + (p_h / 3),
                            area_size_set[i].max_x * p_w + (p_w / 3), area_size_set[i].max_y * p_h + (p_h / 3));
                        // Console.WriteLine(String.Format("{0} {1} {2} {3}", area_size_set[i].min_x, area_size_set[i].min_y, area_size_set[i].max_x, area_size_set[i].max_y));
                    }
                    for (int i = 0; i <= r; i++)
                    {
                        g.DrawLine(new Pen(Color.Black), 0, i * p_h, p_w * c, i * p_h);

                    }
                    for (int j = 0; j <= c; j++)
                    {
                        g.DrawLine(new Pen(Color.Black), j * p_w, 0, j * p_w, p_h * r);
                    }
                    break;
                case state.draw:
                    break;
                case state.Base:
                    int B_row = 0, B_col = 0;
                    for (int i = 0; i < data.Length; i++)
                    {
                        if (BASE[B_row, B_col] != null)
                        {
                            String drawString = BASE[B_row, B_col].ToString();

                            Font drawFont = new Font("Arial", 8);
                            SolidBrush drawBrush = new SolidBrush(Color.Black);
                           
                            g.FillRectangle(new SolidBrush(data[B_row, B_col].draw_color), new Rectangle(data[B_row, B_col].position_c * p_w, data[B_row, B_col].position_r * p_h, p_w, p_h));
                            
                            g.DrawString(drawString, drawFont, drawBrush, new PointF(B_col * p_w + (p_w / 3), B_row * p_h + (p_h / 3)));

                            B_row = (B_col == c - 1) ? B_row + 1 : B_row;
                            B_col = (B_col == c - 1) ? 0 : B_col + 1;
                        }
                        else
                        {
                            break;
                        }
                    }

                    for (int i = 0; i < area_size_set.Count; i++)
                    {
                        g.DrawLine(new Pen(Color.Red, 2), area_size_set[i].min_x * p_w + (p_w / 3), area_size_set[i].min_y * p_h + (p_h / 3),
                            area_size_set[i].max_x * p_w + (p_w / 3), area_size_set[i].max_y * p_h + (p_h / 3));
                        // Console.WriteLine(String.Format("{0} {1} {2} {3}", area_size_set[i].min_x, area_size_set[i].min_y, area_size_set[i].max_x, area_size_set[i].max_y));
                    }
                    for (int i = 0; i <= r; i++)
                    {
                        g.DrawLine(new Pen(Color.Black), 0, i * p_h, p_w * c, i * p_h);

                    }
                    for (int j = 0; j <= c; j++)
                    {
                        g.DrawLine(new Pen(Color.Black), j * p_w, 0, j * p_w, p_h * r);
                    }
                    break;
                default:
                    break;

            }
        }
        private Vector3 cross(Vector3 left, Vector3 right)
        {
            Vector3 ans;
            ans.x = left.y * right.z - left.z * right.y;
            ans.y = left.z * right.x - left.x * right.z;
            ans.z = left.x * right.y - left.y * right.x;
            return ans;
        }
        public void OpenCSV(ref int[,] array ,string filePath)
        {
          
            FileStream fs = new FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            StreamReader sr = new StreamReader(fs, Encoding.UTF8);

            string strLine = "";

            string[] aryLine;
            int row = 0;
            while ((strLine = sr.ReadLine()) != null)
            {
                aryLine = strLine.Split(',');
               
               for (int j = 0; j < aryLine.Length; j++)
               {
                    try
                    {
                        BASE[row, j] = (int)double.Parse(aryLine[j]);
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine(aryLine[j]);
                    }
                }
    
                row++;
            }
            //if (aryLine != null && aryLine.Length > 0)
            //{
            //    dt.DefaultView.Sort = tableHead[2] + " " + "DESC";
            //}
            sr.Close();
            fs.Close();

        }
    }
}
