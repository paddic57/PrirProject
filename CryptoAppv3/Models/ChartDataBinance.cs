using System.Collections.Generic;

namespace CryptoAppv3.Models
{
    public class ChartDataBinance
    {
        public double lastUpdateId { get; set; }
        public List<List<string>> bids { get; set; }
        public List<List<string>> asks { get; set; }

        public List<double> bidsQuantity = new List<double>();
        public List<double> asksQuantity = new List<double>();
        public List<string> asksPrices = new List<string>();
        public List<string> bidsPrices = new List<string>();

    }
}
