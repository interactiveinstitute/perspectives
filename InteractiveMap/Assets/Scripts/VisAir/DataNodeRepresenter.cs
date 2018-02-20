using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DataNode))]
public class DataNodeRepresenter : MonoBehaviour
{
    private DataNode dataNode;

    // Trail
    [Header("Trail")]
    [SerializeField]
    private GameObject trailPrefab;

    private GameObject trail;

    [SerializeField]
    private bool showTrail;

    // Trail Color
    [SerializeField]
    private bool showTrailColor;

    [SerializeField]
    private VisData trailColorData;

    [SerializeField]
    private Gradient trailSpectrum;

    // Trail height
    [SerializeField]
    private bool showTrailHeight;

    [SerializeField]
    private VisData trailHeightData;

    private float trailHeight = 0.5f;

    // Projection
    [Header("Projection")]
    [SerializeField]
    private GameObject projectionPrefab;

    [SerializeField]
    private bool showProjection;

    [SerializeField]
    private VisData projectionColorData;

    [SerializeField]
    private Gradient projectionSpectrum;

    private double lastTimeStamp = -1;
    
    private void Start()
    {
        dataNode = GetComponent<DataNode>();
        trail = transform.Find("trailManager").gameObject;
        lastTimeStamp = dataNode.LastData.Timestamp;
    }

	/// <summary>
	/// Update this instance.
	/// </summary>
    private void Update()
    {
        if (dataNode.LastData.Timestamp != lastTimeStamp) {
            trailColorData.Update(dataNode);
            trailHeightData.Update(dataNode);
            projectionColorData.Update(dataNode);
            OnDataUpdated();
            lastTimeStamp = dataNode.LastData.Timestamp;
        }
        var newPosition = new Vector3(trail.transform.position.x, trailHeight * 10f, trail.transform.position.z);
        trail.transform.position = Vector3.MoveTowards(trail.transform.position, newPosition, 4f * Time.deltaTime);
    }

	/// <summary>
	/// Called when data is updated
	/// </summary>
    private void OnDataUpdated()
    {
		if (showTrail) {
			// Activates and sets trail color and height data

			trail.SetActive(true);
			var activeTrail = trail.transform.GetChild(0).GetComponent<TrailRenderer>();
			Color oldColor = activeTrail.startColor;
			Color newColor = Color.gray;

			if (showTrailColor) {
				newColor = CalculateGradient(trailColorData, trailSpectrum);
			}

			if (showTrailHeight) {
				trailHeight = (trailHeightData.Value - trailHeightData.MinValue) / (trailHeightData.MaxValue - trailHeightData.MinValue);
			}
			else {
				trailHeight = 0.5f;
			}

			// Create new trail
			var newTrail = Instantiate(trailPrefab, trail.transform.position, trail.transform.rotation, trail.transform);
			var newTrailRenderer = newTrail.GetComponent<TrailRenderer>();
			newTrailRenderer.endColor = oldColor; //set previous startcolor to new endcolor
			newTrailRenderer.startColor = newColor;

			// Stop active trail from following bike and delete after activeTrail.time secs
			activeTrail.transform.parent = null;
			Destroy(activeTrail.gameObject, activeTrail.time);
		} else {
			trail.SetActive(false);
		}

        if (showProjection) {
			// Place new projection and plan its destruction

			var newPosition = transform.position + new Vector3(0,25,0);
			var newBlobProjector = Instantiate(projectionPrefab, newPosition, Quaternion.LookRotation(new Vector3(0, -1, 0)));

			var projector = newBlobProjector.GetComponent<Projector>();
			var newMaterial = new Material(projector.material);

			newMaterial.color = CalculateGradient(projectionColorData,projectionSpectrum);
			projector.material = newMaterial;

			Destroy(newBlobProjector, 5f);
	    }
    }

	private Color CalculateGradient(float value, float minValue, float maxValue, Gradient spectrum)
    {
        Color retColor;
        if (value < minValue) {
            retColor = spectrum.Evaluate(0);
        } else if (value > maxValue) {
            retColor = spectrum.Evaluate(1);
        } else {
            retColor = spectrum.Evaluate((value - minValue) / (maxValue - minValue)); // maths value to be between 0 and 1
        }
        return retColor;
    }

	private Color CalculateGradient(VisData data, Gradient spectrum)
	{
		return CalculateGradient(data.Value, data.MinValue, data.MaxValue, spectrum);
	}

    [System.Serializable]
    private class VisData
    {
        [SerializeField]
        [Tooltip("Name of which data column to use in the SensorDevice.Columns list")]
        private string columnName;

        [SerializeField]
        private float minValue;

        [SerializeField]
        private float maxValue;

		public float Value { get; private set; }
        public float MinValue {
            get { return minValue; }
            set { minValue = value; }
        }
        public float MaxValue {
            get { return maxValue; }
            set { maxValue = value; }
        }
		public float LastValue { get; private set; }

        public bool Update(DataNode node)
        {
            int column = node.Columns.IndexOf(columnName);
            if (column == -1) {
                return false;
            }
            LastValue = Value;
            Value = (float)node.LastData.Values[column];
            return true;
        }
    }
}
