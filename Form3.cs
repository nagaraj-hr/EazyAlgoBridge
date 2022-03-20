using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KiteConnect;
namespace EazyAlgoBridge
{
    public partial class Form3 : Form
    {
        public Form3( KK kiteConnect1)
        {
            InitializeComponent();
            // Console.WriteLine("Order Id: " + response["data"]["order_id"]);
            // Example call for functions like "GetHoldings" that returns a data structure
            PositionResponse pos = kiteConnect1.kite1.GetPositions();
            if (pos.Net == null)
                return;
            foreach (Position pos1 in pos.Net)
            {
                //posstring = posstring + "Product " + pos1.Product.ToString() + " Instrument: " + pos1.TradingSymbol.ToString() + " Quantity: " + pos1.Quantity.ToString() + " Avg. " + pos1.AveragePrice.ToString() + " LTP: " + pos1.LastPrice.ToString() + " PNL: " + pos1.PNL.ToString() + "\n\n";
                positionGrid.Columns["avgprice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                positionGrid.Columns["ltp"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                positionGrid.Columns["pnl"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                decimal profitloss = pos1.SellValue - pos1.BuyValue + (pos1.Quantity * pos1.LastPrice * pos1.Multiplier);
                positionGrid.Rows.Add(pos1.Product.ToString(), pos1.TradingSymbol.ToString(), pos1.Quantity.ToString(), pos1.AveragePrice.ToString("#,0.00"), pos1.LastPrice.ToString("#,0.00"), profitloss.ToString("#,0.00") ); //pos1.PNL.ToString("#,0.00")

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
