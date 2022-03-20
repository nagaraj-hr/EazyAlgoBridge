using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KiteConnect;
using System.Windows.Forms;

namespace EazyAlgoBridge
{
    public partial class KK
    {
        public Kite kite1;
        public string MyAccessToken = "";
        public string MyPublicToken ="";
        public string kurl;
        public string MyAPIKey;
        public string MySecret;
        public string MyRequestToken;
        public KiteConnect.User MyUser;
        Action MyActionMethod = KiteSessionExpiry;
        static Ticker ticker;
        public string InitializeKite(string apiKey, string apiSecrect)
        {
            string retval = "";
            try
            {
                // Import library
                MyAPIKey = apiKey; //"0cp2m1keh8tsny46";
                MySecret = apiSecrect; //"859g7y8thr2mj9w80yw9iilsxqs0zqf5";

                // Initialize Kiteconnect using apiKey. Enabling Debug will give logs of requests and responses
                kite1 = new Kite(MyAPIKey, Debug: true);

                // Collect login url to authenticate user. Load this URL in browser or WebView. 
                // After successful authentication this will redirect to your redirect url with request token.
                kurl = kite1.GetLoginURL();
                retval = kurl;
                globalClass.writetoLogFile("Kite login URL: " + kurl);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                globalClass.writetoLogFile("Exception in InitializeKite: " + e.Message);
                retval = "";
            }
            finally
            {
                ;
            }

            return kurl;
        }
        public int StartKiteSession(string apiRequestToken, string apiSecret)
        {
            int retval = 0;
            try
            {
                // System.Diagnostics.Process.Start(kurl);
                // Collect tokens and user details using the request token
                MyUser = kite1.GenerateSession(apiRequestToken, apiSecret);

                // Persist these tokens in database or settings
                MyAccessToken = MyUser.AccessToken;
                MyPublicToken = MyUser.PublicToken;

                // Initialize Kite APIs with access token
                kite1.SetAccessToken(MyAccessToken);

                // Set session expiry callback. Method can be separate function also.
                //kite1.SetSessionExpiryHook(() => MessageBox.Show("Need to login again"));
                kite1.SetSessionExpiryHook(KiteSessionExpiry);
                globalClass.writetoLogFile("Success StartKiteSession");

                //start ticker 
                KK.initTicker(MyAPIKey, MyAccessToken);
                //
                retval = 1;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                globalClass.writetoLogFile("Exception in StartKiteSession: " + e.Message);
                retval = e.HResult;
            }
            finally
            {
                ;
            }
            return retval;
        }


        // getPostiongs
        public void GetPostitions()
        {
            // Console.WriteLine("Order Id: " + response["data"]["order_id"]);
            string posstring = "";
            // Example call for functions like "GetHoldings" that returns a data structure
            PositionResponse pos = kite1.GetPositions();
            if (pos.Net == null)
                return;
            foreach (Position pos1 in pos.Net)
            {
                posstring = posstring + "Product "  + pos1.Product.ToString() + " Instrument: " + pos1.TradingSymbol.ToString() + " Quantity: " + pos1.Quantity.ToString() + " Avg. " + pos1.AveragePrice.ToString() + " LTP: " + pos1.LastPrice.ToString() + " PNL: " + pos1.PNL.ToString()   + "\n\n";

            }
            //MessageBox.Show(posstring);

            // Console.WriteLine(holdings[0].AveragePrice);
        }
        public void GetOrders()
        {
            string orderstring = "";
            List<Order> orders = kite1.GetOrders();
            if (orders == null)
                return;
            foreach (Order order1 in orders)
            {
                orderstring= orderstring + "Time: " + order1.OrderTimestamp.ToString() + " Type: " + order1.TransactionType + " Instrument: " + order1.Tradingsymbol +  " Product: " + order1.Product + " Qty: " + order1.Quantity.ToString() + " Avg.price: " + order1.AveragePrice.ToString() + " Status: " + order1.Status  + "\n\n";
            }
            MessageBox.Show(orderstring);

        }

