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

        public async Task DeleteVisit(int userId, string visitId)
        {
            var visitIdGuid = new Guid(visitId);

            try
            {
                using (var db = new VisitsContext())
                {
                    var visitEntity = db.UserVisits.Where(v => v.VisitId == visitIdGuid && v.UserId == userId).FirstOrDefault();

                    if (visitEntity != null)
                    {
                        db.UserVisits.Remove(visitEntity);
                        await db.SaveChangesAsync();
                    }
                }
            }
            catch (Exception exc)
            {
                throw new RepositoryException("Problem deleting a visit.", exc);
            }
        }

        public async Task<Visit> GetVisit(string visitId)
        {
            var vid = new Guid(visitId);
            UserVisits visitEntity = null;

            try
            {
                using (var db = new VisitsContext())
                    visitEntity = db.UserVisits.Where(v => v.VisitId == vid).FirstOrDefault();
            }
            catch (Exception exc)
            {
                throw new RepositoryException("Problem getting a visit.", exc);
            }

            Visit result = null;

            if (visitEntity != null)
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
            UserVisits[] visits = null;

            try
            {
                using (var db = new VisitsContext())
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
            var visitEntity = new UserVisits()
            {
                CityId = visit.CityId,
                Created = visit.Created,
                StateId = (byte)visit.StateId,
                UserId = visit.User,
                VisitId = new Guid(visit.VisitId)
            };

            using (var db = new VisitsContext())
            {
                try
                {
                    db.UserVisits.Add(visitEntity);
                    await db.SaveChangesAsync();
                }
                catch (Exception exc)
                {
                    throw new RepositoryException("Problem saving a visit.", exc);
                }
            }
        }

        public async Task<IEnumerable<short>> GetVisitsDistinctStateIds(int userId)
        {
            short[] stateIds = null;

            try
            {
                byte[] byteStateIds = null;

                using (var db = new VisitsContext())
                    byteStateIds = db.UserVisits.Where(v => v.UserId == userId).Select(v => v.StateId).Distinct().ToArray();

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
