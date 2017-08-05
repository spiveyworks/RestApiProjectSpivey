using System;
using System.Linq;
using System.Threading.Tasks;
using VisitsRepository;

namespace SqlVisitsRepository
{
    public class SqlVisitsRepository : IVisitsRepository
    {
        private string _connectionString { get; set; }

        public SqlVisitsRepository(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public Task DeleteVisit(string visitId)
        {
            throw new NotImplementedException();
        }

        public async Task<Visit> GetVisit(string visitId)
        {
            var vid = new Guid(visitId);
            var db = new VisitsContext();
            var rv = db.UserVisits.Where(v => v.VisitId == vid).FirstOrDefault();
            Visit result = null;

            result = new Visit()
            {
                CityId = rv.CityId,
                Created = rv.Created,
                StateId = rv.StateId,
                User = rv.UserId.ToString()
            };

            return result;
        }

        public Task<Visit> GetVisitByStateId(short stateId)
        {
            throw new NotImplementedException();
        }

        public Task SaveVisit(Visit visit)
        {
            throw new NotImplementedException();
        }
    }
}
