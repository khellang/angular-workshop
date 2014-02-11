using System;
using Nancy;

namespace NancyAngularTodo.Modules
{
    public class DemoModule : NancyModule
    {
        public DemoModule() : base("/demo")
        {
            Get["/"] = _ =>
            {
                throw new NotImplementedException();
            };
        }
    }
}