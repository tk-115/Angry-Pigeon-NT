using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {
    private const int START_ROGATK_AFTER_METERS = 20;
    private const int START_PVO_AFTER_METERS = 100;
    private const int START_PVO_ADD_PROBABILITY_COEFFICIENT = 15;

    public int LengthOfBlock = 25;
    private int _pvo_probability = 35;

    //Система спавна бонусов
    [SerializeField] private BonusSpawner _bonusSpawner;

    #region Pedestrian stuff

    //Пути для пешеходов
    [SerializeField] private List<PedestrianPath> _pedestrianPaths;

    //Все пешеходы в блоке
    private List<Pedestrian> _pedestrians = new List<Pedestrian>();

    //Свойство путей пешеходов
    public List<PedestrianPath> PedestrianPaths => _pedestrianPaths;

    #endregion

    #region Rogatk stuff

    [SerializeField] private List<Transform> _rogatkSpawnPoints;

    private Rogatk _rogatk;

    #endregion

    #region PVO stuff
    
    [SerializeField] private Transform _pvoSpawnPoint;

    public Transform PVOSpawnPoint => _pvoSpawnPoint;

    private PVO _pvo;

    #endregion

    //Добавить нового пешехода в блок
    public void AddInitializedPedestrian(Pedestrian pedestrian) => _pedestrians.Add(pedestrian);

    //Добавить рогатчика в блок
    public void AddInitializedRogatk(Rogatk rogatk) => _rogatk = rogatk;

    //Добавить ПВО в блок
    public void AddInitializedPVO(PVO pvo) => _pvo = pvo;

    //Запуск логики блока - противники начинают свою логику 
    public void StartBlock(int meters) {
        //spawn bonus if system посчитает нужным
        if (_bonusSpawner != null) _bonusSpawner.SpawnBonus();

        //start logic on all available enemys
        if (_pedestrians.Count > 0) foreach (var pedestrian in _pedestrians) pedestrian.StartWalk();

        //Если пройденные метры позволяют работать рогатчику, определить ему позицию и начать ожидание цели
        if (meters >= START_ROGATK_AFTER_METERS) {
            //рандомно определяем позицию рогатчика из доступных
            int rogatkSpawnPointId = Random.Range(0, _rogatkSpawnPoints.Count);
            _rogatk.transform.position = _rogatkSpawnPoints[rogatkSpawnPointId].position;
            _rogatk.SetIDLE();
        } else {
            //отключаем рогатчика 
            _rogatk.gameObject.SetActive(false);
        }

        //Если ПВО есть в этом блоке
        if (_pvo != null) {
            //Отключить его по-умолчанию
            _pvo.gameObject.SetActive(false);
            //Если пройденные метры позволяют работать ПВО, включить его
            if (meters >= START_PVO_AFTER_METERS) {
                int workOrNot = Random.Range(0, 100);
                if (workOrNot < _pvo_probability) {
                    _pvo.gameObject.SetActive(true);
                }
                //Прибавить вероятность спавна ПВО после каждой итерации блока
                if (_pvo_probability < 100) _pvo_probability += START_PVO_ADD_PROBABILITY_COEFFICIENT;
            }
        }
    }

    //Сброс логики блока - противники лечатся для последующего юза
    public void ResetBlock() {
        if (_bonusSpawner != null) _bonusSpawner.Shutdown();

        if (_pedestrians.Count > 0) foreach (var pedestrian in _pedestrians) pedestrian.DamageProcessor.Heal();

        _rogatk.gameObject.SetActive(true);
        _rogatk.DamageProcessor.Heal();
        _rogatk.RogatkShooter.Reload();

        if (_pvo != null) {
            _pvo.Reload();
            _pvo.gameObject.SetActive(false);
        }
    }
}