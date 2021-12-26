﻿using CsvHelper.Configuration;
using System;
using Trainer.BLL.DTO;

namespace Trainer.BLL.Infrastructure
{
    public class PatientMap : ClassMap<PatientDTO>
    {
        public PatientMap()
        {
            Map(m => m.Id).Name("Id");
            Map(m => m.LastName).Name("LastName");
            Map(m => m.FirstName).Name("FirstName");
            Map(m => m.MiddleName).Name("MiddleName");
            Map(m => m.Age).Name("Age");
            Map(m => m.Sex).Name("Sex");
            Map(m => m.Email).Name("Email");
            Map(m => m.About).Name("About").Convert(m =>
            {
                return m.Value.About != null ? $"{m.Value.About}" : String.Empty;
            });
            Map(m => m.Hobbies).Name("Hobbies").Convert(m =>
            {
                return m.Value.Hobbies != null ? $"{m.Value.Hobbies}" : String.Empty;
            });
        }
    }
}
