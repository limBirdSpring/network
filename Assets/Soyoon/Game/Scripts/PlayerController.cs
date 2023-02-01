using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace SoYoon
{
    public class PlayerController : MonoBehaviourPun, IPunObservable
    {
        [SerializeField]
        private int shootCount;
        [SerializeField]
        private Bullet bulletPrefab;


        private void Update()
        {
            if (photonView.IsMine)
            {
                Move();
                Shoot();
            }
        }

        private void Move()
        {
            float vInput = Input.GetAxis("Vertical");
            float hInput = Input.GetAxis("Horizontal");

            transform.Translate(20 * vInput * Time.deltaTime * Vector3.forward, Space.Self);
            transform.Rotate(90 * hInput * Time.deltaTime * Vector3.up, Space.Self);
        }

        private void Shoot()
        {
            if (!Input.GetButtonDown("Jump"))
                return;

            photonView.RPC("CreateBullet", RpcTarget.All, transform.position, transform.rotation);
            shootCount++;
        }

        [PunRPC]
        private void CreateBullet(Vector3 position, Quaternion rotation)
        {
            Instantiate(bulletPrefab, position, rotation);
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(shootCount);
            }

            if (stream.IsReading)
            {
                shootCount = (int)stream.ReceiveNext();
            }
        }
    }
}
