namespace EM.Cloud.Models.View
{
    public class InvokationDisplayViewModel : SmartSolucionesCuba.SAPRESSC.Core.Persistence.Models.IEntityDisplayModel<Persistence.Entities.Invokation, string>
    {
        public string Id { get; set; }

        public System.DateTime At { get; set; }

        public string Session { get; set; }

        public string NetAddress { get; set; }

        public string RequestMethod { get; set; }

        public string RequestPath { get; set; }

        public string RouteArea { get; set; }

        public string RouteController { get; set; }

        public string RouteAction { get; set; }

        public void Import(Persistence.Entities.Invokation entity)
        {
            Id = entity.Id;
            At = entity.At;
            Session = entity.Session;
            NetAddress = entity.NetAddress;
            RequestMethod = entity.RequestMethod;
            RequestPath = entity.RequestPath;
            RouteArea = entity.RouteArea;
            RouteController = entity.RouteController;
            RouteAction = entity.RouteAction;
        }
    }
}
