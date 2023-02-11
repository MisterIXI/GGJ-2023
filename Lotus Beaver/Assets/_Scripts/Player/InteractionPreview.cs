using System;
using UnityEngine;

[Serializable]
public class InteractionPreview
{
    [SerializeField] private Transform _transform;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    public Transform Transform => _transform;
    public SpriteRenderer SpriteRenderer => _spriteRenderer;
}
