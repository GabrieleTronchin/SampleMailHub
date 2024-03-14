using Mail.Hub.Domain.Models;
using Mail.Hub.Domain.Reciver;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;


namespace Mail.Hub.Domain
{
    public static partial class ServicesExtensions
    {

        public static IServiceCollection AddDomains(this IServiceCollection services,
                                                    IConfiguration configuration)
        {
            services.AddTransient<MailHubService>();
            services.AddTransient<IReviceMailService, ReviceMailService>();

            services.AddOptions<SenderMailOptions>().Bind(configuration.GetSection($"{nameof(SenderMailOptions)}")).ValidateDataAnnotations();
            services.AddOptions<ReciverMailOptions>().Bind(configuration.GetSection($"{nameof(ReciverMailOptions)}")).ValidateDataAnnotations();

            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(typeof(NewMailMessageHandler).Assembly);
            });

            services.AddQuartz(q =>
            {
                q.AddJobAndTrigger<IncomeMailsJob>(configuration);
            });

            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);


            return services;
        }



        public static void AddJobAndTrigger<T>(
       this IServiceCollectionQuartzConfigurator quartz,
       IConfiguration config)
       where T : IJob
        {
            // Use the name of the IJob as the appsettings.json key
            string jobName = typeof(T).Name;

            // Try and load the schedule from configuration

            var cronSchedule = config[jobName];

            // Some minor validation
            if (string.IsNullOrEmpty(cronSchedule))
            {
                throw new Exception($"No Quartz.NET Cron schedule found for job in configuration at {jobName}");
            }

            // register the job as before
            var jobKey = new JobKey(jobName);
            quartz.AddJob<T>(opts => opts.WithIdentity(jobKey));

            quartz.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity(jobName + "-trigger")
                .WithCronSchedule(cronSchedule)); // use the schedule from configuration
        }

    }
}
