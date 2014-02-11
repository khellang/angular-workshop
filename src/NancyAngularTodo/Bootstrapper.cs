using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using NancyAngularTodo.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Database.Server;

namespace NancyAngularTodo
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            RegisterSerializer(container);
            RegisterDocumentStore(container);
        }

        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            container.Register(container.Resolve<IDocumentStore>().OpenAsyncSession());
            container.Register<ITodoItemRepository, TodoItemRepository>();
        }

        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            pipelines.AfterRequest.AddItemToEndOfPipeline(async (ctx, ct) =>
            {
                if (ctx.Response.StatusCode != HttpStatusCode.InternalServerError)
                {
                    await container.Resolve<IAsyncDocumentSession>().SaveChangesAsync();
                }
            });
        }

        private static void RegisterDocumentStore(TinyIoCContainer container)
        {
            NonAdminHttp.EnsureCanListenToWhenInNonAdminContext(1337);

            var documentStore = new EmbeddableDocumentStore
            {
                DataDirectory = "~/App_Data/Database",
                UseEmbeddedHttpServer = true
            };

            documentStore.Configuration.Port = 1337;

            container.Register(documentStore.Initialize());
        }

        private static void RegisterSerializer(TinyIoCContainer container)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            container.Register(JsonSerializer.Create(settings));
        }
    }
}