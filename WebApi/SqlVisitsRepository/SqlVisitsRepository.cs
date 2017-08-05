using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisitsRepository;

namespace SqlVisitsRepository
{
    //The purpose of this is to show how it could be a SQL implementation or a new project could be created
    //for MongoDB or AWS DynamoDB and this repository implementation could be maintained or deprecated in
    //the future. But the web project won't be affected by choosing a different persistence technology. The
    //VisitId is represented in the generic repository interface entities as a string, but in the SQL
    //implementation it is being saved as a GUID. Normally you would want these data types to align, but
    //there might be a reason for having a difference between the persistence implementation and the
    //type shared in the generic repository interfaces. However, this can also cause the bad effect of
    //implementation bleed-thru, where the wider type of string can allow more options than are allowed
    //in the SQL implementation's GUID and now that lower level dependency is causing problems.

    /// <summary>
    /// Persists visits to a SQL database
    /// </summary>
    public class SqlVisitsRepository : IVisitsRepository
    {
        private DbContextOptions<VisitsContext> _options = null;
        private string _connectionString { get; set; }
        private DbContextOptions<VisitsContext> Options
        {
            get
            {
                if (this._options == null)
                {
                    var optionsBuilder = new DbContextOptionsBuilder<VisitsContext>();
                    this._options = optionsBuilder.UseSqlServer(this._connectionString).Options;
                }

                return this._options;
            }
        }

        public SqlVisitsRepository(string connectionString)
        {
            this._connectionString = connectionString;
        }
        
        public async Task DeleteVisit(int userId, string visitId)
        {
            var visitIdGuid = new Guid(visitId);

            try
            {
                using (var db = new VisitsContext(this.Options))
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
                using (var db = new VisitsContext(this.Options))
                    visitEntity = db.UserVisits.Where(v => v.VisitId == vid).FirstOrDefault();
            }
            catch (Exception exc)
            {
                throw new RepositoryException("Problem getting a visit.", exc);
            }

            Visit result = null;

            //Convert the entity framework entity (or whatever persistence technology specific entity) to the
            //generic repository entity.
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
                using (var db = new VisitsContext(this.Options))
                    visits = db.UserVisits.Where(v => v.UserId == userId).OrderBy(v => v.Created).Skip(skip).Take(take).ToArray();
            }
            catch (Exception exc)
            {
                throw new RepositoryException("Problem getting visits by userId.", exc);
            }
            
            var results = new List<Visit>();

            foreach (var v in visits)
            {
                //Convert the entity framework entity (or whatever persistence technology specific entity) to the
                //generic repository entity.
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
            //Convert the generic repository entity to the persistence specific entity.
            var visitEntity = new UserVisits()
            {
                CityId = visit.CityId,
                Created = visit.Created,
                StateId = (byte)visit.StateId, //This is a type conversion, which would ideally be made the same type all the way through if it can be helped.
                UserId = visit.User,
                VisitId = new Guid(visit.VisitId)
            };

            using (var db = new VisitsContext(this.Options))
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

                using (var db = new VisitsContext(this.Options))
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
