using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TutorialDialogue : MonoBehaviour
{
    public int page = 0;
    [SerializeField] private TMP_Text _textbox;
    [SerializeField] private TMP_Text _textspace;
    [SerializeField] private string[] _text;
    [SerializeField] private bool[] ready;

    [Header("Build Tower")] 
    [SerializeField] private Purchase _purchase;
    [SerializeField] private DiceSocket dSocket1;
    [SerializeField] private DiceSocket dSocket2;
    [SerializeField] private Dice[] dice;
    [SerializeField] private Tower tower;
    [SerializeField] private Transform towerLoc;


    [Header("Place Tower")]
    private Tower t;
    [SerializeField] private Creep enemy;

    [Header("Reroll")]
    [SerializeField] private Dice _die;
    [SerializeField] private Transform rollLoc;

    private void Awake()
    {
        page = 0;
        _textbox.text = _text[page];
        _textspace.enabled = ready[page];
        
        // 34567
        //     10
        //         11
    }

    private void Start()
    {
        //Tower
        dSocket1.valid = new[] { true, true, false, false, false, false };
        dSocket2.valid = new[] { true, true, false, false, false, false };
        dSocket1.open = false;
        dSocket2.open = false;
        _purchase.cost = 2;
        _purchase.validRolls = (1, 2);
        _purchase.UpdateDisplay();
        dice[0].val = 1;
        dice[1].val = 1;
        dice[2].val = 3;
        
    }

    public void OnProceed(InputAction.CallbackContext context)
    {
        if (context.performed && ready[page])
        {
            page++;

            if(page >= ready.Length)
            {
                SceneManager.LoadScene(2);
                return;
            }

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
            case 4:
            {
                //Build Tower
                
                dSocket1.open = true;
                dSocket2.open = true;
                dSocket1.OnPop += dice1 =>
                {
                    t = Instantiate(tower, towerLoc.position, Quaternion.identity);
                    t.gameObject.layer = LayerMask.NameToLayer("Default");
                    ready[4] = true;
                    _textspace.enabled = ready[4];
                    Destroy(dice[0].gameObject);
                    Destroy(dice[1].gameObject);
                };
                break;
            }
            case 5:
            {
                enemy = Instantiate(enemy, Game.path[0], Quaternion.identity);
                enemy.SetVal(6);
                Game.Creeps.Add(enemy);
                t.gameObject.layer = LayerMask.NameToLayer("Tower");
                while (!t.placed)
                {
                    yield return new WaitForFixedUpdate();
                }

                ready[5] = true;
                _textspace.enabled = ready[5];
                break;
            }
            case 6:
            {
                while (t.dice == 0)
                {
                    yield return new WaitForFixedUpdate();
                }
                ready[6] = true;
                _textspace.enabled = ready[6];
                break;
            }
            case 7:
            {
                yield return new WaitForSeconds(1f);
                _textbox.text += ".";
                yield return new WaitForSeconds(1f);
                _textbox.text += ".";
                yield return new WaitForSeconds(1f);
                _textbox.text += ".";
                ready[7] = true;
                _textspace.enabled = ready[7];
                break;
            }
            case 10:
            {
                Dice obj = Instantiate(_die, rollLoc.position, Quaternion.identity);
                obj.transform.localScale = Vector3.one * 0.2f;
                obj.val = 6;
                ready[10] = true;
                _textspace.enabled = ready[10];
                break;
            }
            case 11:
            {
                while (t.dice != 6)
                {
                    yield return new WaitForFixedUpdate();
                }
                ready[11] = true;
                _textspace.enabled = ready[11];

                break;
            }

        }
    }

}
