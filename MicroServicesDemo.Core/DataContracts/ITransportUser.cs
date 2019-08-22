namespace MicroServicesDemo.DataContracts
{
    public interface ITransportUser
    {
        string Username { get; set; }
        string Bio { get; set; }
        string Image { get; set; }
    }

    public interface IBlogTransportUser: ITransportUser
    {
        bool Following { get; set; }
    }

    public class TransportUser: ITransportUser
    {
        public string Username { get; set; }
        public string Bio { get; set; }
        public string Image { get; set; }
    }

    public class BlogTransportUser: TransportUser, IBlogTransportUser
    {
        public bool Following { get; set; }
    }
}
