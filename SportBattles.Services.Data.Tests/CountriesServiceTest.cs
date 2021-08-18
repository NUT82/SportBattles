namespace SportBattles.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Moq;
    using SportBattles.Data;
    using SportBattles.Data.Common.Repositories;
    using SportBattles.Data.Models;
    using SportBattles.Data.Repositories;
    using SportBattles.Services.Mapping;
    using SportBattles.Web.ViewModels.Administration.Country;
    using Xunit;

    public class CountriesServiceTest
    {
        public CountriesServiceTest()
        {
            this.InitializeMapper();
        }

        [Fact]
        public async Task AddToSportWorkCorrectly()
        {
            var mockRepoSport = new Mock<IDeletableEntityRepository<Sport>>();
            IList<Sport> sports = new List<Sport>
            {
                new Sport { Name = "Football", Id = 1 },
                new Sport { Name = "Tennis", Id = 2 },
                new Sport { Name = "Basketball", Id = 3 },
            };
            mockRepoSport.Setup(x => x.All()).Returns(sports.AsQueryable());

            var mockRepoCountry = new Mock<IDeletableEntityRepository<Country>>();
            IList<Country> countries = new List<Country>
            {
                new Country { Id = 1, Name = "Bulgaria", Code = "BG", FlagUrl = "http://bg.test.png", Sports = sports },
                new Country { Id = 2, Name = "England", Code = "EN", FlagUrl = "http://en.test.png", Sports = sports },
                new Country { Id = 3, Name = "Romania", Code = "RO", FlagUrl = "http://ro.test.png", Sports = new List<Sport>() },
            };
            mockRepoCountry.Setup(x => x.All())
                .Returns(countries.AsQueryable());
            var service = new CountriesService(mockRepoCountry.Object, mockRepoSport.Object);

            await service.AddToSport(3, 2);
            Assert.True(countries[2].Sports.Contains(sports[1]));

            var countryId = await service.AddToSport(3, 2);
            Assert.Equal(3, countryId);

            await Assert.ThrowsAsync<ArgumentNullException>(() => service.AddToSport(1, 4));
            await Assert.ThrowsAsync<ArgumentNullException>(() => service.AddToSport(4, 1));
        }

        [Fact]
        public async Task GetAllForSportReturnListWithRightCountryForGivenSportId()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var dbContext = new ApplicationDbContext(options);

            dbContext.Sports.Add(new Sport { Id = 1, Name = "TestSport" });
            dbContext.Sports.Add(new Sport { Id = 2, Name = "TestSport2" });
            dbContext.Countries.Add(new Country { Id = 1, Name = "TestCountry" });
            dbContext.Countries.Add(new Country { Id = 2, Name = "TestCountry2" });
            await dbContext.SaveChangesAsync();

            var countryRepository = new EfDeletableEntityRepository<Country>(dbContext);
            var sportRepository = new EfDeletableEntityRepository<Sport>(dbContext);

            var service = new CountriesService(countryRepository, sportRepository);
            await service.AddToSport(1, 1);

            var result = service.GetAllForSport<CountriesViewModel>(1);
            Assert.True(result.Count() == 1);
            Assert.DoesNotContain(result, c => c.Id == 2);

            await service.AddToSport(2, 1);
            result = service.GetAllForSport<CountriesViewModel>(1);
            Assert.True(result.Count() == 2);
        }

        [Fact]
        public async Task GetAllOthersForSportReturnListWithCountriesWithoutGivenSportId()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var dbContext = new ApplicationDbContext(options);

            dbContext.Sports.Add(new Sport { Id = 1, Name = "TestSport" });
            dbContext.Sports.Add(new Sport { Id = 2, Name = "TestSport2" });
            dbContext.Countries.Add(new Country { Id = 1, Name = "TestCountry" });
            dbContext.Countries.Add(new Country { Id = 2, Name = "TestCountry2" });
            await dbContext.SaveChangesAsync();

            var countryRepository = new EfDeletableEntityRepository<Country>(dbContext);
            var sportRepository = new EfDeletableEntityRepository<Sport>(dbContext);

            var service = new CountriesService(countryRepository, sportRepository);
            await service.AddToSport(1, 1);

            var result = service.GetAllOthersForSport<CountriesViewModel>(1);
            Assert.True(result.Count() == 1);
            Assert.Contains(result, c => c.Id == 2);
        }

        private void InitializeMapper() => AutoMapperConfig.
            RegisterMappings(Assembly.Load("SportBattles.Web.ViewModels"));
    }
}
