namespace ExampleRegistry.Structurizr
{
    using System;
    using System.IO;
    using System.Linq;
    using Microsoft.Extensions.Configuration;
    using global::Structurizr;
    using global::Structurizr.Api;
    using global::Structurizr.AdrTools;

    public class Program
    {
        private const string WorkspaceUrlFormat = "https://structurizr.com/workspace/{0}";

        private const string PersonUserName = "Gebruiker";

        public static class Ids
        {
            public static int PersonUser = 10000;
            public static int SoftwareSystemExampleRegistry = 10001;
            public static int ContainerApi = 10002;
            public static int ContainerApiRunner = 10003;
            public static int ContainerApiStore = 10004;
            public static int ContainerEventStore = 10005;

            public static int SoftwareSystemProjectionProducer = 10007;
            public static int SoftwareSystemApi = 10008;
        }

        public static class CustomTags
        {
            public static string Store = "Store";
            public static string Event = "Event";
            public static string Command = "Command";
            public static string Https = "HTTPS";
            public static string EntityFramework = "Entity Framework";
            public static string SqlStreamStore = "SqlStreamStore";
            public static string Direct = "Direct";
        }

        // This crap is because structurizr.com expects integers for ids, while structurizr.net wants strings
        private static readonly string PersonUserId = Ids.PersonUser.ToString();
        private static readonly string SoftwareSystemExampleRegistryId = Ids.SoftwareSystemExampleRegistry.ToString();
        private static readonly string ContainerApiId = Ids.ContainerApi.ToString();
        private static readonly string ContainerApiRunnerId = Ids.ContainerApiRunner.ToString();
        private static readonly string ContainerApiStoreId = Ids.ContainerApiStore.ToString();
        private static readonly string ContainerEventStoreId = Ids.ContainerEventStore.ToString();

        private static readonly string SoftwareSystemProjectionProducerId = Ids.SoftwareSystemProjectionProducer.ToString();
        private static readonly string SoftwareSystemApiId = Ids.SoftwareSystemApi.ToString();

        private static void Main()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile($"appsettings.{Environment.MachineName.ToLowerInvariant()}.json", optional: true, reloadOnChange: false)
                .Build();

            var workspaceId = long.Parse(configuration["Structurizr:WorkspaceId"]);
            var apiKey = configuration["Structurizr:ApiKey"];
            var apiSecret = configuration["Structurizr:ApiSecret"];
            var adrPath = configuration["Structurizr:AdrPath"];

            var workspace = new Workspace("ExampleRegistry", "Voorbeeld register.")
            {
                Version = DateTime.Today.ToString("yyyy-MM-dd"),
            };

            var model = workspace.Model;

            var user = CreatePersonUser(model);

            var exampleRegistry = CreateSystemExampleRegistry(model);

            var api = CreateContainerApi(exampleRegistry);
            var apiRunner = CreateContainerApiRunner(exampleRegistry);
            var apiStore = CreateContainerApiStore(exampleRegistry);
            var eventStore = CreateContainerEventStore(exampleRegistry);

            user.Uses(exampleRegistry, "raadpleegt", "HTTPS").AddTags(CustomTags.Https);
            user.Uses(api, "raadpleegt", "HTTPS").AddTags(CustomTags.Https);

            api.Uses(apiStore, "leest gegevens", "Entity Framework").AddTags(CustomTags.EntityFramework);
            api.Uses(eventStore, "produceert events", "SqlStreamStore").AddTags(CustomTags.SqlStreamStore);

            apiRunner.Uses(eventStore, "leest events", "SqlStreamStore").AddTags(CustomTags.SqlStreamStore);
            apiRunner.Uses(apiStore, "projecteert gegevens", "Entity Framework").AddTags(CustomTags.EntityFramework);

            CreateApiFake(model);
            //CreateApiRunnerFake(model);

            var views = workspace.Views;

            CreateContextView(views, model);
            CreateApiContainerView(views, model);
            CreateApiRunnerContainerView(views, model);

            ConfigureStyles(views);

            var adrDirectory = new DirectoryInfo(adrPath);
            var adrToolsImporter = new AdrToolsImporter(workspace, adrDirectory);
            adrToolsImporter.ImportArchitectureDecisionRecords(exampleRegistry);

            UploadWorkspaceToStructurizr(workspace, workspaceId, apiKey, apiSecret);
        }

        private static Person CreatePersonUser(Model model)
        {
            var user = model
                .AddPerson(
                    PersonUserName,
                    "Een gebruiker van het voorbeeld register.");

            user.Id = PersonUserId;

            return user;
        }

        private static SoftwareSystem CreateSystemExampleRegistry(Model model)
        {
            var exampleRegistry = model
                .AddSoftwareSystem(
                    Location.Internal,
                    "ExampleRegistry",
                    "Het `voorbeeld register laat gebruikers toe alle authentieke gegevens van een voorbeeld te raadplegen.");

            exampleRegistry.Id = SoftwareSystemExampleRegistryId;
            exampleRegistry.Url = "https://github.com/informatievlaanderen/example-registry";

            return exampleRegistry;
        }

