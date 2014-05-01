using System.Drawing;

namespace PrintServer
{
    public class PrintNode
    {
        public int Top { get; set; }
        public int Left { get; set; }
        public string Name { get; set; }
        public Color Color { get; set; }
        public Font Font { get; set; }
    }
}
