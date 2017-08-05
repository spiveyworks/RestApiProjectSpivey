using System;
using System.Threading.Tasks;

namespace VisitsRepository
{
    public interface IVisitsRepository
    {
        Task SaveVisit(Visit visit);
        Task<Visit> GetVisit(string visitId);
        Task DeleteVisit(string visitId);
        Task<Visit> GetVisitByStateId(short stateId);
    }
}
