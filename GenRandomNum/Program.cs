using org.in2bits.MyXls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GenRandomNum
{
    class Program
    {
        static void Main(string[] args)
        {
            int num = 0;
            int length = 0;
            int fileType = 0;
            int[] fileTypes = new int[] { 1, 2, 3 };
            Nullable<bool> containsChar = null;
            while (num == 0)
            {
                Console.Write("请输入要生成的个数：");
                string strNum = Console.ReadLine();
                int.TryParse(strNum, out num);
            }
            while (length == 0)
            {
                Console.Write("请输入长度：");
                string strLength = Console.ReadLine();
                int.TryParse(strLength, out length);
            }
            while (!fileTypes.Contains(fileType))
            {
                Console.Write("请输入保存的文件的类型(1、txt，2、csv，3、xls)：");
                string strFileType = Console.ReadLine();
                int.TryParse(strFileType, out fileType);
            }
            while (containsChar == null)
            {
                Console.Write("是否包含字符(0、不包含，1、包含)：");
                string strContains = Console.ReadLine();
                if (strContains.Equals("1"))
                {
                    containsChar = true;
                }
                else if (strContains.Equals("0"))
                {
                    containsChar = false;
                }
            }
            List<string> codes = GenCode(num, length, containsChar.Value);

            try
            {
                SaveFile(fileType, codes);
            }
            catch (Exception ex)
            {

            }

            Console.WriteLine("生成完成，按任意键退出... ...");
            Console.ReadKey();
        }
        #region 生成密码
        static List<string> GenCode(int num, int length, bool containsChar)
        {
            Random ro = new Random(10);
            long tick = DateTime.Now.Ticks;
            Random ran = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
            List<string> list = new List<string>();
            for (int i = 0; i < num; i++)
            {
                try
                {
                    string code = GenCode(ran, length, containsChar);
                    Console.WriteLine(code);
                    list.Add(code);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            return list;
        }
        static string GenCode(Random ran, int length, bool containChar)
        {
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = GenCode(ran, containChar);
            }
            return new string(chars);
        }
        static char GenCode(Random ran, bool containsChar)
        {
            int type = 0;
            if (containsChar)
            {
                type = ran.Next(0, 2);
            }
            if (type == 0)
            {
                return GenIntCode(ran);
            }
            else
            {
                return GenCharCode(ran);
            }
        }
        static char GenIntCode(Random ran)
        {
            int next = ran.Next(0, 10);
            return next.ToString()[0];
        }
        static char GenCharCode(Random ran)
        {
            int next = ran.Next(97, 123);
            return Convert.ToChar(next);
        }
        #endregion
        #region 保存文件
        static void SaveFile(int type, List<string> codes)
        {
            switch (type)
            {
                case 1:
                default:
                    SaveTXT(codes);
                    break;
                case 2:
                    SaveCSV(codes);
                    break;
                case 3:
                    SaveXLS(codes);
                    break;
            }
        }
        static void SaveTXT(List<string> codes)
        {
            if (codes == null) return;
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
            File.WriteAllLines(fileName, codes.ToArray(), Encoding.Default);
        }
        static void SaveCSV(List<string> codes)
        {
            if (codes == null) return;
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
            File.WriteAllLines(fileName, codes.ToArray(), Encoding.Default);
        }

        static void SaveXLS(List<string> codes)
        {
            try
            {
                XlsDocument xlsDoc = new XlsDocument();
                xlsDoc.FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";

                XF titleXF = xlsDoc.NewXF();
                titleXF.Font.Bold = true;
                titleXF.Font.FontName = "宋体";

                Worksheet sheet = xlsDoc.Workbook.Worksheets.Add("sheet1");

                for (int i = 0; i < codes.Count; i++)
                {
                    sheet.Cells.Add(i + 1, 1, codes[i]);
                }


                xlsDoc.Save();
            }
            catch (Exception ex)
            {
                throw new Exception("生成Excel文件失败", ex);
            }
        }

        #endregion
    }
}
