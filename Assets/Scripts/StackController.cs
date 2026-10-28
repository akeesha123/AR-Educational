
using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class StackController : MonoBehaviour
{
    [Header("Stack Settings")]
    public GameObject blockPrefab;
    public GameObject stackModel;
    public int maxBlocks = 5;
    public float blockHeight = 0.25f;

    [Header("UI")]
    public TMP_Text counterText;
    public TMP_Text explanationText;
    public TMP_Text codeDisplayText;

    private Stack<GameObject> blocks = new Stack<GameObject>();

    private void Start()
    {
        UpdateCounter();
        ShowDefaultCode();
    }

    public void Push()
    {
        Debug.Log("PUSH BUTTON WORKS!");

        if (blockPrefab == null)
        {
            SetExplanation("Please assign Block Prefab.");
            Debug.LogError("Block Prefab is not assigned!");
            return;
        }

        if (blocks.Count >= maxBlocks)
        {
            SetExplanation("Stack is full! Maximum size is " + maxBlocks + ".");
            return;
        }

        Vector3 basePosition = stackModel != null
            ? stackModel.transform.position
            : Vector3.zero;

        Quaternion baseRotation = stackModel != null
            ? stackModel.transform.rotation
            : Quaternion.identity;

        Vector3 targetPosition = basePosition +
            Vector3.up * (0.5f + blocks.Count * blockHeight);

        Vector3 startPosition = targetPosition + Vector3.up * 0.5f;

        GameObject block = Instantiate(
            blockPrefab,
            startPosition,
            baseRotation
        );

        block.transform.localScale = new Vector3(
            0.4f,
            blockHeight,
            0.4f
        );

        blocks.Push(block);

        StartCoroutine(
            MoveBlock(block.transform, startPosition, targetPosition)
        );

        SetExplanation("PUSH: Added a block to the top of the stack.");
        UpdateCounter();
        ShowOperationCode("PUSH");
    }

    public void Pop()
    {
        Debug.Log("POP BUTTON WORKS!");

        if (blocks.Count == 0)
        {
            SetExplanation("Stack is empty! Nothing to remove.");
            ShowOperationCode("POP");
            return;
        }

        GameObject block = blocks.Pop();

        if (block != null)
        {
            Destroy(block);
        }

        SetExplanation("POP: Removed the top block from the stack.");
        UpdateCounter();
        ShowOperationCode("POP");
    }

    private IEnumerator MoveBlock(
        Transform block,
        Vector3 startPosition,
        Vector3 targetPosition)
    {
        float duration = 0.4f;
        float elapsed = 0f;

        while (elapsed < duration && block != null)
        {
            elapsed += Time.deltaTime;

            float progress = Mathf.Clamp01(elapsed / duration);

            block.position = Vector3.Lerp(
                startPosition,
                targetPosition,
                progress
            );

            yield return null;
        }

        if (block != null)
        {
            block.position = targetPosition;
        }
    }

    private void UpdateCounter()
    {
        if (counterText != null)
        {
            counterText.text = "Stack Size: " + blocks.Count;
        }
    }

    private void SetExplanation(string message)
    {
        if (explanationText != null)
        {
            explanationText.text = message;
        }
    }

    private void ShowDefaultCode()
    {
        if (codeDisplayText != null)
        {
            codeDisplayText.text =
                "<b>STACK ALGORITHM</b>\n\n" +
                "PUSH(item):\n" +
                "    if stack is full:\n" +
                "        show Stack Full\n" +
                "    else:\n" +
                "        add item to top\n\n" +
                "POP():\n" +
                "    if stack is empty:\n" +
                "        show Stack Empty\n" +
                "    else:\n" +
                "        remove top item";
        }
    }

    private void ShowOperationCode(string operation)
    {
        if (codeDisplayText == null)
            return;

        string pushCode =
            "<b>STACK ALGORITHM</b>\n\n" +
            "<color=#FFD54F>PUSH(item):</color>\n" +
            "    if stack is full:\n" +
            "        show Stack Full\n" +
            "    else:\n" +
            "        add item to top\n\n";

        string popCode =
            "<color=#FFD54F>POP():</color>\n" +
            "    if stack is empty:\n" +
            "        show Stack Empty\n" +
            "    else:\n" +
            "        remove top item";

        codeDisplayText.text = pushCode + popCode;
    }
}
