using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trainer.DAL.EF;
using Trainer.DAL.Entities;
using Trainer.DAL.Interfaces;

namespace Trainer.DAL.Repositories
{
    public class PatientRepository : IRepository<Patient>
    {
        private readonly TrainerContext Db;

        public PatientRepository(TrainerContext db)
        {
            Db = db;
        }

        public async Task<IEnumerable<Patient>> GetAll()
        {
            return Db.Patients;
        }

        public async Task<Patient> Get(Guid id)
        {
            return await Db.Patients.FindAsync(id);
        }

        public async Task<Patient> Create(Patient patient)
        {
            await Db.Patients.AddAsync(patient);
            await Db.SaveChangesAsync();
            return patient;
        }

        public async Task<Patient> Update(Patient patient)
        {
            Db.Patients.Update(patient);
            await Db.SaveChangesAsync();
            return patient;
        }

        public async Task Delete(Guid id)
        {
            Patient patient = Db.Patients.Find(id);
            if (patient != null)
            {
                Db.Patients.Remove(patient);
            }
            await Db.SaveChangesAsync(); 
        }

        public async Task<IEnumerable<Patient>> Range(IEnumerable<Patient> list)
        {
            await Db.Patients.AddRangeAsync(list);
            await Db.SaveChangesAsync();
            return list;
        }
    }
}
