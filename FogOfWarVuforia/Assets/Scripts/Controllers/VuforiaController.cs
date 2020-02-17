/*==============================================================================
Copyright (c) 2019 PTC Inc. All Rights Reserved.

Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using UnityEngine;
using Vuforia;

/// <summary>
/// A custom handler that implements the ITrackableEventHandler interface.
///
/// Changes made to this file could be overwritten when upgrading the Vuforia version.
/// When implementing custom event handler behavior, consider inheriting from this class instead.
/// </summary>
public class VuforiaController : MonoBehaviour, ITrackableEventHandler
{
    public BoardClass board;
    public UIController uiController;
    public PlayerClass red;
    public PlayerClass blue;

    public GameObject redPlayer;
    public GameObject redEnemy;
    public GameObject redUpArrow;
    public GameObject redDownArrow;
    public GameObject redLeftArrow;
    public GameObject redRightArrow;

    public GameObject bluePlayer;
    public GameObject blueEnemy;
    public GameObject blueUpArrow;
    public GameObject blueDownArrow;
    public GameObject blueLeftArrow;
    public GameObject blueRightArrow;

 

    public bool isFound = false;
    #region PROTECTED_MEMBER_VARIABLES

    protected TrackableBehaviour mTrackableBehaviour;
    protected TrackableBehaviour.Status m_PreviousStatus;
    protected TrackableBehaviour.Status m_NewStatus;

    #endregion // PROTECTED_MEMBER_VARIABLES

    #region UNITY_MONOBEHAVIOUR_METHODS

    protected virtual void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
    }

    protected virtual void OnDestroy()
    {
        if (mTrackableBehaviour)
            mTrackableBehaviour.UnregisterTrackableEventHandler(this);
    }

    #endregion // UNITY_MONOBEHAVIOUR_METHODS

    #region PUBLIC_METHODS

    /// <summary>
    ///     Implementation of the ITrackableEventHandler function called when the
    ///     tracking state changes.
    /// </summary>
    public void OnTrackableStateChanged(
        TrackableBehaviour.Status previousStatus,
        TrackableBehaviour.Status newStatus)
    {
        m_PreviousStatus = previousStatus;
        m_NewStatus = newStatus;

        Debug.Log("Trackable " + mTrackableBehaviour.TrackableName +
                  " " + mTrackableBehaviour.CurrentStatus +
                  " -- " + mTrackableBehaviour.CurrentStatusInfo);

        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            OnTrackingFound();
        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
                 newStatus == TrackableBehaviour.Status.NO_POSE)
        {
            OnTrackingLost();
        }
        else
        {
            // For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
            // Vuforia is starting, but tracking has not been lost or found yet
            // Call OnTrackingLost() to hide the augmentations
            OnTrackingLost();
        }
    }

    public void HideAll()
    {
        redUpArrow.SetActive(false);
        redDownArrow.SetActive(false);
        redLeftArrow.SetActive(false);
        redRightArrow.SetActive(false);

        blueUpArrow.SetActive(false);
        blueDownArrow.SetActive(false);
        blueLeftArrow.SetActive(false);
        blueRightArrow.SetActive(false);

        bluePlayer.SetActive(false);
        redEnemy.SetActive(false);

        redPlayer.SetActive(false);
        blueEnemy.SetActive(false);

        foreach (GameObject go in blue.items)
            go.SetActive(false);
        foreach (GameObject go in red.items)
            go.SetActive(false);
    }

    public void RefreshPositions()
    {
        if (isFound)
        {

            if (uiController.activePlayer == red)
            {
                redPlayer.SetActive(true);
                redPlayer.transform.localPosition = new Vector3(-0.75f + red.position.x * 0.2f, 0.2f, -0.75f + red.position.y * 0.2f);
                blueEnemy.SetActive(true);
                blueEnemy.transform.localPosition = new Vector3(-0.75f + red.enemy.position.x * 0.2f, 0.2f, -0.75f + red.enemy.position.y * 0.2f);
                foreach (GameObject go in blue.items)
                    go.SetActive(false);
                foreach (GameObject go in red.items)
                    go.SetActive(true);
                bluePlayer.SetActive(false);
                redEnemy.SetActive(false);
            }
            else
            {
                bluePlayer.SetActive(true);
                bluePlayer.transform.localPosition = new Vector3(-0.75f + blue.position.x * 0.2f, 0.2f, -0.75f + blue.position.y * 0.2f);
                redEnemy.SetActive(true);
                redEnemy.transform.localPosition = new Vector3(-0.75f + blue.enemy.position.x * 0.2f, 0.2f, -0.75f + blue.enemy.position.y * 0.2f);
                foreach (GameObject go in blue.items)
                    go.SetActive(true);
                foreach (GameObject go in red.items)
                    go.SetActive(false);
                redPlayer.SetActive(false);
                blueEnemy.SetActive(false);
            }
            RefreshArrows();
        }
    }
    public void RefreshArrows()
    {
        if (isFound)
        {
            redUpArrow.SetActive(false);
            redDownArrow.SetActive(false);
            redLeftArrow.SetActive(false);
            redRightArrow.SetActive(false);

            blueUpArrow.SetActive(false);
            blueDownArrow.SetActive(false);
            blueLeftArrow.SetActive(false);
            blueRightArrow.SetActive(false);

            if (uiController.activePlayer == red)
            { 
                if (red.direction == DIRECTIONS.NORTH) redUpArrow.SetActive(true);
                else if (red.direction == DIRECTIONS.SOUTH) redDownArrow.SetActive(true);
                else if (red.direction == DIRECTIONS.WEST) redLeftArrow.SetActive(true);
                else if (red.direction == DIRECTIONS.EAST) redRightArrow.SetActive(true);

            }
            else
            {               
                if (blue.direction == DIRECTIONS.NORTH) blueUpArrow.SetActive(true);
                else if (blue.direction == DIRECTIONS.SOUTH) blueDownArrow.SetActive(true);
                else if (blue.direction == DIRECTIONS.WEST) blueLeftArrow.SetActive(true);
                else if (blue.direction == DIRECTIONS.EAST) blueRightArrow.SetActive(true);

            }
        }
    }

    #endregion // PUBLIC_METHODS

    #region PROTECTED_METHODS

    protected virtual void OnTrackingFound()
    {
        if (mTrackableBehaviour && uiController.uis != UIState.WAIT_MENU)
        {
            isFound = true;
            var rendererComponents = mTrackableBehaviour.GetComponentsInChildren<Renderer>(true);
            var colliderComponents = mTrackableBehaviour.GetComponentsInChildren<Collider>(true);
            var canvasComponents = mTrackableBehaviour.GetComponentsInChildren<Canvas>(true);

            // Enable rendering:
            foreach (var component in rendererComponents)
                component.enabled = true;

            // Enable colliders:
            foreach (var component in colliderComponents)
                component.enabled = true;

            // Enable canvas':
            foreach (var component in canvasComponents)
                component.enabled = true;
            HideAll();
            RefreshPositions();
            RefreshArrows();
        }

    }


    protected virtual void OnTrackingLost()
    {
        if (mTrackableBehaviour)
        {
            isFound = false;
            var rendererComponents = mTrackableBehaviour.GetComponentsInChildren<Renderer>(true);
            var colliderComponents = mTrackableBehaviour.GetComponentsInChildren<Collider>(true);
            var canvasComponents = mTrackableBehaviour.GetComponentsInChildren<Canvas>(true);

            // Disable rendering:
            foreach (var component in rendererComponents)
                component.enabled = false;

            // Disable colliders:
            foreach (var component in colliderComponents)
                component.enabled = false;

            // Disable canvas':
            foreach (var component in canvasComponents)
                component.enabled = false;
        }
    }

    

    #endregion // PROTECTED_METHODS
}
