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
    public class ResultsRepository : IRepository<Results>
    {
        private readonly TrainerContext Db;

        public ResultsRepository(TrainerContext db)
        {
            Db = db;
        }

        public async Task<IEnumerable<Results>> GetAll()
        {
            return Db.Results;
        }

        public async Task<Results> Get(Guid id)
        {
            return await Db.Results.Include(x=>x.Examination).Include(x=>x.Patient).Where(x=>x.Id==id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Results>> GetPatientResults(Guid id)
        {
            return await Db.Results.Include(x => x.Examination).Include(x => x.Patient).Where(x => x.PatientId == id).OrderByDescending(x=>x.Examination.Date).ToListAsync();
        }

        public async Task<Results> Create(Results results)
        {
            results.Patient = Db.Patients.Find(results.PatientId);
            results.Examination = Db.Examinations.Find(results.ExaminationId);
            await Db.Results.AddAsync(results);
            await Db.SaveChangesAsync();
            return results;
        }

        public async Task<Results> Update(Results patient)
        {
            Db.Results.Update(patient);
            await Db.SaveChangesAsync();
            return patient;
        }

        public async Task Delete(Guid id)
        {
            Results results = Db.Results.Find(id);
            if (results != null)
            {
                Db.Results.Remove(results);
            }
            await Db.SaveChangesAsync();
        }

        public Task<IEnumerable<Results>> Range(IEnumerable<Results> list)
        {
            throw new NotImplementedException();
        }
    } 
}
