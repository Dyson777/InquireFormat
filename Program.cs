using System;
using System.Text;
using System.IO;

namespace InquireFormat
{
    class Program
    {
        // 定义静态类用于存储全局变量
        public static class GlobalVariables
        {
            // 定义StringBuilder全局变量
            public static StringBuilder stringBuilder = new StringBuilder();
        }

        static void Main(string[] args)
        {
            Console.Title = "InquireFormat"; // 设置窗口标题
            Console.WriteLine();
            if (args.Length > 0)
            {
                Console.WriteLine("{0}\t\t{1}", "图形格式", "文件名"); // 输出列标题
                Console.WriteLine("\t\t\t\t");

                // 遍历拖拽的文件/文件夹
                foreach (string arg in args)
                {
                    if (IsDwgFile(arg))
                    {
                        // 输出dwg文件格式
                        OutputFormat(arg);
                    }
                    else if (Directory.Exists(arg))
                    {
                        foreach (string item in Directory.GetFiles(arg, "*.*", SearchOption.AllDirectories))
                        {
                            // 空文件夹或非dwg文件则继续下一个循环
                            if (item == null || !IsDwgFile(item))
                                continue;

                            // 输出dwg文件格式
                            OutputFormat(item);
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please drag and drop dwg files or folders on the icon."); // 提示需要拖拽文件
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        // 检查文件是否为dwg图形
        private static bool IsDwgFile(string filePath)
        {
            // 获取文件扩展名
            string extension = Path.GetExtension(filePath);

            // 使用字符串比较(不区分大小写)检查扩展名是否为".dwg"
            return StringComparer.OrdinalIgnoreCase.Equals(extension, ".dwg");
        }

        // 输出图形格式
        private static void OutputFormat(string filePath)
        {
            // 尝试打开文件
            try
            {
                // 创建StreamReader对象读取文件
                using (StreamReader streamReader = new StreamReader(filePath))
                {
                    int singleChar;
                    int charCount = 0;
                    // 循环读取6位字符追加到StringBuilder
                    while ((singleChar = streamReader.Read()) != -1 && charCount < 6)
                    {
                        GlobalVariables.stringBuilder.Append((char)singleChar);
                        charCount++;
                    }
                }
                string format = "";
                switch (GlobalVariables.stringBuilder?.ToString())
                {
                    case "AC1015":
                        format = "AutoCAD 2000";
                        break;
                    case "AC1018":
                        format = "AutoCAD 2004";
                        break;
                    case "AC1021":
                        format = "AutoCAD 2007";
                        break;
                    case "AC1024":
                        format = "AutoCAD 2010";
                        break;
                    case "AC1027":
                        format = "AutoCAD 2013";
                        break;
                    case "AC1032":
                        format = "AutoCAD 2018";
                        break;
                    default:
                        format = "unknown\t";
                        break;
                }

                // 清空字符串
                GlobalVariables.stringBuilder.Clear();

                // 输出图形格式和文件名
                Console.ResetColor();
                Console.WriteLine("{0}\t\t{1}", format, Path.GetFileName(filePath));
            }
            catch (IOException)
            {
                // 文件被占用等异常情况输出提示
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("open failed\t\t{0}", Path.GetFileName(filePath));
            }
        }
    }
}
