namespace Heartbeat_Api.Interfaces
{
    public interface ICalculationService
    {
        int TotalRegisteredAgents();
        int TotalUnRegisteredAgents();
        int TotalHeartbeatsPerAgents(int id);

        void SetHeartbeatsForAgents(int id);
        void AddRegisteredAgents(int id);
        void RemoveRegisteredAgents(int id);
        public void AddUnRegisteredAgents();


    }
}
