using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UExpo.Domain.Places;

namespace UExpo.Application.BackgroundServices;

public class YearlyTaskService(IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            DateTime now = DateTime.Now;
            //DateTime nextRun = new DateTime(now.Year + 1, 1, 1);
            DateTime nextRun = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute + 1, now.Second);

            TimeSpan timeUntilNextRun = nextRun - now;

            while(timeUntilNextRun.TotalMilliseconds > Int32.MaxValue)
            {
                await Task.Delay(Int32.MaxValue, cancellationToken);
                timeUntilNextRun = nextRun - DateTime.Now;
            }

            if (timeUntilNextRun.TotalMilliseconds > 0)
            {
                await Task.Delay(timeUntilNextRun, cancellationToken);
            }
            

            if (!cancellationToken.IsCancellationRequested)
            {
                using var scope = serviceProvider.CreateScope();
                var placeService = scope.ServiceProvider.GetRequiredService<IPlaceService>();

                await placeService.UpdateStatusAsync();
            }

        }
    }
}
