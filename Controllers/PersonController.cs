using System;
using System.Threading.Tasks;
using DevTrust_Task.DTOs;
using DevTrust_Task.Data;
using DevTrust_Task.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DevTrust_Task.Models;

namespace DevTrust_Task.Controllers
{
    public class PersonController : ControllerBase
    {
        private readonly IPersonRepo _personRepository;

        private readonly IServices _services;

        public PersonController(
            IPersonRepo personRepository,
            IServices services
        )
        {
            _personRepository = personRepository;
            _services = services;
        }

        [HttpGet("/save/{json}")]
        public Task<long> Save( string json)
        {
            Console.WriteLine(json);

            Person person = new Person();

            person = (Person) _services.Deserialize(person, json);

            return _personRepository.Save(person);
        }

        [HttpGet("/all/{request}")]
        public async Task<ActionResult<string>> GetAll(string request)
        {
            Person allRequest = new Person();

            allRequest = (Person) _services.Deserialize(allRequest, request);

            Console.WriteLine(allRequest.Address.AddressLine);
            //foreach (KeyValuePair<string, dynamic>
            //    kvp
            //    in
            //    _services.Serialize(request)
            //)
            //{
            //    //textBox3.Text += ("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
            //    Console.WriteLine("Key = {0}", kvp.Key);
            //}
            //foreach (KeyValuePair<string, dynamic>
            //    kvp
            //    in
            //    _services.Serialize(request)["address"]
            //)
            //{
            //    //textBox3.Text += ("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
            //    Console.WriteLine("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
            //}
            //string ans = await _personRepository.GetAll(allRequest);
            //System.Console.Write (ans);
            return Ok();
        }
    }
}
