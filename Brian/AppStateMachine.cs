using UnityEngine;
using System.Collections;

public class AppStateMachine : ApplicationStateMachine<AppContext,AppState > {



}
public abstract class AppState : ApplicationState<AppContext>{

}
[System.Serializable]
public class AppContext : ApplicationStateContext{
	public int MainMenuID;
	public int OptionsID;
}
