﻿using CoreMatch.Database;
using CoreMatch.DTO;
using CoreMatch.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace CoreMatch.Controllers
{
    public class PersonController : ApiController
    {
        [HttpGet]
        [Route("api/person")]
        public async Task<PersonDTO[]> Get()
        {
            using (var repo = new PersonRepository())
            {
                return (await repo.GetAll()).Select(x => new PersonDTO(x)).ToArray();
            }
        }

        [HttpGet]
        [Route("api/person/{id}")]
        public async Task<PersonDTO> Get(int id)
        {
            using (var repo = new PersonRepository())
            {
                Person existing = await repo.GetById(id);
                if (existing == null) { throw new HttpResponseException(HttpStatusCode.NotFound); }
                return new PersonDTO(existing);
            }
        }

        [HttpPost]
        [Route("api/person/create")]
        public async Task<int> Post(PersonDTO item)
        {
            using (var repo = new PersonRepository())
            {
                Person person = new Person();
                item.Save(person);

                await repo.Create(person);
                await repo.Commit();
                return person.Id;
            }
        }

        [HttpPut]
        [Route("api/person")]
        public async Task Put(PersonDTO item)
        {
            using (var repo = new PersonRepository())
            {
                Person existing = await repo.GetById(item.Id);
                if (existing == null) { throw new HttpResponseException(HttpStatusCode.NotFound); }
                item.Save(existing);
                await repo.Commit();
            }
        }

        [HttpDelete]
        [Route("api/person")]
        public async Task Delete(PersonDTO item)
        {
            using (var repo = new PersonRepository())
            {
                Person existing = await repo.GetById(item.Id);
                if (existing == null) { return; }
                await repo.Delete(existing);
                await repo.Commit();
            }
        }
    }
}