using System;
using Content.Data;
using Content.Data.Enums;
using UnityEngine;

namespace Content.Gameplay
{
    public class PlayerController : PlayableEntityController
    {
        protected override Color GenerateEntityColour() => _persistentDataService.Settings.PlayerColour switch
        {
            PlayerColour.Red => Color.red,
            PlayerColour.Green => Color.green,
            PlayerColour.Blue => Color.blue,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}