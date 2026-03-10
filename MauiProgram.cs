using Microsoft.Extensions.Logging;
using Microsoft.Maui.Handlers;

namespace POS_OLDWAY_SALOON
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            EntryHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
            {
                #if ANDROID
                    handler.PlatformView.BackgroundTintList = 
                        Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
                #endif
            });
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
