using System.Collections.Generic;
using ArucoUnity.Plugin;
using System;

namespace ArucoUnity.Objects.Trackers
{
  /// <summary>
  /// Manages a list of <see cref="ArucoObject"/>.
  /// </summary>
  public interface IArucoObjectsController
  {
    // Events

    /// <summary>
    /// Called when an ArUco object has been added to <see cref="ArucoObjects"/>.
    /// </summary>
    event Action<ArucoObject> ArucoObjectAdded;

    /// <summary>
    /// Called when an ArUco object has been removed from <see cref="ArucoObjects"/>.
    /// </summary>
    event Action<ArucoObject> ArucoObjectRemoved;

    /// <summary>
    /// Called when a new dictionary has been added to <see cref="ArucoObjects"/>.
    /// </summary>
    event Action<Aruco.Dictionary> DictionaryAdded;

    /// <summary>
    /// Called when a dictionary has been removed from <see cref="ArucoObjects"/>.
    /// </summary>
    event Action<Aruco.Dictionary> DictionaryRemoved;

    // Properties

    /// <summary>
    /// Gets the list of the ArUco objects to detect.
    /// </summary>
    Dictionary<Aruco.Dictionary, Dictionary<int, ArucoObject>> ArucoObjects { get; }

    // Methods

    /// <summary>
    /// Adds an ArUco object to the <see cref="ArucoObjects"/> list.
    /// </summary>
    /// <param name="arucoObject">The ArUco object to add.</param>
    void AddArucoObject(ArucoObject arucoObject);

    /// <summary>
    /// Removes an ArUco object to the <see cref="ArucoObjects"/> list.
    /// </summary>
    /// <param name="arucoObject">The ArUco object to remove.</param>
    void RemoveArucoObject(ArucoObject arucoObject);

    /// <summary>
    /// Returns a sublist from <see cref="ArucoObjects"/> of ArUco objects of a precise type <typeparamref name="U"/> in a certain
    /// <paramref name="dictionary"/>.
    /// </summary>
    /// <typeparam name="U">The type of the ArUco objects in the returned sublist.</typeparam>
    /// <param name="dictionary">The <see cref="Aruco.Dictionary" /> to use.</param>
    /// <returns>The sublist.</returns>
    HashSet<U> GetArucoObjects<U>(Aruco.Dictionary dictionary) where U : ArucoObject;
  }
}
