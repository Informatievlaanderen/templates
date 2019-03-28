namespace ExampleRegistry.Api.Infrastructure
{
    using System.Security.Cryptography.X509Certificates;
    using Microsoft.AspNetCore.Hosting;
    using Be.Vlaanderen.Basisregisters.Api;

    public class Program
    {
        private static class DevelopmentCertificate
        {
            internal const string Name = "localhost.pfx";
            internal const string Key = "dev-pfx-password";
        }

        public static void Main(string[] args) => CreateWebHostBuilder(args).Build().Run();

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
            => new WebHostBuilder()
                .UseDefaultForApi<Startup>(
                    httpPort: 1090,
                    httpsPort: 1444,
                    httpsCertificate: () => new X509Certificate2(DevelopmentCertificate.Name, DevelopmentCertificate.Key),
                    commandLineArgs: args);
    }
}
