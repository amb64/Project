using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAnalyticsSDK;

public class Links : MonoBehaviour
{
    public void OpenLink1()
    {
        GameAnalytics.NewDesignEvent("clicked_link");
        Application.OpenURL("https://www.mind.org.uk/information-support/types-of-mental-health-problems/");
    }

    public void OpenLink2()
    {
        GameAnalytics.NewDesignEvent("clicked_link");
        Application.OpenURL("https://www.nhs.uk/mental-health/");
    }
    public void OpenLink3()
    {
        GameAnalytics.NewDesignEvent("clicked_link");
        Application.OpenURL("https://www.mentalhealth.org.uk/explore-mental-health");
    }

    public void OpenLink4()
    {
        GameAnalytics.NewDesignEvent("clicked_link");
        Application.OpenURL("https://mentalhealth-uk.org/help-and-information/conditions/");
    }
}
