using System;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;

namespace PrintServer
{
    public partial class PrinterForm : Form
    {
        public PrinterForm()
        {
            InitializeComponent();
        }

        public string PrintData { get; set; }

        private void PrinterForm_Load(object sender, EventArgs e)
        {
            this.Refresh();
            Printer printer = new Printer();
            List<PrintContent> contents = printer.CreatePrintContent(this.PrintData);
            foreach (PrintContent content in contents)
            {
                Label lbl = new Label();
                lbl.AutoSize = true;
                lbl.Text = content.Content.Replace("|", "\r\n\r\n");
                lbl.Name = content.PrintNode.Name;
                lbl.Top = content.PrintNode.Top;
                lbl.Left = content.PrintNode.Left;
                lbl.BackColor = Color.FromArgb(0, 0, 255, 255);
                lbl.Font = content.PrintNode.Font;
                this.printArea.Controls.Add(lbl);
            }
            this.Refresh();
            this.TopMost = true;
            this.BringToFront();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool SetupPrinting(PrintDocument doc)
        {
            PrintPreviewDialog dialog = new PrintPreviewDialog();
            dialog.Document = doc;
            dialog.PrintPreviewControl.AutoZoom = false;
            dialog.PrintPreviewControl.Zoom = 0.75;
            dialog.ShowDialog();
            return true;
        }
        private Bitmap printImg;
        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintDocument doc = new PrintDocument();
            doc.PrintController = new StandardPrintController();
            doc.DocumentName = "货运单据";
            printImg = new Bitmap(this.printArea.Width, this.printArea.Height);
            this.printArea.DrawToBitmap(this.printImg, new Rectangle(0,0,this.printImg.Width,this.printImg.Height));
            Graphics g = Graphics.FromImage(printImg);
            doc.PrintPage += new PrintPageEventHandler(doc_PrintPage);
            if (SetupPrinting(doc))
            {
            }
        }

        void doc_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(this.printImg, 0, 0);
        }
    }
}
