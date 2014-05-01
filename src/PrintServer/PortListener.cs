using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Web;
using System;
using System.Windows.Forms;
using System.IO;

namespace PrintServer
{
    public class PortListener
    {
        private TcpListener listener;
        private Thread listenerThread;
        public int Port { get; private set; }
        public bool Listening { get; private set; }
        public PortListener(int port)
        {
            this.Port = port;
            this.Listening = false;
        }
        public void StartListener()
        {
            this.Listening = true;
            listenerThread = new Thread(doListen);
            listenerThread.Start();
        }

        public void StopListener()
        {
            this.Listening = false;
            this.listener.Stop();
            listenerThread.Abort();
        }
        
        private void doListen()
        {
            this.listener = new TcpListener(this.Port);
            this.listener.Start();
            while (this.Listening)
            {
                TcpClient client = this.listener.AcceptTcpClient();
                if (!this.Listening)
                    return;
                ClientProcessor cp = new ClientProcessor(client);
                cp.StartPorcess();
            }
        }
    }
    public class ClientProcessor{
        public TcpClient Client { get; private set; }
        public string RequestString { get; private set; }
        public ClientProcessor(TcpClient client)
        {
            this.Client = client;
        }
        private string printData;
        public void StartPorcess()
        {
            try
            {
                byte[] buffer = new byte[102400];
                NetworkStream ns = Client.GetStream();
                int readcount = ns.Read(buffer, 0, 102400);
                this.RequestString = Encoding.UTF8.GetString(buffer, 0, readcount);
                Debug.Write(RequestString);
                printData = GetPrintDataString(this.RequestString);
                string outData = string.Empty;
                byte[] outbuff;
                if (string.IsNullOrEmpty(printData))
                {
                    outData = FileReader.ReadFile("StartToPrint.htm");
                    outbuff = Encoding.UTF8.GetBytes(outData);
                    ns.Write(outbuff, 0, outbuff.Length);
                    ns.Close();
                    Client.Close();
                    return;
                }

                outData = FileReader.ReadFile("StartToPrint.htm");
                outbuff = Encoding.UTF8.GetBytes(outData);
                ns.Write(outbuff, 0, outbuff.Length);
                ns.Close();
                Client.Close();
                Thread thd = new Thread(CreatePrintForm);
                thd.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("打印失败：{0}", ex.Message));
            }
        }
        private void CreatePrintForm()
        {
            PrinterForm printForm = new PrinterForm();
            printForm.PrintData = printData;
            printForm.ShowDialog();
            printForm.TopMost = true;
            printForm.BringToFront();
        }
        public string GetPrintDataString (string reqStr){
            string[] reqArr = reqStr.Replace("\r\n", "\n").Split('\n');
            string StartMark = "GET /";
            string EndMark = "HTTP";
            // GET /?cmd=print&data=year:2014,month:04,day:22,client:%E5%B0%8F%E5%A4%A9%E9%B9%85,trayno:%E5%B0%8F-01,packagename:%E5%B0%8F%E5%AE%B6%E7%94%B5,volumn:15,qty:4,from:%E6%9D%AD%E5%B7%9E,to:%E5%8C%97%E4%BA%AC,dirver:%E5%BC%A0%E4%B8%89,carno:%E6%B5%99ABCDEF,comment:%E9%98%BF%E6%96%AF%E9%A1%BF%E5%8F%91%E7%94%9F%E5%9C%B0%E6%96%B9 HTTP/1.1
            string getStr = string.Empty;
            foreach(string str in reqArr){
                string tmpstr = str.Trim();
                if (tmpstr.Contains(StartMark))
                {
                    getStr = tmpstr;
                    getStr = getStr.Substring(StartMark.Length, getStr.Length - StartMark.Length);
                    getStr = getStr.Substring(0, getStr.IndexOf(EndMark));
                    break;
                }
            }
            if (string.IsNullOrEmpty(getStr))
                return getStr;
            string[] paramArr = getStr.Split('&');
            foreach (string param in paramArr)
            {
                string[] paramItem = param.Split('=');
                if (paramItem.Length != 2)
                    continue;
                string name = paramItem[0];
                string paramvalue = paramItem[1];
                if (name == "data")
                    return HttpUtility.UrlDecode(paramvalue);
            }
            return string.Empty;
        }
    }
    
}
