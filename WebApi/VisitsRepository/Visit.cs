using System;
using System.Collections.Generic;
using System.Text;

namespace VisitsRepository
{
    public class Visit
    {
        public int CityId { get; set; }
        public short StateId { get; set; }
        public int User { get; set; }
        public DateTime Created { get; set; }
    }
}
