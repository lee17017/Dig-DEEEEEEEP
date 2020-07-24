using UnityEngine;

/// <summary> This class translates its gameobject from 'start' to 'end' when triggered. </summary>
public class NextLayer : MoveSprite
{
    [SerializeField]
    private bool active = false;

    protected override void OnAwake()
    {
        durationModifier = 5;
        base.OnAwake();
    }

    private void Update()
    {
        if (active)
        {
            active = false;
            StartMoving();
        }
    }
}