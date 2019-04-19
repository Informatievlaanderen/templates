namespace ExampleRegistry.Projector.Infrastructure
{
    using System.Security.Cryptography.X509Certificates;
    using Be.Vlaanderen.Basisregisters.Api;
    using Microsoft.AspNetCore.Hosting;

    public class Program
    {
        private static readonly DevelopmentCertificate DevelopmentCertificate =
            new DevelopmentCertificate(
                "localhost.pfx",
                "dev-pfx-password");

        public static void Main(string[] args) => CreateWebHostBuilder(args).Build().Run();

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
            => new WebHostBuilder()
                .UseDefaultForApi<Startup>(
                    new ProgramOptions
                    {
                        Hosting =
                        {
                            HttpPort = 8100,
                            HttpsPort = 9100,
                            HttpsCertificate = DevelopmentCertificate.ToCertificate,
                        },
                        Logging =
                        {
                            WriteTextToConsole = true,
                            WriteJsonToConsole = false
                        },
                        Runtime =
                        {
                            CommandLineArgs = args
                        }
                    });
    }
}
