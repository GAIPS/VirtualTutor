using UnityEngine;

public class VT_Main : MonoBehaviour
{

    private SystemManager manager;

    // Use this for initialization
    void Start()
    {
        manager = new SystemManager();

        // Setup Affective Appraisal
        ModularAffectiveAppraisal appraisal = new ModularAffectiveAppraisal(
                new UserAA_OneEmotion(new Emotion("Fear")),
                new TutorAA_CopyUser()
            );
        manager.AffectiveAppraisal = appraisal;



        manager.Setup();
    }

    // Update is called once per frame
    void Update()
    {
        // TODO When should I update the manager?
        //manager.Update();
    }
}
