namespace IntegrationEvents.Plates
{
    public class ReservePlateEvent : IntegrationEvent
    {
        public Guid PlateId { get; private set; }

        public ReservePlateEvent(Guid plateId)
        {
            PlateId = plateId;
        }
    }
}