using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trainer.DAL.EF;
using Trainer.DAL.Entities;
using Trainer.DAL.Interfaces;

namespace Trainer.DAL.Repositories
{
    public class ExaminationRepository : IRepository<Examination>
    {
        private TrainerContext Db;

        public ExaminationRepository(TrainerContext db)
        {
            Db = db;
        }

        public async Task<IEnumerable<Examination>> GetAll()
        {
            return Db.Examinations.Include(x => x.Patient);
        }

        public async Task<Examination> Get(Guid id)
        {
            return await Db.Examinations.FindAsync(id);
        }

        public async Task<Examination> Create(Examination examination)
        {
            examination.Patient = Db.Patients.Find(examination.PatientId);
            await Db.Examinations.AddAsync(examination);
            await Db.SaveChangesAsync();
            return examination;
        }

        public async Task<Examination> Update(Examination examination)
        {
            Db.Examinations.Update(examination);
            await Db.SaveChangesAsync();
            return examination;
        }

        public IEnumerable<Examination> Find(Func<Examination, Boolean> predicate)
        {
            return Db.Examinations.Where(predicate).ToList();
        }

        public async Task Delete(Guid id)
        {
            Examination patient = Db.Examinations.Find(id);
            if (patient != null)
            {
                Db.Examinations.Remove(patient);
            }
            await Db.SaveChangesAsync();
        }
    }
}
