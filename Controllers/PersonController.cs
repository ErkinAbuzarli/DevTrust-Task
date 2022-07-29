using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevTrust_Task.DTOs;
using DevTrust_Task.Data;
using DevTrust_Task.Models;
using DevTrust_Task.Services;
using Microsoft.AspNetCore.Mvc;

namespace DevTrust_Task.Controllers
{
    public class PersonController : ControllerBase
    {
        private readonly IPersonRepo _personRepository;

        private readonly ISerializer _services;

        public PersonController(
            IPersonRepo personRepository,
            ISerializer services
        )
        {
            _personRepository = personRepository;
            _services = services;
        }

        [HttpGet("/save/{json}")]
        public Task<long> Save(string json)
        {
            Console.WriteLine (json);

            Person person = new Person();

            person = (Person) _services.Deserialize(person, json);

            return _personRepository.Save(person);
        }

        [HttpGet("/all/{request_string}")]
        public async Task<string> GetAll(string request_string)
        {
            GetAllRequest requests = new GetAllRequest();
            requests =
                (GetAllRequest) _services.Deserialize(requests, request_string);
            List<Person> people;
            string json = "{";

            people = await _personRepository.GetAll(requests);

            if (people.Count == 0) return "-1";
            if (people.Count == 1) return _services.Serialize(people[0]);

            foreach (Person person in people)
            {
                json += _services.Serialize(person) + ",";
            }
            json = json.Substring(0, json.Length - 1) + "}";

            return json;
        }
    }
}
