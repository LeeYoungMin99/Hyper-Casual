using System;

public class ObjectPoolingEventArgs : EventArgs
{
    public int Index;
}

public class HealthChangeEventArgs : EventArgs
{
    public float MaxHealth;
    public float CurHealth;
}

public class ExperienceEventArgs : EventArgs
{
    public float ExperienceAmount;
}

public class AbilityEventArgs : EventArgs
{
    public EAbilityTag Ability;
}