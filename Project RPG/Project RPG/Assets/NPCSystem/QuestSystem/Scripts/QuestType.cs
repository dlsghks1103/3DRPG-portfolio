namespace RPG.QuestSystem
{
    public enum QuestType : int
    {
        DestroyEnemy,
        AcquireItem,
    }

    public enum QuestStatus : int
    {
        None,
        Accepted,
        Completed,
        Rewarded,
    }
}