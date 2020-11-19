﻿using System.Runtime.Serialization;

namespace net5app
{
    [DataContract]
    public class DataSubject
    {
        [DataMember]
        public string Name;
        [DataMember]
        public double Weight;

        public DataSubject(string Name, double Weight)
        {
            this.Name = Name;
            this.Weight = Weight;
        }
    }
}