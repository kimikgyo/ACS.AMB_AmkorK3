namespace ACS.RobotMap
{
    public interface IMapReadService
    {
        string MapGuid { get; set; }
        string MapName { get; set; }
        void Start();
        void Stop();
    }
}
