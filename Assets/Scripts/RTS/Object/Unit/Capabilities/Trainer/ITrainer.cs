using UnityEngine;

namespace RTS.Object.Unit.Capabilities.Trainer
{
    public interface ITrainer
    {
        [SerializeField] public GameObject SpawnPoint { get; }
        [SerializeField] public GameObject RallyPoint { get; set; }
    }
}
