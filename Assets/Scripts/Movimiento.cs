using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movimiento : MonoBehaviour
{
    [SerializeField] private float velocidadMovimiento;
    [SerializeField] private Transform[] puntosMovimiento;
    [SerializeField] private float distanciaMinima;

    public int siguientePaso = 0;
    private GameObject centroGO;

    public NavMeshAgent agent;

    public float range;

    public float tiempoEspera;
    public bool esperando = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        centroGO = GameObject.Find("Centro");
    }

    void Update()
    {
        //Cambia de punto al estar en la distancia minima del anterior punto
        if (!esperando && agent.enabled && agent.remainingDistance <= agent.stoppingDistance)
        {
            Vector3 point;
            if (RandomPoint(centroGO.transform.position, range, out point)) //pass in our centre point and radius of area
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //so you can see with gizmos
                esperando = true;
                StartCoroutine("Esperar", point);
            }
        }
    }
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) //documentation: https://docs.unity3d.com/ScriptReference/AI.NavMesh.SamplePosition.html
        {
            //the 1.0f is the max distance from the random point to a point on the navmesh, might want to increase if range is big
            //or add a for loop like in the documentation
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    IEnumerator ActivarAgente(int tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        agent.enabled = true;
    }

    IEnumerator Esperar(Vector3 point)
    {
        yield return new WaitForSeconds(tiempoEspera);
        if (agent.enabled)
        {
            agent.SetDestination(point);
        }
        
        esperando = false;
    }
}
