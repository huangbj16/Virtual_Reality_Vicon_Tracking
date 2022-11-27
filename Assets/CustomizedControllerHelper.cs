using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizedControllerHelper : MonoBehaviour
{
    public GameObject m_modelOculusTouchQuest2LeftController;
    public GameObject m_modelOculusTouchQuest2RightController;

    public OVRInput.Controller m_controller;

    private Animator m_animator;

    private GameObject m_activeController;

    private bool m_hasInputFocus = true;
    private bool m_hasInputFocusPrev = false;

    private enum ControllerType
    {
        QuestAndRiftS = 1,
        Rift = 2,
        Quest2 = 3,
    }

    private ControllerType activeControllerType = ControllerType.Rift;

    private bool m_prevControllerConnected = false;
    private bool m_prevControllerConnectedCached = false;

    void Start()
    {
        OVRPlugin.SystemHeadset headset = OVRPlugin.GetSystemHeadsetType();
        activeControllerType = ControllerType.Quest2;
        m_modelOculusTouchQuest2LeftController.SetActive(false);
        m_modelOculusTouchQuest2RightController.SetActive(false);

        OVRManager.InputFocusAcquired += InputFocusAquired;
        OVRManager.InputFocusLost += InputFocusLost;
    }

    void Update()
    {
        bool controllerConnected = OVRInput.IsControllerConnected(m_controller);

        if ((controllerConnected != m_prevControllerConnected) || !m_prevControllerConnectedCached || (m_hasInputFocus != m_hasInputFocusPrev))
        {
            if (activeControllerType == ControllerType.Quest2)
            {
                m_modelOculusTouchQuest2LeftController.SetActive(controllerConnected && (m_controller == OVRInput.Controller.LTouch));
                m_modelOculusTouchQuest2RightController.SetActive(controllerConnected && (m_controller == OVRInput.Controller.RTouch));

                m_animator = m_controller == OVRInput.Controller.LTouch ? m_modelOculusTouchQuest2LeftController.GetComponent<Animator>() :
                    m_modelOculusTouchQuest2RightController.GetComponent<Animator>();
                m_activeController = m_controller == OVRInput.Controller.LTouch ? m_modelOculusTouchQuest2LeftController : m_modelOculusTouchQuest2RightController;
            }

            m_activeController.SetActive(m_hasInputFocus && controllerConnected);

            m_prevControllerConnected = controllerConnected;
            m_prevControllerConnectedCached = true;
            m_hasInputFocusPrev = m_hasInputFocus;
        }

        if (m_animator != null)
        {
            m_animator.SetFloat("Button 1", OVRInput.Get(OVRInput.Button.One, m_controller) ? 1.0f : 0.0f);
            m_animator.SetFloat("Button 2", OVRInput.Get(OVRInput.Button.Two, m_controller) ? 1.0f : 0.0f);
            m_animator.SetFloat("Button 3", OVRInput.Get(OVRInput.Button.Start, m_controller) ? 1.0f : 0.0f);

            m_animator.SetFloat("Joy X", OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, m_controller).x);
            m_animator.SetFloat("Joy Y", OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, m_controller).y);

            m_animator.SetFloat("Trigger", OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, m_controller));
            m_animator.SetFloat("Grip", OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, m_controller));
        }
    }

    public void InputFocusAquired()
    {
        m_hasInputFocus = true;
    }

    public void InputFocusLost()
    {
        m_hasInputFocus = false;
    }
}

