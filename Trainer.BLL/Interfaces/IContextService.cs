using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trainer.BLL.DTO;
using Trainer.DAL.Util.Constant;

namespace Trainer.BLL.Interfaces
{
    public interface IContextService
    {
        Task<PatientDTO> GetPatient(Guid id);
        Task<IEnumerable<PatientDTO>> GetPatients(SortState sortOrder);
        Task<PatientDTO> Create(PatientDTO patientDTO);
        Task<PatientDTO> Update(PatientDTO patientDTO);
        Task DeletePatient(Guid id);

        Task<ExaminationDTO> GetExamination(Guid id);
        Task <IEnumerable<ExaminationDTO>> GetExaminations(SortState sortOrder);
        Task<ExaminationDTO> Create(ExaminationDTO examinationDTO);
        Task<ExaminationDTO> Update(ExaminationDTO examinationDTO);
        Task DeleteExamination(Guid id);
    }
}
