using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VisitsRepository
{
    public interface IVisitsRepository
    {
        Task SaveVisit(Visit visit);
        Task<Visit> GetVisit(string visitId);
        Task DeleteVisit(string visitId);
        Task<IEnumerable<Visit>> GetVisitsByUserId(int userId, int skip, int take);
        Task<IEnumerable<short>> GetVisitsDistinctStateIds(int userId);
    }
}
