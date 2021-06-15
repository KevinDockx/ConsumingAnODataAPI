using AirVinyl;
using Microsoft.OData.Client;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AirVinylClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var airVinylContainer = new AirVinylContainer(new Uri("https://localhost:5001/odata"));

            //var people = await airVinylContainer.People.ExecuteAsync();
            //var recordStores = await airVinylContainer.RecordStores.ExecuteAsync();

            var peopleQuery = airVinylContainer.People.Where(p => p.PersonId == 1) as DataServiceQuery;
            var recordStoresQuery = airVinylContainer.RecordStores;

            var batchResponse = await airVinylContainer.ExecuteBatchAsync(
                peopleQuery,
                recordStoresQuery);

            foreach (var operationResponse in batchResponse)
            {
                var peopleResponse = operationResponse as QueryOperationResponse<AirVinyl.Person>;
                if (peopleResponse != null)
                {
                    foreach (AirVinyl.Person person in peopleResponse)
                    {
                        Console.WriteLine($"{person.PersonId} {person.FirstName} {person.LastName}");
                    }
                }
                var recordsStoresResponse = operationResponse as QueryOperationResponse<RecordStore>;
                if (recordsStoresResponse != null)
                {
                    foreach (var recordStore in recordsStoresResponse)
                    {
                        Console.WriteLine($"{recordStore.RecordStoreId} {recordStore.Name}");
                    }
                }
            }


            airVinylContainer.AddToPeople(new AirVinyl.Person()
            {
                FirstName = "John the First",
                LastName = "Smith",
                AmountOfCashToSpend = 400,
                DateOfBirth = new DateTimeOffset(new DateTime(1980, 5, 10)),
                Email = "someaddress@someserver.com",
                NumberOfRecordsOnWishList = 10,
                Gender = Gender.Male
            });

            airVinylContainer.AddToPeople(new AirVinyl.Person()
            {
                FirstName = "John the Second",
                LastName = "Smith",
                AmountOfCashToSpend = 400,
                DateOfBirth = new DateTimeOffset(new DateTime(1980, 5, 10)),
                Email = "someaddress@someserver.com",
                NumberOfRecordsOnWishList = 10,
                Gender = Gender.Male
            });

            await airVinylContainer.SaveChangesAsync(SaveChangesOptions.BatchWithSingleChangeset);

            var people = airVinylContainer.People;
            foreach (var person in people)
            {
                Console.WriteLine($"{person.PersonId} {person.FirstName} {person.LastName}");
            }

            Console.ReadLine();
        }

        public static async Task CodeGeneration()
        {
            var airVinylContainer = new AirVinylContainer(new Uri("https://localhost:5001/odata"));

            //var people = await airVinylContainer.People
            //    .AddQueryOption("$expand", "VinylRecords")
            //    .AddQueryOption("$select", "PersonId,FirstName")
            //    .AddQueryOption("$orderby", "FirstName")
            //    .AddQueryOption("$top", "2")
            //    .AddQueryOption("$skip", "4")
            //    .ExecuteAsync();

            var people = airVinylContainer.People
                //.Expand(p => p.VinylRecords)
                .OrderBy(p => p.FirstName)
                .Skip(4)
                .Take(2)
                .Select(p => new { p.PersonId, p.FirstName, VinylRecords = p.VinylRecords });


            foreach (var person in people)
            {
                Console.WriteLine($"{person.PersonId} {person.FirstName}");

                //  airVinylContainer.LoadProperty(person, "VinylRecords");

                foreach (var vinylRecord in person.VinylRecords)
                {
                    Console.WriteLine($"---- {vinylRecord.VinylRecordId} {vinylRecord.Title}");
                }
            }

            var kevin = await airVinylContainer.People.ByKey(1).GetValueAsync();
            Console.WriteLine($"{kevin.PersonId} {kevin.FirstName} {kevin.LastName}");
        }

        public static void NoCodeGeneration()
        {
            var localContext = new MyLocalContainer(new Uri("https://localhost:5001/odata"));
            var kevin = localContext.People.Where(p => p.PersonId == 1).First(); //.Execute();
            Console.WriteLine($"{kevin.FirstName} {kevin.LastName}");

            //foreach (var person in people)
            //{
            //    Console.WriteLine($"{person.PersonId} {person.FirstName} {person.LastName}");
            //}

            var personToCreate = new Person()
            {
                FirstName = "Servilia",
                LastName = "Smith",
                AmountOfCashToSpend = 400,
                DateOfBirth = new DateTimeOffset(new DateTime(1980, 5, 10)),
                Email = "someaddress@someserver.com",
                NumberOfRecordsOnWishList = 10
            };

            localContext.AddPerson(personToCreate);

            LogTrackedPeople(localContext);

            localContext.SaveChanges();

            LogTrackedPeople(localContext);

            personToCreate.FirstName = "Marcus";

            localContext.UpdateObject(personToCreate);

            LogTrackedPeople(localContext);

            localContext.SaveChanges();

            LogTrackedPeople(localContext);

            localContext.DeleteObject(personToCreate);

            LogTrackedPeople(localContext);

            localContext.SaveChanges();

            LogTrackedPeople(localContext);

            var people = localContext.People.Execute();

            foreach (var person in people)
            {
                Console.WriteLine($"{person.PersonId} {person.FirstName} {person.LastName}");
            }
        }

        public static void LogTrackedPeople(MyLocalContainer container)
        {
            foreach (var entityDescriptor in container.EntityTracker.Entities)
            {
                if (entityDescriptor.Entity is Person castedPerson)
                {
                    Console.WriteLine($"{entityDescriptor.State} - {castedPerson.PersonId} " +
                        $"{castedPerson.FirstName} {castedPerson.LastName}");
                }
            }
        }

    }
}
