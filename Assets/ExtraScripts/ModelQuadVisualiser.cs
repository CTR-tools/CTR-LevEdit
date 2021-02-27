using UnityEngine;
using Random = System.Random;

public class ModelQuadVisualiser : MonoBehaviour
{
    public Mesh mesh;
    private bool visible = false;
    public MeshCollider Collider;
    public MeshRenderer Renderer;
    public Vector3 Center;
    public Vector3 Normal;

    private int frame = 0;
    private void Start()
    {
        frame = (int)(UnityEngine.Random.value*32.0f);
    }
    private void OnDrawGizmosSelected()
    {
        /*if (Selection.Contains(transform.gameObject))*/
        {
            //Gizmos.DrawWireCube(boxCollider.center,boxCollider.size);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = visible ?  Color.white : Color.red;
        Gizmos.DrawWireMesh(mesh);
    }

    private void Update()
    {
        if ((++frame) % 8 > 0) return;
        if (CountQuads.MainCamera == null) return;
        if (GeometryUtility.TestPlanesAABB(CountQuads.Planes, Collider.bounds))
        {
            Vector3 offset = (Center - CountQuads.MainCamera.transform.position).normalized;
            
            Debug.DrawRay(Center,Normal, new Color(1f,0f,1f,0.2f));
            if (
                Vector3.Dot(Normal, CountQuads.MainCamera.transform.position - Center) <= 0 ||
                Physics.Linecast(CountQuads.MainCamera.transform.position, Center + Normal * 0.01f ))
            {
                if (visible)
                {
                    CountQuads.QuadCount--;
                }
                visible = false;
            }
            else
            {
                if (!visible)
                {
                    CountQuads.QuadCount++;
                }
                visible = true;
                if (Vector3.Distance(Center, CountQuads.MainCamera.transform.position) < 32f)
                {
                    CountQuads.CloseQuads++;
                }
            }
        }
        else
        {
            if (visible)
            {
                CountQuads.QuadCount--;
            }
            visible = false;
        }
    }

}
