namespace TheJazMaster.CatAndAmy.Artifacts;

public interface IOverdriveReductionPreventerArtifact
{
	bool ShouldReduceOverdrive(State s, Combat c);
}