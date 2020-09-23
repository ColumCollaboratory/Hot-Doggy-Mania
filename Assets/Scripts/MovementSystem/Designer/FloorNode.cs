using UnityEngine;

namespace HotDoggyMania.MovementSystemDesigner
{
    /// <summary>
    /// Represents a floor node-path in the design layout of a movement network.
    /// </summary>
    public sealed class FloorNode : MonoBehaviour
    {
        #region Inspector Fields
        [Range(1, 100)][Tooltip("Defines the length of this path.")]
        [SerializeField] private float length = 1f;
        #endregion
        #region Accessors
        /// <summary>
        /// The specified length of this floor path in the right direction from the transform.
        /// </summary>
        public float Length { get { return length; } }
        #endregion
        #region Designer Gizmo Drawing
        private void OnDrawGizmos()
        {
            if (MoveSystemDesignerSettings.ShowLayout)
            {
                Gizmos.color = MoveSystemDesignerSettings.FloorColor;
                Vector3 endPosition = transform.position + length * Vector3.right;
                Gizmos.DrawLine(transform.position, endPosition);
                Gizmos.DrawSphere(transform.position, MoveSystemDesignerSettings.NodeRadius);
                Gizmos.DrawSphere(endPosition, MoveSystemDesignerSettings.NodeRadius);
            }
        }
        #endregion
    }
}
