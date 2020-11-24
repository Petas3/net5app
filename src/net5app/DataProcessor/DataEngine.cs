using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net5app
{
    internal class DataEngine
    {
        private readonly DataStruct data;
        private readonly List<IMetric> metrics = new List<IMetric>();

        internal DataEngine(DataStruct data)
        {
            this.data = data;
        }

        internal void AddMetric(IMetric metric)
        {
            metrics.Add(metric);
        }

        /// <summary>
        /// Processes group stats
        /// </summary>
        internal void ProcessGroups()
        {
            Parallel.ForEach(data.Groups, (currentGroup) => { ProcessGroup(currentGroup); });
        }

        private void ProcessGroup(DataGroup dg)
        {
            //Compute metrics for each record
            foreach (DataRecord r in dg.Records)
                foreach (IMetric m in metrics)
                    r.MetricValues.Add(m.GetName(), m.GetMetricValue(r.SubjectValues));

            //Foreach subject get stats
            foreach (DataSubject ds in data.Subjects)
            {
                byte[] values = dg.Records.GetSubjectValues(ds.Name);
                dg.Statistics.Add(new DataStatistics(ds.Name, values, ds.MinAllowedValue, ds.MaxAllowedValue));
            }
            
            //Foreach metric get stats
            foreach (IMetric dm in metrics)
            {
                byte[] values = dg.Records.GetMetricValues(dm.GetName());
                dg.Statistics.Add(new DataStatistics(dm.GetName(), values, dm.GetMinValue(), dm.GetMaxValue()));
            }
        }

        /// <summary>
        /// Processes aggregate stats
        /// </summary>
        internal void ProcessAggregate()
        {
            if (data.Groups.Count == 1)
            {
                //Copy
                foreach (DataStatistics ds in data.Groups[0].Statistics)
                    data.Statistics.Add(ds);
            }
            else
            {
                //Easy way, can be improved by agregate processing
                //Average can be weighed
                //Modus arrays can be added
                //Median of medians shall be used as default, but whole array needs to be processed again

                //Foreach subject get stats
                foreach (DataSubject ds in data.Subjects)
                {
                    byte[] values = data.Groups.SelectMany(g => g.Records.GetSubjectValues(ds.Name)).ToArray();
                    data.Statistics.Add(new DataStatistics(ds.Name, values, ds.MinAllowedValue, ds.MaxAllowedValue));
                }

                //Foreach metric get stats
                foreach (IMetric dm in metrics)
                {
                    byte[] values = data.Groups.SelectMany(g => g.Records.GetMetricValues(dm.GetName())).ToArray();
                    data.Statistics.Add(new DataStatistics(dm.GetName(), values, dm.GetMinValue(), dm.GetMaxValue()));
                }
            }
        }
    }
}