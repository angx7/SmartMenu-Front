using DotNetEnv;
using Microsoft.Extensions.Logging;

#if ANDROID
using Android.App;
using System.IO;
using Android.Content.Res;
#endif

namespace SmartMenu
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            string envPath;

#if ANDROID
            // Ruta destino accesible para .env
            string destEnvPath = Path.Combine(FileSystem.AppDataDirectory, ".env");

            try
            {
                // Lee desde Assets/env (sin punto)
                var context = Android.App.Application.Context;
                using var assetStream = context.Assets.Open("env");
                using var reader = new StreamReader(assetStream);
                File.WriteAllText(destEnvPath, reader.ReadToEnd());

                envPath = destEnvPath;
            }
            catch (Exception ex)
            {
                throw new FileNotFoundException("No se pudo copiar .env desde assets", ex);
            }
#else
            // Para Windows/macOS/Linux
            envPath = Path.Combine(AppContext.BaseDirectory, ".env");
#endif

            // Cargar variables de entorno
            if (File.Exists(envPath))
            {
                Env.Load(envPath);
                System.Diagnostics.Debug.WriteLine("✅ NGROK_URL cargado: " + Environment.GetEnvironmentVariable("NGROK_URL"));
            }
            else
            {
                throw new FileNotFoundException($".env no encontrado en: {envPath}");
            }

            // Inicialización de MAUI
            var builder = MauiApp.CreateBuilder();
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
