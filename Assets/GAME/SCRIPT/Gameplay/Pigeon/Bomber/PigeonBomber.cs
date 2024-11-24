using UnityEngine;

public class PigeonBomber : MonoBehaviour {
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private PigeonView _pigeonView;
    private float _cooldown;
    private BombPool _bombPool;

    public float CoolDown => _cooldown;

    public void Initialize(Bomb bomb) {
        //Запускаем пул бомб и иницилизируем нужное количество нужного типа бомб
        _bombPool = new BombPool(bomb, _spawnPoint);
        _cooldown = bomb.CoolDown;
    }

    public void DropBomb() {
        Bomb bomb = _bombPool.GetBomb(true);
        bomb.transform.SetPositionAndRotation(_spawnPoint.position, Quaternion.identity);

        if (bomb != null) {
            _pigeonView.BombSFXPlay();
            bomb.Drop();
        }
    }
}