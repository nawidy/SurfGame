// References:
//              http://docs.unity3d.com/ScriptReference/Mesh.html
//              http://docs.unity3d.com/ScriptReference/MeshFilter.html


using UnityEngine;
using System.Collections;

public class TerrainGenerator : MonoBehaviour 
{
    public Shader terrainShader;

    public float terrainPieceWidth;
    public float terrainPieceHeight;
    public int   maxNumTerrainPieces;
    public int   numSegmentsPerTerrainPiece; 

    public float minMultiple;
    public float maxMultiple;

    public PhysicsMaterial2D physicsMat;
    public Texture terrainTexture;
    public bool    hasCollider;
    public bool    topIsUnique;
    public bool    shouldNormalizeUVs;
	public bool    shouldScrollTextures;

	public  Vector2 textureScrollSpeed = Vector2.zero;

    private int        terrainPiece   = 0;
    private GameObject playerObj;
    private float      prevYEndPoint;
    private Vector3    nextSpawnLocation = Vector3.zero;


    public void Start () 
    {
        playerObj = GameObject.FindWithTag("Player");

        float height = (topIsUnique ? 1.0f : -1.0f) * terrainPieceHeight;
        prevYEndPoint = height * Random.Range(minMultiple, maxMultiple);

        for (int i = 0; i < maxNumTerrainPieces; ++i)
        {
            GenerateNextTerrainPiece();
        }
  }

    public void Update () 
    {
        if (ItIsTimeToGenerateNextTerrainPiece())
        {
            GenerateNextTerrainPiece();
            RemovePreviousTerrainPiece();
        }
    }



    private bool ItIsTimeToGenerateNextTerrainPiece()
    {
		return playerObj.transform.localPosition.x > 
			   transform.GetChild(transform.childCount/2).transform.localPosition.x;
    }

    private void GenerateNextTerrainPiece()
    {
		GameObject newTerrain = new GameObject("GeneratedTerrain_" + terrainPiece);
        newTerrain.tag = "Terrain";
        MeshRenderer renderer       = newTerrain.AddComponent<MeshRenderer>();
        MeshFilter meshFilter       = newTerrain.AddComponent<MeshFilter>();
        PolygonCollider2D collider  = hasCollider          ? newTerrain.AddComponent<PolygonCollider2D>() : null;
		TextureScroll textureScroll = shouldScrollTextures ? newTerrain.AddComponent<TextureScroll>()     : null;
        newTerrain.AddComponent<BuoyancyEffect>();

        newTerrain.transform.parent = this.transform;

        // Generates mesh, meshFilter.mesh gets/creates the mesh to be used
        GenerateTerrainGeometry(newTerrain, renderer, meshFilter.mesh, collider);

		if (shouldScrollTextures)
		{
            textureScroll.scrollSpeed = textureScrollSpeed;
		}

		newTerrain.transform.localPosition = nextSpawnLocation;
        nextSpawnLocation.x += terrainPieceWidth;

        ++terrainPiece;
    }




