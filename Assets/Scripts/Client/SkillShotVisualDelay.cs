using System.Collections;
using UnityEngine;

namespace Client
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SkillShotVisualDelay : MonoBehaviour
    { 
        [SerializeField] private int delayFrameCount = 1;
        
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.enabled = false;
        }

        private IEnumerator Start()
        {
            for (var i = 0; i < delayFrameCount; i++)
            {
                yield return null;
            }
            _spriteRenderer.enabled = true;
        }
    }

}
