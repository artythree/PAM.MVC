using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Text;

namespace PAM.DataN
{
    public class MarketData
    {
        
        [Key, Column(Order = 0)]
        public string Ticker { get; set; }
        public double PercentChange { get; set; }
        [Key, Column(Order = 1)]
        public DateTime Date { get; set; }
        

    }
}
