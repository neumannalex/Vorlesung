using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vorlesung.Client.Pages.Stocks
{
    // Interne Darstellung

    public class IntrayDayDataset
    {
        public IntradayMetaData MetaData { get; set; }// = new IntradayMetaData();
        public List<IntradayRecord> Series { get; set; }// = new List<IntradayRecord>();
    }

    public class IntradayRecord
    {
        public DateTime Date { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public long Volume { get; set; }
    }

    public class IntradayMetaData
    {
        public string Information { get; set; }
        public string Symbol { get; set; }
        public DateTime LastRefreshed { get; set; }
        public string Interval { get; set; }
        public string OutputSize { get; set; }
        public string TimeZone { get; set; }
    }

    // AlphaVantage API

    public class AlphaVantageCompanyOverview
    {
        [JsonProperty("Symbol")]
        public string Symbol { get; set; }

        [JsonProperty("AssetType")]
        public string AssetType { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("Exchange")]
        public string Exchange { get; set; }

        [JsonProperty("Currency")]
        public string Currency { get; set; }

        [JsonProperty("Country")]
        public string Country { get; set; }

        [JsonProperty("Sector")]
        public string Sector { get; set; }

        [JsonProperty("Industry")]
        public string Industry { get; set; }

        [JsonProperty("Address")]
        public string Address { get; set; }

        [JsonProperty("FullTimeEmployees")]
        public string FullTimeEmployees { get; set; }

        [JsonProperty("FiscalYearEnd")]
        public string FiscalYearEnd { get; set; }

        [JsonProperty("LatestQuarter")]
        public string LatestQuarter { get; set; }

        [JsonProperty("MarketCapitalization")]
        public string MarketCapitalization { get; set; }

        [JsonProperty("EBITDA")]
        public string EBITDA { get; set; }

        [JsonProperty("PERatio")]
        public string PERatio { get; set; }

        [JsonProperty("PEGRatio")]
        public string PEGRatio { get; set; }

        [JsonProperty("BookValue")]
        public string BookValue { get; set; }

        [JsonProperty("DividendPerShare")]
        public string DividendPerShare { get; set; }

        [JsonProperty("DividendYield")]
        public string DividendYield { get; set; }

        [JsonProperty("EPS")]
        public string EPS { get; set; }

        [JsonProperty("RevenuePerShareTTM")]
        public string RevenuePerShareTTM { get; set; }

        [JsonProperty("ProfitMargin")]
        public string ProfitMargin { get; set; }

        [JsonProperty("OperatingMarginTTM")]
        public string OperatingMarginTTM { get; set; }

        [JsonProperty("ReturnOnAssetsTTM")]
        public string ReturnOnAssetsTTM { get; set; }

        [JsonProperty("ReturnOnEquityTTM")]
        public string ReturnOnEquityTTM { get; set; }

        [JsonProperty("RevenueTTM")]
        public string RevenueTTM { get; set; }

        [JsonProperty("GrossProfitTTM")]
        public string GrossProfitTTM { get; set; }

        [JsonProperty("DilutedEPSTTM")]
        public string DilutedEPSTTM { get; set; }

        [JsonProperty("QuarterlyEarningsGrowthYOY")]
        public string QuarterlyEarningsGrowthYOY { get; set; }

        [JsonProperty("QuarterlyRevenueGrowthYOY")]
        public string QuarterlyRevenueGrowthYOY { get; set; }

        [JsonProperty("AnalystTargetPrice")]
        public string AnalystTargetPrice { get; set; }

        [JsonProperty("TrailingPE")]
        public string TrailingPE { get; set; }

        [JsonProperty("ForwardPE")]
        public string ForwardPE { get; set; }

        [JsonProperty("PriceToSalesRatioTTM")]
        public string PriceToSalesRatioTTM { get; set; }

        [JsonProperty("PriceToBookRatio")]
        public string PriceToBookRatio { get; set; }

        [JsonProperty("EVToRevenue")]
        public string EVToRevenue { get; set; }

        [JsonProperty("EVToEBITDA")]
        public string EVToEBITDA { get; set; }

        [JsonProperty("Beta")]
        public string Beta { get; set; }

        [JsonProperty("52WeekHigh")]
        public string _52WeekHigh { get; set; }

        [JsonProperty("52WeekLow")]
        public string _52WeekLow { get; set; }

        [JsonProperty("50DayMovingAverage")]
        public string _50DayMovingAverage { get; set; }

        [JsonProperty("200DayMovingAverage")]
        public string _200DayMovingAverage { get; set; }

        [JsonProperty("SharesOutstanding")]
        public string SharesOutstanding { get; set; }

        [JsonProperty("SharesFloat")]
        public string SharesFloat { get; set; }

        [JsonProperty("SharesShort")]
        public string SharesShort { get; set; }

        [JsonProperty("SharesShortPriorMonth")]
        public string SharesShortPriorMonth { get; set; }

        [JsonProperty("ShortRatio")]
        public string ShortRatio { get; set; }

        [JsonProperty("ShortPercentOutstanding")]
        public string ShortPercentOutstanding { get; set; }

        [JsonProperty("ShortPercentFloat")]
        public string ShortPercentFloat { get; set; }

        [JsonProperty("PercentInsiders")]
        public string PercentInsiders { get; set; }

        [JsonProperty("PercentInstitutions")]
        public string PercentInstitutions { get; set; }

        [JsonProperty("ForwardAnnualDividendRate")]
        public string ForwardAnnualDividendRate { get; set; }

        [JsonProperty("ForwardAnnualDividendYield")]
        public string ForwardAnnualDividendYield { get; set; }

        [JsonProperty("PayoutRatio")]
        public string PayoutRatio { get; set; }

        [JsonProperty("DividendDate")]
        public string DividendDate { get; set; }

        [JsonProperty("ExDividendDate")]
        public string ExDividendDate { get; set; }

        [JsonProperty("LastSplitFactor")]
        public string LastSplitFactor { get; set; }

        [JsonProperty("LastSplitDate")]
        public string LastSplitDate { get; set; }
    }


    public class AlphaVantageSymbolSearchResult
    {
        [JsonProperty(PropertyName = "bestMatches")]
        public List<AlphaVantageSymbolInfo> BestMatches { get; set; } = new List<AlphaVantageSymbolInfo>();
    }

    public class AlphaVantageSymbolInfo
    {
        [JsonProperty(PropertyName = "1. symbol")]
        public string Symbol { get; set; }

        [JsonProperty(PropertyName = "2. name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "3. type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "4. region")]
        public string Region { get; set; }

        [JsonProperty(PropertyName = "5. marketOpen")]
        public string MarketOpen { get; set; }

        [JsonProperty(PropertyName = "6. marketClose")]
        public string MarketClose { get; set; }

        [JsonProperty(PropertyName = "7. timezone")]
        public string Timezone { get; set; }

        [JsonProperty(PropertyName = "8. currency")]
        public string Currency { get; set; }

        [JsonProperty(PropertyName = "9. matchScore")]
        public double MatchScore { get; set; }
    }

    public class AlphaVantageTimeSeriesIntraday60Min
    {
        [JsonProperty(PropertyName = "Meta Data")]
        public AlphaVantageTimeSeriesIntradayMetaData MetaData { get; set; }

        [JsonProperty(PropertyName = "Time Series (60min)")]
        public Dictionary<DateTime, AlphaVantageTimeSeriesIntradayValue> Series { get; set; } = new Dictionary<DateTime, AlphaVantageTimeSeriesIntradayValue>();
    }

    public class AlphaVantageTimeSeriesIntradayMetaData
    {
        [JsonProperty(PropertyName = "1. Information")]
        public string Information { get; set; }

        [JsonProperty(PropertyName = "2. Symbol")]
        public string Symbol { get; set; }

        [JsonProperty(PropertyName = "3. Last Refreshed")]
        public DateTime LastRefreshed { get; set; }

        [JsonProperty(PropertyName = "4. Interval")]
        public string Interval { get; set; }

        [JsonProperty(PropertyName = "5. Output Size")]
        public string OutputSize { get; set; }

        [JsonProperty(PropertyName = "6. Time Zone")]
        public string TimeZone { get; set; }
    }

    public class AlphaVantageTimeSeriesIntradayValue
    {
        [JsonProperty(PropertyName = "1. open")]
        public double Open { get; set; }

        [JsonProperty(PropertyName = "2. high")]
        public double High { get; set; }

        [JsonProperty(PropertyName = "3. low")]
        public double Low { get; set; }

        [JsonProperty(PropertyName = "4. close")]
        public double Close { get; set; }

        [JsonProperty(PropertyName = "5. volume")]
        public long Volume { get; set; }

    }
}
