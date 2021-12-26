using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using Trainer.DAL.Util.Constant;

namespace Trainer.BLL.DTO
{
    public class PatientDTO
    {
        public Guid Id
        {
            get;
            set;
        }

        public string FirstName
        {
            get;
            set;
        }
          
        public string MiddleName
        {
            get;
            set;
        }

        public string LastName
        {
            get;
            set;
        }

        public int Age
        {
            get;
            set;
        }

        public Sex Sex
        {
            get;
            set;
        }

        public string About
        {
            get;
            set;
        }

        public string Hobbies
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }

        public IList<ExaminationDTO> Examinations
        {
            get;
            set;
        }

        public IList<ResultsDTO> Results
        {
            get;
            set;
        }
    }
}
