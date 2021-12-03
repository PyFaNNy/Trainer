using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trainer.DAL.EF;
using Trainer.DAL.Entities;
using Trainer.DAL.Interfaces;

namespace Trainer.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private TrainerContext Db;
        private PatientRepository PatientRepository;
        private ExaminationRepository ExaminationRepository;
        private ResultsRepository ResultsRepository;


        public EFUnitOfWork(TrainerContext db)
        {
            Db = db;
        }
        public IRepository<Patient> Patients
        {
            get
            {
                if (PatientRepository == null)
                    PatientRepository = new PatientRepository(Db);
                return PatientRepository;
            }
        }

        public IRepository<Examination> Examinations
        {
            get
            {
                if (ExaminationRepository == null)
                    ExaminationRepository = new ExaminationRepository(Db);
                return ExaminationRepository;
            }
        }

        public IRepository<Results> Results
        {
            get
            {
                if (ResultsRepository == null)
                    ResultsRepository = new ResultsRepository(Db);
                return ResultsRepository;
            }
        }

        public async Task<IEnumerable<Results>> GetPatientResults(Guid id)
        {
            return await ((ResultsRepository)Results).GetPatientResults(id);
        }
    }
}
