using System;
using System.Collections.Generic;
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
                User = rv.UserId
            };

            return result;
        }

        public async Task<IEnumerable<Visit>> GetVisitsByUserId(int userId, int skip, int take)
        {
            var db = new VisitsContext();
            var visits = db.UserVisits.Where(v => v.UserId == userId).OrderBy(v => v.Created).Skip(skip).Take(take).ToArray();
            var results = new List<Visit>();

            foreach (var v in visits)
            {
                results.Add(new Visit()
                {
                    CityId = v.CityId,
                    Created = v.Created,
                    StateId = v.StateId,
                    User = v.UserId
                });
            }

            return results;
        }

        public async Task SaveVisit(Visit visit)
        {
            throw new NotImplementedException();
        }
    }
}
