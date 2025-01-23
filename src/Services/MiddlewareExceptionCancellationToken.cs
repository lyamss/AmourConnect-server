using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Services
{
    
    
    [AttributeUsage(AttributeTargets.Class)]


    internal sealed class MiddlewareExceptionCancellationToken : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {

            context.ExceptionHandled = true;


            Console.ForegroundColor = ConsoleColor.DarkGreen;

            switch (context.Exception)
            {
                case TaskCanceledException canceledException:
                    Console.WriteLine("Task Canceled was canceled: " + canceledException.Message);
                    break;

                case OperationCanceledException canceledOperationException:
                    Console.WriteLine("Operation was canceled: " + canceledOperationException.Message);
                    break;

                default:
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("aie Internal Server 500 => " + context.Exception.Message);
                    Console.ResetColor();
                    break;
            }
        }
    }
}