using MauiEFStress.Service;

/* Unmerged change from project 'MauiEFStress (net8.0-android)'
Before:
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
After:
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
*/

/* Unmerged change from project 'MauiEFStress (net8.0-windows10.0.19041.0)'
Before:
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
After:
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
*/
using Microsoft.EntityFrameworkCore;

namespace MauiEFStress
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddScoped<DbContext, PeopleContext>();

            /*
#if DEBUG
		builder.Logging.AddDebug();
#endif
            */
            return builder.Build();
        }
    }
}