        private static Container CreateContainerApi(SoftwareSystem exampleRegistry)
        {
            var api = exampleRegistry
                .AddContainer(
                    "Loket API",
                    "Publiek beschikbare API, bedoeld ter integratie in het loket.",
                    "REST/HTTPS");

            api.Id = ContainerApiId;

            return api;
        }

        private static Container CreateContainerApiRunner(SoftwareSystem exampleRegistry)
        {
            var apiRunner = exampleRegistry
                .AddContainer(
                    "Loket API projecties",
                    "Asynchrone runner die events verwerkt ten behoeve van de loket API.",
                    "Event Sourcing");

            apiRunner.Id = ContainerApiRunnerId;

            return apiRunner;
        }

        private static Container CreateContainerApiStore(SoftwareSystem exampleRegistry)
        {
            var apiStore = exampleRegistry
                .AddContainer(
                    "Loket API gegevens",
                    "Gegevens geoptimaliseerd voor de loket API.",
                    "SQL Server");

            apiStore.Id = ContainerApiStoreId;
            apiStore.AddTags(CustomTags.Store);

            return apiStore;
        }

        private static Container CreateContainerEventStore(SoftwareSystem exampleRegistry)
        {
            var eventStore = exampleRegistry
                .AddContainer(
                    "Eventstore",
                    "Authentieke bron van gegevens, opgeslagen als een stroom van events.",
                    "SQL Server");

            eventStore.Id = ContainerEventStoreId;
            eventStore.AddTags(CustomTags.Store);

            return eventStore;
        }

        private static void CreateApiFake(Model model)
        {
            var exampleRegistry = model.GetSoftwareSystemWithId(SoftwareSystemExampleRegistryId);

            var apiStore = exampleRegistry.GetContainerWithId(ContainerApiStoreId);
            var eventStore = exampleRegistry.GetContainerWithId(ContainerEventStoreId);

            var api = model
                .AddSoftwareSystem(
                    Location.Internal,
                    "Loket API",
                    "Publiek beschikbare API, bedoeld ter integratie in het loket.");

            api.Id = SoftwareSystemApiId;

            api
                .Uses(apiStore, "loket api leest gegevens", "Entity Framework")
                .AddTags(CustomTags.EntityFramework, CustomTags.Direct);

            api
                .Uses(eventStore, "loket api produceert events", "SqlStreamStore")
                .AddTags(CustomTags.SqlStreamStore, CustomTags.Direct);
        }

        //private static void CreateApiRunnerFake(Model model)
        //{
        //    var exampleRegistry = model.GetSoftwareSystemWithId(SoftwareSystemExampleRegistryId);

        //    var apiStore = exampleRegistry.GetContainerWithId(ContainerApiStoreId);
        //    var eventStore = exampleRegistry.GetContainerWithId(ContainerEventStoreId);

        //    var projectionProducer = model
        //        .AddSoftwareSystem(
        //            Location.Internal,
        //            "Loket API projecties runner",
        //            "Asynchrone runner die events verwerkt ten behoeve van de loket API.");

        //    projectionProducer.Id = SoftwareSystemProjectionProducerId;
        //    //projectionProducer.Url = string.Format(_workspaceUrlViewFormat, "Loket%20API%20overzicht");

        //    apiStore
        //        .Uses(projectionProducer, "runner projecteert gegevens", "Entity Framework", InteractionStyle.Asynchronous)
        //        .AddTags(CustomTags.EntityFramework, CustomTags.Direct);

        //    eventStore
        //        .Uses(projectionProducer, "runner leest events", "SqlStreamStore", InteractionStyle.Asynchronous)
        //        .AddTags(CustomTags.SqlStreamStore, CustomTags.Direct);
        //}

        private static void CreateContextView(ViewSet views, Model model)
        {
            var exampleRegistry = model.GetSoftwareSystemWithId(SoftwareSystemExampleRegistryId);
            var user = model.GetPersonWithName(PersonUserName);

            var contextView = views
                .CreateSystemContextView(
                    exampleRegistry,
                    "Globaal overzicht",
                    "Globaal overzicht van het voorbeeld register.");

            contextView.Add(exampleRegistry);
            contextView.Add(user);

            contextView.PaperSize = PaperSize.A6_Portrait;
        }