    private void GenerateTerrainGeometry(GameObject terrainObj, 
                                         MeshRenderer terrainRenderer, 
                                         Mesh mesh, 
                                         PolygonCollider2D collider)
    {
        mesh.Clear();


        // Set up the vertices for the mesh
        Vector3[] vertices = new Vector3[4 + (numSegmentsPerTerrainPiece * 2)];
        vertices[0] = Vector3.zero;
        vertices[1] = new Vector3(0, prevYEndPoint, 0);

        float heightDir = (topIsUnique ? 1.0f : -1.0f) * terrainPieceHeight;
        int numVerts = vertices.Length;
        for (int i = 0; i < numSegmentsPerTerrainPiece; ++i)
        {
            float percentage = (i + 1.0f) / (float)(1.0f + numSegmentsPerTerrainPiece);
            float xPos = terrainPieceWidth * percentage;
            vertices[2 + i] = new Vector3(xPos,
                                          heightDir * Random.Range(minMultiple, maxMultiple),
                                          0);
            vertices[numVerts - (i + 1)] = new Vector3(xPos, 0, 0);
        }
        prevYEndPoint = heightDir * Random.Range(minMultiple, maxMultiple);
        vertices[2 + numSegmentsPerTerrainPiece] = new Vector3(terrainPieceWidth, prevYEndPoint, 0);
        vertices[3 + numSegmentsPerTerrainPiece] = new Vector3(terrainPieceWidth, 0, 0);

        mesh.vertices = vertices;

/*
        mesh.vertices = new Vector3[] 
        {
            new Vector3(0, 0, 0),
            
            new Vector3(0,                       terrainPieceHeight * 0.8f, 0),
            new Vector3(terrainPieceWidth * .3f, terrainPieceHeight * 0.9f, 0),
            new Vector3(terrainPieceWidth * .8f, terrainPieceHeight * 1.0f, 0),
            new Vector3(terrainPieceWidth,       terrainPieceHeight * 1.3f, 0),
            
            new Vector3(terrainPieceWidth,       0, 0),
            new Vector3(terrainPieceWidth * .8f, 0, 0),
            new Vector3(terrainPieceWidth * .3f, 0, 0)
        };
//*/

/*
        mesh.triangles = new int[]
        {
            0, 1, 2,
            0, 2, 7,

            7, 2, 3,
            7, 3, 6,

            6, 3, 4,
            6, 4, 5
        };
//*/




        // Verts need to be in CW (clockwise) order
        int numTriangles = vertices.Length - 2; 
        int[] triangles  = new int[ 3 * numTriangles ];
        int firstIndex = 0;
        int lastIndex  = vertices.Length;
        if (topIsUnique)
        {
            for (int i = 0, j = 1; i < triangles.Length; i += 6, ++j)
            {
                triangles[i]     = firstIndex;
                triangles[i + 1] = j;
                triangles[i + 2] = j + 1;

                --lastIndex;
                triangles[i + 3] = firstIndex;
                triangles[i + 4] = j + 1;
                triangles[i + 5] = lastIndex;
                firstIndex = lastIndex;
            }
        }
        else
        {
            for (int i = 0, j = 1; i < triangles.Length; i += 6, ++j)
            {
                triangles[i]     = firstIndex;
                triangles[i + 1] = j + 1;
                triangles[i + 2] = j;

                --lastIndex;
                triangles[i + 3] = firstIndex;
                triangles[i + 4] = lastIndex;
                triangles[i + 5] = j + 1;
                firstIndex = lastIndex;
            }   
        }

        mesh.triangles = triangles;

        // Recalculate normals for lighting/shaders and bounds for culling
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();


		// Convert 3D Vec array to 2D Vec array
        int vertVecLength = mesh.vertices.Length;
        Vector2[] verts2D = new Vector2[vertVecLength];
        for (int i = 0; i < vertVecLength; ++i)
        {
            verts2D[i] = mesh.vertices[i].toVec2();
        }


        // Set vertices for polygon collider and set physics material
        if (collider != null)
        {
            collider.points = verts2D;
            collider.sharedMaterial = physicsMat;
            collider.isTrigger = true;
        }

        // UVs [0,1] range, automatically loops around (and tiles) this texture's UVs
        if (shouldNormalizeUVs)
        {
            float maxValueY = heightDir * maxMultiple;
            for(int i = 0; i < verts2D.Length; ++i)
            {
                verts2D[i] = new Vector2(verts2D[i].x / terrainPieceWidth,
                                         verts2D[i].y / maxValueY);
            }
        }
        mesh.uv = verts2D;

        // Apply texture and set rendering material
        if (terrainRenderer.sharedMaterial == null)
        {
            terrainRenderer.sharedMaterial = new Material(terrainShader);

        }
        terrainRenderer.sharedMaterial.mainTexture = terrainTexture;
    }



    private void RemovePreviousTerrainPiece()
    {
        GameObject.DestroyImmediate(transform.GetChild(0).gameObject);
    }
}
