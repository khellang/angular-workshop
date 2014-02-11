using Nancy;

namespace NancyAngularTodo.Modules
{
    public class MainModule : NancyModule
    {
        public MainModule()
        {
            Get["/"] = _ => View["index"];
        }
    }
}