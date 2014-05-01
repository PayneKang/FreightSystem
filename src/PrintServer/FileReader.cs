using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace PrintServer
{
    public class FileReader
    {
        public const int BUFFERSIZE = 10240;
        public static string ReadFile(string filename)
        {
            FileStream s = new FileStream(filename, FileMode.Open);
            byte[] buffer = new byte[BUFFERSIZE];
            int count = s.Read(buffer, 0, BUFFERSIZE);
            StringBuilder sb = new StringBuilder();
            while (count > 0)
            {
                sb.Append(System.Text.Encoding.UTF8.GetString(buffer, 0, count));
                count = s.Read(buffer, 0, BUFFERSIZE);
            }
            s.Close();
            return sb.ToString();
        }
    }
}
