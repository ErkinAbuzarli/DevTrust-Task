using System.Threading.Tasks;
using DevTrust_Task.DTOs;
using DevTrust_Task.Models;

namespace DevTrust_Task.Data
{
    public interface IPersonRepo
    { 
        Task<string> GetAll(GetAllRequest request);
        Task<long> Save(Person person);
    }
}