using System.Collections.Generic;
using System.Threading.Tasks;
using DevTrust_Task.DTOs;
using DevTrust_Task.Models;

namespace DevTrust_Task.Data
{
    public interface IPersonRepo
    {
        Task<List<Person>> GetAll(GetAllRequest request);
        Task<long> Save(Person person);
    }
}