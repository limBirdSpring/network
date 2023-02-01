using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoYoon
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField]
        private float speed;

        private void Start()
        {
            Destroy(gameObject, 3f);
        }

        private void Update()
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
        }
    }
}
