using Economy;
using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class ContractSystem : MonoBehaviour
{
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }

    public enum ContractType
    {
        SellCon,
        TortureCon,
        EarnCon
        // Add more contract types as needed
    }

    [System.Serializable]
    public class Contract
    {
        public ContractType type;
        public Difficulty difficulty;
        public int baseReward;
        public int basePenalty;
        public float[] difficultyMultipliers;
        public float timeLimit; 
        public bool isCompleted;
        public bool isFailed;
        public float elapsedTime;
        public int goalAmount;
        public int progressTracker;

    }

    [SerializeField] private Contract[] initContracts;
    static public Contract[] contracts;

    private SoulManager _soulManager;


    private void Start()
    {
        _soulManager = FindObjectOfType<SoulManager>();
        contracts = initContracts;
    }

    void Update()
    {
        foreach (Contract contract in contracts)
        {
            if (!contract.isCompleted && !contract.isFailed)
            {
                contract.elapsedTime += Time.deltaTime;

                // Check if the time limit is exceeded
                if (contract.elapsedTime > contract.timeLimit)
                {
                    // Contract failed due to exceeding time limit
                    contract.isFailed = true;
                    int punishment = Mathf.RoundToInt(contract.basePenalty * contract.difficultyMultipliers[(int)contract.difficulty]);
                    PunishPlayer(punishment, contract);
                    Debug.Log("Contract failed! Time limit exceeded.");
                }
                else
                {
                    // Check if the contract's success criteria are met
                    if (CheckCompleted(contract))
                    {
                        // Contract successfully completed
                        contract.isCompleted = true;
                        int reward = Mathf.RoundToInt(contract.baseReward * contract.difficultyMultipliers[(int)contract.difficulty]);
                        RewardPlayer(reward, contract);
                        Debug.Log("Contract completed successfully!");
                    }
                }
            }
        }
    }


    static public void UpdateConProgress(ContractType type, int value)
    {
        foreach (Contract contract in contracts)
        {
            if (!contract.isCompleted && !contract.isFailed)
            {
                if(contract.type == type)
                {
                    contract.progressTracker += value;
                }
            }
        }
    }

    bool CheckCompleted(Contract contract)
    {
        if(contract.progressTracker >= contract.goalAmount)
        {
            return true;
        }
        return false;
    }

    void RewardPlayer(int reward, Contract contract)
    {
        switch (contract.type)
        {
            case ContractType.SellCon:
                _soulManager.AddMoney(reward);
                return;
            case ContractType.TortureCon:
                _soulManager.AddMoney(reward);
                return;
            case ContractType.EarnCon:
                _soulManager.AddMoney(reward);
                return;
            default:
                return;
        }
    }

    void PunishPlayer(int penalty, Contract contract)
    {
        switch (contract.type)
        {
            case ContractType.SellCon:
                _soulManager.SubtractMoney(penalty);
                return;
            case ContractType.TortureCon:
                _soulManager.SubtractMoney(penalty);
                return;
            case ContractType.EarnCon:
                _soulManager.SubtractMoney(penalty);
                return;
            default:
                return;
        }
    }
}
