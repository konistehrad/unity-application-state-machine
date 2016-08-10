using UnityEngine;
using System.Collections;


public class ApplicationStateMachine<T, R> : MonoBehaviour where T : ApplicationStateContext where R : ApplicationState<T>{

	public enum TransitionResponse{Failure, Success}
	[SerializeField] R[] states;
	int currentStateId = 0;

	[SerializeField] T context;

	void Start(){
		InitializeStates();
	}

	void Update(){
		UpdateFSM();
	}

	void UpdateFSM(){

		ApplicationState<T> currentState = states[currentStateId];
		int result = currentState.UpdateState(context);
		if(result != -1){
			TransitionToState(result);
		}
		for(int i=0; i<states.Length; i++){
			if(i != currentStateId){
				states[i].BackgroundUpdate(currentState);
			}
		}

	}


	void InitializeStates(){
		//for(int i=0; i<states.Length; i++){
			
	//	}
	}

	bool CanTransition(int targetStateId){
		if(targetStateId <0 || targetStateId >= states.Length){
			return false;
		}

		ApplicationState<T> targetState = states[targetStateId];
		ApplicationState<T> currentState = states[currentStateId];
		if(targetState == currentState){
			return false;
		}

		return true;
	}

	public TransitionResponse TransitionToState(int targetStateId){
		///make sure we are pointing to a valid state
		if(targetStateId <0 || targetStateId >= states.Length){
			return TransitionResponse.Failure;
		}

		ApplicationState<T> targetState = states[targetStateId];
		ApplicationState<T> currentState = states[currentStateId];

		/// we should not permit re-entry on application states, since they are globally scoped
		/// we could specifically permit this is the specif state allows it as a 'reset'
		if(targetState == currentState){
			return TransitionResponse.Failure;
		}

		currentState.Exit(context);
		targetState.Enter(context);
		currentStateId = targetStateId;
		return TransitionResponse.Success;
		
		
	}

}

public abstract class ApplicationState<T> : MonoBehaviour where T : ApplicationStateContext{

	protected enum TransitionStatus{Idle, Enter, Active, Exit}
	protected TransitionStatus status;



	public virtual void Exit(T context){
		status = TransitionStatus.Exit;
	}
	public virtual void Enter(T context){
		status = TransitionStatus.Enter;
	}

	public abstract int UpdateState(T context);

	public abstract void BackgroundUpdate(ApplicationState<T> currentState);
}

public abstract class ApplicationStateContext{

}