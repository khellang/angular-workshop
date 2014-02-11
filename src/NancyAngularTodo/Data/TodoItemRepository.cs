using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client;

namespace NancyAngularTodo.Data
{
    public class TodoItemRepository : ITodoItemRepository
    {
        private readonly IAsyncDocumentSession _session;

        public TodoItemRepository(IAsyncDocumentSession session)
        {
            _session = session;
        }

        public Task<IList<TodoItem>>  GetAll()
        {
            return _session.Query<TodoItem>().ToListAsync();
        }

        public Task<TodoItem> GetById(string id)
        {
            return _session.LoadAsync<TodoItem>(id);
        }

        public Task Store(TodoItem item)
        {
            return _session.StoreAsync(item);
        }

        public void Delete(TodoItem item)
        {
            _session.Delete(item);
        }
    }
}