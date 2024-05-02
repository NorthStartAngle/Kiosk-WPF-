using System;
using System.Collections.Generic;

namespace AtmLib.Monitor
{
    public class Device : IComparable {
        public int No { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public List<KeyValuePair<string, object>> Props;
        public Boolean IsEnabled { get; set; }
        public string IconPath { get; set; }

        public Device() {
            Id = "";
            Name = "";
            Props = new List<KeyValuePair<string, object>>();
        }

        public int CompareTo(object obj) {
            if (obj == null || Name == null)
                return -1;

            return Name.CompareTo(((Device)obj).Name);
        }

        public bool Equals(Device other) {
            if (Id != other.Id)
                return false;
            if (Name != other.Name)
                return false;
            if (IsEnabled != other.IsEnabled)
                return false;
            if (Props.Count != other.Props.Count)
                return false;

            for (var i = 0; i < Props.Count; i++) {
                if (Props[i].Key != other.Props[i].Key || Props[i].Value.ToString() != other.Props[i].Value.ToString())
                    return false;
            }

            return true;
        }
    }
}
