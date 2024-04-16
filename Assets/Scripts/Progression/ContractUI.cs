using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ContractSystem;
using static UnityEngine.Rendering.DebugUI;

public class ContractUI : MonoBehaviour
{
    public TMP_Dropdown conDropdown;

    public TMP_Dropdown difDropdown;

    public List<ContractSystem.Contract> contracts;

    public TMP_Text Goal;
    public TMP_Text Time;
    public TMP_Text Reward;
    public TMP_Text Punishment;
    public TMP_Text Info;
    public TMP_Text AmountActive;

    public void StartContract()
    {
        ContractSystem.Contract contract = contracts[conDropdown.value];
        
        Contract newContract = new Contract();
        newContract.type = contract.type;
        newContract.difficulty = (Difficulty)difDropdown.value;
        newContract.baseReward = contract.baseReward;
        newContract.basePenalty = contract.basePenalty;
        newContract.difficultyMultipliers = contract.difficultyMultipliers;
        float multi = contract.difficultyMultipliers[difDropdown.value];
        newContract.timeLimit = contract.timeLimit / multi;
        newContract.isCompleted = contract.isCompleted;
        newContract.isFailed = contract.isFailed;
        newContract.elapsedTime = contract.elapsedTime;
        newContract.goalAmount = contract.goalAmount;
        newContract.progressTracker = contract.progressTracker;
        newContract.info = contract.info;

    ContractSystem.contracts.Add(newContract);
        
    }


    private void Update()
    {
        ContractSystem.Contract contract = contracts[conDropdown.value];
    
    float multi = contract.difficultyMultipliers[difDropdown.value];
        Goal.text = (contract.goalAmount * multi).ToString();
        Time.text = (contract.timeLimit / multi).ToString();
        Reward.text = (contract.baseReward * multi).ToString();
    Punishment.text = (contract.basePenalty * multi).ToString();
        Info.text = contract.info;

        int count = 0;
        foreach (Contract contractCurr in ContractSystem.contracts)
        {
            if (!contractCurr.isCompleted && !contractCurr.isFailed)
            {
                ++count;
            }
        }

        AmountActive.text = "Current active contracts: " + count.ToString();


    }
}
