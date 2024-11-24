using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelPool : MonoBehaviour {
    public const int BLOCK_LENGHT = 25;
    public const int EACH_BLOCK_POOL_AMOUNT = 4;

    [Inject] private BlockFactory _blockFactory;

    private List<Block> _pooledBlocks;
    private int _uniqueBlocksCount;

    public int UniqueBlocksCount => _uniqueBlocksCount;

    public void Initialize() {
        //Фабрика создает и инициализирует блоки вместе с противниками
        _pooledBlocks = new List<Block>(_blockFactory.GetInitializedBlocks(EACH_BLOCK_POOL_AMOUNT));
        _uniqueBlocksCount = _blockFactory.BlocksFromConfigCount;

        //Отключить в инспекторе все блоки для дальнейшего использования
        foreach (var block in _pooledBlocks) block.gameObject.SetActive(false);
    }

    public void ClearPool() {
        _pooledBlocks.Clear();
        _uniqueBlocksCount = 0;
    }

    public Block GetPooledBlockById(int id) {
        /*
            Eсли EACH_BLOCK_POOL_AMOUNT = 4, расчёты следующие:
            0 * 4; 0 * 0 + 4 = 0 < 4
            1 * 4; 1 * 4 + 4 = 4 < 8
            2 * 4; 2 * 4 + 4 = 8 < 12
            и т. д. 
        */
        int start = id * EACH_BLOCK_POOL_AMOUNT;
        int final = id * EACH_BLOCK_POOL_AMOUNT + EACH_BLOCK_POOL_AMOUNT;
        for (int i = start; i < final; i++) {
            if (_pooledBlocks[i].gameObject.activeInHierarchy == false) {
                _pooledBlocks[i].gameObject.SetActive(true);
                return _pooledBlocks[i];
            }
        }
        return null;
    }
}
