using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Main manager class for the HeadSystem module 
// Handles the synchronization of the various controllers, 
//  as well as propagating requests to the right controllers
public class AvatarManager : MonoBehaviour
{
    [SerializeField]
    private List<AvatarController> Controllers;

    internal void Feel(string name, EmotionalState emotion, float intensity)
    {
        AvatarController controller = getController(name);
        if (controller == null)
            return;
        controller.SetMood(emotion, intensity);
    }
    internal void Express(string name, EmotionalState expression, float intensity)
    {
        AvatarController controller = getController(name);
        if (controller == null)
            return;
        controller.ExpressEmotion(expression, intensity);
    }
    internal void Talk(string name, TalkState actionState)
    {
        AvatarController controller = getController(name);
        if (controller == null)
            return;
        controller.DoTalking(actionState);
        StartCoroutine(React(name, actionState));
    }
    internal void Gaze(string name, GazeState actionState)
    {
        AvatarController controller = getController(name);
        if (controller == null)
            return;
        controller.DoGazing(actionState);
    }
    internal void Nod(string name, NodState actionState)
    {
        AvatarController controller = getController(name);
        if (controller == null)
            return;
        controller.DoNodding(actionState);
    }
    internal void setParameter(string name, AnimatorParams param, float value)
    {
        AvatarController controller = getController(name);
        if (controller == null)
            return;
        controller.setParameter(param, value);
    }
    internal void setParameter(string name, ControllerParams param, float value)
    {
        AvatarController controller = getController(name);
        if (controller == null)
            return;
        controller.setParameter(param, value);
    }

    // reaction subroutine for talk actions
    IEnumerator React(string name, TalkState actionState)
    {
        float delay = 0.5f;
        yield return new WaitForSeconds(delay);
        if (actionState.Equals(TalkState.TALK_START))
        {
            AvatarController partnerController = getPartnerController(name);
            partnerController.isListening(true);
            partnerController.DoGazing(GazeState.GAZEAT_PARTNER);
        }
        if (actionState.Equals(TalkState.TALK_END))
        {
            AvatarController partnerController = getPartnerController(name);
            partnerController.isListening(false);
            partnerController.DoNodding(NodState.NOD_END);
            partnerController.DoGazing(GazeState.GAZEBACK_PARTNER);
        }
    }

    // controller fetchers
    private AvatarController getController(string id)
    {
        foreach (var controller in Controllers)
        {
            if (controller.controllerID.Equals(id))
                return controller;
        }
        return null;
    }
    private AvatarController getPartnerController(string id)
    {
        foreach (var controller in Controllers)
        {
            if (!controller.controllerID.Equals(id))
                return controller;
        }
        return null;
    }
    public AvatarParameters getControllerParameters(string controllerID) {
        AvatarController c = getController(controllerID);
        if (c == null)
            return null;
        else
            return c.getParameters();
    }
}