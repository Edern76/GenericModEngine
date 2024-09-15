namespace GenericModEngine.Core.Events;

public class SafetyDisclaimerEvents
{
    /* The intent is the following :
        - The game should subscribe to the WhenShowSafetyDisclaimer event before initializing GenericModEngine
        - GenericModEngine invokes the WhenShowSafetyDisclaimer event if there are any mods with custom assemblies to load
        - The game should then display the safety disclaimer to the user and get the answer from the user
        - Then the game invokes OnSafetyDisclaimerAnswered with the answer
     */
    
    public delegate void ShowSafetyDisclaimerEvent();
    public static event ShowSafetyDisclaimerEvent WhenShowSafetyDisclaimer = new ShowSafetyDisclaimerEvent(() => {});
    
    public delegate void SafetyDisclaimerAnsweredEvent(bool answer);
    public static event SafetyDisclaimerAnsweredEvent OnSafetyDisclaimerAnswered = new SafetyDisclaimerAnsweredEvent((answer) => {});
} 