        public void GetEquityMargins()
        {
            string marginDetails = "";
            UserMargin equityMargins = kite1.GetMargins(Constants.MARGIN_EQUITY);
            if (equityMargins.Net.ToString() == null)
                return;
            marginDetails = "Opening Balance: " + equityMargins.Available.Cash.ToString("#,0.00")  + "\n\nNet value: " + equityMargins.Net.ToString("#,0.00")  + "\n\nUtilised margin: " + equityMargins.Utilised.M2MRealised.ToString("#,0.00") + "\n\nUnrealized: " + equityMargins.Utilised.M2MUnrealised.ToString("#,0.00")  + "\n\nAvailable: " + equityMargins.Available.Cash.ToString("#,0.00") + "\n\nIntraday payin: " + equityMargins.Available.IntradayPayin.ToString("#,0.00") + "\n\nTurnover: " + equityMargins.Utilised.Turnover.ToString("#,0.00") + "\n\nExposure: " + equityMargins.Utilised.Exposure.ToString("#,0.00") + "\n\nPayout: " + equityMargins.Utilised.Payout.ToString("#,0.00") + "\n\nOption premium: " + equityMargins.Utilised.OptionPremium.ToString("#,0.00");
            MessageBox.Show(marginDetails,"Margin Details");
        }

        public int Place_CO_Order1(string sExchange, string sSymbol, string sTransactionType, int iQuantity, decimal dPrice, string sProduct, string sOrderType, decimal sStoplossValue)
        {
            // Example call for functions like "PlaceOrder" that returns Dictionary
            int retval = 0;
            globalClass.writetoLogFile( "sExchange: " + sExchange + "sSymbol: " + sSymbol +  "sTransactionType: "  + sTransactionType + "iQuantity: " + iQuantity.ToString() + "dPrice: "  + dPrice.ToString() + "sProduct: " + sProduct + "sOrderType: " + sOrderType + "sStoplossValue: " + sStoplossValue.ToString());
            Dictionary<string, dynamic> response;
            try
            {
                response = kite1.PlaceOrder(
                    Exchange: sExchange,
                    TradingSymbol: sSymbol,
                    TransactionType: sTransactionType, //Constants.TRANSACTION_TYPE_SELL,
                    Quantity: iQuantity,
                    Price: dPrice,
                    OrderType: sOrderType, //Constants.ORDER_TYPE_MARKET,
                    Product: sProduct, //Constants.PRODUCT_MIS,
                    StoplossValue: sStoplossValue,
                    Variety: Constants.VARIETY_CO
                );
                globalClass.writetoLogFile("Order Id: " + response["data"]["order_id"]);
                return retval;
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message);
                globalClass.writetoLogFile("Exception Placing CO order: " + e.Message);
                retval = e.HResult;
            }
            finally
            {
                ;
            }

            return retval;
        }
        public int Place_BO_Order1(string sExchange, string sSymbol, string sTransactionType, int iQuantity, decimal dPrice, string sProduct, string sOrderType, decimal sStoplossValue, decimal sSquareOffValue)
        {
            // Example call for functions like "PlaceOrder" that returns Dictionary
            int retval = 0;
            Dictionary<string, dynamic> response;
            try
            {
                response = kite1.PlaceOrder(
                Exchange: sExchange,
                TradingSymbol: sSymbol,
                TransactionType: sTransactionType, //Constants.TRANSACTION_TYPE_SELL,
                Quantity: iQuantity,
                Price: dPrice,
                OrderType: sOrderType, //Constants.ORDER_TYPE_MARKET,
                Product: sProduct, //Constants.PRODUCT_MIS,
                StoplossValue: sStoplossValue,
                SquareOffValue: sSquareOffValue,
                Variety: Constants.VARIETY_BO

                );
                globalClass.writetoLogFile("Order Id: " + response["data"]["order_id"]);
                return retval;
            }
            catch (Exception e)
            {
               // MessageBox.Show(e.Message);
                globalClass.writetoLogFile("Exception Placing BO order: " + e.Message);
                retval = e.HResult;
            }
            finally
            {
                ;
            }

            return retval;
        }
        public int Place_NRML_Order1(string sExchange, string sSymbol, string sTransactionType, int iQuantity, decimal dPrice, string sProduct, string sOrderType)
        {
            // Example call for functions like "PlaceOrder" that returns Dictionary
            int retval = 0;
            Dictionary<string, dynamic> response;
            try
            {
                response = kite1.PlaceOrder(
                Exchange: sExchange,
                TradingSymbol: sSymbol,
                TransactionType: sTransactionType, //Constants.TRANSACTION_TYPE_SELL,
                Quantity: iQuantity,
                Price: dPrice,
                OrderType: sOrderType, //Constants.ORDER_TYPE_MARKET,
                Product: sProduct, //Constants.PRODUCT_MIS,
                Variety: Constants.VARIETY_REGULAR

                 );
                globalClass.writetoLogFile("Order Id: " + response["data"]["order_id"]);
                return retval;
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message);
                globalClass.writetoLogFile("Exception Placing NRML order: " + e.Message);
                retval = e.HResult;
            }
            finally
            {
                ;
            }

