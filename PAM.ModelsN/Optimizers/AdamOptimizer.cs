
using System;
using System.Collections.Generic;
using System.Text; 
using PAM.DataN;

namespace PAM.ModelsN.Optimizers
{
    public class Adam
    {
        ApplicationDbContext context = new ApplicationDbContext();
        Random rand = new Random();
        public double Alpha { get; set; }
        public double BetaOne { get; set; }
        public double BetaTwo { get; set; }
        public decimal Epsilon { get; set; }
        public double Gradient { get; set; }



        public Adam(double alpha, double betaOne, double betaTwo, decimal epsilon, double gradient)
        {
            Alpha = alpha;
            BetaOne = betaOne;
            BetaTwo = betaTwo;
            Epsilon = epsilon;
            Gradient = gradient;
        }
        public Adam()
        {

        }
        public bool WeightAdjustor(Perceptron perceptron)
        {
            Adam adam = new Adam(.01, .9, .99, .00000000001m, rand.NextDouble());

            double m = 0;
            double v = 0;
            double m_hat = 0;
            double v_hat = 0;
            double MSE = 0;
            int counterMain = 10;
            int counterTotal = 0;

            
            var marketData = (ICollection<MarketData>)context.MarketDataPoints;
            List<MarketData> marketDataOne = (List<MarketData>)marketData;
            var marketDataMain = marketDataOne.FindAll(ticker => ticker.Ticker == perceptron.Stock);



            while (counterMain > 0)
            {
                int counterInternalOne = 0;
                DateTime date = DateTime.Today.AddYears(-18);
                while (date != DateTime.Today)
                {

                    int counterInternalTwo = counterInternalOne;
                    foreach (NeuronLayerTwo nltwo in perceptron.NLTwos)
                    {
                        nltwo.PercentChange = marketDataMain[counterInternalTwo].PercentChange;
                        counterInternalTwo += 1;
                    }
                   
                    foreach (NeuronLayerOne nlone in perceptron.NLOnes)
                    {
                        double holder = 0;
                        foreach (NeuronLayerTwo nltwo in perceptron.NLTwos)
                        {
                            holder += (nltwo.PercentChange * nlone.WeightsLayerTwo[nltwo.NeuronLayerTwoId].Weight);
                        }
                        nlone.PercentChange = holder / 365;
                    }
                    foreach (Neuron neuron in perceptron.Neurons)
                    {
                        double holder = 0;
                        foreach (NeuronLayerOne nlone in perceptron.NLOnes)
                        {
                            holder += (nlone.PercentChange * neuron.WeightsLayerOne[nlone.NeuronLayerOneId].Weight);
                        }
                        neuron.PercentChange = holder / 365;
                    }
                    double outputHolder = 0;

                    foreach (Neuron neuron1 in perceptron.Neurons)
                    {
                        outputHolder += (neuron1.Weight * neuron1.PercentChange);
                    }
                    double actualChange = marketDataMain[counterInternalOne].PercentChange;
                    counterInternalOne += 1;
                    perceptron.Output = (outputHolder / 365);
                    date = date.AddDays(1);
                    MSE = Math.Pow(actualChange - perceptron.Output , 2);
                    perceptron.Error = ((perceptron.Error * counterTotal) + MSE) / (counterTotal + 1);
                    adam.Gradient = (adam.Gradient - (adam.Alpha * perceptron.Error));
                    m = (adam.BetaOne * m) + ((1 - adam.BetaOne) * adam.Gradient);
                    v = (adam.BetaTwo * v) + ((1 - adam.BetaTwo) * Math.Pow(adam.Gradient, 2));
                    m_hat = m / (1 - Math.Pow(adam.BetaOne, counterTotal));
                    v_hat = v / (1 - Math.Pow(adam.BetaTwo, counterTotal));
                    foreach (NeuronLayerOne nlone in perceptron.NLOnes)
                    {
                        
                        foreach (NeuronLayerTwo nltwo in perceptron.NLTwos)
                        {
                            if (Math.Pow(actualChange - nltwo.PercentChange,2)>MSE)
                            {
                                double internalWeight = nlone.WeightsLayerTwo[nltwo.NeuronLayerTwoId].Weight;
                                internalWeight = internalWeight - adam.Alpha * m_hat / (Math.Sqrt(v_hat) + (double)adam.Epsilon);
                                nlone.WeightsLayerTwo[nltwo.NeuronLayerTwoId].Weight = internalWeight;
                            }
                            else
                            {
                                double internalWeight = nlone.WeightsLayerTwo[nltwo.NeuronLayerTwoId].Weight;
                                internalWeight = internalWeight + adam.Alpha * m_hat / (Math.Sqrt(v_hat) + (double)adam.Epsilon);
                                nlone.WeightsLayerTwo[nltwo.NeuronLayerTwoId].Weight = internalWeight;
                            }
                        }
                        
                    }
                    foreach (Neuron neuron in perceptron.Neurons)
                    {

                        foreach (NeuronLayerOne nlone in perceptron.NLOnes)
                        {
                            if (Math.Pow(actualChange - nlone.PercentChange, 2) > MSE)
                            {
                                double internalWeight = neuron.WeightsLayerOne[nlone.NeuronLayerOneId].Weight;
                                internalWeight = internalWeight - adam.Alpha * m_hat / (Math.Sqrt(v_hat) + (double)adam.Epsilon);
                                neuron.WeightsLayerOne[nlone.NeuronLayerOneId].Weight = internalWeight;
                            }
                            else
                            {
                                double internalWeight = neuron.WeightsLayerOne[nlone.NeuronLayerOneId].Weight;
                                internalWeight = internalWeight + adam.Alpha * m_hat / (Math.Sqrt(v_hat) + (double)adam.Epsilon);
                                neuron.WeightsLayerOne[nlone.NeuronLayerOneId].Weight = internalWeight;
                            }
                        }

                    }
                    foreach(Neuron neuron2 in perceptron.Neurons)
                    {
                        if (Math.Pow(actualChange - neuron2.PercentChange, 2) > MSE)
                        {
                            double internalWeight = neuron2.Weight;
                            internalWeight = internalWeight - adam.Alpha * m_hat / (Math.Sqrt(v_hat) + (double)adam.Epsilon);
                            neuron2.Weight = internalWeight;
                        }
                        else
                        {
                            double internalWeight = neuron2.Weight;
                            internalWeight = internalWeight + adam.Alpha * m_hat / (Math.Sqrt(v_hat) + (double)adam.Epsilon);
                            neuron2.Weight = internalWeight;
                        }
                    }
                    date.AddDays(1);
                }
                counterMain -= 1;
            }
            return true;
        }
    }
}
