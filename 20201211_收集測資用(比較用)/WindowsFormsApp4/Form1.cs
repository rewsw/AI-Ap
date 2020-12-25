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
            Normal, draw
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
        int r = 35, c = 46;
        List<Node> big_node = new List<Node>();
        //List<Sensor_data> data = new List<Sensor_data>();
        Sensor_data[,] data = new Sensor_data[35, 46];
        int p_w;
        int p_h;
        Car car = new Car(0, 0, 0, 0, false, new List<Point>());
        state Now_state = state.Normal;
        List<Point> Draw_point = new List<Point>();
        int[,] class_array = new int[,] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
        int[,] score_range = new int[,] { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } };
        StreamWriter csv_swriter;
        string data_path = System.Windows.Forms.Application.StartupPath + @"\\" + "2020_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "\\";
        bool update_size = false;
        int frame_id = 0;
        AI supervised_ori, supervised_Hao_one, supervised_Hao_two;
        DataTable dt;
        string H_save_path = System.Windows.Forms.Application.StartupPath + @"\\" + "2020_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "\\Picture";
        string nH_save_path = System.Windows.Forms.Application.StartupPath + @"\\" + "2020_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "\\nH";
        public Form1()
        {
            #region color initialize
            Random rand = new Random();
            for (int i = 0; i < 100; i++)
            {

                Rand_color[i] = Color.FromArgb(rand.Next(0, 254), rand.Next(0, 254), rand.Next(0, 25));
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
            supervised_ori = new AI(@"\\10.1.2.88\jack2\David\AI\NN\20201207 Level1 Binary Cross entropy\month_12_day_23_time_11_29\ckpt\weight.csv");
            //supervised_Hao_one = new AI(@"\\10.1.2.88\jack2\David\AI\NN\20201215 Level1 Binary Cross Entropy Hao Net\month_12_day_16_time_15_59\ckpt\weight.csv");
            supervised_Hao_two = new AI(@"\\10.1.2.88\jack2\David\AI\NN\20201215 Level1 Binary Cross Entropy Hao Net(2)\month_12_day_21_time_15_34\ckpt\weight.csv");


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

            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Size = new Size(this.Size.Width - 300, this.Height + 30);
            pictureBox2.Location = new Point(0, 0);
            pictureBox2.Size = pictureBox1.Size;
            pictureBox2.Parent = pictureBox1;
            pictureBox2.BackColor = Color.Transparent;


            Fps_lb.Location = new Point((int)(this.Size.Width * 0.85), (int)(this.Height * 0.95));

            dataGridView1.Location = new Point((int)(this.Size.Width * 0.85), (int)(this.Height * 0.1));
            dataGridView1.Size = new Size((int)(this.Width * 0.15), (int)(this.Height * 0.6));

            dataGridView2.Location = new Point((int)(this.Size.Width * 0.85), (int)(this.Height * 0.75));
            dataGridView2.Size = new Size((int)(this.Width * 0.15), (int)(this.Height * 0.2));

            dataGridView1.ColumnCount = 3;
            dataGridView1.RowCount = 11;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ColumnHeadersVisible = false;
            dataGridView1.Columns[0].Width = dataGridView1.Width / 3;
            dataGridView1.Columns[1].Width = dataGridView1.Width / 3;
            dataGridView1.Columns[2].Width = dataGridView1.Width / 3;
            for (int i = 0; i < 11; i++)
            {
                dataGridView1.Rows[i].Height = dataGridView1.Height / 11 - 1;
            }
            double f = 0.0;
            for (int i = 1; i < 11; i++)
            {

                dataGridView1.Rows[i].Cells[0].Value = f.ToString("F1") + " ~ " + (f + 0.1).ToString("F1");
                f += 0.1;
            }
            dataGridView1.DataSource = dt;
            dataGridView1.Rows[0].Cells[0].Value = "數級";
            dataGridView1.Rows[0].Cells[1].Value = "New";
            dataGridView1.Rows[0].Cells[2].Value = "Ori";
            dataGridView2.ColumnCount = 4;
            dataGridView2.RowCount = 7;
            dataGridView2.RowHeadersVisible = false;
            dataGridView2.ColumnHeadersVisible = false;

            for (int i = 0; i < 4; i++)
            {
                dataGridView2.Columns[i].Width = dataGridView2.Width / 4 - 4;
            }
            dataGridView2.Rows[0].Height = dataGridView2.Height / 7;
            dataGridView2.Rows[1].Height = dataGridView2.Height / 7;
            dataGridView2.Rows[2].Height = dataGridView2.Height / 7 - 1;
            dataGridView2.Rows[3].Height = dataGridView2.Height / 7 - 1;
            dataGridView2.Rows[4].Height = dataGridView2.Height / 7 - 1;
            dataGridView2.Rows[5].Height = dataGridView2.Height / 7 - 1;
            dataGridView2.Rows[6].Height = dataGridView2.Height / 7 - 1;

            dataGridView2.Rows[0].Cells[1].Value = "水";
            dataGridView2.Rows[0].Cells[2].Value = "不定";
            dataGridView2.Rows[0].Cells[3].Value = "手";

            dataGridView2.Rows[1].Cells[0].Value = "數量(new)";
            dataGridView2.Rows[2].Cells[0].Value = "<0.1 or >0.9";
            dataGridView2.Rows[3].Cells[0].Value = "不定";
            dataGridView2.Rows[4].Cells[0].Value = "數量(ori)";
            dataGridView2.Rows[5].Cells[0].Value = "<0.1 or >0.9";
            dataGridView2.Rows[6].Cells[0].Value = "不定";

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
            #endregion
            csv_swriter = new StreamWriter(data_path + "data.csv", true);
            save_t = new Thread(new ThreadStart(save_picture));
            save_t.Start();
        }

        bool have_peak;
        int supervised_num = 0;
        Thread save_t;
        List<Touch_point> now_can_out_Point = new List<Touch_point>();
        List<Touch_point> Alternative = new List<Touch_point>();
        List<save_frame> save_frames = new List<save_frame>();
        List<area> area_size_set = new List<area>();
        void callback_method(Socket sc)
        {
            int frame_number = 0;
            Stopwatch sw = new Stopwatch();
            while (true)
            {
                //sw.Reset();
                //sw.Start();
                int dataLength;
                byte[] myBufferBytes = new byte[100000];
                dataLength = sc.Receive(myBufferBytes);
                frame_number++;
                int now_r = 0, now_c = 0;
                for (int i = 0; i < dataLength; i = i + 2)
                {
                    Int16 ans = (Int16)((byte)myBufferBytes[i] << 8 | (byte)myBufferBytes[i + 1]);
                    data[now_r, now_c] = new Sensor_data(ans, false, now_r, now_c, (ans > 37) ? 1 : 0, 0); //add label and area size initliaize
                    now_r = (now_c == 45) ? now_r + 1 : now_r;
                    now_c = (now_c == 45) ? 0 : now_c + 1;
                }
                frame_id++;
                List<Save_data> sl = new List<Save_data>();
                int label = 2;
                area_size_set.Clear();
                for (int i = 0; i < r; ++i)
                {
                    for (int j = 0; j < c; ++j)
                    {
                        if (PatternMatch(ref data, i, j)) //i 是 row j 是 col
                        {

                            List<short> ans = get_arround(ref data, i, j);
                            area area = new area(0);

                            if (data[i, j].area_label == 1) //caluate area if is label == 1
                            {

                                seed_filling(ref data, j, i, label, ref area);
                                area_size_set.Add(area);
                                label++;
                            }
                            else //already caluate area 
                            {
                                area = area_size_set[data[i, j].area_label - 2];
                            }
                            double bevel_edge_lenght = Math.Sqrt(Math.Pow(area.min_x - area.max_x, 2) + Math.Pow(area.min_y - area.max_y, 2));
                            ans = get_arround7x7(ref data, i, j);
                            int second_area = 0;
                            int sum = get_Negative_value(ref ans, ref second_area);
                            sl.Add(new Save_data(ans.ToArray(), frame_id, data_path + "Picture", area.size, bevel_edge_lenght, j, i, second_area));
                            have_peak = true;
                        }
                    }
                }
                if (have_peak)
                {
                    save_frames.Add(new save_frame(sl.ToArray()));
                    have_peak = false;
                }
                double fps = sw.ElapsedMilliseconds;
                try
                {
                    BeginInvoke(new MethodInvoker(() =>
                    {

                    }));
                }
                catch (InvalidOperationException ex)
                {

                }
                pictureBox1.Invalidate();
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
                if (dirY[i] + y >= 0 && dirY[i] + y < 35 && dirX[i] + x >= 0 && dirX[i] + x < 46 && pixel[dirY[i] + y, dirX[i] + x].area_label == 1)
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
            if (mid < 38) return false;

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
                    update_size = true;
                    break;
                case Keys.NumPad6:
                    pic1_size = new Size(pictureBox1.Size.Width + 1, pictureBox1.Size.Height);
                    update_size = true;
                    break;
                case Keys.NumPad2:
                    pic1_size = new Size(pictureBox1.Size.Width, pictureBox1.Size.Height + 1);
                    update_size = true;
                    break;
                case Keys.NumPad8:
                    pic1_size = new Size(pictureBox1.Size.Width, pictureBox1.Size.Height - 1);
                    update_size = true;
                    break;
                case Keys.S:
                    if (big_node.Count >= 2 && !car.can_go)
                    {
                        car = new Car(big_node[0].X, big_node[0].Y, big_node[0].r, big_node[0].c, true, get_sub_point(big_node));
                    }
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
                            g.DrawString(drawString, drawFont, drawBrush, new PointF(col * p_w + (p_w / 3), row * p_h + (p_h / 3)));

                            row = (col == 45) ? row + 1 : row;
                            col = (col == 45) ? 0 : col + 1;
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
    }
}
