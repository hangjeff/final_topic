using Microsoft.AspNetCore.Mvc;

namespace DapperNews.NAH
{
    public class Newbie 
    {
            public int? slno { get; set; }
            public string Header { get; set; }
            public string News { get; set; }
            public string Url { get; set; }
            public string Date { get; set; }
            public string Description { get; set; }
            public int? NewsID { get; set; }
    }
}
