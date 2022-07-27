using System.Threading.Tasks;
using DevTrust_Task.DTOs;

namespace DevTrust_Task.Data
{
    public interface IPersonRepo
    { 
        Task<string> GetAll(GetAllRequest request);
    }
}