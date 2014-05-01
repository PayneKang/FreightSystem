using System.Drawing.Printing;
using System.Drawing;
using System.Collections.Generic;
using System.Xml;

namespace PrintServer
{
    public class Printer
    {
        public const string PRINT_CONFIG_FILE = "PrintConfig.xml";
        public Dictionary<string, PrintNode> PrintNodes { get; private set; }
        public Printer()
        {
            this.InitPrintNodes();
        }
        public void InitPrintNodes()
        {
            this.PrintNodes = new Dictionary<string, PrintNode>();
            XmlDocument doc = new XmlDocument();
            doc.Load(PRINT_CONFIG_FILE);
            XmlElement root = doc.DocumentElement;
            XmlNodeList nodes = root.SelectNodes("//PrintNodes/PrintNode");
            for (int i = 0; i < nodes.Count; i++)
            {
                XmlNode node = nodes.Item(i);
                int top = 0, left = 0;
                int.TryParse(node.Attributes["Top"].Value, out top);
                int.TryParse(node.Attributes["Left"].Value, out left);
                float size = 0f;
                float.TryParse(node.Attributes["Size"].Value, out size);
                Font font = new Font(node.Attributes["FontName"].Value, size);
                Color color = Color.FromName(node.Attributes["Color"].Value);
                PrintNode pn = new PrintNode()
                {
                    Name = node.Attributes["Name"].Value,
                    Font = font,
                    Top = top,
                    Left = left,
                    Color = color
                };
                PrintNodes.Add(pn.Name.ToUpper(), pn);
            }
        }
        public List<PrintContent> CreatePrintContent(string printdata)
        {
            string[] printItems = printdata.Split(',');
            List<PrintContent> result = new List<PrintContent>();
            foreach(string printItem in printItems){
                string[] itemArr = printItem.Split(':');
                string key = itemArr[0].ToUpper();
                string value = itemArr[1];
                PrintContent content = new PrintContent()
                {
                    PrintNode = PrintNodes[key],
                    Content = value
                };
                result.Add(content);
            }
            return result;
        }
    }
}
