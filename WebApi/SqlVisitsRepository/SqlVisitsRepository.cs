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
            UserVisits visitEntity = null;

            try
            {
                visitEntity = db.UserVisits.Where(v => v.VisitId == vid).FirstOrDefault();
            }
            catch (Exception exc)
            {
                throw new RepositoryException("Problem getting a visit.", exc);
            }

            Visit result = null;

            result = new Visit()
            {
                CityId = visitEntity.CityId,
                Created = visitEntity.Created,
                StateId = visitEntity.StateId,
                User = visitEntity.UserId,
                VisitId = visitEntity.VisitId.ToString()
            };

            return result;
        }

        public async Task<IEnumerable<Visit>> GetVisitsByUserId(int userId, int skip, int take)
        {
            var db = new VisitsContext();
            UserVisits[] visits = null;

            try
            {
                visits = db.UserVisits.Where(v => v.UserId == userId).OrderBy(v => v.Created).Skip(skip).Take(take).ToArray();
            }
            catch (Exception exc)
            {
                throw new RepositoryException("Problem getting visits by userId.", exc);
            }
            
            var results = new List<Visit>();

            foreach (var v in visits)
            {
                results.Add(new Visit()
                {
                    CityId = v.CityId,
                    Created = v.Created,
                    StateId = v.StateId,
                    User = v.UserId,
                    VisitId = v.VisitId.ToString()
                });
            }

            return results;
        }

        public async Task SaveVisit(Visit visit)
        {
            var db = new VisitsContext();
            var visitEntity = new UserVisits()
            {
                CityId = visit.CityId,
                Created = visit.Created,
                StateId = (byte)visit.StateId,
                UserId = visit.User,
                VisitId = new Guid(visit.VisitId)
            };

            try
            {
                db.UserVisits.Add(visitEntity);
            }
            catch (Exception exc)
            {
                throw new RepositoryException("Problem saving a visit.", exc);
            }

            await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<short>> GetVisitsDistinctStateIds(int userId)
        {
            var db = new VisitsContext();
            short[] stateIds = null;

            try
            {
                var byteStateIds = db.UserVisits.Where(v => v.UserId == userId).Select(v => v.StateId).Distinct().ToArray();
                stateIds = byteStateIds.Select(s => (short)s).ToArray();
            }
            catch (Exception exc)
            {
                throw new RepositoryException("Problem getting distinct stateIds for a user.", exc);
            }

            return stateIds;
        }
    }
}
