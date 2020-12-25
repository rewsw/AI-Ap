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
        public static void WriteCVS(ref StreamWriter swd, string fileName,int x,int y,int frame_id, short[] data) //for_榮創的
        {

           
            StringBuilder sbd = new StringBuilder();

            sbd.Append(frame_id).Append(",").Append(x).Append(",").Append(y).Append(",");
            for(int i = 0; i < data.Length-1; i++)
            {
                sbd.Append(data[i]).Append(",");
            }
            sbd.Append(data[data.Length - 1]);
            swd.WriteLine(sbd);
           
        }
    }
}
