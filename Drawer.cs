using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDrawer : MonoBehaviour
{

    private bool mFirstTime = true;
    private Vector3 mPreviousPosition = new Vector3(0, 0, 0);
    private GameObject mObject;
    private Mesh mMesh;
    List<PolygonVertices> mTrail = new List<PolygonVertices>();
    public int NUMBER_OF_EDGES = 5;

    public MyDrawer(GameObject go)
    {
        mObject = go;
        mObject.AddComponent<MeshFilter>();
        mObject.AddComponent<MeshRenderer>();

        mMesh = mObject.GetComponent<MeshFilter>().mesh;
        mMesh.Clear();
    }


    public class PolygonVertices
    {

        public Vector3[] vertices;

        public PolygonVertices(Vector3[] vs)
        {
            vertices = vs;
        }

        public PolygonVertices(Vector3 point, Vector3 normal, int n, float radius, Vector3 movingDirection)
        {
            normal.Normalize();
            vertices = new Vector3[n];
            float angle = 360 / n;
            for (int i = 0; i < n; i++)
            {
                vertices[i] = point + radius * rotateVectorAroundAxis(normal, i * angle, movingDirection);
            }

        }

        Vector3 rotateVectorAroundAxis(Vector3 point, float angle, Vector3 axis)
        {

            Quaternion q = Quaternion.AngleAxis(angle, axis);
            return q * point;
        }

        public string print()
        {
            return "vertices: " + vertices[0] + "    " + vertices[1] + "    " + vertices[2];
        }


    }

    public void addTrail(Vector3 point, Vector3 normal, int n, float radius, Vector3 movingDirection)
    {
        PolygonVertices p = new PolygonVertices(point, normal, n, radius, movingDirection);
        mTrail.Add(p);
    }


    public void renderAll(int n)
    {
        if (mTrail.Count < 2)
        {
            return;
        }
        Vector3[] vs = new Vector3[n * mTrail.Count];

        for (int i = 0; i < mTrail.Count; i++)
        {
            for (int j = 0; j < n; j++)
            {
                vs[i * n + j] = mTrail[i].vertices[j];
            }
        }

        // 12 : for 2 triangles, 2 sides, 2*2*3=12
        

        int[] ts = new int[12 * n * (mTrail.Count - 1)];


        for (int i = 0; i < n * (mTrail.Count - 1); i++)
        {
            {

                ts[i * 12 + 0] = i;
                if ((i + 1) % n == 0)
                {
                    ts[i * 12 + 1] = i + n + 1 - n;
                    ts[i * 12 + 2] = i + 1 - n;
                    ts[i * 12 + 3] = i + n + 1 - n;
                }
                else
                {
                    ts[i * 12 + 1] = i + n + 1;
                    ts[i * 12 + 2] = i + 1;
                    ts[i * 12 + 3] = i + n + 1;
                }

                ts[i * 12 + 4] = i;
                ts[i * 12 + 5] = i + n;

                ts[i * 12 + 6] = ts[i * 12 + 2];
                ts[i * 12 + 7] = ts[i * 12 + 1];
            
                ts[i * 12 + 8] = ts[i * 12 + 0];
                ts[i * 12 + 9] = ts[i * 12 + 5];
                ts[i * 12 + 10] = ts[i * 12 + 4];
                ts[i * 12 + 11] = ts[i * 12 + 3];
            }
        }

        /*for (int i = 0; i < vs.Length; i++)
        {
            Debug.Log("vertices[" + i + "]:  " + vs[i]);
        }

        for (int i = 0; i < ts.Length; i++)
        {
            Debug.Log("triangles[" + i + "]:  " + ts[i]);
        }*/
        mMesh.vertices = vs;
        mMesh.triangles = ts;

    }


    public void addTestTrail()
    {
        mTrail.Clear();
        Vector3[] point1 = new Vector3[] {
            new Vector3(0.0f, 0.0f, 2.0f),
            new Vector3(0.0f, -1.7f, -1.0f),
            new Vector3(0.0f, 1.7f, -1.0f),
            new Vector3(0.0f, 0.0f, -2.0f)
                };
        PolygonVertices p1 = new PolygonVertices(point1);
        Vector3[] point2 = new Vector3[] {
            new Vector3(2.0f, 0.0f, 2f),
            new Vector3(2.0f, -1.7f, -1.0f),
            new Vector3(2.0f, 1.7f, -1.0f),
            new Vector3(2.0f, 0.0f, -2.0f)
        };
        PolygonVertices p2 = new PolygonVertices(point2);
        Vector3[] point3 = new Vector3[] {
            new Vector3(12.0f, 0.0f, 2f),
            new Vector3(12.0f, -1.7f, -1.0f),
            new Vector3(12.0f, 1.7f, -1.0f)
        };
        PolygonVertices p3 = new PolygonVertices(point3);
        mTrail.Add(p1);
        mTrail.Add(p2);
        //mTrail.Add(p3);
    }


    public void Draw(Vector3 position, Vector3 normal)
    {
        {
            if (mPreviousPosition != Vector3.zero)
            {
                addTrail(position, normal, NUMBER_OF_EDGES, 2, (position - mPreviousPosition));
                renderAll(NUMBER_OF_EDGES);
                mFirstTime = false;

            }
        }
        mPreviousPosition = position;
    }


}
