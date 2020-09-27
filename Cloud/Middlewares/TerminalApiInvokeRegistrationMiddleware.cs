using Microsoft.AspNetCore.Routing;
using SmartSolucionesCuba.SAPRESSC.Core.Persistence.Repositories;

namespace EM.Cloud.Middlewares
{
    public class TerminalApiInvokeRegistrationMiddleware : Microsoft.AspNetCore.Http.IMiddleware
    {
        private readonly IEntityRepository<Persistence.Entities.Terminal, string> terminalRepository;
        private readonly IEntityRepository<Persistence.Entities.Invokation, string> invokationRepository;

        public TerminalApiInvokeRegistrationMiddleware(IEntityRepository<Persistence.Entities.Terminal, string> terminalRepository, IEntityRepository<Persistence.Entities.Invokation, string> invokationRepository)
        {
            this.terminalRepository = terminalRepository;
            this.invokationRepository = invokationRepository;
        }

        public async System.Threading.Tasks.Task InvokeAsync(Microsoft.AspNetCore.Http.HttpContext context, Microsoft.AspNetCore.Http.RequestDelegate next)
        {
            var termialId = context.Request.Headers[Api.Common.Providers.ConstantProvider.TERMINAL_ID_HEADER_KEY];

            if (termialId.Count == 1)
            {
                var route = context.GetRouteData();

                var invokation = new Persistence.Entities.Invokation
                {
                    Id = context.TraceIdentifier,
                    NetAddress = context.Connection.RemoteIpAddress.ToString(),
                    Session = context.Session.Id,
                    RequestMethod = context.Request.Method,
                    RequestPath = context.Request.Path,
                    RouteArea = route?.Values["area"].ToString(),
                    RouteAction = route?.Values["controller"].ToString(),
                    RouteController = route?.Values["action"].ToString(),
                    Terminal = terminalRepository.Get(termialId)
                };

                invokationRepository.Add(invokation, "system");
            }

            await next(context);
        }
    }
}
