namespace FCFDFE.Content
{
    public class VariableMTS
    {
        public static string[] strTranserDepts = { "TTY", "TPE", "KHH", "TWKHH", "TWKEL" }; //接轉單位 所需Key值，僅包含基隆地區、桃園地區、高雄分遣組。
        public static string[] strContainerSizeText = { "20", "40", "45" };
        public static string[] strContainerSizeValue = { "20", "40", "45" };
        public static string strOtherText = "其他";

        public static string[] list_QUANITY_UNIT = { "EA", "FT", "GL", "HD", "PCE", "PKG", "PLT", "SET", "UNIT", strOtherText }; //件數單位
        public static string[] list_OVC_SHIP_COMPANY = { "中華航空", "長榮航空", "長榮海運", "陽明海運", "非合約航商" }; //承運航商
        public static string strDefaultCurrency = "USD"; //預設幣別
    }
}