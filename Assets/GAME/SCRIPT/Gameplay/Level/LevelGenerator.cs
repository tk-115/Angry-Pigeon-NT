using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {
    private const float MOVE_SPEED_LIMIT = 10f;
    private const float START_MOVE_SPEED = 4f;
    private const float SPEED_INCREASE_COEFFICIENT = .03f;
    private const float SPEED_INCREASE_EVERY_SEC = 5;
    private const int MAX_BLOCKS_IN_WORLD = 3;
    private const float BLOCK_SPEED_SLOWDOWN_COEFFICIENT = .01f;
    private const float BONUS_SLOWDOWN_SPEED = 1.3f;

    [SerializeField] private Transform _startPointForLevelGenerate;
    [SerializeField] private LevelPool _levelPool;
    [SerializeField] private VoronaSpawner _voronaSpawner;

    private float _currentMoveSpeed;
    private float _prevMoveSpeed;

    private Coroutine _blocksUpdateCoroutine;
    private Coroutine _generateNewBlockCoroutine;
    private Coroutine _blocksMoveCoroutine;
    private Coroutine _blocksSpeedupCoroutine;
    private Coroutine _metersCounterCoroutine;
    private Coroutine _slowDownBlocksCoroutine;
    private Coroutine _speedUpBlocksCorounite;

    private ScoresControll _scoresControll;

    //Задействованные блоки в сгенерированном уровне
    private List<Block> _currentLevelBlocks = new List<Block>();

    public void Initialize(ScoresControll scoresControll) {
        _levelPool.Initialize();
        _scoresControll = scoresControll;
    }

    #region Clearing

    public void ClearAll() {
        foreach (var block in _currentLevelBlocks) {
            block.ResetBlock();
            block.gameObject.SetActive(false);
        }
        _currentLevelBlocks.Clear();
    }

    public void ShutdownGameplay() {
        if (_blocksUpdateCoroutine != null) StopCoroutine(_blocksUpdateCoroutine);
        if (_blocksMoveCoroutine != null) StopCoroutine(_blocksMoveCoroutine);
        if (_blocksSpeedupCoroutine != null) StopCoroutine(_blocksSpeedupCoroutine);
        if (_metersCounterCoroutine != null) StopCoroutine(_metersCounterCoroutine);
        if (_slowDownBlocksCoroutine != null) StopCoroutine(_slowDownBlocksCoroutine);
        if (_speedUpBlocksCorounite != null) StopCoroutine(_speedUpBlocksCorounite);
        ClearAll();
    }

    #endregion

    public void RebuildLevel() {
        ShutdownGameplay();
        SetupLevel();
    }

    public void SetupLevel() {
        _currentMoveSpeed = _prevMoveSpeed = START_MOVE_SPEED;

        _scoresControll.ResetCoin();
        _scoresControll.ResetMeters();
        _scoresControll.ResetScores();

        for (int i = 1; i < MAX_BLOCKS_IN_WORLD + 1; i++) {
            //Первый блок всегда должен быть ID = 0 для гарантированного спавна голубя 
            //в безопасном для игрока месте 
            Block block;
            if (i == 1) block = _levelPool.GetPooledBlockById(0); 
            else block = _levelPool.GetPooledBlockById(Random.Range(0, _levelPool.UniqueBlocksCount));

            //Пройденных метров нет
            block.StartBlock(0);
            if (block != null)  {
                float positionX = _startPointForLevelGenerate.position.x + i * block.LengthOfBlock;
                block.transform.position = new Vector3(positionX, _startPointForLevelGenerate.position.y, _startPointForLevelGenerate.position.z);
                _currentLevelBlocks.Add(block);
            }
        }
        //и уже далее рандомно начинает генерится основная часть уровня
        //за что отвечает корутина BlocksUpdate
        if (_blocksUpdateCoroutine != null) StopCoroutine(_blocksUpdateCoroutine);
        _blocksUpdateCoroutine = StartCoroutine(BlocksUpdate());

        //Так как в процессе геймлея игрок может лететь по сути бесконечно, двигать камеру в координаты на миллион не вариант
        //Нужно двигать сам уровень
        //Для этого запускаем корутину обновления позиции блоков
        if (_blocksMoveCoroutine != null) StopCoroutine(_blocksMoveCoroutine);
        _blocksMoveCoroutine = StartCoroutine(BlocksMove());

        //Ускорение движения со временем
        if (_blocksSpeedupCoroutine != null) StopCoroutine(_blocksSpeedupCoroutine);
        _blocksSpeedupCoroutine = StartCoroutine(SpeedUpBlockMove());

        //Посчет пройденных метров
        if (_metersCounterCoroutine != null) StopCoroutine(_metersCounterCoroutine);
        _metersCounterCoroutine = StartCoroutine(MeterCounter());
    }

    #region Bonus Slow Down Logic

    public void BonusSlowDownLogic() {
        //Замедление возможно на значение скорости не ниже стартовой
        float newSpeed = _currentMoveSpeed - BONUS_SLOWDOWN_SPEED;
        if (newSpeed < START_MOVE_SPEED) return;
        if (_slowDownBlocksCoroutine != null) StopCoroutine(_slowDownBlocksCoroutine);
        _slowDownBlocksCoroutine = StartCoroutine(SlowDownBlocksMove(newSpeed));
    }

    IEnumerator SlowDownBlocksMove(float newSpeed) {
        while (true) {
            _currentMoveSpeed -= BLOCK_SPEED_SLOWDOWN_COEFFICIENT;
            if (_currentMoveSpeed <= newSpeed) {
                _currentMoveSpeed = newSpeed;    //закрепить точное значение
                break;
            }
            yield return new WaitForSeconds(BLOCK_SPEED_SLOWDOWN_COEFFICIENT);
        }
    }

    #endregion

    #region GameOver Logic

    public void GameOverLogic() {
        _prevMoveSpeed = _currentMoveSpeed;
        if (_slowDownBlocksCoroutine != null) StopCoroutine(_slowDownBlocksCoroutine);
        _slowDownBlocksCoroutine = StartCoroutine(SlowDownBlockToZero());
    }

    IEnumerator SlowDownBlockToZero() {
        while (true) {
            _currentMoveSpeed -= BLOCK_SPEED_SLOWDOWN_COEFFICIENT;
            if (_currentMoveSpeed <= 0) {
                _currentMoveSpeed = 0;
                if (_blocksSpeedupCoroutine != null) StopCoroutine(_blocksSpeedupCoroutine);
                if (_metersCounterCoroutine != null) StopCoroutine(_metersCounterCoroutine);
                break;
            }

            yield return new WaitForSeconds(BLOCK_SPEED_SLOWDOWN_COEFFICIENT);
        }
    }

    #endregion

    #region Resurrection Logic

    public void ResurrectionLogic() {
        //завершить, если выполняется корутину сбавления скорости
        if (_slowDownBlocksCoroutine != null) StopCoroutine(_slowDownBlocksCoroutine);

        //восстановить прошлую скорость
        if (_speedUpBlocksCorounite != null) StopCoroutine(_speedUpBlocksCorounite);
        _speedUpBlocksCorounite = StartCoroutine(RestoreSpeed());

        //запустить корутину увеличения скорости
        if (_blocksSpeedupCoroutine != null) StopCoroutine(_blocksSpeedupCoroutine);
        _blocksSpeedupCoroutine = StartCoroutine(SpeedUpBlockMove());

        //запустить корутину подсчета метров
        if (_metersCounterCoroutine != null) StopCoroutine(_metersCounterCoroutine);
        _metersCounterCoroutine = StartCoroutine(MeterCounter());
    }

    IEnumerator RestoreSpeed() {
        while (true) {
            _currentMoveSpeed += BLOCK_SPEED_SLOWDOWN_COEFFICIENT;
            if (_currentMoveSpeed >= _prevMoveSpeed) {
                _currentMoveSpeed = _prevMoveSpeed;    //закрепить точное значение
                break;
            }
            yield return new WaitForSeconds(BLOCK_SPEED_SLOWDOWN_COEFFICIENT);
        }
    }

    #endregion

    #region Common

    // Корутина постоянного прибавления скорости
    IEnumerator SpeedUpBlockMove() {
        while (true) {
            yield return new WaitForSeconds(SPEED_INCREASE_EVERY_SEC);
            if (_currentMoveSpeed < MOVE_SPEED_LIMIT) _currentMoveSpeed += SPEED_INCREASE_COEFFICIENT;
        }
    }

    IEnumerator BlocksMove() {
        while (true) {
            foreach (var block in _currentLevelBlocks) 
                block.transform.Translate(Vector3.left * Time.deltaTime * _currentMoveSpeed);
            yield return null;
        }
    }

    IEnumerator MeterCounter() {
        while (true) {
            _scoresControll.AddMeter();
            yield return new WaitForSeconds(.5f);
        }
    }

    // Корутина создания нового блока и помещение его в конец уже созданных
    IEnumerator GenerateNewBlock() {
        int attempt = 1;
        while (true) {
            Block newBlock = _levelPool.GetPooledBlockById(Random.Range(0, _levelPool.UniqueBlocksCount));
            newBlock.StartBlock(_scoresControll.GetMeter());

            if (newBlock != null) {
                Vector3 positionOfNewBlock = _currentLevelBlocks[_currentLevelBlocks.Count - 1].transform.position;
                float positionXmodified = positionOfNewBlock.x + _currentLevelBlocks[_currentLevelBlocks.Count - 1].LengthOfBlock;
                newBlock.transform.position = new Vector3(positionXmodified, positionOfNewBlock.y, positionOfNewBlock.z);
                newBlock.gameObject.SetActive(true);
                _currentLevelBlocks.Add(newBlock);

                //После успешного респавна нового блока, запрашиваем спавн вороны
                _voronaSpawner.SpawnVorona(_scoresControll.GetMeter());

                //Завершение корутины только в случаи успешной генерации нового блока
                break;
            }
            //Запрошенный блок может быть занят, пробовать взять ещё раз, подсчитывая количество попыток
            else {
                Debug.Log("Needed block busy, attempt to another. Attempt = " + attempt);
                attempt++;
            }
            yield return null;
        }
    }

    // Корутина обновления уровня - удаление не нужных блоков и генерация новых
    IEnumerator BlocksUpdate() {
        while (true) {
            //Если первый блок достиг края экрана, его убрать, новый добавить в конец
            if (_currentLevelBlocks[0].transform.position.x < -25f) {
                if (_generateNewBlockCoroutine == null) {
                    //восстанавливаем изначальные параметры противников в блоке 
                    _currentLevelBlocks[0].ResetBlock();
                    //поместить блок в пул
                    _currentLevelBlocks[0].gameObject.SetActive(false);
                    //данный блок больше не учавствует на игровом поле
                    _currentLevelBlocks.RemoveAt(0);
                    //Взять новый блок из пула и поместить его на игровое поле в конец
                    _generateNewBlockCoroutine = StartCoroutine(GenerateNewBlock()); 
                }
            }
            yield return null;
        }
    }

    #endregion
}