using System.Collections;
using System.Linq;
using System.Collections.Immutable;
using System.Text;
using System.Runtime.Serialization.Json;
using System;
using System.Text.Json.Serialization;

namespace NyaapiDotnet.Si.Models
{
    public class SiRequestParams
    {

        public string Filters { get; set; }

        [JsonPropertyName("c")]
        public ArrayList Categories { get; set; }
        [JsonPropertyName("q")]
        public string Query { get; set; }
        public int? Page { get; set; }
        public long? Limit { get; set; }
        [JsonPropertyName("userID")]
        public string UserID { get; set; }
        [JsonPropertyName("fromID")]
        public string FromID { get; set; }
        [JsonPropertyName("s")]
        public string Status { get; set; }
        [JsonPropertyName("maxage")]
        public string MaxAge { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string DateType { get; set; }
        public string MinSize { get; set; }
        public string MaxSize { get; set; }
        public string SizeType { get; set; }
        public string Sort { get; set; }
        public string Order { get; set; }
        public string[] Languages { get; set; }

        public string buildQueryParams()
        {
            ArrayList queryParams = new ArrayList();
            if(!string.IsNullOrEmpty(Filters))
            {
                queryParams.Add($"f={Filters}");
            }
            if (Categories != null && Categories.Count != 0)
            {
                var cat = String.Concat("c=", String.Join(",", Categories.ToArray()));
                queryParams.Add(cat);
            }
            if (!string.IsNullOrEmpty(Query))
            {
                queryParams.Add($"q={Query}");
            }
            if (!string.IsNullOrEmpty(Sort))
            {
                queryParams.Add($"s={Sort}");
            } else
            {
                queryParams.Add("s=id");
            }
            if (Page != null && Page != 0)
            {
                queryParams.Add($"p={Page}");
            }
            if (!string.IsNullOrEmpty(Order))
            {
                queryParams.Add($"o={Order}");
            } else
            {
                queryParams.Add("o=desc");
            }

            return string.Join("&", queryParams.ToArray());
        }
    }
}