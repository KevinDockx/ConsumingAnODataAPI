using Microsoft.OData.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirVinylClient
{
    [Key("PersonId")]
    public class Person
    {
        public int PersonId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public int NumberOfRecordsOnWishList { get; set; }
        public decimal AmountOfCashToSpend { get; set; }
    }

    public class MyLocalContainer : DataServiceContext
    {
        public DataServiceQuery<Person> People { get; }

        public MyLocalContainer(Uri serviceRoot) : base(serviceRoot)
        { 
            People = base.CreateQuery<Person>("People"); 
        }

        public void AddPerson(Person person)
        {
            base.AddObject("People", person);
        }
    }
}
