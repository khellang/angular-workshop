using Nancy;
using Nancy.ModelBinding;
using NancyAngularTodo.Data;

namespace NancyAngularTodo.Modules
{
    public class TodoModule : NancyModule
    {
        public TodoModule(ITodoItemRepository repository) : base("/todo")
        {
            Get["/", true] = async (_, ct) => await repository.GetAll();

            Post["/", true] = async (_, ct) =>
            {
                var item = this.Bind<TodoItem>();

                await repository.Store(item);

                return Negotiate.WithModel(item).WithStatusCode(HttpStatusCode.Created);
            };

            Put["/{id*}", true] = async (args, ct) =>
            {
                string id = args.id;

                var item = await repository.GetById(id);
                if (item == null)
                {
                    return HttpStatusCode.NotFound;
                }

                var newItem = this.Bind<TodoItem>();

                item.Title = newItem.Title;
                item.IsCompleted = newItem.IsCompleted;

                return item;
            };

            Delete["/{id*}", true] = async (args, ct) =>
            {
                string id = args.id;

                var item = await repository.GetById(id);
                if (item == null)
                {
                    return HttpStatusCode.NotFound;
                }

                repository.Delete(item);

                return HttpStatusCode.OK;
            };
        }
    }
}