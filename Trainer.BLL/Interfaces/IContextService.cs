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
        Task<IEnumerable<PatientDTO>> Range(IEnumerable<PatientDTO> list);
        Task DeletePatient(Guid id);

        Task<ExaminationDTO> GetExamination(Guid id);
        Task <IEnumerable<ExaminationDTO>> GetExaminations(SortState sortOrder);
        Task<ExaminationDTO> Create(ExaminationDTO examinationDTO);
        Task<ExaminationDTO> Update(ExaminationDTO examinationDTO);
        Task<IEnumerable<ExaminationDTO>> Range(IEnumerable<ExaminationDTO> list);
        Task DeleteExamination(Guid id);

        Task<ResultsDTO> GetResult(Guid id);
        Task<IEnumerable<ResultsDTO>> GetResults();
        Task<IEnumerable<ResultsDTO>> GetPatientResults(Guid id);
        Task<ResultsDTO> Create(ResultsDTO examinationDTO);
        Task<ResultsDTO> Update(ResultsDTO examinationDTO);
        Task DeleteResults(Guid id);
    }
}
