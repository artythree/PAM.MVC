using AlphaVantage.Net.Stocks.TimeSeries;
using Newtonsoft.Json;
using PAM.DataN;
using PAM.ModelsN.Optimizers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PAM.ModelsN
{
    public class NetworkInitializer
    {
        APIConnection connection = new APIConnection();
        ApplicationDbContext context = new ApplicationDbContext();


        
        public Perceptron CreatePerceptron(string ticker)
        {
            int perceptronId = -1;
            foreach (Perceptron perceptron in context.Perceptrons)
            {
                if (perceptron.Stock == ticker)
                {
                    perceptronId = perceptron.PerceptronId;
                }
            }
            if (perceptronId == -1)
            {
                Perceptron perceptron = new Perceptron();
                perceptron.Stock = ticker;

                return perceptron;
            }
            return context.Perceptrons.Find(perceptronId);
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


                    x -= 1;
                }

                x = 365;
                while (x > 0)
                {
                    NeuronLayerOne neuronLayerOne = new NeuronLayerOne();
                    neuronLayerOne.PerceptronId = perceptronId;
                    context.LayerOneNeurons.Add(neuronLayerOne);

                    x -= 1;
                }


                x = 365;
                while (x > 0)
                {
                    NeuronLayerTwo neuronLayerTwo = new NeuronLayerTwo();
                    neuronLayerTwo.PerceptronId = perceptronId;
                    context.LayerTwoNeurons.Add(neuronLayerTwo);


                    x -= 1;
                }

                context.SaveChanges();
                return true;
            }
            return false;
        }
        public bool InitializeJoiningTables(string ticker)
        {
            List<WeightJoiningTableLayerOne> listOne = new List<WeightJoiningTableLayerOne>();
            List<WeightJoiningTableLayerTwo> listTwo = new List<WeightJoiningTableLayerTwo>();
            Random random = new Random();
            foreach (Neuron neuron in context.Neurons)
            {
                
                foreach (NeuronLayerOne neuronLayerOne in context.LayerOneNeurons)
                {

                    WeightJoiningTableLayerOne weight = new WeightJoiningTableLayerOne();
                    weight.NeuronId = neuron.NeuronId;
                    weight.NeuronLayerOneId = neuronLayerOne.NeuronLayerOneId;
                    weight.Weight = random.NextDouble();
                    listOne.Add(weight);

                    context.LayerOneWeights.AddRange(listOne);
                    listOne.Clear();
                }

            }
            context.SaveChanges();
            foreach (NeuronLayerOne neuronLayerOne in context.LayerOneNeurons)
            {
                
                foreach (NeuronLayerTwo neuronLayerTwo in context.LayerTwoNeurons)
                {

                    WeightJoiningTableLayerTwo weight = new WeightJoiningTableLayerTwo();
                    weight.NeuronLayerOneId = neuronLayerOne.NeuronLayerOneId;
                    weight.NeuronLayerTwoId = neuronLayerTwo.NeuronLayerTwoId;
                    weight.Weight = random.NextDouble();
                    listTwo.Add(weight);

                    context.LayerTwoWeights.AddRange(listTwo);
                    listTwo.Clear();
                }
            }
            context.SaveChanges();
            return true;
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
