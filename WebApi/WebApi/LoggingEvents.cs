using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi
{
    public static class LoggingEvents
    {
        public static EventId GET_USER_VISITS
        {
            get { return new EventId(1, "GET_USER_VISITS"); }
        }

        public static EventId GET_USER_VISITS_STATES
        {
            get { return new EventId(2, "GET_USER_VISITS_STATES"); }
        }

        public static EventId GET_USER_VISIT
        {
            get { return new EventId(3, "GET_USER_VISIT"); }
        }

        public static EventId POST_USER_VISIT
        {
            get { return new EventId(4, "POST_USER_VISIT"); }
        }

        public static EventId DELETE_USER_VISIT
        {
            get { return new EventId(5, "DELETE_USER_VISIT"); }
        }

        public static EventId POPULATING_CITY_CACHE
        {
            get { return new EventId(6, "POPULATING_CITY_CACHE"); }
        }

        public static EventId POPULATING_STATE_CACHE
        {
            get { return new EventId(7, "POPULATING_STATE_CACHE"); }
        }

        public static EventId GET_STATE_CITIES
        {
            get { return new EventId(8, "GET_STATE_CITIES"); }
        }
    }
}