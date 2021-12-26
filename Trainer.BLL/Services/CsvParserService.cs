

using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Trainer.BLL.DTO;
using Trainer.BLL.Infrastructure;
using Trainer.BLL.Interfaces;

namespace Trainer.BLL.Services
{
    public class CsvParserService : ICsvParserService
    {
        public async Task<IEnumerable<ExaminationDTO>> ReadCsvFileToExamination(IFormFile source)
        {
            try
            {
                using (var reader = new StreamReader(source.OpenReadStream()))
                {
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        csv.Context.RegisterClassMap<ExaminationMap>();
                        var read = csv.Read();
                        var readHeader = csv.ReadHeader();
                        var records = csv.GetRecords<ExaminationDTO>().ToList();
                        return records;
                    }
                }
            }
            catch (UnauthorizedAccessException e)
            {
                throw new Exception(e.Message);
            }
            catch (FieldValidationException e)
            {
                throw new Exception(e.Message);
            }
            catch (CsvHelperException e)
            {
                throw new Exception(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<IEnumerable<PatientDTO>> ReadCsvFileToPatient(IFormFile source)
        {
            try
            {
                using (var reader = new StreamReader(source.OpenReadStream()))
                {
                    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        Delimiter = ";"
                    };
                    using (var csv = new CsvReader(reader, config))
                    {
                        csv.Context.RegisterClassMap<PatientMap>();
                        var read = csv.Read();
                        var readHeader = csv.ReadHeader();
                        var records = csv.GetRecords<PatientDTO>().ToList();
                        return records;
                    }
                }
            }
            catch (UnauthorizedAccessException e)
            {
                throw new Exception(e.Message);
            }
            catch (FieldValidationException e)
            {
                throw new Exception(e.Message);
            }
            catch (CsvHelperException e)
            {
                throw new Exception(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<byte[]> WriteNewCsvFile(IEnumerable<PatientDTO> patientModels)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (StreamWriter sw = new StreamWriter(memoryStream))
                {
                    sw.WriteLine("sep=,");
                    using (CsvWriter cw = new CsvWriter(sw, CultureInfo.InvariantCulture))
                    {
                        cw.WriteHeader<PatientDTO>();
                        cw.NextRecord();
                        cw.Context.RegisterClassMap<PatientMap>();
                        cw.WriteRecords(patientModels);
                        cw.Flush();
                        return memoryStream.ToArray();
                    }
                }
            }
            return null;
        }

        public async Task<byte[]> WriteNewCsvFile(IEnumerable<ExaminationDTO> examinationModels)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (StreamWriter sw = new StreamWriter(memoryStream))
                {
                    sw.WriteLine("sep=,");
                    using (CsvWriter cw = new CsvWriter(sw, CultureInfo.InvariantCulture))
                    {
                        cw.WriteHeader<ExaminationDTO>();
                        cw.NextRecord();
                        cw.WriteRecords(examinationModels);
                        cw.Flush();
                        return memoryStream.ToArray();
                    }
                }
            }
            return null;
        }
    }
}
