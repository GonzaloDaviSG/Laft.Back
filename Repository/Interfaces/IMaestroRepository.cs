using System.Collections.Generic;

namespace protecta.laft.api.Repository.Interfaces
{
    public interface IMaestroRepository<T>
    {
        List<T> GetAll();
        T Get(int Id);
    }
}