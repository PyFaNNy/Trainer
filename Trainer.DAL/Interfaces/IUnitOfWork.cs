using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trainer.DAL.Entities;

namespace Trainer.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Patient> Patients { get; }
        IRepository<Examination> Examinations { get; }
        IRepository<Results> Results { get; }
        Task<IEnumerable<Results>> GetPatientResults(Guid id);
    }
}
