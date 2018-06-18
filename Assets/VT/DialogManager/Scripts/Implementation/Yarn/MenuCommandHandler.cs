using System.Collections;
using System.Collections.Generic;
using HookControl;
using UnityEngine;
using Utilities;
using Yarn;
using YarnDialog;

public class MenuCommandHandler : MonoBehaviour, YarnDialogManager.IDialogHandler
{
    [SerializeField] private List<GameObject> _menuPrefabs = new List<GameObject>();

    public IEnumerator Handle(Dialogue.RunnerResult result, YarnDialogManager manager)
    {
        var commandResult = result as Yarn.Dialogue.CommandResult;
        if (commandResult == null)
        {
            yield break;
        }

        string[] param = commandResult.command.text.Split(' ');

        if (param.Length != 2)
        {
            yield break;
        }

        if (!param[0].ToLower().Contains("menu"))
        {
            yield break;
        }

        // Parse menu name and show it.
        GameObject menuToShow = null;
        foreach (var menuPrefab in _menuPrefabs)
        {
            if (menuPrefab.name.ToLower().Equals(param[1].ToLower()))
            {
                menuToShow = menuPrefab;
                break;
            }
        }

        if (menuToShow == null) yield break;

        Control control = new Control(menuToShow);
        var showResult = control.Show();
        if (showResult == ShowResult.FIRST)
        {
            var go = control.instance;
            var animationHook = go.GetComponent<AnimationHook>();
            if (animationHook)
            {
                animationHook.Show();
                bool isVisible = true;
                animationHook.onHideEnded = () => isVisible = false;
                while (isVisible)
                {
                    yield return null;
                }
                control.Destroy();
            }
            else
            {
                while (control.IsVisible())
                {
                    yield return null;
                }
                control.Destroy();
            }
        }
    }

    public void HandlerReset(YarnDialogManager manager)
    {
    }

    public void HandlerUpdate(YarnDialogManager manager)
    {
    }
}