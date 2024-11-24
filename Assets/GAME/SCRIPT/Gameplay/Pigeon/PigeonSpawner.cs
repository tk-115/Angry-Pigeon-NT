using UnityEngine;
using Zenject;

public class PigeonSpawner : MonoBehaviour {
    [Inject] private PigeonFactory _pigeonFactory;

    public Transform SpawnPoint;

    public PigeonMain SpawnPigeonByID(int pigeonID) {
        return _pigeonFactory.GetPigeon(pigeonID, SpawnPoint.position);
    }
}

