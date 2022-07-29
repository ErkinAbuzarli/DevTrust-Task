using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevTrust_Task;
using DevTrust_Task.DTOs;
using DevTrust_Task.Models;
using Microsoft.EntityFrameworkCore;

namespace DevTrust_Task.Data
{
    public class PersonRepo : IPersonRepo
    {
        private readonly DevTrustContext _context;

        public PersonRepo(DevTrustContext context)
        {
            _context = context;
        }

        public async Task<List<Person>> GetAll(GetAllRequest request)
        {
            List<Person> people =
                await _context
                    .Person
                    .Where(p =>
                        (
                        p.FirstName == request.FirstName ||
                        request.FirstName == null
                        ) &&
                        (
                        p.LastName == request.LastName ||
                        request.LastName == null
                        ) &&
                        (
                        p.Address.City == request.City || request.City == null
                        ))
                    .ToListAsync();
            foreach (Person person in people)
            {
                person.Address =
                    await _context
                        .Address
                        .FirstAsync(p => p.Id == person.AddressId);
            }
            return people;
        }

        public async Task<long> Save(Person person)
        {
            try
            {
                Person existed = _context.Person.First(a => a.Id == person.Id);
                _context.Person.Update(person);
            }
            catch(Exception)
            {
                await _context.Person.AddAsync(person);
            }

            await _context.SaveChangesAsync();

            return person.Id;
        }
    }
}
