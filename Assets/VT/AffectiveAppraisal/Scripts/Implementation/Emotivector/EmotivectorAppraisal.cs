
public class EmotivectorAppraisal : IAffectiveAppraisal
{
    private Emotivector _emotivector; 
    
    public EmotivectorAppraisal()
    {
        _emotivector = new Emotivector(new MartinhoSimplePredictor());
    }

    public void ComputeUserEmotion(History history, User user)
    {
        // Read history and update Emotivectors
        // Add values to the emotivectors using emotivector.AddValue
        
        // Compute Expectancy for all the emotivectors
        Emotivector.Expectancy expectancy = _emotivector.ComputeExpectancy();
        
        // Compute emotion using expectancy
    }

    public void ComputeTutorEmotion(History history, User user, Tutor tutor)
    {
        throw new System.NotImplementedException();
    }
}