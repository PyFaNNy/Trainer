using Trainer.DAL.Entities;

namespace Trainer.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Patient> Patients { get; }
        IRepository<Examination> Examinations { get; }
    }
}
