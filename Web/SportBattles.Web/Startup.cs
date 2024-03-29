﻿namespace SportBattles.Web
{
    using System.Reflection;

    using Azure.Storage.Blobs;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    using SportBattles.Data;
    using SportBattles.Data.Common;
    using SportBattles.Data.Common.Repositories;
    using SportBattles.Data.Models;
    using SportBattles.Data.Repositories;
    using SportBattles.Data.Seeding;
    using SportBattles.Services;
    using SportBattles.Services.Data;
    using SportBattles.Services.Mapping;
    using SportBattles.Services.Messaging;
    using SportBattles.Services.TennisPlayerPictureScraper;
    using SportBattles.Web.Areas.Identity;
    using SportBattles.Web.ViewModels;

    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection")).UseLazyLoadingProxies());

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>()
                .AddSignInManager<MySignInManager<ApplicationUser>>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<CookiePolicyOptions>(
                options =>
                    {
                        options.CheckConsentNeeded = context => true;
                        options.MinimumSameSitePolicy = SameSiteMode.None;
                    });

            services.AddControllersWithViews(
                options =>
                    {
                        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                    }).AddRazorRuntimeCompilation();
            services.AddServerSideBlazor().AddHubOptions(x => x.MaximumReceiveMessageSize = 131072);
            services.AddRazorPages();
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddSingleton(this.configuration);
            services.AddSingleton(x => new BlobServiceClient(this.configuration["ConnectionStrings:AzureBlob"]));

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services
            services.AddTransient<IEmailSender>(serviceProvider => new SendGridEmailSender(this.configuration["SendGrid-ApiKey"]));
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<ISportsService, SportsService>();
            services.AddTransient<IMatchesService, MatchesService>();
            services.AddTransient<ITennisMatchesService, TennisMatchesService>();
            services.AddTransient<IGamesService, GamesService>();
            services.AddTransient<IGamePointsService, GamePointsService>();
            services.AddTransient<IPredictionsService, PredictionsService>();
            services.AddTransient<ITennisPredictionsService, TennisPredictionsService>();
            services.AddTransient<ITeamsService, TeamsService>();
            services.AddTransient<ITennisPlayersService, TennisPlayersService>();
            services.AddTransient<ICountriesService, CountriesService>();
            services.AddTransient<ITournamentsService, TournamentsService>();
            services.AddTransient<ILiveScoreApi, LiveScoreApi>();
            services.AddTransient<ITennisExplorerScraperService, TennisExplorerScraperService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            // Seed data on application startup
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
                new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(
                endpoints =>
                    {
                        endpoints.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapRazorPages();
                        endpoints.MapBlazorHub();
                    });
        }
    }
}
