using UnityEngine.Events;

namespace RTS.Object.Unit.Capabilities.Building
{
    public interface IHarvestReceiver
    {
        public UnityEvent<IHarvestReceiver> BecomeUnavailable { get; set; }
        public void ReceiveHarvest();
    }
}
