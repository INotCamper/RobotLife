using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


namespace Com.MyComapany.MyGame
{
    public class CollectablesSpawner : MonoBehaviour
    {
        #region Public Fields

        [Tooltip("The points that will define the area that items will be spawned")]
        public Vector3 pointA, pointB;
        [Tooltip("The points that will be excluded from the spawn area")]
        public Vector3 notPointA, notPointB;
        public bool _bIsSelected = true;

        #endregion



        #region Private Fields

        [Tooltip("All collectables that will be spawned in the scene")]
        [SerializeField]
        private GameObject[] collectables;
        [Tooltip("The time between collectables spawns")]
        [SerializeField]
        private float spawnTime;

        #endregion



        #region Monobehaivour Callbacks

        void Start()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                StartCoroutine(SpawnColl(spawnTime));
            }
            StartCoroutine(SpawnColl(spawnTime));
            print(PhotonNetwork.CurrentRoom.PlayerCount);
        }

        #region Draw Cube

        private void OnDrawGizmos()
        {
            if (_bIsSelected)
            {
                OnDrawGizmosSelected();
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (pointA != Vector3.zero && pointB != Vector3.zero)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(pointA, new Vector3(pointB.x, pointA.y, pointA.z));
                Gizmos.DrawLine(pointA, new Vector3(pointA.x, pointA.y, pointB.z));
                Gizmos.DrawLine(pointA, new Vector3(pointA.x, pointB.y, pointA.z));
                Gizmos.DrawLine(new Vector3(pointA.x, pointB.y, pointA.z), new Vector3(pointA.x, pointB.y, pointB.z));
                Gizmos.DrawLine(new Vector3(pointA.x, pointB.y, pointA.z), new Vector3(pointB.x, pointB.y, pointA.z));
                Gizmos.DrawLine(pointB, new Vector3(pointB.x, pointA.y, pointB.z));
                Gizmos.DrawLine(pointB, new Vector3(pointA.x, pointB.y, pointB.z));
                Gizmos.DrawLine(pointB, new Vector3(pointB.x, pointB.y, pointA.z));
                Gizmos.DrawLine(new Vector3(pointB.x, pointA.y, pointB.z), new Vector3(pointA.x, pointA.y, pointB.z));
                Gizmos.DrawLine(new Vector3(pointB.x, pointA.y, pointB.z), new Vector3(pointB.x, pointA.y, pointA.z));
            }
            if (notPointA != Vector3.zero && notPointB != Vector3.zero)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(notPointA, new Vector3(notPointB.x, notPointA.y, notPointA.z));
                Gizmos.DrawLine(notPointA, new Vector3(notPointA.x, notPointA.y, notPointB.z));
                Gizmos.DrawLine(notPointA, new Vector3(notPointA.x, notPointB.y, notPointA.z));
                Gizmos.DrawLine(new Vector3(notPointA.x, notPointB.y, notPointA.z), new Vector3(notPointA.x, notPointB.y, notPointB.z));
                Gizmos.DrawLine(new Vector3(notPointA.x, notPointB.y, notPointA.z), new Vector3(notPointB.x, notPointB.y, notPointA.z));
                Gizmos.DrawLine(notPointB, new Vector3(notPointB.x, notPointA.y, notPointB.z));
                Gizmos.DrawLine(notPointB, new Vector3(notPointA.x, notPointB.y, notPointB.z));
                Gizmos.DrawLine(notPointB, new Vector3(notPointB.x, notPointB.y, notPointA.z));
                Gizmos.DrawLine(new Vector3(notPointB.x, notPointA.y, notPointB.z), new Vector3(notPointA.x, notPointA.y, notPointB.z));
                Gizmos.DrawLine(new Vector3(notPointB.x, notPointA.y, notPointB.z), new Vector3(notPointB.x, notPointA.y, notPointA.z));
            }
        }
        #endregion

        #endregion



        #region Custom Callbacks

        public string GetRandomColl(GameObject[] g)
        {
            return g[Random.Range(0, g.Length)].name;
        }
        public Vector3 GetRandomPositionInArea(Vector3 posA, Vector3 posB, Vector3 notPosA, Vector3 notPosB)
        {
            Vector3 randPos = new Vector3(Random.Range(posA.x, posB.x), Random.Range(posA.y, posB.y), Random.Range(posA.z, posB.z));
            while (randPos.x > notPosA.x && randPos.z > notPosA.z && randPos.y > notPosA.y && randPos.y < notPosB.y)
            {
                randPos = new Vector3(Random.Range(posA.x, posB.x), Random.Range(posA.y, posB.y), Random.Range(posA.z, posB.z));
            }
            return randPos;
        }

        #endregion



        #region IEnumerator Callbacks

        IEnumerator SpawnColl(float t)
        {
            PhotonNetwork.Instantiate(GetRandomColl(collectables), GetRandomPositionInArea(pointA, pointB, notPointA, notPointB), Quaternion.identity, 0);
            yield return new WaitForSeconds(t / PhotonNetwork.CurrentRoom.PlayerCount);
            StartCoroutine(SpawnColl(t));
        }

        #endregion
    }
}
