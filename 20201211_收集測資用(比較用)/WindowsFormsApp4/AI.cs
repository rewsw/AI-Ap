using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace WindowsFormsApp4
{
    public class AI
    {
        Dictionary<string, double[][]> dic;
        public AI(string weight_csv_path)
        {
           dic = get_weight(weight_csv_path);
        }
        public double[] calculate(short[] data)
        {
            double[] Fc1 = new double[dic["fc1.bias"][0].Length];
            for (int i = 0; i < dic["fc1.weight"].Length; i++)
            {
                double sum = 0;
                for (int j = 0; j < dic["fc1.weight"][i].Length; j++)
                {
                    sum += (data[j] * dic["fc1.weight"][i][j]);
                }
                Fc1[i] = sum + dic["fc1.bias"][0][i];
                Fc1[i] = (Fc1[i] > 0) ? Fc1[i] : 0;
            }

            double[] Fc2 = new double[dic["fc2.bias"][0].Length];
            for (int i = 0; i < dic["fc2.weight"].Length; i++)
            {
                double sum = 0;
                for (int j = 0; j < dic["fc2.weight"][0].Length; j++)
                {
                    sum += (Fc1[j] * dic["fc2.weight"][i][j]);
                }
                Fc2[i] = sum + dic["fc2.bias"][0][i];
                Fc2[i] = (Fc2[i] > 0) ? Fc2[i] : 0;
            }

            double[] Fc3 = new double[dic["fc3.bias"][0].Length];
            for (int i = 0; i < dic["fc3.weight"].Length; i++)
            {
                double sum = 0;
                for (int j = 0; j < dic["fc3.weight"][0].Length; j++)
                {
                    sum += (Fc2[j] * dic["fc3.weight"][i][j]);
                }
                Fc3[i] = sum + dic["fc3.bias"][0][i];
                Fc3[i] = (Fc3[i] > 0) ? Fc3[i] : 0;
            }

            double[] Fc4 = new double[dic["fc4.bias"][0].Length];
            for (int i = 0; i < dic["fc4.weight"].Length; i++)
            {
                double sum = 0;
                for (int j = 0; j < dic["fc4.weight"][0].Length; j++)
                {
                    sum += (Fc3[j] * dic["fc4.weight"][i][j]);
                }
                Fc4[i] = sum + dic["fc4.bias"][0][i];
                Fc4[i] = (Fc4[i] > 0) ? Fc4[i] : 0;
            }

            double[] Fc5 = new double[dic["fc5.bias"][0].Length];
            for (int i = 0; i < dic["fc5.weight"].Length; i++)
            {
                double sum = 0;
                for (int j = 0; j < dic["fc5.weight"][0].Length; j++)
                {
                    sum += (Fc4[j] * dic["fc5.weight"][i][j]);
                }
                Fc5[i] = sum + dic["fc5.bias"][0][i];
                Fc5[i] = (Fc5[i] > 0) ? Fc5[i] : 0;
            }

            double[] Fc6 = new double[dic["fc6.bias"][0].Length];
            for (int i = 0; i < dic["fc6.weight"].Length; i++)
            {
                double sum = 0;
                for (int j = 0; j < dic["fc6.weight"][0].Length; j++)
                {
                    sum += (Fc5[j] * dic["fc6.weight"][i][j]);
                }
                Fc6[i] = sum + dic["fc6.bias"][0][i];
                
            }
            if(Fc6.Length == 1)
            {
                Fc6[0] = sigmoid(Fc6[0]);
            }
            return Fc6;
        }
        public double[] calculate(double[] data)
        {
            double[] Fc1 = new double[dic["fc1.bias"][0].Length];
            for (int i = 0; i < dic["fc1.weight"].Length; i++)
            {
                double sum = 0;
                for (int j = 0; j < dic["fc1.weight"][i].Length; j++)
                {
                    sum += (data[j] * dic["fc1.weight"][i][j]);
                }
                Fc1[i] = sum + dic["fc1.bias"][0][i];
                Fc1[i] = (Fc1[i] > 0) ? Fc1[i] : 0;
            }

            double[] Fc2 = new double[dic["fc2.bias"][0].Length];
            for (int i = 0; i < dic["fc2.weight"].Length; i++)
            {
                double sum = 0;
                for (int j = 0; j < dic["fc2.weight"][0].Length; j++)
                {
                    sum += (Fc1[j] * dic["fc2.weight"][i][j]);
                }
                Fc2[i] = sum + dic["fc2.bias"][0][i];
                Fc2[i] = (Fc2[i] > 0) ? Fc2[i] : 0;
            }

            double[] Fc3 = new double[dic["fc3.bias"][0].Length];
            for (int i = 0; i < dic["fc3.weight"].Length; i++)
            {
                double sum = 0;
                for (int j = 0; j < dic["fc3.weight"][0].Length; j++)
                {
                    sum += (Fc2[j] * dic["fc3.weight"][i][j]);
                }
                Fc3[i] = sum + dic["fc3.bias"][0][i];
                Fc3[i] = (Fc3[i] > 0) ? Fc3[i] : 0;
            }

            double[] Fc4 = new double[dic["fc4.bias"][0].Length];
            for (int i = 0; i < dic["fc4.weight"].Length; i++)
            {
                double sum = 0;
                for (int j = 0; j < dic["fc4.weight"][0].Length; j++)
                {
                    sum += (Fc3[j] * dic["fc4.weight"][i][j]);
                }
                Fc4[i] = sum + dic["fc4.bias"][0][i];
                Fc4[i] = (Fc4[i] > 0) ? Fc4[i] : 0;
            }

            double[] Fc5 = new double[dic["fc5.bias"][0].Length];
            for (int i = 0; i < dic["fc5.weight"].Length; i++)
            {
                double sum = 0;
                for (int j = 0; j < dic["fc5.weight"][0].Length; j++)
                {
                    sum += (Fc4[j] * dic["fc5.weight"][i][j]);
                }
                Fc5[i] = sum + dic["fc5.bias"][0][i];
                Fc5[i] = (Fc5[i] > 0) ? Fc5[i] : 0;
            }

            double[] Fc6 = new double[dic["fc6.bias"][0].Length];
            for (int i = 0; i < dic["fc6.weight"].Length; i++)
            {
                double sum = 0;
                for (int j = 0; j < dic["fc6.weight"][0].Length; j++)
                {
                    sum += (Fc5[j] * dic["fc6.weight"][i][j]);
                }
                Fc6[i] = sum + dic["fc6.bias"][0][i];

            }
            if (Fc6.Length == 1)
            {
                Fc6[0] = sigmoid(Fc6[0]);
            }
            return Fc6;
        }
        double sigmoid(double x)
        {
            return 1 / (1 + Math.Exp(-x));
        }
        Dictionary<string, double[][]> get_weight(string filePath)
        {
            Dictionary<string, double[][]> weight = new Dictionary<string, double[][]>();
            FileStream fs = new FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            StreamReader sr = new StreamReader(fs, Encoding.UTF8);
            string strLine = "";
            bool can_read = false;
            string Now_dic_name = "";
            List<double[]> row = new List<double[]>();
            while ((strLine = sr.ReadLine()) != null)
            {
                string[] sp = strLine.Split(',');

                if (sp.Length == 1 && !double.TryParse(sp[0], out double a))
                {

                    if (row.Count != 0)
                    {
                        weight.Add(Now_dic_name, row.ToArray());
                        row.Clear();
                    }
                    Now_dic_name = sp[0];
                }
                else
                {
                    double[] data = new double[sp.Length];
                    for (int i = 0; i < sp.Length; i++)
                    {
                        data[i] = double.Parse(sp[i]);
                    }

                    row.Add(data);
                }

            }
            weight.Add(Now_dic_name, row.ToArray());//最後一筆
            foreach (var s in weight)
            {
               // Console.WriteLine(s.Key);
             //   print_matrix(s.Value);
            }
            sr.Close();
            fs.Close();
            return weight;
        }
    }
}
