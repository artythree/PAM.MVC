using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PAM.DataN
{
    public class Perceptron
    {
        [Key]
        public int PerceptronId { get; set; }
        public virtual List<Neuron> Neurons { get; set; }
        public virtual List<NeuronLayerOne> NLOnes { get; set; }
        public virtual List<NeuronLayerTwo> NLTwos { get; set; }
        public string Stock { get; set; }
        public double Error { get; set; }
        public double Output { get; set; }
        public bool OutputMethod(DateTime date)
        {
            ICollection<MarketData> MarketDataPoints = (ICollection<MarketData>)context.MarketDataPoints;
            var z = (List<MarketData>)MarketDataPoints;
            z = z.FindAll(ticker => ticker.Ticker == Stock);
            int x = 0;
            double y = 0;
            while (x < Neurons.Count)
            {
                y += Neurons[x].Weight * z.Find(Data => Data.Date.Date == date.AddDays(-x).Date).PercentChange;
                x += 1;
            }
            if (y > .5)
            {
                return true;
            }
            return false;
        }
        ApplicationDbContext context = new ApplicationDbContext();

    }
    public class PerceptronListItem
    {
        public string Stock { get; set; }
        public double Error { get; set; }
        public double Output { get; set; }
    }

}
