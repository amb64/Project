using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Links : MonoBehaviour
{
    public void OpenLink1()
    {
        Application.OpenURL("https://www.mind.org.uk/information-support/types-of-mental-health-problems/");
    }

    public void OpenLink2()
    {
        Application.OpenURL("https://www.nhs.uk/mental-health/");
    }
    public void OpenLink3()
    {
        Application.OpenURL("https://www.mentalhealth.org.uk/explore-mental-health");
    }

    public void OpenLink4()
    {
        Application.OpenURL("https://mentalhealth-uk.org/help-and-information/conditions/");
    }
}
