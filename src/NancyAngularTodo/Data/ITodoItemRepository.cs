using System.Collections.Generic;
using System.Threading.Tasks;

namespace NancyAngularTodo.Data
{
    public interface ITodoItemRepository
    {
        Task<IList<TodoItem>> GetAll();

        Task<TodoItem> GetById(string id);

        Task Store(TodoItem item);

        void Delete(TodoItem item);
    }
}