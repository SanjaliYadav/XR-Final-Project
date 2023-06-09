// Copyright (C) 2014-2023 Gleechi AB. All rights reserved.

using UnityEngine;
using VirtualGrasp;

namespace VirtualGrasp.Onboarding
{
    public class SoundOnGraspEvents : MonoBehaviour
    {
        public AudioSource graspSoundEffect;
        private int avatarID;

        private void Start()
        {
            avatarID = GetComponentInChildren<SkinnedMeshRenderer>().GetInstanceID();
            VG_Controller.OnGraspTriggered.AddListener(PlayGraspSound);
        }

        public void PlayGraspSound(VG_HandStatus hand)
        {
            if (hand.m_avatarID == avatarID)
                graspSoundEffect.Play();
        }
    }
}