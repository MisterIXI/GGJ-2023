using UnityEngine;
using System;

public interface IInteractable
{

    public void OnInteract(Tile tile);


    public void OnSelection(Tile tile);

}

