using System;
using System.Collections.Generic;
using System.Text;

namespace DogIdentification.Models
{
   public class PredictionResult
    {
        public List<Classification> Predictions { get; set; }
    }

    public class Classification
    {
        public String TagName { get; set; }
        public float Probability { get; set; }

        public Classification(string tagName, float probability)
        {
            TagName = tagName;
            Probability = probability;
        }
    }
}
