using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlissBuddy
{
    public struct ExperienceMethod
    {
        public string Name;
        public string IterationText;
        public string BaseExpText;
        public Dictionary<string, float> ExperienceTable;

        public override string ToString()
        {
            return Name;
        }
    }
}
