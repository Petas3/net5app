using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace net5app
{
    internal class WeighAverageMetric : IMetric
    {
        private readonly Collection<DataSubject> Subjects;
        private readonly double TotalWeigh;

        /// <summary>
        /// Initialize this metric with subjects
        /// </summary>
        /// <param name="Subjects">List of subjects with weighs</param>
        internal WeighAverageMetric(Collection<DataSubject> Subjects)
        {
            if (Subjects == null)
                throw new ArgumentNullException(nameof(Subjects));

            TotalWeigh = 0;
            foreach (DataSubject ds in Subjects)
                TotalWeigh += ds.Weight;
            
            this.Subjects = Subjects; 
        }

        public string GetName()
        {
            return "WeighAverageMetric";
        }

        /// <summary>
        /// This method accepts validated data with guaranteed subject values!
        /// </summary>
        /// <param name="subjectValues">Verified data object</param>
        /// <returns></returns>
        public byte GetMetricValue(Dictionary<string, byte> subjectValues)
        {
            double rt = 0;

            foreach (DataSubject ds in Subjects)
                rt += subjectValues[ds.Name] * ds.Weight;
            rt /= TotalWeigh;

            return (byte)Math.Round(rt);
        }

        public byte GetMinValue() { return 0; }
        public byte GetMaxValue() { return 100; }
    }
}