using System;
using System.Collections.Generic;

namespace SqlVisitsRepository
{
    public partial class UserVisits
    {
        public Guid VisitId { get; set; }
        public int UserId { get; set; }
        public DateTime Created { get; set; }
        public int CityId { get; set; }
        public byte StateId { get; set; }
    }
}
