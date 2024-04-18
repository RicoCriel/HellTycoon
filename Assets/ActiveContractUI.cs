using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using TMPro;
using UnityEngine;
using static ContractSystem;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class ActiveContractUI : MonoBehaviour
{


    public TMP_Text Goal;
    public TMP_Text Time;
    public TMP_Text Reward;
    public TMP_Text Punishment;
    public TMP_Text Info;
    public TMP_Text Type;
    public TMP_Text Difficulty;

    [SerializeField] private GameObject UI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ContractSystem.contracts.Count > 0)
        {
            UI.SetActive(true);
            ContractSystem.Contract contract = ContractSystem.contracts[0];

            float multi = contract.difficultyMultipliers[(int)contract.difficulty];
            Type.text = contract.type.ToString();
            Difficulty.text = contract.difficulty.ToString();
            Goal.text = (contract.goalAmount * multi).ToString();
            Time.text = (contract.timeLimit / multi).ToString();
            Reward.text = (contract.baseReward * multi).ToString();
            Punishment.text = (contract.basePenalty * multi).ToString();
            Info.text = contract.info;
        }
        else
        {
            UI.SetActive(false);

        }
    }
}
