﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Visits
{
    public class VisitRepresentation
    {
        public string City { get; set; }
        public string State { get; set; }
        public string User { get; set; }
        public DateTime Created { get; set; }
    }
}