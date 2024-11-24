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

    //��������������� ����� � ��������������� ������
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
            //������ ���� ������ ������ ���� ID = 0 ��� ���������������� ������ ������ 
            //� ���������� ��� ������ ����� 
            Block block;
            if (i == 1) block = _levelPool.GetPooledBlockById(0); 
            else block = _levelPool.GetPooledBlockById(Random.Range(0, _levelPool.UniqueBlocksCount));

            //���������� ������ ���
            block.StartBlock(0);
            if (block != null)  {
                float positionX = _startPointForLevelGenerate.position.x + i * block.LengthOfBlock;
                block.transform.position = new Vector3(positionX, _startPointForLevelGenerate.position.y, _startPointForLevelGenerate.position.z);
                _currentLevelBlocks.Add(block);
            }
        }
        //� ��� ����� �������� �������� ��������� �������� ����� ������
        //�� ��� �������� �������� BlocksUpdate
        if (_blocksUpdateCoroutine != null) StopCoroutine(_blocksUpdateCoroutine);
        _blocksUpdateCoroutine = StartCoroutine(BlocksUpdate());

        //��� ��� � �������� ������� ����� ����� ������ �� ���� ����������, ������� ������ � ���������� �� ������� �� �������
        //����� ������� ��� �������
        //��� ����� ��������� �������� ���������� ������� ������
        if (_blocksMoveCoroutine != null) StopCoroutine(_blocksMoveCoroutine);
        _blocksMoveCoroutine = StartCoroutine(BlocksMove());

        //��������� �������� �� ��������
        if (_blocksSpeedupCoroutine != null) StopCoroutine(_blocksSpeedupCoroutine);
        _blocksSpeedupCoroutine = StartCoroutine(SpeedUpBlockMove());

        //������ ���������� ������
        if (_metersCounterCoroutine != null) StopCoroutine(_metersCounterCoroutine);
        _metersCounterCoroutine = StartCoroutine(MeterCounter());
    }

    #region Bonus Slow Down Logic

    public void BonusSlowDownLogic() {
        //���������� �������� �� �������� �������� �� ���� ���������
        float newSpeed = _currentMoveSpeed - BONUS_SLOWDOWN_SPEED;
        if (newSpeed < START_MOVE_SPEED) return;
        if (_slowDownBlocksCoroutine != null) StopCoroutine(_slowDownBlocksCoroutine);
        _slowDownBlocksCoroutine = StartCoroutine(SlowDownBlocksMove(newSpeed));
    }

    IEnumerator SlowDownBlocksMove(float newSpeed) {
        while (true) {
            _currentMoveSpeed -= BLOCK_SPEED_SLOWDOWN_COEFFICIENT;
            if (_currentMoveSpeed <= newSpeed) {
                _currentMoveSpeed = newSpeed;    //��������� ������ ��������
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
        //���������, ���� ����������� �������� ��������� ��������
        if (_slowDownBlocksCoroutine != null) StopCoroutine(_slowDownBlocksCoroutine);

        //������������ ������� ��������
        if (_speedUpBlocksCorounite != null) StopCoroutine(_speedUpBlocksCorounite);
        _speedUpBlocksCorounite = StartCoroutine(RestoreSpeed());

        //��������� �������� ���������� ��������
        if (_blocksSpeedupCoroutine != null) StopCoroutine(_blocksSpeedupCoroutine);
        _blocksSpeedupCoroutine = StartCoroutine(SpeedUpBlockMove());

        //��������� �������� �������� ������
        if (_metersCounterCoroutine != null) StopCoroutine(_metersCounterCoroutine);
        _metersCounterCoroutine = StartCoroutine(MeterCounter());
    }

    IEnumerator RestoreSpeed() {
        while (true) {
            _currentMoveSpeed += BLOCK_SPEED_SLOWDOWN_COEFFICIENT;
            if (_currentMoveSpeed >= _prevMoveSpeed) {
                _currentMoveSpeed = _prevMoveSpeed;    //��������� ������ ��������
                break;
            }
            yield return new WaitForSeconds(BLOCK_SPEED_SLOWDOWN_COEFFICIENT);
        }
    }

    #endregion

    #region Common

    // �������� ����������� ����������� ��������
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

    // �������� �������� ������ ����� � ��������� ��� � ����� ��� ���������
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

                //����� ��������� �������� ������ �����, ����������� ����� ������
                _voronaSpawner.SpawnVorona(_scoresControll.GetMeter());

                //���������� �������� ������ � ������ �������� ��������� ������ �����
                break;
            }
            //����������� ���� ����� ���� �����, ��������� ����� ��� ���, ����������� ���������� �������
            else {
                Debug.Log("Needed block busy, attempt to another. Attempt = " + attempt);
                attempt++;
            }
            yield return null;
        }
    }

    // �������� ���������� ������ - �������� �� ������ ������ � ��������� �����
    IEnumerator BlocksUpdate() {
        while (true) {
            //���� ������ ���� ������ ���� ������, ��� ������, ����� �������� � �����
            if (_currentLevelBlocks[0].transform.position.x < -25f) {
                if (_generateNewBlockCoroutine == null) {
                    //��������������� ����������� ��������� ����������� � ����� 
                    _currentLevelBlocks[0].ResetBlock();
                    //��������� ���� � ���
                    _currentLevelBlocks[0].gameObject.SetActive(false);
                    //������ ���� ������ �� ���������� �� ������� ����
                    _currentLevelBlocks.RemoveAt(0);
                    //����� ����� ���� �� ���� � ��������� ��� �� ������� ���� � �����
                    _generateNewBlockCoroutine = StartCoroutine(GenerateNewBlock()); 
                }
            }
            yield return null;
        }
    }

    #endregion
}