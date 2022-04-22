using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public Block[] blockArray;

    [SerializeField] int m_blockCount;

    private void Start()
    {
        blockArray = FindObjectsOfType<Block>();
        m_blockCount = blockArray.Length;
        SubscribeToEvent();
    }

    void SubscribeToEvent()
    {
        foreach (Block block in blockArray)
        {
            block.OnBeingHit += DecreaseBlockCount;
        }

        FindObjectOfType<PlayerController>().OnMouseClick += ResetAllBlocks;
    }

    void DecreaseBlockCount()
    {
        m_blockCount--;
    }

    void ResetAllBlocks()
    {
        foreach (Block block in blockArray)
        {
            if (block.gameObject.activeSelf == false)
            {
                block.gameObject.SetActive(true);
            }

            m_blockCount = blockArray.Length;
        }
    }
}
