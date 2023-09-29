using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;

public class BlochSphereArrow : MonoBehaviour
{
    public Transform blochSphere;
    public Quaternion rotation;
    // Start is called before the first frame update
    void Start()
    {
        rotation = blochSphere.localRotation;
        Debug.Log(rotation);
    }

    public void PauliXGate()
    {
        Debug.Log("Pauli X Gate Activated");
        Debug.Log("Before:" + blochSphere.localRotation.eulerAngles);
        //Pauli X Gate has no effect if the state is in ket plus and ket minus
        Vector3 local = blochSphere.localRotation.eulerAngles;

        if (local == new Vector3(90, 90, 0)) //Positive y axis i.e ket plus i
        {
            blochSphere.localEulerAngles = new Vector3(90, 270, 0); ; //Transform to ket minus i
        }

        if (local == new Vector3(90, 270, 0)) //Negative y axis i.e ket minus i
        {
            blochSphere.localEulerAngles = new Vector3(90, 90, 0); //Transform to ket plus i
        }

        if (local == new Vector3(0, 0, 0)) //Positive z axis i.e. ket 0
        {
            blochSphere.localEulerAngles = new Vector3(0, 180, 180); //Transform to ket 1
        }

        if (local == new Vector3(0, 180, 180)) //Negative z axis i.e ket 1
        {
            blochSphere.localEulerAngles = new Vector3(0, 0, 0); //Transform to ket 0
        }
        Debug.Log("After:" + blochSphere.localRotation.eulerAngles);
    }

    public void PauliZGate()
    {
        Debug.Log("Pauli Z Gate Activated");
        Debug.Log("Before:" + blochSphere.localRotation.eulerAngles);
        Vector3 local = blochSphere.localRotation.eulerAngles;
        if (local == new Vector3(270, 0, 0)) //Positive x axis i.e. ket plus
        {
            blochSphere.localEulerAngles = new Vector3(90, 0, 0); //Transform to ket minus
        }

        if (local == new Vector3(90, 0, 0)) //Negative x axis i.e. ket minus
        {
            blochSphere.localEulerAngles = new Vector3(270, 0, 0); //Transform to ket plus
        }

        if (local == new Vector3(90, 90, 0)) //Positive y axis i.e ket plus i
        {
            blochSphere.localEulerAngles = new Vector3(90, 270, 0); //Transform to ket minus i
        }

        if (local == new Vector3(90, 270, 0)) //Negative y axis i.e ket minus i
        {
            blochSphere.localEulerAngles = new Vector3(90, 90, 0); //Transform to ket plus i
        }
        Debug.Log("After:" + blochSphere.localRotation.eulerAngles);
        //Pauli Z Gate has no effect if the state is in ket 0 or ket 1
    }


    public void PauliYGate()
    {
        Debug.Log("Pauli Y Gate Activated");
        Debug.Log("Before:" + blochSphere.localRotation.eulerAngles);
        Vector3 local = blochSphere.localRotation.eulerAngles;
        if (local == new Vector3(270, 0, 0)) //Positive x axis i.e. ket plus
        {
            blochSphere.localEulerAngles = new Vector3(90, 0, 0); //Transform to ket minus
        }

        if (local == new Vector3(90, 0, 0)) //Negative x axis i.e. ket minus
        {
            blochSphere.localEulerAngles = new Vector3(270, 0, 0); //Transform to ket plus
        }

        //Pauli Y Gate has no effect if the state is in ket plus i or ket minus i

        if (local == new Vector3(0, 0, 0)) //Positive z axis i.e. ket 0
        {
            blochSphere.localEulerAngles = new Vector3(0, 180, 180); //Transform to i ket 1 = ket 1
        }

        if (local == new Vector3(0, 180, 180)) //Negative z axis i.e ket 1
        {
            blochSphere.localEulerAngles = new Vector3(0, 0, 0); //Transform to i ket 0 = ket 0
        }
        Debug.Log("After:" + blochSphere.localRotation.eulerAngles);

    }


    public void HadamardGate()
    {
        Debug.Log("Hadamard Gate Activated");
        Debug.Log("Before:"+blochSphere.localRotation.eulerAngles);
        Vector3 local = blochSphere.localRotation.eulerAngles;
        if (local == new Vector3(0, 0, 0)) //Positive z axis i.e. ket 0
        {
            blochSphere.localEulerAngles = new Vector3(270, 0, 0); //Transform to ket plus (+ve x)
        }

        if (local == new Vector3(0, 180, 180)) //Negative z axis i.e ket 1
        {
            blochSphere.localEulerAngles = new Vector3(90, 0, 0); //Transform to ket minus (-ve x)
        }

        if (local == new Vector3(270, 0, 0)) //Positive x axis i.e. ket plus
        {
            blochSphere.localEulerAngles = new Vector3(0, 0, 0); //Transform to ket 0 (+ve z)
        }

        if (local == new Vector3(90, 0, 0)) //Negative x axis i.e. ket minus
        {
            blochSphere.localEulerAngles = new Vector3(0, 180, 180); //Transform to ket 1 (-ve z)
        }
        Debug.Log("After:" + blochSphere.localRotation.eulerAngles);

    }

    public void phaseBy90()
    {
        Debug.Log("Phase Gate Activated");
        Debug.Log("Before:" + blochSphere.localRotation.eulerAngles);
        Vector3 local = blochSphere.localRotation.eulerAngles;
        if (local == new Vector3(270, 0, 0)) //Positive x axis i.e. ket plus
        {
            blochSphere.localEulerAngles = new Vector3(90, 90, 0); //Transform to ket plus i (+ve y)
        }
        if (local == new Vector3(90, 90, 0)) //Positive y axis i.e ket plus i
        {
            blochSphere.localEulerAngles = new Vector3(90, 0, 0); //Transform to ket minus (-ve x)
        }
        if(local == new Vector3(90, 0, 0)) //Negative x axis i.e. ket minus
        {
            blochSphere.localEulerAngles = new Vector3(90, 270, 0); //Transform to ket minus i (-ve y)
        }
        if(local == new Vector3(90, 270, 0)) //Negative y axis i.e ket minus i
        {
            blochSphere.localEulerAngles = new Vector3(270, 0, 0); //Transform to ket plus (+ve x)
        }
        Debug.Log("After:" + blochSphere.localRotation.eulerAngles);

    }
}
