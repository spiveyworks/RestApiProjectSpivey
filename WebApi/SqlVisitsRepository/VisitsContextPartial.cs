using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace SqlVisitsRepository
{
    public partial class VisitsContext
    {
        public VisitsContext(DbContextOptions options):base(options)
        {
            
        }
    }
}
