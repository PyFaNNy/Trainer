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
            return await Db.Examinations.Include(x => x.Patient).Where(x=> x.Id==id).FirstOrDefaultAsync();
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

        public async Task Delete(Guid id)
        {
            Examination examination = Db.Examinations.Find(id);
            if (examination != null)
            {
                Db.Examinations.Remove(examination);
            }
            await Db.SaveChangesAsync();
        }
    }
}
