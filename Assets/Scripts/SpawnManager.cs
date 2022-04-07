using UnityEngine;
using System;
using Wrld;
using Wrld.Space;
using Wrld.Common.Maths;
using Wrld.Transport;
public class SpawnManager : MonoBehaviour
{
    private readonly LatLongAltitude m_inputCoords = LatLongAltitude.FromDegrees(37.784468, -122.401268, 10.0);
    [SerializeField]
    private GameObject buildingTopEnemy = null,roadEnemy;

    [SerializeField]
    BuildingEnemies[] buildingEnemies;
    [SerializeField]
    RoadEnemies[] roadEnemies;
    bool isHeadingA;
    
    
    [Serializable]
    class BuildingEnemies
    { 
        public string buildingDegreeX,buildingDegreeY; 

    }
    [Serializable]
    class RoadEnemies
    { 
        public string roadDegreeX,roadDegreeY; 

    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemiesOnBuilding();
        
    }

    public void SpawnEnemiesOnBuilding()
    {
        // Api.Instance.CameraApi.MoveTo(cameraLocation, distanceFromInterest: 400, headingDegrees: 0, tiltDegrees: 45);
        for(int i=0;i<buildingEnemies.Length;i++)
        {
            LatLong temp;
            temp=LatLong.FromDegrees(float.Parse(buildingEnemies[i].buildingDegreeX),float.Parse(buildingEnemies[i].buildingDegreeY));
            GenerateEnemyOnBuilding(temp);
            Debug.Log("Enemy Spawned");
        }
    }
    void GenerateEnemyOnBuilding(LatLong latLong)
    {
        var ray = Api.Instance.SpacesApi.LatLongToVerticallyDownRay(latLong);
        LatLongAltitude buildingIntersectionPoint;
        var didIntersectBuilding = Api.Instance.BuildingsApi.TryFindIntersectionWithBuilding(ray, out buildingIntersectionPoint);
        if (didIntersectBuilding)
        {
            var boxAnchor = Instantiate(buildingTopEnemy) as GameObject;
            boxAnchor.GetComponent<GeographicTransform>().SetPosition(buildingIntersectionPoint.GetLatLong());

            var box = boxAnchor.transform.GetChild(0);
            box.localPosition = Vector3.up * (float)buildingIntersectionPoint.GetAltitude();
            Debug.Log("Cannon Spawned"+boxAnchor.gameObject.name);
            // Debug.Break();
            // Destroy(boxAnchor, 2.0f);
        }
    }
    public void SpawnEnemiesOnRoad()
    {
        for(int i=0;i<roadEnemies.Length;i++)
        {
            LatLongAltitude temp;
            temp=LatLongAltitude.FromDegrees(float.Parse(roadEnemies[i].roadDegreeX),float.Parse(roadEnemies[i].roadDegreeX),1.0f);
            GenerateEnemiesOnRoad(temp);
            Debug.Log("Enemy Spawned");
        }
    }
    private void GenerateEnemiesOnRoad(LatLongAltitude latLongAltitude)
    {
        var inputPosition = Api.Instance.SpacesApi.GeographicToWorldPoint(latLongAltitude);
        GameObject temp = Instantiate(roadEnemy) as GameObject;
        temp.transform.localPosition = inputPosition;
        Debug.Break();
    }
}
