using UnityEngine.Events;

namespace MyRTS.Object.Unit.Capabilities.Building
{
    public interface IHarvestReceiver
    {
        public UnityEvent<IHarvestReceiver> BecomeUnavailable { get; set; }
        public void ReceiveHarvest();
    }
}
