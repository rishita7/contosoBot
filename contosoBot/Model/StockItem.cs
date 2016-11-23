namespace contosoBot.Services
{

    public class StockItem 
    {
        public string Status { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public float LastPrice { get; set; }
        public float Change { get; set; }
        public float ChangePercent { get; set; }
        public string Timestamp { get; set; }
        public int MSDate { get; set; }
        public long MarketCap { get; set; }
        public int Volume { get; set; }
        public float ChangeYTD { get; set; }
        public float ChangePercentYTD { get; set; }
        public float High { get; set; }
        public float Low { get; set; }
        public float Open { get; set; }
    }

}