            return retval;
        }
        public int Place_MIS_Order1(string sExchange, string sSymbol, string sTransactionType, int iQuantity, decimal dPrice, string sProduct, string sOrderType)
        {
            // Example call for functions like "PlaceOrder" that returns Dictionary
            int retval = 0;
            Dictionary<string, dynamic> response;
            try
            {
                response = kite1.PlaceOrder(
                Exchange: sExchange,
                TradingSymbol: sSymbol,
                TransactionType: sTransactionType, //Constants.TRANSACTION_TYPE_SELL,
                Quantity: iQuantity,
                Price: dPrice,
                OrderType: sOrderType, //Constants.ORDER_TYPE_MARKET,
                Product: sProduct, //Constants.PRODUCT_MIS,
                Variety: Constants.VARIETY_REGULAR

                 );
                globalClass.writetoLogFile("Order Id: " + response["data"]["order_id"]);
                return retval;
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message);
                globalClass.writetoLogFile("Exception Placing MIS order: " + e.Message);
                retval = e.HResult;
            }
            finally
            {
                ;
            }

            return retval;
        }
        public static void KiteSessionExpiry()
        {
            //MessageBox.Show("Session expired");
            globalClass.writetoLogFile("Kite session Expired");
        }
        public static void initTicker(string apiKey, string accToken)
        {
            ticker = new Ticker(apiKey, accToken);

            ticker.OnTick += OnTick;
            ticker.OnReconnect += OnReconnect;
            ticker.OnNoReconnect += OnNoReconnect;
            ticker.OnError += OnError;
            ticker.OnClose += OnClose;
            ticker.OnConnect += OnConnect;
            ticker.OnOrderUpdate += OnOrderUpdate;

            ticker.EnableReconnect(Interval: 5, Retries: 50);
            ticker.Connect();

            // Subscribing to NIFTY50 and setting mode to LTP
            ticker.Subscribe(Tokens: new UInt32[] { 256265 });
            ticker.SetMode(Tokens: new UInt32[] { 256265 }, Mode: Constants.MODE_LTP);
        }

        private static void OnTokenExpire()
        {
            globalClass.writetoLogFile("Need to login again");
        }

        private static void OnConnect()
        {
            globalClass.writetoLogFile("Connected ticker");
        }

        private static void OnClose()
        {
            globalClass.writetoLogFile("Closed ticker");
        }

        private static void OnError(string Message)
        {
            globalClass.writetoLogFile("Error: " + Message);
        }

        private static void OnNoReconnect()
        {
            globalClass.writetoLogFile("Not reconnecting");
        }

        private static void OnReconnect()
        {
            globalClass.writetoLogFile("Reconnecting");
        }

        private static void OnTick(Tick TickData)
        {

            /*
            PositionResponse pos = kiteConnect1.kite1.GetPositions();
            if (pos.Net == null)
                return;
            foreach (Position pos1 in pos.Net)
            {
                decimal profitloss = pos1.SellValue - pos1.BuyValue + (pos1.Quantity * pos1.LastPrice * pos1.Multiplier);
                positionGrid.Rows.Add(pos1.Product.ToString(), pos1.TradingSymbol.ToString(), pos1.Quantity.ToString(), pos1.AveragePrice.ToString("#,0.00"), pos1.LastPrice.ToString("#,0.00"), profitloss.ToString("#,0.00")); //pos1.PNL.ToString("#,0.00")

            }*/

            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.Invoke((MethodInvoker)(() => f1.Controls["label3"].Text = "NIFTY: " + TickData.LastPrice.ToString("#,0.00")));


        }

        private static void OnOrderUpdate(Order OrderData)
        {
            globalClass.writetoLogFile("OrderUpdate " + Utils.JsonSerialize(OrderData));
        }

    }
}
