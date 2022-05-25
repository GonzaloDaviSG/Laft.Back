using System;
using System.Collections.Generic;
using protecta.laft.api.Models;
namespace protecta.laft.api.Repository.Interfaces
{
    public interface IRegistroRepository
    {
        List<Registro> GetAll();
        Registro Get(int Id);
        void Add(Registro model);
        void Update(Registro model);
    }
}