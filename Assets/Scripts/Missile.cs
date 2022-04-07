using UnityEngine;

public class Missile : MonoBehaviour
{
   
    [SerializeField] float damage=25f,speed=30f,rotateSpeed = 95;
    [SerializeField] Rigidbody rb;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float maxDistancePredict = 100, minDistancePredict = 5, maxTimePrediction = 5;
    private Vector3 standardPrediction, deviatedPrediction;

    [SerializeField] private float deviationAmount = 50, deviationSpeed = 2;

    public HelicopterController target;
    void OnCollisionEnter(Collision c)
    {
        Debug.Log("Missile collided with "+c.gameObject.name);
        Instantiate(Resources.Load("MissileHitParticles", typeof(GameObject)),transform.position,transform.rotation);
        Destroy(gameObject);
    }





    private void FixedUpdate() {
        rb.velocity = transform.forward * speed;

        var leadTimePercentage = Mathf.InverseLerp(minDistancePredict, maxDistancePredict, Vector3.Distance(transform.position, target.transform.position));

        PredictMovement(leadTimePercentage);

        AddDeviation(leadTimePercentage);

        RotateRocket();
    }

    private void PredictMovement(float leadTimePercentage) {
        var predictionTime = Mathf.Lerp(0, maxTimePrediction, leadTimePercentage);

        standardPrediction = target.rb.position + target.rb.velocity * predictionTime;
    }

    private void AddDeviation(float leadTimePercentage) {
        var deviation = new Vector3(Mathf.Cos(Time.time * deviationSpeed), 0, 0);
        
        var predictionOffset = transform.TransformDirection(deviation) * deviationAmount * leadTimePercentage;

        deviatedPrediction = standardPrediction + predictionOffset;
    }

    private void RotateRocket() {
        var heading = deviatedPrediction - transform.position;

        var rotation = Quaternion.LookRotation(heading);
        rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed * Time.deltaTime));
    }
}
