using UnityEngine;

namespace PFrame.Entities.Authoring
{
    [CreateAssetMenu(fileName = "PrefabData_", menuName = "PFrame/PrefabData")]
    public class PrefabDataSO : AGameDataSO
    {
        public GameObject Prefab;
    }
}
