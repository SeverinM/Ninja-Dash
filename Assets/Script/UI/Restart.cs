using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Restart : StateMachineBehaviour {

	public static string flag;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		int lvl;
		if (flag == "restart")
			GameObject.Find("GameManager").GetComponent<GestionJeu>().Retry();
		
		if (flag == "menu")
			GameObject.Find("GameManager").GetComponent<GestionJeu>().Menu();

        if (flag == "titre")
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Titre");
            Destroy(GameObject.Find("Audio Source"));
            Destroy(GameObject.Find("son"));
        }

		if (flag == "next")
			GameObject.Find ("GameManager").GetComponent<GestionJeu> ().nextLevel ();

		if (System.Int32.TryParse(flag,out lvl))
			GameObject.Find ("GameManager").GetComponent<GestionJeu> ().LoadLvl(lvl);
			
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
