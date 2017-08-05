using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Geography
{
    public class StateCitiesRepresentation
    {
        public string State { get; set; }
        public string[] Cities { get; set; }
    }
}
