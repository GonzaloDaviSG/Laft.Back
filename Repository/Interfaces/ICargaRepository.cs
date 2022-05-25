using System;
using System.Collections.Generic;
using protecta.laft.api.Models;

namespace protecta.laft.api.Repository.Interfaces
{
    public interface ICargaRepository
    {
        List<Carga> GetAll();
        Carga Get(int Id);
        void Add(Carga model);
        void Update(Carga model);
    }
}