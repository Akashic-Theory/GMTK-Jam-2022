using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialDialogue : MonoBehaviour
{
    public int page = 0;
    [SerializeField] private TMP_Text _textbox;
    [SerializeField] private TMP_Text _textspace;
    [SerializeField] private string[] _text;
    [SerializeField] private bool[] ready;

    [Header("Build Tower")] 
    [SerializeField] private DiceSocket dSocket1;
    [SerializeField] private DiceSocket dSocket2;
    [SerializeField] private DiceRoller roller;
    [SerializeField] private Transform[] diceSpawnLoc;

    private void Awake()
    {
        page = 0;
        _textbox.text = _text[page];
        _textspace.enabled = ready[page];
        
        // 34567
        //     10
        //         11
    }

    public void OnProceed(InputAction.CallbackContext context)
    {
        if (context.performed && ready[page])
        {
            page++;
            _textbox.text = _text[page];
            _textspace.enabled = ready[page];
            if (!ready[page])
            {
                StartCoroutine(HandleReqs(page));
            }
        }
    }

    private IEnumerator HandleReqs(int stage)
    {
        switch (stage)
        {
            case 3:
            {
                yield return new WaitForSeconds(3f);
                ready[3] = true;
                _textspace.enabled = ready[3];
                break;
            }
            case 4:
            {
                //Build Tower
                yield return new WaitForSeconds(3f);
                ready[3] = true;
                _textspace.enabled = ready[3];
                break;
            }
            
        }
    }
}
