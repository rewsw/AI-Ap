using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace WindowsFormsApp4
{
    public class csv_class
    {
        public static void WriteCVS(ref StreamWriter swd, string fileName, int x, int y, int frame_id, short[] data) //收集訓練資料用途
        {


            StringBuilder sbd = new StringBuilder();

            sbd.Append(frame_id).Append(",").Append(x).Append(",").Append(y).Append(",");
            for (int i = 0; i < data.Length - 1; i++)
            {
                sbd.Append(data[i]).Append(",");
            }
            sbd.Append(data[data.Length - 1]);
            swd.WriteLine(sbd);

        }
        public static void WriteCVS(ref StreamWriter swd, Form1.Save_data Data)
        {
            StringBuilder sbd = new StringBuilder();

            sbd.Append(Data.frame_id).Append(",").Append(Data.x).Append(",").Append(Data.y).Append(",");
            for (int i = 0; i < Data.data_7x7.Length; i++)
            {
                sbd.Append(Data.data_7x7[i]).Append(",");
            }
            sbd.Append(Data.area).Append(",").Append(Data.lenght).Append(",").Append(Data.second_area);
            swd.WriteLine(sbd);
            swd.Flush();
            // Console.WriteLine(Data.frame_id);
        }
        public static void write_all_csv(ref StreamWriter swd, ref Sensor_data[,] data, int row, int col)
        {

            for (int i = 0; i < row; i++)
            {
                StringBuilder sbd = new StringBuilder();
                for (int j = 0; j < col; j++)
                {
                    sbd.Append(data[i, j].value).Append(",");
                }
                swd.WriteLine(sbd);
                swd.Flush();
            }


        }
        public static void write_bitmap_csv(ref StreamWriter swd, ref Sensor_data[,] data, int row, int col)
        {

            for (int i = 0; i < row; i++)
            {
                StringBuilder sbd = new StringBuilder();
                for (int j = 0; j < col; j++)
                {
                    sbd.Append((data[i, j].value >= Form1.peak) ? 1 : 0).Append(",");
                }
                swd.WriteLine(sbd);
                swd.Flush();
            }


        }
        public static void  write_all_csv(ref StreamWriter swd,ref Sensor_data[,] data,int row,int col)
        {
         
            for (int i = 0; i < row; i++)
            {
                StringBuilder sbd = new StringBuilder();
                for (int j = 0; j < col; j++)
                {
                    sbd.Append(data[i,j].value).Append(",");
                }
                swd.WriteLine(sbd);
                swd.Flush();
            }
            
           
        }
        public static void write_bitmap_csv(ref StreamWriter swd, ref Sensor_data[,] data, int row, int col)
        {

            for (int i = 0; i < row; i++)
            {
                StringBuilder sbd = new StringBuilder();
                for (int j = 0; j < col; j++)
                {
                    sbd.Append((data[i, j].value>=38)?1:0).Append(",");
                }
                swd.WriteLine(sbd);
                swd.Flush();
            }


        }
    }
}
