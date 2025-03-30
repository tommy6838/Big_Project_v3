namespace Big_Project_v3.Models
{
    public static class DistrictData
    {
        // 返回台中市29區的資料
        public static List<District> GetTaichungDistricts()
        {
            return new List<District>
            {
                new District { Name = "中區", MinLat = 24.1368, MaxLat = 24.1445, MinLng = 120.6760, MaxLng = 120.6879 },
                new District { Name = "東區", MinLat = 24.1338, MaxLat = 24.1595, MinLng = 120.6830, MaxLng = 120.7050 },
                new District { Name = "南區", MinLat = 24.1110, MaxLat = 24.1330, MinLng = 120.6490, MaxLng = 120.6810 },
                new District { Name = "西區", MinLat = 24.1335, MaxLat = 24.1512, MinLng = 120.6475, MaxLng = 120.6735 },
                new District { Name = "北區", MinLat = 24.1510, MaxLat = 24.1725, MinLng = 120.6605, MaxLng = 120.6875 },
                new District { Name = "西屯區", MinLat = 24.1320, MaxLat = 24.2060, MinLng = 120.5820, MaxLng = 120.6430 },
                new District { Name = "北屯區", MinLat = 24.1670, MaxLat = 24.2190, MinLng = 120.6530, MaxLng = 120.7400 },
                new District { Name = "南屯區", MinLat = 24.1120, MaxLat = 24.1605, MinLng = 120.5940, MaxLng = 120.6600 },
                new District { Name = "太平區", MinLat = 24.0990, MaxLat = 24.1980, MinLng = 120.7200, MaxLng = 120.7600 },
                new District { Name = "大里區", MinLat = 24.1000, MaxLat = 24.1400, MinLng = 120.6800, MaxLng = 120.7200 },
                new District { Name = "霧峰區", MinLat = 24.0350, MaxLat = 24.1050, MinLng = 120.6700, MaxLng = 120.7300 },
                new District { Name = "烏日區", MinLat = 24.0900, MaxLat = 24.1200, MinLng = 120.6200, MaxLng = 120.6700 },
                new District { Name = "豐原區", MinLat = 24.2500, MaxLat = 24.2900, MinLng = 120.6900, MaxLng = 120.7600 },
                new District { Name = "后里區", MinLat = 24.2900, MaxLat = 24.3500, MinLng = 120.7100, MaxLng = 120.7900 },
                new District { Name = "石岡區", MinLat = 24.2600, MaxLat = 24.3000, MinLng = 120.7900, MaxLng = 120.8300 },
                new District { Name = "東勢區", MinLat = 24.2600, MaxLat = 24.3500, MinLng = 120.8200, MaxLng = 120.9200 },
                new District { Name = "和平區", MinLat = 24.2200, MaxLat = 24.5000, MinLng = 120.9400, MaxLng = 121.1600 },
                new District { Name = "新社區", MinLat = 24.1900, MaxLat = 24.2700, MinLng = 120.8200, MaxLng = 120.9200 },
                new District { Name = "潭子區", MinLat = 24.2000, MaxLat = 24.2500, MinLng = 120.7000, MaxLng = 120.7400 },
                new District { Name = "大雅區", MinLat = 24.2100, MaxLat = 24.2600, MinLng = 120.6400, MaxLng = 120.7000 },
                new District { Name = "神岡區", MinLat = 24.2500, MaxLat = 24.2900, MinLng = 120.6700, MaxLng = 120.7100 },
                new District { Name = "大肚區", MinLat = 24.1200, MaxLat = 24.1800, MinLng = 120.5200, MaxLng = 120.6200 },
                new District { Name = "沙鹿區", MinLat = 24.2000, MaxLat = 24.2600, MinLng = 120.5300, MaxLng = 120.6100 },
                new District { Name = "龍井區", MinLat = 24.1800, MaxLat = 24.2200, MinLng = 120.4800, MaxLng = 120.5600 },
                new District { Name = "梧棲區", MinLat = 24.2500, MaxLat = 24.2700, MinLng = 120.5000, MaxLng = 120.5700 },
                new District { Name = "清水區", MinLat = 24.2400, MaxLat = 24.2800, MinLng = 120.5200, MaxLng = 120.5900 },
                new District { Name = "大甲區", MinLat = 24.3400, MaxLat = 24.3800, MinLng = 120.6100, MaxLng = 120.6700 },
                new District { Name = "外埔區", MinLat = 24.3200, MaxLat = 24.3500, MinLng = 120.6400, MaxLng = 120.7000 },
                new District { Name = "大安區", MinLat = 24.3600, MaxLat = 24.4000, MinLng = 120.5800, MaxLng = 120.6400 }
            };
        }
    }

    // 區域類別
    public class District
    {
        public string Name { get; set; }
        public double MinLat { get; set; }
        public double MaxLat { get; set; }
        public double MinLng { get; set; }
        public double MaxLng { get; set; }
    }
}
