using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PAM.Data
{
    
    public class Neuron
    {
       [Key]
        public int NeuronId { get; set; }
        public int PerceptronId { get; set; }
        [ForeignKey(nameof(PerceptronId))]
        public virtual Perceptron Perceptron { get; set; }
        public double Weight { get; set; }
        public double PercentChange { get; set; }
        public virtual List<WeightJoiningTableLayerOne> WeightsLayerOne { get; set; } = new List<WeightJoiningTableLayerOne>();
    }
    public class NeuronLayerOne
    {
        [Key]
        public int NeuronLayerOneId { get; set; }
        [ForeignKey(nameof(Perceptron))]
        public int PerceptronId { get; set; }
        public virtual Perceptron Perceptron { get; set; }
        public double PercentChange { get; set; }
        public virtual List<WeightJoiningTableLayerOne> WeightsLayerOne { get; set; } = new List<WeightJoiningTableLayerOne>();
        public virtual List<WeightJoiningTableLayerTwo> WeightsLayerTwo { get; set; } = new List<WeightJoiningTableLayerTwo>();
    }
    public class NeuronLayerTwo
    {
        [Key]
        public int NeuronLayerTwoId { get; set; }
        [ForeignKey(nameof(Perceptron))]
        public int PerceptronId { get; set; }
        public virtual Perceptron Perceptron { get; set; }
        public virtual ICollection<WeightJoiningTableLayerTwo> WeightsLayerTwo { get; set; }
        public double PercentChange { get; set; }
    }
    public class WeightJoiningTableLayerOne
    {

        public virtual Neuron Neuron { get; set; }

        [ForeignKey(nameof(Neuron))]
        [Key, Column(Order = 0)]
        public int NeuronId { get; set; }

        public virtual NeuronLayerOne NeuronLayerOne { get; set; }

        [ForeignKey(nameof(NeuronLayerOne))]
        [Key, Column(Order =1)]

        public int NeuronLayerOneId { get; set; }
        public double Weight { get; set; }
    }
    public class WeightJoiningTableLayerTwo
    {
        public virtual Neuron NeuronLayerTwo { get; set; }

        [ForeignKey(nameof(NeuronLayerTwo))]
        [Key, Column(Order = 0)]

        public int NeuronLayerTwoId { get; set; }

        public virtual NeuronLayerOne NeuronLayerOne { get; set; }

        [ForeignKey(nameof(NeuronLayerOne))]
        [Key, Column(Order = 1)]

        public int NeuronLayerOneId { get; set; }
        public double Weight { get; set; }
    }
}
