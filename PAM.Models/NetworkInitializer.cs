using AlphaVantage.Net.Stocks.TimeSeries;
using Newtonsoft.Json;
using PAM.Data;
using PAM.Models.Optimizers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks; 

namespace PAM.Models
{
    public class NetworkInitializer
    {
        APIConnection connection = new APIConnection();
        ApplicationDbContext context = new ApplicationDbContext();


        public async Task<string> GetMarketData(string ticker)
        {
            int counter = 0;
            bool dateIncluded = false;
            foreach (MarketData data in context.MarketDataPoints)
            {
                if (data.Ticker == ticker)
                {
                    counter += 1;
                }
                if (data.Date.Date == DateTime.Today.AddDays(-1))
                {
                    dateIncluded = true;
                }
            }
            if (counter == 0 && dateIncluded == false)
            {
                await connection.GetStocksAll(ticker);

                return "Added all stocks";
            }
            else if (counter != 0 && dateIncluded == false)
            {
                await connection.GetStocksToday(ticker);

                return "Added stock for today";
            }
            return "Value already in directory no change.";

        }
        public bool CreateNetwork(string ticker)
        {
            Random random = new Random();
            int perceptronId = -1;
            int neuronCounter = 0;
            foreach (Perceptron perceptron in context.Perceptrons)
            {
                if (perceptron.Stock == ticker)
                {
                    perceptronId = perceptron.PerceptronId;
                }
            }
            foreach (Neuron neuron in context.Neurons)
            {
                if (neuron.PerceptronId == perceptronId)
                {
                    neuronCounter += 1;
                }
            }
            if (perceptronId != -1 && neuronCounter == 0)
            {
                int x = 365;
                while (x > 0)
                {
                    Neuron neuron = new Neuron();
                    neuron.PerceptronId = perceptronId;
                    neuron.Weight = random.NextDouble();
                    context.Neurons.Add(neuron);
                    context.SaveChanges();


                    x -= 1;
                }
                x = 365;
                while (x > 0)
                {
                    NeuronLayerOne neuronLayerOne = new NeuronLayerOne();
                    neuronLayerOne.PerceptronId = perceptronId;
                    context.LayerOneNeurons.Add(neuronLayerOne);
                    context.SaveChanges();

                    x -= 1;
                }

                x = 365;
                while (x > 0)
                {
                    NeuronLayerTwo neuronLayerTwo = new NeuronLayerTwo();
                    neuronLayerTwo.PerceptronId = perceptronId;
                    context.LayerTwoNeurons.Add(neuronLayerTwo);
                    context.SaveChanges();

                    x -= 1;
                }
                foreach (Neuron neuron in context.Neurons)
                {
                    foreach (NeuronLayerOne neuronLayerOne in context.LayerOneNeurons)
                    {

                        WeightJoiningTableLayerOne weight = new WeightJoiningTableLayerOne();
                        weight.NeuronId = neuron.NeuronId;
                        weight.NeuronLayerOneId = neuronLayerOne.NeuronLayerOneId;
                        weight.Weight = random.NextDouble();
                        context.LayerOneWeights.Add(weight);
                        context.SaveChanges();

                    }
                }
                foreach (NeuronLayerOne neuronLayerOne in context.LayerOneNeurons)
                {
                    foreach (NeuronLayerTwo neuronLayerTwo in context.LayerTwoNeurons)
                    {

                        WeightJoiningTableLayerTwo weight = new WeightJoiningTableLayerTwo();
                        weight.NeuronLayerOneId = neuronLayerOne.NeuronLayerOneId;
                        weight.NeuronLayerTwoId = neuronLayerTwo.NeuronLayerTwoId;
                        weight.Weight = random.NextDouble();
                        context.LayerTwoWeights.Add(weight);
                        context.SaveChanges();

                    }
                }
                return true;
            }
            return false;
        }
        public bool TrainNetwork(string ticker)
        {
            
            Adam adam = new Adam();
            
            int perceptronId = -1;
            int neuronCounter = 0;

            foreach (Perceptron perceptron in context.Perceptrons)
            {
                if (perceptron.Stock == ticker)
                {
                    perceptronId = perceptron.PerceptronId;
                }
            }
            foreach (Neuron neuron in context.Neurons)
            {
                if (neuron.PerceptronId == perceptronId)
                {
                    neuronCounter += 1;
                }
            }
            if (perceptronId != -1 && neuronCounter == 365)
            {
                return adam.WeightAdjustor(context.Perceptrons.Find(perceptronId));
            }
            return false;
        }
       
    }
}
