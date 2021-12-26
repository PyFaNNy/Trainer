using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trainer.BLL.DTO;

namespace Trainer.BLL.Interfaces
{
    public interface ICsvParserService
    {
        Task<IEnumerable<PatientDTO>> ReadCsvFileToPatient(IFormFile source);
        Task<IEnumerable<ExaminationDTO>> ReadCsvFileToExamination(IFormFile source);
        Task<byte[]> WriteNewCsvFile(IEnumerable<PatientDTO> employeeModels);
        Task<byte[]> WriteNewCsvFile(IEnumerable<ExaminationDTO> employeeModels);
    }
}
