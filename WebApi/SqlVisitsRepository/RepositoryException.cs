using System;
using System.Collections.Generic;
using System.Text;

namespace SqlVisitsRepository
{
    public class RepositoryException : Exception
    {
        public RepositoryException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