        private static void CreateApiContainerView(ViewSet views, Model model)
        {
            var exampleRegistry = model.GetSoftwareSystemWithId(SoftwareSystemExampleRegistryId);
            var projectionProducer = model.GetSoftwareSystemWithId(SoftwareSystemProjectionProducerId);

            var user = model.GetPersonWithName(PersonUserName);
            var api = exampleRegistry.GetContainerWithId(ContainerApiId);
            var apiStore = exampleRegistry.GetContainerWithId(ContainerApiStoreId);
            var eventStore = exampleRegistry.GetContainerWithId(ContainerEventStoreId);

            var containerView = views.CreateContainerView(
                exampleRegistry,
                "Loket API overzicht",
                "Detail overzicht hoe de loket API aan gegevens komt.");

            containerView.Add(user);
            containerView.Add(api);
            containerView.Add(apiStore);
            containerView.Add(eventStore);

            containerView.Add(projectionProducer);

            containerView.PaperSize = PaperSize.A5_Portrait;
        }

        private static void CreateApiRunnerContainerView(ViewSet views, Model model)
        {
            var exampleRegistry = model.GetSoftwareSystemWithId(SoftwareSystemExampleRegistryId);
            var api = model.GetSoftwareSystemWithId(SoftwareSystemApiId);

            var apiRunner = exampleRegistry.GetContainerWithId(ContainerApiRunnerId);
            var apiStore = exampleRegistry.GetContainerWithId(ContainerApiStoreId);
            var eventStore = exampleRegistry.GetContainerWithId(ContainerEventStoreId);

            var containerView = views.CreateContainerView(
                exampleRegistry,
                "Loket API projecties runner",
                "Detail overzicht hoe gegevens voor de loket API worden gemaakt.");

            containerView.Add(apiRunner);
            containerView.Add(apiStore);
            containerView.Add(eventStore);

            containerView.Add(api);

            containerView.PaperSize = PaperSize.A6_Portrait;

            var start = 100;
            var blockWidth = 450;
            var blockHeight = 300;
            var xPortion = (containerView.PaperSize.height - (start * 3) - (3 * blockHeight)) / (3 - 1);

            var middle = containerView.PaperSize.width / 2 - (blockWidth / 2);
            var left = containerView.PaperSize.width / 4 - (blockWidth / 2);
            var right = (containerView.PaperSize.width / 4 * 3) - (blockWidth / 2);

            SetPosition(containerView, ContainerApiRunnerId, middle, start);
            SetPosition(containerView, ContainerApiStoreId, left, start + blockHeight + xPortion);
            SetPosition(containerView, ContainerEventStoreId, right, start + blockHeight + xPortion);
            SetPosition(containerView, SoftwareSystemApiId, middle, start + blockHeight + (blockHeight + xPortion * 2));
        }

        private static void ConfigureStyles(ViewSet views)
        {
            var styles = views.Configuration.Styles;
            styles.Add(new ElementStyle(Tags.Person) { Background = "#08427b", Color = "#ffffff", Shape = Shape.Person });
            styles.Add(new ElementStyle(Tags.SoftwareSystem) { Background = "#1168bd", Color = "#ffffff" });
            styles.Add(new ElementStyle(Tags.Container) { Background = "#438dd5", Color = "#ffffff" });
            styles.Add(new ElementStyle(Tags.Component) { Background = "#85BBF0", Color = "#444444" });
            styles.Add(new ElementStyle(CustomTags.Store) { Background = "#438DD5", Color = "#ffffff", Shape = Shape.Cylinder });

            styles.Add(new ElementStyle(CustomTags.Event)
            {
                Background = "#85BBF0",
                Color = "#444444",
                Shape = Shape.RoundedBox,
                Width = 690
            });

            styles.Add(new ElementStyle(CustomTags.Command)
            {
                Background = "#85BBF0",
                Color = "#444444",
                Shape = Shape.RoundedBox,
                Width = 690
            });

            styles.Add(new RelationshipStyle(Tags.Asynchronous) { Dashed = true });
            styles.Add(new RelationshipStyle(Tags.Synchronous) { Dashed = false });

            styles.Add(new RelationshipStyle(Tags.Relationship) { Routing = Routing.Orthogonal });
            styles.Add(new RelationshipStyle(CustomTags.Direct) { Routing = Routing.Direct });

            styles.Add(new RelationshipStyle(CustomTags.Https) { Color = "#5a9b44" });
            styles.Add(new RelationshipStyle(CustomTags.EntityFramework) { Color = "#9b4473" });
            styles.Add(new RelationshipStyle(CustomTags.SqlStreamStore) { Color = "#448d9b" });
        }

        private static void SetPosition(View view, string id, int x, int y)
        {
            var element = view.Elements.Single(e => e.Id == id);
            element.X = x;
            element.Y = y;
        }

        private static void UploadWorkspaceToStructurizr(
            Workspace workspace,
            long workspaceId,
            string apiKey,
            string apiSecret)
        {
            var structurizrClient = new StructurizrClient(apiKey, apiSecret) { MergeFromRemote = false };
            structurizrClient.PutWorkspace(workspaceId, workspace);
            Console.WriteLine($"Workspace can be viewed at {string.Format(WorkspaceUrlFormat, workspaceId)}");
        }
    }
}
