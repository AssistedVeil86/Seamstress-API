using System;

namespace Seamstress.VSA.Infrastructure.BackgroundJobs;

public interface IWeeklyReportJob
{
    Task ExecuteAsync();
}
