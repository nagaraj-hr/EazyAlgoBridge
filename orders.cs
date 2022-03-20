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
    public partial class orders : Form
    {
        public orders(KK kiteConnect1)
        {
            InitializeComponent();
            List<Order> orders = kiteConnect1.kite1.GetOrders();
            if (orders == null)
                return;
            foreach (Order order1 in orders)
            {
                //orderstring = orderstring + "Time: " + order1.OrderTimestamp.ToString() + " Type: " + order1.TransactionType + " Instrument: " + order1.Tradingsymbol + " Product: " + order1.Product + " Qty: " + order1.Quantity.ToString() + " Avg.price: " + order1.AveragePrice.ToString() + " Status: " + order1.Status + "\n\n";
                ordersGrid.Columns["avgprice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                ordersGrid.Rows.Add(order1.OrderTimestamp.ToString(), order1.TransactionType, order1.Tradingsymbol, order1.Product, order1.Quantity.ToString(), order1.AveragePrice.ToString("#,0.00"), order1.Status);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
