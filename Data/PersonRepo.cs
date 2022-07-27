
using DevTrust_Task.DTOs;
using DevTrust_Task.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DevTrust_Task.Data
{
    public class PersonRepo : IPersonRepo
    {
        private readonly DevTrustContext _context;

        public PersonRepo(DevTrustContext context)
        {
            _context = context;
        }

        public async Task<string> GetAll(GetAllRequest request)
        {
            return _context.Person.FirstAsync(p => p.Id == 1).Id.ToString();
        }
    }
}