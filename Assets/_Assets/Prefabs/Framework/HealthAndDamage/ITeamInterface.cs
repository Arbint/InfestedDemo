using Unity.Mathematics;
using UnityEngine;

public enum TeamAttitute
{
    Hostile,
    Friendly,
    Neutral
}

public interface ITeamInterface
{
    public uint GetTeamId();

    public TeamAttitute GetTeamAttituteTowards(GameObject otherObject)
    {
        ITeamInterface otherTeamInterface = otherObject.GetComponent<ITeamInterface>();
        if (otherTeamInterface is not null)
        {
            return otherTeamInterface.GetTeamId() == GetTeamId() ? TeamAttitute.Friendly : TeamAttitute.Hostile;
        }

        return TeamAttitute.Neutral;
    }
}
