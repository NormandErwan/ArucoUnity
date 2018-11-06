using ArucoUnity.Plugin;
using System;
using UnityEngine;

namespace ArucoUnity.Objects
{
    /// <summary>
    /// Describes an ChArUco diamond marker.
    /// </summary>
    public class ArucoDiamond : ArucoObject
    {
        // Constants

        /// <summary>
        /// A ChArUco diamond marker is composed of 3x3 squares(3 per side).
        /// </summary>
        protected const int SquareNumberPerSide = 3;

        // Editor fields

        [SerializeField]
        [Tooltip("Side length of each square. In pixels for Creators. In meters for Trackers and Calibrators.")]
        private float squareSideLength;

        [SerializeField]
        [Tooltip("The id of the first marker of the diamond.")]
        private int marker1Id;

        [SerializeField]
        [Tooltip("The id of the second marker of the diamond.")]
        private int marker2Id;

        [SerializeField]
        [Tooltip("The id of the third marker of the diamond.")]
        private int marker3Id;

        [SerializeField]
        [Tooltip("The id of the fourth marker of the diamond.")]
        private int marker4Id;

        // Properties

        /// <summary>
        /// Gets or sets the side length of each square. In pixels for Creators. In meters for Trackers and Calibrators.
        /// </summary>
        public float SquareSideLength
        {
            get { return squareSideLength; }
            set
            {
                OnPropertyUpdating();
                squareSideLength = value;
                OnPropertyUpdated();
            }
        }

        /// <summary>
        /// Gets or sets the four ids of the four markers of the diamond.
        /// </summary>
        public int[] Ids
        {
            get { return ids; }
            set
            {
                if (value.Length != ids.Length)
                {
                    Debug.LogError("Invalid number of Ids: ArucoDiamond requires " + ids.Length + " ids.");
                    return;
                }

                OnPropertyUpdating();
                ids = value;
                OnPropertyUpdated();
            }
        }

        // Variables

        protected int[] ids;

        // MonoBehaviour methods

        protected override void OnValidate()
        {
            ids = new int[] { marker1Id, marker2Id, marker3Id, marker4Id };
            base.OnValidate();
        }
        // ArucoObject methods

        public override Cv.Mat Draw()
        {
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode && (MarkerSideLength <= 0 || SquareSideLength <= 0
                || SquareSideLength <= MarkerSideLength || Dictionary == null))
            {
                return null;
            }
#endif
            Cv.Vec4i ids = new Cv.Vec4i();
            for (int i = 0; i < Ids.Length; ++i)
            {
                ids.Set(i, Ids[i]);
            }

            Cv.Mat image;
            Aruco.DrawCharucoDiamond(Dictionary, ids, GetInPixels(SquareSideLength), GetInPixels(MarkerSideLength), out image);

            return image;
        }

        public override string GenerateName()
        {
            return "ArUcoUnity_DiamondMarker_" + Dictionary.Name + "_Ids_" + Ids[0] + "_" + Ids[1] + "_" + Ids[2] + "_" + Ids[3] + "_SquareSize_"
                + SquareSideLength + "_MarkerSize_" + MarkerSideLength;
        }

        public override Vector3 GetGameObjectScale()
        {
            float sideLength = SquareNumberPerSide * SquareSideLength;
            return new Vector3(sideLength, SquareSideLength, sideLength);
        }

        protected override void UpdateArucoHashCode()
        {
            if (Ids != null)
            {
                ArucoHashCode = GetArucoHashCode(Ids);
            }
        }

        // Methods

        /// <summary>
        /// Computes the hash code of a ChArUco diamond marker.
        /// </summary>
        /// <param name="ids">The list of ids of the four markers.</param>
        /// <returns>The calculated ArUco hash code.</returns>
        public static int GetArucoHashCode(int[] ids)
        {
            int hashCode = 17;
            hashCode = hashCode * 31 + typeof(ArucoDiamond).GetHashCode();
            foreach (var id in ids)
            {
                hashCode = hashCode * 31 + id;
            }
            return hashCode;
        }
    }
}