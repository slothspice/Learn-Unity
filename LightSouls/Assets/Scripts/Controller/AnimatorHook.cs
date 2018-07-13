﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA {

    public class AnimatorHook : MonoBehaviour {

        Animator anim;
        StateManager states;

        public float rm_multi;
        public bool rolling;
        float roll_t;

        

        public void Init( StateManager st) {
            //makes manager and animator local.
            states = st;
            anim = st.anim;
        }

        public void InitForRoll() {
            rolling = true;
            roll_t = 0;
        }

        public void CloseRoll() {
            if (rolling == false)
                return;
            rm_multi = 1;
            roll_t = 0;
            rolling = false;
        }

        void OnAnimatorMove() {

            //if can move, return
            if (states.canMove)
                return;

            //if can't move (falling, action)
            states.rigid.drag = 0;

            if (rm_multi == 0) {
                rm_multi = 1;
            }

            if (rolling == false) {

                //something something moves character during action.
                Vector3 delta = anim.deltaPosition;
                delta.y = 0;
                Vector3 v = (delta * rm_multi) / states.delta;
                states.rigid.velocity = v;

            } else {

                roll_t += states.delta/0.65f;
                //Debug.Log(roll_t);

                if (roll_t > 1) {
                    roll_t = 1;
                }

                float zValue = states.roll_curve.Evaluate(roll_t);
                Vector3 v1 = Vector3.forward * zValue;
                Vector3 relative = transform.TransformDirection(v1);
                Vector3 v2 = (relative * rm_multi); // / states.delta;
                states.rigid.velocity = v2;

            }

        }


    }

}
