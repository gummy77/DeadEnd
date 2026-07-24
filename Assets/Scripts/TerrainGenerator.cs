using System;
using Unity.Mathematics;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [Header("Referenes")]
    [SerializeField] private GameObject groundGameObject;

    [Header("Settings")]
    [SerializeField] private float noiseScale = 1f;
    [SerializeField] private float noiseFrequency = 1f;
    
    private MeshRenderer _meshRenderer;
    private MeshFilter _meshFilter;
    private MeshCollider _meshCollider;

    private Mesh _mesh;

    public static Action GenerationFinished;
    
    public void Start()
    {
        _meshRenderer = groundGameObject.GetComponent<MeshRenderer>();
        _meshCollider = groundGameObject.GetComponent<MeshCollider>();
        _meshFilter = groundGameObject.GetComponent<MeshFilter>();
        
        _mesh = Instantiate(_meshFilter.sharedMesh);
        Vector3[] newVertices = new Vector3[_mesh.vertices.Length];

        for (var index = 0; index < _mesh.vertices.Length; index++)
        {
            newVertices[index] = _mesh.vertices[index] + new Vector3(0, noise.snoise(_mesh.vertices[index] * noiseFrequency) * noiseScale, 0);
        }

        _mesh.SetVertices(newVertices);
        _mesh.MarkModified();
        _mesh.RecalculateNormals();
        
        _meshFilter.sharedMesh = _mesh;
        _meshCollider.sharedMesh = _mesh;
        
        GenerationFinished?.Invoke();
    }
}
