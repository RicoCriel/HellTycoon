using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FreeBuild
{
    public class FreeBuildObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Material _hoverMaterial;

        private Material _defaultMaterial;
        private Renderer _renderer;

        private void Awake()
        {
            if (gameObject.TryGetComponent(out _renderer))
            {
                _defaultMaterial = _renderer.material;
            }
            else
            {
                _renderer = gameObject.GetComponentInChildren<Renderer>();
                if (_renderer != null)
                {
                    _defaultMaterial = _renderer.material;
                }
            }

            if (_hoverMaterial == null)
            {
                _hoverMaterial = _defaultMaterial;
            }
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            _renderer.material = _hoverMaterial;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _renderer.material = _defaultMaterial;
        }
    }
}
