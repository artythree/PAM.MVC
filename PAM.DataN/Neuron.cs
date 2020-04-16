using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PAM.DataN
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

        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(NeuronId))]
        public virtual Neuron Neuron { get; set; }
        public int? NeuronId { get; set; }

        [ForeignKey(nameof(NeuronLayerOneId))]
        public virtual NeuronLayerOne NeuronLayerOne { get; set; }
        public int? NeuronLayerOneId { get; set; }
        public double Weight { get; set; }
    }
    public class WeightJoiningTableLayerTwo
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(NeuronLayerTwoId))]
        public virtual Neuron NeuronLayerTwo { get; set; }
        public int? NeuronLayerTwoId { get; set; }

        [ForeignKey(nameof(NeuronLayerOneId))]
        public virtual NeuronLayerOne NeuronLayerOne { get; set; }
        public int? NeuronLayerOneId { get; set; }
        public double Weight { get; set; }
    }
}
