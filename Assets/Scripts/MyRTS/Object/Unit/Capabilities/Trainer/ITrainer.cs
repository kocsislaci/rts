using UnityEngine;

namespace MyRTS.Object.Unit.Capabilities.Trainer
{
    public interface ITrainer
    {
        [SerializeField] public GameObject SpawnPoint { get; }
        [SerializeField] public GameObject RallyPoint { get; set; }
    }
}
