using System.Collections;
using Trackers.Utils;
using UnityEngine;

namespace Scripts.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class CharacterVaultController : MonoBehaviour
    {
        [SerializeField] private float maxRange;
        [SerializeField] private float maxVaultRange;
        [SerializeField] private float maxVaultableHeight;
        [SerializeField] private float vaultDuration = 2f;
        [SerializeField] private AnimationCurve vaultOnCurve;
        [SerializeField] private AnimationCurve vaultOverCurve;

        private new Rigidbody rigidbody;

        private bool isVaulting;
        private float vaultTime;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (isVaulting)
            {
                return;
            }
            
            Vector3 aroundPosition = RotationUtil.GetVector(0, transform.eulerAngles.y, 0f, Vector3.forward, 
                transform.position, Vector3.one);

            Vector3 directionVector = aroundPosition - transform.position;
            directionVector = directionVector.normalized;

            Vector3 castPosition = transform.position;
            castPosition.y += 0.2f;

            RaycastHit[] hits = Physics.RaycastAll(castPosition, directionVector, maxRange);
            
            Debug.DrawRay(castPosition, directionVector * maxRange, Color.magenta);

            GameObject vaultableObject = null;
            Vector3 vaultableImpactPoint = Vector3.zero;

            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];
                
                if (hit.transform.gameObject.layer != 9)
                {
                    continue;
                }
                
                Debug.DrawRay(hit.point, Vector3.up * 2f, Color.green, vaultDuration);

                vaultableObject = hit.transform.gameObject;
                vaultableImpactPoint = hit.point;
                
                break;
            }

            if (vaultableObject == null)
            {
                return;
            }

            bool willVaultOver = true;

            Vector3 vaultOverTargetPosition = vaultableImpactPoint + (directionVector * maxVaultRange);

            float castHeight = 2f;
            castPosition = vaultOverTargetPosition;
            castPosition.y += castHeight;

            hits = Physics.RaycastAll(castPosition, Vector3.down, 2f);

            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];

                if (hit.transform.gameObject == vaultableObject)
                {
                    willVaultOver = false;
                }
            }

            Debug.DrawRay(castPosition, Vector3.down * castHeight, willVaultOver ? Color.green : Color.red,
                vaultDuration, false);
            
            DoVault(willVaultOver);
        }

        private void DoVault(bool vaultOver)
        {
            isVaulting = true;
            StartCoroutine(VaultEnumerator(vaultOver));
        }

        private IEnumerator VaultEnumerator(bool vaultOver)
        {
            AnimationCurve vaultCurve = vaultOver ? vaultOverCurve : vaultOnCurve;

            Vector3 vaultStartPosition = transform.position;
            float vaultStartRotationY = transform.eulerAngles.y;
            
            while (true)
            {
                float vaultProgress = (1 / vaultDuration) * vaultTime;
                
                if (vaultProgress >= 1f)
                {
                    vaultTime = 0f;
                    isVaulting = false;
                    break;
                }
                
                float vaultDistance = ((maxVaultRange + maxRange) / vaultDuration) * vaultTime;
                float vaultHeight = vaultCurve.Evaluate(vaultProgress);
                
                Vector3 vaultDistanceVector = new Vector3(0, 0, vaultDistance);
                Vector3 currentVaultPosition = RotationUtil.GetVector(0, vaultStartRotationY, 0, vaultDistanceVector,
                    vaultStartPosition, Vector3.one);
                currentVaultPosition.y += vaultHeight;

                rigidbody.velocity = Vector3.zero;
                rigidbody.position = currentVaultPosition;
                
                vaultTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}