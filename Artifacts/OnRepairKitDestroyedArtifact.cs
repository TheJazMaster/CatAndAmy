namespace TheJazMaster.CatAndAmy.Artifacts;

public interface IOnRepairKitDestroyedArtifact
{
	void OnRepairKitDestroyed(State s, Combat c, bool wasPlayer, int worldX);
}