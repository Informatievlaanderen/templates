namespace ExampleRegistry.Api.Infrastructure
{
    using System.Security.Cryptography.X509Certificates;
    using Microsoft.AspNetCore.Hosting;
    using Be.Vlaanderen.Basisregisters.Api;

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
                            HttpPort = 8000,
                            HttpsPort = 9000,
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
