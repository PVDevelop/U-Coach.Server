namespace PVDevelop.UCoach.Server.Mapper
{
    public interface IMapper
    {
        TDest Map<TDest>(object source);
    }
}
