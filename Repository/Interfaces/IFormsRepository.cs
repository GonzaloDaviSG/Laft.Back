using System.Collections.Generic;
using System;
using protecta.laft.api.DTO;
using protecta.laft.api.Models;

namespace protecta.laft.api.Repository.Interfaces {
    public interface IFormsRepository {
       
      List<Forms> GetFormsList ();
    }
}